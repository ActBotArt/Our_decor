using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Our_decor.Services
{
    public class AuthService
    {
        private static AuthService instance;
        private readonly DatabaseService databaseService;

        private AuthService()
        {
            databaseService = DatabaseService.Instance;
        }

        public static AuthService Instance
        {
            get
            {
                if (instance == null)
                    instance = new AuthService();
                return instance;
            }
        }

        public async Task<bool> ValidateUserAsync(string login, string password)
        {
            try
            {
                var query = "SELECT COUNT(*) FROM Users WHERE Login = @Login AND Password = @Password";
                var parameters = new[]
                {
                    new SqlParameter("@Login", login),
                    new SqlParameter("@Password", password)
                };

                var result = await databaseService.ExecuteScalarAsync(query, parameters);
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> GetUserRoleAsync(string login)
        {
            try
            {
                var query = "SELECT Role FROM Users WHERE Login = @Login";
                var parameter = new SqlParameter("@Login", login);

                var result = await databaseService.ExecuteScalarAsync(query, parameter);
                return result?.ToString() ?? "User";
            }
            catch (Exception)
            {
                return "User";
            }
        }
    }
}