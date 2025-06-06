﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Our_decor.Models;
using Our_decor.Services;

namespace Our_decor.Forms
{
    public partial class MainForm : Form
    {
        private readonly string _userRole;
        private List<Product> _products;
        private readonly DatabaseService _db;
        private bool _isDragging = false;
        private Point _dragCursorPoint;
        private Point _dragFormPoint;

        public MainForm(string role = "User")
        {
            InitializeComponent();
            _userRole = role;
            _db = DatabaseService.Instance;

            dataGridView.RowsDefaultCellStyle.Font = new Font("Gabriola", 12F);
            dataGridView.CellFormatting += dataGridView_CellFormatting;

            ConfigureForm();
            LoadImages();
            _ = LoadDataAsync();
        }

        private void LoadImages()
        {
            try
            {
                string iconPath = Path.Combine(Application.StartupPath, "..", "..", "Resources", "logo.ico");
                if (File.Exists(iconPath)) this.Icon = new Icon(iconPath);

                string logoPath = Path.Combine(Application.StartupPath, "..", "..", "Resources", "logo.png");
                if (File.Exists(logoPath))
                {
                    pictureBoxLogo.Image = Image.FromFile(logoPath);
                    pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка загрузки изображений: {ex.Message}");
            }
        }

        private void ConfigureForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = $"Our Decor - {_userRole}";
            this.BackColor = Color.White;
            this.MinimumSize = new Size(1000, 600);

            panelTop.BackColor = Color.FromArgb(187, 217, 178);
            panelTop.MouseDown += PanelTop_MouseDown;
            panelTop.MouseMove += PanelTop_MouseMove;
            panelTop.MouseUp += PanelTop_MouseUp;

            ConfigureButtons();
            ConfigureSearch();
            SetupDataGridView();
            SetupAccess();
        }

        private void ConfigureButtons()
        {
            foreach (Button btn in new[] { btnAdd, btnEdit, btnDelete, btnExit, btnMaterials })
            {
                btn.BackColor = Color.FromArgb(45, 96, 51);
                btn.ForeColor = Color.White;
                btn.Font = new Font("Gabriola", 14F);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Cursor = Cursors.Hand;
                btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 130, 70);
                btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(35, 75, 40);
            }
        }

        private void ConfigureSearch()
        {
            lblSearch.Font = new Font("Gabriola", 14F);
            lblSearch.ForeColor = Color.FromArgb(45, 96, 51);
            lblSearch.Size = new Size(60, 35);
            lblSearch.Location = new Point(520, 17);
            lblSearch.Text = "Поиск:";

            txtSearch.Font = new Font("Gabriola", 12F);
            txtSearch.Size = new Size(300, 35);
            txtSearch.Location = new Point(580, 15);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.TextChanged += txtSearch_TextChanged;
        }

        private void SetupDataGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ReadOnly = true;
            dataGridView.RowHeadersVisible = false;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.GridColor = Color.FromArgb(187, 217, 178);
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // Заголовки
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 96, 51);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Gabriola", 14F, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.ColumnHeadersHeight = 40;
            dataGridView.EnableHeadersVisualStyles = false;

            // Строки
            dataGridView.RowTemplate.Height = 35;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 247, 240);
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(187, 217, 178);
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.FromArgb(45, 96, 51);

            var baseStyle = new DataGridViewCellStyle
            {
                Font = new Font("Gabriola", 12F),
                BackColor = Color.White,
                SelectionBackColor = Color.FromArgb(187, 217, 178),
                SelectionForeColor = Color.FromArgb(45, 96, 51),
                Padding = new Padding(5)
            };

            // Очищаем прежние колонки
            dataGridView.Columns.Clear();

            // 1) Столбец «Артикул» (привязка к свойству Article)
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Article",
                HeaderText = "Артикул",
                DataPropertyName = "Article",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(209, 238, 197)
                }
            });

            // 2) НЕПРИВЯЗАННЫЙ столбец «Тип продукта» — будем заполнять вручную
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TypeName",
                HeaderText = "Тип продукта",
                // НЕ указываем DataPropertyName, т.к. будем заполнять ячейки вручную
                Width = 200,
                DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 0, 0, 0)
                }
            });

            // 3) Столбец «Наименование» (привязка к свойству Name)
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Наименование",
                DataPropertyName = "Name",
                Width = 300,
                DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 0, 0, 0)
                }
            });

            // 4) «Расчетная стоимость»
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CalculatedCost",
                HeaderText = "Расчетная стоимость",
                DataPropertyName = "CalculatedCost",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N2",
                    Padding = new Padding(0, 0, 10, 0)
                }
            });

            // 5) «Мин. стоимость»
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MinPartnerCost",
                HeaderText = "Мин. стоимость",
                DataPropertyName = "MinPartnerCost",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N2",
                    Padding = new Padding(0, 0, 10, 0)
                }
            });

            // 6) «Ширина рулона»
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RollWidth",
                HeaderText = "Ширина рулона",
                DataPropertyName = "RollWidth",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N2",
                    Padding = new Padding(0, 0, 10, 0)
                }
            });

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                // Форматируем только числовые столбцы «Мин. стоимость» и «Расчетная стоимость»
                if ((e.ColumnIndex == dataGridView.Columns["MinPartnerCost"].Index ||
                     e.ColumnIndex == dataGridView.Columns["CalculatedCost"].Index) &&
                    e.Value != null)
                {
                    if (decimal.TryParse(e.Value.ToString(), out decimal value))
                    {
                        e.Value = $"{value:N2} ₽";
                        e.FormattingApplied = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка форматирования ячейки: {ex.Message}");
            }
        }

        private void SetupAccess()
        {
            btnAdd.Visible = _userRole != "User";
            btnEdit.Visible = _userRole != "User";
            btnDelete.Visible = _userRole == "Admin";
            btnMaterials.Visible = true;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Запрос сразу возвращает TypeName и остальные поля
                var query = @"
                    SELECT 
                        p.Id,
                        p.Article,
                        p.ProductTypeId,
                        p.Name,
                        p.Description,
                        p.MinPartnerCost,
                        p.RollWidth,
                        p.ProductionTime,
                        p.WorkshopNumber,
                        p.WorkersCount,
                        ISNULL(pt.TypeName, 'Не указан') as TypeName,
                        ISNULL(pt.Coefficient, 0) as Coefficient,
                        ISNULL((
                            SELECT SUM(m.Cost * pm.Quantity)
                            FROM ProductMaterials pm
                            JOIN Materials m ON pm.MaterialId = m.Id
                            WHERE pm.ProductId = p.Id
                        ), 0) as CalculatedCost
                    FROM Products p 
                    LEFT JOIN ProductTypes pt ON p.ProductTypeId = pt.Id
                    ORDER BY p.Article";

                DataTable dt = await _db.ExecuteQueryAsync(query);
                _products = new List<Product>();

                foreach (DataRow row in dt.Rows)
                {
                    var prod = new Product
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Article = row["Article"].ToString(),
                        ProductTypeId = Convert.ToInt32(row["ProductTypeId"]),
                        Name = row["Name"].ToString(),
                        Description = row["Description"]?.ToString(),
                        MinPartnerCost = Convert.ToDecimal(row["MinPartnerCost"]),
                        RollWidth = Convert.ToDecimal(row["RollWidth"]),
                        ProductionTime = Convert.ToInt32(row["ProductionTime"]),
                        WorkshopNumber = Convert.ToInt32(row["WorkshopNumber"]),
                        WorkersCount = Convert.ToInt32(row["WorkersCount"]),
                        CalculatedCost = Convert.ToDecimal(row["CalculatedCost"]),
                        // Сохраняем только TypeName и Coefficient в ProductType, чтобы потом вывести TypeName
                        ProductType = new ProductType
                        {
                            Id = Convert.ToInt32(row["ProductTypeId"]),
                            TypeName = row["TypeName"].ToString(),
                            Coefficient = Convert.ToDecimal(row["Coefficient"])
                        }
                    };
                    _products.Add(prod);
                }

                // Привязываем список к DataGridView
                dataGridView.DataSource = null;
                dataGridView.DataSource = _products;

                // ПОСЛЕ того, как DataSource установлен, заполняем вручную ячейку "TypeName"
                for (int i = 0; i < _products.Count; i++)
                {
                    var typeName = _products[i].ProductType.TypeName;
                    dataGridView.Rows[i].Cells["TypeName"].Value = typeName;
                }

                UpdateTotalLabel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка загрузки данных: {ex.Message}");
                MessageBox.Show(
                    "Произошла ошибка при загрузке данных.\nПроверьте подключение к базе данных.",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void UpdateTotalLabel()
        {
            var source = dataGridView.DataSource as List<Product>;
            lblTotal.Text = $"Всего продукции: {source?.Count ?? 0}";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_products == null) return;

            try
            {
                var searchText = txtSearch.Text.ToLower().Trim();
                var filtered = string.IsNullOrEmpty(searchText)
                    ? _products
                    : _products.Where(p =>
                        p.Article.ToLower().Contains(searchText) ||
                        p.Name.ToLower().Contains(searchText) ||
                        p.ProductType.TypeName.ToLower().Contains(searchText))
                    .ToList();

                dataGridView.DataSource = null;
                dataGridView.DataSource = filtered;

                // Снова заполняем вручную столбец TypeName в отфильтрованных данных
                for (int i = 0; i < filtered.Count; i++)
                {
                    dataGridView.Rows[i].Cells["TypeName"].Value = filtered[i].ProductType.TypeName;
                }

                UpdateTotalLabel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка поиска: {ex.Message}");
            }
        }

        private void PanelTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _dragCursorPoint = Cursor.Position;
                _dragFormPoint = this.Location;
            }
        }

        private void PanelTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(_dragCursorPoint));
                this.Location = Point.Add(_dragFormPoint, new Size(diff));
            }
        }

        private void PanelTop_MouseUp(object sender, MouseEventArgs e)
        {
            _isDragging = false;
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (_userRole == "User")
            {
                MessageBox.Show(
                    "У вас нет прав для добавления продукции",
                    "Доступ запрещен",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            using (var form = new ProductForm())
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    await LoadDataAsync();
                }
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (_userRole == "User")
            {
                MessageBox.Show(
                    "У вас нет прав для редактирования продукции",
                    "Доступ запрещен",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Выберите продукцию для редактирования",
                    "Внимание",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var product = (Product)dataGridView.SelectedRows[0].DataBoundItem;
            using (var form = new ProductForm(product))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    await LoadDataAsync();
                }
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (_userRole != "Admin")
            {
                MessageBox.Show(
                    "У вас нет прав для удаления продукции",
                    "Доступ запрещен",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Выберите продукцию для удаления",
                    "Внимание",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var product = (Product)dataGridView.SelectedRows[0].DataBoundItem;
            if (MessageBox.Show(
                    $"Вы действительно хотите удалить продукцию?\n\nАртикул: {product.Article}\nНаименование: {product.Name}",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    var query = "DELETE FROM Products WHERE Id = @Id";
                    var parameter = new System.Data.SqlClient.SqlParameter("@Id", product.Id);
                    await _db.ExecuteNonQueryAsync(query, parameter);
                    await LoadDataAsync();
                    MessageBox.Show(
                        "Продукция успешно удалена",
                        "Информация",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка удаления: {ex.Message}");
                    MessageBox.Show(
                        $"Ошибка удаления: {ex.Message}",
                        "Ошибка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void btnMaterials_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Выберите продукцию для просмотра материалов",
                    "Внимание",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var product = (Product)dataGridView.SelectedRows[0].DataBoundItem;
            using (var form = new MaterialsForm(product, _userRole))
            {
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(
                    "Вы действительно хотите выйти?",
                    "Подтверждение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
