using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
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

                if (await authService.ValidateUserAsync(txtLogin.Text, txtPassword.Text))
                {
                    string role = await authService.GetUserRoleAsync(txtLogin.Text);
                    this.Hide();
                    new MainForm(role).ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Text = "";
                    txtLogin.Focus();
                }
            }
            catch (Exception ex)
            {
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