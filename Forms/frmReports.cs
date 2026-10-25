using MajesticDesktop.Services;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    public partial class frmReports : Form
    {
        private readonly ReportsService reportsService = new ReportsService();
        private string currentReportType = "Customers";
        private bool isLoading;
        private int printRowIndex;
        private int printPageNumber;
        private bool printWithTotal; // المتغير للتحكم في طباعة الإجمالي

        // الألوان الموحدة 
        private static readonly Color ClrBgDark = Color.FromArgb(15, 23, 42);
        private static readonly Color ClrBgPanel = Color.FromArgb(30, 41, 59);
        private static readonly Color ClrAccent = Color.FromArgb(59, 130, 246);
        private static readonly Color ClrGridHeader = Color.FromArgb(20, 30, 45);
        private static readonly Color ClrGridHeaderText = Color.White;
        private static readonly Color ClrText = Color.FromArgb(241, 245, 249);

        public frmReports()
        {
            InitializeComponent();

            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            this.DoubleBuffered = true;
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvReport, new object[] { true });
        }

        private async void frmReports_Load(object sender, EventArgs e)
        {
            isLoading = true;
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTo.Value = DateTime.Now;

            ApplyGridStyle();
            await LoadComboBoxesAsync();

            isLoading = false;

            SelectTabButton(btnCustomers, "Customers");
        }

        private DialogResult Msg(string text, string caption, MessageBoxIcon icon)
        {
            return MessageBox.Show(this, text, caption, MessageBoxButtons.OK, icon,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private async Task LoadComboBoxesAsync()
        {
            try
            {
                cmbLocation.DisplayMember = "Name";
                cmbLocation.ValueMember = "Id";
                cmbLocation.DataSource = await reportsService.GetAllLocationsAsync();

                DataTable dtProds = await reportsService.GetAllProductsAsync();
                DataRow dr = dtProds.NewRow();
                dr["Id"] = 0;
                dr["Name"] = "--- كل المنتجات ---";
                dtProds.Rows.InsertAt(dr, 0);
                cmbProduct.DisplayMember = "Name";
                cmbProduct.ValueMember = "Id";
                cmbProduct.DataSource = dtProds;

                DataTable dtRegions = await reportsService.GetDistinctRegionsAsync();
                DataRow drR = dtRegions.NewRow();
                drR["Region"] = "الكل";
                dtRegions.Rows.InsertAt(drR, 0);
                cmbRegion.DisplayMember = "Region";
                cmbRegion.ValueMember = "Region";
                cmbRegion.DataSource = dtRegions;
            }
            catch (Exception ex)
            {
                Msg("خطأ في تحميل الفلاتر: " + ex.Message, "خطأ", MessageBoxIcon.Error);
            }
        }

        private void ApplyGridStyle()
        {
            this.BackColor = ClrBgDark;
            pnlFilters.BackColor = ClrBgPanel;
            pnlFooter.BackColor = ClrBgPanel;
            pnlTopNav.BackColor = ClrGridHeader;

            dgvReport.BackgroundColor = ClrBgDark;
            dgvReport.BorderStyle = BorderStyle.None;
            dgvReport.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvReport.GridColor = Color.FromArgb(51, 65, 85);
            dgvReport.EnableHeadersVisualStyles = false;
            dgvReport.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvReport.ColumnHeadersHeight = 55;
            dgvReport.ColumnHeadersDefaultCellStyle.BackColor = ClrGridHeader;
            dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = ClrGridHeaderText;
            dgvReport.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            dgvReport.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.ColumnHeadersDefaultCellStyle.SelectionBackColor = ClrGridHeader;

            dgvReport.DefaultCellStyle.BackColor = ClrBgPanel;
            dgvReport.DefaultCellStyle.ForeColor = ClrText;
            dgvReport.DefaultCellStyle.SelectionBackColor = ClrAccent;
            dgvReport.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvReport.DefaultCellStyle.Font = new Font("Segoe UI", 12.5F, FontStyle.Bold);
            // محاذاة كل الأعمدة للمنتصف
            dgvReport.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReport.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);

            dgvReport.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 50, 71);
            dgvReport.RowTemplate.Height = 50;
            dgvReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReport.AllowUserToAddRows = false;
            dgvReport.AllowUserToDeleteRows = false;
            dgvReport.AllowUserToResizeRows = false;
            dgvReport.ReadOnly = true;
            dgvReport.RowHeadersVisible = false;
            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.MultiSelect = false;
        }

        private void FormatColumns()
        {
            foreach (DataGridViewColumn col in dgvReport.Columns)
            {
                col.MinimumWidth = 70;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

                // إجبار المحاذاة في المنتصف دائماً ليكون الجدول منظماً
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                if (col.ValueType == typeof(decimal) || col.ValueType == typeof(double) || col.ValueType == typeof(float))
                    col.DefaultCellStyle.Format = "N2";

                if (col.HeaderText.StartsWith("كود") || col.Name.StartsWith("كود"))
                {
                    col.Visible = false;
                }
                else if (col.Name == "م")
                {
                    col.FillWeight = 30;
                }
                else if (col.HeaderText.StartsWith("اسم") || col.HeaderText.StartsWith("البيان"))
                {
                    // نسبة معقولة لعرض الاسم بدون ما يكون بعيد عن باقي البيانات وبدون ما يكون يمين
                    col.FillWeight = 110;
                }
            }
        }

        private async void SelectTabButton(Button active, string repType)
        {
            currentReportType = repType;

            foreach (Button b in new[] { btnCustomers, btnSuppliers, btnInventory, btnProfit })
            {
                bool on = (b == active);
                b.BackColor = on ? ClrAccent : ClrBgPanel;
                b.ForeColor = on ? Color.White : ClrText;
                b.FlatAppearance.MouseOverBackColor = on ? ClrAccent : Color.FromArgb(51, 65, 85);
            }

            pnlGrpProduct.Visible = (repType == "Profit");
            pnlGrpRegion.Visible = (repType == "Customers");

            bool useDates = (repType == "Customers" || repType == "Profit");
            pnlGrpFrom.Visible = useDates;
            pnlGrpTo.Visible = useDates;

            dgvReport.Cursor = (repType == "Suppliers") ? Cursors.Default : Cursors.Hand;

            await GenerateReportAsync();
        }

        private void btnCustomers_Click(object sender, EventArgs e) { SelectTabButton(btnCustomers, "Customers"); }
        private void btnSuppliers_Click(object sender, EventArgs e) { SelectTabButton(btnSuppliers, "Suppliers"); }
        private void btnInventory_Click(object sender, EventArgs e) { SelectTabButton(btnInventory, "Inventory"); }
        private void btnProfit_Click(object sender, EventArgs e) { SelectTabButton(btnProfit, "Profit"); }

        private async void btnSearch_Click(object sender, EventArgs e) { await GenerateReportAsync(); }

        private async void Filter_Changed(object sender, EventArgs e)
        {
            if (!isLoading) await GenerateReportAsync();
        }

        private string GetReportTitle()
        {
            switch (currentReportType)
            {
                case "Customers": return "تقرير العملاء والمديونيات";
                case "Suppliers": return "تقرير الموردين والأرصدة";
                case "Inventory": return "تقرير جرد المخازن";
                case "Profit": return "تقرير ربحية المنتجات";
                default: return "تقرير النظام";
            }
        }

        private async Task GenerateReportAsync()
        {
            if (isLoading || cmbLocation.SelectedValue == null) return;

            bool usesDates = (currentReportType == "Customers" || currentReportType == "Profit");
            if (usesDates && dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                Msg("تاريخ (من) لازم يكون قبل أو يساوي تاريخ (إلى).", "تنبيه", MessageBoxIcon.Warning);
                return;
            }

            int locId = Convert.ToInt32(cmbLocation.SelectedValue);

            try
            {
                Cursor = Cursors.WaitCursor;
                DataTable dt = null;

                switch (currentReportType)
                {
                    case "Customers":
                        string reg = cmbRegion.SelectedValue?.ToString();
                        dt = await reportsService.GetCustomersReportAsync(locId, reg, dtpFrom.Value, dtpTo.Value);
                        break;
                    case "Suppliers":
                        dt = await reportsService.GetSuppliersReportAsync(locId, dtpFrom.Value, dtpTo.Value);
                        break;
                    case "Inventory":
                        dt = await reportsService.GetInventoryReportAsync(locId);
                        break;
                    case "Profit":
                        int prodId = Convert.ToInt32(cmbProduct.SelectedValue);
                        dt = await reportsService.GetProfitabilityReportAsync(locId, prodId, dtpFrom.Value, dtpTo.Value);
                        break;
                }

                if (dt != null)
                {
                    dt.Columns.Add("م", typeof(int)).SetOrdinal(0);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["م"] = i + 1;
                    }

                    dgvReport.DataSource = dt;
                    FormatColumns(); // التنسيق الصحيح الموحد للمنتصف والمقاسات المضبوطة
                    UpdateFooter(dt);
                }
            }
            catch (Exception ex)
            {
                Msg(ex.Message, "خطأ", MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void UpdateFooter(DataTable dt)
        {
            lblRecords.Text = "عدد السجلات: " + dt.Rows.Count.ToString("N0");
            lblHint.Text = (currentReportType == "Suppliers") ? "" : "💡 دبل كليك على أي صف لعرض التفاصيل/الخزنة";

            string colName = null, label = null;
            switch (currentReportType)
            {
                case "Customers": colName = "المديونية الحالية"; label = "إجمالي المديونية"; break;
                case "Suppliers": colName = "الرصيد الافتتاحي"; label = "إجمالي الأرصدة"; break;
                case "Inventory": colName = "إجمالي القيمة"; label = "إجمالي قيمة المخزون"; break;
                case "Profit": colName = "صافي الربح"; label = "إجمالي صافي الربح"; break;
            }

            decimal total = 0;
            if (colName != null && dt.Columns.Contains(colName))
            {
                foreach (DataRow r in dt.Rows)
                    if (r[colName] != DBNull.Value) total += Convert.ToDecimal(r[colName]);
                lblTotal.Text = label + ": " + total.ToString("N2") + " ج.م";
            }
            else lblTotal.Text = "";
        }

        private void btnColumns_Click(object sender, EventArgs e)
        {
            if (dgvReport.Columns.Count == 0) return;

            ContextMenuStrip cm = new ContextMenuStrip();
            cm.RightToLeft = RightToLeft.Yes;
            cm.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            cm.ShowCheckMargin = true;

            cm.Closing += (s, ev) =>
            {
                if (ev.CloseReason == ToolStripDropDownCloseReason.ItemClicked) ev.Cancel = true;
            };

            foreach (DataGridViewColumn col in dgvReport.Columns)
            {
                if (col.Name.StartsWith("كود")) continue;

                DataGridViewColumn c = col;
                ToolStripMenuItem item = new ToolStripMenuItem(c.HeaderText);
                item.Checked = c.Visible;
                item.CheckOnClick = true;
                item.CheckedChanged += (s, ev) =>
                {
                    bool willHideLast = !item.Checked &&
                        dgvReport.Columns.Cast<DataGridViewColumn>().Count(x => x.Visible && !x.Name.StartsWith("كود")) <= 1;
                    if (willHideLast) { item.Checked = true; return; }
                    c.Visible = item.Checked;
                };
                cm.Items.Add(item);
            }
            cm.Show(btnColumns, new Point(0, btnColumns.Height));
        }

        private void dgvReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvReport.Rows[e.RowIndex];

            if (currentReportType == "Inventory" || currentReportType == "Profit")
            {
                if (!dgvReport.Columns.Contains("كود المنتج")) return;
                int pId = Convert.ToInt32(row.Cells["كود المنتج"].Value);
                string pName = Convert.ToString(row.Cells["اسم المنتج"].Value);
                int lId = Convert.ToInt32(cmbLocation.SelectedValue);

                using (frmProductHistory frm = new frmProductHistory(pId, pName, lId, dtpFrom.Value, dtpTo.Value))
                {
                    frm.ShowDialog(this);
                }
            }
            else if (currentReportType == "Customers")
            {
                if (!dgvReport.Columns.Contains("كود العميل")) return;
                int cId = Convert.ToInt32(row.Cells["كود العميل"].Value);
                string cName = Convert.ToString(row.Cells["اسم العميل"].Value);

                using (frmCustomerHistory frm = new frmCustomerHistory(cId, cName, dtpFrom.Value, dtpTo.Value))
                {
                    frm.ShowDialog(this);
                }
            }
        }

        private static string CellToText(object v)
        {
            if (v == null || v == DBNull.Value) return "";
            if (v is DateTime) return ((DateTime)v).ToString("yyyy-MM-dd HH:mm");
            if (v is decimal) return ((decimal)v).ToString("0.##", CultureInfo.InvariantCulture);
            if (v is double) return ((double)v).ToString("0.##", CultureInfo.InvariantCulture);
            return Convert.ToString(v, CultureInfo.InvariantCulture);
        }

        private static string CsvEscape(string v)
        {
            if (string.IsNullOrEmpty(v)) return "";
            if (v.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0)
                return "\"" + v.Replace("\"", "\"\"") + "\"";
            return v;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvReport.Rows.Count == 0)
            {
                Msg("مفيش بيانات للتصدير.", "تنبيه", MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV Excel File (*.csv)|*.csv";
                sfd.FileName = "Report_" + currentReportType + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                if (sfd.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    var cols = dgvReport.Columns.Cast<DataGridViewColumn>()
                        .Where(c => c.Visible && !c.Name.StartsWith("كود")).OrderBy(c => c.DisplayIndex).ToList();

                    byte[] bom = new byte[] { 0xEF, 0xBB, 0xBF };

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Join(",", cols.Select(c => CsvEscape(c.HeaderText))));

                    foreach (DataGridViewRow row in dgvReport.Rows)
                        sb.AppendLine(string.Join(",", cols.Select(c => CsvEscape(CellToText(row.Cells[c.Index].Value)))));

                    using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(bom, 0, bom.Length);
                        byte[] data = Encoding.UTF8.GetBytes(sb.ToString());
                        fs.Write(data, 0, data.Length);
                    }

                    Msg("تم التصدير بنجاح! الملف جاهز.", "نجاح", MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    Msg("خطأ أثناء التصدير: " + ex.Message, "خطأ", MessageBoxIcon.Error);
                }
            }
        }

        // ================= الطباعة الاحترافية والذكية =================
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvReport.Rows.Count == 0)
            {
                Msg("لا توجد بيانات لطباعتها.", "تنبيه", MessageBoxIcon.Warning);
                return;
            }

            // سؤال المستخدم عن طباعة الإجمالي
            string totalLabelName = string.IsNullOrWhiteSpace(lblTotal.Text) ? "الإجمالي" : lblTotal.Text.Split(':')[0];

            DialogResult askTotal = MessageBox.Show(this,
                $"هل ترغب في طباعة [{totalLabelName}] أسفل التقرير المطبوع؟\n\n- اضغط (نعم) للطباعة بالإجمالي.\n- اضغط (لا) للطباعة بدون الإجمالي.",
                "خيارات الطباعة السرية",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);

            if (askTotal == DialogResult.Cancel) return; // المستخدم لغى العملية

            printWithTotal = (askTotal == DialogResult.Yes);

            using (PrintDocument pd = new PrintDocument())
            {
                pd.DefaultPageSettings.Landscape = true;
                pd.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);
                pd.BeginPrint += (s, ev) => { printRowIndex = 0; printPageNumber = 0; };
                pd.PrintPage += Pd_PrintPage;

                using (PrintDialog dlg = new PrintDialog())
                {
                    dlg.Document = pd;
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                        pd.Print();
                }
            }
        }

        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            printPageNumber++;
            Graphics g = e.Graphics;
            Rectangle m = e.MarginBounds;

            var cols = dgvReport.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible && !c.Name.StartsWith("كود")).OrderBy(c => c.DisplayIndex).ToList();

            if (cols.Count == 0) { e.HasMorePages = false; return; }

            float totalW = cols.Sum(c => (float)c.Width);
            if (totalW <= 0) totalW = cols.Count;

            using (Font fTitle = new Font("Segoe UI", 20, FontStyle.Bold))
            using (Font fSub = new Font("Segoe UI", 11, FontStyle.Regular))
            using (Font fHeader = new Font("Segoe UI", 11, FontStyle.Bold))
            using (Font fRow = new Font("Segoe UI", 10, FontStyle.Bold))
            using (StringFormat sfCenter = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft })
            using (StringFormat sfRight = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft })
            using (Pen penThick = new Pen(Color.Black, 2))
            using (Pen penThin = new Pen(Color.Gray, 1))
            {
                float y = m.Top;

                // Header
                g.DrawString("ماجيستيك - Majestic", fTitle, Brushes.Black, new RectangleF(m.Left, y, m.Width, 36), sfCenter);
                y += 40;

                g.DrawString(GetReportTitle() + " | " + cmbLocation.Text, new Font("Segoe UI", 16, FontStyle.Bold), Brushes.Black, new RectangleF(m.Left, y, m.Width, 30), sfCenter);
                y += 35;

                g.DrawLine(penThick, m.Left, y, m.Right, y);
                y += 10;

                // Info Section
                bool usesDates = (currentReportType == "Customers" || currentReportType == "Profit");
                string periodText = usesDates ? $"الفترة: من {dtpFrom.Value:yyyy/MM/dd} إلى {dtpTo.Value:yyyy/MM/dd}" : "";
                string printDateText = $"تاريخ الطباعة: {DateTime.Now:yyyy/MM/dd hh:mm tt}";

                g.DrawString(periodText, fSub, Brushes.Black, new RectangleF(m.Right - 400, y, 400, 25), sfRight);
                g.DrawString(printDateText, fSub, Brushes.Black, new RectangleF(m.Left, y, 400, 25), new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
                y += 30;

                // Table Header
                float x = m.Right;
                float headerHeight = 35;

                foreach (var col in cols)
                {
                    float w = m.Width * col.Width / totalW;
                    RectangleF rect = new RectangleF(x - w, y, w, headerHeight);
                    g.FillRectangle(Brushes.LightGray, rect);
                    g.DrawRectangle(Pens.Black, Rectangle.Round(rect));
                    g.DrawString(col.HeaderText, fHeader, Brushes.Black, rect, sfCenter);
                    x -= w;
                }
                y += headerHeight;

                // Table Rows
                const float rowH = 30;
                int bottomLimit = m.Bottom - 60; // Leave space for footer

                while (printRowIndex < dgvReport.Rows.Count && y + rowH <= bottomLimit)
                {
                    DataGridViewRow row = dgvReport.Rows[printRowIndex];
                    x = m.Right;

                    foreach (var col in cols)
                    {
                        float w = m.Width * col.Width / totalW;
                        RectangleF rect = new RectangleF(x - w, y, w, rowH);

                        if (printRowIndex % 2 == 1)
                            g.FillRectangle(Brushes.WhiteSmoke, rect);

                        g.DrawRectangle(penThin, Rectangle.Round(rect));

                        object cellVal = row.Cells[col.Index].FormattedValue;
                        string text = cellVal == null ? "" : cellVal.ToString();

                        StringFormat currentSf = sfCenter;
                        RectangleF textRect = currentSf == sfRight ? new RectangleF(rect.X, rect.Y, rect.Width - 5, rect.Height) : rect;

                        g.DrawString(text, fRow, Brushes.Black, textRect, currentSf);
                        x -= w;
                    }
                    y += rowH;
                    printRowIndex++;
                }

                // Page Footer
                g.DrawLine(penThick, m.Left, y + 10, m.Right, y + 10);

                if (printRowIndex >= dgvReport.Rows.Count) // End of report
                {
                    // طباعة الإجمالي فقط لو المستخدم اختار "نعم"
                    if (printWithTotal && !string.IsNullOrWhiteSpace(lblTotal.Text))
                    {
                        g.DrawString(lblTotal.Text, new Font("Segoe UI", 12, FontStyle.Bold), Brushes.Black, new RectangleF(m.Left, y + 20, m.Width, 30), sfRight);
                    }
                }

                g.DrawString($"صفحة {printPageNumber}", fSub, Brushes.Gray, new RectangleF(m.Left, m.Bottom - 20, m.Width, 20), sfCenter);
            }

            e.HasMorePages = printRowIndex < dgvReport.Rows.Count;
        }
    }
}