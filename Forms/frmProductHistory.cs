using MajesticDesktop.Services;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    public partial class frmProductHistory : Form
    {
        private static readonly Color ClrBgDark = Color.FromArgb(15, 23, 42);
        private static readonly Color ClrBgPanel = Color.FromArgb(30, 41, 59);
        private static readonly Color ClrGridHeader = Color.FromArgb(20, 30, 45);
        private static readonly Color ClrGridHeaderText = Color.White;
        private static readonly Color ClrText = Color.FromArgb(241, 245, 249);
        private static readonly Color ClrAccent = Color.FromArgb(59, 130, 246);
        private static readonly Color ClrGreen = Color.FromArgb(34, 197, 94);
        private static readonly Color ClrOrange = Color.FromArgb(245, 158, 11);

        private readonly Font boldFont = new Font("Segoe UI", 11.5F, FontStyle.Bold);

        public frmProductHistory(int productId, string productName, int locationId, DateTime from, DateTime to)
        {
            InitializeComponent();

            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            Text = "سجل حركة المنتج";
            lblTitle.Text = "📦  سجل حركة المنتج: " + productName;
            lblSubtitle.Text = "الفترة: من " + from.ToString("yyyy/MM/dd") + " إلى " + to.ToString("yyyy/MM/dd");

            this.DoubleBuffered = true;
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvHistory, new object[] { true });

            ApplyGridStyle();
            _ = LoadHistoryAsync(productId, locationId, from, to);
        }

        private async Task LoadHistoryAsync(int productId, int locationId, DateTime from, DateTime to)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                DataTable dt = await new ReportsService().GetProductHistoryAsync(productId, locationId, from, to);

                if (dt != null)
                {
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

                if (col.Name.StartsWith("كود") || col.HeaderText.StartsWith("كود"))
                {
                    col.Visible = false;
                }
                else if (col.Name == "م")
                {
                    col.DisplayIndex = 0; // إجبار عمود م ليكون الأول
                    col.FillWeight = 15;
                }
                else if (col.HeaderText == "التاريخ")
                {
                    col.DisplayIndex = 1; // الثاني
                    col.FillWeight = 40;
                }
                else if (col.HeaderText == "نوع الحركة")
                {
                    col.DisplayIndex = 2; // الثالث
                    col.FillWeight = 30;
                }
                else if (col.HeaderText == "الطرف الثاني")
                {
                    col.DisplayIndex = 3; // الرابع
                    col.FillWeight = 60;
                }
                else if (col.HeaderText == "الكمية")
                {
                    col.DisplayIndex = 4; // الخامس
                    col.FillWeight = 20;
                }
                else if (col.HeaderText == "السعر")
                {
                    col.DisplayIndex = 5; // السادس والأخير
                    col.FillWeight = 25;
                }
            }
        }

        private void UpdateTotals(DataTable dt)
        {
            decimal salesQty = 0, salesVal = 0, purQty = 0, purVal = 0;
            foreach (DataRow r in dt.Rows)
            {
                decimal q = r["الكمية"] == DBNull.Value ? 0 : Convert.ToDecimal(r["الكمية"]);
                decimal p = r["السعر"] == DBNull.Value ? 0 : Convert.ToDecimal(r["السعر"]);

                if (Convert.ToString(r["نوع الحركة"]) == "مبيعات")
                {
                    salesQty += q;
                    salesVal += q * p;
                }
                else
                {
                    purQty += q;
                    purVal += q * p;
                }
            }
            lblTotals.Text = "المبيعات: " + salesQty.ToString("N2") + " وحدة (" + salesVal.ToString("N2") + " ج.م)"
                           + "      |      المشتريات: " + purQty.ToString("N2") + " وحدة (" + purVal.ToString("N2") + " ج.م)"
                           + "      |      عدد الحركات: " + dt.Rows.Count;
        }

        private void dgvHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (dgvHistory.Columns[e.ColumnIndex].HeaderText == "نوع الحركة")
            {
                e.CellStyle.ForeColor = Convert.ToString(e.Value) == "مبيعات" ? ClrGreen : ClrOrange;
                e.CellStyle.Font = boldFont;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}