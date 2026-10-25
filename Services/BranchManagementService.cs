using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MajesticDesktop.Database;
using MajesticDesktop.Models;

namespace MajesticDesktop.Services
{
    public class ProductLinkedToOtherBranchException : Exception
    {
        public int ProductId { get; }
        public string OtherBranchName { get; }

        public ProductLinkedToOtherBranchException(int productId, string otherBranchName)
            : base("المنتج \"" + productId + "\" مسجل بالفعل كمنتج خاص بفرع \"" + otherBranchName + "\".\nلو أكملت، المنتج هيتحول لمنتج عام ويظهر في كل الفروع.")
        {
            ProductId = productId;
            OtherBranchName = otherBranchName;
        }
    }

    public class BranchShiftStats
    {
        public decimal CashSales { get; set; }
        public decimal Returns { get; set; }
        public decimal Expenses { get; set; }
        public decimal Advances { get; set; }
        public decimal OwedToUs { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal NetProfit => GrossProfit - Expenses;
        public decimal ExpectedCash { get; set; }
        public decimal NetCash { get; set; }
        public decimal InstaPayKurdi { get; set; }
        public decimal InstaPayHakosh { get; set; }
        public decimal TotalSales { get; set; }
        public decimal CashierDeficit { get; set; }
    }

    public class BranchManagementService
    {
        private const string CashOnlyFilter = @"
                      AND ISNULL(PaymentMethod, N'نقدي') IN (N'نقدي', N'كاش')
                      AND ISNULL(TransactionType, N'') NOT LIKE N'%انستا%'
                      AND ISNULL(TransactionType, N'') NOT LIKE N'%إنستا%'
                      AND ISNULL(TransactionType, N'') NOT LIKE N'%فودافون%'
                      AND ISNULL(TransactionType, N'') NOT LIKE N'%محفظ%'
                      AND ISNULL(TransactionType, N'') NOT LIKE N'%الكردي%'
                      AND ISNULL(TransactionType, N'') NOT LIKE N'%الحكش%'
                      AND NOT (ISNULL(Description, N'') LIKE N'إيراد مبيعات%'
                               AND (Description LIKE N'%انستا%' OR Description LIKE N'%فودافون%' OR Description LIKE N'%محفظة%'))";

        private static readonly string[] MonthNames =
        {
            "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو",
            "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر"
        };

        // ===================== Helpers =====================
        private static void AddDecimalParameter(SqlCommand cmd, string name, decimal value)
        {
            SqlParameter p = cmd.Parameters.Add(name, SqlDbType.Decimal);
            p.Precision = 18;
            p.Scale = 2;
            p.Value = value;
        }

        private static bool ColumnExists(SqlConnection con, SqlTransaction trans, string table, string column)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT CASE WHEN COL_LENGTH(@Table, @Column) IS NULL THEN 0 ELSE 1 END", con, trans))
            {
                cmd.Parameters.Add("@Table", SqlDbType.NVarChar, 128).Value = table;
                cmd.Parameters.Add("@Column", SqlDbType.NVarChar, 128).Value = column;
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }

        private static bool TableExists(SqlConnection con, string table)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT CASE WHEN OBJECT_ID(@Table, 'U') IS NULL THEN 0 ELSE 1 END", con))
            {
                cmd.Parameters.Add("@Table", SqlDbType.NVarChar, 128).Value = table;
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }

        private static int GetBranchProductsCategoryId(SqlConnection con, SqlTransaction trans)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Id FROM Categories WHERE Name = N'منتجات الفرع' ORDER BY Id", con, trans))
            {
                object result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
            }
        }

        private static string GetBranchName(SqlConnection con, SqlTransaction trans, int branchId)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT Name FROM Locations WHERE Id = @Id", con, trans))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = branchId;
                object result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? result.ToString() : ("فرع رقم " + branchId);
            }
        }

        private static decimal GetCurrentExpectedCash(SqlConnection con, SqlTransaction trans, int branchId)
        {
            string query = @"
                SELECT ISNULL(SUM(CASE WHEN TransactionType IN (N'وارد', N'مبيعات') THEN Amount ELSE -Amount END), 0)
                FROM CashTransactions
                WHERE ISNULL(LocationId, 1) = @BranchID AND ISNULL(IsClosed, 0) = 0 " + CashOnlyFilter;

            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                return Convert.ToDecimal(cmd.ExecuteScalar());
            }
        }

        private static decimal GetCashierDeficit(SqlConnection con, SqlTransaction trans, int branchId)
        {
            const string query = @"
                SELECT ISNULL(SUM(CASE WHEN Description = N'عجز درج' THEN Amount ELSE 0 END), 0)
                     - ISNULL(SUM(CASE WHEN Description = N'سداد عجز وردية سابق' THEN Amount ELSE 0 END), 0)
                FROM CashTransactions
                WHERE ISNULL(LocationId, 1) = @BranchID";

            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                decimal deficit = Convert.ToDecimal(cmd.ExecuteScalar());
                return deficit < 0 ? 0 : deficit;
            }
        }

        private static decimal GetBranchOwedToUs(SqlConnection con, SqlTransaction trans, int branchId)
        {
            // اكتشاف اسم عمود المعرف في جدول العملاء ديناميكياً
            string cId = ColumnExists(con, trans, "Customers", "CustomerId") ? "CustomerId" : "Id";

            string query = $@"
                SELECT ISNULL(SUM(
                    ISNULL(c.Balance, 0)
                    + ISNULL((SELECT SUM(RemainingAmount) FROM Sales WHERE CustomerId = c.[{cId}] AND RemainingAmount > 0), 0)
                    - ISNULL((SELECT SUM(TotalRefundAmount) FROM SalesReturns WHERE CustomerId = c.[{cId}]), 0)
                    - ISNULL((SELECT SUM(Amount) FROM CustomerPayments WHERE CustomerId = c.[{cId}]), 0)
                ), 0)
                FROM Customers c
                WHERE c.IsActive = 1 AND c.LocationId = @BranchID";

            using (SqlCommand cmd = new SqlCommand(query, con, trans))
            {
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                object result = cmd.ExecuteScalar();
                return result != null && result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        public DataTable GetActiveCustomersForBranch(int branchId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = DbConnection.CreateConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM Customers WHERE ISNULL(IsActive, 1) = 1 AND LocationId = @BranchID ORDER BY Name", con))
            {
                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { da.Fill(dt); }
            }
            return dt;
        }

        // ===================== Schema =====================
        public void EnsureSchemaUpdated()
        {
            string[] statements =
            {
                "IF COL_LENGTH(N'CashTransactions', N'PaymentMethod') IS NULL ALTER TABLE CashTransactions ADD PaymentMethod NVARCHAR(100) NULL;",
                "IF COL_LENGTH(N'CashTransactions', N'IsClosed') IS NULL ALTER TABLE CashTransactions ADD IsClosed BIT NOT NULL DEFAULT 0;",
                "IF COL_LENGTH(N'CashTransactions', N'IsMonthlyClosed') IS NULL ALTER TABLE CashTransactions ADD IsMonthlyClosed BIT NOT NULL DEFAULT 0;",

                "IF COL_LENGTH(N'Sales', N'PaymentMethod') IS NULL ALTER TABLE Sales ADD PaymentMethod NVARCHAR(100) NULL;",
                "IF COL_LENGTH(N'Sales', N'IsShiftClosed') IS NULL ALTER TABLE Sales ADD IsShiftClosed BIT NOT NULL DEFAULT 0;",
                "IF COL_LENGTH(N'Sales', N'IsMonthlyClosed') IS NULL ALTER TABLE Sales ADD IsMonthlyClosed BIT NOT NULL DEFAULT 0;",

                @"IF OBJECT_ID(N'Returns', N'U') IS NULL
                  CREATE TABLE [Returns] (
                      Id INT IDENTITY(1,1) PRIMARY KEY,
                      CustomerID INT NULL,
                      LocationId INT NULL,
                      TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
                      ReturnDate DATETIME NOT NULL DEFAULT GETDATE(),
                      IsShiftClosed BIT NOT NULL DEFAULT 0,
                      IsMonthlyClosed BIT NOT NULL DEFAULT 0
                  );",
                "IF COL_LENGTH(N'Returns', N'LocationId') IS NULL ALTER TABLE [Returns] ADD LocationId INT NULL;",
                "IF COL_LENGTH(N'Returns', N'IsShiftClosed') IS NULL ALTER TABLE [Returns] ADD IsShiftClosed BIT NOT NULL DEFAULT 0;",
                "IF COL_LENGTH(N'Returns', N'IsMonthlyClosed') IS NULL ALTER TABLE [Returns] ADD IsMonthlyClosed BIT NOT NULL DEFAULT 0;",

                @"IF OBJECT_ID(N'ReturnDetails', N'U') IS NULL
                  CREATE TABLE ReturnDetails (
                      Id INT IDENTITY(1,1) PRIMARY KEY,
                      ReturnID INT NOT NULL,
                      ProductID INT NOT NULL,
                      Quantity DECIMAL(18,2) NOT NULL DEFAULT 0,
                      UnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
                      TotalPrice DECIMAL(18,2) NOT NULL DEFAULT 0
                  );",

                @"IF OBJECT_ID(N'BranchProducts', N'U') IS NULL
                  CREATE TABLE BranchProducts (
                      BranchID INT NOT NULL,
                      ProductID INT NOT NULL,
                      PurchasePrice DECIMAL(18,2) NOT NULL DEFAULT 0,
                      CONSTRAINT PK_BranchProducts PRIMARY KEY (BranchID, ProductID)
                  );",

                @"IF OBJECT_ID(N'BranchMonthlyTreasury', N'U') IS NULL
                  CREATE TABLE BranchMonthlyTreasury (
                      Id INT IDENTITY(1,1) PRIMARY KEY,
                      TransactionDate DATETIME NOT NULL DEFAULT GETDATE(),
                      Debit DECIMAL(18,2) NOT NULL DEFAULT 0,
                      Credit DECIMAL(18,2) NOT NULL DEFAULT 0,
                      Note NVARCHAR(500) NULL,
                      BranchID INT NOT NULL
                  );",
                "IF COL_LENGTH(N'BranchMonthlyTreasury', N'ArchiveYear') IS NULL ALTER TABLE BranchMonthlyTreasury ADD ArchiveYear INT NULL;",
                "IF COL_LENGTH(N'BranchMonthlyTreasury', N'ArchiveMonth') IS NULL ALTER TABLE BranchMonthlyTreasury ADD ArchiveMonth INT NULL;"
            };

            List<string> errors = new List<string>();

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                con.Open();

                foreach (string sql in statements)
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex.Message);
                    }
                }

                try
                {
                    if (ColumnExists(con, null, "Returns", "BranchID") && ColumnExists(con, null, "Returns", "LocationId"))
                    {
                        using (SqlCommand cmd = new SqlCommand("UPDATE [Returns] SET LocationId = BranchID WHERE LocationId IS NULL AND BranchID IS NOT NULL;", con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }
            }

            if (errors.Count > 0)
                throw new Exception("تعذر تحديث بنية بعض الجداول:\n" + string.Join("\n", errors));
        }

        // ===================== الفروع =====================
        public DataTable GetAllBranches()
        {
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                // إخفاء الفرع الرئيسي من شاشة إدارة الفروع (Id <> 1)
                using (SqlCommand cmd = new SqlCommand("SELECT Id AS BranchID, Name AS BranchName, ISNULL(Type, N'فرع بيع') AS BranchType, ISNULL(IsActive, 1) AS IsActive FROM Locations WHERE Id <> 1 ORDER BY Id", con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public void AddBranch(string name, string type)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("اسم الفرع غير صالح.");
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                con.Open();
                using (SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(1) FROM Locations WHERE Name = @Name", con))
                {
                    cmdCheck.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = name.Trim();
                    if (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0) throw new Exception("الفرع موجود بالفعل.");
                }
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Locations (Name, Type, IsActive) VALUES (@Name, @Type, 1)", con))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = name.Trim();
                    cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(type) ? "فرع بيع" : type.Trim();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateBranch(int id, string name, string type)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(name)) throw new ArgumentException("بيانات غير صالحة.");
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                con.Open();

                using (SqlCommand cmdCheck = new SqlCommand("SELECT COUNT(1) FROM Locations WHERE Name = @Name AND Id <> @Id", con))
                {
                    cmdCheck.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = name.Trim();
                    cmdCheck.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    if (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0)
                        throw new Exception("يوجد فرع آخر بنفس الاسم بالفعل.");
                }

                using (SqlCommand cmd = new SqlCommand("UPDATE Locations SET Name = @Name, Type = @Type WHERE Id = @Id", con))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = name.Trim();
                    cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 100).Value = string.IsNullOrWhiteSpace(type) ? "فرع بيع" : type.Trim();
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ===================== أرصدة المنتجات =====================
        public DataTable GetBranchProducts(int branchId)
        {
            if (branchId <= 0) throw new ArgumentException("رقم الفرع غير صحيح.");
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                string query = @"
                    SELECT p.Id AS ProductID, p.Name AS ProductName,
                           ISNULL(bp.PurchasePrice, ISNULL(p.PurchasePrice, 0)) AS PurchasePrice,
                           ISNULL(s.Quantity, 0) AS StockQuantity
                    FROM Products p
                    LEFT JOIN BranchProducts bp ON p.Id = bp.ProductID AND bp.BranchID = @BranchID
                    LEFT JOIN Stock s ON p.Id = s.ProductId AND s.LocationId = @BranchID
                    WHERE (p.BranchId IS NULL OR p.BranchId = @BranchID)
                      AND ISNULL(p.IsActive, 1) = 1
                    ORDER BY p.Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public void AddBranchSpecificProduct(int branchId, string productName, decimal purchasePrice, decimal addedQty, bool confirmMakeGlobal = false)
        {
            if (branchId <= 0 || string.IsNullOrWhiteSpace(productName) || addedQty < 0) throw new ArgumentException("بيانات المدخلات غير صالحة.");
            productName = productName.Trim();
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                con.Open();
                using (SqlTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        int productId = 0;
                        object currentBranchIdObj = null;

                        using (SqlCommand cmdCheck = new SqlCommand("SELECT Id, BranchId FROM Products WHERE Name = @Name", con, trans))
                        {
                            cmdCheck.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = productName;
                            using (SqlDataReader reader = cmdCheck.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    productId = Convert.ToInt32(reader["Id"]);
                                    currentBranchIdObj = reader["BranchId"];
                                }
                            }
                        }

                        if (productId > 0)
                        {
                            bool belongsToOtherBranch = currentBranchIdObj != DBNull.Value && Convert.ToInt32(currentBranchIdObj) != branchId;
                            if (belongsToOtherBranch)
                            {
                                if (!confirmMakeGlobal)
                                {
                                    string otherBranchName = GetBranchName(con, trans, Convert.ToInt32(currentBranchIdObj));
                                    throw new ProductLinkedToOtherBranchException(productId, otherBranchName);
                                }

                                using (SqlCommand cmdMakeGlobal = new SqlCommand("UPDATE Products SET BranchId = NULL WHERE Id = @Id", con, trans))
                                {
                                    cmdMakeGlobal.Parameters.Add("@Id", SqlDbType.Int).Value = productId;
                                    cmdMakeGlobal.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            int categoryId = GetBranchProductsCategoryId(con, trans);

                            using (SqlCommand cmd = new SqlCommand("INSERT INTO Products (Name, CategoryId, IsActive, AlertLimit, PurchasePrice, BranchId) VALUES (@Name, @CategoryId, 1, 5, @Purchase, @BranchID); SELECT CAST(SCOPE_IDENTITY() AS INT);", con, trans))
                            {
                                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 200).Value = productName;
                                cmd.Parameters.Add("@CategoryId", SqlDbType.Int).Value = categoryId;
                                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                AddDecimalParameter(cmd, "@Purchase", purchasePrice);
                                productId = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                        }

                        int existsBp;
                        using (SqlCommand cmdCheckBp = new SqlCommand("SELECT COUNT(1) FROM BranchProducts WHERE BranchID = @BranchID AND ProductID = @ProdID", con, trans))
                        {
                            cmdCheckBp.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                            cmdCheckBp.Parameters.Add("@ProdID", SqlDbType.Int).Value = productId;
                            existsBp = Convert.ToInt32(cmdCheckBp.ExecuteScalar());
                        }

                        if (existsBp == 0)
                        {
                            using (SqlCommand cmdInsBp = new SqlCommand("INSERT INTO BranchProducts (BranchID, ProductID, PurchasePrice) VALUES (@BranchID, @ProdID, @Purchase)", con, trans))
                            {
                                cmdInsBp.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                cmdInsBp.Parameters.Add("@ProdID", SqlDbType.Int).Value = productId;
                                AddDecimalParameter(cmdInsBp, "@Purchase", purchasePrice);
                                cmdInsBp.ExecuteNonQuery();
                            }
                        }

                        if (addedQty > 0)
                        {
                            using (SqlCommand cmdUpdInv = new SqlCommand("UPDATE Stock SET Quantity = ISNULL(Quantity, 0) + @Qty WHERE ProductId = @ProdID AND LocationId = @BranchID", con, trans))
                            {
                                AddDecimalParameter(cmdUpdInv, "@Qty", addedQty);
                                cmdUpdInv.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                cmdUpdInv.Parameters.Add("@ProdID", SqlDbType.Int).Value = productId;
                                if (cmdUpdInv.ExecuteNonQuery() == 0)
                                {
                                    using (SqlCommand cmdInsInv = new SqlCommand("INSERT INTO Stock (ProductId, LocationId, Quantity) VALUES (@ProdID, @BranchID, @Qty)", con, trans))
                                    {
                                        cmdInsInv.Parameters.Add("@ProdID", SqlDbType.Int).Value = productId;
                                        cmdInsInv.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                        AddDecimalParameter(cmdInsInv, "@Qty", addedQty);
                                        cmdInsInv.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        trans.Commit();
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        public void SaveBranchProductCustomPricing(int branchId, int productId, decimal purchasePrice, decimal qty)
        {
            SaveBranchProductCustomPricingBatch(branchId, new List<Tuple<int, decimal, decimal>> { Tuple.Create(productId, purchasePrice, qty) });
        }

        public void SaveBranchProductCustomPricingBatch(int branchId, List<Tuple<int, decimal, decimal>> items)
        {
            if (branchId <= 0) throw new ArgumentException("رقم الفرع غير صحيح.");
            if (items == null || items.Count == 0) return;

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                con.Open();
                using (SqlTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in items)
                        {
                            int productId = item.Item1;
                            decimal purchasePrice = item.Item2;
                            decimal qty = item.Item3;
                            if (productId <= 0 || purchasePrice < 0 || qty < 0)
                                throw new ArgumentException("بيانات غير صالحة لأحد الأصناف.");

                            int existsBp;
                            using (SqlCommand cmdCheckBp = new SqlCommand("SELECT COUNT(1) FROM BranchProducts WHERE BranchID = @BranchID AND ProductID = @ProdID", con, trans))
                            {
                                cmdCheckBp.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                cmdCheckBp.Parameters.Add("@ProdID", SqlDbType.Int).Value = productId;
                                existsBp = Convert.ToInt32(cmdCheckBp.ExecuteScalar());
                            }

                            if (existsBp > 0)
                            {
                                using (SqlCommand cmdUpd = new SqlCommand("UPDATE BranchProducts SET PurchasePrice = @Purchase WHERE BranchID = @BranchID AND ProductID = @ProdID", con, trans))
                                {
                                    AddDecimalParameter(cmdUpd, "@Purchase", purchasePrice);
                                    cmdUpd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                    cmdUpd.Parameters.Add("@ProdID", SqlDbType.Int).Value = productId;
                                    cmdUpd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                using (SqlCommand cmdIns = new SqlCommand("INSERT INTO BranchProducts (BranchID, ProductID, PurchasePrice) VALUES (@BranchID, @ProdID, @Purchase)", con, trans))
                                {
                                    cmdIns.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                    cmdIns.Parameters.Add("@ProdID", SqlDbType.Int).Value = productId;
                                    AddDecimalParameter(cmdIns, "@Purchase", purchasePrice);
                                    cmdIns.ExecuteNonQuery();
                                }
                            }

                            using (SqlCommand cmdUpdInv = new SqlCommand("UPDATE Stock SET Quantity = @Qty WHERE LocationId = @BranchID AND ProductId = @ProdID", con, trans))
                            {
                                AddDecimalParameter(cmdUpdInv, "@Qty", qty);
                                cmdUpdInv.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                cmdUpdInv.Parameters.Add("@ProdID", SqlDbType.Int).Value = productId;
                                if (cmdUpdInv.ExecuteNonQuery() == 0)
                                {
                                    using (SqlCommand cmdInsInv = new SqlCommand("INSERT INTO Stock (ProductId, LocationId, Quantity) VALUES (@ProdID, @BranchID, @Qty)", con, trans))
                                    {
                                        cmdInsInv.Parameters.Add("@ProdID", SqlDbType.Int).Value = productId;
                                        cmdInsInv.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                        AddDecimalParameter(cmdInsInv, "@Qty", qty);
                                        cmdInsInv.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        trans.Commit();
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        // ===================== السلف والمصروفات =====================
        public void SaveBranchExpense(int branchId, DataTable expenses)
        {
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                con.Open();
                using (SqlTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        string insertExpense = "INSERT INTO CashTransactions (TransactionDate, TransactionType, Amount, Description, LocationId, IsClosed, PaymentMethod) VALUES (GETDATE(), @ExpType, @Amount, @Note, @BranchID, 0, N'نقدي')";
                        foreach (DataRow row in expenses.Rows)
                        {
                            using (SqlCommand cmd = new SqlCommand(insertExpense, con, trans))
                            {
                                cmd.Parameters.Add("@ExpType", SqlDbType.NVarChar, 200).Value = row["Type"];
                                AddDecimalParameter(cmd, "@Amount", Convert.ToDecimal(row["Amount"]));
                                cmd.Parameters.Add("@Note", SqlDbType.NVarChar, 500).Value = row["Notes"];
                                cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        trans.Commit();
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        public DataTable GetBranchExpensesHistory(int branchId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                string query = @"
                    SELECT TransactionDate, TransactionType, Amount, Description 
                    FROM CashTransactions 
                    WHERE ISNULL(LocationId, 1) = @BranchID 
                      AND TransactionType IN (N'سلفة', N'سلف', N'مصروف', N'مصروفات محل', N'مرتبات', N'بنزين سيارة', N'أخرى')
                    ORDER BY TransactionDate DESC";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { da.Fill(dt); }
                }
            }
            return dt;
        }

        // ===================== المرتجعات =====================
        public DataTable GetBranchReturnsHistory(int branchId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                string query = @"
                    SELECT r.ReturnDate, ISNULL(c.Name, N'عميل نقدي') AS CustomerName, r.TotalAmount 
                    FROM [Returns] r
                    LEFT JOIN Customers c ON r.CustomerID = c.Id
                    WHERE ISNULL(r.LocationId, 1) = @BranchID
                    ORDER BY r.ReturnDate DESC";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { da.Fill(dt); }
                }
            }
            return dt;
        }

        public void SaveBranchReturn(int branchId, int customerId, decimal totalAmount, bool isCashRefund, List<SaleDetailModel> returnedItems)
        {
            if (branchId <= 0 || returnedItems == null || returnedItems.Count == 0 || totalAmount <= 0) throw new ArgumentException("بيانات المرتجع غير صالحة.");
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                con.Open();
                using (SqlTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        int returnId;

                        bool hasBranchIdColumn = ColumnExists(con, trans, "Returns", "BranchID");

                        string insertReturnQuery = hasBranchIdColumn
                            ? @"INSERT INTO [Returns] (CustomerID, BranchID, LocationId, TotalAmount, ReturnDate, IsShiftClosed, IsMonthlyClosed)
                                VALUES (@CustID, @BranchID, @BranchID, @Total, GETDATE(), 0, 0);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);"
                            : @"INSERT INTO [Returns] (CustomerID, LocationId, TotalAmount, ReturnDate, IsShiftClosed, IsMonthlyClosed)
                                VALUES (@CustID, @BranchID, @Total, GETDATE(), 0, 0);
                                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        using (SqlCommand cmd = new SqlCommand(insertReturnQuery, con, trans))
                        {
                            cmd.Parameters.Add("@CustID", SqlDbType.Int).Value = customerId > 0 ? (object)customerId : DBNull.Value;
                            cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                            AddDecimalParameter(cmd, "@Total", totalAmount);
                            returnId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        foreach (var item in returnedItems)
                        {
                            using (SqlCommand cmdDet = new SqlCommand("INSERT INTO ReturnDetails (ReturnID, ProductID, Quantity, UnitPrice, TotalPrice) VALUES (@RetID, @ProdID, @Qty, @Price, @Total)", con, trans))
                            {
                                cmdDet.Parameters.Add("@RetID", SqlDbType.Int).Value = returnId;
                                cmdDet.Parameters.Add("@ProdID", SqlDbType.Int).Value = item.ProductID;
                                AddDecimalParameter(cmdDet, "@Qty", item.Quantity);
                                AddDecimalParameter(cmdDet, "@Price", item.UnitPrice);
                                AddDecimalParameter(cmdDet, "@Total", item.Quantity * item.UnitPrice);
                                cmdDet.ExecuteNonQuery();
                            }

                            using (SqlCommand cmdInv = new SqlCommand("UPDATE Stock SET Quantity = ISNULL(Quantity, 0) + @Qty WHERE ProductId = @ProdID AND LocationId = @BranchID", con, trans))
                            {
                                AddDecimalParameter(cmdInv, "@Qty", item.Quantity);
                                cmdInv.Parameters.Add("@ProdID", SqlDbType.Int).Value = item.ProductID;
                                cmdInv.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                if (cmdInv.ExecuteNonQuery() == 0)
                                {
                                    using (SqlCommand cmdInsInv = new SqlCommand("INSERT INTO Stock (ProductId, LocationId, Quantity) VALUES (@ProdID, @BranchID, @Qty)", con, trans))
                                    {
                                        cmdInsInv.Parameters.Add("@ProdID", SqlDbType.Int).Value = item.ProductID;
                                        cmdInsInv.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                        AddDecimalParameter(cmdInsInv, "@Qty", item.Quantity);
                                        cmdInsInv.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        if (isCashRefund)
                        {
                            using (SqlCommand cmdTreasury = new SqlCommand("INSERT INTO CashTransactions (TransactionDate, TransactionType, Amount, Description, LocationId, IsClosed, PaymentMethod) VALUES (GETDATE(), N'صادر', @Amount, @Note, @BranchID, 0, N'نقدي')", con, trans))
                            {
                                AddDecimalParameter(cmdTreasury, "@Amount", totalAmount);
                                cmdTreasury.Parameters.Add("@Note", SqlDbType.NVarChar, 500).Value = "سداد قيمة مرتجع نقدي - مرتجع رقم " + returnId;
                                cmdTreasury.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                cmdTreasury.ExecuteNonQuery();
                            }
                        }
                        else if (customerId > 0)
                        {
                            using (SqlCommand cmdDebt = new SqlCommand("UPDATE Customers SET Balance = ISNULL(Balance, 0) - @Total WHERE Id = @CustID", con, trans))
                            {
                                AddDecimalParameter(cmdDebt, "@Total", totalAmount);
                                cmdDebt.Parameters.Add("@CustID", SqlDbType.Int).Value = customerId;
                                cmdDebt.ExecuteNonQuery();
                            }
                        }

                        trans.Commit();
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        // ===================== إحصائيات الوردية =====================
        public BranchShiftStats GetBranchDailyShiftStats(int branchId)
        {
            return GetShiftStats(branchId, false);
        }

        public BranchShiftStats GetBranchMonthlyShiftStats(int branchId)
        {
            return GetShiftStats(branchId, true);
        }

        private BranchShiftStats GetShiftStats(int branchId, bool monthly)
        {
            BranchShiftStats stats = new BranchShiftStats();

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                con.Open();

                // الكود الديناصوري الذكي: استكشاف أسماء الأعمدة في قاعدة بياناتك عشان نتفادى خطأ Invalid Column Name للأبد
                string sId = ColumnExists(con, null, "Sales", "SaleId") ? "SaleId" : (ColumnExists(con, null, "Sales", "InvoiceId") ? "InvoiceId" : "Id");
                string pId = ColumnExists(con, null, "Products", "ProductId") ? "ProductId" : "Id";
                string rId = ColumnExists(con, null, "Returns", "ReturnId") ? "ReturnId" : "Id";

                string ctScope = monthly
                    ? "ISNULL(IsClosed, 0) = 1 AND ISNULL(IsMonthlyClosed, 0) = 0"
                    : "ISNULL(IsClosed, 0) = 0";

                string retScope = monthly
                    ? "ISNULL(IsShiftClosed, 0) = 1 AND ISNULL(IsMonthlyClosed, 0) = 0"
                    : "ISNULL(IsShiftClosed, 0) = 0";

                string salesScope = retScope;
                string salesScopeAliased = salesScope.Replace("IsShiftClosed", "s.IsShiftClosed").Replace("IsMonthlyClosed", "s.IsMonthlyClosed");
                string retScopeAliased = retScope.Replace("IsShiftClosed", "r.IsShiftClosed").Replace("IsMonthlyClosed", "r.IsMonthlyClosed");

                string query = $@"
        DECLARE @CashSales DECIMAL(18,2) = 0, @Ret DECIMAL(18,2) = 0, @Exp DECIMAL(18,2) = 0, @Adv DECIMAL(18,2) = 0,
                @Profit DECIMAL(18,2) = 0, @ReturnedProfit DECIMAL(18,2) = 0, @Expected DECIMAL(18,2) = 0, @NetCash DECIMAL(18,2) = 0,
                @InstaKurdi DECIMAL(18,2) = 0, @InstaHakosh DECIMAL(18,2) = 0, @TotalSales DECIMAL(18,2) = 0,
                @TotalDeficit DECIMAL(18,2) = 0, @TotalPaid DECIMAL(18,2) = 0;

        SELECT
            @Exp = ISNULL(SUM(CASE WHEN TransactionType IN (N'مصروف', N'مصروفات محل', N'مرتبات', N'بنزين سيارة', N'أخرى') THEN Amount ELSE 0 END), 0),
            @Adv = ISNULL(SUM(CASE WHEN TransactionType IN (N'سلفة', N'سلف') THEN Amount ELSE 0 END), 0)
        FROM CashTransactions
        WHERE ISNULL(LocationId, 1) = @BranchID AND {ctScope};

        SELECT @Ret = ISNULL(SUM(TotalAmount), 0)
        FROM [Returns]
        WHERE ISNULL(LocationId, 1) = @BranchID AND {retScope};

        SELECT
            @CashSales = ISNULL(SUM(CASE WHEN PaymentMethod IS NULL OR PaymentMethod IN (N'نقدي', N'كاش') THEN PaidAmount ELSE 0 END), 0),
            @InstaKurdi = ISNULL(SUM(CASE WHEN PaymentMethod LIKE N'%الكردي%' THEN PaidAmount ELSE 0 END), 0),
            @InstaHakosh = ISNULL(SUM(CASE WHEN PaymentMethod LIKE N'%الحكش%' THEN PaidAmount ELSE 0 END), 0),
            @TotalSales = ISNULL(SUM(NetAmount), 0)
        FROM Sales
        WHERE ISNULL(LocationId, 1) = @BranchID AND {salesScope};

        SELECT @Profit = ISNULL(SUM((ISNULL(d.UnitPrice, 0) - ISNULL(bp.PurchasePrice, ISNULL(p.PurchasePrice, 0))) * ISNULL(d.Quantity, 0)), 0)
        FROM SaleDetails d
        INNER JOIN Sales s ON d.SaleId = s.[{sId}]
        INNER JOIN Products p ON d.ProductId = p.[{pId}]
        LEFT JOIN BranchProducts bp ON bp.ProductID = d.ProductId AND bp.BranchID = ISNULL(s.LocationId, 1)
        WHERE ISNULL(s.LocationId, 1) = @BranchID AND {salesScopeAliased};

        SELECT @Profit = @Profit - ISNULL(SUM(Discount), 0)
        FROM Sales
        WHERE ISNULL(LocationId, 1) = @BranchID AND {salesScope};

        SELECT @ReturnedProfit = ISNULL(SUM((ISNULL(rd.UnitPrice, 0) - ISNULL(bp2.PurchasePrice, ISNULL(p2.PurchasePrice, 0))) * ISNULL(rd.Quantity, 0)), 0)
        FROM ReturnDetails rd
        INNER JOIN [Returns] r ON rd.ReturnID = r.[{rId}]
        INNER JOIN Products p2 ON rd.ProductID = p2.[{pId}]
        LEFT JOIN BranchProducts bp2 ON bp2.ProductID = rd.ProductID AND bp2.BranchID = ISNULL(r.LocationId, 1)
        WHERE ISNULL(r.LocationId, 1) = @BranchID AND {retScopeAliased};

        SET @Profit = @Profit - @ReturnedProfit;

        SELECT @TotalDeficit = ISNULL(SUM(Amount), 0)
        FROM CashTransactions
        WHERE ISNULL(LocationId, 1) = @BranchID AND Description = N'عجز درج';

        SELECT @TotalPaid = ISNULL(SUM(Amount), 0)
        FROM CashTransactions
        WHERE ISNULL(LocationId, 1) = @BranchID AND Description = N'سداد عجز وردية سابق';

        SELECT @Expected = ISNULL(SUM(CASE WHEN TransactionType IN (N'وارد', N'مبيعات') THEN Amount ELSE -Amount END), 0)
        FROM CashTransactions
        WHERE ISNULL(LocationId, 1) = @BranchID AND ISNULL(IsClosed, 0) = 0 {CashOnlyFilter};

        SELECT @NetCash = ISNULL(SUM(Debit), 0) - ISNULL(SUM(Credit), 0)
        FROM BranchMonthlyTreasury
        WHERE BranchID = @BranchID AND ArchiveYear IS NULL;

        SELECT @CashSales AS CashSales, @Ret AS Ret, @Exp AS Expenses, @Adv AS Advances, @Profit AS Profit,
               @Expected AS Expected, @NetCash AS NetCash, @InstaKurdi AS InstaKurdi,
               @InstaHakosh AS InstaHakosh, @TotalSales AS TotalSales, @TotalDeficit AS TotalDeficit, @TotalPaid AS TotalPaid;";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stats.CashSales = Convert.ToDecimal(reader["CashSales"]);
                            stats.Returns = Convert.ToDecimal(reader["Ret"]);
                            stats.Expenses = Convert.ToDecimal(reader["Expenses"]);
                            stats.Advances = Convert.ToDecimal(reader["Advances"]);
                            stats.GrossProfit = Convert.ToDecimal(reader["Profit"]);
                            stats.ExpectedCash = Convert.ToDecimal(reader["Expected"]);
                            stats.NetCash = Convert.ToDecimal(reader["NetCash"]);
                            stats.InstaPayKurdi = Convert.ToDecimal(reader["InstaKurdi"]);
                            stats.InstaPayHakosh = Convert.ToDecimal(reader["InstaHakosh"]);
                            stats.TotalSales = Convert.ToDecimal(reader["TotalSales"]);

                            decimal deficit = Convert.ToDecimal(reader["TotalDeficit"]) - Convert.ToDecimal(reader["TotalPaid"]);
                            stats.CashierDeficit = deficit < 0 ? 0 : deficit;
                        }
                    }
                }

                try
                {
                    stats.OwedToUs = GetBranchOwedToUs(con, null, branchId);
                }
                catch
                {
                    stats.OwedToUs = 0;
                }
            }

            return stats;
        }

        // ===================== الوردية اليومية =====================
        public DataTable GetBranchDailyTransactions(int branchId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                string query = $@"
                    SELECT
                        FORMAT(TransactionDate, 'hh:mm tt', 'en-US') AS [وقت الحركة],
                        TransactionType AS [نوع الحركة],
                        CAST(CASE WHEN TransactionType IN (N'وارد', N'مبيعات') THEN Amount ELSE 0 END AS DECIMAL(18,2)) AS [وارد (إيراد)],
                        CAST(CASE WHEN TransactionType IN (N'وارد', N'مبيعات') THEN 0 ELSE Amount END AS DECIMAL(18,2)) AS [صادر (منصرف)],
                        Description AS [البيان]
                    FROM CashTransactions
                    WHERE ISNULL(LocationId, 1) = @BranchID AND ISNULL(IsClosed, 0) = 0 {CashOnlyFilter}
                    ORDER BY TransactionDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { da.Fill(dt); }
                }
            }
            return dt;
        }

        public void CloseBranchShiftWithAudit(int branchId, decimal expected, decimal actual, decimal diff)
        {
            if (branchId <= 0) throw new ArgumentException("الفرع غير صالح.");
            if (actual < 0) throw new ArgumentException("رصيد الدرج الفعلي غير صحيح.");

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                con.Open();
                using (SqlTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        decimal expectedNow = GetCurrentExpectedCash(con, trans, branchId);

                        if (Math.Abs(expectedNow - expected) > 0.009m)
                        {
                            throw new Exception("اتسجلت حركات جديدة على الفرع بعد ما الشاشة اتحدّثت (الدفتري بقى " + expectedNow.ToString("N2") + " ج.م).\nاضغط على تبويب \"الوردية اليومية\" تاني لتحديث الأرقام وأعد الجرد.");
                        }

                        decimal realDiff = actual - expectedNow;

                        if (realDiff != 0)
                        {
                            string type = realDiff < 0 ? "صادر" : "وارد";
                            string note = realDiff < 0 ? "عجز درج" : "زيادة درج";

                            using (SqlCommand cmdAudit = new SqlCommand("INSERT INTO CashTransactions (TransactionDate, TransactionType, Amount, Description, LocationId, IsClosed, PaymentMethod) VALUES (GETDATE(), @Type, @Amount, @Note, @BranchID, 0, N'نقدي')", con, trans))
                            {
                                cmdAudit.Parameters.Add("@Type", SqlDbType.NVarChar, 50).Value = type;
                                AddDecimalParameter(cmdAudit, "@Amount", Math.Abs(realDiff));
                                cmdAudit.Parameters.Add("@Note", SqlDbType.NVarChar, 255).Value = note;
                                cmdAudit.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                cmdAudit.ExecuteNonQuery();
                            }
                        }

                        if (actual > 0)
                        {
                            using (SqlCommand cmdMonthly = new SqlCommand("INSERT INTO BranchMonthlyTreasury (TransactionDate, Debit, Credit, Note, BranchID) VALUES (GETDATE(), @Actual, 0, N'ترحيل صافي درج', @BranchID)", con, trans))
                            {
                                AddDecimalParameter(cmdMonthly, "@Actual", actual);
                                cmdMonthly.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                                cmdMonthly.ExecuteNonQuery();
                            }
                        }

                        using (SqlCommand cmdClose = new SqlCommand("UPDATE CashTransactions SET IsClosed = 1 WHERE ISNULL(LocationId, 1) = @BranchID AND ISNULL(IsClosed, 0) = 0", con, trans))
                        {
                            cmdClose.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                            cmdClose.ExecuteNonQuery();
                        }

                        using (SqlCommand cmdSales = new SqlCommand("UPDATE Sales SET IsShiftClosed = 1 WHERE ISNULL(LocationId, 1) = @BranchID AND ISNULL(IsShiftClosed, 0) = 0", con, trans))
                        {
                            cmdSales.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                            cmdSales.ExecuteNonQuery();
                        }

                        using (SqlCommand cmdReturns = new SqlCommand("UPDATE [Returns] SET IsShiftClosed = 1 WHERE ISNULL(LocationId, 1) = @BranchID AND ISNULL(IsShiftClosed, 0) = 0", con, trans))
                        {
                            cmdReturns.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                            cmdReturns.ExecuteNonQuery();
                        }

                        trans.Commit();
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        public void PayBranchDeficit(int branchId, decimal amount)
        {
            if (branchId <= 0 || amount <= 0) throw new ArgumentException("بيانات السداد غير صالحة.");

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                con.Open();
                using (SqlTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        decimal deficit = GetCashierDeficit(con, trans, branchId);

                        if (amount > deficit + 0.009m)
                            throw new Exception("المبلغ أكبر من العجز المستحق (" + deficit.ToString("N2") + " ج.م).");

                        using (SqlCommand cmd = new SqlCommand("INSERT INTO CashTransactions (TransactionDate, TransactionType, Amount, Description, LocationId, IsClosed, PaymentMethod) VALUES (GETDATE(), N'وارد', @Amount, N'سداد عجز وردية سابق', @BranchID, 0, N'نقدي')", con, trans))
                        {
                            AddDecimalParameter(cmd, "@Amount", amount);
                            cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                            cmd.ExecuteNonQuery();
                        }

                        trans.Commit();
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        // ===================== التقفيل الشهري =====================
        public DataTable GetMonthlyTreasuryLog(int branchId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                const string query = @"
                    SELECT TransactionDate AS [التاريخ],
                           ISNULL(Debit, 0) AS [وارد (ترحيل)],
                           ISNULL(Credit, 0) AS [صادر (توريد)],
                           Note AS [البيان]
                    FROM BranchMonthlyTreasury
                    WHERE BranchID = @BranchID AND ArchiveYear IS NULL
                    ORDER BY TransactionDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { da.Fill(dt); }
                }
            }
            return dt;
        }

        public void CloseMonthlyBranchTreasury(int branchId)
        {
            if (branchId <= 0) throw new ArgumentException("الفرع غير صالح.");

            DateTime now = DateTime.Now;

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                con.Open();
                using (SqlTransaction trans = con.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmdArchive = new SqlCommand("UPDATE BranchMonthlyTreasury SET ArchiveYear = @Year, ArchiveMonth = @Month WHERE BranchID = @BranchID AND ArchiveYear IS NULL", con, trans))
                        {
                            cmdArchive.Parameters.Add("@Year", SqlDbType.Int).Value = now.Year;
                            cmdArchive.Parameters.Add("@Month", SqlDbType.Int).Value = now.Month;
                            cmdArchive.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                            cmdArchive.ExecuteNonQuery();
                        }

                        using (SqlCommand cmdSales = new SqlCommand("UPDATE Sales SET IsMonthlyClosed = 1 WHERE ISNULL(LocationId, 1) = @BranchID AND ISNULL(IsShiftClosed, 0) = 1 AND ISNULL(IsMonthlyClosed, 0) = 0", con, trans))
                        {
                            cmdSales.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                            cmdSales.ExecuteNonQuery();
                        }

                        using (SqlCommand cmdCash = new SqlCommand("UPDATE CashTransactions SET IsMonthlyClosed = 1 WHERE ISNULL(LocationId, 1) = @BranchID AND ISNULL(IsClosed, 0) = 1 AND ISNULL(IsMonthlyClosed, 0) = 0", con, trans))
                        {
                            cmdCash.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                            cmdCash.ExecuteNonQuery();
                        }

                        using (SqlCommand cmdReturns = new SqlCommand("UPDATE [Returns] SET IsMonthlyClosed = 1 WHERE ISNULL(LocationId, 1) = @BranchID AND ISNULL(IsShiftClosed, 0) = 1 AND ISNULL(IsMonthlyClosed, 0) = 0", con, trans))
                        {
                            cmdReturns.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                            cmdReturns.ExecuteNonQuery();
                        }

                        trans.Commit();
                    }
                    catch { trans.Rollback(); throw; }
                }
            }
        }

        public DataTable GetArchivedMonthsSummary(int branchId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                const string query = @"
                    SELECT ArchiveYear, ArchiveMonth,
                           ISNULL(SUM(ISNULL(Debit, 0)), 0) - ISNULL(SUM(ISNULL(Credit, 0)), 0) AS NetAmount
                    FROM BranchMonthlyTreasury
                    WHERE BranchID = @BranchID AND ArchiveYear IS NOT NULL
                    GROUP BY ArchiveYear, ArchiveMonth
                    ORDER BY ArchiveYear DESC, ArchiveMonth DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { da.Fill(dt); }
                }
            }

            dt.Columns.Add("PeriodLabel", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                int year = Convert.ToInt32(row["ArchiveYear"]);
                int month = Convert.ToInt32(row["ArchiveMonth"]);
                string monthName = month >= 1 && month <= 12 ? MonthNames[month - 1] : month.ToString();
                row["PeriodLabel"] = monthName + " " + year;
            }

            return dt;
        }

        public DataTable GetArchivedMonthDetails(int branchId, int year, int month)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                const string query = @"
                    SELECT TransactionDate AS [التاريخ],
                           ISNULL(Debit, 0) AS [وارد (ترحيل)],
                           ISNULL(Credit, 0) AS [صادر (توريد)],
                           Note AS [البيان]
                    FROM BranchMonthlyTreasury
                    WHERE BranchID = @BranchID AND ArchiveYear = @Year AND ArchiveMonth = @Month
                    ORDER BY TransactionDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@BranchID", SqlDbType.Int).Value = branchId;
                    cmd.Parameters.Add("@Year", SqlDbType.Int).Value = year;
                    cmd.Parameters.Add("@Month", SqlDbType.Int).Value = month;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd)) { da.Fill(dt); }
                }
            }
            return dt;
        }
    }
}