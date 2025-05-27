namespace Our_decor.Forms
{
    partial class MaterialEditForm
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblMaterialType = new System.Windows.Forms.Label();
            this.cmbMaterialType = new System.Windows.Forms.ComboBox();
            this.lblMaterialName = new System.Windows.Forms.Label();
            this.txtMaterialName = new System.Windows.Forms.TextBox();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.lblCost = new System.Windows.Forms.Label();
            this.txtCost = new System.Windows.Forms.TextBox();
            this.lblUnit = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.lblStockQuantity = new System.Windows.Forms.Label();
            this.txtStockQuantity = new System.Windows.Forms.TextBox();
            this.lblMinQuantity = new System.Windows.Forms.Label();
            this.txtMinQuantity = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblDateTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUser = new System.Windows.Forms.ToolStripStatusLabel();

            this.panelTop.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();

            // panelTop
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(45, 96, 51);
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.btnClose);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Height = 40;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Gabriola", 16F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(10, 5);
            this.lblTitle.Text = "Добавление материала";

            // btnClose
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Font = new System.Drawing.Font("Gabriola", 14F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(460, 0);
            this.btnClose.Size = new System.Drawing.Size(40, 40);
            this.btnClose.Text = "×";
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);

            // lblMaterialType
            this.lblMaterialType.AutoSize = true;
            this.lblMaterialType.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblMaterialType.Location = new System.Drawing.Point(20, 60);
            this.lblMaterialType.Text = "Тип материала:";

            // cmbMaterialType
            this.cmbMaterialType.Font = new System.Drawing.Font("Gabriola", 12F);
            this.cmbMaterialType.Location = new System.Drawing.Point(150, 60);
            this.cmbMaterialType.Size = new System.Drawing.Size(320, 30);
            this.cmbMaterialType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // lblMaterialName
            this.lblMaterialName.AutoSize = true;
            this.lblMaterialName.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblMaterialName.Location = new System.Drawing.Point(20, 100);
            this.lblMaterialName.Text = "Наименование:";

            // txtMaterialName
            this.txtMaterialName.Font = new System.Drawing.Font("Gabriola", 12F);
            this.txtMaterialName.Location = new System.Drawing.Point(150, 100);
            this.txtMaterialName.Size = new System.Drawing.Size(320, 30);

            // lblQuantity
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblQuantity.Location = new System.Drawing.Point(20, 140);
            this.lblQuantity.Text = "Количество:";

            // txtQuantity
            this.txtQuantity.Font = new System.Drawing.Font("Gabriola", 12F);
            this.txtQuantity.Location = new System.Drawing.Point(150, 140);
            this.txtQuantity.Size = new System.Drawing.Size(150, 30);

            // lblCost
            this.lblCost.AutoSize = true;
            this.lblCost.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblCost.Location = new System.Drawing.Point(20, 180);
            this.lblCost.Text = "Цена:";

            // txtCost
            this.txtCost.Font = new System.Drawing.Font("Gabriola", 12F);
            this.txtCost.Location = new System.Drawing.Point(150, 180);
            this.txtCost.Size = new System.Drawing.Size(150, 30);

            // lblUnit
            this.lblUnit.AutoSize = true;
            this.lblUnit.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblUnit.Location = new System.Drawing.Point(20, 220);
            this.lblUnit.Text = "Единица измерения:";

            // txtUnit
            this.txtUnit.Font = new System.Drawing.Font("Gabriola", 12F);
            this.txtUnit.Location = new System.Drawing.Point(150, 220);
            this.txtUnit.Size = new System.Drawing.Size(150, 30);

            // lblStockQuantity
            this.lblStockQuantity.AutoSize = true;
            this.lblStockQuantity.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblStockQuantity.Location = new System.Drawing.Point(20, 260);
            this.lblStockQuantity.Text = "Количество на складе:";

            // txtStockQuantity
            this.txtStockQuantity.Font = new System.Drawing.Font("Gabriola", 12F);
            this.txtStockQuantity.Location = new System.Drawing.Point(150, 260);
            this.txtStockQuantity.Size = new System.Drawing.Size(150, 30);

            // lblMinQuantity
            this.lblMinQuantity.AutoSize = true;
            this.lblMinQuantity.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblMinQuantity.Location = new System.Drawing.Point(20, 300);
            this.lblMinQuantity.Text = "Минимальное количество:";

            // txtMinQuantity
            this.txtMinQuantity.Font = new System.Drawing.Font("Gabriola", 12F);
            this.txtMinQuantity.Location = new System.Drawing.Point(150, 300);
            this.txtMinQuantity.Size = new System.Drawing.Size(150, 30);

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(45, 96, 51);
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Font = new System.Drawing.Font("Gabriola", 14F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(100, 350);
            this.btnSave.Size = new System.Drawing.Size(140, 40);
            this.btnSave.Text = "Сохранить";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(45, 96, 51);
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.Font = new System.Drawing.Font("Gabriola", 14F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(260, 350);
            this.btnCancel.Size = new System.Drawing.Size(140, 40);
            this.btnCancel.Text = "Отмена";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // statusStrip
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(187, 217, 178);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.lblDateTime,
                new System.Windows.Forms.ToolStripSeparator(),
                this.lblUser
            });
            this.statusStrip.Location = new System.Drawing.Point(0, 428);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(500, 22);

            // lblDateTime
            this.lblDateTime.Font = new System.Drawing.Font("Gabriola", 12F);
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(45, 96, 51);

            // lblUser
            this.lblUser.Font = new System.Drawing.Font("Gabriola", 12F);
            this.lblUser.ForeColor = System.Drawing.Color.FromArgb(45, 96, 51);

            // MaterialEditForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(187, 217, 178);
            this.ClientSize = new System.Drawing.Size(500, 450);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.panelTop,
                this.lblMaterialType,
                this.cmbMaterialType,
                this.lblMaterialName,
                this.txtMaterialName,
                this.lblQuantity,
                this.txtQuantity,
                this.lblCost,
                this.txtCost,
                this.lblUnit,
                this.txtUnit,
                this.lblStockQuantity,
                this.txtStockQuantity,
                this.lblMinQuantity,
                this.txtMinQuantity,
                this.btnSave,
                this.btnCancel,
                this.statusStrip
            });
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblMaterialType;
        private System.Windows.Forms.ComboBox cmbMaterialType;
        private System.Windows.Forms.Label lblMaterialName;
        private System.Windows.Forms.TextBox txtMaterialName;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Label lblCost;
        private System.Windows.Forms.TextBox txtCost;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label lblStockQuantity;
        private System.Windows.Forms.TextBox txtStockQuantity;
        private System.Windows.Forms.Label lblMinQuantity;
        private System.Windows.Forms.TextBox txtMinQuantity;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblDateTime;
        private System.Windows.Forms.ToolStripStatusLabel lblUser;
    }
}