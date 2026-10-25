using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MajesticDesktop.Services
{
    public class IssueStockService
    {
        public IssueStockService()
        {
            EnsureTablesExist();
        }

        private void EnsureTablesExist()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                const string query = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StockTransfers' AND xtype='U')
                    BEGIN
                        CREATE TABLE StockTransfers (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            FromLocationId INT NOT NULL,
                            ToLocationId INT NOT NULL,
                            TransferDate DATETIME NOT NULL
                        );
                    END

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StockTransferDetails' AND xtype='U')
                    BEGIN
                        CREATE TABLE StockTransferDetails (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            TransferId INT NOT NULL,
                            ProductId INT NOT NULL,
                            Quantity INT NOT NULL
                        );
                    END";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // أداء ممتاز O(1)
        public int GetAvailableQuantity(int locationId, int productId)
        {
            const string query = "SELECT ISNULL(SUM(Quantity), 0) FROM Stock WHERE LocationId = @LocationId AND ProductId = @ProductId;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;
                command.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        // استخدام الـ Transaction هنا بيضمن أداء عالي جداً وحماية للبيانات
        public void SaveIssueStock(IssueStock issueStock)
        {
            if (issueStock == null || issueStock.Details.Count == 0)
                throw new ArgumentException("بيانات الصرف غير مكتملة.");

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. تسجيل رأس الإذن (نقل من وإلى)
                        const string insertTransferQuery = @"
                            INSERT INTO StockTransfers (FromLocationId, ToLocationId, TransferDate) 
                            VALUES (@FromLocationId, @ToLocationId, @TransferDate);
                            SELECT SCOPE_IDENTITY();";

                        int transferId;
                        using (SqlCommand cmdTransfer = new SqlCommand(insertTransferQuery, connection, transaction))
                        {
                            cmdTransfer.Parameters.Add("@FromLocationId", SqlDbType.Int).Value = issueStock.FromLocationId;
                            cmdTransfer.Parameters.Add("@ToLocationId", SqlDbType.Int).Value = issueStock.ToLocationId;
                            cmdTransfer.Parameters.Add("@TransferDate", SqlDbType.DateTime).Value = issueStock.IssueDate;
                            transferId = Convert.ToInt32(cmdTransfer.ExecuteScalar());
                        }

                        const string insertDetailQuery = @"
                            INSERT INTO StockTransferDetails (TransferId, ProductId, Quantity) 
                            VALUES (@TransferId, @ProductId, @Quantity);";

                        const string deductStockQuery = @"
                            UPDATE Stock 
                            SET Quantity = Quantity - @Quantity 
                            WHERE ProductId = @ProductId AND LocationId = @FromLocationId;";

                        const string addStockQuery = @"
                            IF EXISTS (SELECT 1 FROM Stock WHERE ProductId = @ProductId AND LocationId = @ToLocationId)
                            BEGIN
                                UPDATE Stock 
                                SET Quantity = Quantity + @Quantity 
                                WHERE ProductId = @ProductId AND LocationId = @ToLocationId;
                            END
                            ELSE
                            BEGIN
                                INSERT INTO Stock (ProductId, LocationId, Quantity) 
                                VALUES (@ProductId, @ToLocationId, @Quantity);
                            END";

                        foreach (IssueStockDetail detail in issueStock.Details)
                        {
                            // 2. تسجيل تفاصيل الإذن
                            using (SqlCommand cmdDetail = new SqlCommand(insertDetailQuery, connection, transaction))
                            {
                                cmdDetail.Parameters.Add("@TransferId", SqlDbType.Int).Value = transferId;
                                cmdDetail.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdDetail.Parameters.Add("@Quantity", SqlDbType.Int).Value = detail.Quantity;
                                cmdDetail.ExecuteNonQuery();
                            }

                            // 3. خصم من المصدر
                            using (SqlCommand cmdDeduct = new SqlCommand(deductStockQuery, connection, transaction))
                            {
                                cmdDeduct.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdDeduct.Parameters.Add("@FromLocationId", SqlDbType.Int).Value = issueStock.FromLocationId;
                                cmdDeduct.Parameters.Add("@Quantity", SqlDbType.Int).Value = detail.Quantity;
                                cmdDeduct.ExecuteNonQuery();
                            }

                            // 4. إضافة للفرع المستلم
                            using (SqlCommand cmdAdd = new SqlCommand(addStockQuery, connection, transaction))
                            {
                                cmdAdd.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdAdd.Parameters.Add("@ToLocationId", SqlDbType.Int).Value = issueStock.ToLocationId;
                                cmdAdd.Parameters.Add("@Quantity", SqlDbType.Int).Value = detail.Quantity;
                                cmdAdd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}