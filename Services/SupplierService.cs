using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace MajesticDesktop.Services
{
    public class SupplierService
    {
        public SupplierService()
        {
            EnsureSchemaUpdated();
        }

        private void EnsureSchemaUpdated()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                const string query = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Suppliers' AND xtype='U')
                    BEGIN
                        CREATE TABLE Suppliers (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Name NVARCHAR(150) NOT NULL,
                            Phone NVARCHAR(50) NULL,
                            Region NVARCHAR(150) NULL,
                            Notes NVARCHAR(500) NULL,
                            Balance DECIMAL(18,2) NOT NULL DEFAULT 0,
                            IsActive BIT NOT NULL DEFAULT 1,
                            LocationId INT NOT NULL DEFAULT 1
                        );
                    END

                    IF COL_LENGTH('Suppliers', 'LocationId') IS NULL ALTER TABLE Suppliers ADD LocationId INT NOT NULL DEFAULT 1;
                    IF COL_LENGTH('Suppliers', 'Region') IS NULL ALTER TABLE Suppliers ADD Region NVARCHAR(150) NULL;
                    IF COL_LENGTH('Suppliers', 'Notes') IS NULL ALTER TABLE Suppliers ADD Notes NVARCHAR(500) NULL;

                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SupplierPayments' AND xtype='U')
                    BEGIN
                        CREATE TABLE SupplierPayments (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            SupplierId INT NOT NULL,
                            Amount DECIMAL(18,2) NOT NULL,
                            PaymentDate DATETIME NOT NULL DEFAULT GETDATE(),
                            LocationId INT NOT NULL DEFAULT 1
                        );
                    END
                    IF COL_LENGTH('SupplierPayments', 'LocationId') IS NULL ALTER TABLE SupplierPayments ADD LocationId INT NOT NULL DEFAULT 1;
                ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try { connection.Open(); command.ExecuteNonQuery(); } catch { }
                }
            }
        }

        public DataTable GetAllLocations()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = DbConnection.CreateConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM Locations WHERE IsActive = 1 ORDER BY Id", con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
            return dt;
        }

        public List<Supplier> GetAllSuppliers(int locationId)
        {
            List<Supplier> suppliers = new List<Supplier>();
            string query = @"
                DECLARE @Sql NVARCHAR(MAX) = N'
                SELECT 
                    s.Id, s.Name, s.Phone, s.Region, s.Notes,
                    (ISNULL(s.Balance, 0) ';
                
                IF EXISTS (SELECT 1 FROM sysobjects WHERE name = 'PurchaseInvoices')
                    SET @Sql = @Sql + N' + ISNULL((SELECT SUM(RemainingAmount) FROM PurchaseInvoices WHERE SupplierId = s.Id AND RemainingAmount > 0), 0) ';
                IF EXISTS (SELECT 1 FROM sysobjects WHERE name = 'PurchaseReturns')
                    SET @Sql = @Sql + N' - ISNULL((SELECT SUM(TotalRefundAmount) FROM PurchaseReturns WHERE SupplierId = s.Id), 0) ';
                    
                SET @Sql = @Sql + N' - ISNULL((SELECT SUM(Amount) FROM SupplierPayments WHERE SupplierId = s.Id), 0)) AS CurrentBalance 
                FROM Suppliers s 
                WHERE s.IsActive = 1 AND s.LocationId = @LocId ORDER BY s.Id ASC;';
                
                EXEC sp_executesql @Sql, N'@LocId INT', @LocId = @LocationId;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string sName = reader["Name"].ToString();
                        if (sName.Contains("???") || sName.Contains("؟") || sName == "/") sName = "مورد نقدي";

                        suppliers.Add(new Supplier
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = sName,
                            Phone = reader["Phone"]?.ToString(),
                            Region = reader["Region"]?.ToString(),
                            Notes = reader["Notes"]?.ToString(),
                            Balance = Convert.ToDecimal(reader["CurrentBalance"])
                        });
                    }
                }
            }
            return suppliers;
        }

        public decimal GetSupplierInitialBalance(int supplierId)
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand("SELECT ISNULL(Balance, 0) FROM Suppliers WHERE Id = @Id;", connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = supplierId;
                connection.Open();
                var result = command.ExecuteScalar();
                return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        public void AddSupplier(Supplier supplier, int locationId)
        {
            const string query = "INSERT INTO Suppliers (Name, Phone, Region, Notes, Balance, IsActive, LocationId) VALUES (@Name, @Phone, @Region, @Notes, @Balance, 1, @LocationId);";
            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = supplier.Name.Trim();
                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(supplier.Phone) ? (object)DBNull.Value : supplier.Phone.Trim();
                command.Parameters.Add("@Region", SqlDbType.NVarChar, 150).Value = string.IsNullOrWhiteSpace(supplier.Region) ? (object)DBNull.Value : supplier.Region.Trim();
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(supplier.Notes) ? (object)DBNull.Value : supplier.Notes.Trim();
                command.Parameters.Add("@Balance", SqlDbType.Decimal).Value = supplier.Balance;
                command.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateSupplier(Supplier supplier)
        {
            const string query = "UPDATE Suppliers SET Name = @Name, Phone = @Phone, Region = @Region, Notes = @Notes, Balance = @Balance WHERE Id = @Id;";
            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = supplier.Id;
                command.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = supplier.Name.Trim();
                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(supplier.Phone) ? (object)DBNull.Value : supplier.Phone.Trim();
                command.Parameters.Add("@Region", SqlDbType.NVarChar, 150).Value = string.IsNullOrWhiteSpace(supplier.Region) ? (object)DBNull.Value : supplier.Region.Trim();
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(supplier.Notes) ? (object)DBNull.Value : supplier.Notes.Trim();
                command.Parameters.Add("@Balance", SqlDbType.Decimal).Value = supplier.Balance;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteSupplier(int supplierId)
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand("UPDATE Suppliers SET IsActive = 0 WHERE Id = @Id;", connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = supplierId;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // تم التصحيح: بقى بياخد وسيلة الدفع كباراميتر، وبيسجلها في CashTransactions
        // كـ TransactionType بدل ما كانت مثبتة على N'منصرف' دايمًا. النتيجة: لو السداد
        // بإنستاباي الكردي أو الحكش، الفلوس بتتستبعد تلقائيًا من حساب الكاش النقدي في
        // تقارير الخزنة (بنفس آلية استبعاد مبيعات الإنستا)، يعني بتتخصم من نفس المحفظة
        // اللي دفعت بيها فعليًا مش من رصيد الدرج النقدي.
        public void PaySupplierDebt(int supplierId, decimal amount, int locationId, string paymentMethod)
        {
            if (amount <= 0) throw new ArgumentException("المبلغ يجب أن يكون أكبر من الصفر.");

            string method = string.IsNullOrWhiteSpace(paymentMethod) ? "نقدي" : paymentMethod.Trim();

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string insertPaymentQuery = "INSERT INTO SupplierPayments (SupplierId, Amount, PaymentDate, LocationId) VALUES (@Id, @Amount, GETDATE(), @LocationId);";
                        using (SqlCommand cmdInsert = new SqlCommand(insertPaymentQuery, connection, transaction))
                        {
                            cmdInsert.Parameters.Add("@Id", SqlDbType.Int).Value = supplierId;
                            cmdInsert.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
                            cmdInsert.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;
                            cmdInsert.ExecuteNonQuery();
                        }

                        const string cashQuery = "INSERT INTO CashTransactions (LocationId, TransactionDate, TransactionType, Amount, Description, ReferenceNumber) VALUES (@LocationId, GETDATE(), @Method, @Amount, N'سداد دفعة لمورد كود ' + CAST(@Id AS NVARCHAR) + N' عبر ' + @Method, N'SupplierPayment');";
                        using (SqlCommand cmdCash = new SqlCommand(cashQuery, connection, transaction))
                        {
                            cmdCash.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;
                            cmdCash.Parameters.Add("@Id", SqlDbType.Int).Value = supplierId;
                            cmdCash.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
                            cmdCash.Parameters.Add("@Method", SqlDbType.NVarChar, 100).Value = method;
                            cmdCash.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch { transaction.Rollback(); throw; }
                }
            }
        }

        public DataTable GetSupplierLedger(int supplierId)
        {
            DataTable dt = new DataTable();
            string query = @"
                DECLARE @Sql NVARCHAR(MAX) = N'';
                IF EXISTS (SELECT 1 FROM sysobjects WHERE name = 'PurchaseInvoices')
                    SET @Sql = @Sql + N'SELECT Id as RefId, InvoiceDate as TransDate, N''فاتورة مشتريات ('' + ISNULL(CAST(Id AS NVARCHAR), '''') + N'')'' as Description, RemainingAmount as Credit, 0 as Debit, ''Invoice'' as TransType FROM PurchaseInvoices WHERE SupplierId = @Id AND RemainingAmount > 0 UNION ALL ';
                IF EXISTS (SELECT 1 FROM sysobjects WHERE name = 'PurchaseReturns')
                    SET @Sql = @Sql + N'SELECT Id as RefId, ReturnDate as TransDate, N''مرتجع مشتريات'' as Description, 0 as Credit, TotalRefundAmount as Debit, ''Return'' as TransType FROM PurchaseReturns WHERE SupplierId = @Id AND TotalRefundAmount > 0 UNION ALL ';
                
                SET @Sql = @Sql + N'SELECT Id as RefId, PaymentDate as TransDate, N''سداد دفعة للمورد'' as Description, 0 as Credit, Amount as Debit, ''Payment'' as TransType FROM SupplierPayments WHERE SupplierId = @Id ORDER BY TransDate ASC';
                
                EXEC sp_executesql @Sql, N'@Id INT', @Id = @SuppId;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@SuppId", SqlDbType.Int).Value = supplierId;
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public void ImportSuppliersFromCsv(string filePath, int locationId)
        {
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string query = "INSERT INTO Suppliers (Name, Phone, Region, Notes, Balance, IsActive, LocationId) VALUES (@Name, @Phone, @Region, @Notes, @Balance, 1, @LocationId);";

                        for (int i = 1; i < lines.Length; i++)
                        {
                            string line = lines[i];
                            if (string.IsNullOrWhiteSpace(line)) continue;

                            List<string> values = ParseCsvLine(line);

                            if (values.Count >= 6)
                            {
                                string name = values[1].Trim();
                                string phone = values[2].Trim();
                                string region = values[3].Trim();
                                string notes = values[4].Trim();
                                decimal balance = 0;

                                decimal.TryParse(values[5].Trim(), out balance);

                                if (!string.IsNullOrEmpty(name))
                                {
                                    using (SqlCommand command = new SqlCommand(query, connection, transaction))
                                    {
                                        command.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = name;
                                        command.Parameters.Add("@Phone", SqlDbType.NVarChar, 50).Value = string.IsNullOrWhiteSpace(phone) ? (object)DBNull.Value : phone;
                                        command.Parameters.Add("@Region", SqlDbType.NVarChar, 150).Value = string.IsNullOrWhiteSpace(region) ? (object)DBNull.Value : region;
                                        command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes;
                                        command.Parameters.Add("@Balance", SqlDbType.Decimal).Value = balance;
                                        command.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;
                                        command.ExecuteNonQuery();
                                    }
                                }
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

        private static List<string> ParseCsvLine(string line)
        {
            List<string> result = new List<string>();
            StringBuilder field = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            field.Append('"');
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        field.Append(c);
                    }
                }
                else
                {
                    if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else if (c == ',')
                    {
                        result.Add(field.ToString());
                        field.Clear();
                    }
                    else
                    {
                        field.Append(c);
                    }
                }
            }

            result.Add(field.ToString());
            return result;
        }
    }
}