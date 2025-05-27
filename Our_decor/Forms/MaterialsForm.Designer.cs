using System;
using System.Drawing;
using System.Windows.Forms;

namespace Our_decor.Forms
{
    partial class MaterialForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new Panel();
            this.lblTitle = new Label();
            this.btnClose = new Button();

            this.dataGridMaterials = new DataGridView();
            this.btnAdd = new Button();
            this.btnDelete = new Button();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridMaterials)).BeginInit();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();

            // Верхняя панель
            this.panelTop.BackColor = Color.FromArgb(45, 96, 51);
            this.panelTop.Dock = DockStyle.Top;
            this.panelTop.Height = 40;
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.btnClose);

            // Заголовок
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Gabriola", 16F);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(10, 5);
            this.lblTitle.Text = "Материалы продукции";

            // Кнопка закрытия
            this.btnClose.Size = new Size(40, 40);
            this.btnClose.Location = new Point(460, 0);
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Font = new Font("Gabriola", 14F);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Text = "×";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // Таблица материалов
            this.dataGridMaterials.Location = new Point(10, 50);
            this.dataGridMaterials.Size = new Size(480, 300);
            this.dataGridMaterials.BackgroundColor = Color.White;
            this.dataGridMaterials.BorderStyle = BorderStyle.None;
            this.dataGridMaterials.AllowUserToAddRows = false;
            this.dataGridMaterials.AllowUserToDeleteRows = false;
            this.dataGridMaterials.ReadOnly = true;
            this.dataGridMaterials.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridMaterials.MultiSelect = false;
            this.dataGridMaterials.RowHeadersVisible = false;
            this.dataGridMaterials.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridMaterials.ColumnHeadersHeight = 40;
            this.dataGridMaterials.RowTemplate.Height = 35;
            this.dataGridMaterials.EnableHeadersVisualStyles = false;
            this.dataGridMaterials.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 96, 51);
            this.dataGridMaterials.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dataGridMaterials.ColumnHeadersDefaultCellStyle.Font = new Font("Gabriola", 14F);
            this.dataGridMaterials.DefaultCellStyle.Font = new Font("Gabriola", 12F);

            // Добавление колонок
            this.dataGridMaterials.Columns.AddRange(new DataGridViewColumn[] {
                new DataGridViewTextBoxColumn
                {
                    Name = "MaterialName",
                    HeaderText = "Наименование",
                    Width = 200
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "MaterialType",
                    HeaderText = "Тип",
                    Width = 120
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Quantity",
                    HeaderText = "Количество",
                    Width = 100
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "Unit",
                    HeaderText = "Ед. изм.",
                    Width = 60
                }
            });

            // Кнопка добавления
            this.btnAdd.Location = new Point(120, 360);
            this.btnAdd.Size = new Size(120, 35);
            this.btnAdd.Text = "Добавить";
            this.btnAdd.BackColor = Color.FromArgb(45, 96, 51);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.Font = new Font("Gabriola", 14F);
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);

            // Кнопка удаления
            this.btnDelete.Location = new Point(260, 360);
            this.btnDelete.Size = new Size(120, 35);
            this.btnDelete.Text = "Удалить";
            this.btnDelete.BackColor = Color.FromArgb(45, 96, 51);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Font = new Font("Gabriola", 14F);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            // Настройка формы
            this.ClientSize = new Size(500, 410);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(187, 217, 178);

            // Добавление элементов на форму
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.dataGridMaterials);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnDelete);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridMaterials)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Panel panelTop;
        private Label lblTitle;
        private Button btnClose;
        private DataGridView dataGridMaterials;
        private Button btnAdd;
        private Button btnDelete;
    }
}