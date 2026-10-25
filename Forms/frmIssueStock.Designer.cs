using System.Drawing;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    internal static class BranchThemeIssueStock
    {
        public static readonly Color Bg = Color.FromArgb(15, 23, 42);
        public static readonly Color Header = Color.FromArgb(9, 14, 28);
        public static readonly Color Surface = Color.FromArgb(30, 41, 59);
        public static readonly Color Field = Color.FromArgb(24, 34, 51);
        public static readonly Color Input = Color.FromArgb(15, 23, 42);
        public static readonly Color Border = Color.FromArgb(51, 65, 85);
        public static readonly Color Text = Color.FromArgb(241, 245, 249);
        public static readonly Color Muted = Color.FromArgb(148, 163, 184);
        public static readonly Color Accent = Color.FromArgb(59, 130, 246);
        public static readonly Color Green = Color.FromArgb(22, 163, 74);
        public static readonly Color Red = Color.FromArgb(220, 38, 38);

        public static Font UiFont(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font("Segoe UI", size, style);
        }
    }

    partial class frmIssueStock
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // التهيئة البسيطة جداً عشان الديزاينر ميضربش
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblToLocation = new System.Windows.Forms.Label();
            this.cmbToLocation = new System.Windows.Forms.ComboBox();
            this.lblFromLocation = new System.Windows.Forms.Label();
            this.cmbFromLocation = new System.Windows.Forms.ComboBox();
            this.lblIssueDate = new System.Windows.Forms.Label();
            this.dtpIssueDate = new System.Windows.Forms.DateTimePicker();
            this.pnlProductEntry = new System.Windows.Forms.Panel();
            this.lblAvailableQty = new System.Windows.Forms.Label();
            this.txtAvailableQty = new System.Windows.Forms.TextBox();
            this.btnRemoveProduct = new System.Windows.Forms.Button();
            this.btnAddProduct = new System.Windows.Forms.Button();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.dgvDetails = new System.Windows.Forms.DataGridView();
            this.colSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.txtIssueNotes = new System.Windows.Forms.TextBox();
            this.lblIssueNotes = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.pnlProductEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1200, 90);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = false;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Height = 45;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "صرف البضاعة للفروع 🚚";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = false;
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubtitle.Height = 35;
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "نقل البضاعة من المخزن الرئيسي إلى الفروع الأخرى";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.lblToLocation);
            this.pnlInfo.Controls.Add(this.cmbToLocation);
            this.pnlInfo.Controls.Add(this.lblFromLocation);
            this.pnlInfo.Controls.Add(this.cmbFromLocation);
            this.pnlInfo.Controls.Add(this.lblIssueDate);
            this.pnlInfo.Controls.Add(this.dtpIssueDate);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfo.Location = new System.Drawing.Point(0, 90);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(1200, 100);
            this.pnlInfo.TabIndex = 1;
            // 
            // lblFromLocation
            // 
            this.lblFromLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFromLocation.AutoSize = true;
            this.lblFromLocation.Location = new System.Drawing.Point(1000, 20);
            this.lblFromLocation.Name = "lblFromLocation";
            this.lblFromLocation.Size = new System.Drawing.Size(152, 18);
            this.lblFromLocation.TabIndex = 0;
            this.lblFromLocation.Text = "من (المخزن المصدر):";
            // 
            // cmbFromLocation
            // 
            this.cmbFromLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFromLocation.FormattingEnabled = true;
            this.cmbFromLocation.Location = new System.Drawing.Point(820, 48);
            this.cmbFromLocation.Name = "cmbFromLocation";
            this.cmbFromLocation.Size = new System.Drawing.Size(330, 27);
            this.cmbFromLocation.TabIndex = 1;
            this.cmbFromLocation.SelectedIndexChanged += new System.EventHandler(this.cmbFromLocation_SelectedIndexChanged);
            // 
            // lblToLocation
            // 
            this.lblToLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblToLocation.AutoSize = true;
            this.lblToLocation.Location = new System.Drawing.Point(620, 20);
            this.lblToLocation.Name = "lblToLocation";
            this.lblToLocation.Size = new System.Drawing.Size(147, 18);
            this.lblToLocation.TabIndex = 4;
            this.lblToLocation.Text = "إلى (الفرع المستلم):";
            // 
            // cmbToLocation
            // 
            this.cmbToLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbToLocation.FormattingEnabled = true;
            this.cmbToLocation.Location = new System.Drawing.Point(440, 48);
            this.cmbToLocation.Name = "cmbToLocation";
            this.cmbToLocation.Size = new System.Drawing.Size(330, 27);
            this.cmbToLocation.TabIndex = 5;
            // 
            // lblIssueDate
            // 
            this.lblIssueDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIssueDate.AutoSize = true;
            this.lblIssueDate.Location = new System.Drawing.Point(230, 20);
            this.lblIssueDate.Name = "lblIssueDate";
            this.lblIssueDate.Size = new System.Drawing.Size(95, 18);
            this.lblIssueDate.TabIndex = 2;
            this.lblIssueDate.Text = "تاريخ الصرف:";
            // 
            // dtpIssueDate
            // 
            this.dtpIssueDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpIssueDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpIssueDate.Location = new System.Drawing.Point(50, 48);
            this.dtpIssueDate.Name = "dtpIssueDate";
            this.dtpIssueDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dtpIssueDate.RightToLeftLayout = true;
            this.dtpIssueDate.Size = new System.Drawing.Size(270, 27);
            this.dtpIssueDate.TabIndex = 3;
            // 
            // pnlProductEntry
            // 
            this.pnlProductEntry.Controls.Add(this.lblAvailableQty);
            this.pnlProductEntry.Controls.Add(this.txtAvailableQty);
            this.pnlProductEntry.Controls.Add(this.btnRemoveProduct);
            this.pnlProductEntry.Controls.Add(this.btnAddProduct);
            this.pnlProductEntry.Controls.Add(this.lblQuantity);
            this.pnlProductEntry.Controls.Add(this.txtQuantity);
            this.pnlProductEntry.Controls.Add(this.lblProduct);
            this.pnlProductEntry.Controls.Add(this.cmbProduct);
            this.pnlProductEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProductEntry.Location = new System.Drawing.Point(0, 190);
            this.pnlProductEntry.Name = "pnlProductEntry";
            this.pnlProductEntry.Size = new System.Drawing.Size(1200, 100);
            this.pnlProductEntry.TabIndex = 2;
            // 
            // lblProduct
            // 
            this.lblProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(1050, 20);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(86, 18);
            this.lblProduct.TabIndex = 0;
            this.lblProduct.Text = "اسم المنتج:";
            // 
            // cmbProduct
            // 
            this.cmbProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.Location = new System.Drawing.Point(730, 48);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(410, 27);
            this.cmbProduct.TabIndex = 1;
            this.cmbProduct.SelectedIndexChanged += new System.EventHandler(this.cmbProduct_SelectedIndexChanged);
            // 
            // lblAvailableQty
            // 
            this.lblAvailableQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAvailableQty.AutoSize = true;
            this.lblAvailableQty.Location = new System.Drawing.Point(590, 20);
            this.lblAvailableQty.Name = "lblAvailableQty";
            this.lblAvailableQty.Size = new System.Drawing.Size(107, 18);
            this.lblAvailableQty.TabIndex = 6;
            this.lblAvailableQty.Text = "المتاح بالمصدر:";
            // 
            // txtAvailableQty
            // 
            this.txtAvailableQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAvailableQty.Location = new System.Drawing.Point(590, 48);
            this.txtAvailableQty.Name = "txtAvailableQty";
            this.txtAvailableQty.ReadOnly = true;
            this.txtAvailableQty.Size = new System.Drawing.Size(120, 28);
            this.txtAvailableQty.TabIndex = 7;
            this.txtAvailableQty.Text = "0";
            this.txtAvailableQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblQuantity
            // 
            this.lblQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(440, 20);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(123, 18);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "الكمية المنصرفة:";
            // 
            // txtQuantity
            // 
            this.txtQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuantity.Location = new System.Drawing.Point(440, 48);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(130, 28);
            this.txtQuantity.TabIndex = 3;
            this.txtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQuantity_KeyPress);
            // 
            // btnAddProduct
            // 
            this.btnAddProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddProduct.Location = new System.Drawing.Point(230, 40);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(180, 45);
            this.btnAddProduct.TabIndex = 4;
            this.btnAddProduct.Text = "إضافة / تعديل ➕";
            this.btnAddProduct.UseVisualStyleBackColor = false;
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);
            // 
            // btnRemoveProduct
            // 
            this.btnRemoveProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveProduct.Location = new System.Drawing.Point(50, 40);
            this.btnRemoveProduct.Name = "btnRemoveProduct";
            this.btnRemoveProduct.Size = new System.Drawing.Size(160, 45);
            this.btnRemoveProduct.TabIndex = 5;
            this.btnRemoveProduct.Text = "حذف صنف 🗑";
            this.btnRemoveProduct.UseVisualStyleBackColor = false;
            this.btnRemoveProduct.Click += new System.EventHandler(this.btnRemoveProduct_Click);
            // 
            // dgvDetails
            // 
            this.dgvDetails.AllowUserToAddRows = false;
            this.dgvDetails.AllowUserToDeleteRows = false;
            this.dgvDetails.AllowUserToResizeRows = false;
            this.dgvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetails.Location = new System.Drawing.Point(0, 290);
            this.dgvDetails.MultiSelect = false;
            this.dgvDetails.Name = "dgvDetails";
            this.dgvDetails.ReadOnly = true;
            this.dgvDetails.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvDetails.RowHeadersVisible = false;
            this.dgvDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetails.Size = new System.Drawing.Size(1200, 310);
            this.dgvDetails.TabIndex = 3;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.lblIssueNotes);
            this.pnlFooter.Controls.Add(this.txtIssueNotes);
            this.pnlFooter.Controls.Add(this.btnSave);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 600);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1200, 120);
            this.pnlFooter.TabIndex = 4;
            // 
            // lblIssueNotes
            // 
            this.lblIssueNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIssueNotes.AutoSize = true;
            this.lblIssueNotes.Location = new System.Drawing.Point(950, 20);
            this.lblIssueNotes.Name = "lblIssueNotes";
            this.lblIssueNotes.Size = new System.Drawing.Size(155, 18);
            this.lblIssueNotes.TabIndex = 2;
            this.lblIssueNotes.Text = "ملاحظات للفرع (رد):";
            // 
            // txtIssueNotes
            // 
            this.txtIssueNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIssueNotes.Location = new System.Drawing.Point(400, 20);
            this.txtIssueNotes.Multiline = true;
            this.txtIssueNotes.Name = "txtIssueNotes";
            this.txtIssueNotes.Size = new System.Drawing.Size(530, 80);
            this.txtIssueNotes.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Location = new System.Drawing.Point(30, 30);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(320, 60);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "تنفيذ وصرف البضاعة ✔";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmIssueStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1200, 720);
            this.Controls.Add(this.dgvDetails);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlProductEntry);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(1000, 650);
            this.Name = "frmIssueStock";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "صرف البضاعة للفروع";
            this.Load += new System.EventHandler(this.frmIssueStock_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.pnlProductEntry.ResumeLayout(false);
            this.pnlProductEntry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.ResumeLayout(false);
        }

        private void ApplyCustomUIStyles()
        {
            this.Font = BranchThemeIssueStock.UiFont(12F);
            this.BackColor = BranchThemeIssueStock.Bg;

            pnlHeader.BackColor = BranchThemeIssueStock.Header;

            lblTitle.Font = BranchThemeIssueStock.UiFont(24F, FontStyle.Bold);
            lblTitle.ForeColor = BranchThemeIssueStock.Text;
            lblTitle.TextAlign = ContentAlignment.BottomCenter;

            lblSubtitle.Font = BranchThemeIssueStock.UiFont(12F);
            lblSubtitle.ForeColor = BranchThemeIssueStock.Muted;
            lblSubtitle.TextAlign = ContentAlignment.TopCenter;

            pnlInfo.BackColor = BranchThemeIssueStock.Surface;
            pnlProductEntry.BackColor = BranchThemeIssueStock.Bg;
            pnlFooter.BackColor = BranchThemeIssueStock.Surface;

            lblFromLocation.Font = BranchThemeIssueStock.UiFont(13.5F, FontStyle.Bold);
            lblFromLocation.ForeColor = BranchThemeIssueStock.Text;
            lblToLocation.Font = BranchThemeIssueStock.UiFont(13.5F, FontStyle.Bold);
            lblToLocation.ForeColor = BranchThemeIssueStock.Text;
            lblIssueDate.Font = BranchThemeIssueStock.UiFont(13.5F, FontStyle.Bold);
            lblIssueDate.ForeColor = BranchThemeIssueStock.Text;

            lblProduct.Font = BranchThemeIssueStock.UiFont(13.5F, FontStyle.Bold);
            lblProduct.ForeColor = BranchThemeIssueStock.Text;
            lblAvailableQty.Font = BranchThemeIssueStock.UiFont(13.5F, FontStyle.Bold);
            lblAvailableQty.ForeColor = Color.FromArgb(248, 113, 113);
            lblQuantity.Font = BranchThemeIssueStock.UiFont(13.5F, FontStyle.Bold);
            lblQuantity.ForeColor = BranchThemeIssueStock.Text;
            lblIssueNotes.Font = BranchThemeIssueStock.UiFont(13.5F, FontStyle.Bold);
            lblIssueNotes.ForeColor = BranchThemeIssueStock.Text;

            cmbFromLocation.BackColor = BranchThemeIssueStock.Input;
            cmbFromLocation.ForeColor = BranchThemeIssueStock.Text;
            cmbFromLocation.Font = BranchThemeIssueStock.UiFont(14F, FontStyle.Bold);
            cmbFromLocation.FlatStyle = FlatStyle.Flat;

            cmbToLocation.BackColor = BranchThemeIssueStock.Input;
            cmbToLocation.ForeColor = BranchThemeIssueStock.Text;
            cmbToLocation.Font = BranchThemeIssueStock.UiFont(14F, FontStyle.Bold);
            cmbToLocation.FlatStyle = FlatStyle.Flat;

            dtpIssueDate.Font = BranchThemeIssueStock.UiFont(14F, FontStyle.Bold);

            cmbProduct.BackColor = BranchThemeIssueStock.Input;
            cmbProduct.ForeColor = BranchThemeIssueStock.Text;
            cmbProduct.Font = BranchThemeIssueStock.UiFont(14F, FontStyle.Bold);
            cmbProduct.FlatStyle = FlatStyle.Flat;

            txtAvailableQty.BackColor = Color.FromArgb(45, 30, 35);
            txtAvailableQty.ForeColor = Color.FromArgb(248, 113, 113);
            txtAvailableQty.Font = BranchThemeIssueStock.UiFont(14F, FontStyle.Bold);
            txtAvailableQty.BorderStyle = BorderStyle.FixedSingle;

            txtQuantity.BackColor = BranchThemeIssueStock.Input;
            txtQuantity.ForeColor = BranchThemeIssueStock.Text;
            txtQuantity.Font = BranchThemeIssueStock.UiFont(14F, FontStyle.Bold);
            txtQuantity.BorderStyle = BorderStyle.FixedSingle;

            txtIssueNotes.BackColor = BranchThemeIssueStock.Input;
            txtIssueNotes.ForeColor = BranchThemeIssueStock.Text;
            txtIssueNotes.Font = BranchThemeIssueStock.UiFont(14F, FontStyle.Regular);
            txtIssueNotes.BorderStyle = BorderStyle.FixedSingle;

            btnAddProduct.BackColor = BranchThemeIssueStock.Green;
            btnAddProduct.ForeColor = Color.White;
            btnAddProduct.Font = BranchThemeIssueStock.UiFont(13F, FontStyle.Bold);
            btnAddProduct.FlatAppearance.MouseOverBackColor = ControlPaint.Light(BranchThemeIssueStock.Green, 0.15F);

            btnRemoveProduct.BackColor = BranchThemeIssueStock.Red;
            btnRemoveProduct.ForeColor = Color.White;
            btnRemoveProduct.Font = BranchThemeIssueStock.UiFont(13F, FontStyle.Bold);
            btnRemoveProduct.FlatAppearance.MouseOverBackColor = ControlPaint.Light(BranchThemeIssueStock.Red, 0.15F);

            btnSave.BackColor = BranchThemeIssueStock.Accent;
            btnSave.ForeColor = Color.White;
            btnSave.Font = BranchThemeIssueStock.UiFont(16F, FontStyle.Bold);
            btnSave.FlatAppearance.MouseOverBackColor = ControlPaint.Light(BranchThemeIssueStock.Accent, 0.15F);

            // Grid Styles
            dgvDetails.BackgroundColor = BranchThemeIssueStock.Bg;
            dgvDetails.GridColor = BranchThemeIssueStock.Border;

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            headerStyle.BackColor = BranchThemeIssueStock.Header;
            headerStyle.ForeColor = BranchThemeIssueStock.Muted;
            headerStyle.SelectionBackColor = BranchThemeIssueStock.Header;
            headerStyle.SelectionForeColor = BranchThemeIssueStock.Muted;
            headerStyle.Font = BranchThemeIssueStock.UiFont(14F, FontStyle.Bold);
            dgvDetails.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvDetails.ColumnHeadersHeight = 55;

            DataGridViewCellStyle defaultStyle = new DataGridViewCellStyle();
            defaultStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            defaultStyle.BackColor = BranchThemeIssueStock.Surface;
            defaultStyle.ForeColor = BranchThemeIssueStock.Text;
            defaultStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            defaultStyle.SelectionForeColor = Color.White;
            defaultStyle.Font = BranchThemeIssueStock.UiFont(13F, FontStyle.Bold);
            dgvDetails.DefaultCellStyle = defaultStyle;

            DataGridViewCellStyle altStyle = new DataGridViewCellStyle();
            altStyle.BackColor = BranchThemeIssueStock.Field;
            dgvDetails.AlternatingRowsDefaultCellStyle = altStyle;

            dgvDetails.RowTemplate.Height = 55;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvDetails.Columns.AddRange(new DataGridViewColumn[] { colSerial, colProductId, colProductName, colQuantity });

            colSerial.Name = "colSerial";
            colSerial.HeaderText = "م";
            colSerial.FillWeight = 30F;
            colSerial.ReadOnly = true;
            colSerial.SortMode = DataGridViewColumnSortMode.NotSortable;

            colProductId.Name = "colProductId";
            colProductId.HeaderText = "رقم المنتج";
            colProductId.Visible = false;

            colProductName.Name = "colProductName";
            colProductName.HeaderText = "اسم المنتج";
            colProductName.FillWeight = 220F;
            colProductName.ReadOnly = true;
            colProductName.SortMode = DataGridViewColumnSortMode.NotSortable;

            colQuantity.Name = "colQuantity";
            colQuantity.HeaderText = "الكمية المنصرفة (اضغط للتعديل)";
            colQuantity.FillWeight = 150F;
            colQuantity.ReadOnly = false;
            colQuantity.SortMode = DataGridViewColumnSortMode.NotSortable;

            // تمييز عمود الكمية عشان اليوزر يعرف إنه يقدر يعدله
            colQuantity.DefaultCellStyle.BackColor = Color.FromArgb(50, 75, 105);
            colQuantity.DefaultCellStyle.ForeColor = Color.White;
            colQuantity.DefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 120, 190);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblFromLocation;
        private System.Windows.Forms.ComboBox cmbFromLocation;
        private System.Windows.Forms.Label lblToLocation;
        private System.Windows.Forms.ComboBox cmbToLocation;
        private System.Windows.Forms.Label lblIssueDate;
        private System.Windows.Forms.DateTimePicker dtpIssueDate;
        private System.Windows.Forms.Panel pnlProductEntry;
        private System.Windows.Forms.Label lblAvailableQty;
        private System.Windows.Forms.TextBox txtAvailableQty;
        private System.Windows.Forms.Button btnRemoveProduct;
        private System.Windows.Forms.Button btnAddProduct;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSerial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblIssueNotes;
        private System.Windows.Forms.TextBox txtIssueNotes;

        #endregion
    }
}