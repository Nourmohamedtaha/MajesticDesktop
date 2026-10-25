using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;

namespace MajesticDesktop.Services
{
    public class CustomerService
    {
        public const int SystemCashCustomerId = 1;

        // طرق السداد المدعومة (الكلمتين «الكردي» و«الحكش» هما اللي خزينة الإدارة بتدوّر عليهم)
        public static readonly string[] PaymentMethods = { "نقدي", "انستاباي الكردي", "انستاباي الحكش" };

        private static readonly object _schemaLock = new object();
        private static bool _schemaChecked;

        // لو غيّرت تعريف الـ View غيّر رقم النسخة هنا عشان يتحدّث مرة واحدة
        private const string ViewVersionMarker = "/*cb_v1*/";

        public CustomerService()
        {
            EnsureSchemaUpdated();
        }

        // ======================= تجهيز الداتابيز (مرة واحدة لكل تشغيل) =======================
        private static void EnsureSchemaUpdated()
        {
            if (_schemaChecked) return;

            lock (_schemaLock)
            {
                if (_schemaChecked) return;

                using (SqlConnection connection = DbConnection.CreateConnection())
                {
                    connection.Open();

                    const string schemaSql = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CustomerPayments' AND xtype='U')
                        BEGIN
                            CREATE TABLE CustomerPayments (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                CustomerId INT NOT NULL,
                                Amount DECIMAL(18,2) NOT NULL,
                                PaymentDate DATETIME NOT NULL DEFAULT GETDATE()
                            );
                        END

                        IF COL_LENGTH('Customers', 'LocationId') IS NULL 
                            ALTER TABLE Customers ADD LocationId INT NOT NULL DEFAULT 1;

                        IF COL_LENGTH('Customers', 'Region') IS NULL 
                            ALTER TABLE Customers ADD Region NVARCHAR(150) NULL;

                        IF COL_LENGTH('Customers', 'Notes') IS NULL 
                            ALTER TABLE Customers ADD Notes NVARCHAR(500) NULL;

                        IF COL_LENGTH('CustomerPayments', 'LocationId') IS NULL 
                            ALTER TABLE CustomerPayments ADD LocationId INT NOT NULL DEFAULT 1;

                        -- طريقة الدفع (نقدي / انستاباي الكردي / انستاباي الحكش) - الخزينة بتقراه
                        IF COL_LENGTH('CustomerPayments', 'PaymentMethod') IS NULL 
                            ALTER TABLE CustomerPayments ADD PaymentMethod NVARCHAR(50) NULL;";

                    using (SqlCommand command = new SqlCommand(schemaSql, connection))
                    {
                        command.CommandTimeout = 60;
                        command.ExecuteNonQuery();
                    }

                    EnsureCustomerBalanceView(connection);
                }

                _schemaChecked = true;
            }
        }

        // الـ View بيتعدّل بس لو علامة النسخة مش موجودة في تعريفه (مفيش ALTER في كل فتحة شاشة).
        // نفس المعادلة لازم تفضل متطابقة في أي مكان تاني بيحسب المديونية؛ الأفضل إنهم كلهم يقروا من الـ View.
        private static void EnsureCustomerBalanceView(SqlConnection connection)
        {
            const string checkSql = @"
                SELECT CASE WHEN OBJECT_DEFINITION(OBJECT_ID(N'dbo.CustomerCurrentBalances')) LIKE N'%/*cb_v1*/%' THEN 1 ELSE 0 END;";

            using (SqlCommand cmd = new SqlCommand(checkSql, connection))
            {
                if (Convert.ToInt32(cmd.ExecuteScalar()) == 1) return;
            }

            const string ensureViewExists = @"
                IF NOT EXISTS (SELECT 1 FROM sys.views WHERE name = 'CustomerCurrentBalances')
                    EXEC('CREATE VIEW dbo.CustomerCurrentBalances AS
                          SELECT CAST(0 AS INT) AS CustomerId, CAST(0 AS DECIMAL(18,2)) AS CurrentBalance WHERE 1 = 0');";

            const string syncViewDefinition = @"
                ALTER VIEW dbo.CustomerCurrentBalances AS
                /*cb_v1*/
                SELECT c.Id AS CustomerId,
                       (ISNULL(c.Balance, 0)
                        + ISNULL((SELECT SUM(s.RemainingAmount) FROM Sales s WHERE s.CustomerId = c.Id AND s.RemainingAmount > 0), 0)
                        - ISNULL((SELECT SUM(r.TotalRefundAmount) FROM SalesReturns r WHERE r.CustomerId = c.Id), 0)
                        - ISNULL((SELECT SUM(p.Amount) FROM CustomerPayments p WHERE p.CustomerId = c.Id), 0)) AS CurrentBalance
                FROM Customers c;";

            using (SqlCommand cmd = new SqlCommand(ensureViewExists, connection)) cmd.ExecuteNonQuery();
            using (SqlCommand cmd = new SqlCommand(syncViewDefinition, connection)) cmd.ExecuteNonQuery();
        }

        // ======================= أدوات مساعدة =======================
        private static object ToDb(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? (object)DBNull.Value : s.Trim();
        }

        private static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return "";
            s = s.Trim();
            return s.Length <= max ? s : s.Substring(0, max);
        }

        private static SqlParameter AddMoney(SqlCommand cmd, string name, decimal value)
        {
            SqlParameter p = cmd.Parameters.Add(name, SqlDbType.Decimal);
            p.Precision = 18;
            p.Scale = 2;
            p.Value = Math.Round(value, 2, MidpointRounding.AwayFromZero);
            return p;
        }

        private static void SafeRollback(SqlTransaction transaction)
        {
            try { transaction.Rollback(); }
            catch (InvalidOperationException) { /* الـ transaction اتقفلت بالفعل */ }
        }

        private static string ReadString(SqlDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? null : Convert.ToString(reader.GetValue(ordinal));
        }

        // بيقبل الأرقام العربية/الهندية والفاصلة العربية، وبيستخدم InvariantCulture
        public static bool TryParseAmount(string text, out decimal value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(text)) return false;

            StringBuilder sb = new StringBuilder();
            foreach (char ch in text.Trim())
            {
                if (ch >= '\u0660' && ch <= '\u0669') sb.Append((char)('0' + (ch - '\u0660')));
                else if (ch >= '\u06F0' && ch <= '\u06F9') sb.Append((char)('0' + (ch - '\u06F0')));
                else if (ch == '\u066B') sb.Append('.');
                else if (ch == '\u066C') continue;
                else sb.Append(ch);
            }
            return decimal.TryParse(sb.ToString(), NumberStyles.Number, CultureInfo.InvariantCulture, out value);
        }

        // ======================= استعلامات =======================
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

        public List<Customer> GetAllCustomers(int locationId)
        {
            List<Customer> customers = new List<Customer>();
            const string query = @"
                SELECT c.Id, c.Name, c.Phone, c.Region, c.Notes, cb.CurrentBalance
                FROM Customers c 
                INNER JOIN CustomerCurrentBalances cb ON cb.CustomerId = c.Id
                WHERE c.IsActive = 1 AND c.LocationId = @LocationId
                ORDER BY c.Id ASC;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    int oId = reader.GetOrdinal("Id");
                    int oName = reader.GetOrdinal("Name");
                    int oPhone = reader.GetOrdinal("Phone");
                    int oRegion = reader.GetOrdinal("Region");
                    int oNotes = reader.GetOrdinal("Notes");
                    int oBalance = reader.GetOrdinal("CurrentBalance");

                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader.GetValue(oId));
                        string name = ReadString(reader, oName) ?? "";

                        // إصلاح الأسماء التالفة بقى مقتصر على العميل النقدي الأساسي بس (مش أي اسم فيه "؟")
                        if (id == SystemCashCustomerId && (name.Contains("???") || name.Contains("؟") || name == "/"))
                            name = "عميل نقدي";

                        customers.Add(new Customer
                        {
                            Id = id,
                            Name = name,
                            Phone = ReadString(reader, oPhone),
                            Region = ReadString(reader, oRegion),
                            Notes = ReadString(reader, oNotes),
                            Balance = reader.IsDBNull(oBalance) ? 0m : Convert.ToDecimal(reader.GetValue(oBalance))
                        });
                    }
                }
            }
            return customers;
        }

        public decimal GetCustomerInitialBalance(int customerId)
        {
            const string query = "SELECT ISNULL(Balance, 0) FROM Customers WHERE Id = @Id;";
            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = customerId;
                connection.Open();
                object result = command.ExecuteScalar();
                return (result != null && result != DBNull.Value) ? Convert.ToDecimal(result) : 0m;
            }
        }

        // ======================= إضافة / تعديل / حذف =======================
        public void AddCustomer(Customer customer, int locationId)
        {
            const string query = @"INSERT INTO Customers (Name, Phone, Region, Notes, Balance, IsActive, LocationId) 
                                   VALUES (@Name, @Phone, @Region, @Notes, @Balance, 1, @LocationId);";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = Truncate(customer.Name, 150);
                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 50).Value = ToDb(customer.Phone);
                command.Parameters.Add("@Region", SqlDbType.NVarChar, 150).Value = ToDb(customer.Region);
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = ToDb(customer.Notes);
                AddMoney(command, "@Balance", customer.Balance);
                command.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer.Id == SystemCashCustomerId)
                throw new InvalidOperationException("لا يمكن تعديل العميل النقدي الأساسي.");

            const string query = @"UPDATE Customers 
                                   SET Name = @Name, Phone = @Phone, Region = @Region, Notes = @Notes, Balance = @Balance 
                                   WHERE Id = @Id AND IsActive = 1;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = customer.Id;
                command.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = Truncate(customer.Name, 150);
                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 50).Value = ToDb(customer.Phone);
                command.Parameters.Add("@Region", SqlDbType.NVarChar, 150).Value = ToDb(customer.Region);
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = ToDb(customer.Notes);
                AddMoney(command, "@Balance", customer.Balance);

                connection.Open();
                if (command.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException("العميل غير موجود أو اتحذف.");
            }
        }

        public void DeleteCustomer(int customerId)
        {
            if (customerId == SystemCashCustomerId)
                throw new InvalidOperationException("لا يمكن حذف العميل النقدي الأساسي.");

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();

                // الحماية في الـ Service كمان (مش في الشاشة بس): ممنوع حذف عميل عليه رصيد
                using (SqlCommand check = new SqlCommand(
                    "SELECT ISNULL(CurrentBalance, 0) FROM CustomerCurrentBalances WHERE CustomerId = @Id;", connection))
                {
                    check.Parameters.Add("@Id", SqlDbType.Int).Value = customerId;
                    object bal = check.ExecuteScalar();
                    if (bal != null && bal != DBNull.Value && Convert.ToDecimal(bal) != 0m)
                        throw new InvalidOperationException("لا يمكن حذف عميل عليه رصيد. سدّد الرصيد أو صفّره الأول.");
                }

                using (SqlCommand command = new SqlCommand("UPDATE Customers SET IsActive = 0 WHERE Id = @Id AND IsActive = 1;", connection))
                {
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = customerId;
                    if (command.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("العميل غير موجود أو اتحذف بالفعل.");
                }
            }
        }

        // ======================= سداد المديونية =======================
        // نقدي: بيتسجل في CustomerPayments + حركة وارد في الكاش (زي ما كان).
        // انستاباي: بيتسجل في CustomerPayments بس مع طريقة الدفع، لأن الفلوس مش في الدرج
        // وخزينة الإدارة بتحسبه من CustomerPayments.PaymentMethod.
        public void PayCustomerDebt(int customerId, decimal amount, int locationId, string paymentMethod = "نقدي")
        {
            amount = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
            if (amount <= 0) throw new ArgumentException("المبلغ يجب أن يكون أكبر من الصفر.");
            if (customerId == SystemCashCustomerId) throw new InvalidOperationException("العميل النقدي ليس عليه مديونية.");

            if (string.IsNullOrWhiteSpace(paymentMethod)) paymentMethod = PaymentMethods[0];
            if (Array.IndexOf(PaymentMethods, paymentMethod) < 0) throw new ArgumentException("طريقة الدفع غير معروفة.");
            bool isCash = paymentMethod == PaymentMethods[0];

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // العميل لازم يكون موجود ونشط وتابع للفرع
                        string customerName;
                        using (SqlCommand cmdCheck = new SqlCommand(
                            "SELECT Name FROM Customers WHERE Id = @Id AND IsActive = 1 AND LocationId = @LocationId;", connection, transaction))
                        {
                            cmdCheck.Parameters.Add("@Id", SqlDbType.Int).Value = customerId;
                            cmdCheck.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;
                            object nameObj = cmdCheck.ExecuteScalar();
                            if (nameObj == null) throw new InvalidOperationException("العميل غير موجود أو غير نشط في هذا الفرع.");
                            customerName = Convert.ToString(nameObj);
                        }

                        const string insertPaymentQuery = @"
                            INSERT INTO CustomerPayments (CustomerId, Amount, PaymentDate, LocationId, PaymentMethod) 
                            VALUES (@Id, @Amount, GETDATE(), @LocationId, @Method);";
                        using (SqlCommand cmdInsert = new SqlCommand(insertPaymentQuery, connection, transaction))
                        {
                            cmdInsert.Parameters.Add("@Id", SqlDbType.Int).Value = customerId;
                            AddMoney(cmdInsert, "@Amount", amount);
                            cmdInsert.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;
                            cmdInsert.Parameters.Add("@Method", SqlDbType.NVarChar, 50).Value = paymentMethod;
                            cmdInsert.ExecuteNonQuery();
                        }

                        if (isCash)
                        {
                            const string cashQuery = @"
                                INSERT INTO CashTransactions (LocationId, TransactionDate, TransactionType, Amount, Description, ReferenceNumber) 
                                VALUES (@LocationId, GETDATE(), N'وارد', @Amount, @Desc, N'CustomerDebt');";
                            using (SqlCommand cmdCash = new SqlCommand(cashQuery, connection, transaction))
                            {
                                cmdCash.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;
                                AddMoney(cmdCash, "@Amount", amount);
                                cmdCash.Parameters.Add("@Desc", SqlDbType.NVarChar, 255).Value =
                                    "سداد مديونية من عميل كود " + customerId + " - " + Truncate(customerName, 100);
                                cmdCash.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        SafeRollback(transaction);
                        throw;
                    }
                }
            }
        }

        // ======================= كشف الحساب =======================
        public DataTable GetCustomerLedger(int customerId)
        {
            DataTable dt = new DataTable();
            const string query = @"
                SELECT 
                    Id AS RefId, 
                    InvoiceDate AS TransDate, 
                    N'فاتورة مبيعات (' + ISNULL(CAST(InvoiceNumber AS NVARCHAR(50)), CAST(Id AS NVARCHAR(20))) + N')' AS Description, 
                    CAST(RemainingAmount AS DECIMAL(18,2)) AS Debit, 
                    CAST(0 AS DECIMAL(18,2)) AS Credit, 
                    'Invoice' AS TransType 
                FROM Sales 
                WHERE CustomerId = @Id AND RemainingAmount > 0

                UNION ALL

                SELECT 
                    Id, 
                    PaymentDate, 
                    N'سداد دفعة (' + ISNULL(PaymentMethod, N'نقدي') + N')', 
                    CAST(0 AS DECIMAL(18,2)), 
                    CAST(Amount AS DECIMAL(18,2)), 
                    'Payment' 
                FROM CustomerPayments 
                WHERE CustomerId = @Id

                UNION ALL

                SELECT 
                    Id, 
                    ReturnDate, 
                    N'مرتجع مبيعات', 
                    CAST(0 AS DECIMAL(18,2)), 
                    CAST(TotalRefundAmount AS DECIMAL(18,2)), 
                    'Return' 
                FROM SalesReturns 
                WHERE CustomerId = @Id AND TotalRefundAmount > 0

                ORDER BY TransDate ASC, TransType ASC, RefId ASC;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = customerId;
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        // ======================= استيراد من CSV =======================
        // بيقرا الأعمدة من العناوين (الاسم/الهاتف/المنطقة/الملحوظة/المديونية)، ولو العناوين مش معروفة
        // بيفترض الترتيب القديم: الاسم، الهاتف، الرصيد، المنطقة، الملحوظة.
        // بيتخطى المكرر (نفس الاسم + الهاتف في نفس الفرع). بيرجّع عدد المضاف.
        public int ImportCustomersFromCsv(string filePath, int locationId, out int skipped)
        {
            skipped = 0;
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
            if (lines.Length < 2) return 0;

            string[] header = SplitCsvLine(lines[0].TrimStart('\uFEFF'));
            int iName = -1, iPhone = -1, iBal = -1, iRegion = -1, iNotes = -1;

            for (int i = 0; i < header.Length; i++)
            {
                string h = header[i].Trim().ToLowerInvariant();
                if (h == "الاسم" || h == "اسم العميل" || h == "name") iName = i;
                else if (h == "الهاتف" || h == "الموبايل" || h == "رقم الهاتف" || h == "phone") iPhone = i;
                else if (h == "المديونية" || h == "الرصيد" || h == "الرصيد الافتتاحي" || h == "balance") iBal = i;
                else if (h == "المنطقة" || h == "region") iRegion = i;
                else if (h == "الملحوظة" || h == "ملاحظات" || h == "الملاحظات" || h == "notes") iNotes = i;
            }
            if (iName < 0) { iName = 0; iPhone = 1; iBal = 2; iRegion = 3; iNotes = 4; }

            int added = 0;

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        HashSet<string> existing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        using (SqlCommand cmdEx = new SqlCommand(
                            "SELECT Name, Phone FROM Customers WHERE IsActive = 1 AND LocationId = @Loc;", connection, transaction))
                        {
                            cmdEx.Parameters.Add("@Loc", SqlDbType.Int).Value = locationId;
                            using (SqlDataReader r = cmdEx.ExecuteReader())
                            {
                                while (r.Read())
                                    existing.Add(DuplicateKey(ReadString(r, 0), ReadString(r, 1)));
                            }
                        }

                        const string query = @"INSERT INTO Customers (Name, Phone, Balance, Region, Notes, IsActive, LocationId) 
                                               VALUES (@Name, @Phone, @Balance, @Region, @Notes, 1, @LocationId);";

                        // أمر واحد بيتكرر لكل سطر (بدل ما نبني SqlCommand جديد لكل صف)
                        using (SqlCommand insert = new SqlCommand(query, connection, transaction))
                        {
                            SqlParameter pName = insert.Parameters.Add("@Name", SqlDbType.NVarChar, 150);
                            SqlParameter pPhone = insert.Parameters.Add("@Phone", SqlDbType.NVarChar, 50);
                            SqlParameter pBal = AddMoney(insert, "@Balance", 0m);
                            SqlParameter pRegion = insert.Parameters.Add("@Region", SqlDbType.NVarChar, 150);
                            SqlParameter pNotes = insert.Parameters.Add("@Notes", SqlDbType.NVarChar, 500);
                            insert.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;

                            for (int i = 1; i < lines.Length; i++)
                            {
                                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                                string[] v = SplitCsvLine(lines[i]);

                                string name = Truncate(Cell(v, iName), 150);
                                if (name.Length == 0) { skipped++; continue; }

                                string phone = Truncate(Cell(v, iPhone), 50);
                                string key = DuplicateKey(name, phone);
                                if (existing.Contains(key)) { skipped++; continue; }

                                decimal balance = 0;
                                string balText = Cell(v, iBal);
                                if (balText.Length > 0 && !TryParseAmount(balText, out balance))
                                    throw new FormatException("السطر " + (i + 1) + ": الرصيد \"" + balText + "\" غير صحيح.");

                                pName.Value = name;
                                pPhone.Value = ToDb(phone);
                                pBal.Value = Math.Round(balance, 2, MidpointRounding.AwayFromZero);
                                pRegion.Value = ToDb(Truncate(Cell(v, iRegion), 150));
                                pNotes.Value = ToDb(Truncate(Cell(v, iNotes), 500));
                                insert.ExecuteNonQuery();

                                existing.Add(key);
                                added++;
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        SafeRollback(transaction);
                        throw;
                    }
                }
            }
            return added;
        }

        private static string DuplicateKey(string name, string phone)
        {
            return (name ?? "").Trim() + "|" + (phone ?? "").Trim();
        }

        private static string Cell(string[] values, int index)
        {
            return (index >= 0 && index < values.Length) ? values[index].Trim() : "";
        }

        private static string[] SplitCsvLine(string line)
        {
            List<string> result = new List<string>();
            StringBuilder sb = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"') { sb.Append('"'); i++; }
                        else inQuotes = false;
                    }
                    else sb.Append(c);
                }
                else
                {
                    if (c == '"') inQuotes = true;
                    else if (c == ',') { result.Add(sb.ToString()); sb.Clear(); }
                    else sb.Append(c);
                }
            }
            result.Add(sb.ToString());
            return result.ToArray();
        }
    }
}