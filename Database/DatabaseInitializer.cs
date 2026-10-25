using System.Data.SqlClient;

namespace MajesticDesktop.Database
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            CreateCategoriesTable();
            CreateProductsTable();
            CreateLocationsTable();
            CreateStockTable();
            CreateRequestsTable();
            CreateRequestDetailsTable();
            CreateSuppliersTable();
            CreatePurchasesTable();
            CreatePurchaseDetailsTable();
        }

        private static void CreateCategoriesTable()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                string query = @"
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Categories')
                    BEGIN
                        CREATE TABLE Categories (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Name NVARCHAR(100) NOT NULL,
                            Description NVARCHAR(500) NULL,
                            IsActive BIT NOT NULL DEFAULT 1
                        )
                    END";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CreateProductsTable()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                string query = @"
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Products')
                    BEGIN
                        CREATE TABLE Products (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Name NVARCHAR(150) NOT NULL,
                            PurchasePrice DECIMAL(18,2) NOT NULL,
                            ImagePath NVARCHAR(500) NULL,
                            CategoryId INT NOT NULL,
                            IsActive BIT NOT NULL DEFAULT 1,
                            CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
                        )
                    END";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CreateLocationsTable()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                string query = @"
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Locations')
                    BEGIN
                        CREATE TABLE Locations (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Name NVARCHAR(150) NOT NULL,
                            Type NVARCHAR(50) NOT NULL
                        )
                    END";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CreateStockTable()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                string query = @"
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Stock')
                    BEGIN
                        CREATE TABLE Stock (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            ProductId INT NOT NULL,
                            LocationId INT NOT NULL,
                            Quantity INT NOT NULL DEFAULT 0,
                            CONSTRAINT FK_Stock_Products FOREIGN KEY (ProductId) REFERENCES Products(Id),
                            CONSTRAINT FK_Stock_Locations FOREIGN KEY (LocationId) REFERENCES Locations(Id)
                        )
                    END";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CreateRequestsTable()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                string query = @"
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Requests')
                    BEGIN
                        CREATE TABLE Requests (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            LocationId INT NOT NULL,
                            RequestDate DATETIME DEFAULT GETDATE(),
                            Status NVARCHAR(50) DEFAULT N'قيد الانتظار',
                            CONSTRAINT FK_Requests_Locations FOREIGN KEY (LocationId) REFERENCES Locations(Id)
                        )
                    END";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CreateRequestDetailsTable()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                string query = @"
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'RequestDetails')
                    BEGIN
                        CREATE TABLE RequestDetails (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            RequestId INT NOT NULL,
                            ProductId INT NOT NULL,
                            RequestedQuantity INT NOT NULL,
                            CONSTRAINT FK_RequestDetails_Requests FOREIGN KEY (RequestId) REFERENCES Requests(Id),
                            CONSTRAINT FK_RequestDetails_Products FOREIGN KEY (ProductId) REFERENCES Products(Id)
                        )
                    END";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CreateSuppliersTable()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                string query = @"
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Suppliers')
                    BEGIN
                        CREATE TABLE Suppliers (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            Name NVARCHAR(150) NOT NULL,
                            Phone NVARCHAR(50) NULL,
                            Balance DECIMAL(18,2) NOT NULL DEFAULT 0,
                            IsActive BIT NOT NULL DEFAULT 1
                        )
                    END";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CreatePurchasesTable()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                string query = @"
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Purchases')
                    BEGIN
                        CREATE TABLE Purchases (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            InvoiceNumber NVARCHAR(50) NOT NULL,
                            SupplierId INT NOT NULL,
                            LocationId INT NOT NULL,
                            InvoiceDate DATETIME NOT NULL DEFAULT GETDATE(),
                            TotalAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
                            PaidAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
                            RemainingAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
                            CONSTRAINT FK_Purchases_Suppliers FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id),
                            CONSTRAINT FK_Purchases_Locations FOREIGN KEY (LocationId) REFERENCES Locations(Id)
                        )
                    END";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CreatePurchaseDetailsTable()
        {
            using (SqlConnection connection = DbConnection.CreateConnection())
            {
                connection.Open();
                string query = @"
                    IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PurchaseDetails')
                    BEGIN
                        CREATE TABLE PurchaseDetails (
                            Id INT IDENTITY(1,1) PRIMARY KEY,
                            PurchaseId INT NOT NULL,
                            ProductId INT NOT NULL,
                            Quantity INT NOT NULL,
                            UnitPrice DECIMAL(18,2) NOT NULL,
                            TotalPrice DECIMAL(18,2) NOT NULL,
                            CONSTRAINT FK_PurchaseDetails_Purchases FOREIGN KEY (PurchaseId) REFERENCES Purchases(Id),
                            CONSTRAINT FK_PurchaseDetails_Products FOREIGN KEY (ProductId) REFERENCES Products(Id)
                        )
                    END";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}