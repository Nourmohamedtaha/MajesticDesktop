using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace MajesticDesktop.Services
{
    public class TreasuryStats
    {
        public decimal TotalBalance { get; set; }
        public decimal TotalCash { get; set; }
        public decimal InstaKurdi { get; set; }
        public decimal InstaHakosh { get; set; }
        public decimal TotalSales { get; set; }
        public decimal NetProfit { get; set; }
        public decimal TotalDebts { get; set; }
        public decimal HQManualIn { get; set; }
        public decimal HQManualOut { get; set; }
    }

    public class MainTreasuryService
    {
        private const int HQ_LOCATION_ID = 0;

        private static bool _schemaChecked;
        private static readonly object _schemaLock = new object();

        public MainTreasuryService() { }

        private static void EnsureSchema()
        {
            if (_schemaChecked) return;

            lock (_schemaLock)
            {
                if (_schemaChecked) return;

                using (SqlConnection connection = DbConnection.CreateConnection())
                {
                    connection.Open();
                    const string query = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TreasuryMonthlyArchive' AND xtype='U')
                        BEGIN
                            CREATE TABLE TreasuryMonthlyArchive (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                ArchiveMonth NVARCHAR(20) NOT NULL,
                                ClosingDate DATETIME NOT NULL DEFAULT GETDATE(),
                                TotalBalance DECIMAL(18,2) NOT NULL DEFAULT 0,
                                TotalCash DECIMAL(18,2) NOT NULL DEFAULT 0,
                                TotalDigital DECIMAL(18,2) NOT NULL DEFAULT 0,
                                TotalMainSales DECIMAL(18,2) NOT NULL DEFAULT 0,
                                TotalBranchSales DECIMAL(18,2) NOT NULL DEFAULT 0,
                                HQManualIn DECIMAL(18,2) NOT NULL DEFAULT 0,
                                HQManualOut DECIMAL(18,2) NOT NULL DEFAULT 0,
                                MainWarehouseTransfers DECIMAL(18,2) NOT NULL DEFAULT 0,
                                TotalProfit DECIMAL(18,2) NOT NULL DEFAULT 0,
                                TotalDebts DECIMAL(18,2) NOT NULL DEFAULT 0,
                                TotalPurchases DECIMAL(18,2) NOT NULL DEFAULT 0
                            );
                        END

                        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'CashTransactions') AND name = 'PaymentMethod')
                        BEGIN
                            ALTER TABLE CashTransactions ADD PaymentMethod NVARCHAR(50) DEFAULT N'نقدي' WITH VALUES;
                        END

                        IF COL_LENGTH('TreasuryMonthlyArchive', 'InstaKurdi') IS NULL ALTER TABLE TreasuryMonthlyArchive ADD InstaKurdi DECIMAL(18,2) NOT NULL DEFAULT 0;
                        IF COL_LENGTH('TreasuryMonthlyArchive', 'InstaHakosh') IS NULL ALTER TABLE TreasuryMonthlyArchive ADD InstaHakosh DECIMAL(18,2) NOT NULL DEFAULT 0;
                        IF COL_LENGTH('TreasuryMonthlyArchive', 'TotalSales') IS NULL ALTER TABLE TreasuryMonthlyArchive ADD TotalSales DECIMAL(18,2) NOT NULL DEFAULT 0;

                        IF COL_LENGTH('Sales', 'IsShiftClosed') IS NULL ALTER TABLE Sales ADD IsShiftClosed BIT NOT NULL DEFAULT 0;
                        IF COL_LENGTH('Sales', 'IsMonthlyClosed') IS NULL ALTER TABLE Sales ADD IsMonthlyClosed BIT NOT NULL DEFAULT 0;

                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_CashTransactions_Location_Date' AND object_id = OBJECT_ID(N'CashTransactions'))
                            EXEC(N'CREATE INDEX IX_CashTransactions_Location_Date ON CashTransactions (LocationId, TransactionDate)');

                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_CashTransactions_Location_Type' AND object_id = OBJECT_ID(N'CashTransactions'))
                            EXEC(N'CREATE INDEX IX_CashTransactions_Location_Type ON CashTransactions (LocationId, TransactionType)');

                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Sales_ShiftClosed_Date' AND object_id = OBJECT_ID(N'Sales'))
                            EXEC(N'CREATE INDEX IX_Sales_ShiftClosed_Date ON Sales (IsShiftClosed, InvoiceDate)');

                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_SaleDetails_SaleId' AND object_id = OBJECT_ID(N'SaleDetails'))
                            EXEC(N'CREATE INDEX IX_SaleDetails_SaleId ON SaleDetails (SaleId)');";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 120;
                        command.ExecuteNonQuery();
                    }
                }

                _schemaChecked = true;
            }
        }

        private TreasuryStats ReadTreasuryStats(SqlConnection connection, SqlTransaction transaction, DateTime fromDate, DateTime toDate)
        {
            TreasuryStats stats = new TreasuryStats();

            const string query = @"
                DECLARE @TotalCash DECIMAL(18,2) = 0;
                DECLARE @InstaKurdi DECIMAL(18,2) = 0;
                DECLARE @InstaHakosh DECIMAL(18,2) = 0;
                DECLARE @TotalSales DECIMAL(18,2) = 0;
                DECLARE @NetProfit DECIMAL(18,2) = 0;
                DECLARE @ReturnedProfit DECIMAL(18,2) = 0;
                DECLARE @TotalExpenses DECIMAL(18,2) = 0;
                DECLARE @TotalDebts DECIMAL(18,2) = 0;
                DECLARE @HQManualIn DECIMAL(18,2) = 0;
                DECLARE @HQManualOut DECIMAL(18,2) = 0;

                DECLARE @LifeTimeMain DECIMAL(18,2) = 0;
                IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'MonthlyTreasury') AND COL_LENGTH('MonthlyTreasury', 'NetTotal') IS NOT NULL
                    EXEC sp_executesql N'SELECT @v = ISNULL(SUM(NetTotal), 0) FROM MonthlyTreasury',
                        N'@v DECIMAL(18,2) OUTPUT', @v = @LifeTimeMain OUTPUT;

                DECLARE @HQCashIn DECIMAL(18,2) = 0;
                DECLARE @HQCashOut DECIMAL(18,2) = 0;
                SELECT @HQCashIn = ISNULL(SUM(Amount), 0) FROM CashTransactions WHERE LocationId = @HQId AND TransactionType IN (N'وارد', N'إيراد') AND (PaymentMethod IS NULL OR PaymentMethod IN (N'نقدي', N'كاش'));
                SELECT @HQCashOut = ISNULL(SUM(Amount), 0) FROM CashTransactions WHERE LocationId = @HQId AND TransactionType IN (N'منصرف', N'صادر') AND (PaymentMethod IS NULL OR PaymentMethod IN (N'نقدي', N'كاش'));
                SET @TotalCash = @LifeTimeMain + @HQCashIn - @HQCashOut;

                DECLARE @SalesKurdi DECIMAL(18,2) = 0, @SalesHakosh DECIMAL(18,2) = 0;
                IF COL_LENGTH('Sales', 'PaymentMethod') IS NOT NULL
                    EXEC sp_executesql N'
                        SELECT @k = ISNULL(SUM(CASE WHEN PaymentMethod LIKE N''%الكردي%'' THEN PaidAmount ELSE 0 END), 0),
                               @h = ISNULL(SUM(CASE WHEN PaymentMethod LIKE N''%الحكش%'' THEN PaidAmount ELSE 0 END), 0)
                        FROM Sales WHERE IsShiftClosed = 1',
                        N'@k DECIMAL(18,2) OUTPUT, @h DECIMAL(18,2) OUTPUT',
                        @k = @SalesKurdi OUTPUT, @h = @SalesHakosh OUTPUT;

                DECLARE @PayKurdi DECIMAL(18,2) = 0, @PayHakosh DECIMAL(18,2) = 0;
                IF COL_LENGTH('CustomerPayments', 'PaymentMethod') IS NOT NULL
                    EXEC sp_executesql N'
                        SELECT @k = ISNULL(SUM(CASE WHEN PaymentMethod LIKE N''%الكردي%'' THEN Amount ELSE 0 END), 0),
                               @h = ISNULL(SUM(CASE WHEN PaymentMethod LIKE N''%الحكش%'' THEN Amount ELSE 0 END), 0)
                        FROM CustomerPayments',
                        N'@k DECIMAL(18,2) OUTPUT, @h DECIMAL(18,2) OUTPUT',
                        @k = @PayKurdi OUTPUT, @h = @PayHakosh OUTPUT;

                DECLARE @ManualInstaKurdiIn DECIMAL(18, 2) = 0, @ManualInstaKurdiOut DECIMAL(18,2) = 0;
                DECLARE @ManualInstaHakoshIn DECIMAL(18, 2) = 0, @ManualInstaHakoshOut DECIMAL(18,2) = 0;

                SELECT @ManualInstaKurdiIn = ISNULL(SUM(Amount), 0) FROM CashTransactions
                WHERE LocationId = @HQId AND TransactionType IN(N'وارد', N'إيراد')
                  AND PaymentMethod LIKE N'%الكردي%' AND ISNULL(Description, '') NOT LIKE N'%تصفية%';

                SELECT @ManualInstaKurdiOut = ISNULL(SUM(Amount), 0) FROM CashTransactions
                WHERE LocationId = @HQId AND TransactionType IN(N'منصرف', N'صادر')
                  AND PaymentMethod LIKE N'%الكردي%' AND ISNULL(Description, '') NOT LIKE N'%تصفية%';

                SELECT @ManualInstaHakoshIn = ISNULL(SUM(Amount), 0) FROM CashTransactions
                WHERE LocationId = @HQId AND TransactionType IN(N'وارد', N'إيراد')
                  AND PaymentMethod LIKE N'%الحكش%' AND ISNULL(Description, '') NOT LIKE N'%تصفية%';

                SELECT @ManualInstaHakoshOut = ISNULL(SUM(Amount), 0) FROM CashTransactions
                WHERE LocationId = @HQId AND TransactionType IN(N'منصرف', N'صادر')
                  AND PaymentMethod LIKE N'%الحكش%' AND ISNULL(Description, '') NOT LIKE N'%تصفية%';

                SET @InstaKurdi = @SalesKurdi + @PayKurdi + @ManualInstaKurdiIn - @ManualInstaKurdiOut;
                SET @InstaHakosh = @SalesHakosh + @PayHakosh + @ManualInstaHakoshIn - @ManualInstaHakoshOut;

                DECLARE @LiqKurdiHQ DECIMAL(18, 2) = (SELECT ISNULL(SUM(Amount), 0) FROM CashTransactions WHERE LocationId = @HQId AND Description LIKE N'%تصفية أرصدة إنستاباي الكردي%');
                DECLARE @LiqHakoshHQ DECIMAL(18, 2) = (SELECT ISNULL(SUM(Amount), 0) FROM CashTransactions WHERE LocationId = @HQId AND Description LIKE N'%تصفية أرصدة إنستاباي الحكش%');

                DECLARE @LiqBranchGeneric DECIMAL(18, 2) = 0;
                IF COL_LENGTH('CashTransactions', 'IsClosed') IS NOT NULL
                    EXEC sp_executesql N'
                        SELECT @v = ISNULL(SUM(Amount), 0) FROM CashTransactions
                        WHERE LocationId <> @hq AND IsClosed = 1 AND Description LIKE N''%تسييل محافظ%''',
                        N'@v DECIMAL(18,2) OUTPUT, @hq INT', @v = @LiqBranchGeneric OUTPUT, @hq = @HQId;

                SET @InstaKurdi = @InstaKurdi - @LiqKurdiHQ;
                SET @InstaHakosh = @InstaHakosh - @LiqHakoshHQ;

                IF @LiqBranchGeneric > 0
                BEGIN
                    IF @InstaKurdi >= @LiqBranchGeneric
                        SET @InstaKurdi = @InstaKurdi - @LiqBranchGeneric;
                    ELSE
                    BEGIN
                        SET @LiqBranchGeneric = @LiqBranchGeneric - @InstaKurdi;
                        SET @InstaKurdi = 0;
                        SET @InstaHakosh = @InstaHakosh - @LiqBranchGeneric;
                    END
                END

                IF @InstaKurdi < 0 SET @InstaKurdi = 0;
                IF @InstaHakosh < 0 SET @InstaHakosh = 0;

                SELECT @TotalSales = ISNULL(SUM(NetAmount), 0) FROM Sales
                WHERE IsShiftClosed = 1 AND InvoiceDate >= @From AND InvoiceDate < DATEADD(DAY, 1, @To);

                SELECT @HQManualIn = ISNULL(SUM(Amount), 0) FROM CashTransactions
                WHERE LocationId = @HQId AND TransactionType IN(N'وارد', N'إيراد')
                  AND TransactionDate >= @From AND TransactionDate < DATEADD(DAY, 1, @To)
                  AND ISNULL(Description, '') NOT LIKE N'%تصفية%';

                SELECT @HQManualOut = ISNULL(SUM(Amount), 0) FROM CashTransactions
                WHERE LocationId = @HQId AND TransactionType IN(N'منصرف', N'صادر')
                  AND TransactionDate >= @From AND TransactionDate < DATEADD(DAY, 1, @To)
                  AND ISNULL(Description, '') NOT LIKE N'%تصفية%';

                SELECT @NetProfit = ISNULL(SUM((ISNULL(sd.UnitPrice, 0) - ISNULL(p.PurchasePrice, 0)) * ISNULL(sd.Quantity, 0)), 0)
                FROM SaleDetails sd INNER JOIN Sales s ON sd.SaleId = s.Id INNER JOIN Products p ON sd.ProductId = p.Id
                WHERE s.IsShiftClosed = 1 AND s.InvoiceDate >= @From AND s.InvoiceDate < DATEADD(DAY, 1, @To);

                IF EXISTS(SELECT 1 FROM sys.tables WHERE name = 'SalesReturnDetails')
                BEGIN
                    EXEC sp_executesql N'
                        SELECT @val = ISNULL(SUM((ISNULL(srd.UnitPrice, 0) - ISNULL(p.PurchasePrice, 0)) * ISNULL(srd.ReturnedQuantity, 0)), 0)
                        FROM SalesReturnDetails srd
                        INNER JOIN SalesReturns sr ON srd.ReturnId = sr.Id
                        INNER JOIN Products p ON srd.ProductId = p.Id
                        WHERE sr.IsShiftClosed = 1 AND sr.ReturnDate >= @pFrom AND sr.ReturnDate < DATEADD(DAY, 1, @pTo)',
                        N'@val DECIMAL(18,2) OUTPUT, @pFrom DATE, @pTo DATE',
                        @val = @ReturnedProfit OUTPUT, @pFrom = @From, @pTo = @To;
                END
                SET @NetProfit = @NetProfit - @ReturnedProfit;

                DECLARE @ExpFilter NVARCHAR(100) = N'1 = 1';
                IF COL_LENGTH('CashTransactions', 'IsClosed') IS NOT NULL AND COL_LENGTH('CashTransactions', 'IsMonthlyClosed') IS NOT NULL
                    SET @ExpFilter = N'(IsClosed = 1 OR IsMonthlyClosed = 1)';
                ELSE IF COL_LENGTH('CashTransactions', 'IsClosed') IS NOT NULL
                    SET @ExpFilter = N'IsClosed = 1';
                ELSE IF COL_LENGTH('CashTransactions', 'IsMonthlyClosed') IS NOT NULL
                    SET @ExpFilter = N'IsMonthlyClosed = 1';

                DECLARE @ExpSql NVARCHAR(MAX) = N'
                    SELECT @e = ISNULL(SUM(Amount), 0) FROM CashTransactions
                    WHERE TransactionType IN(N''مصروف'', N''مصروفات محل'', N''مصروفات بوفيه'', N''مرتبات'', N''بنزين سيارة'', N''نثريات'', N''صيانة'', N''إيجار'', N''مصروفات أخرى'', N''أخرى'')
                      AND ' + @ExpFilter + N'
                      AND TransactionDate >= @pFrom AND TransactionDate < DATEADD(DAY, 1, @pTo)';
                EXEC sp_executesql @ExpSql, N'@e DECIMAL(18,2) OUTPUT, @pFrom DATE, @pTo DATE',
                    @e = @TotalExpenses OUTPUT, @pFrom = @From, @pTo = @To;

                SET @NetProfit = @NetProfit - @TotalExpenses;

                IF EXISTS(SELECT 1 FROM sys.tables WHERE name = 'Customers')
                BEGIN
                    SELECT @TotalDebts = ISNULL(SUM(CurrentBalance), 0)
                    FROM(
                        SELECT
                            (ISNULL(c.Balance, 0)
                            + ISNULL((SELECT SUM(RemainingAmount) FROM Sales WHERE CustomerId = c.Id AND RemainingAmount > 0), 0)
                            - ISNULL((SELECT SUM(TotalRefundAmount) FROM SalesReturns WHERE CustomerId = c.Id), 0)
                            - ISNULL((SELECT SUM(Amount) FROM CustomerPayments WHERE CustomerId = c.Id), 0)) AS CurrentBalance
                        FROM Customers c
                        WHERE c.IsActive = 1
                    ) AS GlobalDebts;
                END

                SELECT(@TotalCash + @InstaKurdi + @InstaHakosh) AS Balance, @TotalCash AS Cash, @InstaKurdi AS InstaKurdi, @InstaHakosh AS InstaHakosh,
                       @TotalSales AS TotalSales, @NetProfit AS NetProfit, @TotalDebts AS Debts, @HQManualIn AS HQIn, @HQManualOut AS HQOut; ";

            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.CommandTimeout = 120;
                command.Parameters.Add("@HQId", SqlDbType.Int).Value = HQ_LOCATION_ID;
                command.Parameters.Add("@From", SqlDbType.Date).Value = fromDate.Date;
                command.Parameters.Add("@To", SqlDbType.Date).Value = toDate.Date;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        stats.TotalBalance = Convert.ToDecimal(reader["Balance"]);
                        stats.TotalCash = Convert.ToDecimal(reader["Cash"]);
                        stats.InstaKurdi = Convert.ToDecimal(reader["InstaKurdi"]);
                        stats.InstaHakosh = Convert.ToDecimal(reader["InstaHakosh"]);
                        stats.TotalSales = Convert.ToDecimal(reader["TotalSales"]);
                        stats.NetProfit = Convert.ToDecimal(reader["NetProfit"]);
                        stats.TotalDebts = Convert.ToDecimal(reader["Debts"]);
                        stats.HQManualIn = Convert.ToDecimal(reader["HQIn"]);
                        stats.HQManualOut = Convert.ToDecimal(reader["HQOut"]);
                    }
                }
            }
            return stats;
        }

        public TreasuryStats GetTreasuryStats(DateTime fromDate, DateTime toDate)
        {
            EnsureSchema();
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                return ReadTreasuryStats(connection, null, fromDate, toDate);
            }
        }

        public DataTable GetTransactions(DateTime fromDate, DateTime toDate)
        {
            EnsureSchema();
            DataTable dt = new DataTable();
            const string query = @"
                SELECT Id, TransactionDate, TransactionType, Amount, PaymentMethod, Description, ReferenceNumber 
                FROM CashTransactions 
                WHERE LocationId = @HQId 
                  AND TransactionDate >= @From
                  AND TransactionDate < DATEADD(DAY, 1, @To)
                ORDER BY TransactionDate DESC, Id DESC;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@HQId", SqlDbType.Int).Value = HQ_LOCATION_ID;
                command.Parameters.Add("@From", SqlDbType.Date).Value = fromDate.Date;
                command.Parameters.Add("@To", SqlDbType.Date).Value = toDate.Date;

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public void AddTransaction(CashTransaction transaction, string paymentMethod = "نقدي")
        {
            EnsureSchema();
            const string query = @"
                INSERT INTO CashTransactions (LocationId, TransactionDate, TransactionType, Amount, Description, ReferenceNumber, PaymentMethod)
                VALUES (@HQId, @TransactionDate, @TransactionType, @Amount, @Description, @ReferenceNumber, @PaymentMethod);";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@HQId", SqlDbType.Int).Value = HQ_LOCATION_ID;
                command.Parameters.Add("@TransactionDate", SqlDbType.DateTime).Value = transaction.TransactionDate;
                command.Parameters.Add("@TransactionType", SqlDbType.NVarChar, 50).Value = transaction.TransactionType;
                command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = transaction.Amount;
                command.Parameters.Add("@Description", SqlDbType.NVarChar, 255).Value = (object)transaction.Description ?? DBNull.Value;
                command.Parameters.Add("@ReferenceNumber", SqlDbType.NVarChar, 50).Value = (object)transaction.ReferenceNumber ?? DBNull.Value;
                command.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar, 50).Value = paymentMethod;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public bool CloseMonthlyTreasury(TreasuryStats stats, out string errorMessage)
        {
            errorMessage = "";
            EnsureSchema();

            string currentMonthYear = DateTime.Now.ToString("MM-yyyy", CultureInfo.InvariantCulture);
            string closingRef = "MONTH-CLOSE-" + currentMonthYear;
            DateTime monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string checkQuery = "SELECT COUNT(1) FROM TreasuryMonthlyArchive WHERE ArchiveMonth = @Month;";
                        using (SqlCommand cmdCheck = new SqlCommand(checkQuery, connection, transaction))
                        {
                            cmdCheck.Parameters.Add("@Month", SqlDbType.NVarChar, 20).Value = currentMonthYear;
                            if (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0)
                            {
                                errorMessage = "تم تقفيل الخزنة وتصفيرها مسبقاً وأرشفتها لهذا الشهر!";
                                transaction.Rollback();
                                return false;
                            }
                        }

                        TreasuryStats fresh = ReadTreasuryStats(connection, transaction, monthStart, DateTime.Now);

                        if (Math.Abs(fresh.TotalCash - stats.TotalCash) > 0.009m
                            || Math.Abs(fresh.InstaKurdi - stats.InstaKurdi) > 0.009m
                            || Math.Abs(fresh.InstaHakosh - stats.InstaHakosh) > 0.009m
                            || Math.Abs(fresh.TotalSales - stats.TotalSales) > 0.009m
                            || Math.Abs(fresh.NetProfit - stats.NetProfit) > 0.009m
                            || Math.Abs(fresh.HQManualIn - stats.HQManualIn) > 0.009m
                            || Math.Abs(fresh.HQManualOut - stats.HQManualOut) > 0.009m)
                        {
                            throw new Exception("اتسجلت حركات جديدة في الخزنة بعد ما الشاشة اتحدّثت.\nاضغط بحث/تحديث وأعد المحاولة.");
                        }

                        const string insertArchiveQuery = @"
                            INSERT INTO TreasuryMonthlyArchive 
                            (ArchiveMonth, TotalBalance, TotalCash, InstaKurdi, InstaHakosh, TotalSales, HQManualIn, HQManualOut, TotalProfit)
                            VALUES (@Month, @Bal, @Cash, @IK, @IH, @Sales, @In, @Out, @Profit);";

                        using (SqlCommand cmdArc = new SqlCommand(insertArchiveQuery, connection, transaction))
                        {
                            cmdArc.Parameters.Add("@Month", SqlDbType.NVarChar, 20).Value = currentMonthYear;
                            cmdArc.Parameters.Add("@Bal", SqlDbType.Decimal).Value = fresh.TotalBalance;
                            cmdArc.Parameters.Add("@Cash", SqlDbType.Decimal).Value = fresh.TotalCash;
                            cmdArc.Parameters.Add("@IK", SqlDbType.Decimal).Value = fresh.InstaKurdi;
                            cmdArc.Parameters.Add("@IH", SqlDbType.Decimal).Value = fresh.InstaHakosh;
                            cmdArc.Parameters.Add("@Sales", SqlDbType.Decimal).Value = fresh.TotalSales;
                            cmdArc.Parameters.Add("@In", SqlDbType.Decimal).Value = fresh.HQManualIn;
                            cmdArc.Parameters.Add("@Out", SqlDbType.Decimal).Value = fresh.HQManualOut;
                            cmdArc.Parameters.Add("@Profit", SqlDbType.Decimal).Value = fresh.NetProfit;
                            cmdArc.ExecuteNonQuery();
                        }

                        if (fresh.TotalCash > 0)
                            InsertHqWithdrawal(connection, transaction, fresh.TotalCash, "تصفية وسحب الكاش النقدي من الخزينة عن شهر " + currentMonthYear, closingRef, "نقدي");
                        if (fresh.InstaKurdi > 0)
                            InsertHqWithdrawal(connection, transaction, fresh.InstaKurdi, "تصفية أرصدة إنستاباي الكردي عن شهر " + currentMonthYear, closingRef, "انستاباي الكردي");
                        if (fresh.InstaHakosh > 0)
                            InsertHqWithdrawal(connection, transaction, fresh.InstaHakosh, "تصفية أرصدة إنستاباي الحكش عن شهر " + currentMonthYear, closingRef, "انستاباي الحكش");

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        try { transaction.Rollback(); } catch { }
                        errorMessage = ex.Message;
                        return false;
                    }
                }
            }
        }

        private static void InsertHqWithdrawal(SqlConnection connection, SqlTransaction transaction, decimal amount, string description, string refNumber, string paymentMethod)
        {
            const string query = @"INSERT INTO CashTransactions (LocationId, TransactionDate, TransactionType, Amount, Description, ReferenceNumber, PaymentMethod)
                                   VALUES (@HQId, GETDATE(), N'منصرف', @Amount, @Desc, @RefNum, @PayMethod);";
            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.Add("@HQId", SqlDbType.Int).Value = HQ_LOCATION_ID;
                cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = amount;
                cmd.Parameters.Add("@Desc", SqlDbType.NVarChar, 255).Value = description;
                cmd.Parameters.Add("@RefNum", SqlDbType.NVarChar, 50).Value = refNumber;
                cmd.Parameters.Add("@PayMethod", SqlDbType.NVarChar, 50).Value = paymentMethod;
                cmd.ExecuteNonQuery();
            }
        }
    }
}