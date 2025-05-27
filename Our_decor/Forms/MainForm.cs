using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Our_decor.Models;

namespace Our_decor.Forms
{
    public partial class MainForm : Form
    {
        private readonly string userRole;

        public MainForm(string role = "User")
        {
            InitializeComponent();
            userRole = role;
            this.Text = $"Our Decor - {role}";
            SetupDataGridView();
            SetupControls();
            _ = LoadDataAsync();
        }

        private void SetupDataGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ReadOnly = true;
            dataGridView.MultiSelect = false;
            dataGridView.RowHeadersVisible = false;

            dataGridView.Columns.Clear();
            dataGridView.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn
                {
                    Name = "Id",
                    DataPropertyName = "Id",
                    Visible = false
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "ArticleNumber",
                    HeaderText = "Артикул",
                    DataPropertyName = "Article",
                    Width = 100
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "ProductType",
                    HeaderText = "Тип продукта",
                    DataPropertyName = "ProductType",
                    Width = 120
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Title",
                    HeaderText = "Наименование",
                    DataPropertyName = "Name",
                    Width = 250
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "MinCostForAgent",
                    HeaderText = "Мин. стоимость",
                    DataPropertyName = "MinPartnerCost",
                    Width = 120,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "C2"
                    }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Width",
                    HeaderText = "Ширина рулона",
                    DataPropertyName = "RollWidth",
                    Width = 120,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "N2"
                    }
                }
            });
        }

        private void SetupControls()
        {
            btnAdd.Visible = userRole != "User";
            btnEdit.Visible = userRole != "User";
            btnDelete.Visible = userRole == "Admin";
        }

        private async Task LoadDataAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var products = await Product.GetAllProductsAsync();
                dataGridView.DataSource = products;

                lblTotal.Text = $"Всего продуктов: {products.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (userRole == "User")
            {
                MessageBox.Show("У вас нет прав для добавления продукции",
                    "Доступ запрещен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var form = new ProductForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        await form.Result.SaveAsync();
                        await LoadDataAsync();
                        MessageBox.Show("Продукция успешно добавлена",
                            "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при добавлении продукции: {ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (userRole == "User")
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

            var selectedProduct = (Product)dataGridView.SelectedRows[0].DataBoundItem;
            using (var form = new ProductForm(selectedProduct))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        await form.Result.SaveAsync();
                        await LoadDataAsync();
                        MessageBox.Show("Продукция успешно обновлена",
                            "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обновлении продукции: {ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (userRole != "Admin")
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

            var selectedProduct = (Product)dataGridView.SelectedRows[0].DataBoundItem;

            if (MessageBox.Show(
                $"Вы действительно хотите удалить продукцию?\n\nАртикул: {selectedProduct.Article}\nНаименование: {selectedProduct.Name}",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    await Product.DeleteAsync(selectedProduct.Id);
                    await LoadDataAsync();
                    MessageBox.Show("Продукция успешно удалена",
                        "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении продукции: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }
    }
}