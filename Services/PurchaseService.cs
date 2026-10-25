using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MajesticDesktop.Services
{
    public class PurchaseService
    {
        public string GenerateInvoiceNumber()
        {
            string invoiceNumber = "INV-" + DateTime.Now.ToString("yyyyMMdd") + "-";
            const string query = "SELECT ISNULL(MAX(Id), 0) + 1 FROM Purchases;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                int nextId = Convert.ToInt32(command.ExecuteScalar());
                invoiceNumber += nextId.ToString("D4");
            }

            return invoiceNumber;
        }

        public void SavePurchaseInvoice(Purchase purchase)
        {
            if (purchase == null || purchase.Details.Count == 0)
                throw new ArgumentException("بيانات الفاتورة غير مكتملة.");

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string insertPurchaseQuery = @"
                            INSERT INTO Purchases 
                            (InvoiceNumber, SupplierId, LocationId, InvoiceDate, TotalAmount, PaidAmount, RemainingAmount)
                            VALUES 
                            (@InvoiceNumber, @SupplierId, @LocationId, @InvoiceDate, @TotalAmount, @PaidAmount, @RemainingAmount);
                            SELECT SCOPE_IDENTITY();";

                        int purchaseId;

                        using (SqlCommand cmdPurchase = new SqlCommand(insertPurchaseQuery, connection, transaction))
                        {
                            cmdPurchase.Parameters.Add("@InvoiceNumber", SqlDbType.NVarChar, 50).Value = purchase.InvoiceNumber;
                            cmdPurchase.Parameters.Add("@SupplierId", SqlDbType.Int).Value = purchase.SupplierId > 0 ? (object)purchase.SupplierId : DBNull.Value;
                            cmdPurchase.Parameters.Add("@LocationId", SqlDbType.Int).Value = purchase.LocationId;
                            cmdPurchase.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = purchase.InvoiceDate;
                            cmdPurchase.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = purchase.TotalAmount;
                            cmdPurchase.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = purchase.PaidAmount;
                            cmdPurchase.Parameters.Add("@RemainingAmount", SqlDbType.Decimal).Value = purchase.RemainingAmount;

                            purchaseId = Convert.ToInt32(cmdPurchase.ExecuteScalar());
                        }

                        const string stockInfoQuery = @"
                            SELECT ISNULL(s.Quantity, 0) AS OldQty, ISNULL(p.PurchasePrice, 0) AS OldPrice
                            FROM Products p
                            LEFT JOIN Stock s ON p.Id = s.ProductId AND s.LocationId = @LocationId
                            WHERE p.Id = @ProductId;";

                        const string insertDetailQuery = @"
                            INSERT INTO PurchaseDetails 
                            (PurchaseId, ProductId, Quantity, UnitPrice, TotalPrice)
                            VALUES 
                            (@PurchaseId, @ProductId, @Quantity, @UnitPrice, @TotalPrice);";

                        const string updateStockQuery = @"
                            IF EXISTS (SELECT 1 FROM Stock WHERE ProductId = @ProductId AND LocationId = @LocationId)
                            BEGIN
                                UPDATE Stock SET Quantity = Quantity + @Quantity WHERE ProductId = @ProductId AND LocationId = @LocationId;
                            END
                            ELSE
                            BEGIN
                                INSERT INTO Stock (ProductId, LocationId, Quantity) VALUES (@ProductId, @LocationId, @Quantity);
                            END";

                        const string updateProductPriceQuery = @"
                            UPDATE Products SET PurchasePrice = @NewAveragePrice WHERE Id = @ProductId;";

                        foreach (PurchaseDetail detail in purchase.Details)
                        {
                            using (SqlCommand cmdDetail = new SqlCommand(insertDetailQuery, connection, transaction))
                            {
                                cmdDetail.Parameters.Add("@PurchaseId", SqlDbType.Int).Value = purchaseId;
                                cmdDetail.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdDetail.Parameters.Add("@Quantity", SqlDbType.Int).Value = detail.Quantity;
                                cmdDetail.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = detail.UnitPrice;
                                cmdDetail.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = detail.TotalPrice;
                                cmdDetail.ExecuteNonQuery();
                            }

                            decimal oldQty = 0;
                            decimal oldPrice = 0;

                            using (SqlCommand cmdStockInfo = new SqlCommand(stockInfoQuery, connection, transaction))
                            {
                                cmdStockInfo.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdStockInfo.Parameters.Add("@LocationId", SqlDbType.Int).Value = purchase.LocationId;
                                using (SqlDataReader reader = cmdStockInfo.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        oldQty = Convert.ToDecimal(reader["OldQty"]);
                                        oldPrice = Convert.ToDecimal(reader["OldPrice"]);
                                    }
                                }
                            }

                            decimal newAveragePrice = detail.UnitPrice;
                            if (oldQty > 0)
                            {
                                decimal totalOldValue = oldQty * oldPrice;
                                decimal totalNewValue = detail.Quantity * detail.UnitPrice;
                                decimal totalQty = oldQty + detail.Quantity;
                                newAveragePrice = (totalOldValue + totalNewValue) / totalQty;
                            }

                            using (SqlCommand cmdStock = new SqlCommand(updateStockQuery, connection, transaction))
                            {
                                cmdStock.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdStock.Parameters.Add("@LocationId", SqlDbType.Int).Value = purchase.LocationId;
                                cmdStock.Parameters.Add("@Quantity", SqlDbType.Int).Value = detail.Quantity;
                                cmdStock.ExecuteNonQuery();
                            }

                            using (SqlCommand cmdUpdatePrice = new SqlCommand(updateProductPriceQuery, connection, transaction))
                            {
                                cmdUpdatePrice.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdUpdatePrice.Parameters.Add("@NewAveragePrice", SqlDbType.Decimal).Value = newAveragePrice;
                                cmdUpdatePrice.ExecuteNonQuery();
                            }
                        }

                        if (purchase.SupplierId > 0 && purchase.RemainingAmount > 0)
                        {
                            const string updateSupplierQuery = "UPDATE Suppliers SET Balance = Balance + @RemainingAmount WHERE Id = @SupplierId;";
                            using (SqlCommand cmdSupplier = new SqlCommand(updateSupplierQuery, connection, transaction))
                            {
                                cmdSupplier.Parameters.Add("@SupplierId", SqlDbType.Int).Value = purchase.SupplierId;
                                cmdSupplier.Parameters.Add("@RemainingAmount", SqlDbType.Decimal).Value = purchase.RemainingAmount;
                                cmdSupplier.ExecuteNonQuery();
                            }
                        }

                        if (purchase.PaidAmount > 0)
                        {
                            const string insertCashQuery = @"
                                INSERT INTO CashTransactions (LocationId, TransactionDate, TransactionType, Amount, Description, ReferenceNumber)
                                VALUES (@LocationId, GETDATE(), N'منصرف', @Amount, @Description, @RefNumber);";

                            using (SqlCommand cmdCash = new SqlCommand(insertCashQuery, connection, transaction))
                            {
                                cmdCash.Parameters.Add("@LocationId", SqlDbType.Int).Value = purchase.LocationId;
                                cmdCash.Parameters.Add("@Amount", SqlDbType.Decimal).Value = purchase.PaidAmount;
                                cmdCash.Parameters.Add("@Description", SqlDbType.NVarChar, 255).Value = "سداد مشتريات - فاتورة رقم " + purchase.InvoiceNumber;
                                cmdCash.Parameters.Add("@RefNumber", SqlDbType.NVarChar, 50).Value = purchase.InvoiceNumber;
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
        public DataTable GetPurchaseRecords(int? locationId, DateTime fromDate, DateTime toDate, string supplierName = "")
        {
            DataTable dt = new DataTable();

            string query = @"
        SELECT 
            p.Id, 
            p.InvoiceNumber, 
            p.InvoiceDate, 
            ISNULL(s.Name, N'مورد نقدي') AS SupplierName, 
            ISNULL(l.Name, N'المخزن الرئيسي') AS LocationName, 
            p.TotalAmount, 
            0 AS Discount, -- المشتريات مفيش فيها خصم في الموديل بتاعك
            p.TotalAmount AS NetAmount, -- الصافي هو الإجمالي
            p.PaidAmount, 
            p.RemainingAmount,
            N'نقدي' AS PaymentMethod -- افتراضي
        FROM Purchases p
        LEFT JOIN Suppliers s ON p.SupplierId = s.Id
        LEFT JOIN Locations l ON p.LocationId = l.Id
        WHERE CAST(p.InvoiceDate AS DATE) >= CAST(@FromDate AS DATE) 
          AND CAST(p.InvoiceDate AS DATE) <= CAST(@ToDate AS DATE)";

            if (locationId.HasValue && locationId.Value > 0)
            {
                query += " AND p.LocationId = @LocationId";
            }

            if (!string.IsNullOrWhiteSpace(supplierName))
            {
                query += " AND ISNULL(s.Name, N'مورد نقدي') LIKE N'%' + @SupplierName + '%'";
            }

            query += " ORDER BY p.InvoiceDate DESC;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate.Date;
                command.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate.Date;

                if (locationId.HasValue && locationId.Value > 0)
                {
                    command.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId.Value;
                }

                if (!string.IsNullOrWhiteSpace(supplierName))
                {
                    command.Parameters.Add("@SupplierName", SqlDbType.NVarChar).Value = supplierName;
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }
    }
}