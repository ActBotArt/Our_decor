namespace Our_decor.Forms
{
    partial class ProductForm
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
            this.lblArticle = new System.Windows.Forms.Label();
            this.txtArticle = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblMinCost = new System.Windows.Forms.Label();
            this.numMinCost = new System.Windows.Forms.NumericUpDown();
            this.lblWidth = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblNameError = new System.Windows.Forms.Label();
            this.lblPriceError = new System.Windows.Forms.Label();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(96)))), ((int)(((byte)(51)))));
            this.panelTop.Controls.Add(this.lblTitle);
            this.panelTop.Controls.Add(this.btnClose);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(667, 49);
            this.panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Gabriola", 16F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(13, 6);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(225, 50);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Добавление продукции";
            // 
            // btnClose
            // 
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Gabriola", 14F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(613, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(53, 49);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "×";
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblArticle
            // 
            this.lblArticle.AutoSize = true;
            this.lblArticle.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblArticle.Location = new System.Drawing.Point(27, 74);
            this.lblArticle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblArticle.Name = "lblArticle";
            this.lblArticle.Size = new System.Drawing.Size(94, 45);
            this.lblArticle.TabIndex = 1;
            this.lblArticle.Text = "Артикул:";
            // 
            // txtArticle
            // 
            this.txtArticle.Font = new System.Drawing.Font("Gabriola", 12F);
            this.txtArticle.Location = new System.Drawing.Point(200, 74);
            this.txtArticle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtArticle.Name = "txtArticle";
            this.txtArticle.Size = new System.Drawing.Size(425, 41);
            this.txtArticle.TabIndex = 2;
            this.txtArticle.TextChanged += new System.EventHandler(this.ValidateInput);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblType.Location = new System.Drawing.Point(27, 123);
            this.lblType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(132, 45);
            this.lblType.TabIndex = 3;
            this.lblType.Text = "Тип продукта:";
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Font = new System.Drawing.Font("Gabriola", 12F);
            this.cmbType.Location = new System.Drawing.Point(200, 123);
            this.cmbType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(425, 45);
            this.cmbType.TabIndex = 4;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.ValidateInput);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblName.Location = new System.Drawing.Point(27, 172);
            this.lblName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(138, 45);
            this.lblName.TabIndex = 5;
            this.lblName.Text = "Наименование:";
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Gabriola", 12F);
            this.txtName.Location = new System.Drawing.Point(200, 172);
            this.txtName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(425, 41);
            this.txtName.TabIndex = 6;
            this.txtName.TextChanged += new System.EventHandler(this.ValidateInput);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblDescription.Location = new System.Drawing.Point(27, 234);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(100, 45);
            this.lblDescription.TabIndex = 8;
            this.lblDescription.Text = "Описание:";
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Gabriola", 12F);
            this.txtDescription.Location = new System.Drawing.Point(200, 234);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(425, 73);
            this.txtDescription.TabIndex = 9;
            // 
            // lblMinCost
            // 
            this.lblMinCost.AutoSize = true;
            this.lblMinCost.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblMinCost.Location = new System.Drawing.Point(27, 320);
            this.lblMinCost.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMinCost.Name = "lblMinCost";
            this.lblMinCost.Size = new System.Drawing.Size(155, 45);
            this.lblMinCost.TabIndex = 10;
            this.lblMinCost.Text = "Мин. стоимость:";
            // 
            // numMinCost
            // 
            this.numMinCost.DecimalPlaces = 2;
            this.numMinCost.Font = new System.Drawing.Font("Gabriola", 12F);
            this.numMinCost.Location = new System.Drawing.Point(200, 320);
            this.numMinCost.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numMinCost.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numMinCost.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numMinCost.Name = "numMinCost";
            this.numMinCost.Size = new System.Drawing.Size(120, 41);
            this.numMinCost.TabIndex = 11;
            this.numMinCost.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numMinCost.ValueChanged += new System.EventHandler(this.ValidateInput);
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblWidth.Location = new System.Drawing.Point(27, 369);
            this.lblWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(148, 45);
            this.lblWidth.TabIndex = 13;
            this.lblWidth.Text = "Ширина рулона:";
            // 
            // numWidth
            // 
            this.numWidth.DecimalPlaces = 2;
            this.numWidth.Font = new System.Drawing.Font("Gabriola", 12F);
            this.numWidth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numWidth.Location = new System.Drawing.Point(200, 369);
            this.numWidth.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numWidth.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(120, 41);
            this.numWidth.TabIndex = 14;
            this.numWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numWidth.ValueChanged += new System.EventHandler(this.ValidateInput);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(96)))), ((int)(((byte)(51)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Gabriola", 14F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(133, 431);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(187, 49);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(96)))), ((int)(((byte)(51)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Gabriola", 14F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(347, 431);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(187, 49);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblNameError
            // 
            this.lblNameError.AutoSize = true;
            this.lblNameError.Font = new System.Drawing.Font("Gabriola", 10F);
            this.lblNameError.ForeColor = System.Drawing.Color.Red;
            this.lblNameError.Location = new System.Drawing.Point(200, 209);
            this.lblNameError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNameError.Name = "lblNameError";
            this.lblNameError.Size = new System.Drawing.Size(0, 31);
            this.lblNameError.TabIndex = 7;
            this.lblNameError.Visible = false;
            // 
            // lblPriceError
            // 
            this.lblPriceError.AutoSize = true;
            this.lblPriceError.Font = new System.Drawing.Font("Gabriola", 10F);
            this.lblPriceError.ForeColor = System.Drawing.Color.Red;
            this.lblPriceError.Location = new System.Drawing.Point(413, 326);
            this.lblPriceError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPriceError.Name = "lblPriceError";
            this.lblPriceError.Size = new System.Drawing.Size(0, 31);
            this.lblPriceError.TabIndex = 12;
            this.lblPriceError.Visible = false;
            // 
            // ProductForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(217)))), ((int)(((byte)(178)))));
            this.ClientSize = new System.Drawing.Size(667, 517);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.lblArticle);
            this.Controls.Add(this.txtArticle);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblNameError);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblMinCost);
            this.Controls.Add(this.numMinCost);
            this.Controls.Add(this.lblPriceError);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.numWidth);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ProductForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblArticle;
        private System.Windows.Forms.TextBox txtArticle;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblNameError;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblMinCost;
        private System.Windows.Forms.NumericUpDown numMinCost;
        private System.Windows.Forms.Label lblPriceError;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}