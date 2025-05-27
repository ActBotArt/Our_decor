using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Our_decor.Services
{
    public class AuthService
    {
        private static AuthService instance;
        private readonly DatabaseService _db;

        private AuthService()
        {
            _db = DatabaseService.Instance;
        }

        public static AuthService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AuthService();
                }
                return instance;
            }
        }

        public async Task<bool> ValidateUserAsync(string login, string password)
        {
            try
            {
                Debug.WriteLine("=== Начало процесса аутентификации ===");
                Debug.WriteLine($"Введенный логин: '{login}'");
                Debug.WriteLine($"Введенный пароль: '{password}'");

                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    Debug.WriteLine("Ошибка: пустой логин или пароль");
                    return false;
                }

                var query = @"SELECT COUNT(1) FROM Users 
                            WHERE Login = @Login AND Password = @Password";

                var parameters = new[]
                {
                    new SqlParameter
                    {
                        ParameterName = "@Login",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 50,
                        Value = login.Trim()
                    },
                    new SqlParameter
                    {
                        ParameterName = "@Password",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 100,
                        Value = password.Trim()
                    }
                };

                Debug.WriteLine("Выполнение запроса к базе данных...");

                // Сначала проверим подключение
                if (!await _db.TestConnectionAsync())
                {
                    Debug.WriteLine("Ошибка: нет подключения к базе данных");
                    return false;
                }

                // Выполняем запрос
                var result = await _db.ExecuteQueryAsync(query, parameters);

                if (result != null && result.Rows.Count > 0)
                {
                    var count = Convert.ToInt32(result.Rows[0][0]);
                    Debug.WriteLine($"Результат запроса: найдено {count} совпадений");

                    // Для отладки выведем всех пользователей
                    var usersQuery = "SELECT Login, Password, Role FROM Users";
                    var usersResult = await _db.ExecuteQueryAsync(usersQuery);
                    Debug.WriteLine("Список пользователей в базе:");
                    foreach (DataRow row in usersResult.Rows)
                    {
                        Debug.WriteLine($"Логин: {row["Login"]}, Пароль: {row["Password"]}, Роль: {row["Role"]}");
                    }

                    return count > 0;
                }

                Debug.WriteLine("Ошибка: результат запроса null или пустой");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Исключение при аутентификации: {ex}");
                return false;
            }
            finally
            {
                Debug.WriteLine("=== Конец процесса аутентификации ===");
            }
        }

        public async Task<string> GetUserRoleAsync(string login)
        {
            try
            {
                var query = "SELECT Role FROM Users WHERE Login = @Login";
                var parameter = new SqlParameter("@Login", SqlDbType.NVarChar, 50) { Value = login.Trim() };

                var result = await _db.ExecuteQueryAsync(query, parameter);

                if (result != null && result.Rows.Count > 0)
                {
                    var role = result.Rows[0]["Role"].ToString();
                    Debug.WriteLine($"Получена роль для пользователя {login}: {role}");
                    return role;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при получении роли: {ex}");
            }

            return "User";
        }
    }
}