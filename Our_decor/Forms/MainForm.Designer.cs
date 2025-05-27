using System;
using System.Windows.Forms;
using System.Drawing;

namespace Our_decor.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelTop;
        private PictureBox pictureBoxLogo;
        private Label lblSearch;
        private TextBox txtSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnMaterials;
        private Button btnExit;
        private DataGridView dataGridView;
        private Label lblTotal;

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
            this.pictureBoxLogo = new PictureBox();
            this.lblSearch = new Label();
            this.txtSearch = new TextBox();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnMaterials = new Button();
            this.btnExit = new Button();
            this.dataGridView = new DataGridView();
            this.lblTotal = new Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = Color.FromArgb(187, 217, 178);
            this.panelTop.Controls.Add(this.pictureBoxLogo);
            this.panelTop.Controls.Add(this.lblSearch);
            this.panelTop.Controls.Add(this.txtSearch);
            this.panelTop.Controls.Add(this.btnAdd);
            this.panelTop.Controls.Add(this.btnEdit);
            this.panelTop.Controls.Add(this.btnDelete);
            this.panelTop.Controls.Add(this.btnMaterials);
            this.panelTop.Controls.Add(this.btnExit);
            this.panelTop.Dock = DockStyle.Top;
            this.panelTop.Location = new Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new Size(1000, 60);
            this.panelTop.TabIndex = 0;
            this.panelTop.MouseDown += new MouseEventHandler(this.PanelTop_MouseDown);
            this.panelTop.MouseMove += new MouseEventHandler(this.PanelTop_MouseMove);
            this.panelTop.MouseUp += new MouseEventHandler(this.PanelTop_MouseUp);
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Location = new Point(12, 5);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new Size(50, 50);
            this.pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 8;
            this.pictureBoxLogo.TabStop = false;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new Font("Gabriola", 14F);
            this.lblSearch.ForeColor = Color.FromArgb(45, 96, 51);
            this.lblSearch.Location = new Point(500, 15);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new Size(58, 35);
            this.lblSearch.TabIndex = 7;
            this.lblSearch.Text = "Поиск:";
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new Font("Gabriola", 12F);
            this.txtSearch.Location = new Point(560, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new Size(300, 35);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.TextChanged += new EventHandler(this.txtSearch_TextChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = Color.FromArgb(45, 96, 51);
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.Font = new Font("Gabriola", 14F);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Location = new Point(80, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(100, 35);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = Color.FromArgb(45, 96, 51);
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = FlatStyle.Flat;
            this.btnEdit.Font = new Font("Gabriola", 14F);
            this.btnEdit.ForeColor = Color.White;
            this.btnEdit.Location = new Point(190, 12);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(100, 35);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Изменить";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = Color.FromArgb(45, 96, 51);
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Gabriola", 14F);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Location = new Point(300, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(100, 35);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            // 
            // btnMaterials
            // 
            this.btnMaterials.BackColor = Color.FromArgb(45, 96, 51);
            this.btnMaterials.FlatAppearance.BorderSize = 0;
            this.btnMaterials.FlatStyle = FlatStyle.Flat;
            this.btnMaterials.Font = new Font("Gabriola", 14F);
            this.btnMaterials.ForeColor = Color.White;
            this.btnMaterials.Location = new Point(410, 12);
            this.btnMaterials.Name = "btnMaterials";
            this.btnMaterials.Size = new Size(100, 35);
            this.btnMaterials.TabIndex = 4;
            this.btnMaterials.Text = "Материалы";
            this.btnMaterials.UseVisualStyleBackColor = false;
            this.btnMaterials.Click += new EventHandler(this.btnMaterials_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
            this.btnExit.BackColor = Color.FromArgb(45, 96, 51);
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = FlatStyle.Flat;
            this.btnExit.Font = new Font("Gabriola", 14F);
            this.btnExit.ForeColor = Color.White;
            this.btnExit.Location = new Point(888, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new Size(100, 35);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Выход";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new EventHandler(this.btnExit_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right);
            this.dataGridView.BackgroundColor = Color.White;
            this.dataGridView.BorderStyle = BorderStyle.None;
            this.dataGridView.Location = new Point(12, 70);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new Size(976, 478);
            this.dataGridView.TabIndex = 1;
            this.dataGridView.ReadOnly = true;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.MultiSelect = false;
            this.dataGridView.RowHeadersVisible = false;
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new Font("Gabriola", 14F);
            this.lblTotal.ForeColor = Color.FromArgb(45, 96, 51);
            this.lblTotal.Location = new Point(12, 558);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new Size(144, 35);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "Всего продукции: 0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(1000, 600);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = FormBorderStyle.None;
            this.MinimumSize = new Size(1000, 600);
            this.Name = "MainForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Our Decor";
            this.FormClosing += new FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
