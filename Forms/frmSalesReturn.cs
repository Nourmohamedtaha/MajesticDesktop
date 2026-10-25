using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MajesticDesktop.Models;
using MajesticDesktop.Services;

namespace MajesticDesktop.Forms
{
    public partial class frmSalesReturn : Form
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
        private static readonly Color ClrRedHover = Color.FromArgb(236, 112, 99);
        private static readonly Color ClrOrange = Color.FromArgb(230, 126, 34);

        private static readonly Font FontLabel = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        private static readonly Font FontInput = new Font("Segoe UI", 12F, FontStyle.Bold);
        private static readonly Font FontButton = new Font("Segoe UI", 12F, FontStyle.Bold);

        // ===================== بيانات الشركة للطباعة =====================
        // 💡 تم إضافة كلمة 'new' هنا لمنع التعارض مع خاصية Control.CompanyName
        private new const string CompanyName = "ماجيستيك - Majestic";

        // ===================== الخدمات =====================
        private SalesReturnService returnService;
        private SalesService salesService;
        private ProductService productService;
        private LocationService locationService;

        private List<Product> availableProducts;
        private List<Customer> availableCustomers;
        private int currentLocationId = 1;
        private int _currentReturnId = 0;

        private int _printRowIndex = 0;
        private int _printPageNumber = 0;

        private Button btnOptions;
        private CheckBox chkDeductFromDebt;

        public frmSalesReturn()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            returnService = new SalesReturnService();
            salesService = new SalesService();
            productService = new ProductService();
            locationService = new LocationService();

            BuildDynamicUI();
            BuildGridColumnsAndStyle();

            EnableDoubleBuffering(dgvDetails);

            dgvDetails.AutoGenerateColumns = false;
            dgvDetails.RowsAdded += DgvDetails_RowsChanged;
            dgvDetails.RowsRemoved += DgvDetails_RowsChanged;
        }

        private void EnableDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgv, new object[] { true });
        }

        private void DgvDetails_RowsChanged(object sender, DataGridViewRowsAddedEventArgs e) => UpdateRowNumbers();
        private void DgvDetails_RowsChanged(object sender, DataGridViewRowsRemovedEventArgs e) => UpdateRowNumbers();

        private void UpdateRowNumbers()
        {
            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (!row.IsNewRow)
                    row.Cells["colSerial"].Value = (row.Index + 1).ToString();
            }
        }

        private void frmSalesReturn_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            LoadInitialData();
        }

        // ============================================================
        // أدوات التصميم الديناميكي 
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
            return new ComboBox { Font = FontInput, BackColor = ClrBgInput, ForeColor = ClrText, FlatStyle = FlatStyle.Flat, DropDownStyle = ComboBoxStyle.DropDown, RightToLeft = RightToLeft.Yes };
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
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.') e.Handled = true;
            TextBox tb = sender as TextBox;
            if (tb != null && e.KeyChar == '.' && tb.Text.Contains(".")) e.Handled = true;
        }

        // ============================================================
        // بناء الشاشة وتوزيع العناصر
        // ============================================================
        private void BuildDynamicUI()
        {
            this.BackColor = ClrBgDark;
            this.Padding = new Padding(15);

            // 1. بحث العميل والفرع
            Panel pnlSearch = new Panel { BackColor = ClrBgPanel, Dock = DockStyle.Top, AutoSize = true, RightToLeft = RightToLeft.Yes, Padding = new Padding(10) };
            var searchRow = CreateGridRow(new float[] { 25f, 35f, 40f }, 90);

            cmbLocation = CreateSearchableCombo();
            cmbLocation.Enabled = false;

            cmbCustomer = CreateSearchableCombo();
            cmbCustomer.SelectedIndexChanged += cmbCustomer_SelectedIndexChanged;

            Panel pnlChkWrap = new Panel { Dock = DockStyle.Fill, Margin = new Padding(5), BackColor = ClrCard, RightToLeft = RightToLeft.Yes, Padding = new Padding(10, 20, 10, 10) };
            chkDeductFromDebt = new CheckBox { Text = "خصم من المديونية (لا يتم رد مبلغ كاش للعميل)", Font = FontLabel, ForeColor = Color.Gray, Dock = DockStyle.Fill, RightToLeft = RightToLeft.Yes, Cursor = Cursors.Hand, AutoCheck = true, Enabled = false };
            pnlChkWrap.Controls.Add(chkDeductFromDebt);

            searchRow.Controls.Add(CreateField("الفرع المستلم", cmbLocation), 0, 0);
            searchRow.Controls.Add(CreateField("اسم العميل (اختياري)", cmbCustomer), 1, 0);
            searchRow.Controls.Add(pnlChkWrap, 2, 0);
            pnlSearch.Controls.Add(searchRow);

            // 2. إدخال الأصناف
            Panel pnlEntry = new Panel { BackColor = ClrBgPanel, Dock = DockStyle.Top, AutoSize = true, RightToLeft = RightToLeft.Yes, Padding = new Padding(10), Margin = new Padding(0, 15, 0, 15) };
            var entryRow = CreateGridRow(new float[] { 40f, 15f, 15f, 15f, 15f }, 90);

            cmbProduct = CreateSearchableCombo();
            cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;

            txtQuantity = CreateTextBox();
            txtQuantity.KeyPress += IntegerOnly_KeyPress;

            txtUnitPrice = CreateTextBox();
            txtUnitPrice.KeyPress += DecimalOnly_KeyPress;

            btnAddProduct = CreateActionButton("إضافة صنف ⬇", ClrGreen, ClrGreenHover);
            btnAddProduct.Click += btnAddProduct_Click;

            btnRemoveProduct = CreateActionButton("حذف صنف ✖", ClrRed, ClrRedHover);
            btnRemoveProduct.Click += btnRemoveProduct_Click;

            entryRow.Controls.Add(CreateField("المنتج المرتجع *", cmbProduct), 0, 0);
            entryRow.Controls.Add(CreateField("الكمية *", txtQuantity), 1, 0);
            entryRow.Controls.Add(CreateField("سعر الإرجاع *", txtUnitPrice), 2, 0);
            entryRow.Controls.Add(btnAddProduct, 3, 0);
            entryRow.Controls.Add(btnRemoveProduct, 4, 0);
            pnlEntry.Controls.Add(entryRow);

            // 3. الإجماليات والحفظ
            Panel pnlFooter = new Panel { BackColor = ClrBgPanel, Dock = DockStyle.Bottom, AutoSize = true, RightToLeft = RightToLeft.Yes, Padding = new Padding(10) };
            var footerRow = CreateGridRow(new float[] { 40f, 20f, 15f, 25f }, 100);

            txtNotes = CreateTextBox(false, ClrText, HorizontalAlignment.Right);

            txtTotalRefund = CreateTextBox(true, Color.FromArgb(255, 100, 100));
            txtTotalRefund.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            txtTotalRefund.Text = "0.00";

            btnOptions = CreateActionButton("خيارات ⚙", Color.FromArgb(52, 73, 94), Color.FromArgb(100, 116, 139));
            btnOptions.Click += btnOptions_Click;

            btnSave = CreateActionButton("حفظ المرتجع وتحديث الأرصدة ✔", ClrBlue, Color.FromArgb(41, 128, 185));
            btnSave.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnSave.Click += btnSave_Click;

            footerRow.Controls.Add(CreateField("سبب المرتجع / ملاحظات الإرجاع", txtNotes), 0, 0);
            footerRow.Controls.Add(CreateField("إجمالي مبلغ المرتجعات", txtTotalRefund, Color.FromArgb(255, 100, 100)), 1, 0);
            footerRow.Controls.Add(btnOptions, 2, 0);
            footerRow.Controls.Add(btnSave, 3, 0);
            pnlFooter.Controls.Add(footerRow);

            // 4. الجدول
            dgvDetails = new DataGridView { Dock = DockStyle.Fill };
            Panel pnlGridWrap = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 10, 0, 10) };
            pnlGridWrap.Controls.Add(dgvDetails);

            this.Controls.Add(pnlGridWrap);
            this.Controls.Add(pnlFooter);
            this.Controls.Add(pnlEntry);
            this.Controls.Add(pnlSearch);
        }

        private void BuildGridColumnsAndStyle()
        {
            var styleHeaderBlue = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, BackColor = ClrBlue, Font = new Font("Segoe UI", 11.5F, FontStyle.Bold), ForeColor = Color.White, SelectionBackColor = ClrBlue };
            var styleRows = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter, BackColor = ClrBgGrid, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.White, SelectionBackColor = ClrGreen };
            var styleAlt = new DataGridViewCellStyle { BackColor = ClrBgGridAlt, ForeColor = Color.White };

            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSerial", HeaderText = "م", FillWeight = 20 });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProductId", Visible = false });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "colProductName", HeaderText = "اسم الصنف", FillWeight = 220 });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "colReturnQty", HeaderText = "الكمية المرتجعة", FillWeight = 100 });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "colUnitPrice", HeaderText = "سعر الإرجاع", FillWeight = 100 });
            dgvDetails.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTotalRefund", HeaderText = "الإجمالي", FillWeight = 120 });

            dgvDetails.RightToLeft = RightToLeft.Yes; dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetails.BackgroundColor = ClrBgGrid; dgvDetails.BorderStyle = BorderStyle.None;
            dgvDetails.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None; dgvDetails.ColumnHeadersDefaultCellStyle = styleHeaderBlue;
            dgvDetails.ColumnHeadersHeight = 46; dgvDetails.DefaultCellStyle = styleRows; dgvDetails.AlternatingRowsDefaultCellStyle = styleAlt;
            dgvDetails.EnableHeadersVisualStyles = false; dgvDetails.RowHeadersVisible = false; dgvDetails.RowTemplate.Height = 42;
            dgvDetails.AllowUserToAddRows = false; dgvDetails.ReadOnly = true; dgvDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        // ============================================================
        // خيارات الطباعة (قائمة منسدلة)
        // ============================================================
        private class CustomMenuRenderer : ToolStripProfessionalRenderer
        {
            public CustomMenuRenderer() : base(new CustomColorTable()) { }
        }
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

            ToolStripMenuItem itemPrint = new ToolStripMenuItem("🖨️ طباعة المرتجع (مباشرة)");
            itemPrint.ForeColor = Color.FromArgb(52, 152, 219);
            itemPrint.Padding = new Padding(10, 5, 10, 5);
            itemPrint.Click += (s, ev) => PrintInvoice(false);

            ToolStripMenuItem itemPdf = new ToolStripMenuItem("📄 حفظ كملف PDF");
            itemPdf.ForeColor = Color.FromArgb(231, 76, 60);
            itemPdf.Padding = new Padding(10, 5, 10, 5);
            itemPdf.Click += (s, ev) => PrintInvoice(true);

            ToolStripMenuItem itemExcel = new ToolStripMenuItem("📊 تصدير شيت إكسيل (CSV)");
            itemExcel.ForeColor = Color.FromArgb(46, 204, 113);
            itemExcel.Padding = new Padding(10, 5, 10, 5);
            itemExcel.Click += (s, ev) => ExportToCsv();

            menu.Items.Add(itemPrint);
            menu.Items.Add(new ToolStripSeparator { BackColor = Color.Gray });
            menu.Items.Add(itemPdf);
            menu.Items.Add(new ToolStripSeparator { BackColor = Color.Gray });
            menu.Items.Add(itemExcel);

            menu.Show(btnOptions, new Point(0, -menu.Height - 5));
        }

        private void EnableAdvancedComboBoxFilter(ComboBox cmb, object dataSource, string displayMember, string valueMember)
        {
            cmb.DropDownStyle = ComboBoxStyle.DropDown;
            cmb.AutoCompleteMode = AutoCompleteMode.Suggest;
            cmb.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmb.DisplayMember = displayMember;
            cmb.ValueMember = valueMember;
            cmb.DataSource = dataSource;

            cmb.TextUpdate += (s, e) =>
            {
                var text = cmb.Text.ToLower();
                var items = ((IEnumerable<dynamic>)dataSource)
                    .Where(item =>
                    {
                        string val = item.GetType().GetProperty(displayMember).GetValue(item, null)?.ToString().ToLower() ?? "";
                        return string.IsNullOrEmpty(text) || val.Split(' ').Any(word => word.StartsWith(text));
                    }).ToList();

                cmb.DataSource = null;
                cmb.DataSource = items;
                cmb.DisplayMember = displayMember;
                cmb.ValueMember = valueMember;

                cmb.Text = text;
                cmb.SelectionStart = text.Length;
                cmb.DroppedDown = items.Count > 0;
            };
        }

        // ============================================================
        //  وظائف الـ Logic والبيانات
        // ============================================================

        private void LoadInitialData()
        {
            try
            {
                var locations = locationService.GetAllLocations();
                cmbLocation.DataSource = locations;
                cmbLocation.DisplayMember = "Name";
                cmbLocation.ValueMember = "Id";
                cmbLocation.SelectedValue = currentLocationId;

                availableCustomers = salesService.GetWarehouseCustomersOnly() ?? new List<Customer>();
                availableCustomers.Insert(0, new Customer { Id = 0, Name = "عميل نقدي" });

                EnableAdvancedComboBoxFilter(cmbCustomer, availableCustomers, "Name", "Id");
                cmbCustomer.SelectedIndex = 0;

                availableProducts = productService.GetAllProducts().Where(p => p.IsActive).ToList();
                EnableAdvancedComboBoxFilter(cmbProduct, availableProducts, "Name", "Id");
                cmbProduct.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل البيانات الأساسية:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedValue != null && int.TryParse(cmbCustomer.SelectedValue.ToString(), out int customerId) && customerId > 0)
            {
                chkDeductFromDebt.Enabled = true;
                chkDeductFromDebt.ForeColor = Color.FromArgb(241, 196, 15);
            }
            else
            {
                chkDeductFromDebt.Checked = false;
                chkDeductFromDebt.Enabled = false;
                chkDeductFromDebt.ForeColor = Color.Gray;
            }
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedIndex != -1 && cmbProduct.SelectedValue != null)
            {
                txtQuantity.Text = "1";
                txtUnitPrice.Text = "";
                txtUnitPrice.Focus();
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedIndex == -1 || cmbProduct.SelectedValue == null)
            {
                MessageBox.Show("من فضلك اختر المنتج أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("الرجاء إدخال كمية صحيحة أكبر من الصفر.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtUnitPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("الرجاء إدخال سعر بيع المرتجع بشكل صحيح.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = Convert.ToInt32(cmbProduct.SelectedValue);
            string productName = cmbProduct.Text;
            decimal total = qty * price;

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.Cells["colProductId"].Value != null &&
                    Convert.ToInt32(row.Cells["colProductId"].Value) == productId &&
                    Convert.ToDecimal(row.Cells["colUnitPrice"].Value) == price)
                {
                    int currentQty = Convert.ToInt32(row.Cells["colReturnQty"].Value);
                    row.Cells["colReturnQty"].Value = currentQty + qty;
                    row.Cells["colTotalRefund"].Value = ((currentQty + qty) * price).ToString("0.00");

                    ClearProductEntry();
                    CalculateTotalRefund();
                    return;
                }
            }

            dgvDetails.Rows.Add("", productId, productName, qty, price.ToString("0.00"), total.ToString("0.00"));

            ClearProductEntry();
            CalculateTotalRefund();
        }

        private void ClearProductEntry()
        {
            cmbProduct.SelectedIndex = -1;
            cmbProduct.Text = "";
            txtQuantity.Clear();
            txtUnitPrice.Clear();
            cmbProduct.Focus();
        }

        private void btnRemoveProduct_Click(object sender, EventArgs e)
        {
            if (dgvDetails.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvDetails.SelectedRows.Cast<DataGridViewRow>().ToList())
                {
                    dgvDetails.Rows.Remove(row);
                }
                CalculateTotalRefund();
            }
            else
            {
                MessageBox.Show("من فضلك حدد المنتج المراد حذفه من الجدول أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CalculateTotalRefund()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.Cells["colTotalRefund"].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells["colTotalRefund"].Value);
                }
            }
            txtTotalRefund.Text = total.ToString("0.00");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvDetails.Rows.Count == 0)
            {
                MessageBox.Show("لا يوجد أصناف مضافة للمرتجع.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalRefund = Convert.ToDecimal(txtTotalRefund.Text);
            int customerId = cmbCustomer.SelectedIndex > 0 && cmbCustomer.SelectedValue != null ? Convert.ToInt32(cmbCustomer.SelectedValue) : 0;

            bool deductFromDebt = chkDeductFromDebt.Checked && customerId > 0;
            bool isCashRefund = !deductFromDebt;

            SaleReturn saleReturn = new SaleReturn
            {
                CustomerId = customerId,
                ReturnDate = DateTime.Now,
                TotalRefundAmount = totalRefund,
                LocationId = currentLocationId,
                Notes = txtNotes.Text.Trim()
            };

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.IsNewRow || row.Cells["colProductId"].Value == null) continue;

                saleReturn.Details.Add(new SaleReturnDetail
                {
                    ProductId = Convert.ToInt32(row.Cells["colProductId"].Value),
                    ReturnedQuantity = Convert.ToInt32(row.Cells["colReturnQty"].Value),
                    UnitPrice = Convert.ToDecimal(row.Cells["colUnitPrice"].Value),
                    TotalPrice = Convert.ToDecimal(row.Cells["colTotalRefund"].Value)
                });
            }

            string confirmMsg = isCashRefund ? "سيتم سحب مبلغ المرتجع كاش من الخزينة." : "سيتم خصم مبلغ المرتجع من مديونية العميل.";
            if (MessageBox.Show("هل تريد تأكيد وحفظ المرتجع؟\n" + confirmMsg, "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                returnService.ProcessSalesReturn(saleReturn, isCashRefund);
                _currentReturnId = saleReturn.Id;

                string successMessage = "تم حفظ المرتجع ورد البضاعة للمخزن بنجاح.\n\n";
                if (!isCashRefund && saleReturn.CustomerId > 0)
                    successMessage += $"تم خصم مبلغ ({totalRefund} ج.م) من مديونية العميل.";
                else
                    successMessage += $"تم تسجيل صادر نقدي بقيمة ({totalRefund} ج.م) من الخزنة.";

                if (MessageBox.Show(successMessage + "\nهل تريد طباعة إيصال المرتجع؟", "عملية ناجحة", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    PrintInvoice(false);
                }

                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حفظ المرتجع:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            cmbCustomer.SelectedIndex = 0;
            chkDeductFromDebt.Checked = false;
            txtNotes.Clear();
            txtTotalRefund.Text = "0.00";
            dgvDetails.Rows.Clear();
            ClearProductEntry();
            _currentReturnId = 0;
        }

        // ============================================================
        //  الطباعة 
        // ============================================================
        private void PrintInvoice(bool isPdf)
        {
            if (dgvDetails.Rows.Count == 0)
            {
                MessageBox.Show("لا توجد أصناف لطباعتها.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _printRowIndex = 0;
            _printPageNumber = 0;

            using (PrintDocument printDoc = new PrintDocument())
            {
                printDoc.DefaultPageSettings.Landscape = false;
                printDoc.BeginPrint += (s, ev) => { _printRowIndex = 0; _printPageNumber = 0; };
                printDoc.PrintPage += PrintDoc_PrintPage;

                if (isPdf)
                {
                    printDoc.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                    if (!printDoc.PrinterSettings.IsValid)
                    {
                        MessageBox.Show("تأكد من تفعيل خاصية Microsoft Print to PDF على الجهاز.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    try { printDoc.Print(); }
                    catch (Exception ex) { MessageBox.Show("حدث خطأ أثناء التصدير: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                else
                {
                    using (PrintDialog pd = new PrintDialog { Document = printDoc })
                    {
                        if (pd.ShowDialog() == DialogResult.OK)
                        {
                            try { printDoc.Print(); }
                            catch (Exception ex) { MessageBox.Show("حدث خطأ أثناء الطباعة: " + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        }
                    }
                }
            }
        }

        private void DrawArabicText(Graphics g, string text, Font font, Brush brush, RectangleF rect, StringAlignment alignment = StringAlignment.Center)
        {
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = alignment;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.DirectionRightToLeft | StringFormatFlags.NoWrap;
                g.DrawString(text ?? "", font, brush, rect, sf);
            }
        }

        private void DrawTableCell(Graphics g, Pen pen, Font font, string text, RectangleF rect, bool fill = false, StringAlignment alignment = StringAlignment.Center)
        {
            if (fill)
            {
                using (Brush brush = new SolidBrush(Color.FromArgb(235, 235, 235)))
                {
                    g.FillRectangle(brush, rect);
                }
            }
            g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            DrawArabicText(g, text, font, Brushes.Black, new RectangleF(rect.X + 4, rect.Y, rect.Width - 8, rect.Height), alignment);
        }

        private void DrawSummaryRow(Graphics g, Font fontLabel, Font fontValue, string label, string value, float x, float y, float width, float height, bool isBold = false)
        {
            float labelWidth = width * 0.50f;
            float valueWidth = width * 0.50f;

            RectangleF labelRect = new RectangleF(x + valueWidth, y, labelWidth, height);
            RectangleF valueRect = new RectangleF(x, y, valueWidth, height);

            Font currentLabelFont = isBold ? fontValue : fontLabel;

            using (StringFormat sfLabel = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft })
            using (StringFormat sfValue = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft })
            {
                g.DrawString(label, currentLabelFont, Brushes.Black, labelRect, sfLabel);
                g.DrawString(value, fontValue, Brushes.Black, valueRect, sfValue);
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = e.MarginBounds;
            _printPageNumber++;
            bool isFirstPage = _printRowIndex == 0;

            using (Font fontCompany = new Font("Tahoma", 22F, FontStyle.Bold))
            using (Font fontTitle = new Font("Tahoma", 16F, FontStyle.Bold))
            using (Font fontInfoBold = new Font("Tahoma", 12F, FontStyle.Bold))
            using (Font fontInfo = new Font("Tahoma", 12F, FontStyle.Regular))
            using (Font fontHeader = new Font("Tahoma", 12F, FontStyle.Bold))
            using (Font fontRow = new Font("Tahoma", 11F, FontStyle.Regular))
            using (Font fontTotalsLabel = new Font("Tahoma", 11F, FontStyle.Regular))
            using (Font fontTotalsValue = new Font("Tahoma", 12F, FontStyle.Bold))
            using (Pen penStandard = new Pen(Color.Black, 1))
            using (Pen penThick = new Pen(Color.Black, 2))
            using (StringFormat sfRight = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft })
            using (StringFormat sfLeft = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft })
            using (StringFormat sfCenter = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft })
            {
                int y = bounds.Top;
                int width = bounds.Width;
                int left = bounds.Left;
                int right = bounds.Right;

                // ==========================================
                // 1. الهيدر 
                // ==========================================
                if (isFirstPage)
                {
                    g.DrawString(CompanyName, fontCompany, Brushes.Black, new RectangleF(left, y, width, 40), sfCenter);
                    y += 45;
                    g.DrawString("إشعار مرتجع مبيعات", fontTitle, Brushes.Black, new RectangleF(left, y, width, 30), sfCenter);
                    y += 40;
                    g.DrawLine(penThick, left, y, right, y);
                    y += 15;

                    string customer = string.IsNullOrWhiteSpace(cmbCustomer.Text) ? "عميل نقدي" : cmbCustomer.Text;

                    g.DrawString("التاريخ: " + DateTime.Now.ToString("yyyy/MM/dd hh:mm tt"), fontInfo, Brushes.Black, new RectangleF(left + width / 2, y, width / 2, 25), sfRight);
                    y += 30;

                    g.DrawString("العميل: " + customer, fontInfoBold, Brushes.Black, new RectangleF(left, y, width / 2, 25), sfRight);
                    g.DrawString("الفرع المستلم: " + cmbLocation.Text, fontInfo, Brushes.Black, new RectangleF(left + width / 2, y, width / 2, 25), sfRight);
                    y += 30;

                    string refundType = chkDeductFromDebt.Checked ? "تسوية وخصم من المديونية" : "رد نقدية كاش";
                    g.DrawString("طريقة الاسترداد: " + refundType, fontInfoBold, Brushes.Black, new RectangleF(left, y, width, 25), sfRight);
                    y += 35;

                    g.DrawLine(penThick, left, y, right, y);
                    y += 20;
                }
                else
                {
                    g.DrawString(CompanyName + " - تابع مرتجع مبيعات", fontInfo, Brushes.Black, new RectangleF(left, y, width, 25), sfCenter);
                    y += 30;
                    g.DrawLine(penThick, left, y, right, y);
                    y += 20;
                }

                // ==========================================
                // 2. جدول المرتجع
                // ==========================================
                float[] colRatios = { 0.08f, 0.42f, 0.12f, 0.18f, 0.20f };
                string[] headers = { "م", "اسم الصنف", "الكمية", "سعر الإرجاع", "الإجمالي" };
                float[] colWidths = colRatios.Select(r => r * width).ToArray();

                float headerHeight = 35;
                float x = right;

                g.FillRectangle(Brushes.LightGray, left, y, width, headerHeight);
                g.DrawRectangle(penStandard, left, y, width, headerHeight);

                for (int i = 0; i < headers.Length; i++)
                {
                    RectangleF rect = new RectangleF(x - colWidths[i], y, colWidths[i], headerHeight);
                    g.DrawRectangle(penStandard, rect.X, rect.Y, rect.Width, rect.Height);
                    g.DrawString(headers[i], fontHeader, Brushes.Black, rect, sfCenter);
                    x -= colWidths[i];
                }
                y += (int)headerHeight;

                float rowHeight = 35;
                int bottomLimit = bounds.Bottom - 180;

                while (_printRowIndex < dgvDetails.Rows.Count)
                {
                    if (y + rowHeight > bottomLimit)
                    {
                        e.HasMorePages = true;
                        DrawArabicText(g, CompanyName + "  -  توقيع المستلم: .....................", fontInfo, Brushes.Black, new RectangleF(left, bounds.Bottom - 20, width, 20), StringAlignment.Center);
                        return;
                    }

                    DataGridViewRow row = dgvDetails.Rows[_printRowIndex];
                    string[] rowData = {
                        (_printRowIndex + 1).ToString(),
                        row.Cells["colProductName"].Value?.ToString() ?? "",
                        row.Cells["colReturnQty"].Value?.ToString() ?? "",
                        row.Cells["colUnitPrice"].Value?.ToString() ?? "",
                        row.Cells["colTotalRefund"].Value?.ToString() ?? ""
                    };

                    x = right;
                    for (int i = 0; i < rowData.Length; i++)
                    {
                        RectangleF rect = new RectangleF(x - colWidths[i], y, colWidths[i], rowHeight);
                        g.DrawRectangle(penStandard, rect.X, rect.Y, rect.Width, rect.Height);

                        StringFormat cellFormat = (i == 1) ? sfRight : sfCenter;
                        RectangleF textRect = new RectangleF(rect.X + 2, rect.Y, rect.Width - 4, rect.Height);
                        g.DrawString(rowData[i], fontRow, Brushes.Black, textRect, cellFormat);

                        x -= colWidths[i];
                    }

                    y += (int)rowHeight;
                    _printRowIndex++;
                }

                // ==========================================
                // 3. الإجماليات
                // ==========================================
                y += 15;

                decimal.TryParse(txtTotalRefund.Text, out decimal totalRefund);

                float totalsWidth = colWidths[4] + colWidths[3] + colWidths[2];
                float totalsLeft = left;
                float lineHeight = 30;

                g.DrawRectangle(penStandard, totalsLeft, y, totalsWidth, lineHeight);
                DrawSummaryRow(g, fontTotalsLabel, fontTotalsValue, "إجمالي مبلغ المرتجع:", totalRefund.ToString("N2") + " ج.م", totalsLeft, y, totalsWidth, lineHeight, true);
                y += (int)lineHeight;

                if (!string.IsNullOrWhiteSpace(txtNotes.Text))
                {
                    y += 20;
                    g.DrawString("سبب المرتجع:", fontHeader, Brushes.Black, new RectangleF(left, y, width, 25), sfRight);
                    y += 25;
                    g.DrawString(txtNotes.Text, fontRow, Brushes.Black, new RectangleF(left, y, width, 40), sfRight);
                }

                DrawArabicText(g, CompanyName + "  -  توقيع المستلم: .....................", fontInfo, Brushes.Black, new RectangleF(left, bounds.Bottom - 20, width, 20), StringAlignment.Center);
            }
            e.HasMorePages = false;
        }

        private void ExportToCsv()
        {
            if (dgvDetails.Rows.Count == 0) return;
            SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV File|*.csv", FileName = $"مرتجع_{DateTime.Now.ToString("yyyyMMdd")}.csv" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                {
                    sw.WriteLine("اسم المنتج,الكمية המرتجعة,السعر,الإجمالي");
                    foreach (DataGridViewRow row in dgvDetails.Rows) sw.WriteLine($"{row.Cells["colProductName"].Value},{row.Cells["colReturnQty"].Value},{row.Cells["colUnitPrice"].Value},{row.Cells["colTotalRefund"].Value}");
                }
                MessageBox.Show("تم التصدير بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ==========================================
        // تعريف الأدوات للديزاينر الداخلي
        // ==========================================
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.Button btnAddProduct;
        private System.Windows.Forms.Button btnRemoveProduct;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.TextBox txtTotalRefund;
        private System.Windows.Forms.Button btnSave;
    }
}