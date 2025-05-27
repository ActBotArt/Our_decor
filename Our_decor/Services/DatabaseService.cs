using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Our_decor.Services
{
    public class DatabaseService
    {
        private static DatabaseService instance;
        private readonly string connectionString;

        private DatabaseService()
        {
            connectionString = "Server=.\\SQLEXPRESS;Database=decorDB;Trusted_Connection=True;";
        }

        public static DatabaseService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatabaseService();
                }
                return instance;
            }
        }

        public async Task<DataTable> ExecuteQueryAsync(string query, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    var dataTable = new DataTable();
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        await Task.Run(() => adapter.Fill(dataTable));
                    }
                    return dataTable;
                }
            }
        }

        public async Task<object> ExecuteScalarAsync(string query, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return await command.ExecuteScalarAsync();
                }
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string query, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}