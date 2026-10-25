namespace MajesticDesktop.Forms
{
    partial class frmProducts
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
            this.pnlProductData = new System.Windows.Forms.Panel();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.picProductImage = new System.Windows.Forms.PictureBox();
            this.btnChooseImage = new System.Windows.Forms.Button();
            this.tlpFields = new System.Windows.Forms.TableLayoutPanel();
            this.lblProductName = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblPurchasePrice = new System.Windows.Forms.Label();
            this.lblQty = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.txtPurchasePrice = new System.Windows.Forms.TextBox();
            this.txtInitialQty = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.lblAlertLimit = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.txtAlertLimit = new System.Windows.Forms.TextBox();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.tlpButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.tlpSearch = new System.Windows.Forms.TableLayoutPanel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblCategoryFilter = new System.Windows.Forms.Label();
            this.cmbCategoryFilter = new System.Windows.Forms.ComboBox();
            this.chkLowStock = new System.Windows.Forms.CheckBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblProductsTitle = new System.Windows.Forms.Label();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.colRowNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchasePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlertLimit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.pnlHeader.SuspendLayout();
            this.pnlProductData.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.pnlImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picProductImage)).BeginInit();
            this.tlpFields.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.tlpSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.SuspendLayout();

            // ==========================================
            // pnlHeader (تم إصلاح ترتيب ومسافات العناوين)
            // ==========================================
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(36)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1200, 85);
            this.pnlHeader.TabIndex = 0;

            // lblTitle
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1200, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "📦 إدارة الأصناف والمخزون";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblSubtitle
            this.lblSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(190)))), ((int)(((byte)(210)))));
            this.lblSubtitle.Location = new System.Drawing.Point(0, 50);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(1200, 25);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "إضافة وتعديل بيانات المنتجات وسعر التكلفة والرصيد الافتتاحي";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            // ==========================================
            // pnlProductData (الخانات والأزرار)
            // ==========================================
            this.pnlProductData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(45)))), ((int)(((byte)(60)))));
            this.pnlProductData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProductData.Controls.Add(this.tlpMain);
            this.pnlProductData.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProductData.Location = new System.Drawing.Point(0, 85);
            this.pnlProductData.Name = "pnlProductData";
            this.pnlProductData.Size = new System.Drawing.Size(1200, 275); // كبرنا الارتفاع شوية للمسافات
            this.pnlProductData.TabIndex = 1;

            // tlpMain
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpMain.Controls.Add(this.tlpFields, 0, 0);
            this.tlpMain.Controls.Add(this.pnlImage, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.TabIndex = 0;

            // pnlImage
            this.pnlImage.Controls.Add(this.picProductImage);
            this.pnlImage.Controls.Add(this.btnChooseImage);
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImage.Location = new System.Drawing.Point(3, 3);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Padding = new System.Windows.Forms.Padding(15);
            this.pnlImage.Size = new System.Drawing.Size(194, 267);
            this.pnlImage.TabIndex = 1;

            // picProductImage
            this.picProductImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picProductImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.picProductImage.Location = new System.Drawing.Point(15, 15);
            this.picProductImage.Name = "picProductImage";
            this.picProductImage.Size = new System.Drawing.Size(164, 160);
            this.picProductImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picProductImage.TabIndex = 0;
            this.picProductImage.TabStop = false;

            // btnChooseImage
            this.btnChooseImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnChooseImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChooseImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnChooseImage.FlatAppearance.BorderSize = 0;
            this.btnChooseImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseImage.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnChooseImage.ForeColor = System.Drawing.Color.White;
            this.btnChooseImage.Location = new System.Drawing.Point(15, 175);
            this.btnChooseImage.Name = "btnChooseImage";
            this.btnChooseImage.Size = new System.Drawing.Size(164, 40);
            this.btnChooseImage.TabIndex = 1;
            this.btnChooseImage.Text = "صورة المنتج";
            this.btnChooseImage.UseVisualStyleBackColor = false;
            this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click);

            // tlpFields (تم ضبط مسافات الصفوف لراحة العين)
            this.tlpFields.ColumnCount = 4;
            this.tlpFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpFields.Controls.Add(this.lblProductName, 0, 0);
            this.tlpFields.Controls.Add(this.lblCategory, 1, 0);
            this.tlpFields.Controls.Add(this.lblPurchasePrice, 2, 0);
            this.tlpFields.Controls.Add(this.lblQty, 3, 0);
            this.tlpFields.Controls.Add(this.txtProductName, 0, 1);
            this.tlpFields.Controls.Add(this.cmbCategory, 1, 1);
            this.tlpFields.Controls.Add(this.txtPurchasePrice, 2, 1);
            this.tlpFields.Controls.Add(this.txtInitialQty, 3, 1);
            this.tlpFields.Controls.Add(this.lblNotes, 0, 2);
            this.tlpFields.Controls.Add(this.lblAlertLimit, 2, 2);
            this.tlpFields.Controls.Add(this.txtNotes, 0, 3);
            this.tlpFields.SetColumnSpan(this.txtNotes, 2);
            this.tlpFields.Controls.Add(this.txtAlertLimit, 2, 3);
            this.tlpFields.Controls.Add(this.chkIsActive, 3, 3);
            this.tlpFields.Controls.Add(this.tlpButtons, 0, 4);
            this.tlpFields.SetColumnSpan(this.tlpButtons, 4);
            this.tlpFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFields.Location = new System.Drawing.Point(203, 3);
            this.tlpFields.Name = "tlpFields";
            this.tlpFields.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.tlpFields.RowCount = 5;
            this.tlpFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpFields.TabIndex = 0;

            // Labels
            this.lblProductName.AutoSize = true;
            this.lblProductName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblProductName.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblProductName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblProductName.Location = new System.Drawing.Point(744, 26);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(228, 19);
            this.lblProductName.TabIndex = 0;
            this.lblProductName.Text = "اسم المنتج *";

            this.lblCategory.AutoSize = true;
            this.lblCategory.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblCategory.Location = new System.Drawing.Point(506, 26);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(232, 19);
            this.lblCategory.TabIndex = 1;
            this.lblCategory.Text = "القسم *";

            this.lblPurchasePrice.AutoSize = true;
            this.lblPurchasePrice.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblPurchasePrice.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblPurchasePrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblPurchasePrice.Location = new System.Drawing.Point(268, 26);
            this.lblPurchasePrice.Name = "lblPurchasePrice";
            this.lblPurchasePrice.Size = new System.Drawing.Size(232, 19);
            this.lblPurchasePrice.TabIndex = 2;
            this.lblPurchasePrice.Text = "سعر التكلفة (شراء)";

            this.lblQty.AutoSize = true;
            this.lblQty.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblQty.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblQty.Location = new System.Drawing.Point(23, 26);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(239, 19);
            this.lblQty.TabIndex = 3;
            this.lblQty.Text = "رصيد المخزن المبدئي";

            // Inputs
            this.txtProductName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProductName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtProductName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProductName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtProductName.ForeColor = System.Drawing.Color.White;
            this.txtProductName.Location = new System.Drawing.Point(754, 55);
            this.txtProductName.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(218, 29);
            this.txtProductName.TabIndex = 4;

            this.cmbCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCategory.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.cmbCategory.ForeColor = System.Drawing.Color.White;
            this.cmbCategory.Location = new System.Drawing.Point(516, 55);
            this.cmbCategory.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(222, 29);
            this.cmbCategory.TabIndex = 5;

            this.txtPurchasePrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPurchasePrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtPurchasePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPurchasePrice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtPurchasePrice.ForeColor = System.Drawing.Color.White;
            this.txtPurchasePrice.Location = new System.Drawing.Point(278, 55);
            this.txtPurchasePrice.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtPurchasePrice.Name = "txtPurchasePrice";
            this.txtPurchasePrice.Size = new System.Drawing.Size(222, 29);
            this.txtPurchasePrice.TabIndex = 6;
            this.txtPurchasePrice.Text = "0";
            this.txtPurchasePrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPurchasePrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyNumbers_KeyPress);

            this.txtInitialQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInitialQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtInitialQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInitialQty.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtInitialQty.ForeColor = System.Drawing.Color.White;
            this.txtInitialQty.Location = new System.Drawing.Point(30, 55);
            this.txtInitialQty.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtInitialQty.Name = "txtInitialQty";
            this.txtInitialQty.Size = new System.Drawing.Size(228, 29);
            this.txtInitialQty.TabIndex = 7;
            this.txtInitialQty.Text = "0";
            this.txtInitialQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtInitialQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyNumbers_KeyPress);

            // Row 3 Labels
            this.lblNotes.AutoSize = true;
            this.lblNotes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblNotes.Location = new System.Drawing.Point(744, 106);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(228, 19);
            this.lblNotes.TabIndex = 8;
            this.lblNotes.Text = "ملاحظات تفصيلية";

            this.lblAlertLimit.AutoSize = true;
            this.lblAlertLimit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblAlertLimit.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblAlertLimit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblAlertLimit.Location = new System.Drawing.Point(268, 106);
            this.lblAlertLimit.Name = "lblAlertLimit";
            this.lblAlertLimit.Size = new System.Drawing.Size(232, 19);
            this.lblAlertLimit.TabIndex = 9;
            this.lblAlertLimit.Text = "حد النواقص";

            // Row 4 Inputs
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlpFields.SetColumnSpan(this.txtNotes, 2);
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtNotes.ForeColor = System.Drawing.Color.White;
            this.txtNotes.Location = new System.Drawing.Point(516, 135);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(456, 29);
            this.txtNotes.TabIndex = 10;

            this.txtAlertLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAlertLimit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtAlertLimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAlertLimit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtAlertLimit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            this.txtAlertLimit.Location = new System.Drawing.Point(278, 135);
            this.txtAlertLimit.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtAlertLimit.Name = "txtAlertLimit";
            this.txtAlertLimit.Size = new System.Drawing.Size(222, 29);
            this.txtAlertLimit.TabIndex = 11;
            this.txtAlertLimit.Text = "5";
            this.txtAlertLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAlertLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAlertLimit_KeyPress);

            this.chkIsActive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Checked = true;
            this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsActive.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.chkIsActive.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.chkIsActive.Location = new System.Drawing.Point(30, 137);
            this.chkIsActive.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(228, 25);
            this.chkIsActive.TabIndex = 12;
            this.chkIsActive.Text = "منتج نشط ومتاح للبيع";
            this.chkIsActive.UseVisualStyleBackColor = true;

            // tlpButtons
            this.tlpButtons.ColumnCount = 4;
            this.tlpFields.SetColumnSpan(this.tlpButtons, 4);
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpButtons.Controls.Add(this.btnSave, 0, 0);
            this.tlpButtons.Controls.Add(this.btnEdit, 1, 0);
            this.tlpButtons.Controls.Add(this.btnClear, 2, 0);
            this.tlpButtons.Controls.Add(this.btnDelete, 3, 0);
            this.tlpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButtons.Location = new System.Drawing.Point(23, 185);
            this.tlpButtons.Name = "tlpButtons";
            this.tlpButtons.RowCount = 1;
            this.tlpButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpButtons.Size = new System.Drawing.Size(946, 60);
            this.tlpButtons.TabIndex = 13;

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(715, 10);
            this.btnSave.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(226, 45);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "➕ حفظ جديد";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnEdit
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(479, 10);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(226, 45);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "✏️ تعديل المختار";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            // btnClear
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(243, 10);
            this.btnClear.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(226, 45);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "🔄 تفريغ الخانات";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);

            // btnDelete
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(5, 10);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(228, 45);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "🗑️ حذف";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // ==========================================
            // pnlFilter (شريط البحث والفلترة)
            // ==========================================
            this.pnlFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilter.Controls.Add(this.tlpSearch);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 360);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1200, 75); // كبرنا المساحة عشان تناسب البحث
            this.pnlFilter.TabIndex = 2;

            // tlpSearch
            this.tlpSearch.ColumnCount = 6;
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpSearch.Controls.Add(this.lblSearch, 0, 0);
            this.tlpSearch.Controls.Add(this.txtSearch, 1, 0);
            this.tlpSearch.Controls.Add(this.lblCategoryFilter, 2, 0);
            this.tlpSearch.Controls.Add(this.cmbCategoryFilter, 3, 0);
            this.tlpSearch.Controls.Add(this.chkLowStock, 4, 0);
            this.tlpSearch.Controls.Add(this.btnRefresh, 5, 0);
            this.tlpSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSearch.Location = new System.Drawing.Point(0, 0);
            this.tlpSearch.Name = "tlpSearch";
            this.tlpSearch.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.tlpSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpSearch.RowCount = 1;
            this.tlpSearch.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearch.Size = new System.Drawing.Size(1198, 73);
            this.tlpSearch.TabIndex = 0;

            // lblSearch
            this.lblSearch.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSearch.Location = new System.Drawing.Point(1091, 26);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(84, 21);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "بحث ذكي :";

            // txtSearch
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtSearch.ForeColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(744, 22);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(334, 29);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            // lblCategoryFilter
            this.lblCategoryFilter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblCategoryFilter.AutoSize = true;
            this.lblCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblCategoryFilter.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblCategoryFilter.Location = new System.Drawing.Point(636, 26);
            this.lblCategoryFilter.Name = "lblCategoryFilter";
            this.lblCategoryFilter.Size = new System.Drawing.Size(95, 21);
            this.lblCategoryFilter.TabIndex = 2;
            this.lblCategoryFilter.Text = "فلترة بالقسم:";

            // cmbCategoryFilter
            this.cmbCategoryFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCategoryFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cmbCategoryFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoryFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCategoryFilter.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.cmbCategoryFilter.ForeColor = System.Drawing.Color.White;
            this.cmbCategoryFilter.Location = new System.Drawing.Point(282, 22);
            this.cmbCategoryFilter.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.cmbCategoryFilter.Name = "cmbCategoryFilter";
            this.cmbCategoryFilter.Size = new System.Drawing.Size(334, 29);
            this.cmbCategoryFilter.TabIndex = 3;
            this.cmbCategoryFilter.SelectedIndexChanged += new System.EventHandler(this.cmbCategoryFilter_SelectedIndexChanged);

            // chkLowStock
            this.chkLowStock.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkLowStock.AutoSize = true;
            this.chkLowStock.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.chkLowStock.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            this.chkLowStock.Location = new System.Drawing.Point(180, 24);
            this.chkLowStock.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.chkLowStock.Name = "chkLowStock";
            this.chkLowStock.Size = new System.Drawing.Size(82, 25);
            this.chkLowStock.TabIndex = 4;
            this.chkLowStock.Text = "النواقص";
            this.chkLowStock.UseVisualStyleBackColor = true;
            this.chkLowStock.CheckedChanged += new System.EventHandler(this.chkLowStock_CheckedChanged);

            // btnRefresh
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(23, 18);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(144, 37);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "🔄 إعادة تحميل";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // lblProductsTitle
            this.lblProductsTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblProductsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblProductsTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblProductsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lblProductsTitle.Location = new System.Drawing.Point(0, 435);
            this.lblProductsTitle.Name = "lblProductsTitle";
            this.lblProductsTitle.Size = new System.Drawing.Size(1200, 45);
            this.lblProductsTitle.TabIndex = 3;
            this.lblProductsTitle.Text = "قائمة المنتجات الحالية في المخزن";
            this.lblProductsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // dgvProducts
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.AllowUserToResizeRows = false;
            this.dgvProducts.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.dgvProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProducts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvProducts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRowNumber,
            this.colId,
            this.colProductName,
            this.colCategory,
            this.colQuantity,
            this.colPurchasePrice,
            this.colNotes,
            this.colAlertLimit,
            this.colStatus});
            this.dgvProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProducts.EnableHeadersVisualStyles = false;
            this.dgvProducts.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.dgvProducts.Location = new System.Drawing.Point(0, 480);
            this.dgvProducts.MultiSelect = false;
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvProducts.RowHeadersVisible = false;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new System.Drawing.Size(1200, 320);
            this.dgvProducts.TabIndex = 4;
            this.dgvProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellClick);

            // الأعمدة
            this.colRowNumber.HeaderText = "م";
            this.colRowNumber.Name = "colRowNumber";
            this.colRowNumber.ReadOnly = true;
            this.colRowNumber.Width = 60;

            this.colId.HeaderText = "ID";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Visible = false;

            this.colProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colProductName.HeaderText = "اسم المنتج";
            this.colProductName.Name = "colProductName";
            this.colProductName.ReadOnly = true;

            this.colCategory.HeaderText = "القسم";
            this.colCategory.Name = "colCategory";
            this.colCategory.ReadOnly = true;
            this.colCategory.Width = 150;

            this.colQuantity.HeaderText = "المتاح بالمخزن";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.ReadOnly = true;
            this.colQuantity.Width = 130;

            this.colPurchasePrice.HeaderText = "سعر التكلفة";
            this.colPurchasePrice.Name = "colPurchasePrice";
            this.colPurchasePrice.ReadOnly = true;
            this.colPurchasePrice.Width = 120;

            this.colNotes.HeaderText = "ملاحظات";
            this.colNotes.Name = "colNotes";
            this.colNotes.ReadOnly = true;
            this.colNotes.Width = 180;

            this.colAlertLimit.HeaderText = "حد التنبيه";
            this.colAlertLimit.Name = "colAlertLimit";
            this.colAlertLimit.ReadOnly = true;
            this.colAlertLimit.Width = 90;

            this.colStatus.HeaderText = "الحالة";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 110;

            // frmProducts
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.dgvProducts);
            this.Controls.Add(this.lblProductsTitle);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlProductData);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1150, 750);
            this.Name = "frmProducts";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "إدارة الأصناف والمخزون";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmProducts_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlProductData.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            this.pnlImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picProductImage)).EndInit();
            this.tlpFields.ResumeLayout(false);
            this.tlpFields.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.tlpSearch.ResumeLayout(false);
            this.tlpSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel pnlProductData;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnlImage;
        private System.Windows.Forms.PictureBox picProductImage;
        private System.Windows.Forms.Button btnChooseImage;
        private System.Windows.Forms.TableLayoutPanel tlpFields;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblPurchasePrice;
        private System.Windows.Forms.TextBox txtPurchasePrice;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.TextBox txtInitialQty;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label lblAlertLimit;
        private System.Windows.Forms.TextBox txtAlertLimit;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.TableLayoutPanel tlpButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.TableLayoutPanel tlpSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblCategoryFilter;
        private System.Windows.Forms.ComboBox cmbCategoryFilter;
        private System.Windows.Forms.CheckBox chkLowStock;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblProductsTitle;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRowNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPurchasePrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlertLimit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    }
}