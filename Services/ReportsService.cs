using MajesticDesktop.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MajesticDesktop.Services
{
    public class ReportsService
    {
        public async Task<DataTable> GetAllLocationsAsync()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM Locations WHERE IsActive = 1 ORDER BY Id", con))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }

        public async Task<DataTable> GetAllProductsAsync()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM Products WHERE IsActive = 1 ORDER BY Name", con))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }

        public async Task<DataTable> GetDistinctRegionsAsync()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT Region FROM Customers WHERE IsActive = 1 AND Region IS NOT NULL AND Region <> '' ORDER BY Region", con))
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    dt.Load(reader);
                }
            }
            return dt;
        }

        public async Task<DataTable> GetCustomersReportAsync(int locationId, string region, DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();
            string regionFilter = (!string.IsNullOrEmpty(region) && region != "الكل") ? " AND c.Region = @Region " : "";

            string query = $@"
                SELECT 
                    c.Id AS [كود العميل], 
                    c.Name AS [اسم العميل], 
                    c.Phone AS [الموبايل], 
                    c.Region AS [المنطقة], 
                    ISNULL((SELECT SUM(NetAmount) FROM Sales WHERE CustomerId = c.Id AND CAST(InvoiceDate AS DATE) BETWEEN @From AND @To), 0) AS [إجمالي المسحوبات],
                    ISNULL((SELECT SUM(Amount) FROM CustomerPayments WHERE CustomerId = c.Id AND CAST(PaymentDate AS DATE) BETWEEN @From AND @To), 0) AS [إجمالي السداد],
                    c.Balance AS [الرصيد الافتتاحي],
                    (ISNULL(c.Balance, 0) 
                     + ISNULL((SELECT SUM(RemainingAmount) FROM Sales WHERE CustomerId = c.Id AND RemainingAmount > 0), 0)
                     - ISNULL((SELECT SUM(TotalRefundAmount) FROM SalesReturns WHERE CustomerId = c.Id), 0)
                     - ISNULL((SELECT SUM(Amount) FROM CustomerPayments WHERE CustomerId = c.Id), 0)) AS [المديونية الحالية]
                FROM Customers c 
                WHERE c.IsActive = 1 AND c.LocationId = @LocId {regionFilter}
                ORDER BY c.Name";

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LocId", locationId);
                    cmd.Parameters.AddWithValue("@From", from.Date);
                    cmd.Parameters.AddWithValue("@To", to.Date);
                    if (!string.IsNullOrEmpty(region) && region != "الكل")
                        cmd.Parameters.AddWithValue("@Region", region);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync()) { dt.Load(reader); }
                }
            }
            return dt;
        }

        public async Task<DataTable> GetSuppliersReportAsync(int locationId, DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT 
                    s.Id AS [كود المورد], 
                    s.Name AS [اسم المورد], 
                    s.Phone AS [الموبايل], 
                    s.Region AS [المنطقة], 
                    s.Balance AS [الرصيد الافتتاحي]
                FROM Suppliers s 
                WHERE s.IsActive = 1 AND s.LocationId = @LocId
                ORDER BY s.Name";

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LocId", locationId);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync()) { dt.Load(reader); }
                }
            }
            return dt;
        }

        public async Task<DataTable> GetInventoryReportAsync(int locationId)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT 
                    p.Id AS [كود المنتج],
                    p.Name AS [اسم المنتج],
                    ISNULL(s.Quantity, 0) AS [الرصيد الحالي],
                    ISNULL(bp.PurchasePrice, ISNULL(p.PurchasePrice, 0)) AS [سعر التكلفة],
                    (ISNULL(s.Quantity, 0) * ISNULL(bp.PurchasePrice, ISNULL(p.PurchasePrice, 0))) AS [إجمالي القيمة]
                FROM Products p
                LEFT JOIN Stock s ON p.Id = s.ProductId AND s.LocationId = @LocId
                LEFT JOIN BranchProducts bp ON bp.ProductID = p.Id AND bp.BranchID = @LocId
                WHERE p.IsActive = 1 AND (p.BranchId IS NULL OR p.BranchId = @LocId)
                ORDER BY p.Name";

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LocId", locationId);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync()) { dt.Load(reader); }
                }
            }
            return dt;
        }

        public async Task<DataTable> GetProfitabilityReportAsync(int locationId, int productId, DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();
            string productFilter = productId > 0 ? "AND p.Id = @ProdId" : "";

            string query = $@"
                SELECT 
                    p.Id AS [كود المنتج],
                    p.Name AS [اسم المنتج],
                    SUM(sd.Quantity) AS [الكمية المباعة],
                    SUM(sd.Quantity * sd.UnitPrice) / NULLIF(SUM(sd.Quantity), 0) AS [متوسط سعر البيع],
                    SUM(sd.Quantity * ISNULL(bp.PurchasePrice, ISNULL(p.PurchasePrice, 0))) / NULLIF(SUM(sd.Quantity), 0) AS [التكلفة],
                    SUM(sd.Quantity * sd.UnitPrice) AS [إجمالي الإيراد],
                    SUM(sd.Quantity * ISNULL(bp.PurchasePrice, ISNULL(p.PurchasePrice, 0))) AS [إجمالي التكلفة],
                    SUM((sd.UnitPrice - ISNULL(bp.PurchasePrice, ISNULL(p.PurchasePrice, 0))) * sd.Quantity) AS [صافي الربح],
                    CASE WHEN SUM(sd.Quantity * sd.UnitPrice) = 0 THEN 0
                         ELSE SUM((sd.UnitPrice - ISNULL(bp.PurchasePrice, ISNULL(p.PurchasePrice, 0))) * sd.Quantity) * 100.0
                              / SUM(sd.Quantity * sd.UnitPrice) END AS [هامش الربح %]
                FROM SaleDetails sd
                INNER JOIN Sales s ON sd.SaleId = s.Id
                INNER JOIN Products p ON sd.ProductId = p.Id
                LEFT JOIN BranchProducts bp ON bp.ProductID = p.Id AND bp.BranchID = ISNULL(s.LocationId, 1)
                WHERE ISNULL(s.LocationId, 1) = @LocId 
                  AND CAST(s.InvoiceDate AS DATE) >= @From 
                  AND CAST(s.InvoiceDate AS DATE) <= @To
                  {productFilter}
                GROUP BY p.Id, p.Name
                ORDER BY [صافي الربح] DESC";

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@LocId", locationId);
                    cmd.Parameters.AddWithValue("@From", from.Date);
                    cmd.Parameters.AddWithValue("@To", to.Date);
                    if (productId > 0) cmd.Parameters.AddWithValue("@ProdId", productId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync()) { dt.Load(reader); }
                }
            }
            return dt;
        }

        public async Task<DataTable> GetProductHistoryAsync(int productId, int locationId, DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();
            string query = @"
                DECLARE @Sql NVARCHAR(MAX) = N'';
                
                SET @Sql = @Sql + N'
                    SELECT s.InvoiceDate AS [التاريخ], N''مبيعات'' AS [نوع الحركة], ISNULL(c.Name, N''عميل نقدي'') AS [الطرف الثاني], sd.Quantity AS [الكمية], sd.UnitPrice AS [السعر]
                    FROM SaleDetails sd 
                    INNER JOIN Sales s ON sd.SaleId = s.Id 
                    LEFT JOIN Customers c ON s.CustomerId = c.Id 
                    WHERE sd.ProductId = @PId AND s.LocationId = @LId AND CAST(s.InvoiceDate AS DATE) BETWEEN @From AND @To ';
                
                IF EXISTS (SELECT 1 FROM sysobjects WHERE name = 'PurchaseInvoiceDetails')
                BEGIN
                    SET @Sql = @Sql + N' UNION ALL 
                    SELECT pinv.InvoiceDate AS [التاريخ], N''مشتريات'' AS [نوع الحركة], ISNULL(sup.Name, N''مورد نقدي'') AS [الطرف الثاني], pid.Quantity AS [الكمية], pid.UnitPrice AS [السعر]
                    FROM PurchaseInvoiceDetails pid 
                    INNER JOIN PurchaseInvoices pinv ON pid.InvoiceId = pinv.Id 
                    LEFT JOIN Suppliers sup ON pinv.SupplierId = sup.Id 
                    WHERE pid.ProductId = @PId AND pinv.LocationId = @LId AND CAST(pinv.InvoiceDate AS DATE) BETWEEN @From AND @To ';
                END
                
                SET @Sql = @Sql + N' ORDER BY [التاريخ] DESC';
                
                EXEC sp_executesql @Sql, N'@PId INT, @LId INT, @From DATE, @To DATE', @PId = @ProductId, @LId = @LocationId, @From = @FromDate, @To = @ToDate;
            ";

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@LocationId", locationId);
                    cmd.Parameters.AddWithValue("@FromDate", from.Date);
                    cmd.Parameters.AddWithValue("@ToDate", to.Date);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync()) { dt.Load(reader); }
                }
            }
            return dt;
        }

        public async Task<DataTable> GetCustomerLedgerAsync(int customerId, DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT 
                    InvoiceDate AS [التاريخ], 
                    N'فاتورة مبيعات رقم ' + CAST(InvoiceNumber AS NVARCHAR(50)) AS [البيان], 
                    NetAmount AS [مدين (عليه)], 
                    CAST(0 AS DECIMAL(18,2)) AS [دائن (سدد)]
                FROM Sales 
                WHERE CustomerId = @CId AND CAST(InvoiceDate AS DATE) BETWEEN @From AND @To
                
                UNION ALL
                
                SELECT 
                    PaymentDate AS [التاريخ], 
                    N'سداد دفعة' AS [البيان], 
                    CAST(0 AS DECIMAL(18,2)) AS [مدين (عليه)], 
                    Amount AS [دائن (سدد)]
                FROM CustomerPayments 
                WHERE CustomerId = @CId AND CAST(PaymentDate AS DATE) BETWEEN @From AND @To
                
                ORDER BY [التاريخ] DESC";

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CId", customerId);
                    cmd.Parameters.AddWithValue("@From", from.Date);
                    cmd.Parameters.AddWithValue("@To", to.Date);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync()) { dt.Load(reader); }
                }
            }
            return dt;
        }
    }
}