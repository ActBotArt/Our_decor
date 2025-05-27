using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Our_decor.Models;

namespace Our_decor.Forms
{
    public partial class ProductForm : Form
    {
        private bool isDragging = false;
        private Point lastCursor;
        private Point lastForm;
        private bool isNewProduct;
        private readonly Product existingProduct;

        public ProductForm(Product product = null)
        {
            InitializeComponent();
            existingProduct = product;
            isNewProduct = (existingProduct == null);
            InitializeProductTypes();
            SetupForm();
            SetupEvents();
        }

        private async void InitializeProductTypes()
        {
            try
            {
                cmbType.Items.Clear();
                var types = await Product.GetProductTypesAsync();
                foreach (var type in types)
                {
                    cmbType.Items.Add(type);
                }

                if (cmbType.Items.Count > 0)
                {
                    cmbType.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке типов продукции: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupForm()
        {
            this.Text = isNewProduct ? "Добавление продукции" : "Редактирование продукции";
            lblTitle.Text = this.Text;

            if (!isNewProduct && existingProduct != null)
            {
                txtArticle.Text = existingProduct.Article;
                txtArticle.ReadOnly = true;
                foreach (ProductType type in cmbType.Items)
                {
                    if (type.TypeName == existingProduct.ProductType.TypeName)
                    {
                        cmbType.SelectedItem = type;
                        break;
                    }
                }
                txtName.Text = existingProduct.Name;
                txtDescription.Text = existingProduct.Description;
                numMinCost.Value = existingProduct.MinPartnerCost;
                numWidth.Value = existingProduct.RollWidth;
            }
            else
            {
                txtArticle.Text = GenerateArticle();
                numMinCost.Value = 1000;
                numWidth.Value = 1.06m;
            }
        }

        private string GenerateArticle()
        {
            return DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        }

        private void SetupEvents()
        {
            panelTop.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = true;
                    lastCursor = Cursor.Position;
                    lastForm = this.Location;
                }
            };

            panelTop.MouseMove += (s, e) =>
            {
                if (isDragging)
                {
                    int xDiff = Cursor.Position.X - lastCursor.X;
                    int yDiff = Cursor.Position.Y - lastCursor.Y;
                    this.Location = new Point(lastForm.X + xDiff, lastForm.Y + yDiff);
                }
            };

            panelTop.MouseUp += (s, e) => isDragging = false;

            txtArticle.TextChanged += ValidateInput;
            txtName.TextChanged += ValidateInput;
            cmbType.SelectedIndexChanged += ValidateInput;
            numMinCost.ValueChanged += ValidateInput;
            numWidth.ValueChanged += ValidateInput;

            var toolTip = new ToolTip { ShowAlways = true };
            toolTip.SetToolTip(txtArticle, "Уникальный артикул продукции");
            toolTip.SetToolTip(cmbType, "Тип продукции");
            toolTip.SetToolTip(txtName, "Наименование продукции");
            toolTip.SetToolTip(txtDescription, "Описание продукции (необязательно)");
            toolTip.SetToolTip(numMinCost, "Минимальная стоимость (руб.)");
            toolTip.SetToolTip(numWidth, "Ширина рулона (м)");
        }

        private void ValidateInput(object sender, EventArgs e)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtArticle.Text))
            {
                isValid = false;
                txtArticle.BackColor = Color.MistyRose;
            }
            else
            {
                txtArticle.BackColor = SystemColors.Window;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                isValid = false;
                txtName.BackColor = Color.MistyRose;
            }
            else
            {
                txtName.BackColor = SystemColors.Window;
            }

            if (cmbType.SelectedItem == null)
            {
                isValid = false;
                cmbType.BackColor = Color.MistyRose;
            }
            else
            {
                cmbType.BackColor = SystemColors.Window;
            }

            if (numMinCost.Value <= 0)
            {
                isValid = false;
                numMinCost.BackColor = Color.MistyRose;
            }
            else
            {
                numMinCost.BackColor = SystemColors.Window;
            }

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
            try
            {
                btnSave.Enabled = false;
                Cursor = Cursors.WaitCursor;

                var product = new Product
                {
                    Id = existingProduct?.Id ?? 0,
                    Article = txtArticle.Text.Trim(),
                    ProductType = (ProductType)cmbType.SelectedItem,
                    Name = txtName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    MinPartnerCost = numMinCost.Value,
                    RollWidth = numWidth.Value,
                    WorkshopNumber = 1,
                    WorkersCount = 1
                };

                await product.SaveAsync();

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
            if (MessageBox.Show("Вы уверены, что хотите отменить изменения?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            btnCancel_Click(sender, e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(this, EventArgs.Empty);
            }
            else if (e.KeyCode == Keys.Enter && btnSave.Enabled)
            {
                btnSave_Click(this, EventArgs.Empty);
            }
        }

        public Product Result { get; private set; }
    }
}