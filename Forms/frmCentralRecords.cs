using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using MajesticDesktop.Models;
using MajesticDesktop.Services;

namespace MajesticDesktop.Forms
{
    public partial class frmCentralRecords : Form
    {
        // ===================== ألوان وثيم الشاشة الموحد =====================
        private static readonly Color ClrBgDark = Color.FromArgb(15, 23, 42);
        private static readonly Color ClrBgPanel = Color.FromArgb(30, 41, 59);
        private static readonly Color ClrBgInput = Color.FromArgb(15, 23, 42);
        private static readonly Color ClrCard = Color.FromArgb(24, 34, 51);
        private static readonly Color ClrText = Color.FromArgb(241, 245, 249);
        private static readonly Color ClrTextMuted = Color.FromArgb(148, 163, 184);

        private static readonly Color ClrBlue = Color.FromArgb(59, 130, 246);
        private static readonly Color ClrBlueHover = Color.FromArgb(37, 99, 235);
        private static readonly Color ClrRed = Color.FromArgb(220, 38, 38);
        private static readonly Color ClrGreen = Color.FromArgb(22, 163, 74);

        // خطوط كبيرة وواضحة
        private static readonly Font FontTitle = new Font("Segoe UI", 24F, FontStyle.Bold);
        private static readonly Font FontSubtitle = new Font("Segoe UI", 12F, FontStyle.Regular);
        private static readonly Font FontLabel = new Font("Segoe UI", 12.5F, FontStyle.Bold);
        private static readonly Font FontInput = new Font("Segoe UI", 13.5F, FontStyle.Bold);
        private static readonly Font FontButton = new Font("Segoe UI", 14F, FontStyle.Bold);

        private SalesService salesService;
        private PurchaseService purchaseService;
        private LocationService locationService;
        private string currentRecordType = "Sales";

        public frmCentralRecords()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            salesService = new SalesService();
            purchaseService = new PurchaseService();
            locationService = new LocationService();

            BuildSolidUI();
            EnableDoubleBuffering(dgvRecords);
        }

        private void EnableDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, dgv, new object[] { true });
        }

        private static void Fail(Exception ex)
        {
            MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        }

        private void frmCentralRecords_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                LoadLocations();
                dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                dtpTo.Value = DateTime.Now.Date;
                btnTabSales.PerformClick();
            }
            catch (Exception ex)
            {
                Fail(ex);
            }
        }

        #region Solid UI Builder (Fixed Centering & Colors)

        private void BuildSolidUI()
        {
            this.Controls.Clear();
            this.BackColor = ClrBgDark;
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;

            // ================= 1. Top Tabs =================
            pnlTopTabs.BackColor = Color.FromArgb(9, 14, 28);
            pnlTopTabs.Height = 70;
            pnlTopTabs.Dock = DockStyle.Top;
            pnlTopTabs.Padding = new Padding(15, 10, 15, 0);

            tlpTabs.Dock = DockStyle.Fill;
            tlpTabs.RightToLeft = RightToLeft.Yes;
            tlpTabs.ColumnCount = 2;
            tlpTabs.ColumnStyles.Clear();
            tlpTabs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpTabs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpTabs.RowCount = 1;
            tlpTabs.RowStyles.Clear();
            tlpTabs.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            StyleTabButton(btnTabSales, "🛒 سجلات المبيعات");
            StyleTabButton(btnTabPurchases, "📦 سجلات المشتريات");

            tlpTabs.Controls.Clear();
            tlpTabs.Controls.Add(btnTabSales, 0, 0);
            tlpTabs.Controls.Add(btnTabPurchases, 1, 0);
            pnlTopTabs.Controls.Add(tlpTabs);

            // ================= 2. Main Title Header (Fixed Centering) =================
            pnlHeader.BackColor = Color.FromArgb(9, 14, 28);
            pnlHeader.Height = 90;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Padding = new Padding(0);

            // 💡 السر هنا: إيقاف الـ AutoSize عشان الـ Label ياخد عرض الشاشة بالكامل ويتوسطن صح
            lblTitle.AutoSize = false;
            lblTitle.Font = FontTitle;
            lblTitle.ForeColor = ClrText;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 50;
            lblTitle.TextAlign = ContentAlignment.BottomCenter;
            lblTitle.Text = "السجلات المركزية للنظام";

            lblSubtitle.AutoSize = false;
            lblSubtitle.Font = FontSubtitle;
            lblSubtitle.ForeColor = ClrTextMuted;
            lblSubtitle.Dock = DockStyle.Top;
            lblSubtitle.Height = 35;
            lblSubtitle.TextAlign = ContentAlignment.TopCenter;
            lblSubtitle.Text = "استعراض والبحث في فواتير النظام مع إمكانية التعديل والمراجعة";

            pnlHeader.Controls.Clear();
            pnlHeader.Controls.Add(lblSubtitle);
            pnlHeader.Controls.Add(lblTitle);

            // Spacer
            pnlSpacer1.Height = 15;
            pnlSpacer1.BackColor = ClrBgDark;
            pnlSpacer1.Dock = DockStyle.Top;

            // ================= 3. Filter Area =================
            pnlFilter.BackColor = ClrBgPanel;
            pnlFilter.Height = 110;
            pnlFilter.Dock = DockStyle.Top;
            pnlFilter.Padding = new Padding(15);
            pnlFilter.RightToLeft = RightToLeft.Yes;

            pnlFilter.ColumnCount = 5;
            pnlFilter.ColumnStyles.Clear();
            pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            pnlFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            pnlFilter.RowCount = 1;
            pnlFilter.RowStyles.Clear();
            pnlFilter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            pnlFilter.Controls.Clear();
            pnlFilter.Controls.Add(CreateFilterField("الفرع:", cmbLocation), 0, 0);
            pnlFilter.Controls.Add(CreateFilterField("من تاريخ:", dtpFrom), 1, 0);
            pnlFilter.Controls.Add(CreateFilterField("إلى تاريخ:", dtpTo), 2, 0);
            pnlFilter.Controls.Add(CreateFilterField("بحث بالاسم أو رقم الفاتورة:", txtCustomerSearch), 3, 0);

            btnSearch.Dock = DockStyle.Fill;
            btnSearch.BackColor = ClrGreen;
            btnSearch.ForeColor = Color.White;
            btnSearch.Font = FontButton;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Text = "تحديث 🔄";
            btnSearch.Margin = new Padding(10, 28, 0, 15);
            pnlFilter.Controls.Add(btnSearch, 4, 0);

            // Spacer
            pnlSpacer2.Height = 15;
            pnlSpacer2.BackColor = ClrBgDark;
            pnlSpacer2.Dock = DockStyle.Top;

            // ================= 4. Grid Setup =================
            pnlGrid.BackColor = ClrBgDark;
            pnlGrid.Padding = new Padding(15, 0, 15, 0);
            pnlGrid.Dock = DockStyle.Fill;

            dgvRecords.AllowUserToAddRows = false;
            dgvRecords.Dock = DockStyle.Fill;

            dgvRecords.Columns.Clear();
            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", Visible = false });
            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn { Name = "colInvoiceNo", HeaderText = "رقم الفاتورة", FillWeight = 90F });
            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDate", HeaderText = "التاريخ", FillWeight = 80F });
            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCustomer", HeaderText = "العميل", FillWeight = 150F });
            dgvRecords.Columns.Add(new DataGridViewTextBoxColumn { Name = "colLocation", HeaderText = "الفرع", FillWeight = 110F });

            var colTotalObj = new DataGridViewTextBoxColumn { Name = "colTotal", HeaderText = "الإجمالي", FillWeight = 90F };
            colTotalObj.DefaultCellStyle.Format = "N2";
            dgvRecords.Columns.Add(colTotalObj);

            var colDiscountObj = new DataGridViewTextBoxColumn { Name = "colDiscount", HeaderText = "الخصم", FillWeight = 70F };
            colDiscountObj.DefaultCellStyle.Format = "N2";
            dgvRecords.Columns.Add(colDiscountObj);

            var colNetObj = new DataGridViewTextBoxColumn { Name = "colNet", HeaderText = "الصافي", FillWeight = 90F };
            colNetObj.DefaultCellStyle.Format = "N2";
            colNetObj.DefaultCellStyle.ForeColor = Color.FromArgb(96, 165, 250);
            colNetObj.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dgvRecords.Columns.Add(colNetObj);

            var colPaidObj = new DataGridViewTextBoxColumn { Name = "colPaid", HeaderText = "المدفوع", FillWeight = 90F };
            colPaidObj.DefaultCellStyle.Format = "N2";
            colPaidObj.DefaultCellStyle.ForeColor = Color.FromArgb(134, 239, 172);
            colPaidObj.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dgvRecords.Columns.Add(colPaidObj);

            var colRemainingObj = new DataGridViewTextBoxColumn { Name = "colRemaining", HeaderText = "المتبقي", FillWeight = 90F };
            colRemainingObj.DefaultCellStyle.Format = "N2";
            colRemainingObj.DefaultCellStyle.ForeColor = Color.FromArgb(248, 113, 113);
            colRemainingObj.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dgvRecords.Columns.Add(colRemainingObj);

            var colPaymentMethodObj = new DataGridViewTextBoxColumn { Name = "colPaymentMethod", HeaderText = "طريقة الدفع", FillWeight = 110F };
            colPaymentMethodObj.DefaultCellStyle.ForeColor = Color.FromArgb(253, 224, 71);
            dgvRecords.Columns.Add(colPaymentMethodObj);

            pnlGrid.Controls.Clear();
            pnlGrid.Controls.Add(dgvRecords);

            ApplyGridStyle();

            // ================= 5. Footer =================
            pnlFooter.BackColor = Color.FromArgb(9, 14, 28);
            pnlFooter.Height = 90;
            pnlFooter.Padding = new Padding(15, 15, 15, 15);
            pnlFooter.Dock = DockStyle.Bottom;

            btnViewInvoice.Dock = DockStyle.Fill;
            btnViewInvoice.BackColor = ClrBlue;
            btnViewInvoice.Cursor = Cursors.Hand;
            btnViewInvoice.FlatStyle = FlatStyle.Flat;
            btnViewInvoice.FlatAppearance.BorderSize = 0;
            btnViewInvoice.FlatAppearance.MouseOverBackColor = ClrBlueHover;
            btnViewInvoice.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnViewInvoice.ForeColor = Color.White;
            btnViewInvoice.Text = "عرض وتعديل الفاتورة المحددة";

            pnlFooter.Controls.Clear();
            pnlFooter.Controls.Add(btnViewInvoice);

            // ================= 6. Z-Order Addition =================
            this.Controls.Add(pnlGrid);
            this.Controls.Add(pnlFooter);
            this.Controls.Add(pnlSpacer2);
            this.Controls.Add(pnlFilter);
            this.Controls.Add(pnlSpacer1);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlTopTabs);
        }

        private Panel CreateFilterField(string labelText, Control inputControl)
        {
            Panel wrap = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Padding = new Padding(5, 10, 5, 5) };

            Label lbl = new Label { Text = labelText, ForeColor = ClrTextMuted, Font = FontLabel, Dock = DockStyle.Top, Height = 25, TextAlign = ContentAlignment.BottomRight };

            inputControl.Dock = DockStyle.Bottom;
            inputControl.Height = 35;
            inputControl.Font = FontInput;

            if (inputControl is ComboBox cmb)
            {
                cmb.BackColor = ClrBgInput;
                cmb.ForeColor = ClrText;
                cmb.FlatStyle = FlatStyle.Flat;
                cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            else if (inputControl is TextBox txt)
            {
                txt.BackColor = ClrBgInput;
                txt.ForeColor = ClrText;
                txt.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (inputControl is DateTimePicker dtp)
            {
                dtp.Format = DateTimePickerFormat.Short;
                dtp.RightToLeft = RightToLeft.Yes;
                dtp.RightToLeftLayout = true;
            }

            wrap.Controls.Add(inputControl);
            wrap.Controls.Add(lbl);
            return wrap;
        }

        private void ApplyGridStyle()
        {
            dgvRecords.BackgroundColor = ClrBgDark;
            dgvRecords.BorderStyle = BorderStyle.None;
            dgvRecords.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRecords.EnableHeadersVisualStyles = false;
            dgvRecords.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // 💡 تم تغيير لون الهيدر للون كحلي غامق مميز (Color.FromArgb(20, 30, 45))
            dgvRecords.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 30, 45);
            dgvRecords.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRecords.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12.5F, FontStyle.Bold);
            dgvRecords.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRecords.ColumnHeadersHeight = 55;

            dgvRecords.DefaultCellStyle.BackColor = ClrBgPanel;
            dgvRecords.DefaultCellStyle.ForeColor = ClrText;
            dgvRecords.DefaultCellStyle.SelectionBackColor = ClrBlueHover;
            dgvRecords.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvRecords.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRecords.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);

            dgvRecords.AlternatingRowsDefaultCellStyle.BackColor = ClrCard;
            dgvRecords.RowTemplate.Height = 50;
            dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecords.ReadOnly = true;
            dgvRecords.RowHeadersVisible = false;
            dgvRecords.MultiSelect = false;
            dgvRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecords.Cursor = Cursors.Hand;
            dgvRecords.RightToLeft = RightToLeft.Yes;
        }

        private static void StyleTabButton(Button b, string text)
        {
            b.Dock = DockStyle.Fill;
            b.Margin = new Padding(2, 0, 2, 0);
            b.BackColor = ClrBgPanel;
            b.ForeColor = ClrText;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            b.Text = text;
        }

        #endregion

        #region Logic & Events

        private void SelectTabButton(Button btnActive, string recordType)
        {
            btnTabSales.BackColor = ClrBgPanel;
            btnTabPurchases.BackColor = ClrBgPanel;

            btnActive.BackColor = ClrBlueHover;
            currentRecordType = recordType;

            if (recordType == "Sales")
            {
                if (dgvRecords.Columns.Contains("colCustomer")) dgvRecords.Columns["colCustomer"].HeaderText = "العميل";
                if (dgvRecords.Columns.Contains("colDiscount")) dgvRecords.Columns["colDiscount"].Visible = true;
                lblTitle.Text = "السجلات المركزية (إدارة المبيعات)";
                btnViewInvoice.Text = "عرض وتعديل فاتورة البيع المحددة";
            }
            else
            {
                if (dgvRecords.Columns.Contains("colCustomer")) dgvRecords.Columns["colCustomer"].HeaderText = "المورد";
                if (dgvRecords.Columns.Contains("colDiscount")) dgvRecords.Columns["colDiscount"].Visible = false;
                lblTitle.Text = "السجلات المركزية (إدارة المشتريات)";
                btnViewInvoice.Text = "عرض فاتورة الشراء المحددة";
            }

            UpdateFilterLabels(recordType);
            txtCustomerSearch.Clear();
            SearchRecords();
        }

        private void UpdateFilterLabels(string recordType)
        {
            foreach (Control wrapPanel in pnlFilter.Controls)
            {
                if (wrapPanel is Panel)
                {
                    foreach (Control ctrl in wrapPanel.Controls)
                    {
                        if (ctrl is Label lbl && lbl.Text.Contains("بحث"))
                        {
                            lbl.Text = recordType == "Sales" ? "بحث باسم العميل:" : "بحث باسم المورد:";
                        }
                    }
                }
            }
        }

        private void btnTabSales_Click(object sender, EventArgs e) => SelectTabButton(btnTabSales, "Sales");
        private void btnTabPurchases_Click(object sender, EventArgs e) => SelectTabButton(btnTabPurchases, "Purchases");

        private void LoadLocations()
        {
            try
            {
                var locationsList = locationService.GetAllLocations() ?? new List<Location>();
                locationsList.Insert(0, new Location { Id = 0, Name = "--- كل الفروع ---" });

                cmbLocation.DataSource = locationsList;
                cmbLocation.DisplayMember = "Name";
                cmbLocation.ValueMember = "Id";
                cmbLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Fail(ex);
            }
        }

        private void SearchRecords()
        {
            try
            {
                int? locationId = null;
                if (cmbLocation.SelectedValue != null && Convert.ToInt32(cmbLocation.SelectedValue) > 0)
                {
                    locationId = Convert.ToInt32(cmbLocation.SelectedValue);
                }

                DateTime fromDate = dtpFrom.Value.Date;
                DateTime toDate = dtpTo.Value.Date.AddDays(1).AddTicks(-1);
                string searchName = txtCustomerSearch.Text.Trim();

                DataTable records = currentRecordType == "Sales"
                    ? salesService.GetSalesRecords(locationId, fromDate, toDate, searchName)
                    : purchaseService.GetPurchaseRecords(locationId, fromDate, toDate, searchName);

                dgvRecords.Rows.Clear();

                if (records != null)
                {
                    foreach (DataRow row in records.Rows)
                    {
                        int r = dgvRecords.Rows.Add();
                        dgvRecords.Rows[r].Cells["colId"].Value = row["Id"];
                        dgvRecords.Rows[r].Cells["colInvoiceNo"].Value = row["InvoiceNumber"];
                        dgvRecords.Rows[r].Cells["colDate"].Value = Convert.ToDateTime(row["InvoiceDate"]).ToShortDateString();

                        if (currentRecordType == "Sales")
                            dgvRecords.Rows[r].Cells["colCustomer"].Value = row["CustomerName"];
                        else
                            dgvRecords.Rows[r].Cells["colCustomer"].Value = row["SupplierName"];

                        dgvRecords.Rows[r].Cells["colLocation"].Value = row["LocationName"];
                        dgvRecords.Rows[r].Cells["colTotal"].Value = Convert.ToDecimal(row["TotalAmount"]);

                        if (currentRecordType == "Sales")
                            dgvRecords.Rows[r].Cells["colDiscount"].Value = Convert.ToDecimal(row["Discount"]);
                        else
                            dgvRecords.Rows[r].Cells["colDiscount"].Value = 0;

                        dgvRecords.Rows[r].Cells["colNet"].Value = Convert.ToDecimal(row["NetAmount"]);
                        dgvRecords.Rows[r].Cells["colPaid"].Value = Convert.ToDecimal(row["PaidAmount"]);
                        dgvRecords.Rows[r].Cells["colRemaining"].Value = Convert.ToDecimal(row["RemainingAmount"]);
                        dgvRecords.Rows[r].Cells["colPaymentMethod"].Value = row["PaymentMethod"];
                    }
                }
            }
            catch (Exception ex)
            {
                Fail(ex);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e) => SearchRecords();

        private void OpenInvoiceView()
        {
            if (dgvRecords.SelectedRows.Count > 0)
            {
                if (int.TryParse(dgvRecords.SelectedRows[0].Cells["colId"].Value?.ToString(), out int invoiceId))
                {
                    if (currentRecordType == "Sales")
                    {
                        using (frmSalesInvoice frm = new frmSalesInvoice(invoiceId, false))
                        {
                            frm.ShowDialog();
                        }
                        SearchRecords();
                    }
                    else
                    {
                        MessageBox.Show("تم تصميم الشاشة حالياً لعرض المبيعات، سيتم إتاحة المشتريات قريباً.", "إشعار", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("من فضلك اختر فاتورة من الجدول أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnViewInvoice_Click(object sender, EventArgs e) => OpenInvoiceView();

        private void dgvRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) OpenInvoiceView();
        }

        #endregion
    }
}