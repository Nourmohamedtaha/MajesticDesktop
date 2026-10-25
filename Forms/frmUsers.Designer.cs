namespace MajesticDesktop.Forms
{
    partial class frmUsers
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
            this.btnPermissions = new System.Windows.Forms.Button();
            this.pnlDataEntry = new System.Windows.Forms.Panel();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.colM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsername = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPassword = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.pnlHeader.SuspendLayout();
            this.pnlDataEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(20, 30, 45); // Dark Theme Header
            this.pnlHeader.Controls.Add(this.btnPermissions);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1000, 85);
            this.pnlHeader.TabIndex = 0;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(680, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(290, 35);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "المستخدمين والصلاحيات";

            // lblSubtitle
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.DarkGray;
            this.lblSubtitle.Location = new System.Drawing.Point(530, 51);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(300, 19);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "إدارة حسابات الفروع والمخزن بنظام (مدير / مسئول فرع)";

            // btnPermissions
            this.btnPermissions.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnPermissions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPermissions.FlatAppearance.BorderSize = 0;
            this.btnPermissions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPermissions.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnPermissions.ForeColor = System.Drawing.Color.White;
            this.btnPermissions.Location = new System.Drawing.Point(25, 25);
            this.btnPermissions.Name = "btnPermissions";
            this.btnPermissions.Size = new System.Drawing.Size(220, 40);
            this.btnPermissions.TabIndex = 2;
            this.btnPermissions.Text = "دليل الصلاحيات 🔐";
            this.btnPermissions.UseVisualStyleBackColor = false;
            this.btnPermissions.Click += new System.EventHandler(this.btnPermissions_Click);

            // pnlDataEntry
            this.pnlDataEntry.BackColor = System.Drawing.Color.FromArgb(30, 41, 59); // Dark Panel
            this.pnlDataEntry.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pnlDataEntry.Controls.Add(this.lblFullName);
            this.pnlDataEntry.Controls.Add(this.txtFullName);
            this.pnlDataEntry.Controls.Add(this.lblUsername);
            this.pnlDataEntry.Controls.Add(this.txtUsername);
            this.pnlDataEntry.Controls.Add(this.lblPassword);
            this.pnlDataEntry.Controls.Add(this.txtPassword);
            this.pnlDataEntry.Controls.Add(this.lblRole);
            this.pnlDataEntry.Controls.Add(this.cmbRole);
            this.pnlDataEntry.Controls.Add(this.lblLocation);
            this.pnlDataEntry.Controls.Add(this.cmbLocation);
            this.pnlDataEntry.Controls.Add(this.chkIsActive);
            this.pnlDataEntry.Controls.Add(this.btnSave);
            this.pnlDataEntry.Controls.Add(this.btnEdit);
            this.pnlDataEntry.Controls.Add(this.btnClear);
            this.pnlDataEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDataEntry.Location = new System.Drawing.Point(0, 85);
            this.pnlDataEntry.Name = "pnlDataEntry";
            this.pnlDataEntry.Size = new System.Drawing.Size(1000, 195);
            this.pnlDataEntry.TabIndex = 1;

            // Labels and Inputs
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblFullName.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblFullName.Location = new System.Drawing.Point(720, 18);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Text = "الاسم بالكامل (عربي)";

            this.txtFullName.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtFullName.ForeColor = System.Drawing.Color.White;
            this.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFullName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtFullName.Location = new System.Drawing.Point(720, 40);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(250, 29);

            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblUsername.Location = new System.Drawing.Point(490, 18);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Text = "اسم الدخول (Username)";

            this.txtUsername.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtUsername.ForeColor = System.Drawing.Color.White;
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtUsername.Location = new System.Drawing.Point(490, 40);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(200, 29);

            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblPassword.Location = new System.Drawing.Point(260, 18);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Text = "كلمة المرور (Password)";

            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtPassword.ForeColor = System.Drawing.Color.White;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtPassword.Location = new System.Drawing.Point(260, 40);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 29);

            this.lblRole.AutoSize = true;
            this.lblRole.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblRole.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblRole.Location = new System.Drawing.Point(30, 18);
            this.lblRole.Name = "lblRole";
            this.lblRole.Text = "مستوى الصلاحية";

            this.cmbRole.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.cmbRole.ForeColor = System.Drawing.Color.White;
            this.cmbRole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.cmbRole.Location = new System.Drawing.Point(30, 40);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(200, 29);
            this.cmbRole.SelectedIndexChanged += new System.EventHandler(this.cmbRole_SelectedIndexChanged);

            // Row 2
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLocation.ForeColor = System.Drawing.Color.FromArgb(245, 158, 11);
            this.lblLocation.Location = new System.Drawing.Point(670, 80);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Text = "الفرع (إجباري لمستوى مسئول الفرع فقط)";

            this.cmbLocation.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.cmbLocation.ForeColor = System.Drawing.Color.White;
            this.cmbLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.cmbLocation.Location = new System.Drawing.Point(670, 105);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(300, 29);
            this.cmbLocation.Enabled = false;

            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.chkIsActive.ForeColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.chkIsActive.Location = new System.Drawing.Point(450, 107);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Text = "حساب نشط (يمكنه الدخول)";
            this.chkIsActive.UseVisualStyleBackColor = true;

            // Buttons
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(34, 197, 94);
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(290, 140);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 40);
            this.btnSave.Text = "حفظ ✔";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(160, 140);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(120, 40);
            this.btnEdit.Text = "تعديل ✏️";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            this.btnClear.BackColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(30, 140);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 40);
            this.btnClear.Text = "تفريغ 🧹";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // DataGridView
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.BackgroundColor = System.Drawing.Color.FromArgb(15, 23, 42); // Dark bg
            this.dgvUsers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.Location = new System.Drawing.Point(0, 280);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersVisible = false;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(1000, 420);
            this.dgvUsers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellClick);

            this.colM.Name = "colM";
            this.colM.HeaderText = "م";
            this.colM.Width = 60;

            this.colId.Name = "colId";
            this.colId.Visible = false;

            this.colFullName.Name = "colFullName";
            this.colFullName.HeaderText = "الاسم بالكامل";
            this.colFullName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;

            this.colUsername.Name = "colUsername";
            this.colUsername.HeaderText = "اسم الدخول";
            this.colUsername.Width = 150;

            this.colPassword.Name = "colPassword";
            this.colPassword.HeaderText = "كلمة المرور";
            this.colPassword.Width = 150;

            this.colRole.Name = "colRole";
            this.colRole.HeaderText = "الصلاحية";
            this.colRole.Width = 130;

            this.colLocation.Name = "colLocation";
            this.colLocation.HeaderText = "الفرع";
            this.colLocation.Width = 150;

            this.colStatus.Name = "colStatus";
            this.colStatus.HeaderText = "الحالة";
            this.colStatus.Width = 100;

            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                this.colM,
                this.colId,
                this.colFullName,
                this.colUsername,
                this.colPassword,
                this.colRole,
                this.colLocation,
                this.colStatus
            });

            // Form Properties
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.dgvUsers);
            this.Controls.Add(this.pnlDataEntry);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "frmUsers";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "المستخدمين والصلاحيات";
            this.Load += new System.EventHandler(this.frmUsers_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlDataEntry.ResumeLayout(false);
            this.pnlDataEntry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Button btnPermissions;
        private System.Windows.Forms.Panel pnlDataEntry;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.DataGridViewTextBoxColumn colM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsername;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPassword;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRole;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    }
}