using System.Drawing;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    partial class frmReports
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        // ---------- دوال مساعدة للتنسيق ----------
        private void StyleTab(Button b, string text)
        {
            b.Dock = DockStyle.Fill;
            b.Margin = new Padding(0);
            b.BackColor = BranchTheme.Surface;
            b.ForeColor = BranchTheme.Text;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = BranchTheme.UiFont(13F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            b.Text = text;
            b.UseVisualStyleBackColor = false;
            b.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void StyleAction(Button b, Color back, string text, int width)
        {
            b.BackColor = back;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = BranchTheme.UiFont(11F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            b.Size = new Size(width, 44);
            b.Margin = new Padding(8, 0, 0, 0);
            b.Text = text;
            b.UseVisualStyleBackColor = false;
            b.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void StyleGroup(Panel grp, Label lbl, Control input, string text, int width)
        {
            grp.Size = new Size(width, 60);
            grp.Margin = new Padding(0, 4, 14, 0);

            lbl.Dock = DockStyle.Top;
            lbl.Height = 24;
            lbl.Text = text;
            lbl.ForeColor = BranchTheme.Text;
            lbl.Font = BranchTheme.UiFont(10.5F, FontStyle.Bold);
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            input.Dock = DockStyle.Top;
            input.Font = BranchTheme.UiFont(11F);

            grp.Controls.Add(input);
            grp.Controls.Add(lbl);
        }

        private void StyleCombo(ComboBox c)
        {
            c.DropDownStyle = ComboBoxStyle.DropDownList;
            c.FlatStyle = FlatStyle.Flat;
            c.BackColor = BranchTheme.Input;
            c.ForeColor = BranchTheme.Text;
        }

        private void InitializeComponent()
        {
            this.pnlTopNav = new Panel();
            this.tlpNav = new TableLayoutPanel();
            this.pnlNavLine = new Panel();
            this.btnCustomers = new Button();
            this.btnSuppliers = new Button();
            this.btnInventory = new Button();
            this.btnProfit = new Button();

            this.pnlFilters = new Panel();
            this.tlpFilters = new TableLayoutPanel();
            this.flpFilters = new FlowLayoutPanel();
            this.flpActions = new FlowLayoutPanel();
            this.pnlGrpLocation = new Panel();
            this.pnlGrpRegion = new Panel();
            this.pnlGrpProduct = new Panel();
            this.pnlGrpFrom = new Panel();
            this.pnlGrpTo = new Panel();
            this.lblLocation = new Label();
            this.lblRegion = new Label();
            this.lblProduct = new Label();
            this.lblFrom = new Label();
            this.lblTo = new Label();
            this.cmbLocation = new ComboBox();
            this.cmbRegion = new ComboBox();
            this.cmbProduct = new ComboBox();
            this.dtpFrom = new DateTimePicker();
            this.dtpTo = new DateTimePicker();
            this.btnSearch = new Button();
            this.btnColumns = new Button();
            this.btnExport = new Button();
            this.btnPrint = new Button();

            this.pnlFooter = new Panel();
            this.tlpFooter = new TableLayoutPanel();
            this.lblRecords = new Label();
            this.lblHint = new Label();
            this.lblTotal = new Label();

            this.dgvReport = new DataGridView();

            this.pnlTopNav.SuspendLayout();
            this.tlpNav.SuspendLayout();
            this.pnlFilters.SuspendLayout();
            this.tlpFilters.SuspendLayout();
            this.flpFilters.SuspendLayout();
            this.flpActions.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.tlpFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();

            // ===== التبويبات (بتاخد عرض الشاشة كله) =====
            this.pnlTopNav.BackColor = BranchTheme.Header;
            this.pnlTopNav.Dock = DockStyle.Top;
            this.pnlTopNav.Height = 68;
            this.pnlTopNav.Controls.Add(this.tlpNav);
            this.pnlTopNav.Controls.Add(this.pnlNavLine);

            this.pnlNavLine.BackColor = BranchTheme.Accent;
            this.pnlNavLine.Dock = DockStyle.Bottom;
            this.pnlNavLine.Height = 3;

            this.tlpNav.Dock = DockStyle.Fill;
            this.tlpNav.Margin = new Padding(0);
            this.tlpNav.Padding = new Padding(0);
            this.tlpNav.ColumnCount = 4;
            this.tlpNav.RowCount = 1;
            for (int i = 0; i < 4; i++)
                this.tlpNav.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.tlpNav.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tlpNav.Controls.Add(this.btnCustomers, 0, 0);
            this.tlpNav.Controls.Add(this.btnSuppliers, 1, 0);
            this.tlpNav.Controls.Add(this.btnInventory, 2, 0);
            this.tlpNav.Controls.Add(this.btnProfit, 3, 0);

            StyleTab(this.btnCustomers, "👥  تقارير العملاء");
            StyleTab(this.btnSuppliers, "🤝  تقارير الموردين");
            StyleTab(this.btnInventory, "📦  جرد المخازن");
            StyleTab(this.btnProfit, "💰  ربحية المنتجات");
            this.btnCustomers.Click += new System.EventHandler(this.btnCustomers_Click);
            this.btnSuppliers.Click += new System.EventHandler(this.btnSuppliers_Click);
            this.btnInventory.Click += new System.EventHandler(this.btnInventory_Click);
            this.btnProfit.Click += new System.EventHandler(this.btnProfit_Click);

            // ===== الفلاتر + الأزرار =====
            this.pnlFilters.BackColor = BranchTheme.Surface;
            this.pnlFilters.Dock = DockStyle.Top;
            this.pnlFilters.Height = 96;
            this.pnlFilters.Padding = new Padding(16, 12, 16, 12);
            this.pnlFilters.Controls.Add(this.tlpFilters);

            this.tlpFilters.Dock = DockStyle.Fill;
            this.tlpFilters.ColumnCount = 2;
            this.tlpFilters.RowCount = 1;
            this.tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tlpFilters.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.tlpFilters.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tlpFilters.Controls.Add(this.flpFilters, 0, 0);   // يمين
            this.tlpFilters.Controls.Add(this.flpActions, 1, 0);   // شمال

            this.flpFilters.Dock = DockStyle.Fill;
            this.flpFilters.Margin = new Padding(0);
            this.flpFilters.WrapContents = false;
            this.flpFilters.FlowDirection = FlowDirection.LeftToRight; // في RTL بيبدأ من اليمين
            this.flpFilters.Controls.Add(this.pnlGrpLocation);
            this.flpFilters.Controls.Add(this.pnlGrpRegion);
            this.flpFilters.Controls.Add(this.pnlGrpProduct);
            this.flpFilters.Controls.Add(this.pnlGrpFrom);
            this.flpFilters.Controls.Add(this.pnlGrpTo);

            this.flpActions.AutoSize = true;
            this.flpActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.flpActions.Anchor = AnchorStyles.None;
            this.flpActions.Margin = new Padding(0);
            this.flpActions.WrapContents = false;
            this.flpActions.FlowDirection = FlowDirection.LeftToRight;
            this.flpActions.Controls.Add(this.btnSearch);
            this.flpActions.Controls.Add(this.btnColumns);
            this.flpActions.Controls.Add(this.btnExport);
            this.flpActions.Controls.Add(this.btnPrint);

            StyleCombo(this.cmbLocation);
            StyleCombo(this.cmbRegion);
            StyleCombo(this.cmbProduct);
            StyleGroup(this.pnlGrpLocation, this.lblLocation, this.cmbLocation, "الفرع", 200);
            StyleGroup(this.pnlGrpRegion, this.lblRegion, this.cmbRegion, "المنطقة", 170);
            StyleGroup(this.pnlGrpProduct, this.lblProduct, this.cmbProduct, "المنتج", 230);

            this.dtpFrom.Format = DateTimePickerFormat.Short;
            this.dtpTo.Format = DateTimePickerFormat.Short;
            this.dtpFrom.CalendarMonthBackground = BranchTheme.Surface;
            this.dtpFrom.CalendarForeColor = BranchTheme.Text;
            this.dtpTo.CalendarMonthBackground = BranchTheme.Surface;
            this.dtpTo.CalendarForeColor = BranchTheme.Text;
            StyleGroup(this.pnlGrpFrom, this.lblFrom, this.dtpFrom, "من تاريخ", 150);
            StyleGroup(this.pnlGrpTo, this.lblTo, this.dtpTo, "إلى تاريخ", 150);

            this.cmbLocation.SelectionChangeCommitted += new System.EventHandler(this.Filter_Changed);
            this.cmbRegion.SelectionChangeCommitted += new System.EventHandler(this.Filter_Changed);
            this.cmbProduct.SelectionChangeCommitted += new System.EventHandler(this.Filter_Changed);
            this.dtpFrom.CloseUp += new System.EventHandler(this.Filter_Changed);
            this.dtpTo.CloseUp += new System.EventHandler(this.Filter_Changed);

            StyleAction(this.btnSearch, BranchTheme.Accent, "🔄  تحديث", 110);
            StyleAction(this.btnColumns, BranchTheme.Purple, "⚙  الأعمدة", 120);
            StyleAction(this.btnExport, BranchTheme.Orange, "📥  تصدير Excel", 150);
            StyleAction(this.btnPrint, BranchTheme.Green, "🖨  طباعة", 110);
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.btnColumns.Click += new System.EventHandler(this.btnColumns_Click);
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);

            // ===== الشريط السفلي =====
            this.pnlFooter.BackColor = BranchTheme.Surface;
            this.pnlFooter.Dock = DockStyle.Bottom;
            this.pnlFooter.Height = 46;
            this.pnlFooter.Controls.Add(this.tlpFooter);

            this.tlpFooter.Dock = DockStyle.Fill;
            this.tlpFooter.ColumnCount = 3;
            this.tlpFooter.RowCount = 1;
            this.tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            this.tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            this.tlpFooter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tlpFooter.Controls.Add(this.lblRecords, 0, 0);
            this.tlpFooter.Controls.Add(this.lblHint, 1, 0);
            this.tlpFooter.Controls.Add(this.lblTotal, 2, 0);

            foreach (Label l in new[] { this.lblRecords, this.lblHint, this.lblTotal })
            {
                l.Dock = DockStyle.Fill;
                l.TextAlign = ContentAlignment.MiddleCenter;
                l.ForeColor = BranchTheme.Text;
                l.Margin = new Padding(0);
            }
            this.lblRecords.Font = BranchTheme.UiFont(11F, FontStyle.Bold);
            this.lblHint.Font = BranchTheme.UiFont(10F);
            this.lblTotal.Font = BranchTheme.UiFont(12F, FontStyle.Bold);

            // ===== الجدول =====
            this.dgvReport.Dock = DockStyle.Fill;
            this.dgvReport.CellDoubleClick += new DataGridViewCellEventHandler(this.dgvReport_CellDoubleClick);

            // ===== الفورم =====
            this.BackColor = BranchTheme.Bg;
            this.ClientSize = new Size(1250, 800);
            this.MinimumSize = new Size(1100, 600);
            this.Controls.Add(this.dgvReport);   // الـ Fill الأول
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.pnlTopNav);
            this.Font = BranchTheme.UiFont(10F);
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = false;      // مهم: بدون عكس الإحداثيات
            this.Text = "التقارير الشاملة والميزانيات";
            this.WindowState = FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmReports_Load);

            this.tlpNav.ResumeLayout(false);
            this.pnlTopNav.ResumeLayout(false);
            this.flpFilters.ResumeLayout(false);
            this.flpActions.ResumeLayout(false);
            this.tlpFilters.ResumeLayout(false);
            this.tlpFilters.PerformLayout();
            this.pnlFilters.ResumeLayout(false);
            this.tlpFooter.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);
        }

        private Panel pnlTopNav;
        private TableLayoutPanel tlpNav;
        private Panel pnlNavLine;
        private Button btnCustomers;
        private Button btnSuppliers;
        private Button btnInventory;
        private Button btnProfit;
        private Panel pnlFilters;
        private TableLayoutPanel tlpFilters;
        private FlowLayoutPanel flpFilters;
        private FlowLayoutPanel flpActions;
        private Panel pnlGrpLocation;
        private Panel pnlGrpRegion;
        private Panel pnlGrpProduct;
        private Panel pnlGrpFrom;
        private Panel pnlGrpTo;
        private Label lblLocation;
        private Label lblRegion;
        private Label lblProduct;
        private Label lblFrom;
        private Label lblTo;
        private ComboBox cmbLocation;
        private ComboBox cmbRegion;
        private ComboBox cmbProduct;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private Button btnSearch;
        private Button btnColumns;
        private Button btnExport;
        private Button btnPrint;
        private Panel pnlFooter;
        private TableLayoutPanel tlpFooter;
        private Label lblRecords;
        private Label lblHint;
        private Label lblTotal;
        private DataGridView dgvReport;
    }
}