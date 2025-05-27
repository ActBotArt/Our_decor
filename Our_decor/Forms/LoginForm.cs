using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Diagnostics;
using Our_decor.Services;

namespace Our_decor.Forms
{
    public partial class LoginForm : Form
    {
        private readonly AuthService authService;

        public LoginForm()
        {
            InitializeComponent();
            LoadImages();
            authService = AuthService.Instance;
        }

        private void LoadImages()
        {
            try
            {
                string iconPath = Path.Combine(Application.StartupPath, "..", "..", "Resources", "logo.ico");
                if (File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                }

                string logoPath = Path.Combine(Application.StartupPath, "..", "..", "Resources", "logo.png");
                if (File.Exists(logoPath))
                {
                    pictureBoxLogo.Image = Image.FromFile(logoPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке изображений: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                btnLogin.Enabled = false;
                Cursor = Cursors.WaitCursor;

                string login = txtLogin.Text.Trim();
                string password = txtPassword.Text.Trim();

                Debug.WriteLine($"Попытка входа:");
                Debug.WriteLine($"Логин: '{login}'");
                Debug.WriteLine($"Пароль: '{password}'");

                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Введите логин и пароль!",
                        "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверяем подключение к БД
                if (!await DatabaseService.Instance.TestConnectionAsync())
                {
                    MessageBox.Show("Нет подключения к базе данных!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (await authService.ValidateUserAsync(login, password))
                {
                    string role = await authService.GetUserRoleAsync(login);
                    Debug.WriteLine($"Успешный вход. Роль: {role}");
                    this.Hide();
                    new MainForm(role).ShowDialog();
                    this.Close();
                }
                else
                {
                    Debug.WriteLine("Неудачная попытка входа");
                    MessageBox.Show("Неверный логин или пароль!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Text = "";
                    txtLogin.Focus();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Исключение при входе: {ex}");
                MessageBox.Show($"Ошибка при входе: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(this, EventArgs.Empty);
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                btnExit_Click(this, EventArgs.Empty);
            }
        }
    }
}