using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using MajesticDesktop.Services;

namespace MajesticDesktop.Forms
{
    public partial class uc_Dashboard : Form
    {
        private DashboardService _dashboardService;

        private static readonly Color CardBack = Color.FromArgb(30, 41, 59);

        public uc_Dashboard()
        {
            InitializeComponent();

            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            _dashboardService = new DashboardService();

            SetupAllCards();
            SetupAllGrids();
            StyleCards();

            dgvShortages.Columns.Clear();
            dgvRequests.Columns.Clear();
            dgvRecentSales.Columns.Clear();

            LoadData();
            refreshTimer.Start();
        }

        // ============================ إعدادات UI المخصصة ============================
        private void SetupAllCards()
        {
            SetupCard(this.pnlCardSales, this.lblCardSalesTitle, this.lblSalesValue, "pnlCardSales", "مبيعات اليوم");
            SetupCard(this.pnlCardProfit, this.lblCardProfitTitle, this.lblProfitValue, "pnlCardProfit", "صافي الربح اليوم");
            SetupCard(this.pnlCardExpenses, this.lblCardExpensesTitle, this.lblExpensesValue, "pnlCardExpenses", "مصروفات اليوم (-)");
            SetupCard(this.pnlCardProducts, this.lblCardProductsTitle, this.lblProductsValue, "pnlCardProducts", "إجمالي المنتجات");
            SetupCard(this.pnlCardRequests, this.lblCardRequestsTitle, this.lblRequestsValue, "pnlCardRequests", "طلبات الفروع المعلّقة");
            SetupCard(this.pnlCardAvgInvoice, this.lblCardAvgInvoiceTitle, this.lblAvgInvoiceValue, "pnlCardAvgInvoice", "متوسط الفاتورة اليوم");
        }

        private void SetupAllGrids()
        {
            SetupGrid(this.dgvShortages);
            SetupGrid(this.dgvRequests);
            SetupGrid(this.dgvRecentSales);
        }

        private void SetupCard(Panel card, Label title, Label value, string cardName, string titleText)
        {
            card.Controls.Add(value);
            card.Controls.Add(title);
            card.BackColor = Color.FromArgb(30, 41, 59);
            card.Dock = DockStyle.Fill;
            card.Margin = new Padding(8);
            card.Name = cardName;

            // هوامش ممتازة عشان توسط المحتوى جوا الكارت بعد ما كبرناه
            card.Padding = new Padding(0, 15, 0, 15);

            title.BackColor = Color.Transparent;
            title.Dock = DockStyle.Top;
            title.Font = new Font("Tahoma", 11.5F, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(148, 163, 184);
            title.Height = 40;
            title.Text = titleText;
            title.TextAlign = ContentAlignment.MiddleCenter;

            value.BackColor = Color.Transparent;
            value.Dock = DockStyle.Fill;
            value.Font = new Font("Tahoma", 20F, FontStyle.Bold);
            value.ForeColor = Color.White;
            value.Text = "0";
            value.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void SetupGrid(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.FromArgb(30, 41, 59);
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.Dock = DockStyle.Fill;
            dgv.EnableHeadersVisualStyles = false;
            dgv.GridColor = Color.FromArgb(51, 65, 85);
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.RightToLeft = RightToLeft.Yes;
            dgv.RowHeadersVisible = false;
            dgv.RowTemplate.Height = 36;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.DefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            dgv.DefaultCellStyle.ForeColor = Color.WhiteSmoke;
            dgv.DefaultCellStyle.Font = new Font("Tahoma", 10.5F, FontStyle.Regular);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 50, 71);
        }

        // ============================ التصميم ============================
        private void StyleCards()
        {
            StyleCardColor(pnlCardSales, Color.FromArgb(34, 197, 94));
            StyleCardColor(pnlCardProfit, Color.FromArgb(168, 85, 247));
            StyleCardColor(pnlCardExpenses, Color.FromArgb(239, 68, 68));
            StyleCardColor(pnlCardProducts, Color.FromArgb(59, 130, 246));
            StyleCardColor(pnlCardRequests, Color.FromArgb(245, 158, 11));
            StyleCardColor(pnlCardAvgInvoice, Color.FromArgb(20, 184, 166));
        }

        private void StyleCardColor(Panel card, Color accent)
        {
            card.Tag = accent;
            card.Paint += Card_Paint;
            card.Resize += Card_Resize;
            ApplyRoundedRegion(card, 12);

            foreach (Control c in card.Controls)
            {
                Label value = c as Label;
                if (value != null && value.Dock == DockStyle.Fill)
                {
                    value.ForeColor = accent;
                }
            }
        }

        private void Card_Paint(object sender, PaintEventArgs e)
        {
            Panel card = (Panel)sender;
            using (SolidBrush brush = new SolidBrush((Color)card.Tag))
            {
                e.Graphics.FillRectangle(brush, 0, 0, card.Width, 4);
            }
        }

        private void Card_Resize(object sender, EventArgs e)
        {
            Control card = (Control)sender;
            ApplyRoundedRegion(card, 12);
            card.Invalidate();
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0) return;

            int d = radius * 2;
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(0, 0, d, d, 180, 90);
                path.AddArc(control.Width - d, 0, d, d, 270, 90);
                path.AddArc(control.Width - d, control.Height - d, d, d, 0, 90);
                path.AddArc(0, control.Height - d, d, d, 90, 90);
                path.CloseFigure();

                Region old = control.Region;
                control.Region = new Region(path);
                if (old != null) old.Dispose();
            }
        }

        // ============================ البيانات ============================
        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                int selShortage = dgvShortages.SelectedRows.Count > 0 ? dgvShortages.SelectedRows[0].Index : -1;
                int selRequest = dgvRequests.SelectedRows.Count > 0 ? dgvRequests.SelectedRows[0].Index : -1;

                DashboardData data = _dashboardService.GetDashboardData();

                lblSalesValue.Text = data.TodaySales.ToString("N2");
                lblProfitValue.Text = data.TodayProfit.ToString("N2");
                lblExpensesValue.Text = data.TodayExpenses.ToString("N2");
                lblProductsValue.Text = data.TotalProducts.ToString();
                lblRequestsValue.Text = data.PendingRequestsCount.ToString();
                lblAvgInvoiceValue.Text = data.TodayAverageInvoice.ToString("N2");

                dgvShortages.DataSource = data.Shortages;
                if (dgvShortages.Columns.Contains("RowNum"))
                {
                    dgvShortages.Columns["RowNum"].HeaderText = "م";
                    dgvShortages.Columns["RowNum"].Width = 40;
                    dgvShortages.Columns["ProductName"].HeaderText = "اسم المنتج";
                    dgvShortages.Columns["CurrentQty"].HeaderText = "الرصيد";
                    dgvShortages.Columns["AlertLimit"].HeaderText = "الحد الأدنى";
                }

                dgvRequests.DataSource = data.RecentRequests;
                if (dgvRequests.Columns.Contains("RowNum"))
                {
                    dgvRequests.Columns["RowNum"].HeaderText = "م";
                    dgvRequests.Columns["RowNum"].Width = 40;
                    dgvRequests.Columns["BranchName"].HeaderText = "اسم الفرع";
                    dgvRequests.Columns["RequestDate"].HeaderText = "التاريخ";
                }

                dgvRecentSales.DataSource = data.RecentSales;
                if (dgvRecentSales.Columns.Contains("RowNum"))
                {
                    dgvRecentSales.Columns["RowNum"].HeaderText = "م";
                    dgvRecentSales.Columns["RowNum"].Width = 40;
                    dgvRecentSales.Columns["Time"].HeaderText = "الوقت";
                    dgvRecentSales.Columns["Amount"].HeaderText = "المبلغ";
                }

                if (selShortage >= 0 && selShortage < dgvShortages.Rows.Count) dgvShortages.Rows[selShortage].Selected = true;
                if (selRequest >= 0 && selRequest < dgvRequests.Rows.Count) dgvRequests.Rows[selRequest].Selected = true;

                lblLastUpdate.Text = "آخر تحديث: " + DateTime.Now.ToString("hh:mm:ss tt", new CultureInfo("ar-EG"));
                lblLastUpdate.ForeColor = Color.FromArgb(148, 163, 184);
            }
            catch
            {
                lblLastUpdate.Text = "⚠ فشل تحديث بيانات الداشبورد";
                lblLastUpdate.ForeColor = Color.FromArgb(239, 68, 68);
            }
        }

        private void dgvShortages_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvShortages.Columns.Contains("CurrentQty"))
            {
                decimal qty;
                decimal.TryParse(Convert.ToString(dgvShortages.Rows[e.RowIndex].Cells["CurrentQty"].Value), out qty);

                dgvShortages.Rows[e.RowIndex].DefaultCellStyle.ForeColor = qty <= 0
                    ? Color.FromArgb(248, 113, 113)
                    : Color.FromArgb(250, 204, 21);
            }
        }

        // ============================ الأزرار ============================
        private void btnNewSale_Click(object sender, EventArgs e)
        {
            try
            {
                frmSalesInvoice frmSales = new frmSalesInvoice();
                frmSales.StartPosition = FormStartPosition.CenterScreen;
                frmSales.WindowState = FormWindowState.Maximized;
                frmSales.ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnSupply_Click(object sender, EventArgs e)
        {
            try
            {
                frmIssueStock frmIssue = new frmIssueStock();
                frmIssue.StartPosition = FormStartPosition.CenterScreen;
                frmIssue.ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnManageRequests_Click(object sender, EventArgs e)
        {
            try
            {
                frmManageRequests frmReq = new frmManageRequests();
                frmReq.StartPosition = FormStartPosition.CenterScreen;
                frmReq.ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (refreshTimer != null)
            {
                refreshTimer.Stop();
            }
            base.OnFormClosed(e);
        }
    }
}