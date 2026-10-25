using MajesticDesktop.Services;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    public partial class frmCustomerHistory : Form
    {
        private const string ColDebit = "مدين (عليه)";
        private const string ColCredit = "دائن (سدد)";

        // الألوان الموحدة
        private static readonly Color ClrBgDark = Color.FromArgb(15, 23, 42);
        private static readonly Color ClrBgPanel = Color.FromArgb(30, 41, 59);
        private static readonly Color ClrGridHeader = Color.FromArgb(20, 30, 45);
        private static readonly Color ClrGridHeaderText = Color.White;
        private static readonly Color ClrText = Color.FromArgb(241, 245, 249);
        private static readonly Color ClrAccent = Color.FromArgb(59, 130, 246);
        private static readonly Color ClrGreen = Color.FromArgb(34, 197, 94);
        private static readonly Color ClrRed = Color.FromArgb(239, 68, 68);

        private readonly Font boldFont = new Font("Segoe UI", 11.5F, FontStyle.Bold);

        public frmCustomerHistory(int customerId, string customerName, DateTime from, DateTime to)
        {
            InitializeComponent();

            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            Text = "كشف حساب العميل";
            lblTitle.Text = "👤  كشف حساب العميل: " + customerName;
            lblSubtitle.Text = "الفترة: من " + from.ToString("yyyy/MM/dd") + " إلى " + to.ToString("yyyy/MM/dd");

            this.DoubleBuffered = true;
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvHistory, new object[] { true });

            ApplyGridStyle();
            _ = LoadHistoryAsync(customerId, from, to);
        }

        private async Task LoadHistoryAsync(int customerId, DateTime from, DateTime to)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                DataTable dt = await new ReportsService().GetCustomerLedgerAsync(customerId, from, to);

                if (dt != null)
                {
                    // إضافة عمود المسلسل (م)
                    dt.Columns.Add("م", typeof(int)).SetOrdinal(0);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["م"] = i + 1;
                    }

                    dgvHistory.DataSource = dt;
                    FormatColumns(); // التعديل الأهم هنا
                    UpdateTotals(dt);
                    dgvHistory.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ApplyGridStyle()
        {
            this.BackColor = ClrBgDark;
            pnlTop.BackColor = ClrGridHeader;
            pnlFooter.BackColor = ClrBgPanel;
            lblTitle.ForeColor = ClrText;
            lblSubtitle.ForeColor = Color.DarkGray;
            lblTotals.ForeColor = ClrText;

            dgvHistory.BackgroundColor = ClrBgDark;
            dgvHistory.BorderStyle = BorderStyle.None;
            dgvHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHistory.GridColor = Color.FromArgb(51, 65, 85);
            dgvHistory.EnableHeadersVisualStyles = false;
            dgvHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvHistory.ColumnHeadersHeight = 50;
            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor = ClrGridHeader;
            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor = ClrGridHeaderText;
            dgvHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            dgvHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHistory.ColumnHeadersDefaultCellStyle.SelectionBackColor = ClrGridHeader;

            dgvHistory.DefaultCellStyle.BackColor = ClrBgPanel;
            dgvHistory.DefaultCellStyle.ForeColor = ClrText;
            dgvHistory.DefaultCellStyle.SelectionBackColor = ClrAccent;
            dgvHistory.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvHistory.DefaultCellStyle.Font = new Font("Segoe UI", 12.5F, FontStyle.Bold);
            // توحيد المحاذاة للمنتصف
            dgvHistory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 50, 71);
            dgvHistory.RowTemplate.Height = 45;
            dgvHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHistory.AllowUserToAddRows = false;
            dgvHistory.AllowUserToDeleteRows = false;
            dgvHistory.AllowUserToResizeRows = false;
            dgvHistory.ReadOnly = true;
            dgvHistory.RowHeadersVisible = false;
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHistory.MultiSelect = false;
        }

        // التعديل هنا لترتيب الأعمدة وضبط محاذاتها بدقة
        private void FormatColumns()
        {
            foreach (DataGridViewColumn col in dgvHistory.Columns)
            {
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                if (col.ValueType == typeof(DateTime))
                    col.DefaultCellStyle.Format = "yyyy/MM/dd  hh:mm tt";
                else if (col.ValueType == typeof(decimal) || col.ValueType == typeof(double))
                    col.DefaultCellStyle.Format = "N2";

                if (col.Name == "م")
                {
                    col.DisplayIndex = 0; // إجبار عمود م ليكون الأول
                    col.FillWeight = 15;
                }
                else if (col.HeaderText == "التاريخ")
                {
                    col.DisplayIndex = 1; // الثاني
                    col.FillWeight = 30;
                }
                else if (col.HeaderText == "البيان")
                {
                    col.DisplayIndex = 2; // الثالث
                    col.FillWeight = 70; // البيان ياخد مساحة أكبر
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; // محاذاة لليمين للبيان
                }
                else if (col.HeaderText == ColDebit)
                {
                    col.DisplayIndex = 3; // الرابع
                    col.FillWeight = 25;
                }
                else if (col.HeaderText == ColCredit)
                {
                    col.DisplayIndex = 4; // الخامس والأخير
                    col.FillWeight = 25;
                }
            }
        }

        private void UpdateTotals(DataTable dt)
        {
            decimal debit = 0, credit = 0;
            foreach (DataRow r in dt.Rows)
            {
                if (r[ColDebit] != DBNull.Value) debit += Convert.ToDecimal(r[ColDebit]);
                if (r[ColCredit] != DBNull.Value) credit += Convert.ToDecimal(r[ColCredit]);
            }

            decimal net = debit - credit;
            string netStatus = net > 0 ? " (عليه)" : (net < 0 ? " (له)" : " (خالص)");

            lblTotals.Text = "إجمالي المدين: " + debit.ToString("N2") + " ج.م"
                           + "      |      إجمالي الدائن: " + credit.ToString("N2") + " ج.م"
                           + "      |      الصافي: " + Math.Abs(net).ToString("N2") + netStatus;
        }

        private void dgvHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null || e.Value == DBNull.Value) return;
            string header = dgvHistory.Columns[e.ColumnIndex].HeaderText;

            if (header == ColDebit || header == ColCredit)
            {
                decimal v;
                if (decimal.TryParse(Convert.ToString(e.Value), out v) && v > 0)
                {
                    e.CellStyle.ForeColor = (header == ColDebit) ? ClrRed : ClrGreen;
                    e.CellStyle.Font = boldFont;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}