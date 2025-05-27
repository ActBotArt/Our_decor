using System;
using System.Data;
using System.Data.SqlClient;

namespace Our_decor.Models
{
    public class ApplicationContext
    {
        private readonly string _connectionString;

        public ApplicationContext()
        {
            _connectionString = "Server=.\\SQLEXPRESS;Database=decorDB;Trusted_Connection=True;";
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}