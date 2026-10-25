using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace MajesticDesktop.Services
{
    public class ShiftService
    {
        public ShiftService()
        {
            EnsureTableExists();
        }

        private void EnsureTableExists()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                const string queryCreate = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ShiftClosings' AND xtype='U')
                    BEGIN
                        CREATE TABLE ShiftClosings (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            LocationId INT NOT NULL DEFAULT 1,
                            UserId INT NOT NULL DEFAULT 1,
                            ClosingDate DATETIME NOT NULL DEFAULT GETDATE(),
                            ExpectedAmount DECIMAL(18,2) NOT NULL,
                            ActualAmount DECIMAL(18,2) NOT NULL,
                            DifferenceAmount DECIMAL(18,2) NOT NULL,
                            Notes NVARCHAR(500)
                        );
                    END";
                using (SqlCommand command = new SqlCommand(queryCreate, connection))
                {
                    command.ExecuteNonQuery();
                }

                // تحديث الجدول لو كان قديم عشان نزود الفرع
                const string queryAlter = @"
                    IF COL_LENGTH('ShiftClosings', 'LocationId') IS NULL
                    BEGIN
                        ALTER TABLE ShiftClosings ADD LocationId INT NOT NULL DEFAULT 1;
                    END";
                using (SqlCommand command = new SqlCommand(queryAlter, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // إحنا مش هنستخدم دالة `GetExpectedCashForToday` القديمة لأننا هنجيب الرصيد مباشرة من `CashRegisterService` بالفرع

        public void SaveShiftClosing(int locationId, ShiftClosing shiftClosing)
        {
            const string query = @"
                INSERT INTO ShiftClosings (LocationId, UserId, ClosingDate, ExpectedAmount, ActualAmount, DifferenceAmount, Notes)
                VALUES (@LocationId, @UserId, @ClosingDate, @ExpectedAmount, @ActualAmount, @DifferenceAmount, @Notes);";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@LocationId", SqlDbType.Int).Value = locationId;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = shiftClosing.UserId;
                command.Parameters.Add("@ClosingDate", SqlDbType.DateTime).Value = shiftClosing.ClosingDate;
                command.Parameters.Add("@ExpectedAmount", SqlDbType.Decimal).Value = shiftClosing.ExpectedAmount;
                command.Parameters.Add("@ActualAmount", SqlDbType.Decimal).Value = shiftClosing.ActualAmount;
                command.Parameters.Add("@DifferenceAmount", SqlDbType.Decimal).Value = shiftClosing.Difference;
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = (object)shiftClosing.Notes ?? DBNull.Value;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}