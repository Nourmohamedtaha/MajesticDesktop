using System.Data.SqlClient;

namespace MajesticDesktop.Database
{
    public static class DbConnection
    {
        private static readonly string connectionString =
            @"Server=DESKTOP-RTI4M70;
              Database=MajesticDB;
              User ID=sa;
            password=Nour@122;";

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}