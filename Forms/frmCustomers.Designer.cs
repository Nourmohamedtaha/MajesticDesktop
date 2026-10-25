using System.Drawing;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    partial class frmCustomers
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        // ---------- لوحة الألوان والخطوط (بتتستخدم كمان في الكود) ----------
        private static readonly Color DzDark = Color.FromArgb(15, 23, 42);
        private static readonly Color DzHeader = Color.FromArgb(9, 14, 28);
        private static readonly Color DzPanel = Color.FromArgb(30, 41, 59);
        private static readonly Color DzBar = Color.FromArgb(20, 30, 45);
        private static readonly Color DzInput = Color.FromArgb(15, 23, 42);
        private static readonly Color DzText = Color.FromArgb(241, 245, 249);
        private static readonly Color DzMuted = Color.FromArgb(148, 163, 184);
        private static readonly Color DzBlue = Color.FromArgb(59, 130, 246);
        private static readonly Color DzGreen = Color.FromArgb(22, 163, 74);
        private static readonly Color DzRed = Color.FromArgb(220, 38, 38);
        private static readonly Color DzOrange = Color.FromArgb(217, 119, 6);
        private static readonly Color DzTeal = Color.FromArgb(20, 184, 166);
        private static readonly Color DzPurple = Color.FromArgb(139, 92, 246);
        private static readonly Color DzSlate = Color.FromArgb(71, 85, 105);
        private static readonly Color DzGridAlt = Color.FromArgb(38, 50, 71);
        private static readonly Color DzGridLine = Color.FromArgb(51, 65, 85);
        private static readonly Color DzSelect = Color.FromArgb(45, 120, 190);

        private static readonly Font DzLabelFont = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        private static readonly Font DzInputFont = new Font("Segoe UI", 12F, FontStyle.Bold);
        private static readonly Font DzButtonFont = new Font("Segoe UI", 11F, FontStyle.Bold);

        // ---------- دوال مساعدة ----------
        private static Button MakeButton(string text, Color back, int width, int height)
        {
            Button b = new Button
            {
                Text = text,
                BackColor = back,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = DzButtonFont,
                Cursor = Cursors.Hand,
                Size = new Size(width, height),
                UseVisualStyleBackColor = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = ControlPaint.Light(back, 0.2F);
            return b;
        }

        private static void StyleText(TextBox t)
        {
            t.BackColor = DzInput;
            t.ForeColor = DzText;
            t.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void StyleCombo(ComboBox c)
        {
            c.BackColor = DzInput;
            c.ForeColor = DzText;
            c.FlatStyle = FlatStyle.Flat;
        }

        // كارت حقل: عنوان فوق + الحقل تحته
        private static Panel MakeField(string caption, Control input)
        {
            Panel p = new Panel { Dock = DockStyle.Fill, Margin = new Padding(6, 4, 6, 4), BackColor = Color.Transparent };
            Label l = new Label
            {
                Text = caption,
                Dock = DockStyle.Top,
                Height = 24,
                ForeColor = DzMuted,
                Font = DzLabelFont,
                TextAlign = ContentAlignment.MiddleCenter
            };
            input.Dock = DockStyle.Top;
            input.Font = DzInputFont;
            p.Controls.Add(input);
            p.Controls.Add(l);
            return p;
        }

        private static DataGridView MakeGrid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
        }

        private static DataGridViewTextBoxColumn MakeColumn(string name, string header, string prop, float weight, bool sortable)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = prop,
                FillWeight = weight,
                ReadOnly = true,
                SortMode = sortable ? DataGridViewColumnSortMode.Programmatic : DataGridViewColumnSortMode.NotSortable
            };
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ================= إنشاء العناصر =================
            this.pnlHeader = new Panel();
            this.lblTitle = new Label();
            this.lblSubtitle = new Label();
            this.pnlHeaderLine = new Panel();

            this.pnlTopBar = new Panel();
            this.tlpTopBar = new TableLayoutPanel();
            this.pnlBranch = new Panel();
            this.lblBranch = new Label();
            this.cmbLocations = new ComboBox();
            this.flpActions = new FlowLayoutPanel();
            this.btnImport = MakeButton("📁 استيراد", DzOrange, 130, 44);
            this.btnPrint = MakeButton("🖨️ طباعة", DzBlue, 120, 44);
            this.btnExport = MakeButton("📥 تصدير Excel", DzTeal, 150, 44);
            this.btnShare = MakeButton("📤 مشاركة", DzPurple, 120, 44);

            this.pnlSearch = new Panel();
            this.tlpSearch = new TableLayoutPanel();
            this.txtSearch = new TextBox();
            this.cmbRegionFilter = new ComboBox();
            this.btnRefresh = MakeButton("🔄 تحديث", DzSlate, 130, 38);

            this.pnlEntry = new Panel();
            this.lblSectionTitle = new Label();
            this.tlpEntry = new TableLayoutPanel();
            this.txtCustomerName = new TextBox();
            this.txtPhone = new TextBox();
            this.txtRegion = new TextBox();
            this.txtBalance = new TextBox();
            this.txtNotes = new TextBox();
            this.tlpButtons = new TableLayoutPanel();
            this.btnSave = MakeButton("💾 إضافة جديد", DzGreen, 100, 34);
            this.btnEdit = MakeButton("✏️ تعديل", DzBlue, 100, 34);
            this.btnClear = MakeButton("🧹 تفريغ", DzSlate, 100, 34);
            this.btnDelete = MakeButton("🗑️ حذف", DzRed, 100, 34);

            this.pnlDebt = new Panel();
            this.tlpDebt = new TableLayoutPanel();
            this.lblPayTitle = new Label();
            this.txtPaymentAmount = new TextBox();
            this.cmbPayMethod = new ComboBox();
            this.btnPayDebt = MakeButton("✅ تأكيد السداد", DzGreen, 190, 44);

            this.splitContainer1 = new SplitContainer();
            this.lblCustomersList = new Label();
            this.lblLedger = new Label();
            this.dgvCustomers = MakeGrid();
            this.dgvLedger = MakeGrid();

            this.colId = MakeColumn("colId", "م", "Id", 35F, true);
            this.colName = MakeColumn("colName", "اسم العميل", "Name", 160F, true);
            this.colPhone = MakeColumn("colPhone", "الهاتف", "Phone", 95F, true);
            this.colRegion = MakeColumn("colRegion", "المنطقة", "Region", 90F, true);
            this.colNotes = MakeColumn("colNotes", "الملحوظة", "Notes", 120F, true);
            this.colBalance = MakeColumn("colBalance", "المديونية", "Balance", 85F, true);

            this.colRefId = MakeColumn("colRefId", "RefId", "RefId", 10F, false);
            this.colTransType = MakeColumn("colTransType", "TransType", "TransType", 10F, false);
            this.colTransDate = MakeColumn("colTransDate", "التاريخ", "TransDate", 70F, false);
            this.colDesc = MakeColumn("colDesc", "البيان", "Description", 200F, false);
            this.colDebit = MakeColumn("colDebit", "مدين (عليه)", "Debit", 80F, false);
            this.colCredit = MakeColumn("colCredit", "دائن (سدد)", "Credit", 80F, false);
            this.colRunBalance = MakeColumn("colRunBalance", "الرصيد", "Balance", 90F, false);
            this.colRefId.Visible = false;
            this.colTransType.Visible = false;

            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLedger)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();

            // ================= الهيدر (العنوان في نص الشاشة) =================
            this.pnlHeader.BackColor = DzHeader;
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 86;
            this.pnlHeader.Controls.Add(this.lblSubtitle);   // الـ Fill الأول
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.pnlHeaderLine);

            this.lblTitle.Dock = DockStyle.Top;
            this.lblTitle.Height = 50;
            this.lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            this.lblTitle.ForeColor = DzText;
            this.lblTitle.Padding = new Padding(0, 6, 0, 0);
            this.lblTitle.Text = "إدارة العملاء والديون 👥";
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            this.lblSubtitle.Dock = DockStyle.Fill;
            this.lblSubtitle.Font = new Font("Segoe UI", 11F);
            this.lblSubtitle.ForeColor = DzMuted;
            this.lblSubtitle.Text = "تسجيل بيانات العملاء ومتابعة الحسابات والمديونيات";
            this.lblSubtitle.TextAlign = ContentAlignment.TopCenter;

            this.pnlHeaderLine.BackColor = DzBlue;
            this.pnlHeaderLine.Dock = DockStyle.Bottom;
            this.pnlHeaderLine.Height = 4;

            // ================= شريط الفرع + أزرار الملفات =================
            this.pnlTopBar.BackColor = DzDark;
            this.pnlTopBar.Dock = DockStyle.Top;
            this.pnlTopBar.Height = 68;
            this.pnlTopBar.Padding = new Padding(16, 10, 16, 6);
            this.pnlTopBar.Controls.Add(this.tlpTopBar);

            // في RTL: العمود 0 على اليمين (الفرع) والأخير على الشمال (الأزرار)
            this.tlpTopBar.Dock = DockStyle.Fill;
            this.tlpTopBar.ColumnCount = 3;
            this.tlpTopBar.RowCount = 1;
            this.tlpTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 340F));
            this.tlpTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tlpTopBar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.tlpTopBar.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tlpTopBar.Controls.Add(this.pnlBranch, 0, 0);
            this.tlpTopBar.Controls.Add(this.flpActions, 2, 0);

            this.pnlBranch.Dock = DockStyle.Fill;
            this.pnlBranch.Margin = new Padding(0);
            this.pnlBranch.Padding = new Padding(0, 9, 0, 0);
            this.pnlBranch.Controls.Add(this.cmbLocations);
            this.pnlBranch.Controls.Add(this.lblBranch);

            this.lblBranch.Dock = DockStyle.Right;
            this.lblBranch.Width = 90;
            this.lblBranch.Font = DzLabelFont;
            this.lblBranch.ForeColor = DzText;
            this.lblBranch.Text = "اختر الفرع:";
            this.lblBranch.TextAlign = ContentAlignment.MiddleCenter;

            StyleCombo(this.cmbLocations);
            this.cmbLocations.Dock = DockStyle.Fill;
            this.cmbLocations.Font = DzInputFont;
            this.cmbLocations.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbLocations.SelectionChangeCommitted += new System.EventHandler(this.cmbLocations_SelectionChangeCommitted);

            this.flpActions.Anchor = AnchorStyles.None;
            this.flpActions.AutoSize = true;
            this.flpActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.flpActions.FlowDirection = FlowDirection.LeftToRight;   // في RTL بيبدأ من اليمين
            this.flpActions.WrapContents = false;
            this.flpActions.Margin = new Padding(0);
            this.flpActions.Controls.Add(this.btnImport);
            this.flpActions.Controls.Add(this.btnPrint);
            this.flpActions.Controls.Add(this.btnExport);
            this.flpActions.Controls.Add(this.btnShare);
            foreach (Button b in new[] { this.btnImport, this.btnPrint, this.btnExport, this.btnShare })
                b.Margin = new Padding(6, 0, 0, 0);

            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            this.btnShare.Click += new System.EventHandler(this.btnShare_Click);

            // ================= البحث والتصفية =================
            this.pnlSearch.BackColor = DzPanel;
            this.pnlSearch.Dock = DockStyle.Top;
            this.pnlSearch.Height = 88;
            this.pnlSearch.Padding = new Padding(16, 8, 16, 8);
            this.pnlSearch.Controls.Add(this.tlpSearch);

            this.tlpSearch.Dock = DockStyle.Fill;
            this.tlpSearch.ColumnCount = 3;
            this.tlpSearch.RowCount = 1;
            this.tlpSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            this.tlpSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            this.tlpSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            this.tlpSearch.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            StyleText(this.txtSearch);
            StyleCombo(this.cmbRegionFilter);
            this.cmbRegionFilter.DropDownStyle = ComboBoxStyle.DropDown;
            this.cmbRegionFilter.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.cmbRegionFilter.AutoCompleteSource = AutoCompleteSource.ListItems;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.cmbRegionFilter.TextChanged += new System.EventHandler(this.cmbRegionFilter_TextChanged);

            this.tlpSearch.Controls.Add(MakeField("بحث بالاسم أو الهاتف", this.txtSearch), 0, 0);
            this.tlpSearch.Controls.Add(MakeField("تصفية بالمنطقة", this.cmbRegionFilter), 1, 0);

            this.btnRefresh.Dock = DockStyle.Fill;
            this.btnRefresh.Margin = new Padding(8, 28, 8, 6);
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            this.tlpSearch.Controls.Add(this.btnRefresh, 2, 0);

            // ================= إدخال بيانات العميل =================
            this.pnlEntry.BackColor = DzPanel;
            this.pnlEntry.Dock = DockStyle.Top;
            this.pnlEntry.Height = 192;
            this.pnlEntry.Controls.Add(this.tlpEntry);        // الـ Fill الأول
            this.pnlEntry.Controls.Add(this.lblSectionTitle);

            this.lblSectionTitle.BackColor = DzBar;
            this.lblSectionTitle.Dock = DockStyle.Top;
            this.lblSectionTitle.Height = 34;
            this.lblSectionTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblSectionTitle.ForeColor = DzText;
            this.lblSectionTitle.Text = "بيانات العميل (اختر عميلًا من القائمة لتعديله، أو اكتب بيانات جديدة للإضافة)";
            this.lblSectionTitle.TextAlign = ContentAlignment.MiddleCenter;

            this.tlpEntry.Dock = DockStyle.Fill;
            this.tlpEntry.Padding = new Padding(12, 8, 12, 8);
            this.tlpEntry.ColumnCount = 4;
            this.tlpEntry.RowCount = 2;
            this.tlpEntry.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            this.tlpEntry.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tlpEntry.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            this.tlpEntry.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            this.tlpEntry.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));
            this.tlpEntry.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F));

            StyleText(this.txtCustomerName);
            StyleText(this.txtPhone);
            StyleText(this.txtRegion);
            StyleText(this.txtBalance);
            StyleText(this.txtNotes);
            this.txtBalance.Text = "0";
            this.txtBalance.TextAlign = HorizontalAlignment.Center;
            this.txtBalance.ForeColor = Color.FromArgb(255, 120, 120);

            this.tlpEntry.Controls.Add(MakeField("اسم العميل *", this.txtCustomerName), 0, 0);
            this.tlpEntry.Controls.Add(MakeField("رقم الهاتف", this.txtPhone), 1, 0);
            this.tlpEntry.Controls.Add(MakeField("المنطقة / العنوان", this.txtRegion), 2, 0);
            this.tlpEntry.Controls.Add(MakeField("الرصيد الافتتاحي (عليه)", this.txtBalance), 3, 0);

            Panel notesField = MakeField("ملاحظات عامة", this.txtNotes);
            this.tlpEntry.Controls.Add(notesField, 0, 1);
            this.tlpEntry.SetColumnSpan(notesField, 2);

            this.tlpButtons.Dock = DockStyle.Fill;
            this.tlpButtons.Margin = new Padding(6, 28, 6, 6);
            this.tlpButtons.ColumnCount = 4;
            this.tlpButtons.RowCount = 1;
            for (int i = 0; i < 4; i++)
                this.tlpButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.tlpButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tlpButtons.Controls.Add(this.btnSave, 0, 0);
            this.tlpButtons.Controls.Add(this.btnEdit, 1, 0);
            this.tlpButtons.Controls.Add(this.btnClear, 2, 0);
            this.tlpButtons.Controls.Add(this.btnDelete, 3, 0);
            foreach (Button b in new[] { this.btnSave, this.btnEdit, this.btnClear, this.btnDelete })
            {
                b.Dock = DockStyle.Fill;
                b.Margin = new Padding(3, 0, 3, 0);
            }
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.tlpEntry.Controls.Add(this.tlpButtons, 2, 1);
            this.tlpEntry.SetColumnSpan(this.tlpButtons, 2);

            // ================= سداد الديون =================
            this.pnlDebt.BackColor = Color.FromArgb(20, 40, 32);
            this.pnlDebt.Dock = DockStyle.Top;
            this.pnlDebt.Height = 76;
            this.pnlDebt.Padding = new Padding(16, 8, 16, 8);
            this.pnlDebt.Controls.Add(this.tlpDebt);

            // [العنوان][المبلغ][طريقة الدفع][الزرار][فاضي]
            this.tlpDebt.Dock = DockStyle.Fill;
            this.tlpDebt.ColumnCount = 5;
            this.tlpDebt.RowCount = 1;
            this.tlpDebt.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.tlpDebt.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            this.tlpDebt.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210F));
            this.tlpDebt.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            this.tlpDebt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tlpDebt.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            this.lblPayTitle.Anchor = AnchorStyles.None;
            this.lblPayTitle.AutoSize = true;
            this.lblPayTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblPayTitle.ForeColor = Color.FromArgb(134, 239, 172);
            this.lblPayTitle.Margin = new Padding(0, 0, 14, 0);
            this.lblPayTitle.Text = "سداد دفعة من مديونية العميل المحدد:";

            this.txtPaymentAmount.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.txtPaymentAmount.BackColor = Color.FromArgb(30, 50, 40);
            this.txtPaymentAmount.ForeColor = Color.White;
            this.txtPaymentAmount.BorderStyle = BorderStyle.FixedSingle;
            this.txtPaymentAmount.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.txtPaymentAmount.Margin = new Padding(6, 0, 6, 0);
            this.txtPaymentAmount.Text = "0";
            this.txtPaymentAmount.TextAlign = HorizontalAlignment.Center;

            StyleCombo(this.cmbPayMethod);
            this.cmbPayMethod.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.cmbPayMethod.Font = DzInputFont;
            this.cmbPayMethod.Margin = new Padding(6, 0, 6, 0);
            this.cmbPayMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbPayMethod.Items.AddRange(new object[] { "نقدي", "انستاباي الكردي", "انستاباي الحكش" });
            this.cmbPayMethod.SelectedIndex = 0;

            this.btnPayDebt.Dock = DockStyle.Fill;
            this.btnPayDebt.Margin = new Padding(6, 2, 6, 2);
            this.btnPayDebt.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.btnPayDebt.Click += new System.EventHandler(this.btnPayDebt_Click);

            this.tlpDebt.Controls.Add(this.lblPayTitle, 0, 0);
            this.tlpDebt.Controls.Add(this.txtPaymentAmount, 1, 0);
            this.tlpDebt.Controls.Add(this.cmbPayMethod, 2, 0);
            this.tlpDebt.Controls.Add(this.btnPayDebt, 3, 0);

            // ================= قائمة العملاء + الكشف =================
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.BackColor = DzDark;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.Panel1.Controls.Add(this.dgvCustomers);
            this.splitContainer1.Panel1.Controls.Add(this.lblCustomersList);
            this.splitContainer1.Panel2.Controls.Add(this.dgvLedger);
            this.splitContainer1.Panel2.Controls.Add(this.lblLedger);

            this.lblCustomersList.BackColor = Color.FromArgb(25, 55, 95);
            this.lblCustomersList.Dock = DockStyle.Top;
            this.lblCustomersList.Height = 38;
            this.lblCustomersList.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            this.lblCustomersList.ForeColor = Color.White;
            this.lblCustomersList.Text = "قائمة العملاء";
            this.lblCustomersList.TextAlign = ContentAlignment.MiddleCenter;

            this.lblLedger.BackColor = DzOrange;
            this.lblLedger.Dock = DockStyle.Top;
            this.lblLedger.Height = 38;
            this.lblLedger.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            this.lblLedger.ForeColor = Color.White;
            this.lblLedger.Text = "كشف حساب العميل (دبل كليك على الفاتورة لفتحها)";
            this.lblLedger.TextAlign = ContentAlignment.MiddleCenter;

            this.dgvCustomers.Columns.AddRange(new DataGridViewColumn[] {
                this.colId, this.colName, this.colPhone, this.colRegion, this.colNotes, this.colBalance });
            this.dgvCustomers.SelectionChanged += new System.EventHandler(this.dgvCustomers_SelectionChanged);
            this.dgvCustomers.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvCustomers_CellFormatting);
            this.dgvCustomers.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.dgvCustomers_ColumnHeaderMouseClick);

            this.dgvLedger.Columns.AddRange(new DataGridViewColumn[] {
                this.colRefId, this.colTransType, this.colTransDate, this.colDesc, this.colDebit, this.colCredit, this.colRunBalance });
            this.dgvLedger.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvLedger_CellDoubleClick);
            this.dgvLedger.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvLedger_CellFormatting);

            // ================= الفورم =================
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.BackColor = DzDark;
            this.ClientSize = new Size(1250, 800);
            this.MinimumSize = new Size(1150, 720);
            this.Controls.Add(this.splitContainer1);   // الـ Fill الأول
            this.Controls.Add(this.pnlDebt);
            this.Controls.Add(this.pnlEntry);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlTopBar);
            this.Controls.Add(this.pnlHeader);
            this.Font = new Font("Segoe UI", 9F);
            this.Name = "frmCustomers";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = false;            // بدون عكس الإحداثيات
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "إدارة العملاء والديون";
            this.Load += new System.EventHandler(this.frmCustomers_Load);

            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLedger)).EndInit();
            this.ResumeLayout(false);
        }

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel pnlHeaderLine;
        private Panel pnlTopBar;
        private TableLayoutPanel tlpTopBar;
        private Panel pnlBranch;
        private Label lblBranch;
        private ComboBox cmbLocations;
        private FlowLayoutPanel flpActions;
        private Button btnImport;
        private Button btnPrint;
        private Button btnExport;
        private Button btnShare;
        private Panel pnlSearch;
        private TableLayoutPanel tlpSearch;
        private TextBox txtSearch;
        private ComboBox cmbRegionFilter;
        private Button btnRefresh;
        private Panel pnlEntry;
        private Label lblSectionTitle;
        private TableLayoutPanel tlpEntry;
        private TextBox txtCustomerName;
        private TextBox txtPhone;
        private TextBox txtRegion;
        private TextBox txtBalance;
        private TextBox txtNotes;
        private TableLayoutPanel tlpButtons;
        private Button btnSave;
        private Button btnEdit;
        private Button btnClear;
        private Button btnDelete;
        private Panel pnlDebt;
        private TableLayoutPanel tlpDebt;
        private Label lblPayTitle;
        private TextBox txtPaymentAmount;
        private ComboBox cmbPayMethod;
        private Button btnPayDebt;
        private SplitContainer splitContainer1;
        private Label lblCustomersList;
        private Label lblLedger;
        private DataGridView dgvCustomers;
        private DataGridView dgvLedger;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colPhone;
        private DataGridViewTextBoxColumn colRegion;
        private DataGridViewTextBoxColumn colNotes;
        private DataGridViewTextBoxColumn colBalance;
        private DataGridViewTextBoxColumn colRefId;
        private DataGridViewTextBoxColumn colTransType;
        private DataGridViewTextBoxColumn colTransDate;
        private DataGridViewTextBoxColumn colDesc;
        private DataGridViewTextBoxColumn colDebit;
        private DataGridViewTextBoxColumn colCredit;
        private DataGridViewTextBoxColumn colRunBalance;
    }
}