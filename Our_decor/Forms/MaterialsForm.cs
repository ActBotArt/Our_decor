using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Our_decor.Forms
{
    public partial class MaterialForm : Form
    {
        private bool isDragging = false;
        private Point lastCursor;
        private Point lastForm;
        private readonly string productArticle;

        public class MaterialData
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public decimal Quantity { get; set; }
            public string Unit { get; set; }
        }

        public MaterialForm(string article)
        {
            InitializeComponent();
            productArticle = article;
            lblTitle.Text = $"Материалы продукции (Артикул: {article})";
            SetupEvents();
            LoadMaterials();
        }

        private void SetupEvents()
        {
            // Перетаскивание формы
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

            // Двойной клик по строке
            dataGridMaterials.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    EditMaterial();
            };
        }

        private void LoadMaterials()
        {
            try
            {
                // TODO: Загрузка материалов из базы данных
                // Пока используем тестовые данные
                dataGridMaterials.Rows.Clear();
                dataGridMaterials.Rows.Add("Бумага", "Основа", "100", "м²");
                dataGridMaterials.Rows.Add("Краска", "Покрытие", "2.5", "л");
                dataGridMaterials.Rows.Add("Клей", "Расходник", "1.5", "кг");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке материалов: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditMaterial()
        {
            if (dataGridMaterials.SelectedRows.Count > 0)
            {
                var row = dataGridMaterials.SelectedRows[0];
                // TODO: Реализовать редактирование материала
                MessageBox.Show($"Редактирование материала: {row.Cells["MaterialName"].Value}",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // TODO: Реализовать добавление материала
            MessageBox.Show("Добавление нового материала",
                "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridMaterials.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите материал для удаления",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dataGridMaterials.SelectedRows[0];
            string materialName = row.Cells["MaterialName"].Value.ToString();

            if (MessageBox.Show(
                $"Вы действительно хотите удалить материал \"{materialName}\"?",
                "Подтверждение удаления",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // TODO: Реализовать удаление из базы данных
                dataGridMaterials.Rows.Remove(row);
                MessageBox.Show("Материал успешно удален",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}