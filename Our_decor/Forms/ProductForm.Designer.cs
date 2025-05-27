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

            // Добавляем метки для ошибок
            this.lblNameError = new System.Windows.Forms.Label();
            this.lblPriceError = new System.Windows.Forms.Label();

            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
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
            this.lblTitle.Text = "Добавление продукции";

            // btnClose
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Font = new System.Drawing.Font("Gabriola", 14F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(460, 0);
            this.btnClose.Size = new System.Drawing.Size(40, 40);
            this.btnClose.Text = "×";
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);

            // lblArticle
            this.lblArticle.AutoSize = true;
            this.lblArticle.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblArticle.Location = new System.Drawing.Point(20, 60);
            this.lblArticle.Text = "Артикул:";

            // txtArticle
            this.txtArticle.Font = new System.Drawing.Font("Gabriola", 12F);
            this.txtArticle.Location = new System.Drawing.Point(150, 60);
            this.txtArticle.Size = new System.Drawing.Size(320, 30);

            // lblType
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblType.Location = new System.Drawing.Point(20, 100);
            this.lblType.Text = "Тип продукта:";

            // cmbType
            this.cmbType.Font = new System.Drawing.Font("Gabriola", 12F);
            this.cmbType.Location = new System.Drawing.Point(150, 100);
            this.cmbType.Size = new System.Drawing.Size(320, 30);
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblName.Location = new System.Drawing.Point(20, 140);
            this.lblName.Text = "Наименование:";

            // txtName
            this.txtName.Font = new System.Drawing.Font("Gabriola", 12F);
            this.txtName.Location = new System.Drawing.Point(150, 140);
            this.txtName.Size = new System.Drawing.Size(320, 30);

            // lblNameError - метка ошибки для названия
            this.lblNameError.AutoSize = true;
            this.lblNameError.Font = new System.Drawing.Font("Gabriola", 10F);
            this.lblNameError.ForeColor = System.Drawing.Color.Red;
            this.lblNameError.Location = new System.Drawing.Point(150, 170);
            this.lblNameError.Size = new System.Drawing.Size(320, 15);
            this.lblNameError.Text = "";
            this.lblNameError.Visible = false;

            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblDescription.Location = new System.Drawing.Point(20, 190);
            this.lblDescription.Text = "Описание:";

            // txtDescription
            this.txtDescription.Font = new System.Drawing.Font("Gabriola", 12F);
            this.txtDescription.Location = new System.Drawing.Point(150, 190);
            this.txtDescription.Size = new System.Drawing.Size(320, 60);
            this.txtDescription.Multiline = true;

            // lblMinCost
            this.lblMinCost.AutoSize = true;
            this.lblMinCost.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblMinCost.Location = new System.Drawing.Point(20, 260);
            this.lblMinCost.Text = "Мин. стоимость:";

            // numMinCost
            this.numMinCost.Font = new System.Drawing.Font("Gabriola", 12F);
            this.numMinCost.Location = new System.Drawing.Point(150, 260);
            this.numMinCost.Size = new System.Drawing.Size(150, 30);
            this.numMinCost.DecimalPlaces = 2;
            this.numMinCost.Maximum = 1000000;
            this.numMinCost.Minimum = 0.01M;

            // lblPriceError - метка ошибки для цены
            this.lblPriceError.AutoSize = true;
            this.lblPriceError.Font = new System.Drawing.Font("Gabriola", 10F);
            this.lblPriceError.ForeColor = System.Drawing.Color.Red;
            this.lblPriceError.Location = new System.Drawing.Point(310, 265);
            this.lblPriceError.Size = new System.Drawing.Size(160, 15);
            this.lblPriceError.Text = "";
            this.lblPriceError.Visible = false;

            // lblWidth
            this.lblWidth.AutoSize = true;
            this.lblWidth.Font = new System.Drawing.Font("Gabriola", 14F);
            this.lblWidth.Location = new System.Drawing.Point(20, 300);
            this.lblWidth.Text = "Ширина рулона:";

            // numWidth
            this.numWidth.Font = new System.Drawing.Font("Gabriola", 12F);
            this.numWidth.Location = new System.Drawing.Point(150, 300);
            this.numWidth.Size = new System.Drawing.Size(150, 30);
            this.numWidth.DecimalPlaces = 2;
            this.numWidth.Maximum = 10;
            this.numWidth.Minimum = 0.01M;
            this.numWidth.Increment = 0.01M;

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

            // ProductForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(187, 217, 178);
            this.ClientSize = new System.Drawing.Size(500, 420);
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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            // Подписка на события валидации
            this.txtArticle.TextChanged += new System.EventHandler(this.ValidateInput);
            this.txtName.TextChanged += new System.EventHandler(this.ValidateInput);
            this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtName_KeyPress);
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.ValidateInput);
            this.numMinCost.ValueChanged += new System.EventHandler(this.ValidateInput);
            this.numWidth.ValueChanged += new System.EventHandler(this.ValidateInput);

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