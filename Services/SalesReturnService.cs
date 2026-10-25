using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MajesticDesktop.Services
{
    public class SalesReturnService
    {
        public SalesReturnService()
        {
            EnsureTablesExist();
        }

        private void EnsureTablesExist()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                const string query = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SalesReturns' AND xtype='U')
                    BEGIN
                        CREATE TABLE SalesReturns (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            OriginalSaleId INT NOT NULL DEFAULT 0,
                            ReturnDate DATETIME NOT NULL,
                            TotalRefundAmount DECIMAL(18,2) NOT NULL,
                            LocationId INT NOT NULL,
                            Notes NVARCHAR(MAX) NULL
                        );
                    END

                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReturns') AND name = 'CustomerId')
                    BEGIN
                        ALTER TABLE SalesReturns ADD CustomerId INT NULL;
                    END

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SalesReturnDetails' AND xtype='U')
                    BEGIN
                        CREATE TABLE SalesReturnDetails (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            ReturnId INT NOT NULL,
                            ProductId INT NOT NULL,
                            ReturnedQuantity INT NOT NULL,
                            UnitPrice DECIMAL(18,2) NOT NULL,
                            TotalPrice DECIMAL(18,2) NOT NULL
                        );
                    END";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ProcessSalesReturn(SaleReturn saleReturn, bool isCashRefund)
        {
            if (saleReturn == null || saleReturn.Details.Count == 0)
                throw new ArgumentException("بيانات المرتجع فارغة.");

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string insertReturnQuery = @"
                            INSERT INTO SalesReturns (OriginalSaleId, CustomerId, ReturnDate, TotalRefundAmount, LocationId, Notes)
                            VALUES (0, @CustomerId, @ReturnDate, @TotalRefundAmount, @LocationId, @Notes);
                            SELECT SCOPE_IDENTITY();";

                        int returnId;
                        using (SqlCommand cmdReturn = new SqlCommand(insertReturnQuery, connection, transaction))
                        {
                            cmdReturn.Parameters.Add("@CustomerId", SqlDbType.Int).Value = saleReturn.CustomerId > 0 ? (object)saleReturn.CustomerId : DBNull.Value;
                            cmdReturn.Parameters.Add("@ReturnDate", SqlDbType.DateTime).Value = saleReturn.ReturnDate;
                            cmdReturn.Parameters.Add("@TotalRefundAmount", SqlDbType.Decimal).Value = saleReturn.TotalRefundAmount;
                            cmdReturn.Parameters.Add("@LocationId", SqlDbType.Int).Value = saleReturn.LocationId;
                            cmdReturn.Parameters.Add("@Notes", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(saleReturn.Notes) ? (object)DBNull.Value : saleReturn.Notes;
                            returnId = Convert.ToInt32(cmdReturn.ExecuteScalar());
                        }

                        const string insertDetailQuery = @"
                            INSERT INTO SalesReturnDetails (ReturnId, ProductId, ReturnedQuantity, UnitPrice, TotalPrice)
                            VALUES (@ReturnId, @ProductId, @ReturnedQuantity, @UnitPrice, @TotalPrice);";

                        const string updateStockQuery = @"
                            UPDATE Stock 
                            SET Quantity = ISNULL(Quantity, 0) + @ReturnedQuantity 
                            WHERE ProductId = @ProductId AND LocationId = @LocationId;";

                        foreach (var detail in saleReturn.Details)
                        {
                            using (SqlCommand cmdDetail = new SqlCommand(insertDetailQuery, connection, transaction))
                            {
                                cmdDetail.Parameters.Add("@ReturnId", SqlDbType.Int).Value = returnId;
                                cmdDetail.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdDetail.Parameters.Add("@ReturnedQuantity", SqlDbType.Int).Value = detail.ReturnedQuantity;
                                cmdDetail.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = detail.UnitPrice;
                                cmdDetail.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = detail.TotalPrice;
                                cmdDetail.ExecuteNonQuery();
                            }

                            using (SqlCommand cmdStock = new SqlCommand(updateStockQuery, connection, transaction))
                            {
                                cmdStock.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdStock.Parameters.Add("@LocationId", SqlDbType.Int).Value = saleReturn.LocationId;
                                cmdStock.Parameters.Add("@ReturnedQuantity", SqlDbType.Int).Value = detail.ReturnedQuantity;
                                cmdStock.ExecuteNonQuery();
                            }
                        }


                        if (isCashRefund)
                        {
                            const string cashQuery = @"
                                INSERT INTO CashTransactions (LocationId, TransactionDate, TransactionType, Amount, Description, ReferenceNumber)
                                VALUES (@CashLocId, GETDATE(), N'صادر', @CashAmount, @CashDesc, @CashRefNo);";

                            using (SqlCommand cmdCash = new SqlCommand(cashQuery, connection, transaction))
                            {
                                cmdCash.Parameters.Add("@CashLocId", SqlDbType.Int).Value = saleReturn.LocationId;
                                cmdCash.Parameters.Add("@CashAmount", SqlDbType.Decimal).Value = saleReturn.TotalRefundAmount;
                                cmdCash.Parameters.Add("@CashDesc", SqlDbType.NVarChar, 255).Value = "مرتجع مبيعات مخزن رئيسي نقدي فوري رقم: " + returnId;
                                cmdCash.Parameters.Add("@CashRefNo", SqlDbType.NVarChar, 50).Value = returnId.ToString();
                                cmdCash.ExecuteNonQuery();
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