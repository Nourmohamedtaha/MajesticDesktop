using System;
using System.Data;
using System.Data.SqlClient;
using MajesticDesktop.Database;

namespace MajesticDesktop.Services
{
    // ADO.NET خام بالكامل (SqlConnection/SqlCommand/SqlDataReader) - مفيش أي Entity Framework أو ORM هنا.
    public class DashboardData
    {
        public int TotalProducts { get; set; }
        public decimal TodaySales { get; set; }
        public decimal TodayExpenses { get; set; }
        public decimal TodayProfit { get; set; }
        public decimal TodayAverageInvoice { get; set; }
        public int PendingRequestsCount { get; set; }
        public DataTable Shortages { get; set; }
        public DataTable RecentRequests { get; set; }
        public DataTable RecentSales { get; set; }
    }

    public class DashboardService
    {
        private const int MAIN_WAREHOUSE_ID = 1;

        public DashboardData GetDashboardData()
        {
            DashboardData data = new DashboardData
            {
                Shortages = new DataTable(),
                RecentRequests = new DataTable(),
                RecentSales = new DataTable()
            };

            data.Shortages.Columns.Add("RowNum");
            data.Shortages.Columns.Add("ProductName");
            data.Shortages.Columns.Add("CurrentQty");
            data.Shortages.Columns.Add("AlertLimit");

            data.RecentRequests.Columns.Add("RowNum");
            data.RecentRequests.Columns.Add("BranchName");
            data.RecentRequests.Columns.Add("RequestDate");

            data.RecentSales.Columns.Add("RowNum");
            data.RecentSales.Columns.Add("Time");
            data.RecentSales.Columns.Add("Amount");

            // FIX: "مصروفات اليوم" كانت بتجمع كل حركة TransactionType = N'صادر' مهما كان نوعها،
            // وده كان بيشمل حركات مالهاش علاقة بمصروف حقيقي (تسوية عجز/زيادة الدرج، سحوبات إدارية...)
            // وفي نفس الوقت بيفوّت المصروفات الحقيقية لأنها متسجلة بأنواع تانية (مصروف، مرتبات...).
            // اتظبطت هنا تطابق بالظبط نفس تصنيف المصروفات في شاشة "يومية الخزنة" (CashRegisterService)
            // عشان الرقمين ميختلفوش عن بعض.
            const string query = @"
                SELECT COUNT(Id) FROM Products WHERE IsActive = 1 AND BranchId IS NULL;

                SELECT ISNULL(SUM(NetAmount), 0), ISNULL(AVG(NetAmount), 0)
                FROM Sales
                WHERE LocationId = @LocId AND CAST(InvoiceDate AS DATE) = CAST(GETDATE() AS DATE);

                SELECT ISNULL(SUM(Amount), 0) FROM CashTransactions
                WHERE LocationId = @LocId
                  AND TransactionType IN (N'مصروف', N'مصروفات محل', N'مصروفات بوفيه', N'مرتبات', N'سلف', N'سلفة', N'نثريات', N'صيانة', N'إيجار', N'مصروفات أخرى', N'أخرى')
                  AND CAST(TransactionDate AS DATE) = CAST(GETDATE() AS DATE);

                SELECT ISNULL(SUM((ISNULL(sd.UnitPrice, 0) - ISNULL(p.PurchasePrice, 0)) * ISNULL(sd.Quantity, 0)), 0)
                FROM SaleDetails sd
                INNER JOIN Sales s ON sd.SaleId = s.Id
                INNER JOIN Products p ON sd.ProductId = p.Id
                WHERE s.LocationId = @LocId AND CAST(s.InvoiceDate AS DATE) = CAST(GETDATE() AS DATE);

                SELECT ROW_NUMBER() OVER (ORDER BY ISNULL(SUM(s.Quantity), 0) ASC) AS RowNum, p.Name, ISNULL(SUM(s.Quantity), 0), ISNULL(p.AlertLimit, 5)
                FROM Products p
                LEFT JOIN Stock s ON p.Id = s.ProductId AND s.LocationId = @LocId
                WHERE p.IsActive = 1 AND p.BranchId IS NULL
                GROUP BY p.Id, p.Name, p.AlertLimit
                HAVING ISNULL(SUM(s.Quantity), 0) <= ISNULL(p.AlertLimit, 5);

                SELECT TOP 8 ROW_NUMBER() OVER (ORDER BY Id DESC) AS RowNum,
                       CONVERT(VARCHAR(8), InvoiceDate, 108) AS SaleTime, NetAmount
                FROM Sales
                WHERE LocationId = @LocId AND CAST(InvoiceDate AS DATE) = CAST(GETDATE() AS DATE)
                ORDER BY Id DESC;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();

                decimal grossProfit = 0;
                decimal totalExpenses = 0;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@LocId", SqlDbType.Int).Value = MAIN_WAREHOUSE_ID;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) data.TotalProducts = Convert.ToInt32(reader[0]);

                        if (reader.NextResult() && reader.Read())
                        {
                            data.TodaySales = Convert.ToDecimal(reader[0]);
                            data.TodayAverageInvoice = Convert.ToDecimal(reader[1]);
                        }

                        if (reader.NextResult() && reader.Read())
                            totalExpenses = Convert.ToDecimal(reader[0]);

                        if (reader.NextResult() && reader.Read())
                            grossProfit = Convert.ToDecimal(reader[0]);

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                data.Shortages.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString());
                            }
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                data.RecentSales.Rows.Add(reader[0].ToString(), reader[1].ToString(), Convert.ToDecimal(reader[2]).ToString("N2"));
                            }
                        }
                    }
                }

                data.TodayExpenses = totalExpenses;
                data.TodayProfit = grossProfit - totalExpenses;

                // جدول الطلبات في try/catch منفصلة: Requests/Locations ممكن تكون مش موجودة
                // في تنصيبة لسه ملحقتش تفعّل نظام الفروع، فمينفعش يوقف باقي الداشبورد لو مش موجودة.
                try
                {
                    const string queryReq = @"
                        SELECT COUNT(Id) FROM Requests WHERE Status = N'قيد الانتظار' OR Status IS NULL;

                        SELECT TOP 8 ROW_NUMBER() OVER (ORDER BY r.Id DESC) AS RowNum, ISNULL(l.Name, N'غير محدد'), ISNULL(CAST(r.RequestDate AS DATE), GETDATE())
                        FROM Requests r
                        LEFT JOIN Locations l ON r.LocationId = l.Id
                        WHERE r.Status = N'قيد الانتظار' OR r.Status IS NULL
                        ORDER BY r.Id DESC;";

                    using (SqlCommand cmdReq = new SqlCommand(queryReq, connection))
                    using (SqlDataReader readerReq = cmdReq.ExecuteReader())
                    {
                        if (readerReq.Read()) data.PendingRequestsCount = Convert.ToInt32(readerReq[0]);
                        if (readerReq.NextResult())
                        {
                            while (readerReq.Read())
                            {
                                data.RecentRequests.Rows.Add(readerReq[0].ToString(), readerReq[1].ToString(), Convert.ToDateTime(readerReq[2]).ToString("yyyy/MM/dd"));
                            }
                        }
                    }
                }
                catch
                {
                    // جدول الطلبات مش موجود لسه - يفضل فاضي بدل ما يوقف الداشبورد كله
                }
            }

            return data;
        }
    }
}