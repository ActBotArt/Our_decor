using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using Our_decor.Models;
using Our_decor.Services;

namespace Our_decor.Forms
{
    public partial class MaterialEditForm : Form
    {
        private readonly int _productId;
        private readonly int? _materialId;
        private readonly DatabaseService _db;
        private readonly bool _isEditing;
        private bool _isDragging = false;
        private Point _dragCursorPoint;
        private Point _dragFormPoint;

        public MaterialEditForm(int productId, int? materialId = null)
        {
            InitializeComponent();
            _productId = productId;
            _materialId = materialId;
            _db = DatabaseService.Instance;
            _isEditing = materialId.HasValue;

            ConfigureForm();
            LoadMaterialTypes();
            if (_isEditing)
            {
                LoadMaterial();
            }
        }

        private void ConfigureForm()
        {
            this.Text = _isEditing ? "Редактирование материала" : "Добавление материала";
            lblTitle.Text = this.Text;
        }

        private async void LoadMaterialTypes()
        {
            try
            {
                var query = "SELECT Id, TypeName FROM MaterialTypes ORDER BY TypeName";
                var dataTable = await _db.ExecuteQueryAsync(query);

                cboMaterialType.DataSource = dataTable;
                cboMaterialType.DisplayMember = "TypeName";
                cboMaterialType.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки типов материалов: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void CboMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaterialType.SelectedValue == null) return;

            try
            {
                var typeId = Convert.ToInt32(cboMaterialType.SelectedValue);
                var query = @"
                    SELECT Id, Name 
                    FROM Materials 
                    WHERE MaterialTypeId = @TypeId 
                    ORDER BY Name";

                var parameter = new SqlParameter("@TypeId", typeId);
                var dataTable = await _db.ExecuteQueryAsync(query, parameter);

                cboMaterial.DataSource = dataTable;
                cboMaterial.DisplayMember = "Name";
                cboMaterial.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки материалов: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadMaterial()
        {
            if (!_materialId.HasValue) return;

            try
            {
                var query = @"
                    SELECT m.*, mt.Id as TypeId, pm.Quantity
                    FROM Materials m
                    JOIN MaterialTypes mt ON m.MaterialTypeId = mt.Id
                    JOIN ProductMaterials pm ON m.Id = pm.MaterialId
                    WHERE pm.ProductId = @ProductId AND m.Id = @MaterialId";

                var parameters = new[]
                {
                    new SqlParameter("@ProductId", _productId),
                    new SqlParameter("@MaterialId", _materialId.Value)
                };

                var dataTable = await _db.ExecuteQueryAsync(query, parameters);
                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    cboMaterialType.SelectedValue = row["TypeId"];
                    cboMaterial.SelectedValue = row["Id"];
                    txtQuantity.Text = row["Quantity"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных материала: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboMaterial.SelectedValue == null)
                {
                    MessageBox.Show("Выберите материал",
                        "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtQuantity.Text, out decimal quantity) || quantity <= 0)
                {
                    MessageBox.Show("Введите корректное количество",
                        "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var materialId = Convert.ToInt32(cboMaterial.SelectedValue);

                if (_isEditing)
                {
                    var updateQuery = @"
                        UPDATE ProductMaterials 
                        SET MaterialId = @MaterialId,
                            Quantity = @Quantity
                        WHERE ProductId = @ProductId 
                          AND MaterialId = @OldMaterialId";

                    var updateParams = new[]
                    {
                        new SqlParameter("@ProductId", _productId),
                        new SqlParameter("@MaterialId", materialId),
                        new SqlParameter("@Quantity", quantity),
                        new SqlParameter("@OldMaterialId", _materialId.Value)
                    };

                    await _db.ExecuteNonQueryAsync(updateQuery, updateParams);
                }
                else
                {
                    // Проверяем, не существует ли уже такой материал
                    var checkQuery = @"
                        SELECT COUNT(*) 
                        FROM ProductMaterials 
                        WHERE ProductId = @ProductId 
                          AND MaterialId = @MaterialId";

                    var checkParams = new[]
                    {
                        new SqlParameter("@ProductId", _productId),
                        new SqlParameter("@MaterialId", materialId)
                    };

                    var exists = Convert.ToInt32(await _db.ExecuteScalarAsync(checkQuery, checkParams)) > 0;

                    if (exists)
                    {
                        MessageBox.Show("Этот материал уже добавлен к продукту",
                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var insertQuery = @"
                        INSERT INTO ProductMaterials (ProductId, MaterialId, Quantity)
                        VALUES (@ProductId, @MaterialId, @Quantity)";

                    var insertParams = new[]
                    {
                        new SqlParameter("@ProductId", _productId),
                        new SqlParameter("@MaterialId", materialId),
                        new SqlParameter("@Quantity", quantity)
                    };

                    await _db.ExecuteNonQueryAsync(insertQuery, insertParams);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
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
    }
}