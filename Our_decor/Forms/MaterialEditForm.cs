using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Our_decor.Services;

namespace Our_decor.Forms
{
    public partial class MaterialEditForm : Form
    {
        private readonly int _productId;
        private readonly DatabaseService _db;
        private bool _isDragging = false;
        private Point _dragCursorPoint;
        private Point _dragFormPoint;

        public MaterialEditForm(int productId)
        {
            InitializeComponent();
            _productId = productId;
            _db = DatabaseService.Instance;
            SetupForm();
            LoadMaterialTypes();
        }

        private void SetupForm()
        {
            this.Text = "Добавление материала";
            lblTitle.Text = this.Text;

            this.BackColor = Color.FromArgb(187, 217, 178);
            panelTop.BackColor = Color.FromArgb(45, 96, 51);

            foreach (Button button in new[] { btnSave, btnCancel, btnClose })
            {
                button.BackColor = Color.FromArgb(45, 96, 51);
                button.ForeColor = Color.White;
                button.Font = new Font("Gabriola", 14F);
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
            }

            foreach (Label label in new[] { lblMaterialType, lblMaterialName, lblQuantity,
                                          lblCost, lblUnit, lblStockQuantity, lblMinQuantity })
            {
                label.Font = new Font("Gabriola", 14F);
                label.ForeColor = Color.FromArgb(45, 96, 51);
            }

            foreach (TextBox textBox in new[] { txtMaterialName, txtQuantity, txtCost,
                                              txtUnit, txtStockQuantity, txtMinQuantity })
            {
                textBox.Font = new Font("Gabriola", 12F);
                textBox.BackColor = Color.White;
                textBox.TextChanged += ValidateInput;
            }

            txtQuantity.Text = "0";
            txtCost.Text = "0";
            txtStockQuantity.Text = "0";
            txtMinQuantity.Text = "0";

            cmbMaterialType.Font = new Font("Gabriola", 12F);
            cmbMaterialType.SelectedIndexChanged += ValidateInput;

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

        private async void LoadMaterialTypes()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                var query = "SELECT * FROM MaterialTypes ORDER BY TypeName";
                var dt = await _db.ExecuteQueryAsync(query);

                cmbMaterialType.DataSource = dt;
                cmbMaterialType.DisplayMember = "TypeName";
                cmbMaterialType.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке типов материалов: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Cancel;
                Close();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ValidateInput(object sender, EventArgs e)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtMaterialName.Text))
            {
                isValid = false;
                txtMaterialName.BackColor = Color.MistyRose;
            }
            else
            {
                txtMaterialName.BackColor = SystemColors.Window;
            }

            if (cmbMaterialType.SelectedItem == null)
            {
                isValid = false;
                cmbMaterialType.BackColor = Color.MistyRose;
            }
            else
            {
                cmbMaterialType.BackColor = SystemColors.Window;
            }

            if (!decimal.TryParse(txtQuantity.Text, out decimal quantity) || quantity <= 0)
            {
                isValid = false;
                txtQuantity.BackColor = Color.MistyRose;
            }
            else
            {
                txtQuantity.BackColor = SystemColors.Window;
            }

            if (!decimal.TryParse(txtCost.Text, out decimal cost) || cost <= 0)
            {
                isValid = false;
                txtCost.BackColor = Color.MistyRose;
            }
            else
            {
                txtCost.BackColor = SystemColors.Window;
            }

            if (string.IsNullOrWhiteSpace(txtUnit.Text))
            {
                isValid = false;
                txtUnit.BackColor = Color.MistyRose;
            }
            else
            {
                txtUnit.BackColor = SystemColors.Window;
            }

            if (!decimal.TryParse(txtStockQuantity.Text, out decimal stockQuantity) || stockQuantity < 0)
            {
                isValid = false;
                txtStockQuantity.BackColor = Color.MistyRose;
            }
            else
            {
                txtStockQuantity.BackColor = SystemColors.Window;
            }

            if (!decimal.TryParse(txtMinQuantity.Text, out decimal minQuantity) || minQuantity < 0)
            {
                isValid = false;
                txtMinQuantity.BackColor = Color.MistyRose;
            }
            else
            {
                txtMinQuantity.BackColor = SystemColors.Window;
            }

            btnSave.Enabled = isValid;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;
                Cursor = Cursors.WaitCursor;

                var materialTypeId = ((DataRowView)cmbMaterialType.SelectedItem)["Id"];
                var quantity = decimal.Parse(txtQuantity.Text);
                var cost = decimal.Parse(txtCost.Text);
                var stockQuantity = decimal.Parse(txtStockQuantity.Text);
                var minQuantity = decimal.Parse(txtMinQuantity.Text);

                var insertMaterialQuery = @"
                    INSERT INTO Materials (Name, MaterialTypeId, Cost, Unit, StockQuantity, MinQuantity)
                    OUTPUT INSERTED.Id
                    VALUES (@Name, @MaterialTypeId, @Cost, @Unit, @StockQuantity, @MinQuantity)";

                var parameters = new[]
                {
                    new SqlParameter("@Name", txtMaterialName.Text.Trim()),
                    new SqlParameter("@MaterialTypeId", materialTypeId),
                    new SqlParameter("@Cost", cost),
                    new SqlParameter("@Unit", txtUnit.Text.Trim()),
                    new SqlParameter("@StockQuantity", stockQuantity),
                    new SqlParameter("@MinQuantity", minQuantity)
                };

                var materialId = await _db.ExecuteScalarAsync(insertMaterialQuery, parameters);

                if (materialId != null)
                {
                    var insertProductMaterialQuery = @"
                        INSERT INTO ProductMaterials (ProductId, MaterialId, Quantity)
                        VALUES (@ProductId, @MaterialId, @Quantity)";

                    parameters = new[]
                    {
                        new SqlParameter("@ProductId", _productId),
                        new SqlParameter("@MaterialId", materialId),
                        new SqlParameter("@Quantity", quantity)
                    };

                    await _db.ExecuteNonQueryAsync(insertProductMaterialQuery, parameters);

                    DialogResult = DialogResult.OK;
                    Close();
                }
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
            return base.ProcessDialogKey(keyData);
        }
    }
}