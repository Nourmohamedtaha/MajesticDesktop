using MajesticDesktop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MajesticDesktop.Database;

namespace MajesticDesktop.Services
{
    public class CategoryService
    {
        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                const string query = "SELECT Id, Name, Description, IsActive FROM Categories ORDER BY Id DESC;";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
                }
            }
            return categories;
        }

        public void AddCategory(Category category)
        {
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                const string query = "INSERT INTO Categories (Name, Description, IsActive) VALUES (@Name, @Description, @IsActive);";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // استخدام SqlDbType لتحسين الأداء (أفضل من AddWithValue)
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = category.Name;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 255).Value = string.IsNullOrEmpty(category.Description) ? (object)DBNull.Value : category.Description;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = category.IsActive;

                    con.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // 2627 هو كود الخطأ في SQL Server لما تحاول تدخل اسم موجود قبل كده (Unique Constraint)
                        if (ex.Number == 2627)
                            throw new Exception("اسم القسم موجود مسبقاً، برجاء اختيار اسم آخر.");
                        throw;
                    }
                }
            }
        }

        public void UpdateCategory(Category category)
        {
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                const string query = "UPDATE Categories SET Name = @Name, Description = @Description, IsActive = @IsActive WHERE Id = @Id;";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = category.Id;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = category.Name;
                    cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 255).Value = string.IsNullOrEmpty(category.Description) ? (object)DBNull.Value : category.Description;
                    cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = category.IsActive;

                    con.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                            throw new Exception("اسم القسم الجديد موجود مسبقاً لقسم آخر، برجاء تغييره.");
                        throw;
                    }
                }
            }
        }

        public void DeleteCategory(int categoryId)
        {
            using (SqlConnection con = DbConnection.CreateConnection())
            {
                const string checkQuery = "SELECT COUNT(Id) FROM Products WHERE CategoryId = @Id;";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                {
                    checkCmd.Parameters.Add("@Id", SqlDbType.Int).Value = categoryId;
                    con.Open();
                    int productCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (productCount > 0)
                    {
                        throw new Exception("لا يمكن حذف هذا القسم لارتباطه بمنتجات مسجلة في النظام.");
                    }
                }

                const string deleteQuery = "DELETE FROM Categories WHERE Id = @Id;";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, con))
                {
                    deleteCmd.Parameters.Add("@Id", SqlDbType.Int).Value = categoryId;
                    deleteCmd.ExecuteNonQuery();
                }
            }
        }
    }
}