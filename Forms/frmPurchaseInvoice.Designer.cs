namespace MajesticDesktop.Forms
{
    partial class frmPurchaseInvoice
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
            this.pnlInvoiceData = new System.Windows.Forms.Panel();
            this.tlpInvoiceData = new System.Windows.Forms.TableLayoutPanel();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.lblInvoiceNo = new System.Windows.Forms.Label();
            this.txtInvoiceNo = new System.Windows.Forms.TextBox();
            this.lblInvoiceDate = new System.Windows.Forms.Label();
            this.dtpInvoiceDate = new System.Windows.Forms.DateTimePicker();
            this.pnlProductEntry = new System.Windows.Forms.Panel();
            this.tlpProductEntry = new System.Windows.Forms.TableLayoutPanel();
            this.lblProduct = new System.Windows.Forms.Label();
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.btnAddNewProduct = new System.Windows.Forms.Button();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.lblUnitPrice = new System.Windows.Forms.Label();
            this.txtUnitPrice = new System.Windows.Forms.TextBox();
            this.btnAddProduct = new System.Windows.Forms.Button();
            this.btnRemoveProduct = new System.Windows.Forms.Button();
            this.pnlTotals = new System.Windows.Forms.Panel();
            this.tlpTotals = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.lblPaidAmount = new System.Windows.Forms.Label();
            this.txtPaidAmount = new System.Windows.Forms.TextBox();
            this.lblRemainingAmount = new System.Windows.Forms.Label();
            this.txtRemainingAmount = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvDetails = new System.Windows.Forms.DataGridView();
            this.colRowNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.pnlHeader.SuspendLayout();
            this.pnlInvoiceData.SuspendLayout();
            this.tlpInvoiceData.SuspendLayout();
            this.pnlProductEntry.SuspendLayout();
            this.tlpProductEntry.SuspendLayout();
            this.pnlTotals.SuspendLayout();
            this.tlpTotals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
            this.SuspendLayout();

            // ==========================================
            // pnlHeader (تم علاج التداخل وإصلاح الـ UI)
            // ==========================================
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
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
            this.lblTitle.Text = "📝 فاتورة مشتريات (إضافة مخزون وتكلفة)";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblSubtitle
            this.lblSubtitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblSubtitle.Location = new System.Drawing.Point(0, 50);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(1200, 25);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "إدخال المشتريات للفرع المحدد وحساب متوسط التكلفة آلياً";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            // ==========================================
            // pnlInvoiceData (تحديد الفرع ورقم الفاتورة)
            // ==========================================
            this.pnlInvoiceData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlInvoiceData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInvoiceData.Controls.Add(this.tlpInvoiceData);
            this.pnlInvoiceData.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInvoiceData.Location = new System.Drawing.Point(0, 85);
            this.pnlInvoiceData.Name = "pnlInvoiceData";
            this.pnlInvoiceData.Size = new System.Drawing.Size(1200, 85); // تكبير المسافة لتناسب الخط
            this.pnlInvoiceData.TabIndex = 1;

            // tlpInvoiceData
            this.tlpInvoiceData.ColumnCount = 6;
            this.tlpInvoiceData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpInvoiceData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpInvoiceData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpInvoiceData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpInvoiceData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpInvoiceData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpInvoiceData.Controls.Add(this.lblLocation, 0, 0);
            this.tlpInvoiceData.Controls.Add(this.cmbLocation, 1, 0);
            this.tlpInvoiceData.Controls.Add(this.lblInvoiceNo, 2, 0);
            this.tlpInvoiceData.Controls.Add(this.txtInvoiceNo, 3, 0);
            this.tlpInvoiceData.Controls.Add(this.lblInvoiceDate, 4, 0);
            this.tlpInvoiceData.Controls.Add(this.dtpInvoiceDate, 5, 0);
            this.tlpInvoiceData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpInvoiceData.Location = new System.Drawing.Point(0, 0);
            this.tlpInvoiceData.Name = "tlpInvoiceData";
            this.tlpInvoiceData.Padding = new System.Windows.Forms.Padding(20, 20, 20, 15);
            this.tlpInvoiceData.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpInvoiceData.RowCount = 1;
            this.tlpInvoiceData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpInvoiceData.TabIndex = 0;

            // lblLocation
            this.lblLocation.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(222)))), ((int)(((byte)(128)))));
            this.lblLocation.Location = new System.Drawing.Point(994, 23);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(161, 21);
            this.lblLocation.TabIndex = 0;
            this.lblLocation.Text = "الفرع / المخزن المستلم:";

            // cmbLocation
            this.cmbLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLocation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.cmbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLocation.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.cmbLocation.ForeColor = System.Drawing.Color.White;
            this.cmbLocation.Location = new System.Drawing.Point(668, 23);
            this.cmbLocation.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(313, 29);
            this.cmbLocation.TabIndex = 1;

            // lblInvoiceNo
            this.lblInvoiceNo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblInvoiceNo.AutoSize = true;
            this.lblInvoiceNo.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblInvoiceNo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblInvoiceNo.Location = new System.Drawing.Point(565, 23);
            this.lblInvoiceNo.Name = "lblInvoiceNo";
            this.lblInvoiceNo.Size = new System.Drawing.Size(90, 21);
            this.lblInvoiceNo.TabIndex = 2;
            this.lblInvoiceNo.Text = "رقم الفاتورة:";

            // txtInvoiceNo
            this.txtInvoiceNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInvoiceNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtInvoiceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInvoiceNo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtInvoiceNo.ForeColor = System.Drawing.Color.White;
            this.txtInvoiceNo.Location = new System.Drawing.Point(320, 23);
            this.txtInvoiceNo.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.ReadOnly = true;
            this.txtInvoiceNo.Size = new System.Drawing.Size(232, 29);
            this.txtInvoiceNo.TabIndex = 3;
            this.txtInvoiceNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;

            // lblInvoiceDate
            this.lblInvoiceDate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblInvoiceDate.AutoSize = true;
            this.lblInvoiceDate.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblInvoiceDate.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblInvoiceDate.Location = new System.Drawing.Point(252, 23);
            this.lblInvoiceDate.Name = "lblInvoiceDate";
            this.lblInvoiceDate.Size = new System.Drawing.Size(55, 21);
            this.lblInvoiceDate.TabIndex = 4;
            this.lblInvoiceDate.Text = "التاريخ:";

            // dtpInvoiceDate
            this.dtpInvoiceDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpInvoiceDate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.dtpInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpInvoiceDate.Location = new System.Drawing.Point(30, 23);
            this.dtpInvoiceDate.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.dtpInvoiceDate.Name = "dtpInvoiceDate";
            this.dtpInvoiceDate.Size = new System.Drawing.Size(209, 29);
            this.dtpInvoiceDate.TabIndex = 5;

            // ==========================================
            // pnlProductEntry (إضافة المنتجات)
            // ==========================================
            this.pnlProductEntry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlProductEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProductEntry.Controls.Add(this.tlpProductEntry);
            this.pnlProductEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlProductEntry.Location = new System.Drawing.Point(0, 170);
            this.pnlProductEntry.Name = "pnlProductEntry";
            this.pnlProductEntry.Size = new System.Drawing.Size(1200, 85);
            this.pnlProductEntry.TabIndex = 2;

            // tlpProductEntry
            this.tlpProductEntry.ColumnCount = 9;
            this.tlpProductEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize)); // lbl Prod
            this.tlpProductEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F)); // cmb Prod
            this.tlpProductEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F)); // + btn
            this.tlpProductEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize)); // lbl Qty
            this.tlpProductEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F)); // txt Qty
            this.tlpProductEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize)); // lbl Price
            this.tlpProductEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F)); // txt Price
            this.tlpProductEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F)); // add btn
            this.tlpProductEntry.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F)); // rem btn
            this.tlpProductEntry.Controls.Add(this.lblProduct, 0, 0);
            this.tlpProductEntry.Controls.Add(this.cmbProduct, 1, 0);
            this.tlpProductEntry.Controls.Add(this.btnAddNewProduct, 2, 0);
            this.tlpProductEntry.Controls.Add(this.lblQuantity, 3, 0);
            this.tlpProductEntry.Controls.Add(this.txtQuantity, 4, 0);
            this.tlpProductEntry.Controls.Add(this.lblUnitPrice, 5, 0);
            this.tlpProductEntry.Controls.Add(this.txtUnitPrice, 6, 0);
            this.tlpProductEntry.Controls.Add(this.btnAddProduct, 7, 0);
            this.tlpProductEntry.Controls.Add(this.btnRemoveProduct, 8, 0);
            this.tlpProductEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProductEntry.Location = new System.Drawing.Point(0, 0);
            this.tlpProductEntry.Name = "tlpProductEntry";
            this.tlpProductEntry.Padding = new System.Windows.Forms.Padding(15, 20, 15, 20);
            this.tlpProductEntry.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpProductEntry.RowCount = 1;
            this.tlpProductEntry.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProductEntry.TabIndex = 0;

            // lblProduct
            this.lblProduct.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblProduct.AutoSize = true;
            this.lblProduct.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblProduct.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblProduct.Location = new System.Drawing.Point(1129, 31);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(53, 21);
            this.lblProduct.TabIndex = 0;
            this.lblProduct.Text = "المنتج:";

            // cmbProduct
            this.cmbProduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cmbProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbProduct.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.cmbProduct.ForeColor = System.Drawing.Color.White;
            this.cmbProduct.Location = new System.Drawing.Point(748, 31);
            this.cmbProduct.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(375, 29);
            this.cmbProduct.TabIndex = 1;
            this.cmbProduct.SelectedIndexChanged += new System.EventHandler(this.cmbProduct_SelectedIndexChanged);

            // btnAddNewProduct
            this.btnAddNewProduct.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnAddNewProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnAddNewProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddNewProduct.FlatAppearance.BorderSize = 0;
            this.btnAddNewProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNewProduct.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnAddNewProduct.ForeColor = System.Drawing.Color.White;
            this.btnAddNewProduct.Location = new System.Drawing.Point(690, 26);
            this.btnAddNewProduct.Margin = new System.Windows.Forms.Padding(0, 3, 10, 3);
            this.btnAddNewProduct.Name = "btnAddNewProduct";
            this.btnAddNewProduct.Size = new System.Drawing.Size(45, 30);
            this.btnAddNewProduct.TabIndex = 2;
            this.btnAddNewProduct.Text = "➕";
            this.btnAddNewProduct.UseVisualStyleBackColor = false;
            this.btnAddNewProduct.Click += new System.EventHandler(this.btnAddNewProduct_Click);

            // lblQuantity
            this.lblQuantity.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblQuantity.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblQuantity.Location = new System.Drawing.Point(636, 31);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(51, 21);
            this.lblQuantity.TabIndex = 3;
            this.lblQuantity.Text = "الكمية:";

            // txtQuantity
            this.txtQuantity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuantity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.txtQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQuantity.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtQuantity.ForeColor = System.Drawing.Color.White;
            this.txtQuantity.Location = new System.Drawing.Point(448, 27);
            this.txtQuantity.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(175, 29);
            this.txtQuantity.TabIndex = 4;
            this.txtQuantity.Text = "1";
            this.txtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyNumbers_KeyPress);

            // lblUnitPrice
            this.lblUnitPrice.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblUnitPrice.AutoSize = true;
            this.lblUnitPrice.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblUnitPrice.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblUnitPrice.Location = new System.Drawing.Point(354, 31);
            this.lblUnitPrice.Name = "lblUnitPrice";
            this.lblUnitPrice.Size = new System.Drawing.Size(81, 21);
            this.lblUnitPrice.TabIndex = 5;
            this.lblUnitPrice.Text = "سعر الشراء:";

            // txtUnitPrice
            this.txtUnitPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUnitPrice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.txtUnitPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUnitPrice.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtUnitPrice.ForeColor = System.Drawing.Color.White;
            this.txtUnitPrice.Location = new System.Drawing.Point(166, 27);
            this.txtUnitPrice.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtUnitPrice.Name = "txtUnitPrice";
            this.txtUnitPrice.Size = new System.Drawing.Size(175, 29);
            this.txtUnitPrice.TabIndex = 6;
            this.txtUnitPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtUnitPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyNumbers_KeyPress);

            // btnAddProduct
            this.btnAddProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnAddProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddProduct.FlatAppearance.BorderSize = 0;
            this.btnAddProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddProduct.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnAddProduct.ForeColor = System.Drawing.Color.White;
            this.btnAddProduct.Location = new System.Drawing.Point(73, 23);
            this.btnAddProduct.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(78, 37);
            this.btnAddProduct.TabIndex = 7;
            this.btnAddProduct.Text = "➕ إضافة";
            this.btnAddProduct.UseVisualStyleBackColor = false;
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);

            // btnRemoveProduct
            this.btnRemoveProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnRemoveProduct.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemoveProduct.FlatAppearance.BorderSize = 0;
            this.btnRemoveProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveProduct.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnRemoveProduct.ForeColor = System.Drawing.Color.White;
            this.btnRemoveProduct.Location = new System.Drawing.Point(20, 23);
            this.btnRemoveProduct.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.btnRemoveProduct.Name = "btnRemoveProduct";
            this.btnRemoveProduct.Size = new System.Drawing.Size(43, 37);
            this.btnRemoveProduct.TabIndex = 8;
            this.btnRemoveProduct.Text = "🗑️";
            this.btnRemoveProduct.UseVisualStyleBackColor = false;
            this.btnRemoveProduct.Click += new System.EventHandler(this.btnRemoveProduct_Click);

            // ==========================================
            // pnlTotals (شريط الإجماليات السفلي)
            // ==========================================
            this.pnlTotals.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlTotals.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTotals.Controls.Add(this.tlpTotals);
            this.pnlTotals.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTotals.Location = new System.Drawing.Point(0, 680);
            this.pnlTotals.Name = "pnlTotals";
            this.pnlTotals.Size = new System.Drawing.Size(1200, 120);
            this.pnlTotals.TabIndex = 4;

            // tlpTotals
            this.tlpTotals.ColumnCount = 7;
            this.tlpTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpTotals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 220F));
            this.tlpTotals.Controls.Add(this.lblTotalAmount, 0, 0);
            this.tlpTotals.Controls.Add(this.txtTotalAmount, 1, 0);
            this.tlpTotals.Controls.Add(this.lblPaidAmount, 2, 0);
            this.tlpTotals.Controls.Add(this.txtPaidAmount, 3, 0);
            this.tlpTotals.Controls.Add(this.lblRemainingAmount, 4, 0);
            this.tlpTotals.Controls.Add(this.txtRemainingAmount, 5, 0);
            this.tlpTotals.Controls.Add(this.btnSave, 6, 0);
            this.tlpTotals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTotals.Location = new System.Drawing.Point(0, 0);
            this.tlpTotals.Name = "tlpTotals";
            this.tlpTotals.Padding = new System.Windows.Forms.Padding(20, 30, 20, 30);
            this.tlpTotals.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpTotals.RowCount = 1;
            this.tlpTotals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTotals.TabIndex = 0;

            // lblTotalAmount
            this.lblTotalAmount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmount.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblTotalAmount.Location = new System.Drawing.Point(1051, 46);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(124, 25);
            this.lblTotalAmount.TabIndex = 0;
            this.lblTotalAmount.Text = "إجمالي الفاتورة:";

            // txtTotalAmount
            this.txtTotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalAmount.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.txtTotalAmount.ForeColor = System.Drawing.Color.White;
            this.txtTotalAmount.Location = new System.Drawing.Point(828, 39);
            this.txtTotalAmount.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.ReadOnly = true;
            this.txtTotalAmount.Size = new System.Drawing.Size(210, 39);
            this.txtTotalAmount.TabIndex = 1;
            this.txtTotalAmount.Text = "0.00";
            this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;

            // lblPaidAmount
            this.lblPaidAmount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPaidAmount.AutoSize = true;
            this.lblPaidAmount.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblPaidAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(222)))), ((int)(((byte)(128)))));
            this.lblPaidAmount.Location = new System.Drawing.Point(656, 46);
            this.lblPaidAmount.Name = "lblPaidAmount";
            this.lblPaidAmount.Size = new System.Drawing.Size(159, 25);
            this.lblPaidAmount.TabIndex = 2;
            this.lblPaidAmount.Text = "المبلغ المدفوع (كاش):";

            // txtPaidAmount
            this.txtPaidAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPaidAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtPaidAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaidAmount.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.txtPaidAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(222)))), ((int)(((byte)(128)))));
            this.txtPaidAmount.Location = new System.Drawing.Point(433, 39);
            this.txtPaidAmount.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtPaidAmount.Name = "txtPaidAmount";
            this.txtPaidAmount.Size = new System.Drawing.Size(210, 39);
            this.txtPaidAmount.TabIndex = 3;
            this.txtPaidAmount.Text = "0";
            this.txtPaidAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPaidAmount.TextChanged += new System.EventHandler(this.txtPaidAmount_TextChanged);
            this.txtPaidAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnlyNumbers_KeyPress);

            // lblRemainingAmount
            this.lblRemainingAmount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRemainingAmount.AutoSize = true;
            this.lblRemainingAmount.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblRemainingAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            this.lblRemainingAmount.Location = new System.Drawing.Point(293, 46);
            this.lblRemainingAmount.Name = "lblRemainingAmount";
            this.lblRemainingAmount.Size = new System.Drawing.Size(127, 25);
            this.lblRemainingAmount.TabIndex = 4;
            this.lblRemainingAmount.Text = "الآجل (المتبقي):";

            // txtRemainingAmount
            this.txtRemainingAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemainingAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.txtRemainingAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemainingAmount.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.txtRemainingAmount.ForeColor = System.Drawing.Color.White;
            this.txtRemainingAmount.Location = new System.Drawing.Point(70, 39);
            this.txtRemainingAmount.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtRemainingAmount.Name = "txtRemainingAmount";
            this.txtRemainingAmount.ReadOnly = true;
            this.txtRemainingAmount.Size = new System.Drawing.Size(210, 39);
            this.txtRemainingAmount.TabIndex = 5;
            this.txtRemainingAmount.Text = "0.00";
            this.txtRemainingAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(25, 35);
            this.btnSave.Margin = new System.Windows.Forms.Padding(15, 5, 5, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(30, 48); // هيظبط مع הـ AutoSize
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "حفظ الفاتورة ✔";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // ==========================================
            // dgvDetails (جدول الأصناف)
            // ==========================================
            this.dgvDetails.AllowUserToAddRows = false;
            this.dgvDetails.AllowUserToDeleteRows = false;
            this.dgvDetails.AllowUserToResizeRows = false;
            this.dgvDetails.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.dgvDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDetails.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDetails.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRowNumber,
            this.colProductId,
            this.colProductName,
            this.colQuantity,
            this.colUnitPrice,
            this.colTotalPrice});
            this.dgvDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetails.EnableHeadersVisualStyles = false;
            this.dgvDetails.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.dgvDetails.Location = new System.Drawing.Point(0, 255);
            this.dgvDetails.MultiSelect = false;
            this.dgvDetails.Name = "dgvDetails";
            this.dgvDetails.ReadOnly = true;
            this.dgvDetails.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dgvDetails.RowHeadersVisible = false;
            this.dgvDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetails.Size = new System.Drawing.Size(1200, 425);
            this.dgvDetails.TabIndex = 3;

            // colRowNumber
            this.colRowNumber.HeaderText = "م";
            this.colRowNumber.Name = "colRowNumber";
            this.colRowNumber.ReadOnly = true;
            this.colRowNumber.Width = 60;

            // colProductId
            this.colProductId.HeaderText = "رقم المنتج";
            this.colProductId.Name = "colProductId";
            this.colProductId.ReadOnly = true;
            this.colProductId.Visible = false;

            // colProductName
            this.colProductName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colProductName.HeaderText = "اسم المنتج";
            this.colProductName.Name = "colProductName";
            this.colProductName.ReadOnly = true;

            // colQuantity
            this.colQuantity.HeaderText = "الكمية المضافة";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.ReadOnly = true;
            this.colQuantity.Width = 150;

            // colUnitPrice
            this.colUnitPrice.HeaderText = "سعر الشراء للوحدة";
            this.colUnitPrice.Name = "colUnitPrice";
            this.colUnitPrice.ReadOnly = true;
            this.colUnitPrice.Width = 180;

            // colTotalPrice
            this.colTotalPrice.HeaderText = "إجمالي الصنف";
            this.colTotalPrice.Name = "colTotalPrice";
            this.colTotalPrice.ReadOnly = true;
            this.colTotalPrice.Width = 180;

            // ==========================================
            // frmPurchaseInvoice
            // ==========================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.dgvDetails);
            this.Controls.Add(this.pnlProductEntry);
            this.Controls.Add(this.pnlInvoiceData);
            this.Controls.Add(this.pnlTotals);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1150, 700);
            this.Name = "frmPurchaseInvoice";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "فاتورة مشتريات (إضافة مخزون)";
            this.Load += new System.EventHandler(this.frmPurchaseInvoice_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlInvoiceData.ResumeLayout(false);
            this.tlpInvoiceData.ResumeLayout(false);
            this.tlpInvoiceData.PerformLayout();
            this.pnlProductEntry.ResumeLayout(false);
            this.tlpProductEntry.ResumeLayout(false);
            this.tlpProductEntry.PerformLayout();
            this.pnlTotals.ResumeLayout(false);
            this.tlpTotals.ResumeLayout(false);
            this.tlpTotals.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel pnlInvoiceData;
        private System.Windows.Forms.TableLayoutPanel tlpInvoiceData;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Label lblInvoiceNo;
        private System.Windows.Forms.TextBox txtInvoiceNo;
        private System.Windows.Forms.Label lblInvoiceDate;
        private System.Windows.Forms.DateTimePicker dtpInvoiceDate;
        private System.Windows.Forms.Panel pnlProductEntry;
        private System.Windows.Forms.TableLayoutPanel tlpProductEntry;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.Button btnAddNewProduct;
        private System.Windows.Forms.Label lblQuantity;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.Label lblUnitPrice;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.Button btnAddProduct;
        private System.Windows.Forms.Button btnRemoveProduct;
        private System.Windows.Forms.Panel pnlTotals;
        private System.Windows.Forms.TableLayoutPanel tlpTotals;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.Label lblPaidAmount;
        private System.Windows.Forms.TextBox txtPaidAmount;
        private System.Windows.Forms.Label lblRemainingAmount;
        private System.Windows.Forms.TextBox txtRemainingAmount;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRowNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalPrice;
    }
}