using System.Drawing;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    // ألوان الثيم الموحدة
    internal static class BranchThemeRequests
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

    partial class frmBranchRequests
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
            // بناء الـ Controls فقط بدون تنسيق لحماية الـ Designer
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.pnlHeaderLine = new System.Windows.Forms.Panel();
            this.pnlRequestInfo = new System.Windows.Forms.Panel();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.lblRequestDate = new System.Windows.Forms.Label();
            this.dtpRequestDate = new System.Windows.Forms.DateTimePicker();
            this.pnlProductEntry = new System.Windows.Forms.Panel();
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
            this.btnSave = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlRequestInfo.SuspendLayout();
            this.pnlProductEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.pnlHeaderLine);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1000, 90);
            this.pnlHeader.TabIndex = 0;
            // 
            // pnlHeaderLine
            // 
            this.pnlHeaderLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlHeaderLine.Height = 4;
            this.pnlHeaderLine.Name = "pnlHeaderLine";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = false;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Height = 45;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "طلبات الفروع 📝";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = false;
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubtitle.Height = 35;
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "إنشاء طلب بضاعة جديد من الفرع للمخزن الرئيسي";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pnlRequestInfo
            // 
            this.pnlRequestInfo.Controls.Add(this.lblLocation);
            this.pnlRequestInfo.Controls.Add(this.cmbLocation);
            this.pnlRequestInfo.Controls.Add(this.lblRequestDate);
            this.pnlRequestInfo.Controls.Add(this.dtpRequestDate);
            this.pnlRequestInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRequestInfo.Location = new System.Drawing.Point(0, 90);
            this.pnlRequestInfo.Name = "pnlRequestInfo";
            this.pnlRequestInfo.Size = new System.Drawing.Size(1000, 100);
            this.pnlRequestInfo.TabIndex = 1;
            // 
            // lblLocation
            // 
            this.lblLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(880, 20);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(95, 18);
            this.lblLocation.TabIndex = 0;
            this.lblLocation.Text = "الفرع الطالب:";
            // 
            // cmbLocation
            // 
            this.cmbLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(600, 48);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(365, 27);
            this.cmbLocation.TabIndex = 1;
            // 
            // lblRequestDate
            // 
            this.lblRequestDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRequestDate.AutoSize = true;
            this.lblRequestDate.Location = new System.Drawing.Point(440, 20);
            this.lblRequestDate.Name = "lblRequestDate";
            this.lblRequestDate.Size = new System.Drawing.Size(87, 18);
            this.lblRequestDate.TabIndex = 2;
            this.lblRequestDate.Text = "تاريخ الطلب:";
            // 
            // dtpRequestDate
            // 
            this.dtpRequestDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpRequestDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpRequestDate.Location = new System.Drawing.Point(310, 48);
            this.dtpRequestDate.Name = "dtpRequestDate";
            this.dtpRequestDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dtpRequestDate.RightToLeftLayout = true;
            this.dtpRequestDate.Size = new System.Drawing.Size(220, 27);
            this.dtpRequestDate.TabIndex = 3;
            // 
            // pnlProductEntry
            // 
            this.pnlProductEntry.Controls.Add(this.btnRemoveProduct);
            this.pnlProductEntry.Controls.Add(this.btnAddProduct);
            this.pnlProductEntry.Controls.Add(this.lblQuantity);
            this.pnlProductEntry.Controls.Add(this.txtQuantity);
            this.pnlProductEntry.Controls.Add(this.lblProduct);
            this.pnlProductEntry.Controls.Add(this.cmbProduct);
            this.pnlProductEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProductEntry.Location = new System.Drawing.Point(0, 190);
            this.pnlProductEntry.Name = "pnlProductEntry";
            this.pnlProductEntry.Size = new System.Drawing.Size(1000, 100);
            this.pnlProductEntry.TabIndex = 2;
            // 
            // lblProduct
            // 
            this.lblProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(890, 20);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(86, 18);
            this.lblProduct.TabIndex = 0;
            this.lblProduct.Text = "اسم المنتج:";
            // 
            // cmbProduct
            // 
            this.cmbProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.Location = new System.Drawing.Point(540, 45);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(425, 27);
            this.cmbProduct.TabIndex = 1;
            // 
            // lblQuantity
            // 
            this.lblQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(380, 20);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(126, 18);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "الكمية المطلوبة:";
            // 
            // txtQuantity
            // 
            this.txtQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQuantity.Location = new System.Drawing.Point(360, 45);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(150, 28);
            this.txtQuantity.TabIndex = 3;
            this.txtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQuantity_KeyPress);
            // 
            // btnAddProduct
            // 
            this.btnAddProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddProduct.FlatAppearance.BorderSize = 0;
            this.btnAddProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddProduct.Location = new System.Drawing.Point(180, 39);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(150, 42);
            this.btnAddProduct.TabIndex = 4;
            this.btnAddProduct.Text = "إضافة للطلب ➕";
            this.btnAddProduct.UseVisualStyleBackColor = false;
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);
            // 
            // btnRemoveProduct
            // 
            this.btnRemoveProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveProduct.FlatAppearance.BorderSize = 0;
            this.btnRemoveProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveProduct.Location = new System.Drawing.Point(20, 39);
            this.btnRemoveProduct.Name = "btnRemoveProduct";
            this.btnRemoveProduct.Size = new System.Drawing.Size(140, 42);
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
            this.dgvDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetails.EnableHeadersVisualStyles = false;
            this.dgvDetails.Location = new System.Drawing.Point(0, 290);
            this.dgvDetails.MultiSelect = false;
            this.dgvDetails.Name = "dgvDetails";
            this.dgvDetails.ReadOnly = true;
            this.dgvDetails.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvDetails.RowHeadersVisible = false;
            this.dgvDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetails.Size = new System.Drawing.Size(1000, 310);
            this.dgvDetails.TabIndex = 3;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.btnSave);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 600);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1000, 100);
            this.pnlFooter.TabIndex = 4;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(30, 20);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(320, 60);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "تأكيد وإرسال الطلب للمخزن ✔";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmBranchRequests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.dgvDetails);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlProductEntry);
            this.Controls.Add(this.pnlRequestInfo);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "frmBranchRequests";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "طلبات الفروع";
            this.Load += new System.EventHandler(this.frmBranchRequests_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlRequestInfo.ResumeLayout(false);
            this.pnlRequestInfo.PerformLayout();
            this.pnlProductEntry.ResumeLayout(false);
            this.pnlProductEntry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void ApplyCustomUIStyles()
        {
            this.Font = BranchThemeRequests.UiFont(12F);
            this.BackColor = BranchThemeRequests.Bg;

            pnlHeader.BackColor = BranchThemeRequests.Header;
            pnlHeaderLine.BackColor = BranchThemeRequests.Accent;

            lblTitle.Font = BranchThemeRequests.UiFont(22F, FontStyle.Bold);
            lblTitle.ForeColor = BranchThemeRequests.Text;
            lblTitle.TextAlign = ContentAlignment.BottomCenter;

            lblSubtitle.Font = BranchThemeRequests.UiFont(11.5F);
            lblSubtitle.ForeColor = BranchThemeRequests.Muted;
            lblSubtitle.TextAlign = ContentAlignment.TopCenter;

            pnlRequestInfo.BackColor = BranchThemeRequests.Surface;
            pnlProductEntry.BackColor = BranchThemeRequests.Bg;
            pnlFooter.BackColor = BranchThemeRequests.Surface;

            lblLocation.Font = BranchThemeRequests.UiFont(13.5F, FontStyle.Bold);
            lblLocation.ForeColor = BranchThemeRequests.Text;
            lblRequestDate.Font = BranchThemeRequests.UiFont(13.5F, FontStyle.Bold);
            lblRequestDate.ForeColor = BranchThemeRequests.Text;
            lblProduct.Font = BranchThemeRequests.UiFont(13.5F, FontStyle.Bold);
            lblProduct.ForeColor = BranchThemeRequests.Text;
            lblQuantity.Font = BranchThemeRequests.UiFont(13.5F, FontStyle.Bold);
            lblQuantity.ForeColor = BranchThemeRequests.Text;

            cmbLocation.BackColor = BranchThemeRequests.Input;
            cmbLocation.ForeColor = BranchThemeRequests.Text;
            cmbLocation.Font = BranchThemeRequests.UiFont(14F, FontStyle.Bold);
            cmbLocation.FlatStyle = FlatStyle.Flat;

            dtpRequestDate.Font = BranchThemeRequests.UiFont(14F, FontStyle.Bold);

            cmbProduct.BackColor = BranchThemeRequests.Input;
            cmbProduct.ForeColor = BranchThemeRequests.Text;
            cmbProduct.Font = BranchThemeRequests.UiFont(14F, FontStyle.Bold);
            cmbProduct.FlatStyle = FlatStyle.Flat;

            txtQuantity.BackColor = BranchThemeRequests.Input;
            txtQuantity.ForeColor = BranchThemeRequests.Text;
            txtQuantity.Font = BranchThemeRequests.UiFont(14F, FontStyle.Bold);
            txtQuantity.BorderStyle = BorderStyle.FixedSingle;

            btnAddProduct.BackColor = BranchThemeRequests.Green;
            btnAddProduct.ForeColor = Color.White;
            btnAddProduct.Font = BranchThemeRequests.UiFont(13F, FontStyle.Bold);
            btnAddProduct.FlatAppearance.MouseOverBackColor = ControlPaint.Light(BranchThemeRequests.Green, 0.15F);

            btnRemoveProduct.BackColor = BranchThemeRequests.Red;
            btnRemoveProduct.ForeColor = Color.White;
            btnRemoveProduct.Font = BranchThemeRequests.UiFont(13F, FontStyle.Bold);
            btnRemoveProduct.FlatAppearance.MouseOverBackColor = ControlPaint.Light(BranchThemeRequests.Red, 0.15F);

            btnSave.BackColor = BranchThemeRequests.Accent;
            btnSave.ForeColor = Color.White;
            btnSave.Font = BranchThemeRequests.UiFont(16F, FontStyle.Bold);
            btnSave.FlatAppearance.MouseOverBackColor = ControlPaint.Light(BranchThemeRequests.Accent, 0.15F);

            // Grid Styles
            dgvDetails.BackgroundColor = BranchThemeRequests.Bg;
            dgvDetails.GridColor = BranchThemeRequests.Border;

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            headerStyle.BackColor = BranchThemeRequests.Header;
            headerStyle.ForeColor = BranchThemeRequests.Muted;
            headerStyle.SelectionBackColor = BranchThemeRequests.Header;
            headerStyle.SelectionForeColor = BranchThemeRequests.Muted;
            headerStyle.Font = BranchThemeRequests.UiFont(13F, FontStyle.Bold);
            dgvDetails.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvDetails.ColumnHeadersHeight = 50;

            DataGridViewCellStyle defaultStyle = new DataGridViewCellStyle();
            defaultStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            defaultStyle.BackColor = BranchThemeRequests.Surface;
            defaultStyle.ForeColor = BranchThemeRequests.Text;
            defaultStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            defaultStyle.SelectionForeColor = Color.White;
            defaultStyle.Font = BranchThemeRequests.UiFont(12.5F, FontStyle.Bold);
            dgvDetails.DefaultCellStyle = defaultStyle;

            DataGridViewCellStyle altStyle = new DataGridViewCellStyle();
            altStyle.BackColor = BranchThemeRequests.Field;
            dgvDetails.AlternatingRowsDefaultCellStyle = altStyle;

            dgvDetails.RowTemplate.Height = 48;
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
            colQuantity.HeaderText = "الكمية المطلوبة";
            colQuantity.FillWeight = 120F;
            colQuantity.ReadOnly = true;
            colQuantity.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlHeaderLine;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel pnlRequestInfo;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Label lblRequestDate;
        private System.Windows.Forms.DateTimePicker dtpRequestDate;
        private System.Windows.Forms.Panel pnlProductEntry;
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

        #endregion
    }
}