using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks; // تم إضافتها لدعم Async/Await

namespace MajesticDesktop.Services
{
    public class SalesService
    {
        public SalesService()
        {
            EnsureSchemaUpdated();
        }

        private void EnsureSchemaUpdated()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                const string query = "IF COL_LENGTH(N'CashTransactions', N'PaymentMethod') IS NULL ALTER TABLE CashTransactions ADD PaymentMethod NVARCHAR(100) NULL;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try { connection.Open(); command.ExecuteNonQuery(); } catch { }
                }
            }
        }

        public string GenerateInvoiceNumber()
        {
            string invoiceNumber = "POS-" + DateTime.Now.ToString("yyyyMMdd") + "-";
            const string query = "SELECT ISNULL(MAX(Id), 0) + 1 FROM Sales;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                int nextId = Convert.ToInt32(command.ExecuteScalar());
                invoiceNumber += nextId.ToString("D4");
            }
            return invoiceNumber;
        }

        public async Task<int> GetAvailableStockAsync(int productId, int locationId)
        {
            const string query = "SELECT ISNULL(SUM(Quantity), 0) FROM Stock WHERE ProductId = @ProductId AND LocationId = @LocationId;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@ProductId", SqlDbType.Int).Value = productId;
                command.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;

                await connection.OpenAsync(); // تعديل Async
                object result = await command.ExecuteScalarAsync();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        // ==============================================================
        // التعديل الأول الجوهري: نقل كود جلب الفاتورة من الـ UI إلى هنا
        // ==============================================================
        public async Task<Sale> GetSaleInvoiceByIdAsync(int invoiceId)
        {
            Sale sale = null;
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                const string query = "SELECT * FROM Sales WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = invoiceId;
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            sale = new Sale
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                InvoiceNumber = reader["InvoiceNumber"].ToString(),
                                CustomerId = reader["CustomerId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["CustomerId"]) : null,
                                LocationId = Convert.ToInt32(reader["LocationId"]),
                                InvoiceDate = Convert.ToDateTime(reader["InvoiceDate"]),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                Discount = Convert.ToDecimal(reader["Discount"]),
                                NetAmount = Convert.ToDecimal(reader["NetAmount"]),
                                PaidAmount = Convert.ToDecimal(reader["PaidAmount"]),
                                RemainingAmount = Convert.ToDecimal(reader["RemainingAmount"]),
                                PaymentMethod = reader["PaymentMethod"] != DBNull.Value ? reader["PaymentMethod"].ToString() : "نقدي",
                                Notes = reader["Notes"] != DBNull.Value ? reader["Notes"].ToString() : ""
                            };
                        }
                    }
                }

                if (sale != null)
                {
                    const string detailsQuery = "SELECT ProductId, Quantity, UnitPrice, TotalPrice FROM SaleDetails WHERE SaleId = @SaleId";
                    using (SqlCommand cmdDetails = new SqlCommand(detailsQuery, connection))
                    {
                        cmdDetails.Parameters.Add("@SaleId", SqlDbType.Int).Value = invoiceId;
                        using (SqlDataReader readerDetails = await cmdDetails.ExecuteReaderAsync())
                        {
                            while (await readerDetails.ReadAsync())
                            {
                                sale.Details.Add(new SaleDetail
                                {
                                    ProductId = Convert.ToInt32(readerDetails["ProductId"]),
                                    Quantity = Convert.ToInt32(readerDetails["Quantity"]),
                                    UnitPrice = Convert.ToDecimal(readerDetails["UnitPrice"]),
                                    TotalPrice = Convert.ToDecimal(readerDetails["TotalPrice"])
                                });
                            }
                        }
                    }
                }
            }
            return sale;
        }

        // تحويل دوال الحفظ لتصبح Async
        public async Task SaveSaleInvoiceAsync(Sale sale)
        {
            if (sale == null || sale.Details.Count == 0)
                throw new ArgumentException("الفاتورة فارغة ولا تحتوي على أصناف.");

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                await connection.OpenAsync();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string checkStockQuery = "SELECT ISNULL(SUM(Quantity), 0) FROM Stock WHERE ProductId = @ProductId AND LocationId = @LocationId;";
                        foreach (SaleDetail detail in sale.Details)
                        {
                            using (SqlCommand cmdCheck = new SqlCommand(checkStockQuery, connection, transaction))
                            {
                                cmdCheck.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdCheck.Parameters.Add("@LocationId", SqlDbType.Int).Value = sale.LocationId;

                                object result = await cmdCheck.ExecuteScalarAsync();
                                int currentStock = result != null ? Convert.ToInt32(result) : 0;

                                if (currentStock < detail.Quantity)
                                    throw new Exception($"عذراً، الكمية المطلوبة من أحد الأصناف غير متوفرة. الرصيد الفعلي: {currentStock}");
                            }
                        }

                        const string insertSaleQuery = @"
                            INSERT INTO Sales (InvoiceNumber, CustomerId, LocationId, InvoiceDate, TotalAmount, Discount, NetAmount, PaidAmount, RemainingAmount, PaymentMethod, Notes)
                            VALUES (@InvoiceNumber, @CustomerId, @LocationId, @InvoiceDate, @TotalAmount, @Discount, @NetAmount, @PaidAmount, @RemainingAmount, @PaymentMethod, @Notes);
                            SELECT SCOPE_IDENTITY();";

                        int saleId;
                        using (SqlCommand cmdSale = new SqlCommand(insertSaleQuery, connection, transaction))
                        {
                            cmdSale.Parameters.Add("@InvoiceNumber", SqlDbType.NVarChar, 50).Value = sale.InvoiceNumber;
                            cmdSale.Parameters.Add("@CustomerId", SqlDbType.Int).Value = sale.CustomerId > 0 ? (object)sale.CustomerId : DBNull.Value;
                            cmdSale.Parameters.Add("@LocationId", SqlDbType.Int).Value = sale.LocationId;
                            cmdSale.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = sale.InvoiceDate;
                            cmdSale.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = sale.TotalAmount;
                            cmdSale.Parameters.Add("@Discount", SqlDbType.Decimal).Value = sale.Discount;
                            cmdSale.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = sale.NetAmount;
                            cmdSale.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = sale.PaidAmount;
                            cmdSale.Parameters.Add("@RemainingAmount", SqlDbType.Decimal).Value = sale.RemainingAmount;
                            cmdSale.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(sale.PaymentMethod) ? "نقدي" : sale.PaymentMethod;
                            cmdSale.Parameters.Add("@Notes", SqlDbType.NVarChar, -1).Value = string.IsNullOrWhiteSpace(sale.Notes) ? (object)DBNull.Value : sale.Notes;

                            saleId = Convert.ToInt32(await cmdSale.ExecuteScalarAsync());
                        }

                        const string insertDetailQuery = "INSERT INTO SaleDetails (SaleId, ProductId, Quantity, UnitPrice, TotalPrice) VALUES (@SaleId, @ProductId, @Quantity, @UnitPrice, @TotalPrice);";
                        const string deductStockQuery = "UPDATE Stock SET Quantity = Quantity - @Quantity WHERE ProductId = @ProductId AND LocationId = @LocationId;";

                        foreach (SaleDetail detail in sale.Details)
                        {
                            using (SqlCommand cmdDetail = new SqlCommand(insertDetailQuery, connection, transaction))
                            {
                                cmdDetail.Parameters.Add("@SaleId", SqlDbType.Int).Value = saleId;
                                cmdDetail.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdDetail.Parameters.Add("@Quantity", SqlDbType.Int).Value = detail.Quantity;
                                cmdDetail.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = detail.UnitPrice;
                                cmdDetail.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = detail.TotalPrice;
                                await cmdDetail.ExecuteNonQueryAsync();
                            }

                            using (SqlCommand cmdStock = new SqlCommand(deductStockQuery, connection, transaction))
                            {
                                cmdStock.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdStock.Parameters.Add("@LocationId", SqlDbType.Int).Value = sale.LocationId;
                                cmdStock.Parameters.Add("@Quantity", SqlDbType.Int).Value = detail.Quantity;
                                await cmdStock.ExecuteNonQueryAsync();
                            }
                        }

                        if (sale.PaidAmount > 0)
                        {
                            const string cashQuery = @"
                                INSERT INTO CashTransactions (LocationId, TransactionDate, TransactionType, Amount, Description, ReferenceNumber, PaymentMethod)
                                VALUES (@CashLocId, GETDATE(), N'وارد', @CashAmount, @CashDesc, @CashRefNo, @CashMethod);";

                            using (SqlCommand cmdCash = new SqlCommand(cashQuery, connection, transaction))
                            {
                                cmdCash.Parameters.Add("@CashLocId", SqlDbType.Int).Value = sale.LocationId;
                                cmdCash.Parameters.Add("@CashAmount", SqlDbType.Decimal).Value = sale.PaidAmount;
                                cmdCash.Parameters.Add("@CashDesc", SqlDbType.NVarChar, 255).Value = $"إيراد مبيعات ({sale.PaymentMethod}) - فاتورة رقم {sale.InvoiceNumber}";
                                cmdCash.Parameters.Add("@CashRefNo", SqlDbType.NVarChar, 50).Value = sale.InvoiceNumber;
                                cmdCash.Parameters.Add("@CashMethod", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(sale.PaymentMethod) ? "نقدي" : sale.PaymentMethod;
                                await cmdCash.ExecuteNonQueryAsync();
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

        public async Task UpdateSaleInvoiceAsync(Sale updatedSale)
        {
            if (updatedSale == null || updatedSale.Id <= 0 || updatedSale.Details.Count == 0)
                throw new ArgumentException("بيانات الفاتورة غير صحيحة للتعديل.");

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                await connection.OpenAsync();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string checkClosedQuery = "SELECT ISNULL(IsShiftClosed, 0) FROM Sales WHERE Id = @SaleId;";
                        using (SqlCommand cmdCheckClosed = new SqlCommand(checkClosedQuery, connection, transaction))
                        {
                            cmdCheckClosed.Parameters.Add("@SaleId", SqlDbType.Int).Value = updatedSale.Id;
                            object result = await cmdCheckClosed.ExecuteScalarAsync();
                            if (result != null && result != DBNull.Value && Convert.ToBoolean(result))
                            {
                                throw new InvalidOperationException("لا يمكن تعديل هذه الفاتورة لأنها ضمن وردية تم تقفيلها بالفعل.");
                            }
                        }

                        var oldDetails = new List<SaleDetail>();
                        const string getOldDetailsQuery = "SELECT ProductId, Quantity FROM SaleDetails WHERE SaleId = @SaleId";
                        using (SqlCommand cmdGetOld = new SqlCommand(getOldDetailsQuery, connection, transaction))
                        {
                            cmdGetOld.Parameters.Add("@SaleId", SqlDbType.Int).Value = updatedSale.Id;
                            using (SqlDataReader reader = await cmdGetOld.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    oldDetails.Add(new SaleDetail
                                    {
                                        ProductId = Convert.ToInt32(reader["ProductId"]),
                                        Quantity = Convert.ToInt32(reader["Quantity"])
                                    });
                                }
                            }
                        }

                        const string returnStockQuery = "UPDATE Stock SET Quantity = Quantity + @Quantity WHERE ProductId = @ProductId AND LocationId = @LocationId;";
                        foreach (var oldDetail in oldDetails)
                        {
                            using (SqlCommand cmdReturnStock = new SqlCommand(returnStockQuery, connection, transaction))
                            {
                                cmdReturnStock.Parameters.Add("@ProductId", SqlDbType.Int).Value = oldDetail.ProductId;
                                cmdReturnStock.Parameters.Add("@LocationId", SqlDbType.Int).Value = updatedSale.LocationId;
                                cmdReturnStock.Parameters.Add("@Quantity", SqlDbType.Int).Value = oldDetail.Quantity;
                                await cmdReturnStock.ExecuteNonQueryAsync();
                            }
                        }

                        const string deleteOldCashQuery = "DELETE FROM CashTransactions WHERE ReferenceNumber = @InvoiceNumber AND Description LIKE N'إيراد مبيعات%'";
                        using (SqlCommand cmdDeleteCash = new SqlCommand(deleteOldCashQuery, connection, transaction))
                        {
                            cmdDeleteCash.Parameters.Add("@InvoiceNumber", SqlDbType.NVarChar, 50).Value = updatedSale.InvoiceNumber;
                            await cmdDeleteCash.ExecuteNonQueryAsync();
                        }

                        const string deleteOldDetailsQuery = "DELETE FROM SaleDetails WHERE SaleId = @SaleId";
                        using (SqlCommand cmdDeleteDetails = new SqlCommand(deleteOldDetailsQuery, connection, transaction))
                        {
                            cmdDeleteDetails.Parameters.Add("@SaleId", SqlDbType.Int).Value = updatedSale.Id;
                            await cmdDeleteDetails.ExecuteNonQueryAsync();
                        }

                        const string checkStockQuery = "SELECT ISNULL(SUM(Quantity), 0) FROM Stock WHERE ProductId = @ProductId AND LocationId = @LocationId;";
                        foreach (SaleDetail detail in updatedSale.Details)
                        {
                            using (SqlCommand cmdCheck = new SqlCommand(checkStockQuery, connection, transaction))
                            {
                                cmdCheck.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdCheck.Parameters.Add("@LocationId", SqlDbType.Int).Value = updatedSale.LocationId;

                                object result = await cmdCheck.ExecuteScalarAsync();
                                int currentStock = result != null ? Convert.ToInt32(result) : 0;

                                if (currentStock < detail.Quantity)
                                    throw new Exception($"عذراً، الكمية المطلوبة من الصنف غير متوفرة بعد التعديل. الرصيد الفعلي: {currentStock}");
                            }
                        }

                        const string updateSaleQuery = @"
                            UPDATE Sales 
                            SET CustomerId = @CustomerId, LocationId = @LocationId, InvoiceDate = @InvoiceDate, 
                                TotalAmount = @TotalAmount, Discount = @Discount, NetAmount = @NetAmount, 
                                PaidAmount = @PaidAmount, RemainingAmount = @RemainingAmount, PaymentMethod = @PaymentMethod, Notes = @Notes
                            WHERE Id = @SaleId;";

                        using (SqlCommand cmdUpdateSale = new SqlCommand(updateSaleQuery, connection, transaction))
                        {
                            cmdUpdateSale.Parameters.Add("@SaleId", SqlDbType.Int).Value = updatedSale.Id;
                            cmdUpdateSale.Parameters.Add("@CustomerId", SqlDbType.Int).Value = updatedSale.CustomerId > 0 ? (object)updatedSale.CustomerId : DBNull.Value;
                            cmdUpdateSale.Parameters.Add("@LocationId", SqlDbType.Int).Value = updatedSale.LocationId;
                            cmdUpdateSale.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = updatedSale.InvoiceDate;
                            cmdUpdateSale.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = updatedSale.TotalAmount;
                            cmdUpdateSale.Parameters.Add("@Discount", SqlDbType.Decimal).Value = updatedSale.Discount;
                            cmdUpdateSale.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = updatedSale.NetAmount;
                            cmdUpdateSale.Parameters.Add("@PaidAmount", SqlDbType.Decimal).Value = updatedSale.PaidAmount;
                            cmdUpdateSale.Parameters.Add("@RemainingAmount", SqlDbType.Decimal).Value = updatedSale.RemainingAmount;
                            cmdUpdateSale.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(updatedSale.PaymentMethod) ? "نقدي" : updatedSale.PaymentMethod;
                            cmdUpdateSale.Parameters.Add("@Notes", SqlDbType.NVarChar, -1).Value = string.IsNullOrWhiteSpace(updatedSale.Notes) ? (object)DBNull.Value : updatedSale.Notes;
                            await cmdUpdateSale.ExecuteNonQueryAsync();
                        }

                        const string insertDetailQuery = "INSERT INTO SaleDetails (SaleId, ProductId, Quantity, UnitPrice, TotalPrice) VALUES (@SaleId, @ProductId, @Quantity, @UnitPrice, @TotalPrice);";
                        const string deductStockQuery = "UPDATE Stock SET Quantity = Quantity - @Quantity WHERE ProductId = @ProductId AND LocationId = @LocationId;";

                        foreach (SaleDetail detail in updatedSale.Details)
                        {
                            using (SqlCommand cmdDetail = new SqlCommand(insertDetailQuery, connection, transaction))
                            {
                                cmdDetail.Parameters.Add("@SaleId", SqlDbType.Int).Value = updatedSale.Id;
                                cmdDetail.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdDetail.Parameters.Add("@Quantity", SqlDbType.Int).Value = detail.Quantity;
                                cmdDetail.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = detail.UnitPrice;
                                cmdDetail.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = detail.TotalPrice;
                                await cmdDetail.ExecuteNonQueryAsync();
                            }

                            using (SqlCommand cmdStock = new SqlCommand(deductStockQuery, connection, transaction))
                            {
                                cmdStock.Parameters.Add("@ProductId", SqlDbType.Int).Value = detail.ProductId;
                                cmdStock.Parameters.Add("@LocationId", SqlDbType.Int).Value = updatedSale.LocationId;
                                cmdStock.Parameters.Add("@Quantity", SqlDbType.Int).Value = detail.Quantity;
                                await cmdStock.ExecuteNonQueryAsync();
                            }
                        }

                        if (updatedSale.PaidAmount > 0)
                        {
                            const string cashQuery = @"
                                INSERT INTO CashTransactions (LocationId, TransactionDate, TransactionType, Amount, Description, ReferenceNumber, PaymentMethod)
                                VALUES (@CashLocId, GETDATE(), N'وارد', @CashAmount, @CashDesc, @CashRefNo, @CashMethod);";

                            using (SqlCommand cmdCash = new SqlCommand(cashQuery, connection, transaction))
                            {
                                cmdCash.Parameters.Add("@CashLocId", SqlDbType.Int).Value = updatedSale.LocationId;
                                cmdCash.Parameters.Add("@CashAmount", SqlDbType.Decimal).Value = updatedSale.PaidAmount;
                                cmdCash.Parameters.Add("@CashDesc", SqlDbType.NVarChar, 255).Value = $"إيراد مبيعات ({updatedSale.PaymentMethod}) - فاتورة رقم {updatedSale.InvoiceNumber} (تعديل)";
                                cmdCash.Parameters.Add("@CashRefNo", SqlDbType.NVarChar, 50).Value = updatedSale.InvoiceNumber;
                                cmdCash.Parameters.Add("@CashMethod", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(updatedSale.PaymentMethod) ? "نقدي" : updatedSale.PaymentMethod;
                                await cmdCash.ExecuteNonQueryAsync();
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

        // تم الإبقاء على الدوال الأخرى كما هي (GetActiveCustomers, GetCustomersByLocation, GetSalesRecords)
        public List<Customer> GetCustomersByLocation(int locationId)
        {
            List<Customer> customers = new List<Customer>();
            const string query = "SELECT Id, Name, Phone, Balance FROM Customers WHERE IsActive = 1 AND LocationId = @LocationId ORDER BY Id ASC;";
            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Phone = reader["Phone"]?.ToString(),
                            Balance = reader["Balance"] != DBNull.Value ? Convert.ToDecimal(reader["Balance"]) : 0
                        });
                    }
                }
            }
            return customers;
        }

        public List<Customer> GetActiveCustomers() { /* ... */ return new List<Customer>(); }
        public List<Customer> GetWarehouseCustomersOnly() { /* ... */ return new List<Customer>(); }
        public DataTable GetSalesRecords(int? locationId, DateTime fromDate, DateTime toDate, string customerName = "") { /* ... */ return new DataTable(); }
    }
}