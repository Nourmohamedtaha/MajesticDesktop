namespace MajesticDesktop.Forms
{
    partial class frmCategories
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.pnlCategoryData = new System.Windows.Forms.Panel();
            this.lblCategoryName = new System.Windows.Forms.Label();
            this.txtCategoryName = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblCategoriesTitle = new System.Windows.Forms.Label();
            this.dgvCategories = new System.Windows.Forms.DataGridView();
            this.pnlHeader.SuspendLayout();
            this.pnlCategoryData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).BeginInit();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1000, 90);
            this.pnlHeader.TabIndex = 0;
            this.pnlHeader.Click += new System.EventHandler(this.ClearSelection_Click);

            // lblTitle
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1000, 50);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📁 إدارة الأقسام";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Click += new System.EventHandler(this.ClearSelection_Click);

            // lblSubtitle
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblSubtitle.Location = new System.Drawing.Point(0, 52);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(1000, 38);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "إضافة وتعديل وتنظيم أقسام المنتجات في المخزن الرئيسي";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSubtitle.Click += new System.EventHandler(this.ClearSelection_Click);

            // pnlCategoryData
            this.pnlCategoryData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlCategoryData.Controls.Add(this.lblCategoryName);
            this.pnlCategoryData.Controls.Add(this.txtCategoryName);
            this.pnlCategoryData.Controls.Add(this.lblDescription);
            this.pnlCategoryData.Controls.Add(this.txtDescription);
            this.pnlCategoryData.Controls.Add(this.chkIsActive);
            this.pnlCategoryData.Controls.Add(this.btnAdd);
            this.pnlCategoryData.Controls.Add(this.btnUpdate);
            this.pnlCategoryData.Controls.Add(this.btnDelete);
            this.pnlCategoryData.Controls.Add(this.btnClear);
            this.pnlCategoryData.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCategoryData.Location = new System.Drawing.Point(0, 90);
            this.pnlCategoryData.Name = "pnlCategoryData";
            this.pnlCategoryData.Size = new System.Drawing.Size(1000, 235);
            this.pnlCategoryData.TabIndex = 1;
            this.pnlCategoryData.Click += new System.EventHandler(this.ClearSelection_Click);

            // lblCategoryName
            this.lblCategoryName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCategoryName.AutoSize = true;
            this.lblCategoryName.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblCategoryName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCategoryName.Location = new System.Drawing.Point(720, 35);
            this.lblCategoryName.Name = "lblCategoryName";
            this.lblCategoryName.Size = new System.Drawing.Size(89, 21);
            this.lblCategoryName.TabIndex = 0;
            this.lblCategoryName.Text = "اسم القسم";

            // txtCategoryName
            this.txtCategoryName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCategoryName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtCategoryName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCategoryName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtCategoryName.ForeColor = System.Drawing.Color.White;
            this.txtCategoryName.Location = new System.Drawing.Point(450, 31);
            this.txtCategoryName.Name = "txtCategoryName";
            this.txtCategoryName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtCategoryName.Size = new System.Drawing.Size(250, 29);
            this.txtCategoryName.TabIndex = 1;

            // lblDescription
            this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblDescription.Location = new System.Drawing.Point(720, 72);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 21);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "الوصف";

            // txtDescription
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtDescription.ForeColor = System.Drawing.Color.White;
            this.txtDescription.Location = new System.Drawing.Point(450, 68);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtDescription.Size = new System.Drawing.Size(250, 55);
            this.txtDescription.TabIndex = 3;

            // chkIsActive
            this.chkIsActive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.chkIsActive.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.chkIsActive.Location = new System.Drawing.Point(450, 135);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkIsActive.Size = new System.Drawing.Size(104, 25);
            this.chkIsActive.TabIndex = 4;
            this.chkIsActive.Text = "القسم نشط";
            this.chkIsActive.UseVisualStyleBackColor = true;

            // btnAdd
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(295, 28);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 38);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "➕ إضافة";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // btnUpdate
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnUpdate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdate.FlatAppearance.BorderSize = 0;
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(155, 28);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(120, 38);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "✏️ تعديل";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            // btnDelete
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(20, 28);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 38);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "🗑️ حذف";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // btnClear
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(20, 80);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(395, 38);
            this.btnClear.TabIndex = 8;
            this.btnClear.Text = "🔄 مسح وتفريغ الحقول";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // lblCategoriesTitle
            this.lblCategoriesTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCategoriesTitle.AutoSize = true;
            this.lblCategoriesTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblCategoriesTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lblCategoriesTitle.Location = new System.Drawing.Point(860, 340);
            this.lblCategoriesTitle.Name = "lblCategoriesTitle";
            this.lblCategoriesTitle.Size = new System.Drawing.Size(117, 25);
            this.lblCategoriesTitle.TabIndex = 2;
            this.lblCategoriesTitle.Text = "قائمة الأقسام";
            this.lblCategoriesTitle.Click += new System.EventHandler(this.ClearSelection_Click);

            // dgvCategories
            this.dgvCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCategories.Location = new System.Drawing.Point(25, 375);
            this.dgvCategories.Name = "dgvCategories";
            this.dgvCategories.Size = new System.Drawing.Size(950, 300);
            this.dgvCategories.TabIndex = 3;
            this.dgvCategories.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCategories_CellClick);

            // frmCategories
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(25)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.dgvCategories);
            this.Controls.Add(this.lblCategoriesTitle);
            this.Controls.Add(this.pnlCategoryData);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "frmCategories";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إدارة الأقسام";
            this.Load += new System.EventHandler(this.frmCategories_Load);
            this.Click += new System.EventHandler(this.ClearSelection_Click);
            this.pnlHeader.ResumeLayout(false);
            this.pnlCategoryData.ResumeLayout(false);
            this.pnlCategoryData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel pnlCategoryData;
        private System.Windows.Forms.Label lblCategoryName;
        private System.Windows.Forms.TextBox txtCategoryName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblCategoriesTitle;
        private System.Windows.Forms.DataGridView dgvCategories;
    }
}