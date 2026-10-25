using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MajesticDesktop.Services
{
    public class SettingsService
    {
        public SettingsService()
        {
            EnsureTableExists();
        }

        private void EnsureTableExists()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                const string query = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SystemSettings' AND xtype='U')
                    BEGIN
                        CREATE TABLE SystemSettings (
                            Id INT PRIMARY KEY DEFAULT 1,
                            CompanyName NVARCHAR(200) NOT NULL,
                            Phone NVARCHAR(100),
                            Address NVARCHAR(300),
                            TaxNumber NVARCHAR(100),
                            Logo VARBINARY(MAX)
                        );
                        
                        INSERT INTO SystemSettings (Id, CompanyName, Phone, Address, TaxNumber) 
                        VALUES (1, 'اسم الشركة (الافتراضي)', '', '', '');
                    END";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task<SystemSetting> GetSettingsAsync()
        {
            SystemSetting setting = new SystemSetting();
            const string query = "SELECT CompanyName, Phone, Address, TaxNumber, Logo FROM SystemSettings WHERE Id = 1;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            setting.CompanyName = reader["CompanyName"].ToString();
                            setting.Phone = reader["Phone"].ToString();
                            setting.Address = reader["Address"].ToString();
                            setting.TaxNumber = reader["TaxNumber"].ToString();

                            if (reader["Logo"] != DBNull.Value)
                            {
                                setting.Logo = (byte[])reader["Logo"];
                            }
                        }
                    }
                }
            }
            return setting;
        }

        public async Task SaveSettingsAsync(SystemSetting setting)
        {
            const string query = @"
                UPDATE SystemSettings 
                SET CompanyName = @CompanyName, 
                    Phone = @Phone, 
                    Address = @Address, 
                    TaxNumber = @TaxNumber, 
                    Logo = @Logo 
                WHERE Id = 1;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 200).Value = setting.CompanyName;
                    command.Parameters.Add("@Phone", SqlDbType.NVarChar, 100).Value = (object)setting.Phone ?? DBNull.Value;
                    command.Parameters.Add("@Address", SqlDbType.NVarChar, 300).Value = (object)setting.Address ?? DBNull.Value;
                    command.Parameters.Add("@TaxNumber", SqlDbType.NVarChar, 100).Value = (object)setting.TaxNumber ?? DBNull.Value;

                    if (setting.Logo != null && setting.Logo.Length > 0)
                    {
                        command.Parameters.Add("@Logo", SqlDbType.VarBinary).Value = setting.Logo;
                    }
                    else
                    {
                        command.Parameters.Add("@Logo", SqlDbType.VarBinary).Value = DBNull.Value;
                    }

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}