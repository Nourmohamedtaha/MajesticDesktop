using MajesticDesktop.Database;
using MajesticDesktop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MajesticDesktop.Services
{
    public class ProductService
    {
        public ProductService()
        {
            EnsureSchemaUpdated();
        }

        private void EnsureSchemaUpdated()
        {
            const string query = @"
                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Products') AND name = 'IsActive')
                BEGIN ALTER TABLE Products ADD IsActive BIT NOT NULL CONSTRAINT DF_Products_IsActive DEFAULT 1; END

                IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Products') AND name = 'Notes')
                BEGIN ALTER TABLE Products ADD Notes NVARCHAR(500) NULL; END";

            using (var connection = DbConnection.CreateConnection())
            using (var command = new SqlCommand(query, connection))
            {
                try { connection.Open(); command.ExecuteNonQuery(); } catch { }
            }
        }

        public void AddProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (string.IsNullOrWhiteSpace(product.Name)) throw new ArgumentException("اسم المنتج مطلوب.");
            if (product.CategoryId <= 0) throw new ArgumentException("يجب اختيار القسم.");
            if (product.PurchasePrice < 0) throw new ArgumentException("سعر الشراء لا يمكن أن يكون سالبًا.");

            const string query = @"
                INSERT INTO Products (CategoryId, Name, PurchasePrice, ImagePath, IsActive, AlertLimit, Notes)
                VALUES (@CategoryId, @Name, @PurchasePrice, @ImagePath, @IsActive, @AlertLimit, @Notes);
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                using (SqlTransaction trans = connection.BeginTransaction())
                {
                    try
                    {
                        int newId = 0;
                        using (SqlCommand command = new SqlCommand(query, connection, trans))
                        {
                            command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = product.CategoryId;
                            command.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = product.Name.Trim();
                            command.Parameters.Add("@PurchasePrice", SqlDbType.Decimal).Value = product.PurchasePrice;
                            command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(product.ImagePath) ? (object)DBNull.Value : product.ImagePath.Trim();
                            command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = product.IsActive;
                            command.Parameters.Add("@AlertLimit", SqlDbType.Int).Value = product.AlertLimit;
                            command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(product.Notes) ? (object)DBNull.Value : product.Notes.Trim();

                            newId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        // إضافة الكمية الافتتاحية للمخزن الرئيسي فوراً
                        if (product.CurrentQuantity > 0)
                        {
                            const string stockQuery = "INSERT INTO Stock (LocationId, ProductId, Quantity) VALUES (1, @ProductId, @Qty);";
                            using (SqlCommand cmdStock = new SqlCommand(stockQuery, connection, trans))
                            {
                                cmdStock.Parameters.Add("@ProductId", SqlDbType.Int).Value = newId;
                                cmdStock.Parameters.Add("@Qty", SqlDbType.Decimal).Value = product.CurrentQuantity;
                                cmdStock.ExecuteNonQuery();
                            }
                        }
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            const string query = @"
                SELECT 
                    p.Id, 
                    p.CategoryId, 
                    p.Name, 
                    p.PurchasePrice, 
                    p.ImagePath, 
                    p.IsActive, 
                    p.Notes,
                    ISNULL(p.AlertLimit, 5) AS AlertLimit,
                    ISNULL(SUM(s.Quantity), 0) AS CurrentQuantity
                FROM Products p
                LEFT JOIN Stock s ON p.Id = s.ProductId AND s.LocationId = 1
                WHERE p.BranchId IS NULL
                GROUP BY p.Id, p.CategoryId, p.Name, p.PurchasePrice, p.ImagePath, p.IsActive, p.AlertLimit, p.Notes
                ORDER BY p.Id ASC;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            CategoryId = Convert.ToInt32(reader["CategoryId"]),
                            Name = reader["Name"].ToString(),
                            PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]),
                            ImagePath = reader["ImagePath"] == DBNull.Value ? null : reader["ImagePath"].ToString(),
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            AlertLimit = Convert.ToInt32(reader["AlertLimit"]),
                            Notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString(),
                            CurrentQuantity = Convert.ToDecimal(reader["CurrentQuantity"])
                        });
                    }
                }
            }
            return products;
        }

        public void UpdateProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (product.Id <= 0) throw new ArgumentException("رقم المنتج غير صحيح.");
            if (string.IsNullOrWhiteSpace(product.Name)) throw new ArgumentException("اسم المنتج مطلوب.");
            if (product.CategoryId <= 0) throw new ArgumentException("يجب اختيار القسم.");
            if (product.PurchasePrice < 0) throw new ArgumentException("سعر الشراء لا يمكن أن يكون سالبًا.");

            const string query = @"
                UPDATE Products
                SET CategoryId = @CategoryId, Name = @Name, PurchasePrice = @PurchasePrice, 
                    ImagePath = @ImagePath, AlertLimit = @AlertLimit, IsActive = @IsActive, Notes = @Notes
                WHERE Id = @Id;";

            using (SqlConnection connection = DbConnection.CreateConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@Id", SqlDbType.Int).Value = product.Id;
                command.Parameters.Add("@CategoryId", SqlDbType.Int).Value = product.CategoryId;
                command.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = product.Name.Trim();
                command.Parameters.Add("@PurchasePrice", SqlDbType.Decimal).Value = product.PurchasePrice;
                command.Parameters.Add("@ImagePath", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(product.ImagePath) ? (object)DBNull.Value : product.ImagePath.Trim();
                command.Parameters.Add("@AlertLimit", SqlDbType.Int).Value = product.AlertLimit;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = product.IsActive;
                command.Parameters.Add("@Notes", SqlDbType.NVarChar, 500).Value = string.IsNullOrWhiteSpace(product.Notes) ? (object)DBNull.Value : product.Notes.Trim();

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(int productId)
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                string checkQuery = @"
                    IF EXISTS (SELECT 1 FROM sysobjects WHERE name='Stock' AND xtype='U')
                    BEGIN SELECT ISNULL(SUM(Quantity), 0) FROM Stock WHERE ProductId = @Id END
                    ELSE BEGIN SELECT 0 END";

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.Add("@Id", SqlDbType.Int).Value = productId;
                    int stockQty = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (stockQty > 0)
                        throw new Exception("لا يمكن حذف هذا المنتج لوجود رصيد حالي له في المخزن.");
                }

                string deleteQuery = "DELETE FROM Products WHERE Id = @Id;";
                using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection))
                {
                    deleteCmd.Parameters.Add("@Id", SqlDbType.Int).Value = productId;
                    deleteCmd.ExecuteNonQuery();
                }
            }
        }
    }
}