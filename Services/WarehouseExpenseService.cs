using MajesticDesktop.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks; // هامة لعمليات Async

namespace MajesticDesktop.Services
{
    public class WarehouseExpenseService
    {
        // مصدر واحد للتصنيفات: الكومبو في الشاشة ومرشّح التقرير بيقروا من هنا،
        // فمش ممكن تختلف عن بعض أو ينسى حد يحدّث الاتنين.
        public static readonly string[] ExpenseCategories =
        {
            "مصروفات بوفيه", "مرتبات", "نثريات", "صيانة", "إيجار", "مصروفات أخرى"
        };

        public static readonly string[] PaymentSources =
        {
            "نقدي", "إنستاباي الكردي", "إنستاباي الحكش"
        };

        public WarehouseExpenseService()
        {
            EnsureSchemaUpdated();
        }

        private void EnsureSchemaUpdated()
        {
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                con.Open();
                string query = @"
                    IF COL_LENGTH('CashTransactions', 'BorrowerName') IS NULL 
                    BEGIN
                        ALTER TABLE CashTransactions ADD BorrowerName NVARCHAR(150) NULL;
                    END
                    
                    IF COL_LENGTH('CashTransactions', 'PaymentSource') IS NULL 
                    BEGIN
                        ALTER TABLE CashTransactions ADD PaymentSource NVARCHAR(100) NULL;
                    END

                    IF COL_LENGTH('CashTransactions', 'PaymentMethod') IS NULL 
                    BEGIN
                        ALTER TABLE CashTransactions ADD PaymentMethod NVARCHAR(100) NULL;
                    END
                ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // التعديل الأول: التحويل لدالة Async لمنع التجميد أثناء الحفظ
        public async Task AddWarehouseExpenseAsync(string operationType, string expenseCategory, decimal amount, string paymentSource, string notes, string borrowerName = "")
        {
            if (amount <= 0) throw new ArgumentException("يجب إدخال مبلغ صحيح أكبر من الصفر.");
            if (string.IsNullOrWhiteSpace(paymentSource)) throw new ArgumentException("يجب اختيار مصدر الدفع.");

            string finalTransactionType = (operationType == "سلفة") ? "سلفة" : expenseCategory;

            if (operationType == "سلفة" && string.IsNullOrWhiteSpace(borrowerName))
            {
                throw new ArgumentException("برجاء إدخال اسم المستلف.");
            }
            if (operationType == "مصروف" && string.IsNullOrWhiteSpace(expenseCategory))
            {
                throw new ArgumentException("برجاء اختيار تصنيف المصروف.");
            }

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                await con.OpenAsync();

                // بنسجل PaymentSource (النص التفصيلي) وPaymentMethod (نفس القيمة) مع بعض،
                // عشان أي حساب "كاش فقط" في تقارير الفروع (اللي بيفلتر على PaymentMethod)
                // يستبعد صح مصروفات المخزن اللي اتدفعت إنستاباي، ويحسب بس اللي فعلاً نقدي.
                string query = @"
                    INSERT INTO CashTransactions 
                    (TransactionDate, TransactionType, Amount, Description, LocationId, IsClosed, BorrowerName, PaymentSource, PaymentMethod) 
                    VALUES 
                    (GETDATE(), @Type, @Amount, @Note, 1, 0, @BorrowerName, @PaymentSource, @PaymentMethod);
                ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 200).Value = finalTransactionType;
                    cmd.Parameters.Add("@PaymentSource", SqlDbType.NVarChar, 100).Value = paymentSource;
                    cmd.Parameters.Add("@PaymentMethod", SqlDbType.NVarChar, 100).Value = paymentSource;

                    SqlParameter paramAmount = new SqlParameter("@Amount", SqlDbType.Decimal);
                    paramAmount.Precision = 18;
                    paramAmount.Scale = 2;
                    paramAmount.Value = amount;
                    cmd.Parameters.Add(paramAmount);

                    cmd.Parameters.Add("@Note", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(notes) ? (object)DBNull.Value : notes.Trim();
                    cmd.Parameters.Add("@BorrowerName", SqlDbType.NVarChar, 150).Value = string.IsNullOrWhiteSpace(borrowerName) ? (object)DBNull.Value : borrowerName.Trim();

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // التعديل الثاني: التحويل لدالة Async وتعبئة DataTable باستخدام DataReader لأفضل أداء
        public async Task<DataTable> GetWarehouseExpensesAsync(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();

            List<string> categories = new List<string>(ExpenseCategories) { "سلفة" };
            string inClause = string.Join(",", categories.Select((c, i) => "@Cat" + i));

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                string query = $@"
                    SELECT 
                        Id, 
                        FORMAT(TransactionDate, 'yyyy/MM/dd hh:mm tt') AS [التاريخ والوقت], 
                        TransactionType AS [نوع العملية], 
                        ISNULL(BorrowerName, '') AS [اسم المستلف],
                        Amount AS [المبلغ], 
                        ISNULL(PaymentSource, 'نقدي') AS [مصدر الدفع],
                        ISNULL(Description, '') AS [البيان] 
                    FROM CashTransactions 
                    WHERE LocationId = 1 
                      AND CAST(TransactionDate AS DATE) >= CAST(@From AS DATE)
                      AND CAST(TransactionDate AS DATE) <= CAST(@To AS DATE)
                      AND TransactionType IN ({inClause})
                    ORDER BY TransactionDate DESC";

                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@From", SqlDbType.Date).Value = fromDate.Date;
                    cmd.Parameters.Add("@To", SqlDbType.Date).Value = toDate.Date;
                    for (int i = 0; i < categories.Count; i++)
                        cmd.Parameters.Add("@Cat" + i, SqlDbType.NVarChar, 200).Value = categories[i];

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }
    }
}