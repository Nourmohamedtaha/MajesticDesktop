using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MajesticDesktop.Services
{
    public class AuthService
    {
        public static User CurrentUser { get; private set; }

        public AuthService()
        {
            EnsureSchema();
        }

        private void EnsureSchema()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                const string query = @"
                    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Users') AND name = 'LocationId')
                    BEGIN
                        ALTER TABLE Users ADD LocationId INT NULL;
                    END

                    UPDATE Roles SET RoleName = N'مدير' WHERE RoleName IN ('SuperAdmin', 'Super Admin', N'سوبر ادمن');
                    UPDATE Roles SET RoleName = N'مسئول فرع' WHERE RoleName IN ('BranchAdmin', 'Branch Admin', 'Admin', N'ادمن');

                    IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = N'مدير')
                    BEGIN
                        INSERT INTO Roles (RoleName) VALUES (N'مدير');
                    END

                    IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = N'مسئول فرع')
                    BEGIN
                        INSERT INTO Roles (RoleName) VALUES (N'مسئول فرع');
                    END";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // التعديل: التحويل لـ Async لعدم تجميد واجهة الدخول
        public async Task<bool> LoginAsync(string username, string password)
        {
            const string query = @"
                SELECT u.Id, u.FullName, u.Username, u.RoleId, r.RoleName, u.IsActive, u.LocationId,
                       l.Name AS BranchName
                FROM Users u
                INNER JOIN Roles r ON u.RoleId = r.Id
                LEFT JOIN Locations l ON u.LocationId = l.Id
                WHERE u.Username = @Username AND u.Password = @Password AND u.IsActive = 1;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = username;
                    command.Parameters.Add("@Password", SqlDbType.NVarChar, 100).Value = password;

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string roleName = reader["RoleName"].ToString();

                            // 🛠️ السماح للمدير فقط بالدخول للديسكتوب
                            if (roleName != "مدير")
                            {
                                return false;
                            }

                            CurrentUser = new User
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FullName = reader["FullName"].ToString(),
                                Username = reader["Username"].ToString(),
                                RoleId = Convert.ToInt32(reader["RoleId"]),
                                RoleName = roleName,
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                LocationId = reader["LocationId"] != DBNull.Value ? Convert.ToInt32(reader["LocationId"]) : (int?)null,
                                BranchName = reader["BranchName"]?.ToString() ?? string.Empty
                            };
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}