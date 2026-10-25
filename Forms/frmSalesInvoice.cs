using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks; // هامة لعمليات Async
using System.Windows.Forms;
using MajesticDesktop.Models;
using MajesticDesktop.Services;

namespace MajesticDesktop.Forms
{
    public partial class frmSalesInvoice : Form
    {
        // ===================== ألوان الشاشة =====================
        private static readonly Color ClrBgDark = Color.FromArgb(26, 32, 44);
        private static readonly Color ClrBgPanel = Color.FromArgb(28, 41, 60);
        private static readonly Color ClrBgInput = Color.FromArgb(35, 50, 70);
        private static readonly Color ClrCard = Color.FromArgb(24, 34, 48);
        private static readonly Color ClrBgGrid = Color.FromArgb(22, 31, 43);
        private static readonly Color ClrBgGridAlt = Color.FromArgb(26, 36, 50);
        private static readonly Color ClrText = Color.White;
        private static readonly Color ClrTextMuted = Color.FromArgb(178, 190, 195);

        private static readonly Color ClrBlue = Color.FromArgb(52, 152, 219);
        private static readonly Color ClrBlueHover = Color.FromArgb(93, 173, 226);
        private static readonly Color ClrPurple = Color.FromArgb(155, 89, 182);
        private static readonly Color ClrPurpleHover = Color.FromArgb(175, 122, 197);
        private static readonly Color ClrGreen = Color.FromArgb(46, 204, 113);
        private static readonly Color ClrGreenHover = Color.FromArgb(64, 212, 126);
        private static readonly Color ClrRed = Color.FromArgb(231, 76, 60);
        private static readonly Color ClrOrange = Color.FromArgb(230, 126, 34);

        private static readonly Font FontLabel = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        private static readonly Font FontInput = new Font("Segoe UI", 12F, FontStyle.Bold);
        private static readonly Font FontButton = new Font("Segoe UI", 12F, FontStyle.Bold);

        // ===================== بيانات الشركة =====================
        private new const string CompanyName = "ماجيستيك - Majestic";
        private const string CompanyPhone = "01000000000";

        // ===================== الخدمات =====================
        private SalesService salesService;
        private ProductService productService;
        private LocationService locationService;
        private List<Product> availableProducts;
        private int _currentInvoiceId = 0;
        private int _currentUserLocationId = 1;
        private bool _isViewOnly = false;

        private int _printRowIndex = 0;
        private int _printPageNumber = 0;

        public frmSalesInvoice()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            salesService = new SalesService();
            productService = new ProductService();
            locationService = new LocationService();

            BuildDynamicUI();
            BuildGridColumnsAndStyle();
            SetupTextBoxPlaceholders();
            EnableDoubleBuffering(dgvDetails);
        }

        public frmSalesInvoice(int invoiceId, bool isViewOnly = false) : this()
        {
            if (invoiceId > 0)
            {
                _currentInvoiceId = invoiceId;
                _isViewOnly = isViewOnly;
            }
        }

        private void EnableDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgv, new object[] { true });
        }

        // التعديل: تحويل الـ Load لـ Async
        private async void frmSalesInvoice_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            try
            {
                LoadInitialData();

                if (_currentInvoiceId > 0)
                {
                    await LoadInvoiceDataByIdAsync(_currentInvoiceId);
                    if (_isViewOnly) ApplyViewOnlyMode();
                    else
                    {
                        btnSave.Text = "تعديل الفاتورة ✔";
                        btnSave.BackColor = ClrOrange;
                    }
                }
                else
                {
                    ResetInvoice();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل الشاشة:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // التعديل الثالث: دالة مساعدة لضمان قراءة الكسور العشرية بشكل سليم في أي جهاز
        private decimal ParseSafeDecimal(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            // استبدال أي فاصلة بنقطة لضمان التوحيد، ثم التحويل باستخدام InvariantCulture
            string sanitized = text.Replace(",", ".");
            decimal.TryParse(sanitized, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result);
            return result;
        }

        // ============================================================
        // أدوات التصميم الديناميكي (Responsive Tools)
        // ============================================================
        private TableLayoutPanel CreateGridRow(float[] columnWidths, int height)
        {
            var tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = height,
                RowCount = 1,
                ColumnCount = columnWidths.Length,
                RightToLeft = RightToLeft.Yes,
                Padding = new Padding(5),
                BackColor = Color.Transparent
            };
            for (int i = 0; i < columnWidths.Length; i++)
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, columnWidths[i]));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            return tlp;
        }

        private Panel CreateField(string labelText, Control input, Color? highlightColor = null)
        {
            Panel wrap = new Panel { Dock = DockStyle.Fill, Margin = new Padding(5), BackColor = ClrCard, RightToLeft = RightToLeft.Yes, Padding = new Padding(10, 8, 10, 8) };
            Label lbl = new Label { Text = labelText, ForeColor = highlightColor ?? ClrTextMuted, Font = FontLabel, Dock = DockStyle.Top, Height = 24, TextAlign = ContentAlignment.MiddleRight, RightToLeft = RightToLeft.Yes };
            input.Height = 35; input.Dock = DockStyle.Top;
            wrap.Controls.Add(input); wrap.Controls.Add(lbl);
            input.BringToFront(); return wrap;
        }

        private TextBox CreateTextBox(bool isReadOnly = false, Color? foreColor = null, HorizontalAlignment align = HorizontalAlignment.Center)
        {
            return new TextBox { Font = FontInput, BackColor = ClrBgInput, ForeColor = foreColor ?? ClrText, BorderStyle = BorderStyle.FixedSingle, RightToLeft = RightToLeft.Yes, TextAlign = align, ReadOnly = isReadOnly };
        }

        private ComboBox CreateSearchableCombo()
        {
            return new ComboBox { Font = FontInput, BackColor = ClrBgInput, ForeColor = ClrText, FlatStyle = FlatStyle.Flat, DropDownStyle = ComboBoxStyle.DropDown, AutoCompleteMode = AutoCompleteMode.SuggestAppend, AutoCompleteSource = AutoCompleteSource.ListItems, RightToLeft = RightToLeft.Yes };
        }

        private Button CreateActionButton(string text, Color normal, Color hover)
        {
            Button b = new Button { Text = text, Dock = DockStyle.Fill, BackColor = normal, ForeColor = ClrText, Font = FontButton, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Margin = new Padding(5, 15, 5, 5), RightToLeft = RightToLeft.Yes, TextAlign = ContentAlignment.MiddleCenter };
            b.FlatAppearance.BorderSize = 0;
            b.MouseEnter += (s, e) => b.BackColor = hover; b.MouseLeave += (s, e) => b.BackColor = normal;
            return b;
        }

        private void IntegerOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void DecimalOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            // التعديل: السماح بالنقطة والفاصلة لعدم تقييد المستخدم بناءً على لغة جهازه
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
                e.Handled = true;

            TextBox tb = sender as TextBox;
            if (tb != null && (e.KeyChar == '.' || e.KeyChar == ',') && (tb.Text.Contains(".") || tb.Text.Contains(",")))
                e.Handled = true;
        }

        private void BuildDynamicUI()
        {
            this.BackColor = ClrBgDark; this.Padding = new Padding(15);

            Panel pnlInfo = new Panel { BackColor = ClrBgPanel, Dock = DockStyle.Top, AutoSize = true, RightToLeft = RightToLeft.Yes, Padding = new Padding(10) };
            var infoRow = CreateGridRow(new float[] { 25f, 25f, 10f, 20f, 20f }, 90);

            cmbLocation = CreateSearchableCombo(); cmbLocation.SelectedIndexChanged += cmbLocation_SelectedIndexChanged;
            cmbCustomer = CreateSearchableCombo();
            btnAddCustomer = CreateActionButton("عميل ➕", ClrPurple, ClrPurpleHover); btnAddCustomer.Click += btnAddCustomer_Click;
            txtInvoiceNo = CreateTextBox(true, ClrBlue);

            Panel pnlDate = new Panel { Dock = DockStyle.Fill, Margin = new Padding(5), BackColor = ClrCard, Padding = new Padding(10, 8, 10, 8) };
            chkManualDate = new CheckBox { Text = "تاريخ يدوي", ForeColor = ClrTextMuted, Font = FontLabel, Dock = DockStyle.Top, Height = 24, RightToLeft = RightToLeft.Yes };
            dtpInvoiceDate = new DateTimePicker { Dock = DockStyle.Top, Height = 35, Font = FontInput, Format = DateTimePickerFormat.Short, Enabled = false, RightToLeft = RightToLeft.Yes };
            pnlDate.Controls.Add(dtpInvoiceDate); pnlDate.Controls.Add(chkManualDate); dtpInvoiceDate.BringToFront();

            chkManualDate.CheckedChanged += (s, ev) =>
            {
                dtpInvoiceDate.Enabled = chkManualDate.Checked;
                if (!chkManualDate.Checked) dtpInvoiceDate.Value = DateTime.Now;
            };

            infoRow.Controls.Add(CreateField("المكان / الفرع", cmbLocation), 0, 0);
            infoRow.Controls.Add(CreateField("اسم العميل", cmbCustomer), 1, 0);
            infoRow.Controls.Add(btnAddCustomer, 2, 0);
            infoRow.Controls.Add(CreateField("رقم الفاتورة", txtInvoiceNo, ClrBlue), 3, 0);
            infoRow.Controls.Add(pnlDate, 4, 0);
            pnlInfo.Controls.Add(infoRow);

            Panel pnlEntry = new Panel { BackColor = ClrBgPanel, Dock = DockStyle.Top, AutoSize = true, RightToLeft = RightToLeft.Yes, Padding = new Padding(10), Margin = new Padding(0, 10, 0, 10) };
            var entryRow = CreateGridRow(new float[] { 30f, 10f, 15f, 15f, 10f, 20f }, 90);

            cmbProduct = CreateSearchableCombo(); cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;
            txtAvailableQty = CreateTextBox(true, ClrOrange);
            txtQuantity = CreateTextBox(); txtQuantity.KeyPress += IntegerOnly_KeyPress;
            txtUnitPrice = CreateTextBox(); txtUnitPrice.KeyPress += DecimalOnly_KeyPress;
            txtDiscount = CreateTextBox(); txtDiscount.KeyPress += DecimalOnly_KeyPress; txtDiscount.TextChanged += txtDiscount_TextChanged;
            btnAddProduct = CreateActionButton("إضافة للفاتورة ⬇", ClrGreen, ClrGreenHover); btnAddProduct.Click += btnAddProduct_Click;

            entryRow.Controls.Add(CreateField("المنتج *", cmbProduct), 0, 0);
            entryRow.Controls.Add(CreateField("المتاح", txtAvailableQty, ClrOrange), 1, 0);
            entryRow.Controls.Add(CreateField("الكمية *", txtQuantity), 2, 0);
            entryRow.Controls.Add(CreateField("سعر البيع *", txtUnitPrice), 3, 0);
            entryRow.Controls.Add(CreateField("الخصم", txtDiscount), 4, 0);
            entryRow.Controls.Add(btnAddProduct, 5, 0);
            pnlEntry.Controls.Add(entryRow);

            Panel pnlBottom = new Panel { BackColor = ClrBgPanel, Dock = DockStyle.Bottom, AutoSize = true, RightToLeft = RightToLeft.Yes, Padding = new Padding(10) };
            var totalsRow = CreateGridRow(new float[] { 15f, 15f, 15f, 15f, 40f }, 95);
            txtTotalAmount = CreateTextBox(true, ClrText);
            txtNetAmount = CreateTextBox(true, ClrBlue);
            txtPaidAmount = CreateTextBox(false, ClrGreen); txtPaidAmount.TextChanged += txtPaidAmount_TextChanged; txtPaidAmount.KeyPress += DecimalOnly_KeyPress;
            txtRemainingAmount = CreateTextBox(true, ClrRed);
            cmbPaymentMethod = CreateSearchableCombo(); cmbPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;

            totalsRow.Controls.Add(CreateField("الإجمالي", txtTotalAmount), 0, 0);
            totalsRow.Controls.Add(CreateField("الصافي المطلوب", txtNetAmount, ClrBlue), 1, 0);
            totalsRow.Controls.Add(CreateField("المدفوع", txtPaidAmount, ClrGreen), 2, 0);
            totalsRow.Controls.Add(CreateField("المتبقي", txtRemainingAmount, ClrRed), 3, 0);
            totalsRow.Controls.Add(CreateField("طريقة الدفع", cmbPaymentMethod), 4, 0);

            var footerRow = CreateGridRow(new float[] { 50f, 10f, 15f, 25f }, 95);
            txtNotes = CreateTextBox(false, ClrText, HorizontalAlignment.Right);
            btnShare = CreateActionButton("مشاركة 🔗", Color.FromArgb(39, 174, 96), Color.FromArgb(46, 204, 113)); btnShare.Click += btnShare_Click;
            btnOptions = CreateActionButton("خيارات ⚙", Color.FromArgb(52, 73, 94), Color.FromArgb(100, 116, 139)); btnOptions.Click += btnOptions_Click;
            btnSave = CreateActionButton("حفظ الفاتورة ✔", ClrBlue, ClrBlueHover); btnSave.Click += btnSave_Click;

            footerRow.Controls.Add(CreateField("ملاحظات الفاتورة", txtNotes), 0, 0);
            footerRow.Controls.Add(btnShare, 1, 0);
            footerRow.Controls.Add(btnOptions, 2, 0);
            footerRow.Controls.Add(btnSave, 3, 0);

            pnlBottom.Controls.Add(footerRow); pnlBottom.Controls.Add(totalsRow);

            dgvDetails = new DataGridView { Dock = DockStyle.Fill };
            dgvDetails.CellDoubleClick += dgvDetails_CellDoubleClick;
            dgvDetails.CellContentClick += dgvDetails_CellContentClick;
            Panel pnlGridWrap = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 0, 10) };
            pnlGridWrap.Controls.Add(dgvDetails);

            this.Controls.Add(pnlGridWrap); this.Controls.Add(pnlBottom); this.Controls.Add(pnlEntry); this.Controls.Add(pnlInfo);
        }

        private void BuildGridColumnsAndStyle()
        {
            var styleHeaderBlue = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, BackColor = ClrBlue, Font = new Font("Segoe UI", 11.5F, FontStyle.Bold), ForeColor = Color.White, SelectionBackColor = ClrBlue };
            var styleRows = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, BackColor = ClrBgGrid, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.White, SelectionBackColor = ClrGreen };
            var styleAlt = new DataGridViewCellStyle { BackColor = ClrBgGridAlt, ForeColor = Color.White };
            var styleDelBtn = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, BackColor = ClrRed, ForeColor = Color.White };

            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProductId", Visible = false });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProductName", HeaderText = "اسم المنتج" });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "colQuantity", HeaderText = "الكمية" });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "colUnitPrice", HeaderText = "السعر" });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTotalPrice", HeaderText = "الإجمالي" });
            dgvDetails.Columns.Add(new DataGridViewButtonColumn { Name = "colDelete", HeaderText = "حذف", Text = "✖", UseColumnTextForButtonValue = true, DefaultCellStyle = styleDelBtn, FlatStyle = FlatStyle.Flat, Width = 80 });

            dgvDetails.RightToLeft = RightToLeft.Yes; dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetails.BackgroundColor = ClrBgGrid; dgvDetails.BorderStyle = BorderStyle.None;
            dgvDetails.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None; dgvDetails.ColumnHeadersDefaultCellStyle = styleHeaderBlue;
            dgvDetails.ColumnHeadersHeight = 46; dgvDetails.DefaultCellStyle = styleRows; dgvDetails.AlternatingRowsDefaultCellStyle = styleAlt;
            dgvDetails.EnableHeadersVisualStyles = false; dgvDetails.RowHeadersVisible = false; dgvDetails.RowTemplate.Height = 42;
            dgvDetails.AllowUserToAddRows = false; dgvDetails.ReadOnly = true; dgvDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private class CustomMenuRenderer : ToolStripProfessionalRenderer { public CustomMenuRenderer() : base(new CustomColorTable()) { } }
        private class CustomColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected => Color.FromArgb(52, 73, 94);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(52, 73, 94);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(52, 73, 94);
            public override Color MenuItemBorder => Color.Transparent;
            public override Color ToolStripDropDownBackground => Color.FromArgb(35, 50, 70);
            public override Color ImageMarginGradientBegin => Color.FromArgb(35, 50, 70);
            public override Color ImageMarginGradientMiddle => Color.FromArgb(35, 50, 70);
            public override Color ImageMarginGradientEnd => Color.FromArgb(35, 50, 70);
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Renderer = new CustomMenuRenderer();
            menu.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            menu.RightToLeft = RightToLeft.Yes;
            menu.ShowImageMargin = false;

            // التعديل الثاني: منع الـ Memory Leak بتدمير القائمة بعد إغلاقها
            menu.Closed += (s, ev) => menu.Dispose();

            ToolStripMenuItem itemPrint = new ToolStripMenuItem("🖨️ طباعة الفاتورة (مباشرة)"); itemPrint.ForeColor = ClrBlue;
            itemPrint.Padding = new Padding(10, 5, 10, 5); itemPrint.Click += (s, ev) => PrintInvoice(false);

            ToolStripMenuItem itemPdf = new ToolStripMenuItem("📄 حفظ كملف PDF"); itemPdf.ForeColor = ClrRed;
            itemPdf.Padding = new Padding(10, 5, 10, 5); itemPdf.Click += (s, ev) => PrintInvoice(true);

            ToolStripMenuItem itemExcel = new ToolStripMenuItem("📊 تصدير شيت إكسيل (CSV)"); itemExcel.ForeColor = ClrGreen;
            itemExcel.Padding = new Padding(10, 5, 10, 5); itemExcel.Click += (s, ev) => ExportToCsv();

            menu.Items.Add(itemPrint); menu.Items.Add(new ToolStripSeparator { BackColor = Color.Gray });
            menu.Items.Add(itemPdf); menu.Items.Add(new ToolStripSeparator { BackColor = Color.Gray });
            menu.Items.Add(itemExcel);

            menu.Show(btnOptions, new Point(0, -menu.Height - 5));
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            if (dgvDetails.Rows.Count == 0)
            {
                MessageBox.Show("الفاتورة فارغة، لا يوجد ما يمكن مشاركته.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            string customer = string.IsNullOrWhiteSpace(cmbCustomer.Text) ? "عميل نقدي" : cmbCustomer.Text;
            string msg = $"مرحباً،\nفاتورة مبيعات جديدة رقم: {txtInvoiceNo.Text}\nالعميل: {customer}\nالإجمالي المطلوب: {txtNetAmount.Text} ج.م\nشكراً لتعاملكم معنا!";
            string url = "https://api.whatsapp.com/send?text=" + Uri.EscapeDataString(msg);

            try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true }); }
            catch { try { System.Diagnostics.Process.Start(url); } catch { MessageBox.Show("تعذر فتح واتساب.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); } }
        }

        private void SetupTextBoxPlaceholders()
        {
            AttachPlaceholderEvents(txtQuantity, "1"); AttachPlaceholderEvents(txtUnitPrice, "0.00");
            AttachPlaceholderEvents(txtDiscount, "0.00"); AttachPlaceholderEvents(txtPaidAmount, "0.00");
        }

        private void AttachPlaceholderEvents(TextBox txt, string defaultValue)
        {
            txt.Text = defaultValue; txt.ForeColor = Color.Gray;
            txt.Enter += (s, e) => { if (txt.Text == defaultValue) { txt.Text = ""; txt.ForeColor = Color.White; } };
            txt.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txt.Text)) { txt.Text = defaultValue; txt.ForeColor = Color.Gray; } };
        }

        private void LoadInitialData()
        {
            cmbLocation.DataSource = locationService.GetAllLocations();
            cmbLocation.DisplayMember = "Name"; cmbLocation.ValueMember = "Id";
            cmbLocation.SelectedValue = _currentUserLocationId;

            if (_currentUserLocationId != 1) cmbLocation.Enabled = false;

            if (cmbLocation.SelectedValue != null && int.TryParse(cmbLocation.SelectedValue.ToString(), out int locId))
                LoadCustomers(locId);

            availableProducts = productService.GetAllProducts().Where(p => p.IsActive).ToList();
            cmbProduct.DataSource = availableProducts; cmbProduct.DisplayMember = "Name"; cmbProduct.ValueMember = "Id"; cmbProduct.SelectedIndex = -1;

            cmbPaymentMethod.Items.Clear();
            cmbPaymentMethod.Items.AddRange(new string[] { "نقدي", "انستاباي الكردي", "انستاباي الحكش" });
            cmbPaymentMethod.SelectedIndex = 0;
        }

        private void LoadCustomers(int locationId)
        {
            cmbCustomer.DataSource = salesService.GetCustomersByLocation(locationId);
            cmbCustomer.DisplayMember = "Name"; cmbCustomer.ValueMember = "Id"; cmbCustomer.SelectedIndex = -1;
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            if (cmbLocation.SelectedValue != null && int.TryParse(cmbLocation.SelectedValue.ToString(), out int locId)) LoadCustomers(locId);
        }

        private void ResetInvoice()
        {
            txtInvoiceNo.Text = salesService.GenerateInvoiceNumber();
            chkManualDate.Checked = false; dtpInvoiceDate.Value = DateTime.Now; dtpInvoiceDate.Enabled = false;
            cmbCustomer.SelectedIndex = -1; cmbPaymentMethod.SelectedIndex = 0;
            dgvDetails.Rows.Clear();
            txtDiscount.Text = "0.00"; txtDiscount.ForeColor = Color.Gray;
            txtPaidAmount.Text = "0.00"; txtPaidAmount.ForeColor = Color.Gray;
            txtNotes.Clear();
            ClearProductEntry(); CalculateFinancials();
        }

        // التعديل: تحويل لجلب الرصيد Async
        private async void UpdateAvailableQuantity()
        {
            if (cmbProduct.SelectedIndex != -1 && cmbProduct.SelectedValue != null && cmbLocation.SelectedValue != null)
            {
                if (int.TryParse(cmbProduct.SelectedValue.ToString(), out int productId) && int.TryParse(cmbLocation.SelectedValue.ToString(), out int locId))
                {
                    int available = await salesService.GetAvailableStockAsync(productId, locId);
                    txtAvailableQty.Text = available.ToString();
                    txtAvailableQty.ForeColor = ClrOrange;
                    return;
                }
            }
            txtAvailableQty.Text = "0"; txtAvailableQty.ForeColor = Color.Gray;
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedIndex != -1 && cmbProduct.SelectedValue != null)
            {
                txtUnitPrice.Text = ""; txtUnitPrice.ForeColor = Color.White;
                txtQuantity.Text = "1"; txtQuantity.ForeColor = Color.White;
                UpdateAvailableQuantity(); txtUnitPrice.Focus();
            }
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAvailableQuantity();
            if (cmbLocation.SelectedValue != null && int.TryParse(cmbLocation.SelectedValue.ToString(), out int locId)) LoadCustomers(locId);
            if (_currentInvoiceId == 0) { dgvDetails.Rows.Clear(); CalculateFinancials(); }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedIndex == -1 || cmbProduct.SelectedValue == null || cmbLocation.SelectedValue == null)
            { MessageBox.Show("من فضلك اختر المنتج والفرع.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
            { MessageBox.Show("من فضلك أدخل كمية صحيحة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            decimal price = ParseSafeDecimal(txtUnitPrice.Text);
            if (price <= 0)
            { MessageBox.Show("من فضلك أدخل سعر بيع صحيح.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (!int.TryParse(txtAvailableQty.Text, out int availableQty)) availableQty = 0;

            int productId = Convert.ToInt32(cmbProduct.SelectedValue);
            DataGridViewRow existingRow = null;

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.Cells["colProductId"].Value != null && Convert.ToInt32(row.Cells["colProductId"].Value) == productId)
                { existingRow = row; break; }
            }

            if (existingRow != null)
            {
                int oldQty = Convert.ToInt32(existingRow.Cells["colQuantity"].Value);
                decimal oldPrice = ParseSafeDecimal(existingRow.Cells["colUnitPrice"].Value.ToString());

                if (oldPrice != price)
                { MessageBox.Show("هذا المنتج موجود بالفعل في الفاتورة بسعر مختلف.\nقم بتعديل السطر الموجود بدل إضافة سعر آخر.", "اختلاف السعر", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                int newQty = oldQty + qty;
                if (_currentInvoiceId == 0 && newQty > availableQty)
                { MessageBox.Show($"الكمية المطلوبة غير متوفرة!\nالمتاح: {availableQty}", "رصيد غير كافٍ", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                existingRow.Cells["colQuantity"].Value = newQty;
                existingRow.Cells["colTotalPrice"].Value = (newQty * price).ToString("0.00", CultureInfo.InvariantCulture);
                ClearProductEntry(); CalculateFinancials(); return;
            }

            if (_currentInvoiceId == 0 && qty > availableQty)
            { MessageBox.Show($"الكمية المطلوبة غير متوفرة!\nالمتاح: {availableQty}", "رصيد غير كافٍ", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            dgvDetails.Rows.Add(productId, cmbProduct.Text, qty, price.ToString("0.00", CultureInfo.InvariantCulture), (qty * price).ToString("0.00", CultureInfo.InvariantCulture));
            ClearProductEntry(); CalculateFinancials();
        }

        private void ClearProductEntry()
        {
            cmbProduct.SelectedIndex = -1; cmbProduct.Text = "";
            txtQuantity.Text = "1"; txtQuantity.ForeColor = Color.Gray;
            txtUnitPrice.Text = "0.00"; txtUnitPrice.ForeColor = Color.Gray;
            txtAvailableQty.Text = "0"; txtAvailableQty.ForeColor = Color.Gray;
            cmbProduct.Focus();
        }

        // التعديل الرابع: تحسين تجربة المستخدم بإلغاء الحذف التلقائي للسطر
        private void dgvDetails_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _isViewOnly) return;
            DataGridViewRow row = dgvDetails.Rows[e.RowIndex];
            cmbProduct.SelectedValue = Convert.ToInt32(row.Cells["colProductId"].Value);
            txtQuantity.Text = row.Cells["colQuantity"].Value.ToString(); txtQuantity.ForeColor = Color.White;
            txtUnitPrice.Text = row.Cells["colUnitPrice"].Value.ToString(); txtUnitPrice.ForeColor = Color.White;
            // تم إزالة dgvDetails.Rows.RemoveAt(e.RowIndex) لتفادي ضياع الصنف بالخطأ
        }

        private void dgvDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || _isViewOnly) return;
            if (dgvDetails.Columns[e.ColumnIndex].Name == "colDelete")
            { dgvDetails.Rows.RemoveAt(e.RowIndex); CalculateFinancials(); }
        }

        private void CalculateFinancials()
        {
            decimal totalAmount = 0;
            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.Cells["colTotalPrice"].Value != null)
                    totalAmount += ParseSafeDecimal(row.Cells["colTotalPrice"].Value.ToString());
            }

            txtTotalAmount.Text = totalAmount.ToString("0.00", CultureInfo.InvariantCulture);
            decimal discount = ParseSafeDecimal(txtDiscount.Text);
            if (discount < 0) discount = 0;
            if (discount > totalAmount) discount = totalAmount;

            decimal netAmount = totalAmount - discount;
            txtNetAmount.Text = netAmount.ToString("0.00", CultureInfo.InvariantCulture);

            decimal paidAmount = ParseSafeDecimal(txtPaidAmount.Text);
            if (paidAmount < 0) paidAmount = 0;
            if (paidAmount > netAmount) paidAmount = netAmount;

            decimal remainingAmount = netAmount - paidAmount;
            txtRemainingAmount.Text = remainingAmount.ToString("0.00", CultureInfo.InvariantCulture);
            txtRemainingAmount.ForeColor = remainingAmount > 0 ? ClrRed : ClrGreen;
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e) { CalculateFinancials(); }
        private void txtPaidAmount_TextChanged(object sender, EventArgs e) { CalculateFinancials(); }

        // التعديل الخامس: الحفظ بشكل Async لعدم تجميد الشاشة
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvDetails.Rows.Count == 0)
            { MessageBox.Show("لا يمكن حفظ فاتورة بدون أصناف.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (cmbLocation.SelectedValue == null)
            { MessageBox.Show("من فضلك حدد الفرع.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            CalculateFinancials();

            decimal totalAmount = ParseSafeDecimal(txtTotalAmount.Text);
            decimal discount = ParseSafeDecimal(txtDiscount.Text);
            decimal netAmount = ParseSafeDecimal(txtNetAmount.Text);
            decimal paidAmount = ParseSafeDecimal(txtPaidAmount.Text);
            decimal remainingAmount = ParseSafeDecimal(txtRemainingAmount.Text);

            Sale sale = new Sale
            {
                Id = _currentInvoiceId,
                InvoiceNumber = txtInvoiceNo.Text,
                CustomerId = cmbCustomer.SelectedIndex != -1 ? (int?)Convert.ToInt32(cmbCustomer.SelectedValue) : null,
                LocationId = Convert.ToInt32(cmbLocation.SelectedValue),
                InvoiceDate = dtpInvoiceDate.Value,
                TotalAmount = totalAmount,
                Discount = discount,
                NetAmount = netAmount,
                PaidAmount = paidAmount,
                RemainingAmount = remainingAmount,
                PaymentMethod = cmbPaymentMethod.Text,
                Notes = txtNotes.Text
            };

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.Cells["colProductId"].Value == null) continue;
                sale.Details.Add(new SaleDetail
                {
                    ProductId = Convert.ToInt32(row.Cells["colProductId"].Value),
                    Quantity = Convert.ToInt32(row.Cells["colQuantity"].Value),
                    UnitPrice = ParseSafeDecimal(row.Cells["colUnitPrice"].Value.ToString()),
                    TotalPrice = ParseSafeDecimal(row.Cells["colTotalPrice"].Value.ToString())
                });
            }

            btnSave.Enabled = false; // حماية من الدبل كليك وقت الحفظ
            try
            {
                if (_currentInvoiceId > 0)
                {
                    await salesService.UpdateSaleInvoiceAsync(sale);
                    MessageBox.Show("تم تعديل الفاتورة بنجاح.", "تم التعديل", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    await salesService.SaveSaleInvoiceAsync(sale);
                    MessageBox.Show("تم حفظ الفاتورة بنجاح.", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetInvoice();
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "خطأ في الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { btnSave.Enabled = true; }
        }

        // التعديل الأول: تحميل الفاتورة بشكل نظيف بعد نقل الكود للسيرفيس
        private async Task LoadInvoiceDataByIdAsync(int invoiceId)
        {
            try
            {
                Sale sale = await salesService.GetSaleInvoiceByIdAsync(invoiceId);
                if (sale == null) return;

                txtInvoiceNo.Text = string.IsNullOrWhiteSpace(sale.InvoiceNumber) ? txtInvoiceNo.Text : sale.InvoiceNumber;
                dtpInvoiceDate.Value = sale.InvoiceDate;
                if (sale.LocationId > 0) cmbLocation.SelectedValue = sale.LocationId;
                if (sale.CustomerId.HasValue) cmbCustomer.SelectedValue = sale.CustomerId.Value;

                txtDiscount.Text = sale.Discount.ToString("0.00", CultureInfo.InvariantCulture); txtDiscount.ForeColor = Color.White;
                txtPaidAmount.Text = sale.PaidAmount.ToString("0.00", CultureInfo.InvariantCulture); txtPaidAmount.ForeColor = Color.White;
                cmbPaymentMethod.Text = sale.PaymentMethod;
                txtNotes.Text = sale.Notes;

                dgvDetails.Rows.Clear();
                foreach (var detail in sale.Details)
                {
                    // جلب اسم الصنف لملء الجريد بناءً على قائمة المنتجات المحملة مسبقاً
                    string prodName = availableProducts?.FirstOrDefault(p => p.Id == detail.ProductId)?.Name ?? "منتج " + detail.ProductId;
                    dgvDetails.Rows.Add(detail.ProductId, prodName, detail.Quantity,
                        detail.UnitPrice.ToString("0.00", CultureInfo.InvariantCulture),
                        detail.TotalPrice.ToString("0.00", CultureInfo.InvariantCulture));
                }
                CalculateFinancials();
            }
            catch (Exception ex)
            { MessageBox.Show("خطأ في تحميل تفاصيل الفاتورة:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void PrintInvoice(bool isPdf)
        {
            if (dgvDetails.Rows.Count == 0) { MessageBox.Show("لا توجد أصناف لطباعتها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            _printRowIndex = 0;

            using (PrintDocument printDoc = new PrintDocument())
            {
                printDoc.DefaultPageSettings.Landscape = false; printDoc.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);
                printDoc.BeginPrint += (s, ev) => { _printRowIndex = 0; }; printDoc.PrintPage += PrintDoc_PrintPage;

                if (isPdf)
                {
                    printDoc.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                    if (!printDoc.PrinterSettings.IsValid) { MessageBox.Show("تأكد من تفعيل Print to PDF", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                    using (SaveFileDialog sfd = new SaveFileDialog { Filter = "PDF File|*.pdf", FileName = $"فاتورة_{txtInvoiceNo.Text}.pdf" })
                    {
                        if (sfd.ShowDialog() != DialogResult.OK) return;
                        printDoc.PrinterSettings.PrintToFile = true; printDoc.PrinterSettings.PrintFileName = sfd.FileName;
                        try { printDoc.Print(); } catch (Exception ex) { MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                }
                else
                {
                    using (PrintDialog pd = new PrintDialog { Document = printDoc })
                    {
                        if (pd.ShowDialog() == DialogResult.OK) try { printDoc.Print(); } catch (Exception ex) { MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    }
                }
            }
        }

        private void DrawArabicText(Graphics g, string text, Font font, Brush brush, RectangleF rect, StringAlignment alignment = StringAlignment.Center)
        {
            using (StringFormat sf = new StringFormat { Alignment = alignment, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft | StringFormatFlags.NoWrap })
                g.DrawString(text ?? "", font, brush, rect, sf);
        }

        private void DrawTableCell(Graphics g, Pen pen, Font font, string text, RectangleF rect, bool isHeader = false, StringAlignment alignment = StringAlignment.Center)
        {
            if (isHeader) using (Brush brush = new SolidBrush(Color.FromArgb(235, 235, 235))) g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            DrawArabicText(g, text, font, Brushes.Black, new RectangleF(rect.X + 4, rect.Y, rect.Width - 8, rect.Height), alignment);
        }

        private void DrawTotalLabelAndValue(Graphics g, string label, string value, Font fontLabel, Font fontValue, float startX, float y, bool isLeftAligned)
        {
            StringFormat sf = new StringFormat { FormatFlags = StringFormatFlags.DirectionRightToLeft };
            if (isLeftAligned)
            {
                g.DrawString(label, fontLabel, Brushes.Black, startX, y, sf);
                SizeF labelSize = g.MeasureString(label, fontLabel, 500, sf);
                g.DrawString(value, fontValue, Brushes.Black, startX - labelSize.Width, y, sf);
            }
            else
            {
                g.DrawString(label, fontLabel, Brushes.Black, startX, y, sf);
                SizeF labelSize = g.MeasureString(label, fontLabel, 500, sf);
                g.DrawString(value, fontValue, Brushes.Black, startX - labelSize.Width, y, sf);
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics; Rectangle bounds = e.MarginBounds;
            int left = bounds.Left, right = bounds.Right, width = bounds.Width; float y = bounds.Top;
            _printPageNumber++; bool isFirstPage = _printRowIndex == 0;

            using (Font fontCompany = new Font("Segoe UI", 22F, FontStyle.Bold))
            using (Font fontTitle = new Font("Segoe UI", 16F, FontStyle.Bold))
            using (Font fontInfo = new Font("Segoe UI", 11F, FontStyle.Regular))
            using (Font fontInfoBold = new Font("Segoe UI", 11F, FontStyle.Bold))
            using (Font fontHeader = new Font("Segoe UI", 10.5F, FontStyle.Bold))
            using (Font fontRow = new Font("Segoe UI", 10F, FontStyle.Regular))
            using (Font fontTotalLabel = new Font("Segoe UI", 11.5F, FontStyle.Bold))
            using (Pen pen = new Pen(Color.Black, 1))
            using (Pen penThick = new Pen(Color.Black, 1.8F))
            {
                if (isFirstPage)
                {
                    DrawArabicText(g, CompanyName, fontCompany, Brushes.Black, new RectangleF(left, y, width, 38)); y += 42;
                    DrawArabicText(g, "فاتورة مبيعات", fontTitle, Brushes.Black, new RectangleF(left, y, width, 30)); y += 38;
                    g.DrawLine(penThick, left, y, right, y); y += 12;

                    string customer = string.IsNullOrWhiteSpace(cmbCustomer.Text) ? "عميل نقدي" : cmbCustomer.Text;
                    float infoHalf = width / 2f;
                    using (StringFormat sfNear = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft })
                    {
                        g.DrawString("رقم الفاتورة: " + txtInvoiceNo.Text, fontInfoBold, Brushes.Black, new RectangleF(right - infoHalf, y, infoHalf, 25), sfNear);
                        g.DrawString("التاريخ: " + dtpInvoiceDate.Value.ToString("yyyy/MM/dd hh:mm tt"), fontInfo, Brushes.Black, new RectangleF(left, y, infoHalf, 25), sfNear); y += 27;
                        g.DrawString("العميل: " + customer, fontInfoBold, Brushes.Black, new RectangleF(right - infoHalf, y, infoHalf, 25), sfNear);
                        g.DrawString("الفرع: " + cmbLocation.Text, fontInfo, Brushes.Black, new RectangleF(left, y, infoHalf, 25), sfNear); y += 27;
                        decimal remaining = ParseSafeDecimal(txtRemainingAmount.Text);
                        g.DrawString("حالة الدفع: " + (remaining <= 0 ? "خالصة" : "متبقي " + remaining.ToString("N2")), fontInfo, Brushes.Black, new RectangleF(left, y, width, 25), sfNear);
                    }
                    y += 35; g.DrawLine(penThick, left, y, right, y); y += 12;
                }
                else
                {
                    DrawArabicText(g, CompanyName + " - تابع فاتورة رقم " + txtInvoiceNo.Text, fontInfo, Brushes.Black, new RectangleF(left, y, width, 25));
                    y += 30; g.DrawLine(penThick, left, y, right, y); y += 12;
                }

                float[] colWidths = { width * 0.09f, width * 0.41f, width * 0.14f, width * 0.16f, width * 0.20f };
                string[] headers = { "م", "اسم الصنف", "الكمية", "السعر", "الإجمالي" };
                float headerHeight = 32; float x = right;

                for (int i = 0; i < headers.Length; i++) { x -= colWidths[i]; DrawTableCell(g, pen, fontHeader, headers[i], new RectangleF(x, y, colWidths[i], headerHeight), true); }
                y += headerHeight; float rowHeight = 31; int bottomLimit = bounds.Bottom - 180;

                while (_printRowIndex < dgvDetails.Rows.Count)
                {
                    if (y + rowHeight > bottomLimit) { e.HasMorePages = true; DrawArabicText(g, CompanyName + "  -  شكراً لتعاملكم معنا", fontInfo, Brushes.Black, new RectangleF(left, bounds.Bottom - 20, width, 20)); return; }
                    DataGridViewRow row = dgvDetails.Rows[_printRowIndex];
                    string[] values = { (_printRowIndex + 1).ToString(), row.Cells["colProductName"].Value?.ToString() ?? "", row.Cells["colQuantity"].Value?.ToString() ?? "", row.Cells["colUnitPrice"].Value?.ToString() ?? "", row.Cells["colTotalPrice"].Value?.ToString() ?? "" };
                    x = right;
                    for (int i = 0; i < values.Length; i++) { x -= colWidths[i]; DrawTableCell(g, pen, fontRow, values[i], new RectangleF(x, y, colWidths[i], rowHeight), false, i == 1 ? StringAlignment.Near : StringAlignment.Center); }
                    y += rowHeight; _printRowIndex++;
                }

                y += 16;
                decimal totalAmount = ParseSafeDecimal(txtTotalAmount.Text); decimal discountAmount = ParseSafeDecimal(txtDiscount.Text);
                decimal netAmount = ParseSafeDecimal(txtNetAmount.Text); decimal paidAmount = ParseSafeDecimal(txtPaidAmount.Text);
                decimal remainingAmount = ParseSafeDecimal(txtRemainingAmount.Text);
                float lineHeight = 28, startXRight = right, startXLeft = right - (width * 0.50f);

                DrawTotalLabelAndValue(g, "الإجمالي الفرعي: ", totalAmount.ToString("N2") + " ج.م", fontRow, fontTotalLabel, startXRight, y, false);
                DrawTotalLabelAndValue(g, "الصافي المطلوب: ", netAmount.ToString("N2") + " ج.م", fontTotalLabel, fontTotalLabel, startXLeft, y, true); y += lineHeight;
                DrawTotalLabelAndValue(g, "الخصم: ", discountAmount.ToString("N2") + " ج.م", fontRow, fontTotalLabel, startXRight, y, false);
                DrawTotalLabelAndValue(g, "المدفوع: ", paidAmount.ToString("N2") + " ج.م", fontRow, fontTotalLabel, startXLeft, y, true); y += lineHeight;
                DrawTotalLabelAndValue(g, "المتبقي: ", remainingAmount.ToString("N2") + " ج.م", fontTotalLabel, fontTotalLabel, startXLeft, y, true); y += lineHeight;

                if (!string.IsNullOrWhiteSpace(txtNotes.Text))
                {
                    y += 10;
                    using (StringFormat sfNear = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft })
                    {
                        g.DrawString("ملاحظات الفاتورة:", fontHeader, Brushes.Black, new RectangleF(left, y, width, 22), sfNear); y += 22;
                        g.DrawString(txtNotes.Text, fontRow, Brushes.Black, new RectangleF(left, y, width, 36), sfNear);
                    }
                }
                DrawArabicText(g, CompanyName + "  -  شكراً لتعاملكم معنا", fontInfo, Brushes.Black, new RectangleF(left, bounds.Bottom - 20, width, 20));
            }
            e.HasMorePages = false;
        }

        private void ExportToCsv()
        {
            if (dgvDetails.Rows.Count == 0) return;
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV File|*.csv", FileName = $"فاتورة_{txtInvoiceNo.Text}.csv" })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                {
                    sw.WriteLine("اسم المنتج,الكمية,السعر,الإجمالي");
                    foreach (DataGridViewRow row in dgvDetails.Rows)
                        sw.WriteLine($"{row.Cells["colProductName"].Value},{row.Cells["colQuantity"].Value},{row.Cells["colUnitPrice"].Value},{row.Cells["colTotalPrice"].Value}");
                }
                MessageBox.Show("تم التصدير بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ApplyViewOnlyMode()
        {
            btnSave.Visible = btnAddProduct.Visible = btnAddCustomer.Visible = btnOptions.Visible = btnShare.Visible = false;
            cmbCustomer.Enabled = cmbLocation.Enabled = dtpInvoiceDate.Enabled = chkManualDate.Enabled = false;
            cmbProduct.Enabled = txtQuantity.Enabled = txtUnitPrice.Enabled = cmbPaymentMethod.Enabled = false;
            txtNotes.ReadOnly = txtDiscount.ReadOnly = txtPaidAmount.ReadOnly = dgvDetails.ReadOnly = true;
            dgvDetails.Columns["colDelete"].Visible = false;
        }
    }
}