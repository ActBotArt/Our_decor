using System;
using System.Drawing;
using System.Windows.Forms;

namespace Our_decor.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // Инициализация компонентов
            this.lblLogin = new Label();
            this.lblPassword = new Label();
            this.txtLogin = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();
            this.btnExit = new Button();
            this.pictureBoxLogo = new PictureBox();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();

            // PictureBox для логотипа
            this.pictureBoxLogo.Size = new Size(200, 100);
            this.pictureBoxLogo.Location = new Point(50, 20);
            this.pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;

            // Надпись "Логин:"
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new Point(40, 140);
            this.lblLogin.Text = "Логин:";
            this.lblLogin.Font = new Font("Gabriola", 14F);
            this.lblLogin.ForeColor = Color.FromArgb(45, 96, 51); // #2D6033
            this.lblLogin.BackColor = Color.FromArgb(187, 217, 178); // #BBD9B2

            // Поле ввода логина
            this.txtLogin.Location = new Point(130, 143);
            this.txtLogin.Size = new Size(140, 25);
            this.txtLogin.Font = new Font("Segoe UI", 9F);
            this.txtLogin.Text = "admin";

            // Надпись "Пароль:"
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new Point(40, 180);
            this.lblPassword.Text = "Пароль:";
            this.lblPassword.Font = new Font("Gabriola", 14F);
            this.lblPassword.ForeColor = Color.FromArgb(45, 96, 51); // #2D6033
            this.lblPassword.BackColor = Color.FromArgb(187, 217, 178); // #BBD9B2

            // Поле ввода пароля
            this.txtPassword.Location = new Point(130, 183);
            this.txtPassword.Size = new Size(140, 25);
            this.txtPassword.Font = new Font("Segoe UI", 9F);
            this.txtPassword.Text = "admin";
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.PasswordChar = '●';

            // Кнопка "Вход"
            this.btnLogin.Location = new Point(70, 230);
            this.btnLogin.Size = new Size(160, 35);
            this.btnLogin.Text = "Вход";
            this.btnLogin.BackColor = Color.FromArgb(45, 96, 51); // #2D6033
            this.btnLogin.ForeColor = Color.White;
            this.btnLogin.FlatStyle = FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.Font = new Font("Gabriola", 14F);
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);

            // Кнопка "Выход"
            this.btnExit.Location = new Point(70, 280);
            this.btnExit.Size = new Size(160, 35);
            this.btnExit.Text = "Выход";
            this.btnExit.BackColor = Color.FromArgb(45, 96, 51); // #2D6033
            this.btnExit.ForeColor = Color.White;
            this.btnExit.FlatStyle = FlatStyle.Flat;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.Font = new Font("Gabriola", 14F);
            this.btnExit.Click += new EventHandler(this.btnExit_Click);

            // Настройка формы
            this.ClientSize = new Size(300, 350);
            this.BackColor = Color.FromArgb(187, 217, 178); // #BBD9B2
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Авторизация";
            this.Font = new Font("Gabriola", 12F);

            // Добавление элементов на форму
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.lblLogin);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnExit);

            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Label lblLogin;
        private Label lblPassword;
        private TextBox txtLogin;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;
        private PictureBox pictureBoxLogo;
    }
}