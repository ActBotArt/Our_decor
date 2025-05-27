using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;
using Our_decor.Models;
using Our_decor.Services;
using System.IO;
using System.Diagnostics;
using System.Data;

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

            dataGridView.CellFormatting += dataGridView_CellFormatting;
            dataGridView.RowsDefaultCellStyle.Font = new Font("Gabriola", 12F);

            ConfigureForm();
            LoadImages();
            _ = LoadDataAsync();
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
            foreach (Button button in new[] { btnAdd, btnEdit, btnDelete, btnExit, btnMaterials })
            {
                button.BackColor = Color.FromArgb(45, 96, 51);
                button.ForeColor = Color.White;
                button.Font = new Font("Gabriola", 14F);
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.Cursor = Cursors.Hand;
                button.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 130, 70);
                button.FlatAppearance.MouseDownBackColor = Color.FromArgb(35, 75, 40);
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
            dataGridView.Font = new Font("Gabriola", 12F);
            dataGridView.GridColor = Color.FromArgb(187, 217, 178);
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            // Настройка заголовков
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 96, 51);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Gabriola", 14F, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.ColumnHeadersHeight = 40;
            dataGridView.EnableHeadersVisualStyles = false;

            // Настройка строк
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

            dataGridView.Columns.Clear();
            dataGridView.Columns.AddRange(
                new DataGridViewTextBoxColumn
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
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Type",
                    HeaderText = "Тип продукта",
                    DataPropertyName = "ProductType.TypeName",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                    {
                        Alignment = DataGridViewContentAlignment.MiddleLeft,
                        Padding = new Padding(10, 0, 0, 0)
                    }
                },
                new DataGridViewTextBoxColumn
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
                },
                new DataGridViewTextBoxColumn
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
                },
                new DataGridViewTextBoxColumn
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
                },
                new DataGridViewTextBoxColumn
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
                }
            );

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.CellFormatting += dataGridView_CellFormatting;
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
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

                var dataTable = await _db.ExecuteQueryAsync(query);
                _products = new List<Product>();

                foreach (DataRow row in dataTable.Rows)
                {
                    var product = new Product
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
                        ProductType = new ProductType
                        {
                            Id = Convert.ToInt32(row["ProductTypeId"]),
                            TypeName = row["TypeName"].ToString(),
                            Coefficient = Convert.ToDecimal(row["Coefficient"])
                        }
                    };

                    _products.Add(product);
                }

                dataGridView.DataSource = null;
                dataGridView.DataSource = _products;
                UpdateTotalLabel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка загрузки данных: {ex.Message}");
                MessageBox.Show("Произошла ошибка при загрузке данных.\nПроверьте подключение к базе данных.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                var filtered = string.IsNullOrEmpty(searchText) ? _products :
                    _products.Where(p =>
                        p.Article.ToLower().Contains(searchText) ||
                        p.Name.ToLower().Contains(searchText) ||
                        p.ProductType.TypeName.ToLower().Contains(searchText))
                    .ToList();

                dataGridView.DataSource = null;
                dataGridView.DataSource = filtered;
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
                Point dif = Point.Subtract(Cursor.Position, new Size(_dragCursorPoint));
                this.Location = Point.Add(_dragFormPoint, new Size(dif));
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
                MessageBox.Show("У вас нет прав для добавления продукции",
                    "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("У вас нет прав для редактирования продукции",
                    "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите продукцию для редактирования",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("У вас нет прав для удаления продукции",
                    "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите продукцию для удаления",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Продукция успешно удалена",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка удаления: {ex.Message}");
                    MessageBox.Show($"Ошибка удаления: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Выберите продукцию для просмотра материалов",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var product = (Product)dataGridView.SelectedRows[0].DataBoundItem;
            using (var form = new MaterialsForm(product, _userRole)) // Передаем роль пользователя
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
            if (MessageBox.Show("Вы действительно хотите выйти?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}