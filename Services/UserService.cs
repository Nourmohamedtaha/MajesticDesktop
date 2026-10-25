using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MajesticDesktop.Services
{
    public class UserService
    {
        public async Task<List<Role>> GetAllRolesAsync()
        {
            var roles = new List<Role>();
            const string query = "SELECT Id, RoleName FROM Roles ORDER BY Id;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        roles.Add(new Role
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            RoleName = reader["RoleName"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }
            return roles;
        }

        public async Task<List<Location>> GetAllLocationsAsync()
        {
            var locations = new List<Location>();
            const string query = @"
                SELECT Id, Name, Type, IsActive
                FROM Locations
                ORDER BY Id;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        locations.Add(new Location
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"]?.ToString() ?? string.Empty,
                            Type = reader["Type"]?.ToString() ?? string.Empty,
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        });
                    }
                }
            }
            return locations;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = new List<User>();

            const string query = @"
                SELECT 
                    u.Id,
                    u.FullName,
                    u.Username,
                    u.Password,
                    u.RoleId,
                    r.RoleName,
                    u.IsActive,
                    u.LocationId,
                    l.Name AS BranchName
                FROM Users u
                INNER JOIN Roles r ON u.RoleId = r.Id
                LEFT JOIN Locations l ON u.LocationId = l.Id
                ORDER BY u.Id DESC;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FullName = reader["FullName"]?.ToString() ?? string.Empty,
                            Username = reader["Username"]?.ToString() ?? string.Empty,
                            Password = reader["Password"]?.ToString() ?? string.Empty,
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            RoleName = reader["RoleName"]?.ToString() ?? string.Empty,
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            LocationId = reader["LocationId"] != DBNull.Value ? Convert.ToInt32(reader["LocationId"]) : (int?)null,
                            BranchName = reader["BranchName"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }
            return users;
        }

        public async Task SaveUserAsync(User user, bool isEdit)
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                await connection.OpenAsync();

                if (isEdit)
                {
                    const string updateQuery = @"
                        UPDATE Users
                        SET 
                            FullName = @FullName,
                            Username = @Username,
                            Password = @Password,
                            RoleId = @RoleId,
                            LocationId = @LocationId,
                            IsActive = @IsActive
                        WHERE Id = @Id;";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 100).Value = user.FullName;
                        cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = user.Username;
                        cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 100).Value = user.Password;
                        cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = user.RoleId;
                        cmd.Parameters.Add("@LocationId", SqlDbType.Int).Value = user.LocationId.HasValue ? (object)user.LocationId.Value : DBNull.Value;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = user.IsActive;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = user.Id;

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                else
                {
                    const string insertQuery = @"
                        INSERT INTO Users
                        (
                            FullName,
                            Username,
                            Password,
                            RoleId,
                            LocationId,
                            IsActive
                        )
                        VALUES
                        (
                            @FullName,
                            @Username,
                            @Password,
                            @RoleId,
                            @LocationId,
                            @IsActive
                        );";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 100).Value = user.FullName;
                        cmd.Parameters.Add("@Username", SqlDbType.NVarChar, 50).Value = user.Username;
                        cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 100).Value = user.Password;
                        cmd.Parameters.Add("@RoleId", SqlDbType.Int).Value = user.RoleId;
                        cmd.Parameters.Add("@LocationId", SqlDbType.Int).Value = user.LocationId.HasValue ? (object)user.LocationId.Value : DBNull.Value;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = user.IsActive;

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public async Task CreateBranchAdminAsync(int locationId, string fullName, string username, string password)
        {
            var roles = await GetAllRolesAsync();
            var branchAdminRole = roles.Find(r => r.RoleName == "مسئول فرع");

            if (branchAdminRole == null)
                throw new Exception("دور 'مسئول فرع' غير موجود في النظام.");

            await SaveUserAsync(
                new User
                {
                    FullName = fullName,
                    Username = username,
                    Password = password,
                    RoleId = branchAdminRole.Id,
                    LocationId = locationId,
                    IsActive = true
                },
                false);
        }

        public async Task DeactivateUserAsync(int userId)
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                await connection.OpenAsync();
                const string query = "UPDATE Users SET IsActive = 0 WHERE Id = @Id;";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = userId;
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}