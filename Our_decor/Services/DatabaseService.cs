using System;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Configuration;

namespace Our_decor.Services
{
    public class DatabaseService
    {
        private static DatabaseService instance;
        private readonly string connectionString;

        private DatabaseService()
        {
            // Читаем EF-строку из app.config
            var efConnection = ConfigurationManager.ConnectionStrings["decorDBEntities"];
            if (efConnection == null)
            {
                throw new ConfigurationErrorsException("Строка подключения 'decorDBEntities' не найдена в конфигурации");
            }

            // Через EntityConnectionStringBuilder берём внутреннюю строку SQL
            var efBuilder = new EntityConnectionStringBuilder(efConnection.ConnectionString);
            connectionString = efBuilder.ProviderConnectionString;
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

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("SELECT 1", connection))
                    {
                        await command.ExecuteScalarAsync();
                        Debug.WriteLine("Подключение успешно установлено");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка подключения: {ex.Message}");
                return false;
            }
        }

        public async Task<DataTable> ExecuteQueryAsync(string query, params SqlParameter[] parameters)
        {
            try
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
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            dataTable.Load(reader);
                        }
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка выполнения запроса: {ex.Message}");
                throw;
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string query, params SqlParameter[] parameters)
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка выполнения запроса: {ex.Message}");
                throw;
            }
        }

        public async Task<object> ExecuteScalarAsync(string query, params SqlParameter[] parameters)
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка выполнения скалярного запроса: {ex.Message}");
                throw;
            }
        }
    }
}
