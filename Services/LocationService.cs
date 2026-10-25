using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MajesticDesktop.Services
{
    public class LocationService
    {
        public List<Location> GetAllLocations()
        {
            List<Location> locations = new List<Location>();

            const string query = @"
                SELECT Id, Name, Type, ISNULL(IsActive, 1) AS IsActive 
                FROM Locations 
                ORDER BY Id ASC;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        locations.Add(new Location
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Type = reader["Type"].ToString(),
                            IsActive = Convert.ToBoolean(reader["IsActive"])
                        });
                    }
                }
            }

            return locations;
        }

        public void AddLocation(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            const string query = @"
                INSERT INTO Locations (Name, Type, IsActive) 
                VALUES (@Name, @Type, @IsActive);";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = location.Name;
                command.Parameters.Add("@Type", SqlDbType.NVarChar, 50).Value = location.Type;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = location.IsActive;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateLocation(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            const string query = @"
                UPDATE Locations 
                SET Name = @Name, 
                    Type = @Type 
                WHERE Id = @Id;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = location.Id;
                command.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = location.Name;
                command.Parameters.Add("@Type", SqlDbType.NVarChar, 50).Value = location.Type;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void ToggleLocationStatus(int id, bool newStatus)
        {
            const string query = @"UPDATE Locations SET IsActive = @IsActive WHERE Id = @Id;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = newStatus;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}