using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MajesticDesktop.Services;

namespace MajesticDesktop.Forms
{
    public partial class frmCashRegister : Form
    {
        private CashRegisterService cashService;
        private DailyStats currentStats;
        private DataView _pendingView;
        private bool _busy;

        // ===================== ألوان وثيم الشاشة الموحد =====================
        private static readonly Color ClrBgDark = Color.FromArgb(15, 23, 42);
        private static readonly Color ClrBgPanel = Color.FromArgb(30, 41, 59);
        private static readonly Color ClrCard = Color.FromArgb(24, 34, 51);
        private static readonly Color ClrCardBorder = Color.FromArgb(51, 65, 85);
        private static readonly Color ClrText = Color.FromArgb(241, 245, 249);
        private static readonly Color ClrTextMuted = Color.FromArgb(148, 163, 184);

        private static readonly Color Positive = Color.FromArgb(34, 197, 94);
        private static readonly Color Negative = Color.FromArgb(239, 68, 68);
        private static readonly Color Gold = Color.FromArgb(250, 204, 21);
        private static readonly Color ProfitPurple = Color.FromArgb(168, 85, 247);
        private static readonly Color ClrBlue = Color.FromArgb(59, 130, 246);
        private static readonly Color ClrBlueHover = Color.FromArgb(37, 99, 235);
        private static readonly Color ClrOrange = Color.FromArgb(245, 158, 11);

        private static readonly Color ClrGridHeader = Color.FromArgb(20, 30, 45);
        private static readonly Color ClrGridHeaderText = Color.White;

        // ===================== خطوط ضخمة وعصرية =====================
        private static readonly Font FontBase = new Font("Segoe UI", 12F, FontStyle.Regular);
        private static readonly Font FontTitle = new Font("Segoe UI", 26F, FontStyle.Bold);
        private static readonly Font FontHeaderBtn = new Font("Segoe UI", 16F, FontStyle.Bold);
        private static readonly Font FontCardTitle = new Font("Segoe UI", 14.5F, FontStyle.Bold);
        private static readonly Font FontCardValue = new Font("Segoe UI", 28F, FontStyle.Bold);
        private static readonly Font FontTableTitle = new Font("Segoe UI", 16F, FontStyle.Bold);
        private static readonly Font FontGridHeader = new Font("Segoe UI", 13.5F, FontStyle.Bold);
        private static readonly Font FontGridCell = new Font("Segoe UI", 12.5F, FontStyle.Bold);
        private static readonly Font FontNetTitle = new Font("Segoe UI", 18F, FontStyle.Bold);
        private static readonly Font FontNetValue = new Font("Segoe UI", 32F, FontStyle.Bold);
        private static readonly Font FontCloseBtn = new Font("Segoe UI", 20F, FontStyle.Bold);

        private static readonly CultureInfo ArEg = CreateArEg();

        private const decimal LargeDiffMin = 500m;
        private const decimal LargeDiffRatio = 0.20m;

        public frmCashRegister()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            this.Opacity = 0;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();

            BuildDynamicUI();
            EnableDoubleBuffering(dgvTransactions);

            this.Shown += frmCashRegister_Shown;
        }

        private static CultureInfo CreateArEg()
        {
            try { return new CultureInfo("ar-EG"); }
            catch (CultureNotFoundException) { return CultureInfo.CurrentCulture; }
        }

        private void EnableDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, dgv, new object[] { true });
        }

        private async void frmCashRegister_Shown(object sender, EventArgs e)
        {
            try { this.Refresh(); }
            finally { this.Opacity = 1; }

            SetBusy(true);
            lblTableTitle.Text = "جاري تهيئة الاتصال بقاعدة البيانات... ⏳";

            try
            {
                cashService = new CashRegisterService();
            }
            catch (Exception ex)
            {
                if (IsDisposed) return;
                SetBusy(false);
                lblTableTitle.Text = "تعذر تهيئة الاتصال ❌";
                Msg("تعذر فتح الشاشة:\n" + ex.Message, "خطأ", MessageBoxIcon.Error);
                return;
            }

            if (IsDisposed) return;
            SetBusy(false);
            await LoadDataAsync();
        }

        private async void frmCashRegister_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && !_busy)
            {
                e.Handled = true;
                await LoadDataAsync();
            }
        }

        #region Solid & Responsive UI Builder

        private void BuildDynamicUI()
        {
            this.SuspendLayout();

            this.Controls.Clear();
            this.BackColor = ClrBgDark;
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Font = FontBase;

            // ================= 1. Header =================
            pnlHeader.BackColor = Color.FromArgb(9, 14, 28);
            pnlHeader.Height = 90;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Padding = new Padding(24, 0, 24, 0);

            tlpHeader.ColumnCount = 3;
            tlpHeader.ColumnStyles.Clear();
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tlpHeader.RowCount = 1;
            tlpHeader.RowStyles.Clear();
            tlpHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpHeader.Dock = DockStyle.Fill;

            lblTitle.Font = FontTitle;
            lblTitle.ForeColor = Color.White;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Text = "يومية خزنة المخزن الرئيسي";

            btnRefresh.BackColor = ClrBlue;
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Font = FontHeaderBtn;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatAppearance.MouseOverBackColor = ClrBlueHover;
            btnRefresh.Cursor = Cursors.Hand;
            btnRefresh.Dock = DockStyle.Fill;
            btnRefresh.Margin = new Padding(0, 18, 0, 18);
            btnRefresh.Text = "تحديث 🔄 (F5)";
            btnRefresh.Click += async (s, e) => await LoadDataAsync();

            tlpHeader.Controls.Clear();
            tlpHeader.Controls.Add(lblTitle, 1, 0);
            tlpHeader.Controls.Add(btnRefresh, 2, 0);
            pnlHeader.Controls.Add(tlpHeader);

            // ================= 2. Stats Cards (4x2) =================
            tlpCards.Height = 280;
            tlpCards.Dock = DockStyle.Top;
            tlpCards.Padding = new Padding(15, 15, 15, 5);
            tlpCards.ColumnCount = 4;
            tlpCards.ColumnStyles.Clear();
            for (int i = 0; i < 4; i++) tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpCards.RowCount = 2;
            tlpCards.RowStyles.Clear();
            tlpCards.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpCards.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            tlpCards.Controls.Clear();
            tlpCards.Controls.Add(CreateDashboardCard("إجمالي المبيعات (كاش)", Positive, out lblMainSales), 0, 0);
            tlpCards.Controls.Add(CreateDashboardCard("إنستاباي الكردي", Color.FromArgb(20, 184, 166), out lblInstaKurdi), 1, 0);
            tlpCards.Controls.Add(CreateDashboardCard("إنستاباي الحكش", ProfitPurple, out lblInstaHakosh), 2, 0);
            tlpCards.Controls.Add(CreateDashboardCard("توريدات الفروع (بانتظار الترحيل)", ClrBlue, out lblBranchReceipts), 3, 0);

            tlpCards.Controls.Add(CreateDashboardCard("مرتجعات المبيعات (-)", Color.FromArgb(236, 72, 153), out lblReturns), 0, 1);
            tlpCards.Controls.Add(CreateDashboardCard("مصروفات المركز (-)", Negative, out lblMainExpenses), 1, 1);
            tlpCards.Controls.Add(CreateDashboardCard("سداد مشتريات بضاعة (-)", ClrOrange, out lblPurchases), 2, 1);
            tlpCards.Controls.Add(CreateDashboardCard("صافي الربح 🎯", Gold, out lblGrossProfit), 3, 1);

            // ================= 3. Table Title =================
            Panel pnlSpacer = new Panel { Dock = DockStyle.Top, Height = 10, BackColor = ClrBgDark };

            lblTableTitle.Dock = DockStyle.Top;
            lblTableTitle.Height = 45;
            lblTableTitle.Font = FontTableTitle;
            lblTableTitle.ForeColor = Color.FromArgb(226, 232, 240);
            lblTableTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTableTitle.Text = "الحركات المفتوحة بالخزنة (لم يتم تقفيلها بعد)";

            // ================= 4. Grid Setup =================
            pnlGridHost.Dock = DockStyle.Fill;
            pnlGridHost.Padding = new Padding(20, 0, 20, 15);
            pnlGridHost.BackColor = ClrBgDark;

            dgvTransactions.AutoGenerateColumns = false;
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.Dock = DockStyle.Fill;

            // التعديل هنا: إرجاع الترتيب الأصلي ليعمل بشكل سليم مع الـ RTL
            // لأن أول عمود يتم إضافته في الـ DataGridView والـ RTL = Yes، سيظهر أقصى اليمين.
            colDate.HeaderText = "التاريخ والوقت"; colDate.Name = "colDate"; colDate.DataPropertyName = "TransactionDate"; colDate.FillWeight = 18F; colDate.DefaultCellStyle.Format = "yyyy/MM/dd   hh:mm tt"; colDate.DefaultCellStyle.FormatProvider = ArEg; colDate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colLocation.HeaderText = "جهة الحركة"; colLocation.Name = "colLocation"; colLocation.DataPropertyName = "LocationName"; colLocation.FillWeight = 15F; colLocation.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colType.HeaderText = "نوع الحركة"; colType.Name = "colType"; colType.DataPropertyName = "TransactionType"; colType.FillWeight = 12F; colType.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colAmount.HeaderText = "المبلغ"; colAmount.Name = "colAmount"; colAmount.DataPropertyName = "Amount"; colAmount.FillWeight = 15F; colAmount.DefaultCellStyle.Format = "N2"; colAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            // البيان سيكون الأخير (يعني أقصى اليسار)، وبمحاذاة لليمين ليبدو منسقاً
            colDesc.HeaderText = "البيان والتفاصيل"; colDesc.Name = "colDesc"; colDesc.DataPropertyName = "Description"; colDesc.FillWeight = 40F; colDesc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvTransactions.Columns.Clear();
            // الترتيب الصحيح: التاريخ يمين ثم الجهة، النوع، المبلغ، والبيان يسار
            dgvTransactions.Columns.AddRange(new DataGridViewColumn[] { colDate, colLocation, colType, colAmount, colDesc });

            ApplyGridStyle();
            dgvTransactions.CellFormatting += dgvTransactions_CellFormatting;

            pnlGridHost.Controls.Clear();
            pnlGridHost.Controls.Add(dgvTransactions);

            // ================= 5. Footer (Close Day - الجنب) =================
            pnlFooter.BackColor = Color.FromArgb(9, 14, 28);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 100;
            pnlFooter.Padding = new Padding(20);

            // التعديل الجوهري لحل مشكلة الرصيد اللازق في الزرار
            tlpFooter.Dock = DockStyle.Fill;
            tlpFooter.ColumnCount = 4; // 4 أعمدة محددة الوظيفة
            tlpFooter.ColumnStyles.Clear();
            tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // العمود الأول: النص (ياخد مساحته بس)
            tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // العمود الثاني: الرقم (ياخد مساحته بس ويفضل جنب النص)
            tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F)); // العمود الثالث: مسافة فارغة (تزق الزرار لأقصى اليسار)
            tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 350F)); // العمود الرابع: الزرار

            tlpFooter.RowCount = 1;
            tlpFooter.RowStyles.Clear();
            tlpFooter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpFooter.RightToLeft = RightToLeft.Yes;

            lblNetCashTitle.Font = FontNetTitle;
            lblNetCashTitle.ForeColor = ClrTextMuted;
            lblNetCashTitle.Dock = DockStyle.Fill;
            lblNetCashTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblNetCashTitle.Text = "صافي النقدية بالخزنة الآن: ";
            lblNetCashTitle.AutoSize = true;

            lblNetCash.Font = FontNetValue;
            lblNetCash.ForeColor = Gold;
            lblNetCash.Dock = DockStyle.Fill;
            lblNetCash.TextAlign = ContentAlignment.MiddleLeft;
            lblNetCash.Text = "0.00 ج.م";
            lblNetCash.AutoSize = true;

            btnCloseDay.Dock = DockStyle.Fill;
            btnCloseDay.BackColor = Negative;
            btnCloseDay.ForeColor = Color.White;
            btnCloseDay.Font = FontCloseBtn;
            btnCloseDay.FlatStyle = FlatStyle.Flat;
            btnCloseDay.FlatAppearance.BorderSize = 0;
            btnCloseDay.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 38, 38);
            btnCloseDay.Cursor = Cursors.Hand;
            btnCloseDay.Text = "🔒 جرد وتقفيل الوردية";
            btnCloseDay.Click += async (s, e) => await btnCloseDay_Click();

            tlpFooter.Controls.Clear();
            tlpFooter.Controls.Add(lblNetCashTitle, 0, 0); // النص يمين
            tlpFooter.Controls.Add(lblNetCash, 1, 0);     // الرقم جنبه مباشرة
            // العمود رقم 2 هيبقى فاضي تماماً
            tlpFooter.Controls.Add(btnCloseDay, 3, 0);    // الزرار أقصى اليسار

            pnlFooter.Controls.Add(tlpFooter);

            // ================= 6. Z-Order Layout =================
            this.Controls.Add(pnlGridHost);
            this.Controls.Add(pnlSpacer);
            this.Controls.Add(lblTableTitle);
            this.Controls.Add(tlpCards);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlFooter);

            this.ResumeLayout(true);
        }

        private Panel CreateDashboardCard(string title, Color accentColor, out Label valueLabel)
        {
            Panel card = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(8),
                BackColor = ClrCard,
                Tag = accentColor,
                Padding = new Padding(5, 5, 5, 5)
            };

            card.Paint += (s, e) =>
            {
                using (Pen p = new Pen(ClrCardBorder)) { e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1); }
                using (SolidBrush b = new SolidBrush(accentColor)) { e.Graphics.FillRectangle(b, 0, 0, card.Width, 6); }
            };

            Label lblCardTitle = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 30,
                Font = FontCardTitle,
                ForeColor = ClrTextMuted,
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.Transparent,
                AutoSize = false
            };

            valueLabel = new Label
            {
                Text = "0.00 ج.م",
                Dock = DockStyle.Fill,
                Font = FontCardValue,
                ForeColor = accentColor,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                AutoSize = false
            };

            card.Controls.Add(valueLabel); card.Controls.Add(lblCardTitle);
            return card;
        }

        private void ApplyGridStyle()
        {
            dgvTransactions.BackgroundColor = ClrBgDark;
            dgvTransactions.BorderStyle = BorderStyle.None;
            dgvTransactions.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTransactions.GridColor = Color.FromArgb(51, 65, 85);

            dgvTransactions.EnableHeadersVisualStyles = false;
            dgvTransactions.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvTransactions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvTransactions.ColumnHeadersDefaultCellStyle.BackColor = ClrGridHeader;
            dgvTransactions.ColumnHeadersDefaultCellStyle.ForeColor = ClrGridHeaderText;
            dgvTransactions.ColumnHeadersDefaultCellStyle.SelectionBackColor = ClrGridHeader;
            dgvTransactions.ColumnHeadersDefaultCellStyle.SelectionForeColor = ClrGridHeaderText;
            dgvTransactions.ColumnHeadersDefaultCellStyle.Font = FontGridHeader;
            dgvTransactions.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTransactions.ColumnHeadersHeight = 50;

            foreach (DataGridViewColumn col in dgvTransactions.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dgvTransactions.DefaultCellStyle.BackColor = ClrBgPanel;
            dgvTransactions.DefaultCellStyle.ForeColor = ClrText;
            dgvTransactions.DefaultCellStyle.SelectionBackColor = ClrBlueHover;
            dgvTransactions.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvTransactions.DefaultCellStyle.Font = FontGridCell;

            dgvTransactions.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 50, 71);
            dgvTransactions.RowTemplate.Height = 45;
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransactions.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            dgvTransactions.ReadOnly = true; dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.MultiSelect = false; dgvTransactions.AllowUserToResizeRows = false;
            dgvTransactions.AllowUserToDeleteRows = false; dgvTransactions.AllowUserToOrderColumns = false;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.RightToLeft = RightToLeft.Yes;
        }

        #endregion

        private DialogResult Msg(string text, string caption, MessageBoxIcon icon, IWin32Window owner = null)
        {
            return MessageBox.Show(owner ?? this, text, caption, MessageBoxButtons.OK, icon,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private DialogResult Ask(string text, string caption, IWin32Window owner)
        {
            return MessageBox.Show(owner, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void SetBusy(bool busy)
        {
            _busy = busy;
            btnRefresh.Enabled = !busy;
            btnCloseDay.Enabled = !busy;
            UseWaitCursor = busy;
        }

        private static string Money(decimal value) { return value.ToString("N2") + " ج.م"; }
        private static bool IsIncome(string type) { return type == "وارد" || type == "إيراد" || type == "مبيعات"; }

        private async Task LoadDataAsync()
        {
            if (cashService == null || _busy) return;

            SetBusy(true);
            lblTableTitle.Text = "جاري تحديث البيانات... 🔄";

            try
            {
                DailyStats loadedStats = await cashService.GetDailyStatsAsync();
                DataTable loadedPending = await cashService.GetPendingTransactionsAsync();

                if (IsDisposed) return;

                currentStats = loadedStats;

                lblMainSales.Text = Money(loadedStats.MainSales);
                lblInstaKurdi.Text = Money(loadedStats.InstaPayKurdi);
                lblInstaHakosh.Text = Money(loadedStats.InstaPayHakosh);
                lblBranchReceipts.Text = Money(loadedStats.PendingBranchReceipts);
                lblReturns.Text = Money(loadedStats.TotalReturns);
                lblMainExpenses.Text = Money(loadedStats.MainExpenses);
                lblPurchases.Text = Money(loadedStats.TotalPurchases);
                lblGrossProfit.Text = Money(loadedStats.NetProfit);

                lblGrossProfit.ForeColor = loadedStats.NetProfit < 0 ? Negative : Gold;

                if (loadedStats.NetCash < 0)
                {
                    lblNetCashTitle.Text = "عجز بالخزنة (يُرحَّل للوردية القادمة):";
                    lblNetCash.ForeColor = Negative;
                }
                else
                {
                    lblNetCashTitle.Text = "صافي النقدية بالخزنة الآن:";
                    lblNetCash.ForeColor = Gold;
                }
                lblNetCash.Text = Money(Math.Abs(loadedStats.NetCash));

                decimal totalIn = 0m, totalOut = 0m;
                foreach (DataRow row in loadedPending.Rows)
                {
                    decimal amount = Convert.ToDecimal(row["Amount"]);
                    if (IsIncome(Convert.ToString(row["TransactionType"]))) totalIn += amount; else totalOut += amount;
                }

                string title = "الحركات المفتوحة بالخزنة (لم يتم تقفيلها بعد) — " + loadedPending.Rows.Count
                             + " حركة | وارد: " + totalIn.ToString("N2") + " | منصرف: " + totalOut.ToString("N2");

                if (Math.Abs((totalIn - totalOut) - loadedStats.NetCash) > 0.009m)
                    title += "  ⚠ لا يطابق صافي الخزنة (اضغط تحديث)";

                lblTableTitle.Text = title;
                _pendingView = loadedPending.DefaultView;
                dgvTransactions.DataSource = _pendingView;

                if (dgvTransactions.Columns.Contains("colDate"))
                    dgvTransactions.Columns["colDate"].DefaultCellStyle.FormatProvider = ArEg;
                dgvTransactions.ClearSelection();
            }
            catch (Exception ex)
            {
                if (!IsDisposed)
                {
                    lblTableTitle.Text = "فشل التحديث ❌";
                    Msg("حدث خطأ أثناء تحميل البيانات:\n" + ex.Message, "خطأ", MessageBoxIcon.Error);
                }
            }
            finally
            {
                if (!IsDisposed) SetBusy(false);
            }
        }

        private void dgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || _pendingView == null || e.RowIndex >= _pendingView.Count) return;
            int c = e.ColumnIndex;
            if (c != colType.Index && c != colAmount.Index) return;

            string type = Convert.ToString(_pendingView[e.RowIndex]["TransactionType"]);
            e.CellStyle.ForeColor = IsIncome(type) ? Positive : Negative;
            if (c == colAmount.Index) e.CellStyle.Font = FontGridCell;
        }

        // ============================ تقفيل الوردية ============================

        private Label MakeLabel(string text, float size, Color color)
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                Text = text,
                ForeColor = color,
                Font = new Font("Segoe UI", size, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0)
            };
        }

        private static bool TryParseAmount(string text, out decimal value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(text)) return false;

            StringBuilder sb = new StringBuilder();
            foreach (char ch in text.Trim())
            {
                if (ch >= '\u0660' && ch <= '\u0669') sb.Append((char)('0' + (ch - '\u0660')));
                else if (ch >= '\u06F0' && ch <= '\u06F9') sb.Append((char)('0' + (ch - '\u06F0')));
                else if (ch == '\u066B') sb.Append('.');
                else if (ch == '\u066C' || ch == ',') continue;
                else sb.Append(ch);
            }
            return decimal.TryParse(sb.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }

        private async Task btnCloseDay_Click()
        {
            if (_busy || currentStats == null) return;

            if (dgvTransactions.Rows.Count == 0)
            {
                Msg("لا توجد حركات مالية مفتوحة لتقفيلها بالمخزن الرئيسي.", "تنبيه", MessageBoxIcon.Information);
                return;
            }

            bool inDeficit = currentStats.NetCash < 0;
            decimal net = currentStats.NetCash;

            await ShowCloseShiftDialogAsync(net, inDeficit);
            await LoadDataAsync();
        }

        private async Task ShowCloseShiftDialogAsync(decimal expectedNetCash, bool inDeficit)
        {
            using (Form prompt = new Form())
            {
                prompt.Text = "جرد الخزنة وإغلاق الوردية";
                prompt.ClientSize = new Size(560, inDeficit ? 340 : 370);
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.RightToLeft = RightToLeft.Yes;
                prompt.RightToLeftLayout = false;
                prompt.BackColor = Color.FromArgb(18, 25, 38);
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.MaximizeBox = false; prompt.MinimizeBox = false; prompt.ShowInTaskbar = false;

                TableLayoutPanel tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, Padding = new Padding(30, 20, 30, 20), BackColor = Color.Transparent };
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

                Label lblExpectedTitle = MakeLabel(inDeficit ? "الخزنة في عجز حاليًا:" : "الرصيد الدفتري المتوقع بالدرج الآن:", 14F, ClrTextMuted);
                Label lblExpectedValue = MakeLabel(Money(Math.Abs(expectedNetCash)), 28F, inDeficit ? Negative : Gold);
                Label lblInstruction = MakeLabel(
                    inDeficit ? "لا يمكن ترحيل مبلغ من خزنة في عجز.\nسيبقى العجز ظاهرًا في الخزنة ويُرحَّل للوردية القادمة كما هو."
                              : "أدخل المبلغ الذي سيتم ترحيله فعليًا الآن من الدرج:", 13F, Color.White);

                TextBox txtActual = new TextBox
                {
                    Anchor = AnchorStyles.Left | AnchorStyles.Right,
                    Font = new Font("Segoe UI", 24F, FontStyle.Bold),
                    BackColor = Color.FromArgb(38, 50, 71),
                    ForeColor = Color.White,
                    TextAlign = HorizontalAlignment.Center,
                    BorderStyle = BorderStyle.FixedSingle,
                    Text = inDeficit ? "0.00" : expectedNetCash.ToString("0.00", CultureInfo.InvariantCulture),
                    Margin = new Padding(0, 6, 0, 6)
                };

                TableLayoutPanel btns = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Margin = new Padding(0) };
                btns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 63F)); btns.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37F));
                btns.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

                Button btnConfirm = new Button { Text = "✓ تأكيد الترحيل والتقفيل", Dock = DockStyle.Fill, Margin = new Padding(6), BackColor = Negative, ForeColor = Color.White, Font = new Font("Segoe UI", 14F, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
                btnConfirm.FlatAppearance.BorderSize = 0;

                Button btnCancel = new Button { Text = "إلغاء", Dock = DockStyle.Fill, Margin = new Padding(6), BackColor = Color.FromArgb(71, 85, 105), ForeColor = Color.White, Font = new Font("Segoe UI", 14F, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
                btnCancel.FlatAppearance.BorderSize = 0;

                btns.Controls.Add(btnConfirm, 0, 0); btns.Controls.Add(btnCancel, 1, 0);

                int row = 0;
                tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F)); tlp.Controls.Add(lblExpectedTitle, 0, row++);
                tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F)); tlp.Controls.Add(lblExpectedValue, 0, row++);
                tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, inDeficit ? 90F : 40F)); tlp.Controls.Add(lblInstruction, 0, row++);
                if (!inDeficit) { tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F)); tlp.Controls.Add(txtActual, 0, row++); }
                tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 68F)); tlp.Controls.Add(btns, 0, row++);
                tlp.RowCount = row;

                prompt.Controls.Add(tlp); prompt.AcceptButton = btnConfirm; prompt.CancelButton = btnCancel;

                if (!inDeficit) prompt.Shown += (s, ev) => { txtActual.Focus(); txtActual.SelectAll(); };

                txtActual.KeyPress += (s, ev) =>
                {
                    if (!char.IsControl(ev.KeyChar) && !char.IsDigit(ev.KeyChar) && (ev.KeyChar != '.') && (ev.KeyChar != ',')) ev.Handled = true;
                    if ((ev.KeyChar == '.' || ev.KeyChar == ',') && ((s as TextBox).Text.IndexOf('.') > -1 || (s as TextBox).Text.IndexOf(',') > -1)) ev.Handled = true;
                };

                btnConfirm.Click += async (s, ev) =>
                {
                    decimal actual = 0m;
                    if (!inDeficit)
                    {
                        if (!TryParseAmount(txtActual.Text, out actual) || actual < 0)
                        {
                            Msg("برجاء إدخال مبلغ صحيح.", "خطأ", MessageBoxIcon.Warning, prompt);
                            txtActual.Focus(); return;
                        }
                        actual = Math.Round(actual, 2, MidpointRounding.AwayFromZero);
                    }

                    string diffMsg;
                    if (inDeficit) diffMsg = $"الخزنة في عجز مقداره {Math.Abs(expectedNetCash):N2} ج.م.\nلن يتم ترحيل أي مبلغ، وسيبقى العجز في الخزنة ويُرحَّل للوردية القادمة كما هو.";
                    else
                    {
                        decimal diff = actual - expectedNetCash;
                        bool largeDiff = Math.Abs(diff) >= Math.Max(LargeDiffMin, Math.Abs(expectedNetCash) * LargeDiffRatio);

                        if (diff == 0) diffMsg = "سيتم ترحيل كامل الرصيد المتوقع بالدرج.";
                        else if (diff > 0) diffMsg = $"يوجد زيادة فعلية بالدرج مقدارها {diff:N2} ج.م عن المتوقع دفتريًا، وسيتم تسجيلها وترحيلها بالكامل.";
                        else diffMsg = $"سيتم ترحيل {actual:N2} ج.م فقط.\nويتبقى {Math.Abs(diff):N2} ج.م بالدرج كرصيد، وسيتم ترحيله تلقائيًا للوردية القادمة.";

                        if (largeDiff) diffMsg = "⚠ الفرق عن المتوقع كبير (" + Math.Abs(diff).ToString("N2") + " ج.م). اتأكد إن المبلغ المكتوب صح.\n\n" + diffMsg;
                    }

                    if (Ask(diffMsg + "\n\nهل أنت متأكد من إتمام العملية؟", "تأكيد نهائي", prompt) != DialogResult.Yes) return;

                    btnConfirm.Enabled = false; btnCancel.Enabled = false;
                    try
                    {
                        await cashService.CloseDailyRegisterAsync(currentStats, actual);
                        Msg("تم تقفيل الوردية وترحيل البيانات بنجاح.", "تم التقفيل", MessageBoxIcon.Information, prompt);
                        prompt.DialogResult = DialogResult.OK;
                    }
                    catch (InvalidOperationException ex)
                    {
                        Msg(ex.Message, "تنبيه", MessageBoxIcon.Warning, prompt);
                        prompt.DialogResult = DialogResult.Cancel;
                    }
                    catch (Exception ex)
                    {
                        Msg("حدث خطأ أثناء التقفيل:\n" + ex.Message, "خطأ", MessageBoxIcon.Error, prompt);
                        btnConfirm.Enabled = true; btnCancel.Enabled = true;
                    }
                };

                btnCancel.Click += (s, ev) => { prompt.DialogResult = DialogResult.Cancel; };
                prompt.ShowDialog(this);
            }
        }
    }
}