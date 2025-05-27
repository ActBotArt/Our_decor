using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Our_decor.Models;
using Our_decor.Services;

namespace Our_decor.Forms
{
    public partial class ProductForm : Form
    {
        private readonly Product _product;
        private readonly DatabaseService _db;
        private bool _isNewProduct;
        private bool _isDragging = false;
        private Point _dragCursorPoint;
        private Point _dragFormPoint;

        public Product Result { get; private set; }

        public ProductForm(Product product = null)
        {
            InitializeComponent();
            _product = product;
            _db = DatabaseService.Instance;
            _isNewProduct = product == null;
            SetupForm();
            LoadProductTypes();
        }

        private void SetupForm()
        {
            // Настройка формы
            this.Text = _isNewProduct ? "Добавление продукции" : "Редактирование продукции";
            lblTitle.Text = this.Text;

            // Настройка внешнего вида
            this.BackColor = Color.FromArgb(187, 217, 178);
            panelTop.BackColor = Color.FromArgb(45, 96, 51);

            foreach (Button button in new[] { btnSave, btnCancel })
            {
                button.BackColor = Color.FromArgb(45, 96, 51);
                button.ForeColor = Color.White;
                button.Font = new Font("Gabriola", 14F);
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
            }

            foreach (Label label in new[] { lblArticle, lblType, lblName, lblDescription, lblMinCost, lblWidth })
            {
                label.Font = new Font("Gabriola", 14F);
                label.ForeColor = Color.FromArgb(45, 96, 51);
            }

            // Настройка полей ввода
            if (!_isNewProduct)
            {
                txtArticle.Text = _product.Article;
                txtArticle.ReadOnly = true;
                txtName.Text = _product.Name;
                txtDescription.Text = _product.Description;
                numMinCost.Value = _product.MinPartnerCost;
                numWidth.Value = _product.RollWidth;
            }
            else
            {
                txtArticle.Text = DateTime.Now.ToString("yyyyMMddHHmmss");
                // Устанавливаем минимальную цену по умолчанию
                numMinCost.Value = 0.01M;
                numWidth.Value = 0.01M;
            }

            // Настройка перетаскивания формы
            panelTop.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    _isDragging = true;
                    _dragCursorPoint = Cursor.Position;
                    _dragFormPoint = this.Location;
                }
            };

            panelTop.MouseMove += (s, e) =>
            {
                if (_isDragging)
                {
                    Point dif = Point.Subtract(Cursor.Position, new Size(_dragCursorPoint));
                    this.Location = Point.Add(_dragFormPoint, new Size(dif));
                }
            };

            panelTop.MouseUp += (s, e) => _isDragging = false;

            ValidateInput(null, EventArgs.Empty);
        }

        private async void LoadProductTypes()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var query = "SELECT * FROM ProductTypes";
                var dt = await _db.ExecuteQueryAsync(query);

                var types = new List<ProductType>();
                foreach (DataRow row in dt.Rows)
                {
                    types.Add(new ProductType
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        TypeName = row["TypeName"].ToString(),
                        Coefficient = Convert.ToDecimal(row["Coefficient"])
                    });
                }

                cmbType.DataSource = types;
                cmbType.DisplayMember = "TypeName";
                cmbType.ValueMember = "Id";

                if (!_isNewProduct)
                {
                    cmbType.SelectedValue = _product.ProductTypeId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке типов продукции: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // Проверка символов в названии - только буквы и цифры
        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Разрешаем управляющие символы (Backspace, Delete и т.д.)
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Разрешаем только буквы (русские и английские), цифры и пробел
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true; // Блокируем ввод недопустимого символа

                // Показываем предупреждение
                ShowNameError("Разрешены только буквы, цифры и пробелы!");

                // Автоматически скрываем сообщение через 3 секунды
                var timer = new Timer();
                timer.Interval = 3000;
                timer.Tick += (s, args) =>
                {
                    HideNameError();
                    timer.Stop();
                    timer.Dispose();
                };
                timer.Start();
            }
        }

        // Проверка названия на корректность
        private bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            // Проверяем, что в строке только буквы, цифры и пробелы
            Regex regex = new Regex(@"^[а-яёА-ЯЁa-zA-Z0-9\s]+$");
            return regex.IsMatch(name.Trim());
        }

        // Показать ошибку названия
        private void ShowNameError(string message)
        {
            lblNameError.Text = message;
            lblNameError.Visible = true;
            txtName.BackColor = Color.MistyRose;
        }

        // Скрыть ошибку названия
        private void HideNameError()
        {
            lblNameError.Visible = false;
            lblNameError.Text = "";
        }

        // Показать ошибку цены
        private void ShowPriceError(string message)
        {
            lblPriceError.Text = message;
            lblPriceError.Visible = true;
            numMinCost.BackColor = Color.MistyRose;
        }

        // Скрыть ошибку цены
        private void HidePriceError()
        {
            lblPriceError.Visible = false;
            lblPriceError.Text = "";
        }

        private void ValidateInput(object sender, EventArgs e)
        {
            bool isValid = true;

            // Проверка артикула
            if (string.IsNullOrWhiteSpace(txtArticle.Text))
            {
                isValid = false;
                txtArticle.BackColor = Color.MistyRose;
            }
            else
            {
                txtArticle.BackColor = SystemColors.Window;
            }

            // Проверка названия
            if (!IsValidName(txtName.Text))
            {
                isValid = false;
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    ShowNameError("Название не может быть пустым!");
                }
                else
                {
                    ShowNameError("Недопустимые символы в названии!");
                }
            }
            else
            {
                txtName.BackColor = SystemColors.Window;
                HideNameError();
            }

            // Проверка типа продукта
            if (cmbType.SelectedItem == null)
            {
                isValid = false;
                cmbType.BackColor = Color.MistyRose;
            }
            else
            {
                cmbType.BackColor = SystemColors.Window;
            }

            // Проверка минимальной стоимости - ОБЯЗАТЕЛЬНО ДОЛЖНА БЫТЬ ЦЕНА!
            if (numMinCost.Value <= 0)
            {
                isValid = false;
                ShowPriceError("Цена обязательна!");
            }
            else if (numMinCost.Value < 0.01M)
            {
                isValid = false;
                ShowPriceError("Минимальная цена 0.01!");
            }
            else
            {
                numMinCost.BackColor = SystemColors.Window;
                HidePriceError();
            }

            // Проверка ширины рулона
            if (numWidth.Value <= 0 || numWidth.Value > 10)
            {
                isValid = false;
                numWidth.BackColor = Color.MistyRose;
            }
            else
            {
                numWidth.BackColor = SystemColors.Window;
            }

            btnSave.Enabled = isValid;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            // Дополнительная проверка перед сохранением
            if (!IsValidName(txtName.Text))
            {
                MessageBox.Show("Название продукта содержит недопустимые символы!\nРазрешены только буквы, цифры и пробелы.",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (numMinCost.Value <= 0)
            {
                MessageBox.Show("Цена продукта обязательна и должна быть больше нуля!",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numMinCost.Focus();
                return;
            }

            try
            {
                btnSave.Enabled = false;
                Cursor = Cursors.WaitCursor;

                var product = _isNewProduct ? new Product() : _product;
                product.Article = txtArticle.Text.Trim();
                product.ProductTypeId = ((ProductType)cmbType.SelectedItem).Id;
                product.Name = txtName.Text.Trim();
                product.Description = txtDescription.Text.Trim();
                product.MinPartnerCost = numMinCost.Value;
                product.RollWidth = numWidth.Value;
                product.WorkshopNumber = 1; // По умолчанию
                product.WorkersCount = 1;   // По умолчанию

                if (_isNewProduct)
                {
                    var query = @"INSERT INTO Products 
                        (Article, ProductTypeId, Name, Description, MinPartnerCost, RollWidth, WorkshopNumber, WorkersCount)
                        VALUES (@Article, @ProductTypeId, @Name, @Description, @MinPartnerCost, @RollWidth, @WorkshopNumber, @WorkersCount);
                        SELECT SCOPE_IDENTITY();";

                    var parameters = new[]
                    {
                        new SqlParameter("@Article", product.Article),
                        new SqlParameter("@ProductTypeId", product.ProductTypeId),
                        new SqlParameter("@Name", product.Name),
                        new SqlParameter("@Description", (object)product.Description ?? DBNull.Value),
                        new SqlParameter("@MinPartnerCost", product.MinPartnerCost),
                        new SqlParameter("@RollWidth", product.RollWidth),
                        new SqlParameter("@WorkshopNumber", product.WorkshopNumber),
                        new SqlParameter("@WorkersCount", product.WorkersCount)
                    };

                    var result = await _db.ExecuteScalarAsync(query, parameters);
                    product.Id = Convert.ToInt32(result);
                }
                else
                {
                    var query = @"UPDATE Products SET 
                        ProductTypeId = @ProductTypeId,
                        Name = @Name,
                        Description = @Description,
                        MinPartnerCost = @MinPartnerCost,
                        RollWidth = @RollWidth,
                        WorkshopNumber = @WorkshopNumber,
                        WorkersCount = @WorkersCount
                        WHERE Id = @Id";

                    var parameters = new[]
                    {
                        new SqlParameter("@Id", product.Id),
                        new SqlParameter("@ProductTypeId", product.ProductTypeId),
                        new SqlParameter("@Name", product.Name),
                        new SqlParameter("@Description", (object)product.Description ?? DBNull.Value),
                        new SqlParameter("@MinPartnerCost", product.MinPartnerCost),
                        new SqlParameter("@RollWidth", product.RollWidth),
                        new SqlParameter("@WorkshopNumber", product.WorkshopNumber),
                        new SqlParameter("@WorkersCount", product.WorkersCount)
                    };

                    await _db.ExecuteNonQueryAsync(query, parameters);
                }

                MessageBox.Show("Продукт успешно сохранен!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Result = product;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnSave.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                btnCancel_Click(this, EventArgs.Empty);
                return true;
            }
            else if (keyData == Keys.Enter && btnSave.Enabled)
            {
                btnSave_Click(this, EventArgs.Empty);
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}