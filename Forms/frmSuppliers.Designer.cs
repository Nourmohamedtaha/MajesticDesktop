namespace MajesticDesktop.Forms
{
    partial class frmSuppliers
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
            this.components = new System.ComponentModel.Container();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.tlpHeader = new System.Windows.Forms.TableLayoutPanel();
            this.pnlBranch = new System.Windows.Forms.Panel();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cmbLocations = new System.Windows.Forms.ComboBox();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnShare = new System.Windows.Forms.Button();
            this.pnlSearchFilter = new System.Windows.Forms.Panel();
            this.tlpSearch = new System.Windows.Forms.TableLayoutPanel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblRegionFilter = new System.Windows.Forms.Label();
            this.cmbRegionFilter = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnlDataEntry = new System.Windows.Forms.Panel();
            this.tlpData = new System.Windows.Forms.TableLayoutPanel();
            this.lblSupplierName = new System.Windows.Forms.Label();
            this.txtSupplierName = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblRegion = new System.Windows.Forms.Label();
            this.txtRegion = new System.Windows.Forms.TextBox();
            this.lblBalance = new System.Windows.Forms.Label();
            this.txtBalance = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.tlpButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblSectionTitle = new System.Windows.Forms.Label();
            this.pnlDebtPayment = new System.Windows.Forms.Panel();
            this.tlpDebt = new System.Windows.Forms.TableLayoutPanel();
            this.lblPaymentMethod = new System.Windows.Forms.Label();
            this.cmbPaymentMethod = new System.Windows.Forms.ComboBox();
            this.lblPaymentAmount = new System.Windows.Forms.Label();
            this.txtPaymentAmount = new System.Windows.Forms.TextBox();
            this.btnPayDebt = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvSuppliers = new System.Windows.Forms.DataGridView();
            this.colRowNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRegion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSuppliersList = new System.Windows.Forms.Label();
            this.dgvLedger = new System.Windows.Forms.DataGridView();
            this.colRefId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRunBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblLedger = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.tlpHeader.SuspendLayout();
            this.pnlBranch.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.pnlSearchFilter.SuspendLayout();
            this.tlpSearch.SuspendLayout();
            this.pnlDataEntry.SuspendLayout();
            this.tlpData.SuspendLayout();
            this.tlpButtons.SuspendLayout();
            this.pnlDebtPayment.SuspendLayout();
            this.tlpDebt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuppliers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLedger)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlHeader.Controls.Add(this.tlpHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1200, 85);
            this.pnlHeader.TabIndex = 0;
            // 
            // tlpHeader
            // 
            this.tlpHeader.ColumnCount = 3;
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpHeader.Controls.Add(this.pnlBranch, 0, 0);
            this.tlpHeader.Controls.Add(this.pnlTitle, 1, 0);
            this.tlpHeader.Controls.Add(this.pnlActions, 2, 0);
            this.tlpHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHeader.Location = new System.Drawing.Point(0, 0);
            this.tlpHeader.Name = "tlpHeader";
            this.tlpHeader.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpHeader.RowCount = 1;
            this.tlpHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHeader.Size = new System.Drawing.Size(1200, 85);
            this.tlpHeader.TabIndex = 0;
            // 
            // pnlBranch
            // 
            this.pnlBranch.Controls.Add(this.lblLocation);
            this.pnlBranch.Controls.Add(this.cmbLocations);
            this.pnlBranch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBranch.Location = new System.Drawing.Point(903, 3);
            this.pnlBranch.Name = "pnlBranch";
            this.pnlBranch.Size = new System.Drawing.Size(294, 79);
            this.pnlBranch.TabIndex = 0;
            // 
            // lblLocation
            // 
            this.lblLocation.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLocation.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblLocation.Location = new System.Drawing.Point(220, 29);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(46, 21);
            this.lblLocation.TabIndex = 2;
            this.lblLocation.Text = "الفرع:";
            // 
            // cmbLocations
            // 
            this.cmbLocations.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cmbLocations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cmbLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLocations.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.cmbLocations.ForeColor = System.Drawing.Color.White;
            this.cmbLocations.FormattingEnabled = true;
            this.cmbLocations.Location = new System.Drawing.Point(20, 25);
            this.cmbLocations.Name = "cmbLocations";
            this.cmbLocations.Size = new System.Drawing.Size(190, 29);
            this.cmbLocations.TabIndex = 3;
            this.cmbLocations.SelectedIndexChanged += new System.EventHandler(this.cmbLocations_SelectedIndexChanged);
            // 
            // pnlTitle
            // 
            this.pnlTitle.Controls.Add(this.lblSubtitle);
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTitle.Location = new System.Drawing.Point(423, 3);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(474, 79);
            this.pnlTitle.TabIndex = 1;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblSubtitle.Location = new System.Drawing.Point(0, 45);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(474, 29);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "تسجيل بيانات الموردين ومتابعة الحسابات والمديونيات";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(474, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "👥 إدارة الموردين والديون";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnImport);
            this.pnlActions.Controls.Add(this.btnPrint);
            this.pnlActions.Controls.Add(this.btnExport);
            this.pnlActions.Controls.Add(this.btnShare);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlActions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlActions.Location = new System.Drawing.Point(3, 3);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(10, 20, 10, 0);
            this.pnlActions.Size = new System.Drawing.Size(414, 79);
            this.pnlActions.TabIndex = 2;
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.btnImport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImport.FlatAppearance.BorderSize = 0;
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnImport.ForeColor = System.Drawing.Color.White;
            this.btnImport.Location = new System.Drawing.Point(279, 20);
            this.btnImport.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(110, 40);
            this.btnImport.TabIndex = 4;
            this.btnImport.Text = "📁 استيراد";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.btnPrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(169, 20);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 40);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "🖨️ طباعة";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(184)))), ((int)(((byte)(166)))));
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(59, 20);
            this.btnExport.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 40);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "📥 تصدير";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnShare
            // 
            this.btnShare.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(92)))), ((int)(((byte)(246)))));
            this.btnShare.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShare.FlatAppearance.BorderSize = 0;
            this.btnShare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShare.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnShare.ForeColor = System.Drawing.Color.White;
            this.btnShare.Location = new System.Drawing.Point(279, 60);
            this.btnShare.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnShare.Name = "btnShare";
            this.btnShare.Size = new System.Drawing.Size(110, 40);
            this.btnShare.TabIndex = 7;
            this.btnShare.Text = "📤 مشاركة";
            this.btnShare.UseVisualStyleBackColor = false;
            this.btnShare.Click += new System.EventHandler(this.btnShare_Click);
            // 
            // pnlSearchFilter
            // 
            this.pnlSearchFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlSearchFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearchFilter.Controls.Add(this.tlpSearch);
            this.pnlSearchFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearchFilter.Location = new System.Drawing.Point(0, 85);
            this.pnlSearchFilter.Name = "pnlSearchFilter";
            this.pnlSearchFilter.Size = new System.Drawing.Size(1200, 75);
            this.pnlSearchFilter.TabIndex = 1;
            // 
            // tlpSearch
            // 
            this.tlpSearch.ColumnCount = 5;
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpSearch.Controls.Add(this.lblSearch, 0, 0);
            this.tlpSearch.Controls.Add(this.txtSearch, 1, 0);
            this.tlpSearch.Controls.Add(this.lblRegionFilter, 2, 0);
            this.tlpSearch.Controls.Add(this.cmbRegionFilter, 3, 0);
            this.tlpSearch.Controls.Add(this.btnRefresh, 4, 0);
            this.tlpSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSearch.Location = new System.Drawing.Point(0, 0);
            this.tlpSearch.Name = "tlpSearch";
            this.tlpSearch.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.tlpSearch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpSearch.RowCount = 1;
            this.tlpSearch.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSearch.Size = new System.Drawing.Size(1198, 73);
            this.tlpSearch.TabIndex = 0;
            // 
            // lblSearch
            // 
            this.lblSearch.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblSearch.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSearch.Location = new System.Drawing.Point(1030, 26);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(145, 21);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "بحث بالاسم أو الهاتف:";
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtSearch.ForeColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(645, 22);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(372, 29);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblRegionFilter
            // 
            this.lblRegionFilter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblRegionFilter.AutoSize = true;
            this.lblRegionFilter.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblRegionFilter.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblRegionFilter.Location = new System.Drawing.Point(525, 26);
            this.lblRegionFilter.Name = "lblRegionFilter";
            this.lblRegionFilter.Size = new System.Drawing.Size(107, 21);
            this.lblRegionFilter.TabIndex = 2;
            this.lblRegionFilter.Text = "تصفية بالمنطقة:";
            // 
            // cmbRegionFilter
            // 
            this.cmbRegionFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbRegionFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbRegionFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbRegionFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cmbRegionFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cmbRegionFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbRegionFilter.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.cmbRegionFilter.ForeColor = System.Drawing.Color.White;
            this.cmbRegionFilter.FormattingEnabled = true;
            this.cmbRegionFilter.Location = new System.Drawing.Point(180, 22);
            this.cmbRegionFilter.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.cmbRegionFilter.Name = "cmbRegionFilter";
            this.cmbRegionFilter.Size = new System.Drawing.Size(332, 29);
            this.cmbRegionFilter.TabIndex = 3;
            this.cmbRegionFilter.TextChanged += new System.EventHandler(this.cmbRegionFilter_TextChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(23, 18);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(144, 37);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "🔄 تحديث";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pnlDataEntry
            // 
            this.pnlDataEntry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlDataEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDataEntry.Controls.Add(this.tlpData);
            this.pnlDataEntry.Controls.Add(this.lblSectionTitle);
            this.pnlDataEntry.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDataEntry.Location = new System.Drawing.Point(0, 160);
            this.pnlDataEntry.Name = "pnlDataEntry";
            this.pnlDataEntry.Size = new System.Drawing.Size(1200, 225);
            this.pnlDataEntry.TabIndex = 2;
            // 
            // tlpData
            // 
            this.tlpData.ColumnCount = 4;
            this.tlpData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpData.Controls.Add(this.lblSupplierName, 0, 0);
            this.tlpData.Controls.Add(this.txtSupplierName, 0, 1);
            this.tlpData.Controls.Add(this.lblPhone, 1, 0);
            this.tlpData.Controls.Add(this.txtPhone, 1, 1);
            this.tlpData.Controls.Add(this.lblRegion, 2, 0);
            this.tlpData.Controls.Add(this.txtRegion, 2, 1);
            this.tlpData.Controls.Add(this.lblBalance, 3, 0);
            this.tlpData.Controls.Add(this.txtBalance, 3, 1);
            this.tlpData.Controls.Add(this.lblNotes, 0, 2);
            this.tlpData.Controls.Add(this.txtNotes, 0, 3);
            this.tlpData.Controls.Add(this.tlpButtons, 2, 3);
            this.tlpData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpData.Location = new System.Drawing.Point(0, 35);
            this.tlpData.Name = "tlpData";
            this.tlpData.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.tlpData.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpData.RowCount = 4;
            this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpData.Size = new System.Drawing.Size(1198, 188);
            this.tlpData.TabIndex = 1;
            // 
            // lblSupplierName
            // 
            this.lblSupplierName.AutoSize = true;
            this.lblSupplierName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSupplierName.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblSupplierName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblSupplierName.Location = new System.Drawing.Point(888, 21);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Size = new System.Drawing.Size(87, 19);
            this.lblSupplierName.TabIndex = 0;
            this.lblSupplierName.Text = "اسم المورد *";
            // 
            // txtSupplierName
            // 
            this.txtSupplierName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSupplierName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtSupplierName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSupplierName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtSupplierName.ForeColor = System.Drawing.Color.White;
            this.txtSupplierName.Location = new System.Drawing.Point(898, 50);
            this.txtSupplierName.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtSupplierName.Name = "txtSupplierName";
            this.txtSupplierName.Size = new System.Drawing.Size(270, 29);
            this.txtSupplierName.TabIndex = 1;
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblPhone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblPhone.Location = new System.Drawing.Point(599, 21);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(78, 19);
            this.lblPhone.TabIndex = 2;
            this.lblPhone.Text = "رقم الهاتف";
            // 
            // txtPhone
            // 
            this.txtPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtPhone.ForeColor = System.Drawing.Color.White;
            this.txtPhone.Location = new System.Drawing.Point(608, 50);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(270, 29);
            this.txtPhone.TabIndex = 3;
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRegion.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblRegion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblRegion.Location = new System.Drawing.Point(309, 21);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(100, 19);
            this.lblRegion.TabIndex = 4;
            this.lblRegion.Text = "المنطقة / العنوان";
            // 
            // txtRegion
            // 
            this.txtRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRegion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtRegion.ForeColor = System.Drawing.Color.White;
            this.txtRegion.Location = new System.Drawing.Point(318, 50);
            this.txtRegion.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtRegion.Name = "txtRegion";
            this.txtRegion.Size = new System.Drawing.Size(270, 29);
            this.txtRegion.TabIndex = 5;
            // 
            // lblBalance
            // 
            this.lblBalance.AutoSize = true;
            this.lblBalance.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblBalance.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblBalance.Location = new System.Drawing.Point(20, 21);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(147, 19);
            this.lblBalance.TabIndex = 6;
            this.lblBalance.Text = "الرصيد الافتتاحي (له)";
            // 
            // txtBalance
            // 
            this.txtBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBalance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBalance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
            this.txtBalance.Location = new System.Drawing.Point(30, 50);
            this.txtBalance.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtBalance.Name = "txtBalance";
            this.txtBalance.Size = new System.Drawing.Size(268, 29);
            this.txtBalance.TabIndex = 7;
            this.txtBalance.Text = "0";
            this.txtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblNotes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblNotes.Location = new System.Drawing.Point(1088, 101);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(87, 19);
            this.lblNotes.TabIndex = 8;
            this.lblNotes.Text = "ملحوظة عامة";
            // 
            // txtNotes
            // 
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tlpData.SetColumnSpan(this.txtNotes, 2);
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtNotes.ForeColor = System.Drawing.Color.White;
            this.txtNotes.Location = new System.Drawing.Point(608, 134);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(560, 29);
            this.txtNotes.TabIndex = 9;
            // 
            // tlpButtons
            // 
            this.tlpButtons.ColumnCount = 4;
            this.tlpData.SetColumnSpan(this.tlpButtons, 2);
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpButtons.Controls.Add(this.btnSave, 0, 0);
            this.tlpButtons.Controls.Add(this.btnEdit, 1, 0);
            this.tlpButtons.Controls.Add(this.btnClear, 2, 0);
            this.tlpButtons.Controls.Add(this.btnDelete, 3, 0);
            this.tlpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButtons.Location = new System.Drawing.Point(23, 123);
            this.tlpButtons.Name = "tlpButtons";
            this.tlpButtons.RowCount = 1;
            this.tlpButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpButtons.Size = new System.Drawing.Size(572, 52);
            this.tlpButtons.TabIndex = 10;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(434, 5);
            this.btnSave.Margin = new System.Windows.Forms.Padding(5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(133, 42);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "➕ حفظ جديد";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.Location = new System.Drawing.Point(291, 5);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(5);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(133, 42);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "✏️ تعديل المختار";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(148, 5);
            this.btnClear.Margin = new System.Windows.Forms.Padding(5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(133, 42);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "🔄 تفريغ الخانات";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(5, 5);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(133, 42);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "🗑️ حذف";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblSectionTitle
            // 
            this.lblSectionTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblSectionTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSectionTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSectionTitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSectionTitle.Location = new System.Drawing.Point(0, 0);
            this.lblSectionTitle.Name = "lblSectionTitle";
            this.lblSectionTitle.Size = new System.Drawing.Size(1198, 35);
            this.lblSectionTitle.TabIndex = 0;
            this.lblSectionTitle.Text = "تسجيل بيانات المورد الجديد";
            this.lblSectionTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlDebtPayment
            // 
            this.pnlDebtPayment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlDebtPayment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDebtPayment.Controls.Add(this.tlpDebt);
            this.pnlDebtPayment.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDebtPayment.Location = new System.Drawing.Point(0, 385);
            this.pnlDebtPayment.Name = "pnlDebtPayment";
            this.pnlDebtPayment.Size = new System.Drawing.Size(1200, 115);
            this.pnlDebtPayment.TabIndex = 3;
            // 
            // tlpDebt
            // 
            this.tlpDebt.ColumnCount = 5;
            this.tlpDebt.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tlpDebt.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDebt.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpDebt.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpDebt.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            // تم التعديل: صف تاني اتضاف عشان يستوعب اختيار وسيلة الدفع فوق خانة المبلغ
            this.tlpDebt.Controls.Add(this.lblPaymentMethod, 1, 0);
            this.tlpDebt.Controls.Add(this.cmbPaymentMethod, 2, 0);
            this.tlpDebt.Controls.Add(this.lblPaymentAmount, 1, 1);
            this.tlpDebt.Controls.Add(this.txtPaymentAmount, 2, 1);
            this.tlpDebt.Controls.Add(this.btnPayDebt, 3, 0);
            this.tlpDebt.SetRowSpan(this.btnPayDebt, 2);
            this.tlpDebt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDebt.Location = new System.Drawing.Point(0, 0);
            this.tlpDebt.Name = "tlpDebt";
            this.tlpDebt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpDebt.RowCount = 2;
            this.tlpDebt.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDebt.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDebt.Size = new System.Drawing.Size(1198, 113);
            this.tlpDebt.TabIndex = 0;
            // 
            // lblPaymentMethod
            // 
            this.lblPaymentMethod.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPaymentMethod.AutoSize = true;
            this.lblPaymentMethod.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPaymentMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblPaymentMethod.Location = new System.Drawing.Point(875, 12);
            this.lblPaymentMethod.Name = "lblPaymentMethod";
            this.lblPaymentMethod.Size = new System.Drawing.Size(90, 21);
            this.lblPaymentMethod.TabIndex = 0;
            this.lblPaymentMethod.Text = "طريقة السداد:";
            // 
            // cmbPaymentMethod
            // 
            this.cmbPaymentMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPaymentMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMethod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPaymentMethod.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            this.cmbPaymentMethod.ForeColor = System.Drawing.Color.White;
            this.cmbPaymentMethod.FormattingEnabled = true;
            this.cmbPaymentMethod.Items.AddRange(new object[] {
            "نقدي",
            "انستاباي الكردي",
            "انستاباي الحكش"});
            this.cmbPaymentMethod.Location = new System.Drawing.Point(463, 8);
            this.cmbPaymentMethod.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.cmbPaymentMethod.Name = "cmbPaymentMethod";
            this.cmbPaymentMethod.SelectedIndex = 0;
            this.cmbPaymentMethod.Size = new System.Drawing.Size(345, 32);
            this.cmbPaymentMethod.TabIndex = 1;
            // 
            // lblPaymentAmount
            // 
            this.lblPaymentAmount.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPaymentAmount.AutoSize = true;
            this.lblPaymentAmount.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            this.lblPaymentAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(222)))), ((int)(((byte)(128)))));
            this.lblPaymentAmount.Location = new System.Drawing.Point(821, 44);
            this.lblPaymentAmount.Name = "lblPaymentAmount";
            this.lblPaymentAmount.Size = new System.Drawing.Size(272, 23);
            this.lblPaymentAmount.TabIndex = 2;
            this.lblPaymentAmount.Text = "سداد دفعة من مديونية المورد المحدد:";
            // 
            // txtPaymentAmount
            // 
            this.txtPaymentAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPaymentAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.txtPaymentAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPaymentAmount.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.txtPaymentAmount.ForeColor = System.Drawing.Color.White;
            this.txtPaymentAmount.Location = new System.Drawing.Point(463, 38);
            this.txtPaymentAmount.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.txtPaymentAmount.Name = "txtPaymentAmount";
            this.txtPaymentAmount.Size = new System.Drawing.Size(345, 34);
            this.txtPaymentAmount.TabIndex = 3;
            this.txtPaymentAmount.Text = "0";
            this.txtPaymentAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnPayDebt
            // 
            this.btnPayDebt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPayDebt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(163)))), ((int)(((byte)(74)))));
            this.btnPayDebt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPayDebt.FlatAppearance.BorderSize = 0;
            this.btnPayDebt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayDebt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnPayDebt.ForeColor = System.Drawing.Color.White;
            this.btnPayDebt.Location = new System.Drawing.Point(303, 36);
            this.btnPayDebt.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.btnPayDebt.Name = "btnPayDebt";
            this.btnPayDebt.Size = new System.Drawing.Size(140, 40);
            this.btnPayDebt.TabIndex = 4;
            this.btnPayDebt.Text = "✅ تأكيد السداد";
            this.btnPayDebt.UseVisualStyleBackColor = false;
            this.btnPayDebt.Click += new System.EventHandler(this.btnPayDebt_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 460);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvSuppliers);
            this.splitContainer1.Panel1.Controls.Add(this.lblSuppliersList);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvLedger);
            this.splitContainer1.Panel2.Controls.Add(this.lblLedger);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer1.Size = new System.Drawing.Size(1200, 340);
            this.splitContainer1.SplitterDistance = 640;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 4;
            // 
            // dgvSuppliers
            // 
            this.dgvSuppliers.AllowUserToAddRows = false;
            this.dgvSuppliers.AllowUserToDeleteRows = false;
            this.dgvSuppliers.AllowUserToResizeRows = false;
            this.dgvSuppliers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSuppliers.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.dgvSuppliers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSuppliers.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSuppliers.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvSuppliers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRowNumber,
            this.colId,
            this.colName,
            this.colPhone,
            this.colRegion,
            this.colNotes,
            this.colBalance});
            this.dgvSuppliers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSuppliers.EnableHeadersVisualStyles = false;
            this.dgvSuppliers.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.dgvSuppliers.Location = new System.Drawing.Point(0, 45);
            this.dgvSuppliers.Name = "dgvSuppliers";
            this.dgvSuppliers.ReadOnly = true;
            this.dgvSuppliers.RowHeadersVisible = false;
            this.dgvSuppliers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSuppliers.TabIndex = 1;
            this.dgvSuppliers.AutoGenerateColumns = false;
            this.dgvSuppliers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSuppliers_CellClick);
            // 
            // colRowNumber
            // 
            this.colRowNumber.HeaderText = "م";
            this.colRowNumber.Name = "colRowNumber";
            this.colRowNumber.ReadOnly = true;
            this.colRowNumber.Width = 60;
            // 
            // colId
            // 
            this.colId.HeaderText = "ID";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Visible = false;
            // 
            // colName
            // 
            this.colName.HeaderText = "اسم المورد";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colPhone
            // 
            this.colPhone.HeaderText = "الهاتف";
            this.colPhone.Name = "colPhone";
            this.colPhone.ReadOnly = true;
            // 
            // colRegion
            // 
            this.colRegion.HeaderText = "المنطقة";
            this.colRegion.Name = "colRegion";
            this.colRegion.ReadOnly = true;
            // 
            // colNotes
            // 
            this.colNotes.HeaderText = "الملحوظة";
            this.colNotes.Name = "colNotes";
            this.colNotes.ReadOnly = true;
            // 
            // colBalance
            // 
            this.colBalance.HeaderText = "المديونية (له)";
            this.colBalance.Name = "colBalance";
            this.colBalance.ReadOnly = true;
            // 
            // lblSuppliersList
            // 
            this.lblSuppliersList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblSuppliersList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSuppliersList.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblSuppliersList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(189)))), ((int)(((byte)(248)))));
            this.lblSuppliersList.Location = new System.Drawing.Point(0, 0);
            this.lblSuppliersList.Name = "lblSuppliersList";
            this.lblSuppliersList.Size = new System.Drawing.Size(640, 45);
            this.lblSuppliersList.TabIndex = 0;
            this.lblSuppliersList.Text = "قائمة الموردين";
            this.lblSuppliersList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvLedger
            // 
            this.dgvLedger.AllowUserToAddRows = false;
            this.dgvLedger.AllowUserToDeleteRows = false;
            this.dgvLedger.AllowUserToResizeRows = false;
            this.dgvLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLedger.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.dgvLedger.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLedger.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLedger.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvLedger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRefId,
            this.colTransType,
            this.colTransDate,
            this.colDesc,
            this.colDebit,
            this.colCredit,
            this.colRunBalance});
            this.dgvLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLedger.EnableHeadersVisualStyles = false;
            this.dgvLedger.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.dgvLedger.Location = new System.Drawing.Point(0, 45);
            this.dgvLedger.Name = "dgvLedger";
            this.dgvLedger.ReadOnly = true;
            this.dgvLedger.RowHeadersVisible = false;
            this.dgvLedger.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLedger.TabIndex = 1;
            this.dgvLedger.AutoGenerateColumns = false;
            this.dgvLedger.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLedger_CellDoubleClick);
            // 
            // colRefId
            // 
            this.colRefId.HeaderText = "RefId";
            this.colRefId.Name = "colRefId";
            this.colRefId.ReadOnly = true;
            this.colRefId.Visible = false;
            // 
            // colTransType
            // 
            this.colTransType.HeaderText = "TransType";
            this.colTransType.Name = "colTransType";
            this.colTransType.ReadOnly = true;
            this.colTransType.Visible = false;
            // 
            // colTransDate
            // 
            this.colTransDate.HeaderText = "التاريخ";
            this.colTransDate.Name = "colTransDate";
            this.colTransDate.ReadOnly = true;
            // 
            // colDesc
            // 
            this.colDesc.HeaderText = "البيان";
            this.colDesc.Name = "colDesc";
            this.colDesc.ReadOnly = true;
            // 
            // colDebit
            // 
            this.colDebit.HeaderText = "مدين (دفعة)";
            this.colDebit.Name = "colDebit";
            this.colDebit.ReadOnly = true;
            // 
            // colCredit
            // 
            this.colCredit.HeaderText = "دائن (بضاعة)";
            this.colCredit.Name = "colCredit";
            this.colCredit.ReadOnly = true;
            // 
            // colRunBalance
            // 
            this.colRunBalance.HeaderText = "الرصيد";
            this.colRunBalance.Name = "colRunBalance";
            this.colRunBalance.ReadOnly = true;
            // 
            // lblLedger
            // 
            this.lblLedger.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.lblLedger.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLedger.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblLedger.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(204)))), ((int)(((byte)(21)))));
            this.lblLedger.Location = new System.Drawing.Point(0, 0);
            this.lblLedger.Name = "lblLedger";
            this.lblLedger.Size = new System.Drawing.Size(554, 45);
            this.lblLedger.TabIndex = 0;
            this.lblLedger.Text = "كشف حساب المورد (دبل كليك على الفاتورة لفتحها)";
            this.lblLedger.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmSuppliers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.pnlDebtPayment);
            this.Controls.Add(this.pnlDataEntry);
            this.Controls.Add(this.pnlSearchFilter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1150, 750);
            this.Name = "frmSuppliers";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إدارة الموردين والديون";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSuppliers_Load);
            this.pnlHeader.ResumeLayout(false);
            this.tlpHeader.ResumeLayout(false);
            this.pnlBranch.ResumeLayout(false);
            this.pnlBranch.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.pnlSearchFilter.ResumeLayout(false);
            this.tlpSearch.ResumeLayout(false);
            this.tlpSearch.PerformLayout();
            this.pnlDataEntry.ResumeLayout(false);
            this.tlpData.ResumeLayout(false);
            this.tlpData.PerformLayout();
            this.tlpButtons.ResumeLayout(false);
            this.pnlDebtPayment.ResumeLayout(false);
            this.tlpDebt.ResumeLayout(false);
            this.tlpDebt.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuppliers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLedger)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.TableLayoutPanel tlpHeader;
        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel pnlBranch;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cmbLocations;
        private System.Windows.Forms.FlowLayoutPanel pnlActions;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnShare;
        private System.Windows.Forms.Panel pnlSearchFilter;
        private System.Windows.Forms.TableLayoutPanel tlpSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblRegionFilter;
        private System.Windows.Forms.ComboBox cmbRegionFilter;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel pnlDataEntry;
        private System.Windows.Forms.Label lblSectionTitle;
        private System.Windows.Forms.TableLayoutPanel tlpData;
        private System.Windows.Forms.Label lblSupplierName;
        private System.Windows.Forms.TextBox txtSupplierName;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.TextBox txtRegion;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.TextBox txtBalance;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.TableLayoutPanel tlpButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel pnlDebtPayment;
        private System.Windows.Forms.TableLayoutPanel tlpDebt;
        private System.Windows.Forms.Label lblPaymentMethod;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;
        private System.Windows.Forms.Label lblPaymentAmount;
        private System.Windows.Forms.TextBox txtPaymentAmount;
        private System.Windows.Forms.Button btnPayDebt;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblSuppliersList;
        private System.Windows.Forms.DataGridView dgvSuppliers;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRowNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBalance;
        private System.Windows.Forms.Label lblLedger;
        private System.Windows.Forms.DataGridView dgvLedger;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRefId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTransDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRunBalance;
    }
}