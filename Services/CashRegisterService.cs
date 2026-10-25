using MajesticDesktop.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks; // ضرورية للـ Async

namespace MajesticDesktop.Services
{
    public class DailyStats
    {
        public decimal TransferredStockValue { get; set; }
        public decimal MainSales { get; set; }
        public decimal InstaPayKurdi { get; set; }
        public decimal InstaPayHakosh { get; set; }
        public decimal DigitalPayments { get; set; }
        public decimal MainExpenses { get; set; }
        public decimal MainAdvances { get; set; }
        public decimal CustomerDebts { get; set; }
        public decimal NetProfit { get; set; }
        public decimal NetCash { get; set; }
        public decimal TotalPurchases { get; set; }
        public decimal TotalReturns { get; set; }
        public decimal BranchReceipts { get; set; }
        public decimal PendingBranchReceipts { get; set; }
    }

    public class CashRegisterService
    {
        public const int MainLocationId = 1;

        private static readonly object _schemaLock = new object();
        private static bool _schemaChecked;

        private sealed class CloseSnapshot
        {
            public decimal MainSales;
            public decimal MainExpenses;
            public decimal NetCash;
            public decimal PendingBranchReceipts;
        }

        public CashRegisterService()
        {
            EnsureSchema();
        }

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
                        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'CashTransactions') AND name = 'IsClosed')
                            ALTER TABLE CashTransactions ADD IsClosed BIT NOT NULL DEFAULT 0;

                        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Sales') AND name = 'IsShiftClosed')
                            ALTER TABLE Sales ADD IsShiftClosed BIT NOT NULL DEFAULT 0;

                        IF OBJECT_ID(N'SalesReturns', N'U') IS NOT NULL
                           AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'SalesReturns') AND name = 'IsShiftClosed')
                            ALTER TABLE SalesReturns ADD IsShiftClosed BIT NOT NULL DEFAULT 0;

                        IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'CashTransactions') AND name = 'PaymentMethod')
                            ALTER TABLE CashTransactions ADD PaymentMethod NVARCHAR(50) DEFAULT N'نقدي' WITH VALUES;

                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='MonthlyTreasury' AND xtype='U')
                        BEGIN
                            CREATE TABLE MonthlyTreasury (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                ClosingDate DATETIME NOT NULL DEFAULT GETDATE(),
                                MainSales DECIMAL(18,2) NOT NULL DEFAULT 0,
                                BranchSales DECIMAL(18,2) NOT NULL DEFAULT 0,
                                MainExpenses DECIMAL(18,2) NOT NULL DEFAULT 0,
                                BranchExpenses DECIMAL(18,2) NOT NULL DEFAULT 0,
                                NetTotal DECIMAL(18,2) NOT NULL DEFAULT 0
                            );
                        END";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 60;
                        command.ExecuteNonQuery();
                    }
                }
                _schemaChecked = true;
            }
        }

        private const string CashOnlyPredicate =
            "ISNULL(PaymentMethod, N'نقدي') IN (N'نقدي', N'كاش') " +
            "AND ISNULL(Description, N'') NOT LIKE N'%انستا%' " +
            "AND ISNULL(Description, N'') NOT LIKE N'%فودافون كاش%'";

        private const string ExpenseTypes =
            "N'مصروف', N'مصروفات محل', N'مصروفات بوفيه', N'مرتبات', N'بنزين سيارة', N'نثريات', N'صيانة', N'إيجار', N'مصروفات أخرى', N'أخرى'";

        private const string PendingBranchSql = @"
            DECLARE @LastHQClose DATETIME = ISNULL((SELECT MAX(ClosingDate) FROM MonthlyTreasury {LOCK}), '2000-01-01');
            DECLARE @PendingBranchReceipts DECIMAL(18,2) = 0;
            IF OBJECT_ID(N'BranchMonthlyTreasury', N'U') IS NOT NULL
                SELECT @PendingBranchReceipts = ISNULL(SUM(Debit), 0)
                FROM BranchMonthlyTreasury {LOCK}
                WHERE Note = N'ترحيل صافي درج' AND TransactionDate > @LastHQClose;
        ";

        private const string CashAggregatesSql = @"
            DECLARE @MainExpenses DECIMAL(18,2) = 0;
            DECLARE @MainAdvances DECIMAL(18,2) = 0;
            DECLARE @TotalPurchases DECIMAL(18,2) = 0;
            DECLARE @CashNet DECIMAL(18,2) = 0;

            SELECT
                @MainExpenses = ISNULL(SUM(CASE WHEN TransactionType IN (" + ExpenseTypes + @") THEN Amount ELSE 0 END), 0),
                @MainAdvances = ISNULL(SUM(CASE WHEN TransactionType IN (N'سلف', N'سلفة') THEN Amount ELSE 0 END), 0),
                @TotalPurchases = ISNULL(SUM(CASE WHEN TransactionType NOT IN (N'وارد', N'إيراد')
                                                   AND (Description LIKE N'%مشتريات%' OR Description LIKE N'%مورد%') THEN Amount ELSE 0 END), 0),
                @CashNet = ISNULL(SUM(CASE WHEN " + CashOnlyPredicate + @"
                                           THEN (CASE WHEN TransactionType IN (N'وارد', N'إيراد') THEN Amount ELSE -Amount END)
                                           ELSE 0 END), 0)
            FROM CashTransactions {LOCK}
            WHERE LocationId = @Loc AND IsClosed = 0;

            DECLARE @NetCash DECIMAL(18,2) = @CashNet + @PendingBranchReceipts;
        ";

        private static readonly string DailyStatsQuery = (@"
            DECLARE @Today DATE = CAST(GETDATE() AS DATE);
            DECLARE @Tomorrow DATE = DATEADD(DAY, 1, @Today);
            DECLARE @TransferredStockValue DECIMAL(18,2) = 0;
            DECLARE @MainSales DECIMAL(18,2) = 0;
            DECLARE @InstaKurdi DECIMAL(18,2) = 0;
            DECLARE @InstaHakosh DECIMAL(18,2) = 0;
            DECLARE @CustomerDebts DECIMAL(18,2) = 0;
            DECLARE @NetProfit DECIMAL(18,2) = 0;
            DECLARE @TotalCost DECIMAL(18,2) = 0;
            DECLARE @TotalSalesForProfit DECIMAL(18,2) = 0;
            DECLARE @ReturnedProfit DECIMAL(18,2) = 0;
            DECLARE @TotalReturns DECIMAL(18,2) = 0;
            DECLARE @BranchReceipts DECIMAL(18,2) = 0;
            " + PendingBranchSql + @"

            IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'StockTransfers')
            BEGIN
                SELECT @TransferredStockValue = ISNULL(SUM(std.Quantity * ISNULL(p.PurchasePrice, 0)), 0)
                FROM StockTransfers st
                INNER JOIN StockTransferDetails std ON st.Id = std.TransferId
                INNER JOIN Products p ON std.ProductId = p.Id
                WHERE st.FromLocationId = @Loc AND st.TransferDate >= @Today AND st.TransferDate < @Tomorrow;
            END

            SELECT
                @MainSales = ISNULL(SUM(CASE WHEN PaymentMethod IS NULL OR PaymentMethod IN (N'نقدي', N'كاش') THEN PaidAmount ELSE 0 END), 0),
                @InstaKurdi = ISNULL(SUM(CASE WHEN PaymentMethod LIKE N'%الكردي%' THEN PaidAmount ELSE 0 END), 0),
                @InstaHakosh = ISNULL(SUM(CASE WHEN PaymentMethod LIKE N'%الحكش%' THEN PaidAmount ELSE 0 END), 0),
                @TotalSalesForProfit = ISNULL(SUM(NetAmount), 0)
            FROM Sales
            WHERE LocationId = @Loc AND IsShiftClosed = 0;

            IF COL_LENGTH('CustomerPayments', 'PaymentMethod') IS NOT NULL AND COL_LENGTH('CustomerPayments', 'LocationId') IS NOT NULL
            BEGIN
                DECLARE @PayKurdi DECIMAL(18,2) = 0, @PayHakosh DECIMAL(18,2) = 0;
                EXEC sp_executesql N'
                    SELECT @k = ISNULL(SUM(CASE WHEN PaymentMethod LIKE N''%الكردي%'' THEN Amount ELSE 0 END), 0),
                           @h = ISNULL(SUM(CASE WHEN PaymentMethod LIKE N''%الحكش%'' THEN Amount ELSE 0 END), 0)
                    FROM CustomerPayments
                    WHERE LocationId = @loc AND PaymentDate > @since',
                    N'@k DECIMAL(18,2) OUTPUT, @h DECIMAL(18,2) OUTPUT, @loc INT, @since DATETIME',
                    @k = @PayKurdi OUTPUT, @h = @PayHakosh OUTPUT, @loc = @Loc, @since = @LastHQClose;
                SET @InstaKurdi = @InstaKurdi + @PayKurdi;
                SET @InstaHakosh = @InstaHakosh + @PayHakosh;
            END

            " + CashAggregatesSql + @"

            SELECT @CustomerDebts = ISNULL(SUM(
                       ISNULL(c.Balance, 0) + ISNULL(sl.Remaining, 0) - ISNULL(rt.Refund, 0) - ISNULL(py.Paid, 0)), 0)
            FROM Customers c
            LEFT JOIN (SELECT CustomerId, SUM(RemainingAmount) AS Remaining FROM Sales WHERE RemainingAmount > 0 GROUP BY CustomerId) sl ON sl.CustomerId = c.Id
            LEFT JOIN (SELECT CustomerId, SUM(TotalRefundAmount) AS Refund FROM SalesReturns GROUP BY CustomerId) rt ON rt.CustomerId = c.Id
            LEFT JOIN (SELECT CustomerId, SUM(Amount) AS Paid FROM CustomerPayments GROUP BY CustomerId) py ON py.CustomerId = c.Id
            WHERE c.IsActive = 1 AND c.LocationId = @Loc;

            SELECT @TotalCost = ISNULL(SUM(sd.Quantity * ISNULL(p.PurchasePrice, 0)), 0)
            FROM SaleDetails sd INNER JOIN Sales s ON sd.SaleId = s.Id INNER JOIN Products p ON sd.ProductId = p.Id
            WHERE s.LocationId = @Loc AND s.IsShiftClosed = 0;

            IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'SalesReturnDetails')
            BEGIN
                SELECT @ReturnedProfit = ISNULL(SUM((ISNULL(srd.UnitPrice, 0) - ISNULL(p.PurchasePrice, 0)) * ISNULL(srd.ReturnedQuantity, 0)), 0)
                FROM SalesReturnDetails srd
                INNER JOIN SalesReturns sr ON srd.ReturnId = sr.Id
                INNER JOIN Products p ON srd.ProductId = p.Id
                WHERE sr.LocationId = @Loc AND sr.IsShiftClosed = 0;
            END

            SET @NetProfit = @TotalSalesForProfit - @TotalCost - @ReturnedProfit - @MainExpenses;

            SELECT @TotalReturns = ISNULL(SUM(TotalRefundAmount), 0) FROM SalesReturns WHERE LocationId = @Loc AND IsShiftClosed = 0;

            IF OBJECT_ID(N'BranchMonthlyTreasury', N'U') IS NOT NULL
                SELECT @BranchReceipts = ISNULL(SUM(Debit), 0)
                FROM BranchMonthlyTreasury
                WHERE Note = N'ترحيل صافي درج' AND TransactionDate >= @Today AND TransactionDate < @Tomorrow;

            SELECT @TransferredStockValue AS TransferredVal, @MainSales AS Sales,
                   @InstaKurdi AS InstaKurdi, @InstaHakosh AS InstaHakosh,
                   @MainExpenses AS Expenses, @MainAdvances AS Advances,
                   @CustomerDebts AS Debts, @NetProfit AS Profit, @NetCash AS NetCash,
                   @TotalPurchases AS Purchases, @TotalReturns AS Returns, @BranchReceipts AS Receipts,
                   @PendingBranchReceipts AS PendingBranchReceipts;").Replace("{LOCK}", "");

        private static readonly string CloseSnapshotQuery = (PendingBranchSql + @"
            DECLARE @MainSales DECIMAL(18,2) = 0;
            SELECT @MainSales = ISNULL(SUM(CASE WHEN PaymentMethod IS NULL OR PaymentMethod IN (N'نقدي', N'كاش') THEN PaidAmount ELSE 0 END), 0)
            FROM Sales {LOCK}
            WHERE LocationId = @Loc AND IsShiftClosed = 0;
            " + CashAggregatesSql + @"
            SELECT @MainSales AS Sales, @MainExpenses AS Expenses, @NetCash AS NetCash, @PendingBranchReceipts AS Pending;")
            .Replace("{LOCK}", "WITH (UPDLOCK, HOLDLOCK)");

        private static readonly string PendingQuery = @"
            DECLARE @LastHQClose DATETIME = ISNULL((SELECT MAX(ClosingDate) FROM MonthlyTreasury), '2000-01-01');
            DECLARE @t TABLE (
                Id INT, TransactionDate DATETIME, LocationName NVARCHAR(100),
                TransactionType NVARCHAR(200), Amount DECIMAL(18,2), Description NVARCHAR(MAX));

            INSERT INTO @t
            SELECT Id, TransactionDate, N'المخزن الرئيسي', TransactionType, Amount, Description
            FROM CashTransactions
            WHERE IsClosed = 0 AND LocationId = @Loc AND " + CashOnlyPredicate + @";

            IF OBJECT_ID(N'BranchMonthlyTreasury', N'U') IS NOT NULL
                INSERT INTO @t
                SELECT Id, TransactionDate, N'توريد من فرع', N'وارد', Debit, N'توريد نقدية - تقفيل وردية فرع'
                FROM BranchMonthlyTreasury
                WHERE Note = N'ترحيل صافي درج' AND TransactionDate > @LastHQClose;

            SELECT Id, TransactionDate, LocationName, TransactionType, Amount, Description
            FROM @t
            ORDER BY TransactionDate DESC, Id DESC;";

        // ======================= أدوات ADO.NET (Async) =======================
        private static SqlParameter AddMoney(SqlParameterCollection ps, string name, decimal value)
        {
            SqlParameter p = ps.Add(name, SqlDbType.Decimal);
            p.Precision = 18;
            p.Scale = 2;
            p.Value = Math.Round(value, 2, MidpointRounding.AwayFromZero);
            return p;
        }

        private static decimal Dec(SqlDataReader r, string column)
        {
            int o = r.GetOrdinal(column);
            return r.IsDBNull(o) ? 0m : Convert.ToDecimal(r.GetValue(o));
        }

        private static void SafeRollback(SqlTransaction transaction)
        {
            try { transaction.Rollback(); } catch (Exception) { }
        }

        private static async Task<int> ExecAsync(SqlConnection c, SqlTransaction t, string sql, Action<SqlParameterCollection> bind)
        {
            using (SqlCommand cmd = new SqlCommand(sql, c, t))
            {
                cmd.CommandTimeout = 30;
                bind?.Invoke(cmd.Parameters);
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        private static async Task InsertCashAsync(SqlConnection c, SqlTransaction t, string type, decimal amount, string description)
        {
            const string sql = @"
                INSERT INTO CashTransactions (TransactionDate, TransactionType, Amount, Description, LocationId, IsClosed, PaymentMethod)
                VALUES (GETDATE(), @Type, @Amount, @Desc, @Loc, 0, N'نقدي');";

            await ExecAsync(c, t, sql, ps =>
            {
                ps.Add("@Type", SqlDbType.NVarChar, 50).Value = type;
                AddMoney(ps, "@Amount", amount);
                ps.Add("@Desc", SqlDbType.NVarChar, 255).Value = description;
                ps.Add("@Loc", SqlDbType.Int).Value = MainLocationId;
            });
        }

        // ======================= القراءة (Async) =======================
        public async Task<DailyStats> GetDailyStatsAsync()
        {
            DailyStats stats = new DailyStats();
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(DailyStatsQuery, connection))
                {
                    command.CommandTimeout = 60;
                    command.Parameters.Add("@Loc", SqlDbType.Int).Value = MainLocationId;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            stats.TransferredStockValue = Dec(reader, "TransferredVal");
                            stats.MainSales = Dec(reader, "Sales");
                            stats.InstaPayKurdi = Dec(reader, "InstaKurdi");
                            stats.InstaPayHakosh = Dec(reader, "InstaHakosh");
                            stats.DigitalPayments = stats.InstaPayKurdi + stats.InstaPayHakosh;
                            stats.MainExpenses = Dec(reader, "Expenses");
                            stats.MainAdvances = Dec(reader, "Advances");
                            stats.CustomerDebts = Dec(reader, "Debts");
                            stats.NetProfit = Dec(reader, "Profit");
                            stats.NetCash = Dec(reader, "NetCash");
                            stats.TotalPurchases = Dec(reader, "Purchases");
                            stats.TotalReturns = Dec(reader, "Returns");
                            stats.BranchReceipts = Dec(reader, "Receipts");
                            stats.PendingBranchReceipts = Dec(reader, "PendingBranchReceipts");
                        }
                    }
                }
            }
            return stats;
        }

        public async Task<DataTable> GetPendingTransactionsAsync()
        {
            DataTable dt = new DataTable("PendingTransactions");
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(PendingQuery, connection))
                {
                    command.CommandTimeout = 60;
                    command.Parameters.Add("@Loc", SqlDbType.Int).Value = MainLocationId;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        dt.Load(reader); // DataReader is much faster and cleaner in Async context
                    }
                }
            }
            return dt;
        }

        private async Task<CloseSnapshot> ReadCloseSnapshotAsync(SqlConnection connection, SqlTransaction transaction)
        {
            CloseSnapshot s = new CloseSnapshot();
            using (SqlCommand command = new SqlCommand(CloseSnapshotQuery, connection, transaction))
            {
                command.CommandTimeout = 30;
                command.Parameters.Add("@Loc", SqlDbType.Int).Value = MainLocationId;

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        s.MainSales = Dec(reader, "Sales");
                        s.MainExpenses = Dec(reader, "Expenses");
                        s.NetCash = Dec(reader, "NetCash");
                        s.PendingBranchReceipts = Dec(reader, "Pending");
                    }
                }
            }
            return s;
        }

        // ======================= تقفيل الوردية (Async) =======================
        public async Task CloseDailyRegisterAsync(DailyStats stats, decimal actualCash)
        {
            if (stats == null) throw new ArgumentNullException(nameof(stats));

            actualCash = Math.Round(actualCash, 2, MidpointRounding.AwayFromZero);
            if (actualCash < 0) throw new InvalidOperationException("المبلغ المرحّل لا يمكن أن يكون سالبًا.");

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                await connection.OpenAsync();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        CloseSnapshot fresh = await ReadCloseSnapshotAsync(connection, transaction);

                        if (fresh.NetCash != stats.NetCash)
                            throw new InvalidOperationException("تغيّرت أرصدة الخزنة منذ فتح الشاشة (تمت حركات جديدة).\nبرجاء الضغط على تحديث البيانات ثم إعادة المحاولة.");

                        decimal expected = fresh.NetCash;

                        if (expected < 0 && actualCash != 0)
                            throw new InvalidOperationException("الخزنة في عجز، ولا يمكن ترحيل مبلغ منها. يجب أن يكون المبلغ المرحّل صفرًا.");

                        decimal difference = actualCash - expected;

                        if (expected >= 0 && difference > 0)
                            await InsertCashAsync(connection, transaction, "وارد", difference, "تسوية زيادة تقفيل وردية");

                        const string insertArchiveQuery = @"
                            INSERT INTO MonthlyTreasury (ClosingDate, MainSales, BranchSales, MainExpenses, BranchExpenses, NetTotal)
                            VALUES (GETDATE(), @MainSales, @BranchSales, @MainExpenses, 0, @NetTotal);";

                        await ExecAsync(connection, transaction, insertArchiveQuery, ps =>
                        {
                            AddMoney(ps, "@MainSales", fresh.MainSales);
                            AddMoney(ps, "@BranchSales", fresh.PendingBranchReceipts);
                            AddMoney(ps, "@MainExpenses", fresh.MainExpenses);
                            AddMoney(ps, "@NetTotal", actualCash);
                        });

                        await ExecAsync(connection, transaction, "UPDATE CashTransactions SET IsClosed = 1 WHERE IsClosed = 0 AND LocationId = @Loc;",
                                        ps => ps.Add("@Loc", SqlDbType.Int).Value = MainLocationId);

                        await ExecAsync(connection, transaction, "UPDATE Sales SET IsShiftClosed = 1 WHERE IsShiftClosed = 0 AND LocationId = @Loc;",
                                        ps => ps.Add("@Loc", SqlDbType.Int).Value = MainLocationId);

                        await ExecAsync(connection, transaction, "IF OBJECT_ID(N'SalesReturns', N'U') IS NOT NULL UPDATE SalesReturns SET IsShiftClosed = 1 WHERE IsShiftClosed = 0 AND LocationId = @Loc;",
                                        ps => ps.Add("@Loc", SqlDbType.Int).Value = MainLocationId);

                        if (expected < 0)
                            await InsertCashAsync(connection, transaction, "صادر", Math.Abs(expected), "عجز مرحّل من الوردية السابقة");
                        else if (difference < 0)
                            await InsertCashAsync(connection, transaction, "وارد", Math.Abs(difference), "رصيد متبقي بالدرج لم يتم ترحيله - مرحل للوردية القادمة");

                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        SafeRollback(transaction);
                        if (ex.Number == 1205) throw new InvalidOperationException("حصل تعارض مع عملية تانية على الداتابيز أثناء التقفيل ولم يتم تنفيذه.\nاضغط تحديث البيانات وحاول تاني.", ex);
                        if (ex.Number == -2) throw new InvalidOperationException("الداتابيز مشغولة بعملية تانية (انتهت مهلة الانتظار) ولم يتم التقفيل.\nحاول تاني بعد لحظات.", ex);
                        throw;
                    }
                    catch
                    {
                        SafeRollback(transaction);
                        throw;
                    }
                }
            }
        }
    }
}