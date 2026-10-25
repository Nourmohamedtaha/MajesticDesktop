using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using MajesticDesktop.Models;
using MajesticDesktop.Services;

namespace MajesticDesktop.Forms
{
    public partial class frmMainTreasury : Form
    {
        private const string SubtitleText = "متابعة شاملة للمبيعات والأرصدة لجميع الفروع والمخزن والإدارة";
        private const string LoadingText = "جاري تحديث البيانات... ⏳";

        private static readonly Color ClrBgDark = Color.FromArgb(15, 23, 42);
        private static readonly Color ClrBgPanel = Color.FromArgb(30, 41, 59);
        private static readonly Color ClrGridHeader = Color.FromArgb(20, 30, 45);
        private static readonly Color ClrGridHeaderText = Color.White;
        private static readonly Color Positive = Color.FromArgb(34, 197, 94);
        private static readonly Color Negative = Color.FromArgb(239, 68, 68);
        private static readonly Color DigitalBlue = Color.FromArgb(59, 130, 246);
        private static readonly Color ProfitNegative = Color.FromArgb(248, 113, 113);
        private static readonly Color ProfitPositive = Color.FromArgb(250, 204, 21);

        private static readonly Font FontCell = new Font("Segoe UI", 12.5F, FontStyle.Bold);
        private static readonly Font FontHeader = new Font("Segoe UI", 13.5F, FontStyle.Bold);

        private MainTreasuryService treasuryService;
        private bool _busy;

        public frmMainTreasury()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            Opacity = 0;
            DoubleBuffered = true;
            KeyPreview = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            UpdateStyles();

            StyleDataGridView();
            EnableDoubleBuffering(dgvTransactions);

            dgvTransactions.CellFormatting += dgvTransactions_CellFormatting;
            KeyDown += frmMainTreasury_KeyDown;
            Shown += frmMainTreasury_Shown;
        }

        private static void EnableDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                dgv,
                new object[] { true });
        }

        private void frmMainTreasury_Load(object sender, EventArgs e)
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                treasuryService = new MainTreasuryService();

                dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpTo.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                ShowError("تعذر فتح شاشة الخزينة", ex);
            }
        }

        private async void frmMainTreasury_Shown(object sender, EventArgs e)
        {
            try
            {
                Refresh();
            }
            finally
            {
                Opacity = 1;
            }

            await RefreshTreasuryDataAsync();
        }

        private async void frmMainTreasury_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && !_busy)
            {
                e.Handled = true;
                await RefreshTreasuryDataAsync();
            }
        }

        private DialogResult Msg(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(this, text, caption, buttons, icon, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void ShowError(string title, Exception ex)
        {
            string details = ex.Message;
            if (ex.InnerException != null) details += "\n" + ex.InnerException.Message;

            try
            {
                File.AppendAllText(Path.Combine(Application.StartupPath, "treasury_errors.log"),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + " | " + title + "\r\n" + ex + "\r\n\r\n");
            }
            catch { }

            Msg(title + ":\n" + details, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void SetBusy(bool busy)
        {
            _busy = busy;

            btnSearch.Enabled = !busy;
            btnAddIn.Enabled = !busy;
            btnAddOut.Enabled = !busy;
            btnCloseMonth.Enabled = !busy;

            UseWaitCursor = busy;
        }

        private static bool IsIncome(string type)
        {
            return type == "وارد" || type == "إيراد" || type == "مبيعات";
        }

        private void StyleDataGridView()
        {
            dgvTransactions.AutoGenerateColumns = false;

            colId.DataPropertyName = "Seq";
            colDate.DataPropertyName = "DateText";
            colType.DataPropertyName = "TypeText";
            colAmount.DataPropertyName = "AmountText";
            colPaymentMethod.DataPropertyName = "PayText";
            colDesc.DataPropertyName = "DescText";
            colRef.DataPropertyName = "RefText";

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
            dgvTransactions.ColumnHeadersDefaultCellStyle.Font = FontHeader;
            dgvTransactions.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTransactions.ColumnHeadersHeight = 50;

            // ضبط توزيع الأعمدة والمحاذاة
            colDesc.FillWeight = 30F; colDesc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colAmount.FillWeight = 15F; colAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colType.FillWeight = 10F; colType.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colPaymentMethod.FillWeight = 15F; colPaymentMethod.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colDate.FillWeight = 20F; colDate.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colRef.FillWeight = 10F; colRef.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colId.FillWeight = 5F; colId.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvTransactions.DefaultCellStyle.BackColor = ClrBgPanel;
            dgvTransactions.DefaultCellStyle.ForeColor = Color.WhiteSmoke;
            dgvTransactions.DefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 120, 190);
            dgvTransactions.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvTransactions.DefaultCellStyle.Font = FontCell;

            dgvTransactions.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 50, 71);
            dgvTransactions.RowTemplate.Height = 50;
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransactions.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            dgvTransactions.ReadOnly = true;
            dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.MultiSelect = false;
            dgvTransactions.AllowUserToResizeRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.AllowUserToOrderColumns = false;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.RightToLeft = RightToLeft.Yes;
        }

        private void dgvTransactions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataRowView item = dgvTransactions.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (item == null)
                return;

            if (e.ColumnIndex == colType.Index || e.ColumnIndex == colAmount.Index)
            {
                e.CellStyle.ForeColor = (bool)item["IsIncome"] ? Positive : Negative;
            }
            else if (e.ColumnIndex == colPaymentMethod.Index && (bool)item["IsDigital"])
            {
                e.CellStyle.ForeColor = DigitalBlue;
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await RefreshTreasuryDataAsync();
        }

        private void btnAddIn_Click(object sender, EventArgs e)
        {
            AddNewTransaction("وارد");
        }

        private void btnAddOut_Click(object sender, EventArgs e)
        {
            AddNewTransaction("منصرف");
        }

        private async void btnCloseMonth_Click(object sender, EventArgs e)
        {
            if (treasuryService == null || _busy)
                return;

            DateTime monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            TreasuryStats stats = null;

            SetBusy(true);
            try
            {
                stats = await Task.Run(() => treasuryService.GetTreasuryStats(monthStart, DateTime.Now));
            }
            catch (Exception ex)
            {
                if (!IsDisposed) ShowError("خطأ أثناء التقفيل", ex);
            }
            finally
            {
                if (!IsDisposed) SetBusy(false);
            }

            if (stats == null || IsDisposed)
                return;

            if (stats.TotalBalance <= 0)
            {
                Msg("الخزينة مصفرة بالفعل، لا يوجد رصيد لتقفيله.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult res = Msg(
                $"هل أنت متأكد من تصفية وتقفيل حسابات الخزينة المركزية؟\n\nالكاش المتاح للسحب: {stats.TotalCash:N2}\nإنستاباي الكردي: {stats.InstaKurdi:N2}\nإنستاباي الحكش: {stats.InstaHakosh:N2}\n\nالإجمالي الكلي: {stats.TotalBalance:N2} ج.م\n\nملحوظة: هتترصد كل الأرقام دي على 0 بعد التقفيل ولحد ما تدخل حركات جديدة.",
                "تأكيد التصفية والأرشفة", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (res != DialogResult.Yes)
                return;

            TreasuryStats snapshot = stats;
            string errorMsg = string.Empty;
            bool closed = false;

            SetBusy(true);
            try
            {
                closed = await Task.Run(() =>
                {
                    string err;
                    bool ok = treasuryService.CloseMonthlyTreasury(snapshot, out err);
                    errorMsg = err;
                    return ok;
                });
            }
            catch (Exception ex)
            {
                if (!IsDisposed) ShowError("خطأ أثناء التقفيل", ex);
                return;
            }
            finally
            {
                if (!IsDisposed) SetBusy(false);
            }

            if (IsDisposed)
                return;

            if (closed)
            {
                Msg("تم تقفيل الخزينة وأرشفة البيانات بنجاح.", "تم التقفيل", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await RefreshTreasuryDataAsync();
            }
            else
            {
                Msg(errorMsg, "مرفوض", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private async Task RefreshTreasuryDataAsync()
        {
            if (treasuryService == null || _busy)
                return;

            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date;

            if (from > to)
            {
                Msg("تاريخ (من) لازم يكون قبل أو يساوي تاريخ (إلى).", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SetBusy(true);
            lblSubtitle.Text = LoadingText;

            try
            {
                Task<TreasuryStats> statsTask = Task.Run(() => treasuryService.GetTreasuryStats(from, to));
                Task<DataTable> rowsTask = Task.Run(() => BuildTransactionsTable(treasuryService.GetTransactions(from, to)));

                await Task.WhenAll(statsTask, rowsTask);

                if (IsDisposed)
                    return;

                ApplyStats(statsTask.Result);
                BindTransactions(rowsTask.Result);
            }
            catch (Exception ex)
            {
                if (!IsDisposed) ShowError("خطأ في تحديث البيانات", ex);
            }
            finally
            {
                if (!IsDisposed)
                {
                    lblSubtitle.Text = SubtitleText;
                    SetBusy(false);
                }
            }
        }

        private void ApplyStats(TreasuryStats stats)
        {
            lblNetTotal.Text = stats.TotalBalance.ToString("N2") + " ج.م";
            lblCashVal.Text = stats.TotalCash.ToString("N2") + " ج.م";
            lblInstaKurdiVal.Text = stats.InstaKurdi.ToString("N2") + " ج.م";
            lblInstaHakoshVal.Text = stats.InstaHakosh.ToString("N2") + " ج.م";
            lblTotalSalesVal.Text = stats.TotalSales.ToString("N2") + " ج.م";

            lblProfitVal.Text = stats.NetProfit.ToString("N2") + " ج.م";
            lblProfitVal.ForeColor = stats.NetProfit < 0 ? ProfitNegative : ProfitPositive;

            lblDebtsVal.Text = stats.TotalDebts.ToString("N2") + " ج.م";
            lblHQInVal.Text = stats.HQManualIn.ToString("N2") + " ج.م";
            lblHQOutVal.Text = stats.HQManualOut.ToString("N2") + " ج.م";
        }

        private static DataTable BuildTransactionsTable(DataTable source)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Seq", typeof(int));
            table.Columns.Add("DateText", typeof(string));
            table.Columns.Add("TypeText", typeof(string));
            table.Columns.Add("AmountText", typeof(string));
            table.Columns.Add("PayText", typeof(string));
            table.Columns.Add("DescText", typeof(string));
            table.Columns.Add("RefText", typeof(string));
            table.Columns.Add("IsIncome", typeof(bool));
            table.Columns.Add("IsDigital", typeof(bool));

            int counter = 1;

            table.BeginLoadData();
            foreach (DataRow row in source.Rows)
            {
                string type = Convert.ToString(row["TransactionType"]);

                string pay = Convert.ToString(row["PaymentMethod"]);
                if (string.IsNullOrEmpty(pay) || pay == "كاش")
                    pay = "نقدي";

                table.Rows.Add(
                    counter++,
                    Convert.ToDateTime(row["TransactionDate"]).ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture),
                    type,
                    Convert.ToDecimal(row["Amount"]).ToString("N2"),
                    pay,
                    Convert.ToString(row["Description"]),
                    Convert.ToString(row["ReferenceNumber"]),
                    IsIncome(type),
                    pay != "نقدي");
            }
            table.EndLoadData();

            return table;
        }

        private void BindTransactions(DataTable table)
        {
            dgvTransactions.SuspendLayout();
            dgvTransactions.DataSource = table;
            dgvTransactions.ClearSelection();
            dgvTransactions.ResumeLayout();
        }

        private async void AddNewTransaction(string type)
        {
            if (treasuryService == null || _busy)
                return;

            DialogResult result;

            using (Form frm = new Form())
            {
                frm.Text = type == "وارد" ? "إيراد أو تمويل خارجي (للإدارة)" : "مصروفات وسحوبات (من الإدارة)";
                frm.ClientSize = new Size(400, 340);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.FormBorderStyle = FormBorderStyle.FixedDialog;
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                frm.ShowInTaskbar = false;
                frm.RightToLeft = RightToLeft.Yes;
                frm.RightToLeftLayout = false;
                frm.BackColor = Color.FromArgb(30, 40, 55);

                Font lblFont = new Font("Segoe UI", 10, FontStyle.Bold);

                TableLayoutPanel tlp = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    Padding = new Padding(30, 20, 30, 20)
                };
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

                Label lblAmt = new Label { Text = "المبلغ:", AutoSize = true, ForeColor = Color.White, Font = lblFont, Anchor = AnchorStyles.Right, Margin = new Padding(0, 0, 0, 4) };
                TextBox txtAmt = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, Font = new Font("Segoe UI", 12, FontStyle.Bold), TextAlign = HorizontalAlignment.Center, Margin = new Padding(0, 0, 0, 12) };

                Label lblPayMethod = new Label { Text = "طريقة الدفع:", AutoSize = true, ForeColor = Color.White, Font = lblFont, Anchor = AnchorStyles.Right, Margin = new Padding(0, 0, 0, 4) };
                ComboBox cmbPayMethod = new ComboBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, Font = new Font("Segoe UI", 11), DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(0, 0, 0, 12) };
                cmbPayMethod.Items.AddRange(new string[] { "نقدي", "انستاباي الكردي", "انستاباي الحكش" });
                cmbPayMethod.SelectedIndex = 0;

                Label lblDesc = new Label { Text = "البيان / الوصف:", AutoSize = true, ForeColor = Color.White, Font = lblFont, Anchor = AnchorStyles.Right, Margin = new Padding(0, 0, 0, 4) };
                TextBox txtDesc = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, Font = new Font("Segoe UI", 11), MaxLength = 255, Margin = new Padding(0, 0, 0, 16) };

                Button btnSave = new Button
                {
                    Text = "حفظ الحركة",
                    Anchor = AnchorStyles.None,
                    Size = new Size(160, 42),
                    BackColor = type == "وارد" ? Positive : Negative,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    UseVisualStyleBackColor = false
                };
                btnSave.FlatAppearance.BorderSize = 0;

                foreach (Control c in new Control[] { lblAmt, txtAmt, lblPayMethod, cmbPayMethod, lblDesc, txtDesc, btnSave })
                {
                    tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    tlp.Controls.Add(c);
                }

                frm.Controls.Add(tlp);
                frm.AcceptButton = btnSave;

                txtAmt.KeyPress += (s, ev) =>
                {
                    if (!char.IsControl(ev.KeyChar) && !char.IsDigit(ev.KeyChar) && ev.KeyChar != '.')
                        ev.Handled = true;

                    if (ev.KeyChar == '.' && (s as TextBox).Text.IndexOf('.') > -1)
                        ev.Handled = true;
                };

                btnSave.Click += (s, ev) =>
                {
                    decimal amount;
                    if (!decimal.TryParse(txtAmt.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out amount) || amount <= 0)
                    {
                        MessageBox.Show(frm, "من فضلك أدخل مبلغ صحيح.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                        return;
                    }

                    try
                    {
                        CashTransaction trans = new CashTransaction
                        {
                            TransactionDate = DateTime.Now,
                            TransactionType = type,
                            Amount = amount,
                            Description = txtDesc.Text.Trim(),
                            ReferenceNumber = "HQ-MANUAL"
                        };

                        treasuryService.AddTransaction(trans, cmbPayMethod.Text);
                        frm.DialogResult = DialogResult.OK;
                    }
                    catch (Exception ex)
                    {
                        ShowError("خطأ أثناء حفظ الحركة", ex);
                    }
                };

                result = frm.ShowDialog(this);
            }

            if (result == DialogResult.OK)
                await RefreshTreasuryDataAsync();
        }
    }
}