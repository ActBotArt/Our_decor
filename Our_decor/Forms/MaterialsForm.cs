using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Our_decor.Models;
using System.Diagnostics;
using Our_decor.Services;
using System.IO;

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
            _timer = new Timer();
            _timer.Interval = 1000; // 1 секунда
            _timer.Tick += Timer_Tick;
            _timer.Start();
            UpdateDateTime(); // Первоначальное обновление
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            lblDateTime.Text = $"Current Date and Time (UTC - YYYY-MM-DD HH:MM:SS formatted): {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}";
            lblUser.Text = $"Current User's Login: {_currentUser}";
        }

        private void ConfigureForm()
        {
            this.Text = $"Материалы для {_product.Name}";
            this.BackColor = Color.White;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.MinimumSize = new Size(800, 600);

            // Настройка верхней панели
            panelTop.Height = 45;
            panelTop.Padding = new Padding(12, 5, 120, 5);
            panelTop.MouseDown += PanelTop_MouseDown;
            panelTop.MouseMove += PanelTop_MouseMove;
            panelTop.MouseUp += PanelTop_MouseUp;

            // Настройка заголовка
            lblTitle.Text = this.Text;
            lblTitle.AutoEllipsis = true;
            lblTitle.MaximumSize = new Size(panelTop.Width - 150, 35);
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Gabriola", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(45, 96, 51);
            lblTitle.Location = new Point(12, 5);
            lblTitle.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            // Настройка кнопки закрытия
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

            // Настройка DataGridView
            SetupDataGridView();

            // Настройка логотипа
            try
            {
                string logoPath = Path.Combine(Application.StartupPath, "..", "..", "Resources", "logo.png");
                if (File.Exists(logoPath))
                {
                    pictureBoxLogo.Image = Image.FromFile(logoPath);
                    pictureBoxLogo.Location = new Point(
                        this.ClientSize.Width - pictureBoxLogo.Width - 20,
                        this.ClientSize.Height - pictureBoxLogo.Height - 70 // Учитываем высоту статусной строки
                    );
                    pictureBoxLogo.BringToFront();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при загрузке логотипа: {ex.Message}");
            }

            // Настройка статусной строки
            statusStrip.BackColor = Color.FromArgb(187, 217, 178);
            lblDateTime.Font = new Font("Segoe UI", 9F);
            lblUser.Font = new Font("Segoe UI", 9F);

            // Настройка видимости элементов для администратора
            btnAdd.Visible = _userRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
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
            dataGridView.RowHeadersVisible = false;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.Font = new Font("Gabriola", 12F);
            dataGridView.GridColor = Color.FromArgb(187, 217, 178);
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

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
                    FillWeight = 10,
                    DefaultCellStyle = new DataGridViewCellStyle(baseStyle)
                    {
                        Format = "N2",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    }
                }
            );
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
                    MessageBox.Show("Материалы не найдены",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!dataGridView.IsHandleCreated || dataGridView.IsDisposed) return;

                dataGridView.Invoke((MethodInvoker)delegate
                {
                    dataGridView.DataSource = dataTable;
                    lblTotal.Text = $"Всего материалов: {dataTable.Rows.Count}";
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке материалов: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        Cursor = Cursors.Default;
                    });
                }
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

            using (var form = new MaterialEditForm(_product.Id))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadMaterials();
                }
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
                    this.ClientSize.Height - pictureBoxLogo.Height - 70 // Учитываем высоту статусной строки
                );
                pictureBoxLogo.BringToFront();
            }
        }
    }
}