﻿// MaterialsForm.cs
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Our_decor.Models;
using Our_decor.Services;

namespace Our_decor.Forms
{
    public partial class MaterialsForm : Form
    {
        private readonly Product _product;
        private readonly DatabaseService _db;
        private readonly string _userRole;
        private readonly string _currentUser;
        private bool _isDragging = false;
        private Point _dragCursorPoint;
        private Point _dragFormPoint;
        private Timer _timer;

        public MaterialsForm(Product product, string userRole, string currentUser = "User")
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            InitializeComponent();

            try
            {
                string iconPath = Path.Combine(Application.StartupPath, "..", "..", "Resources", "icon.ico");
                if (File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                    this.ShowIcon = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при загрузке иконки: {ex.Message}");
            }

            _product = product;
            _db = DatabaseService.Instance;
            _userRole = userRole ?? "User";
            _currentUser = currentUser;

            ConfigureForm();
            LoadMaterials();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            _timer = new Timer
            {
                Interval = 1000
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
            UpdateDateTime();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            lblDateTime.Text = $"Дата и время: {DateTime.Now:dd.MM.yyyy HH:mm:ss}";
            lblUser.Text = $"Пользователь: {_currentUser}";
        }

        private void ConfigureForm()
        {
            this.Text = $"Материалы для {_product.Name}";
            this.BackColor = Color.White;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.MinimumSize = new Size(800, 600);

            panelTop.Height = 45;
            panelTop.Padding = new Padding(12, 5, 120, 5);
            panelTop.MouseDown += PanelTop_MouseDown;
            panelTop.MouseMove += PanelTop_MouseMove;
            panelTop.MouseUp += PanelTop_MouseUp;

            lblTitle.Text = this.Text;
            lblTitle.AutoEllipsis = true;
            lblTitle.MaximumSize = new Size(panelTop.Width - 150, 35);
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Gabriola", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(45, 96, 51);
            lblTitle.Location = new Point(12, 5);
            lblTitle.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            btnClose.BackColor = Color.FromArgb(45, 96, 51);
            btnClose.ForeColor = Color.White;
            btnClose.Font = new Font("Gabriola", 14F);
            btnClose.Size = new Size(100, 35);
            btnClose.Location = new Point(panelTop.Width - btnClose.Width - 12, 5);
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BringToFront();
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 130, 70);
            btnClose.FlatAppearance.MouseDownBackColor = Color.FromArgb(35, 75, 40);

            SetupDataGridView();

            statusStrip.BackColor = Color.FromArgb(187, 217, 178);
            lblDateTime.Font = new Font("Segoe UI", 9F);
            lblDateTime.ForeColor = Color.FromArgb(45, 96, 51);
            lblUser.Font = new Font("Segoe UI", 9F);
            lblUser.ForeColor = Color.FromArgb(45, 96, 51);

            btnAdd.Visible = _userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            btnDelete.Visible = _userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            dataGridView.ReadOnly = !_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        private void SetupDataGridView()
        {
            if (dataGridView == null) return;

            dataGridView.AutoGenerateColumns = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.Font = new Font("Gabriola", 12F);
            dataGridView.GridColor = Color.FromArgb(187, 217, 178);
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 96, 51);
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Gabriola", 14F, FontStyle.Bold);
            dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.ColumnHeadersHeight = 40;
            dataGridView.EnableHeadersVisualStyles = false;

            dataGridView.RowTemplate.Height = 35;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 247, 240);
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(187, 217, 178);
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.FromArgb(45, 96, 51);

            // Добавляем защиту от редактирования
            dataGridView.CellBeginEdit += DataGridView_CellBeginEdit;
            dataGridView.CellValidating += DataGridView_CellValidating;
            dataGridView.CellParsing += DataGridView_CellParsing;
            dataGridView.DataError += DataGridView_DataError;

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
                // Скрытая колонка для MaterialId
                new DataGridViewTextBoxColumn
                {
                    Name = "MaterialId",
                    HeaderText = "ID",
                    DataPropertyName = "MaterialId",
                    Visible = false,
                    ReadOnly = true
                },
                // Скрытая колонка для MaterialTypeId
                new DataGridViewTextBoxColumn
                {
                    Name = "MaterialTypeId",
                    HeaderText = "TypeID",
                    DataPropertyName = "MaterialTypeId",
                    Visible = false,
                    ReadOnly = true
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "TypeName",
                    HeaderText = "Тип материала",
                    DataPropertyName = "TypeName",
                    ReadOnly = true,
                    FillWeight = 15,
                    DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                    {
                        BackColor = Color.FromArgb(209, 238, 197),
                        Padding = new Padding(10, 0, 0, 0)
                    }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "MaterialName",
                    HeaderText = "Наименование",
                    DataPropertyName = "MaterialName",
                    ReadOnly = true,
                    FillWeight = 30,
                    DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                    {
                        Padding = new Padding(10, 0, 0, 0)
                    }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Cost",
                    HeaderText = "Цена",
                    DataPropertyName = "Cost",
                    ReadOnly = true,
                    FillWeight = 10,
                    DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                    {
                        Format = "N2",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Quantity",
                    HeaderText = "Количество",
                    DataPropertyName = "Quantity",
                    FillWeight = 10,
                    DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                    {
                        Format = "N2",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Unit",
                    HeaderText = "Ед. изм.",
                    DataPropertyName = "Unit",
                    ReadOnly = true,
                    FillWeight = 10,
                    DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                    {
                        Alignment = DataGridViewContentAlignment.MiddleCenter
                    }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "StockQuantity",
                    HeaderText = "На складе",
                    DataPropertyName = "StockQuantity",
                    ReadOnly = true,
                    FillWeight = 10,
                    DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                    {
                        Format = "N2",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "MinQuantity",
                    HeaderText = "Мин. кол-во",
                    DataPropertyName = "MinQuantity",
                    ReadOnly = true,
                    FillWeight = 10,
                    DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                    {
                        Format = "N2",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                }
            );
        }

        // Защита от редактирования для обычных пользователей
        private void DataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (!_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                e.Cancel = true;
                return;
            }

            // Разрешаем редактировать только количество
            string columnName = dataGridView.Columns[e.ColumnIndex].Name;
            if (columnName != "Quantity")
            {
                e.Cancel = true;
            }
        }

        // Валидация ввода данных
        private void DataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                return;

            string columnName = dataGridView.Columns[e.ColumnIndex].Name;

            if (columnName == "Quantity")
            {
                if (!decimal.TryParse(e.FormattedValue?.ToString(), out decimal value) || value < 0)
                {
                    e.Cancel = true;
                    MessageBox.Show("Введите корректное положительное число для количества!",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // Парсинг данных
        private void DataGridView_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            string columnName = dataGridView.Columns[e.ColumnIndex].Name;

            if (columnName == "Quantity")
            {
                if (decimal.TryParse(e.Value?.ToString(), out decimal value))
                {
                    e.Value = Math.Round(value, 2);
                    e.ParsingApplied = true;
                }
            }
        }

        // Обработка ошибок данных
        private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show($"Ошибка в данных: {e.Exception.Message}",
                "Ошибка данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.Cancel = true;
        }

        private async void LoadMaterials()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var query = @"
                    SELECT 
                        mt.TypeName,
                        m.Name AS MaterialName, 
                        m.Cost,
                        pm.Quantity,
                        m.Unit,
                        m.StockQuantity,
                        m.MinQuantity,
                        m.Id AS MaterialId,
                        m.MaterialTypeId
                    FROM ProductMaterials pm
                    INNER JOIN Materials m ON pm.MaterialId = m.Id
                    INNER JOIN MaterialTypes mt ON m.MaterialTypeId = mt.Id
                    WHERE pm.ProductId = @ProductId
                    ORDER BY mt.TypeName, m.Name";

                var parameter = new SqlParameter("@ProductId", SqlDbType.Int) { Value = _product.Id };
                var dataTable = await _db.ExecuteQueryAsync(query, parameter);

                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    dataGridView.DataSource = null;
                    lblTotal.Text = "Всего материалов: 0";
                    return;
                }

                dataGridView.DataSource = dataTable;
                lblTotal.Text = $"Всего материалов: {dataTable.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке материалов: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("У вас нет прав для добавления материалов",
                    "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var form = new MaterialEditForm(_product.Id))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadMaterials();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы добавления: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("У вас нет прав для удаления материалов",
                    "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите материал для удаления",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var row = dataGridView.SelectedRows[0];

                // Проверяем существование колонки MaterialId
                if (!dataGridView.Columns.Contains("MaterialId"))
                {
                    MessageBox.Show("Ошибка структуры данных: отсутствует колонка MaterialId",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверяем значение в ячейке
                if (row.Cells["MaterialId"].Value == null || row.Cells["MaterialId"].Value == DBNull.Value)
                {
                    MessageBox.Show("Не удается определить ID материала для удаления",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(row.Cells["MaterialId"].Value.ToString(), out int materialId) || materialId <= 0)
                {
                    MessageBox.Show("Некорректный ID материала",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string materialName = row.Cells["MaterialName"].Value?.ToString() ?? "неизвестный материал";

                if (MessageBox.Show(
                    $"Вы действительно хотите удалить материал '{materialName}'?\n\nВнимание: это действие нельзя отменить!",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    await DeleteMaterial(materialId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении материала: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async System.Threading.Tasks.Task DeleteMaterial(int materialId)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Проверяем, используется ли материал в других продуктах
                var checkQuery = "SELECT COUNT(*) FROM ProductMaterials WHERE MaterialId = @MaterialId AND ProductId <> @CurrentProductId";
                var checkParam1 = new SqlParameter("@MaterialId", SqlDbType.Int) { Value = materialId };
                var checkParam2 = new SqlParameter("@CurrentProductId", SqlDbType.Int) { Value = _product.Id };

                var result = await _db.ExecuteScalarAsync(checkQuery, checkParam1, checkParam2);
                int usageCount = Convert.ToInt32(result);

                if (usageCount > 0)
                {
                    // Удаляем только связь с текущим продуктом
                    var deleteProdMatQuery = "DELETE FROM ProductMaterials WHERE ProductId = @ProductId AND MaterialId = @MaterialId";
                    var param1 = new SqlParameter("@ProductId", SqlDbType.Int) { Value = _product.Id };
                    var param2 = new SqlParameter("@MaterialId", SqlDbType.Int) { Value = materialId };
                    await _db.ExecuteNonQueryAsync(deleteProdMatQuery, param1, param2);

                    MessageBox.Show("Материал был отвязан от данного продукта.\nМатериал используется в других продуктах и не был удален полностью.",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Сначала удаляем связь из ProductMaterials
                    var deleteProdMatQuery = "DELETE FROM ProductMaterials WHERE ProductId = @ProductId AND MaterialId = @MaterialId";
                    var param1 = new SqlParameter("@ProductId", SqlDbType.Int) { Value = _product.Id };
                    var param2 = new SqlParameter("@MaterialId", SqlDbType.Int) { Value = materialId };
                    await _db.ExecuteNonQueryAsync(deleteProdMatQuery, param1, param2);

                    // Затем удаляем сам материал
                    var deleteMaterialQuery = "DELETE FROM Materials WHERE Id = @Id";
                    var param3 = new SqlParameter("@Id", SqlDbType.Int) { Value = materialId };
                    await _db.ExecuteNonQueryAsync(deleteMaterialQuery, param3);

                    MessageBox.Show("Материал успешно удален.",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadMaterials();
            }
            catch (SqlException sqlEx)
            {
                string errorMessage = "Ошибка базы данных при удалении материала.";
                if (sqlEx.Number == 547) // Foreign key constraint violation
                {
                    errorMessage = "Невозможно удалить материал: он используется в других записях.";
                }
                MessageBox.Show($"{errorMessage}\n\nТехническая информация: {sqlEx.Message}",
                    "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла непредвиденная ошибка при удалении: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (lblTitle != null)
            {
                lblTitle.MaximumSize = new Size(panelTop.Width - 150, 35);
            }
            btnClose?.BringToFront();

            if (pictureBoxLogo != null && !pictureBoxLogo.IsDisposed)
            {
                pictureBoxLogo.Location = new Point(
                    this.ClientSize.Width - pictureBoxLogo.Width - 20,
                    this.ClientSize.Height - pictureBoxLogo.Height - 70
                );
                pictureBoxLogo.BringToFront();
            }
        }
    }
}