// MaterialsForm.Designer.cs
using System;
using System.Windows.Forms;
using System.Drawing;

namespace Our_decor.Forms
{
    partial class MaterialsForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelTop;
        private Label lblTitle;
        private Button btnClose;
        private DataGridView dataGridView;
        private Label lblTotal;
        private PictureBox pictureBoxLogo;
        private Button btnAdd;
        private Button btnDelete;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblDateTime;
        private ToolStripStatusLabel lblUser;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelTop = new Panel();
            this.lblTitle = new Label();
            this.btnClose = new Button();
            this.dataGridView = new DataGridView();
            this.lblTotal = new Label();
            this.pictureBoxLogo = new PictureBox();
            this.btnAdd = new Button();
            this.btnDelete = new Button();
            this.statusStrip = new StatusStrip();
            this.lblDateTime = new ToolStripStatusLabel();
            this.lblUser = new ToolStripStatusLabel();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.panelTop.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // 
            // panelTop
            // 
            this.panelTop.BackColor = Color.FromArgb(187, 217, 178);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.btnClose);
            this.panelTop.Dock = DockStyle.Top;
            this.panelTop.Location = new Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new Size(800, 45);
            this.panelTop.TabIndex = 0;
            this.panelTop.Padding = new Padding(12, 5, 120, 5);
            this.panelTop.MouseDown += new MouseEventHandler(this.PanelTop_MouseDown);
            this.panelTop.MouseMove += new MouseEventHandler(this.PanelTop_MouseMove);
            this.panelTop.MouseUp += new MouseEventHandler(this.PanelTop_MouseUp);

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Gabriola", 16F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(45, 96, 51);
            this.lblTitle.Location = new Point(12, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(200, 35);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Материалы";

            // 
            // btnClose
            // 
            this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnClose.BackColor = Color.FromArgb(45, 96, 51);
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.Font = new Font("Gabriola", 14F);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.Location = new Point(688, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(100, 35);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = Color.FromArgb(45, 96, 51);
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.Font = new Font("Gabriola", 14F);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Location = new Point(12, 50);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(150, 35);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Добавить материал";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            this.btnAdd.Visible = false;

            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = Color.FromArgb(45, 96, 51);
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Gabriola", 14F);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Location = new Point(170, 50);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(150, 35);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Удалить материал";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.btnDelete.Visible = false;

            // 
            // dataGridView
            // 
            this.dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dataGridView.BackgroundColor = Color.White;
            this.dataGridView.BorderStyle = BorderStyle.None;
            this.dataGridView.Location = new Point(12, 90);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new Size(776, 430);
            this.dataGridView.TabIndex = 3;
            this.dataGridView.RowHeadersVisible = false;

            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new Font("Gabriola", 14F);
            this.lblTotal.ForeColor = Color.FromArgb(45, 96, 51);
            this.lblTotal.Location = new Point(12, 530);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new Size(200, 35);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "Всего материалов: 0";

            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.pictureBoxLogo.BackColor = Color.Transparent;
            this.pictureBoxLogo.Location = new Point(668, 458);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new Size(120, 100);
            this.pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 5;
            this.pictureBoxLogo.TabStop = false;

            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = Color.FromArgb(187, 217, 178);
            this.statusStrip.Items.AddRange(new ToolStripItem[] {
                this.lblDateTime,
                new ToolStripSeparator(),
                this.lblUser
            });
            this.statusStrip.Location = new Point(0, 578);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new Size(800, 22);
            this.statusStrip.TabIndex = 6;
            this.statusStrip.SizingGrip = false;

            // 
            // lblDateTime
            // 
            this.lblDateTime.Font = new Font("Segoe UI", 9F);
            this.lblDateTime.ForeColor = Color.FromArgb(45, 96, 51);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new Size(350, 17);
            this.lblDateTime.Text = "Дата и время (UTC): ";

            // 
            // lblUser
            // 
            this.lblUser.Font = new Font("Segoe UI", 9F);
            this.lblUser.ForeColor = Color.FromArgb(45, 96, 51);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new Size(200, 17);
            this.lblUser.Text = "Пользователь: ";

            // 
            // MaterialsForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(800, 600);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.panelTop);
            this.FormBorderStyle = FormBorderStyle.None;
            this.MinimumSize = new Size(800, 600);
            this.Name = "MaterialsForm";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Материалы";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
