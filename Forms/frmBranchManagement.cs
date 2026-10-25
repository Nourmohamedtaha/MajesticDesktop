using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MajesticDesktop.Models;
using MajesticDesktop.Services;

namespace MajesticDesktop.Forms
{
    public partial class frmBranchManagement : Form
    {
        private readonly BranchManagementService _branchService;
        private readonly SalesService _salesService;
        private Button _currentTabButton;
        private int _selectedBranchId = 0;
        private bool _isLoaded = false;
        private bool _isBinding = false;
        private bool _isGridLoading = false;

        public frmBranchManagement()
        {
            InitializeComponent();
            ApplyCustomUIStyles();

            _branchService = new BranchManagementService();
            _salesService = new SalesService();

            try { _branchService.EnsureSchemaUpdated(); }
            catch (Exception ex) { Fail("فشل تحديث الجداول", ex); }

            SetupExpenseTypes();
            SetupComboBoxesSearch();
            SetupGridsStyle();

            dgvReturnItems.Paint += DgvReturnItems_Paint;

            dgvBranches.CellFormatting += Global_CellFormatting;
            dgvBranchProducts.CellFormatting += Global_CellFormatting;
            dgvReturnItems.CellFormatting += Global_CellFormatting;
            dgvExpenses.CellFormatting += Global_CellFormatting;
            dgvReturnHistory.CellFormatting += Global_CellFormatting;

            dgvBranchProducts.CellValueChanged += (s, ev) => { if (_isLoaded && !_isGridLoading) CalculateTotalInventoryValue(); };
            dgvBranchProducts.RowsRemoved += (s, ev) => { if (_isLoaded && !_isGridLoading) CalculateTotalInventoryValue(); };
        }

        // ===================== Helpers =====================
        private static DialogResult Msg(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(text, caption, buttons, icon, MessageBoxDefaultButton.Button1,
                MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
        }

        private static void Warn(string text) { Msg(text, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        private static void Info(string text) { Msg(text, "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private static void Fail(string context, Exception ex) { Msg(context + ":\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        private static bool Confirm(string text) { return Msg(text, "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes; }

        private bool TryGetBranchId(out int branchId)
        {
            branchId = 0;
            if (cmbLocations.SelectedValue == null) return false;

            if (cmbLocations.SelectedValue is DataRowView drv)
            {
                if (drv.Row.Table.Columns.Contains("BranchID"))
                    return int.TryParse(drv["BranchID"].ToString(), out branchId) && branchId > 0;
            }

            return int.TryParse(cmbLocations.SelectedValue.ToString(), out branchId) && branchId > 0;
        }

        private int GetSelectedCustomerId()
        {
            if (cmbReturnCustomer.SelectedValue != null && int.TryParse(cmbReturnCustomer.SelectedValue.ToString(), out int id))
                return id;
            return -1;
        }

        private static bool IsAdvanceType(string type)
        {
            return type == "سلفة" || type == "سلف";
        }

        private void DgvReturnItems_Paint(object sender, PaintEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (dgv.Rows.Count == 0)
            {
                string msg = "📦 لا توجد منتجات مضافة إلى المرتجع حالياً";
                using (Font f = BranchTheme.UiFont(16F, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(BranchTheme.Slate))
                {
                    SizeF size = e.Graphics.MeasureString(msg, f);
                    PointF pt = new PointF((dgv.Width - size.Width) / 2, (dgv.Height - size.Height) / 2 + 20);
                    e.Graphics.DrawString(msg, f, brush, pt);
                }
            }
        }

        private void Global_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv != null && e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgv.Columns[e.ColumnIndex].HeaderText == "م")
            {
                e.Value = (e.RowIndex + 1).ToString();
                e.FormattingApplied = true;
            }
        }

        // ===================== Load / Setup =====================
        private void frmBranchManagement_Load(object sender, EventArgs e)
        {
            cmbBranchType.Items.Clear();
            cmbBranchType.Items.Add("فرع بيع");
            cmbBranchType.Items.Add("عربية بيع");
            cmbBranchType.SelectedIndex = 0;

            LoadData();
            SwitchTab(btnTabAddBranch, pnlAddBranch);
            _isLoaded = true;

            if (cmbLocations.Items.Count > 0) cmbLocations.SelectedIndex = 0;
        }

        private void SetupComboBoxesSearch()
        {
            ComboBox[] combos = { cmbLocations, cmbReturnCustomer, cmbReturnProduct, cmbBranchType };
            foreach (var cmb in combos)
            {
                cmb.DropDownStyle = ComboBoxStyle.DropDown;
                cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmb.AutoCompleteSource = AutoCompleteSource.ListItems;
            }
            cmbExpenseType.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void SetupGridsStyle()
        {
            ApplyModernGridDesign(dgvBranches);
            ApplyModernGridDesign(dgvBranchProducts);
            ApplyModernGridDesign(dgvExpenses);
            ApplyModernGridDesign(dgvReturnItems);
            ApplyModernGridDesign(dgvReturnHistory);
            ApplyModernGridDesign(dgvDailyShift);
            ApplyModernGridDesign(dgvMonthlyShift);

            DataGridView[] readOnlyGrids = { dgvBranches, dgvExpenses, dgvReturnHistory, dgvDailyShift, dgvMonthlyShift };
            foreach (var g in readOnlyGrids) g.ReadOnly = true;

            foreach (DataGridViewColumn c in dgvBranchProducts.Columns)
                c.ReadOnly = !(c.Name == "colPurchasePrice" || c.Name == "colStockQty");

            foreach (DataGridViewColumn c in dgvReturnItems.Columns)
                c.ReadOnly = !(c.Name == "colRetQty" || c.Name == "colRetPrice");
        }

        private void ApplyModernGridDesign(DataGridView dgv)
        {
            dgv.BackgroundColor = BranchTheme.Bg;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.EnableHeadersVisualStyles = false;
            dgv.GridColor = BranchTheme.Border;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RightToLeft = RightToLeft.Yes;

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            headerStyle.BackColor = BranchTheme.Header;
            headerStyle.ForeColor = BranchTheme.Muted;
            headerStyle.SelectionBackColor = BranchTheme.Header;
            headerStyle.SelectionForeColor = BranchTheme.Muted;
            headerStyle.Font = BranchTheme.UiFont(13F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle = headerStyle;
            dgv.ColumnHeadersHeight = 50;

            DataGridViewCellStyle defaultStyle = new DataGridViewCellStyle();
            defaultStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            defaultStyle.BackColor = BranchTheme.Surface;
            defaultStyle.ForeColor = BranchTheme.Text;
            defaultStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            defaultStyle.SelectionForeColor = Color.White;
            defaultStyle.Font = BranchTheme.UiFont(12.5F, FontStyle.Bold);
            dgv.DefaultCellStyle = defaultStyle;

            DataGridViewCellStyle altStyle = new DataGridViewCellStyle();
            altStyle.BackColor = BranchTheme.Field;
            dgv.AlternatingRowsDefaultCellStyle = altStyle;

            dgv.RowTemplate.Height = 48;
        }

        private void SetupExpenseTypes()
        {
            cmbExpenseType.Items.Clear();
            cmbExpenseType.Items.Add("سلفة");
            cmbExpenseType.Items.Add("مصروف");
            cmbExpenseType.SelectedIndex = 1;
        }

        private static void FormatMoneyColumns(DataGridView dgv, params int[] columns)
        {
            foreach (int i in columns)
                if (i < dgv.Columns.Count) dgv.Columns[i].DefaultCellStyle.Format = "N2";
        }

        private void LoadData()
        {
            int previousBranchId = 0;
            if (cmbLocations.SelectedValue != null) int.TryParse(cmbLocations.SelectedValue.ToString(), out previousBranchId);

            _isBinding = true;
            try
            {
                DataTable dtLocations = _branchService.GetAllBranches();

                cmbLocations.DisplayMember = "BranchName";
                cmbLocations.ValueMember = "BranchID";
                cmbLocations.DataSource = dtLocations;
                cmbLocations.SelectedIndex = -1;

                dgvBranches.Rows.Clear();
                foreach (DataRow row in dtLocations.Rows)
                {
                    string status = Convert.ToBoolean(row["IsActive"]) ? "نشط" : "مغلق";
                    dgvBranches.Rows.Add("", row["BranchID"], row["BranchName"], row["BranchType"], status);
                }
            }
            catch (Exception ex)
            {
                Fail("فشل تحميل قائمة الفروع", ex);
            }
            finally
            {
                _isBinding = false;
            }

            if (_isLoaded && previousBranchId > 0) cmbLocations.SelectedValue = previousBranchId;
        }

        private void LoadReturnCustomers(int branchId)
        {
            DataTable dtCustomers = new DataTable();
            dtCustomers.Columns.Add("CustomerID", typeof(int));
            dtCustomers.Columns.Add("CustomerName", typeof(string));
            dtCustomers.Rows.Add(0, "عميل نقدي");

            try
            {
                DataTable dtRealCustomers = _branchService.GetActiveCustomersForBranch(branchId);
                foreach (DataRow row in dtRealCustomers.Rows)
                {
                    dtCustomers.Rows.Add(Convert.ToInt32(row["Id"]), row["Name"]?.ToString());
                }
            }
            catch (Exception ex)
            {
                Fail("فشل تحميل قائمة عملاء الفرع", ex);
            }

            cmbReturnCustomer.DisplayMember = "CustomerName";
            cmbReturnCustomer.ValueMember = "CustomerID";
            cmbReturnCustomer.DataSource = dtCustomers;
            cmbReturnCustomer.SelectedIndex = 0;
        }

        private void cmbLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isLoaded || _isBinding) return;

            dgvReturnItems.Rows.Clear();
            chkDeductFromDebt.Checked = false;

            if (!TryGetBranchId(out int branchId)) return;

            try
            {
                LoadBranchProductsGrid();

                DataTable dtProd = _branchService.GetBranchProducts(branchId);
                cmbReturnProduct.DisplayMember = "ProductName";
                cmbReturnProduct.ValueMember = "ProductID";
                cmbReturnProduct.DataSource = dtProd;
                cmbReturnProduct.SelectedIndex = -1;

                LoadReturnCustomers(branchId);
                LoadBranchExpensesHistoryGrid();
                LoadBranchReturnsHistoryGrid();

                if (pnlMonthly.Visible) LoadModernMonthlyData();
                if (pnlCloseShift.Visible) LoadModernDailyShiftData();
            }
            catch (Exception ex)
            {
                Fail("فشل تهيئة التبويبات للفرع المختار", ex);
            }
        }

        // ===================== Tabs =====================
        private void SwitchTab(Button activeBtn, Panel activePanel)
        {
            if (_currentTabButton != null)
            {
                _currentTabButton.BackColor = BranchTheme.Surface;
                _currentTabButton.ForeColor = BranchTheme.Text;
            }
            _currentTabButton = activeBtn;
            _currentTabButton.BackColor = BranchTheme.Accent;
            _currentTabButton.ForeColor = Color.White;

            Panel[] panels = { pnlAddBranch, pnlProducts, pnlReturns, pnlExpenses, pnlCloseShift, pnlMonthly };
            foreach (var pnl in panels) pnl.Visible = false;

            activePanel.Visible = true;
            activePanel.BringToFront();
        }

        private void btnTabAddBranch_Click(object sender, EventArgs e) { SwitchTab(btnTabAddBranch, pnlAddBranch); }
        private void btnTabProducts_Click(object sender, EventArgs e) { SwitchTab(btnTabProducts, pnlProducts); LoadBranchProductsGrid(); }
        private void btnTabReturns_Click(object sender, EventArgs e) { SwitchTab(btnTabReturns, pnlReturns); }
        private void btnTabExpenses_Click(object sender, EventArgs e) { SwitchTab(btnTabExpenses, pnlExpenses); LoadBranchExpensesHistoryGrid(); }
        private void btnTabClose_Click(object sender, EventArgs e) { SwitchTab(btnTabClose, pnlCloseShift); LoadModernDailyShiftData(); }
        private void btnTabMonthly_Click(object sender, EventArgs e) { SwitchTab(btnTabMonthly, pnlMonthly); LoadModernMonthlyData(); }

        // ===================== الفروع =====================
        private void btnSaveNewBranch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBranchName.Text)) { Warn("اكتب اسم الفرع أو المخزن."); return; }
            try
            {
                _branchService.AddBranch(txtBranchName.Text, cmbBranchType.Text);
                Info("تمت الإضافة بنجاح.");
                btnClearBranch_Click(null, null);
                LoadData();
            }
            catch (Exception ex) { Fail("فشل حفظ الفرع", ex); }
        }

        private void btnEditBranch_Click(object sender, EventArgs e)
        {
            if (_selectedBranchId <= 0) { Warn("برجاء اختيار الفرع أو المخزن من الجدول للتعديل."); return; }
            if (string.IsNullOrWhiteSpace(txtBranchName.Text)) { Warn("اكتب اسم الفرع أو المخزن."); return; }
            try
            {
                _branchService.UpdateBranch(_selectedBranchId, txtBranchName.Text, cmbBranchType.Text);
                Info("تم التعديل بنجاح.");
                btnClearBranch_Click(null, null);
                LoadData();
            }
            catch (Exception ex) { Fail("فشل تعديل الفرع", ex); }
        }

        private void btnClearBranch_Click(object sender, EventArgs e)
        {
            _selectedBranchId = 0;
            txtBranchName.Clear();
            if (cmbBranchType.Items.Count > 0) cmbBranchType.SelectedIndex = 0;
        }

        private void dgvBranches_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvBranches.Rows[e.RowIndex];
            if (row.Cells["colBranchID"].Value != null)
            {
                _selectedBranchId = Convert.ToInt32(row.Cells["colBranchID"].Value);
                txtBranchName.Text = row.Cells["colBranchName"].Value?.ToString();
                cmbBranchType.Text = row.Cells["colBranchType"].Value?.ToString();
            }
        }

        // ===================== أرصدة المنتجات =====================
        private void LoadBranchProductsGrid()
        {
            if (!TryGetBranchId(out int branchId)) return;
            try
            {
                _isGridLoading = true;
                DataTable dt = _branchService.GetBranchProducts(branchId);
                dgvBranchProducts.Rows.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    decimal price = row["PurchasePrice"] != DBNull.Value ? Convert.ToDecimal(row["PurchasePrice"]) : 0m;
                    string qty = row["StockQuantity"] != DBNull.Value ? row["StockQuantity"].ToString() : "0";
                    dgvBranchProducts.Rows.Add("", row["ProductID"], row["ProductName"], price.ToString("0.00"), qty);
                }
            }
            catch (Exception ex) { Fail("فشل تحميل أرصدة المنتجات للفرع", ex); }
            finally { _isGridLoading = false; }

            CalculateTotalInventoryValue();
        }

        private void CalculateTotalInventoryValue()
        {
            decimal totalValue = 0;
            foreach (DataGridViewRow row in dgvBranchProducts.Rows)
            {
                if (row.IsNewRow) continue;
                decimal price = 0, qty = 0;
                if (row.Cells["colPurchasePrice"].Value != null)
                    decimal.TryParse(row.Cells["colPurchasePrice"].Value.ToString(), out price);
                if (row.Cells["colStockQty"].Value != null)
                    decimal.TryParse(row.Cells["colStockQty"].Value.ToString(), out qty);
                totalValue += price * qty;
            }
            lblTotalInventoryValue.Text = "إجمالي قيمة بضاعة الفرع: " + totalValue.ToString("N2") + " ج.م";
        }

        private void btnAddBranchProduct_Click(object sender, EventArgs e)
        {
            if (!TryGetBranchId(out int branchId)) { Warn("اختر الفرع أولاً."); return; }
            if (string.IsNullOrWhiteSpace(txtNewProdName.Text)) { Warn("اكتب اسم المنتج."); return; }

            decimal price = 0, qty = 0;
            if (!string.IsNullOrWhiteSpace(txtNewPurchasePrice.Text) && !decimal.TryParse(txtNewPurchasePrice.Text, out price)) { Warn("سعر الشراء غير صحيح."); return; }
            if (!string.IsNullOrWhiteSpace(txtNewQty.Text) && !decimal.TryParse(txtNewQty.Text, out qty)) { Warn("الكمية غير صحيحة."); return; }
            if (price < 0 || qty < 0) { Warn("لا يمكن إدخال قيم سالبة."); return; }

            try
            {
                _branchService.AddBranchSpecificProduct(branchId, txtNewProdName.Text, price, qty);
            }
            catch (ProductLinkedToOtherBranchException plex)
            {
                if (!Confirm(plex.Message + "\n\nهل تريد المتابعة؟")) return;

                try
                {
                    _branchService.AddBranchSpecificProduct(branchId, txtNewProdName.Text, price, qty, true);
                }
                catch (Exception ex2) { Fail("فشل إضافة المنتج بعد التأكيد", ex2); return; }
            }
            catch (Exception ex) { Fail("فشل إضافة المنتج للفرع", ex); return; }

            Info("تمت إضافة المنتج للفرع.");
            txtNewProdName.Clear();
            txtNewPurchasePrice.Clear();
            txtNewQty.Clear();
            LoadBranchProductsGrid();
        }

        private void btnSaveBranchProducts_Click(object sender, EventArgs e)
        {
            if (!TryGetBranchId(out int branchId)) { Warn("اختر الفرع أولاً."); return; }

            var toSave = new List<Tuple<int, decimal, decimal>>();
            foreach (DataGridViewRow row in dgvBranchProducts.Rows)
            {
                if (row.IsNewRow || row.Cells["colProdID"].Value == null) continue;
                int prodId = Convert.ToInt32(row.Cells["colProdID"].Value);

                if (!decimal.TryParse(row.Cells["colPurchasePrice"].Value?.ToString(), out decimal price) ||
                    !decimal.TryParse(row.Cells["colStockQty"].Value?.ToString(), out decimal qty) ||
                    price < 0 || qty < 0)
                {
                    Warn("فيه قيمة غير صحيحة في السطر رقم " + (row.Index + 1) + " (" + row.Cells["colProdName"].Value + ").\nراجع سعر الشراء والكمية.");
                    return;
                }
                toSave.Add(Tuple.Create(prodId, price, qty));
            }

            if (toSave.Count == 0) { Warn("لا توجد بيانات لحفظها."); return; }

            try
            {
                _branchService.SaveBranchProductCustomPricingBatch(branchId, toSave);

                Info("تم حفظ التعديلات.");
                LoadBranchProductsGrid();
            }
            catch (Exception ex) { Fail("فشل حفظ تعديلات المنتجات", ex); }
        }

        // ===================== طباعة / تصدير الجرد =====================
        private void btnExportProducts_Click(object sender, EventArgs e)
        {
            if (dgvBranchProducts.Rows.Count == 0 || cmbLocations.SelectedValue == null)
            {
                Warn("لا يوجد بيانات للطباعة أو التصدير.");
                return;
            }

            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Font = BranchTheme.UiFont(13F, FontStyle.Bold);
            menu.RightToLeft = RightToLeft.Yes;

            ToolStripMenuItem itemPrint = new ToolStripMenuItem("🖨 طباعة / تصدير PDF (اختر الطابعة)");
            itemPrint.Click += (s, ev) => PrintInventoryGrid();

            ToolStripMenuItem itemPreview = new ToolStripMenuItem("👁 معاينة قبل الطباعة");
            itemPreview.Click += (s, ev) => PreviewInventoryGrid();

            ToolStripMenuItem itemExcel = new ToolStripMenuItem("📊 تصدير شيت إكسيل (CSV)");
            itemExcel.Click += (s, ev) => ExportToCsv();

            menu.Items.Add(itemPrint);
            menu.Items.Add(itemPreview);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(itemExcel);

            menu.Show(btnExportProducts, new Point(0, btnExportProducts.Height));
        }

        private void PrintInventoryGrid()
        {
            try
            {
                using (var printer = new DataGridViewPrinter(dgvBranchProducts))
                {
                    printer.Print();
                }
            }
            catch (Exception ex)
            {
                Fail("فشل عملية الطباعة", ex);
            }
        }

        private void PreviewInventoryGrid()
        {
            try
            {
                using (var printer = new DataGridViewPrinter(dgvBranchProducts))
                {
                    printer.PrintPreview();
                }
            }
            catch (Exception ex)
            {
                Fail("فشل عرض معاينة الطباعة", ex);
            }
        }

        private static string Csv(object value)
        {
            string s = value == null ? "" : value.ToString();
            return "\"" + s.Replace("\"", "\"\"") + "\"";
        }

        private void ExportToCsv()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV File|*.csv";
                sfd.FileName = "جرد_" + cmbLocations.Text + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
                    {
                        sw.WriteLine(string.Join(",", new[] { Csv("م"), Csv("اسم المنتج"), Csv("سعر الشراء"), Csv("الكمية المتاحة بالفرع"), Csv("إجمالي القيمة") }));
                        int index = 1;
                        foreach (DataGridViewRow row in dgvBranchProducts.Rows)
                        {
                            if (row.IsNewRow) continue;
                            decimal.TryParse(row.Cells["colPurchasePrice"].Value?.ToString(), out decimal price);
                            decimal.TryParse(row.Cells["colStockQty"].Value?.ToString(), out decimal qty);
                            sw.WriteLine(string.Join(",", new[]
                            {
                                Csv(index++), Csv(row.Cells["colProdName"].Value), Csv(price.ToString("0.00")), Csv(qty), Csv((price * qty).ToString("0.00"))
                            }));
                        }
                    }
                    Info("تم تصدير الإكسيل بنجاح.");
                }
                catch (Exception ex) { Fail("فشل تصدير ملف الـ CSV", ex); }
            }
        }

        // ===================== السلف والمصروفات =====================
        private void LoadBranchExpensesHistoryGrid()
        {
            if (!TryGetBranchId(out int branchId)) return;

            try
            {
                DataTable dt = _branchService.GetBranchExpensesHistory(branchId);
                dgvExpenses.Rows.Clear();

                decimal totalExpenses = 0, totalAdvances = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string type = row["TransactionType"].ToString();
                    decimal amount = Convert.ToDecimal(row["Amount"]);
                    bool isAdvance = IsAdvanceType(type);

                    int r = dgvExpenses.Rows.Add("", Convert.ToDateTime(row["TransactionDate"]).ToString("yyyy/MM/dd hh:mm tt"), type, amount.ToString("N2"), row["Description"]);
                    dgvExpenses.Rows[r].Cells["colExpType"].Style.ForeColor = isAdvance ? BranchTheme.OrangeText : BranchTheme.RedText;

                    if (isAdvance) totalAdvances += amount; else totalExpenses += amount;
                }

                lblExpensesSummary.Text = "إجمالي المصروفات: " + totalExpenses.ToString("N2") + " ج.م      |      إجمالي السلف: " + totalAdvances.ToString("N2") + " ج.م      (السلف لا تُخصم من صافي الربح)";
            }
            catch (Exception ex)
            {
                Fail("فشل تحميل سجل السلف والمصروفات", ex);
            }
        }

        private void btnAddExpense_Click(object sender, EventArgs e)
        {
            if (!TryGetBranchId(out int branchId)) { Warn("اختر الفرع أولاً."); return; }
            if (cmbExpenseType.SelectedIndex < 0) { Warn("اختر نوع الحركة (سلفة / مصروف)."); return; }
            if (!decimal.TryParse(txtExpenseAmount.Text, out decimal amount) || amount <= 0) { Warn("اكتب مبلغ صحيح أكبر من صفر."); return; }
            if (string.IsNullOrWhiteSpace(txtExpenseNotes.Text)) { Warn("اكتب البيان / الملاحظات."); return; }

            try
            {
                DataTable dtExp = new DataTable();
                dtExp.Columns.Add("Type", typeof(string));
                dtExp.Columns.Add("Amount", typeof(decimal));
                dtExp.Columns.Add("Notes", typeof(string));
                dtExp.Rows.Add(cmbExpenseType.Text, amount, txtExpenseNotes.Text.Trim());

                _branchService.SaveBranchExpense(branchId, dtExp);

                bool isAdvance = IsAdvanceType(cmbExpenseType.Text);
                txtExpenseAmount.Clear();
                txtExpenseNotes.Clear();
                Info(isAdvance ? "تم تسجيل السلفة بنجاح." : "تم تسجيل المصروف بنجاح.");
                LoadBranchExpensesHistoryGrid();
            }
            catch (Exception ex) { Fail("فشل حفظ الحركة", ex); }
        }

        // ===================== المرتجعات =====================
        private void LoadBranchReturnsHistoryGrid()
        {
            if (!TryGetBranchId(out int branchId)) return;
            try
            {
                DataTable dt = _branchService.GetBranchReturnsHistory(branchId);
                dgvReturnHistory.Rows.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    dgvReturnHistory.Rows.Add("", Convert.ToDateTime(row["ReturnDate"]).ToString("yyyy/MM/dd hh:mm tt"), row["CustomerName"], Convert.ToDecimal(row["TotalAmount"]).ToString("N2"));
                }
            }
            catch (Exception ex)
            {
                Fail("فشل تحميل سجل المرتجعات السابقة", ex);
            }
        }

        private void cmbReturnCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GetSelectedCustomerId() > 0)
            {
                chkDeductFromDebt.ForeColor = BranchTheme.Gold;
            }
            else
            {
                chkDeductFromDebt.Checked = false;
                chkDeductFromDebt.ForeColor = BranchTheme.Muted;
            }
        }

        private void chkDeductFromDebt_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDeductFromDebt.Checked && GetSelectedCustomerId() <= 0)
            {
                chkDeductFromDebt.Checked = false;
                Warn("اختر عميل (غير العميل النقدي) عشان يتخصم المرتجع من مديونيته.");
            }
        }

        private void btnAddReturnItem_Click(object sender, EventArgs e)
        {
            if (cmbReturnProduct.SelectedValue == null) { Warn("اختر المنتج المرتجع."); return; }
            if (!decimal.TryParse(txtReturnQty.Text, out decimal qty) || qty <= 0) { Warn("اكتب كمية صحيحة."); return; }
            if (!decimal.TryParse(txtReturnPrice.Text, out decimal price) || price < 0) { Warn("اكتب سعر صحيح."); return; }

            int productId = Convert.ToInt32(cmbReturnProduct.SelectedValue);
            bool found = false;

            foreach (DataGridViewRow r in dgvReturnItems.Rows)
            {
                if (r.Cells["colRetProdID"].Value != null
                    && Convert.ToInt32(r.Cells["colRetProdID"].Value) == productId
                    && Convert.ToDecimal(r.Cells["colRetPrice"].Value) == price)
                {
                    r.Cells["colRetQty"].Value = Convert.ToDecimal(r.Cells["colRetQty"].Value) + qty;
                    found = true;
                    break;
                }
            }
            if (!found) dgvReturnItems.Rows.Add("", productId, cmbReturnProduct.Text, qty, price);

            txtReturnQty.Clear();
            txtReturnPrice.Clear();
            cmbReturnProduct.SelectedIndex = -1;
            cmbReturnProduct.Focus();
        }

        private void btnRemoveReturnItem_Click(object sender, EventArgs e)
        {
            if (dgvReturnItems.SelectedRows.Count == 0) { Warn("اختر الصنف اللي عايز تحذفه من الجدول."); return; }
            foreach (DataGridViewRow r in dgvReturnItems.SelectedRows.Cast<DataGridViewRow>().ToList())
                dgvReturnItems.Rows.Remove(r);
        }

        private void btnSaveReturn_Click(object sender, EventArgs e)
        {
            if (!TryGetBranchId(out int branchId)) { Warn("اختر الفرع أولاً."); return; }
            int customerId = GetSelectedCustomerId();
            if (customerId < 0) { Warn("اختر العميل (أو عميل نقدي)."); return; }
            if (dgvReturnItems.Rows.Count == 0) { Warn("أضف صنف واحد على الأقل للمرتجع."); return; }

            bool deductFromDebt = chkDeductFromDebt.Checked && customerId > 0;
            bool isCashRefund = !deductFromDebt;

            decimal totalAmount = 0;
            List<SaleDetailModel> items = new List<SaleDetailModel>();

            foreach (DataGridViewRow r in dgvReturnItems.Rows)
            {
                if (r.IsNewRow || r.Cells["colRetQty"].Value == null || r.Cells["colRetPrice"].Value == null) continue;

                if (!decimal.TryParse(r.Cells["colRetQty"].Value.ToString(), out decimal q) || q <= 0 ||
                    !decimal.TryParse(r.Cells["colRetPrice"].Value.ToString(), out decimal p) || p < 0)
                {
                    Warn("الكمية أو السعر غير صحيح في السطر رقم " + (r.Index + 1) + ".");
                    return;
                }
                totalAmount += q * p;
                items.Add(new SaleDetailModel { ProductID = Convert.ToInt32(r.Cells["colRetProdID"].Value), Quantity = q, UnitPrice = p });
            }

            if (items.Count == 0 || totalAmount <= 0) { Warn("إجمالي المرتجع لازم يكون أكبر من صفر."); return; }

            string how = isCashRefund ? "سيتم صرف المبلغ نقدًا من درج الفرع." : "سيتم خصم المبلغ من مديونية العميل.";
            if (!Confirm("تأكيد حفظ المرتجع بإجمالي " + totalAmount.ToString("N2") + " ج.م؟\n" + how)) return;

            try
            {
                _branchService.SaveBranchReturn(branchId, customerId, totalAmount, isCashRefund, items);
                Info("تم تنفيذ وحفظ المرتجع بنجاح.");
                dgvReturnItems.Rows.Clear();
                chkDeductFromDebt.Checked = false;
                LoadBranchReturnsHistoryGrid();
                LoadBranchProductsGrid();
            }
            catch (Exception ex) { Fail("فشل حفظ المرتجع", ex); }
        }

        // ===================== الوردية اليومية =====================
        private void LoadModernDailyShiftData()
        {
            if (!TryGetBranchId(out int branchId)) return;

            try
            {
                BranchShiftStats stats = _branchService.GetBranchDailyShiftStats(branchId);

                lblCardProfit.Text = stats.NetProfit.ToString("N2");
                lblCardProfit.ForeColor = stats.NetProfit < 0 ? BranchTheme.RedText : BranchTheme.Tint(BranchTheme.Purple, 55);
                lblCardOwedToUs.Text = stats.OwedToUs.ToString("N2");
                lblCardExpenses.Text = stats.Expenses.ToString("N2");
                lblCardAdvances.Text = stats.Advances.ToString("N2");
                lblCardReturns.Text = stats.Returns.ToString("N2");
                lblCardInstaKurdi.Text = stats.InstaPayKurdi.ToString("N2");
                lblCardInstaHakosh.Text = stats.InstaPayHakosh.ToString("N2");
                lblCardSales.Text = stats.CashSales.ToString("N2");
                lblCardTotalSales.Text = stats.TotalSales.ToString("N2");

                lblCashierDeficit.Text = "عجز الكاشير المستحق: " + stats.CashierDeficit.ToString("N2") + " ج.م";
                lblCashierDeficit.ForeColor = stats.CashierDeficit > 0 ? BranchTheme.RedText : BranchTheme.GreenText;

                txtExpectedCash.Text = stats.ExpectedCash.ToString("0.00");
                txtActualCash.Text = stats.ExpectedCash.ToString("0.00");

                lblDiff.Text = "الفرق:";
                txtShiftDifference.Text = "0.00";
                txtShiftDifference.BackColor = BranchTheme.Field;
            }
            catch (Exception ex)
            {
                Fail("فشل تحميل إحصائيات الوردية اليومية", ex);
            }

            try
            {
                dgvDailyShift.DataSource = _branchService.GetBranchDailyTransactions(branchId);
                FormatMoneyColumns(dgvDailyShift, 2, 3);
            }
            catch (Exception ex)
            {
                Fail("فشل تحميل جدول حركات الوردية اليومية", ex);
            }
        }

        private void txtActualCash_TextChanged(object sender, EventArgs e)
        {
            decimal.TryParse(txtExpectedCash.Text, out decimal expected);

            if (decimal.TryParse(txtActualCash.Text, out decimal actual))
            {
                decimal diff = actual - expected;
                txtShiftDifference.Text = Math.Abs(diff).ToString("0.00");

                if (diff < 0)
                {
                    lblDiff.Text = "عجز بالدرج:";
                    txtShiftDifference.BackColor = Color.FromArgb(153, 27, 27);
                }
                else if (diff > 0)
                {
                    lblDiff.Text = "زيادة بالدرج:";
                    txtShiftDifference.BackColor = Color.FromArgb(21, 128, 61);
                }
                else
                {
                    lblDiff.Text = "الفرق:";
                    txtShiftDifference.BackColor = BranchTheme.Field;
                }
            }
        }

        private void btnActionClose_Click(object sender, EventArgs e)
        {
            if (!TryGetBranchId(out int branchId)) { Warn("برجاء اختيار الفرع."); return; }

            decimal.TryParse(txtExpectedCash.Text, out decimal expected);
            if (!decimal.TryParse(txtActualCash.Text, out decimal actual) || actual < 0) { Warn("اكتب رصيد الدرج الفعلي بشكل صحيح."); return; }

            decimal diff = actual - expected;

            if (expected == 0 && dgvDailyShift.Rows.Count == 0)
            {
                Msg("لا توجد حركات لتقفيلها!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string diffText = diff == 0 ? "" : (diff < 0 ? "\nسيتم تسجيل عجز بقيمة " : "\nسيتم تسجيل زيادة بقيمة ") + Math.Abs(diff).ToString("N2") + " ج.م";
            if (!Confirm("هل أنت متأكد من تقفيل الوردية للفرع؟" + diffText)) return;

            try
            {
                _branchService.CloseBranchShiftWithAudit(branchId, expected, actual, diff);
                Info("تم تقفيل الوردية بنجاح.");
                LoadModernDailyShiftData();
            }
            catch (Exception ex) { Fail("فشل تقفيل الوردية", ex); }
        }

        private bool PromptAmount(string title, string message, string buttonText, Color buttonColor, decimal maxAmount, out decimal amount)
        {
            decimal result = 0m;
            using (Form frm = new Form())
            {
                frm.Text = title;
                frm.ClientSize = new Size(500, 260);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.RightToLeft = RightToLeft.Yes;
                frm.RightToLeftLayout = true;
                frm.BackColor = BranchTheme.Surface;
                frm.FormBorderStyle = FormBorderStyle.FixedDialog;
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;

                Label lbl = new Label { Text = message, ForeColor = BranchTheme.Text, Location = new Point(20, 16), Size = new Size(460, 60), Font = BranchTheme.UiFont(13.5F, FontStyle.Bold), TextAlign = ContentAlignment.MiddleLeft };
                TextBox txt = new TextBox { Location = new Point(20, 100), Width = 460, Font = BranchTheme.UiFont(20F, FontStyle.Bold), TextAlign = HorizontalAlignment.Center, BackColor = BranchTheme.Input, ForeColor = BranchTheme.Gold, BorderStyle = BorderStyle.FixedSingle };
                Button btn = new Button { Text = buttonText, Location = new Point(100, 170), Size = new Size(300, 60), BackColor = buttonColor, ForeColor = Color.White, Font = BranchTheme.UiFont(14.5F, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
                btn.FlatAppearance.BorderSize = 0;

                txt.KeyPress += (s, ev) =>
                {
                    if (!char.IsControl(ev.KeyChar) && !char.IsDigit(ev.KeyChar) && ev.KeyChar != '.') ev.Handled = true;
                    if (ev.KeyChar == '.' && ((TextBox)s).Text.IndexOf('.') > -1) ev.Handled = true;
                };

                btn.Click += (s, ev) =>
                {
                    if (decimal.TryParse(txt.Text, out decimal amt) && amt > 0 && amt <= maxAmount)
                    {
                        result = amt;
                        frm.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        Warn("مبلغ غير صحيح أو يتخطى الحد المسموح (" + maxAmount.ToString("N2") + " ج.م).");
                    }
                };

                frm.Controls.Add(lbl);
                frm.Controls.Add(txt);
                frm.Controls.Add(btn);
                frm.AcceptButton = btn;

                bool ok = frm.ShowDialog(this) == DialogResult.OK;
                amount = result;
                return ok;
            }
        }

        private void btnPayDeficit_Click(object sender, EventArgs e)
        {
            if (!TryGetBranchId(out int branchId)) { Warn("اختر الفرع أولاً."); return; }

            BranchShiftStats stats;
            try
            {
                stats = _branchService.GetBranchDailyShiftStats(branchId);
            }
            catch (Exception ex)
            {
                Fail("فشل قراءة إحصائيات العجز", ex);
                return;
            }

            if (stats.CashierDeficit <= 0)
            {
                Msg("لا يوجد عجز مستحق على هذا الفرع أو الكاشير.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string message = "العجز المتراكم المستحق: " + stats.CashierDeficit.ToString("N2") + " ج.م\nأدخل المبلغ الذي سيقوم الكاشير بسداده الآن:";
            if (PromptAmount("تسوية وسداد عجز كاشير", message, "سداد وإضافة للدرج", BranchTheme.Orange, stats.CashierDeficit, out decimal amount))
            {
                try
                {
                    _branchService.PayBranchDeficit(branchId, amount);
                    LoadModernDailyShiftData();
                }
                catch (Exception ex) { Fail("فشل تسديد العجز", ex); }
            }
        }

        // ===================== التقفيل الشهري =====================
        private void LoadModernMonthlyData()
        {
            if (!TryGetBranchId(out int branchId)) return;

            try
            {
                BranchShiftStats stats = _branchService.GetBranchMonthlyShiftStats(branchId);

                lblMonthlyProfit.Text = stats.NetProfit.ToString("N2");
                lblMonthlyProfit.ForeColor = stats.NetProfit < 0 ? BranchTheme.RedText : BranchTheme.Tint(BranchTheme.Purple, 55);
                lblMonthlyOwed.Text = stats.OwedToUs.ToString("N2");
                lblMonthlyExpenses.Text = stats.Expenses.ToString("N2");
                lblMonthlyAdvances.Text = stats.Advances.ToString("N2");
                lblMonthlyReturns.Text = stats.Returns.ToString("N2");
                lblMonthlyInstaKurdi.Text = stats.InstaPayKurdi.ToString("N2");
                lblMonthlyInstaHakosh.Text = stats.InstaPayHakosh.ToString("N2");
                lblMonthlySales.Text = stats.CashSales.ToString("N2");
                lblMTotalSalesVal.Text = stats.TotalSales.ToString("N2");

                lblMonthlyBalanceCard.Text = stats.NetCash.ToString("N2");
                lblMonthlyBalanceCard.ForeColor = stats.NetCash < 0 ? BranchTheme.RedText : BranchTheme.GreenText;
            }
            catch (Exception ex)
            {
                Fail("فشل تحميل إحصائيات التقفيل الشهري", ex);
            }

            try
            {
                dgvMonthlyShift.DataSource = _branchService.GetMonthlyTreasuryLog(branchId);
                if (dgvMonthlyShift.Columns.Count > 0) dgvMonthlyShift.Columns[0].DefaultCellStyle.Format = "yyyy/MM/dd hh:mm tt";
                FormatMoneyColumns(dgvMonthlyShift, 1, 2);
            }
            catch (Exception ex)
            {
                Fail("فشل تحميل سجل الخزنة الشهرية", ex);
            }
        }

        private void btnActionMonthly_Click(object sender, EventArgs e)
        {
            if (!TryGetBranchId(out int branchId)) { Warn("اختر الفرع أولاً."); return; }

            try
            {
                BranchShiftStats stats = _branchService.GetBranchMonthlyShiftStats(branchId);
                bool nothingToClose = stats.NetCash == 0 && dgvMonthlyShift.Rows.Count == 0
                                      && stats.TotalSales == 0 && stats.Expenses == 0 && stats.Advances == 0 && stats.Returns == 0;
                if (nothingToClose)
                {
                    Msg("الخزنة متقفلة بالفعل ولا يوجد رصيد أو حركات لترحيلها!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (!Confirm("تأكيد ترحيل الخزنة الشهرية وأرشفة الشهر الحالي؟\nرصيد الكاش المرحّل: " + stats.NetCash.ToString("N2") + " ج.م\nالبيانات ستظل محفوظة ويمكن مراجعتها من الأرشيف.\n\nملحوظة: الورديات المفتوحة (غير المقفولة) مش هتدخل في الشهر ده."))
                    return;

                _branchService.CloseMonthlyBranchTreasury(branchId);
                Info("تم ترحيل الشهرية وأرشفتها بنجاح.");
                LoadModernMonthlyData();
            }
            catch (Exception ex) { Fail("فشل عملية الترحيل الشهري", ex); }
        }

        private void btnViewMonthlyArchive_Click(object sender, EventArgs e)
        {
            if (!TryGetBranchId(out int branchId)) { Warn("برجاء اختيار الفرع أولاً."); return; }

            DataTable dtArchive;
            try
            {
                dtArchive = _branchService.GetArchivedMonthsSummary(branchId);
            }
            catch (Exception ex)
            {
                Fail("فشل تحميل أرشيف الشهور", ex);
                return;
            }

            if (dtArchive.Rows.Count == 0)
            {
                Msg("لا توجد شهور مقفولة (أرشيف) لهذا الفرع بعد.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (Form frm = new Form())
            {
                frm.Width = 1000;
                frm.Height = 700;
                frm.Text = "أرشيف الخزنة الشهرية - " + cmbLocations.Text;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.RightToLeft = RightToLeft.Yes;
                frm.RightToLeftLayout = true;
                frm.BackColor = BranchTheme.Bg;
                frm.MinimizeBox = false;

                FlowLayoutPanel pnlTop = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 80, BackColor = BranchTheme.Header, RightToLeft = RightToLeft.Yes, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(16, 16, 16, 0) };
                Label lblSelect = new Label { Text = "اختر الشهر المقفول:", ForeColor = BranchTheme.Text, Font = BranchTheme.UiFont(13.5F, FontStyle.Bold), AutoSize = true, Margin = new Padding(6, 8, 6, 0) };
                ComboBox cmbMonths = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Font = BranchTheme.UiFont(13.5F, FontStyle.Bold),
                    Width = 350,
                    BackColor = BranchTheme.Surface,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(6, 4, 6, 0)
                };
                cmbMonths.DataSource = dtArchive;
                cmbMonths.DisplayMember = "PeriodLabel";
                cmbMonths.ValueMember = "ArchiveMonth";

                Label lblNet = new Label { ForeColor = BranchTheme.GreenText, Font = BranchTheme.UiFont(14.5F, FontStyle.Bold), AutoSize = true, Margin = new Padding(40, 8, 6, 0) };

                DataGridView dgv = new DataGridView { Dock = DockStyle.Fill };
                ApplyModernGridDesign(dgv);
                dgv.ReadOnly = true;

                EventHandler loadDetails = (s, ev) =>
                {
                    if (cmbMonths.SelectedItem == null) return;
                    try
                    {
                        DataRowView drv = (DataRowView)cmbMonths.SelectedItem;
                        int y = Convert.ToInt32(drv["ArchiveYear"]);
                        int m = Convert.ToInt32(drv["ArchiveMonth"]);
                        dgv.DataSource = _branchService.GetArchivedMonthDetails(branchId, y, m);
                        if (dgv.Columns.Count > 0) dgv.Columns[0].DefaultCellStyle.Format = "yyyy/MM/dd hh:mm tt";
                        FormatMoneyColumns(dgv, 1, 2);
                        lblNet.Text = "صافي حركة الشهر: " + Convert.ToDecimal(drv["NetAmount"]).ToString("N2") + " ج.م";
                    }
                    catch (Exception ex)
                    {
                        Fail("فشل تحميل تفاصيل الشهر المختار", ex);
                    }
                };
                cmbMonths.SelectedIndexChanged += loadDetails;

                pnlTop.Controls.Add(lblSelect);
                pnlTop.Controls.Add(cmbMonths);
                pnlTop.Controls.Add(lblNet);

                frm.Controls.Add(dgv);
                frm.Controls.Add(pnlTop);

                frm.Shown += (s, ev) => loadDetails(null, EventArgs.Empty);
                frm.ShowDialog(this);
            }
        }


        // =========================================================================================
        private void ApplyCustomUIStyles()
        {
            // Tabs Row
            StyleBar(tlpTabs, DockStyle.Top, 60, Padding.Empty, Color.Transparent);
            SetColumns(tlpTabs, -16.66F, -16.66F, -16.66F, -16.66F, -16.66F, -16.7F);
            SetRows(tlpTabs, -100F);

            Button[] tabButtons = { btnTabAddBranch, btnTabProducts, btnTabReturns, btnTabExpenses, btnTabClose, btnTabMonthly };
            string[] tabTexts = { "تسجيل الفروع", "أرصدة المنتجات", "المرتجعات", "السلف والمصروفات", "الوردية اليومية", "التقفيل الشهري" };
            for (int i = 0; i < tabButtons.Length; i++)
            {
                StyleTab(tabButtons[i], tabTexts[i]);
                tlpTabs.Controls.Add(tabButtons[i], i, 0);
            }

            // Tab 1: Add Branch
            StyleBar(pnlAddBranchTop, DockStyle.Top, 90, new Padding(16, 0, 16, 0), BranchTheme.Surface);
            SetColumns(pnlAddBranchTop, 160F, -100F, 110F, 230F, 150F, 150F, 150F);
            SetRows(pnlAddBranchTop, -100F);
            StyleLabel(lblBranchNameTitle, "اسم الفرع/المخزن:");
            StyleTextBox(txtBranchName, false);
            StyleLabel(lblBranchTypeTitle, "نوع المكان:");
            StyleCombo(cmbBranchType);
            StyleButton(btnSaveNewBranch, "حفظ ✔", BranchTheme.Green, 18, 13F);
            StyleButton(btnEditBranch, "تعديل 📝", BranchTheme.Accent, 18, 13F);
            StyleButton(btnClearBranch, "تفريغ 🧹", BranchTheme.Slate, 18, 13F);
            pnlAddBranchTop.Controls.Add(lblBranchNameTitle, 0, 0);
            pnlAddBranchTop.Controls.Add(txtBranchName, 1, 0);
            pnlAddBranchTop.Controls.Add(lblBranchTypeTitle, 2, 0);
            pnlAddBranchTop.Controls.Add(cmbBranchType, 3, 0);
            pnlAddBranchTop.Controls.Add(btnSaveNewBranch, 4, 0);
            pnlAddBranchTop.Controls.Add(btnEditBranch, 5, 0);
            pnlAddBranchTop.Controls.Add(btnClearBranch, 6, 0);

            // ================= إضافة أعمدة جدول الفروع =================
            dgvBranches.Columns.AddRange(new DataGridViewColumn[] { colBranchSerial, colBranchID, colBranchName, colBranchType, colBranchStatus });
            SetCol(colBranchSerial, "colBranchSerial", "م", 30F);
            SetCol(colBranchID, "colBranchID", "", 10F, false);
            SetCol(colBranchName, "colBranchName", "اسم الفرع أو المخزن", 220F);
            SetCol(colBranchType, "colBranchType", "نوع المكان", 100F);
            SetCol(colBranchStatus, "colBranchStatus", "الحالة", 60F);

            // Tab 2: Products
            StyleBar(pnlProductsTop, DockStyle.Top, 90, new Padding(16, 0, 16, 0), BranchTheme.Surface);
            SetColumns(pnlProductsTop, 120F, -100F, 110F, 150F, 80F, 130F, 200F);
            SetRows(pnlProductsTop, -100F);
            StyleLabel(lblProdNameTitle, "اسم المنتج:");
            StyleTextBox(txtNewProdName, false);
            StyleLabel(lblProdPriceTitle, "سعر الشراء:");
            StyleTextBox(txtNewPurchasePrice, true);
            StyleLabel(lblProdQtyTitle, "الكمية:");
            StyleTextBox(txtNewQty, true);
            StyleButton(btnAddBranchProduct, "إضافة منتج ➕", BranchTheme.Green, 18, 13F);
            pnlProductsTop.Controls.Add(lblProdNameTitle, 0, 0);
            pnlProductsTop.Controls.Add(txtNewProdName, 1, 0);
            pnlProductsTop.Controls.Add(lblProdPriceTitle, 2, 0);
            pnlProductsTop.Controls.Add(txtNewPurchasePrice, 3, 0);
            pnlProductsTop.Controls.Add(lblProdQtyTitle, 4, 0);
            pnlProductsTop.Controls.Add(txtNewQty, 5, 0);
            pnlProductsTop.Controls.Add(btnAddBranchProduct, 6, 0);

            StyleBar(pnlProductsBottom, DockStyle.Bottom, 84, new Padding(0, 8, 0, 0), BranchTheme.Bg);
            SetColumns(pnlProductsBottom, 280F, 280F, -100F);
            SetRows(pnlProductsBottom, -100F);
            StyleButton(btnSaveBranchProducts, "حفظ التعديلات ✔", BranchTheme.Accent, 10, 13F);
            StyleButton(btnExportProducts, "طباعة / تصدير جرد 🖨", BranchTheme.Orange, 10, 13F);
            lblTotalInventoryValue.Dock = DockStyle.Fill;
            lblTotalInventoryValue.Font = BranchTheme.UiFont(16F, FontStyle.Bold);
            lblTotalInventoryValue.ForeColor = BranchTheme.Gold;
            lblTotalInventoryValue.Text = "إجمالي قيمة بضاعة الفرع: 0.00 ج.م";
            lblTotalInventoryValue.TextAlign = ContentAlignment.MiddleRight;
            pnlProductsBottom.Controls.Add(btnSaveBranchProducts, 0, 0);
            pnlProductsBottom.Controls.Add(btnExportProducts, 1, 0);
            pnlProductsBottom.Controls.Add(lblTotalInventoryValue, 2, 0);

            // ================= إضافة أعمدة جدول المنتجات =================
            dgvBranchProducts.Columns.AddRange(new DataGridViewColumn[] { colProdSerial, colProdID, colProdName, colPurchasePrice, colStockQty });
            SetCol(colProdSerial, "colProdSerial", "م", 30F);
            SetCol(colProdID, "colProdID", "", 10F, false);
            SetCol(colProdName, "colProdName", "اسم المنتج", 220F);
            SetCol(colPurchasePrice, "colPurchasePrice", "سعر الشراء", 90F);
            SetCol(colStockQty, "colStockQty", "الكمية المتاحة بالفرع", 110F);

            // Tab 3: Returns
            StyleBar(tlpReturnsRow1, DockStyle.Top, 64, Padding.Empty, Color.Transparent);
            SetColumns(tlpReturnsRow1, 100F, -45F, 100F, -55F);
            SetRows(tlpReturnsRow1, -100F);
            StyleLabel(lblRetCustomerTitle, "العميل:");
            StyleCombo(cmbReturnCustomer);
            StyleLabel(lblRetProductTitle, "المنتج:");
            StyleCombo(cmbReturnProduct);
            tlpReturnsRow1.Controls.Add(lblRetCustomerTitle, 0, 0);
            tlpReturnsRow1.Controls.Add(cmbReturnCustomer, 1, 0);
            tlpReturnsRow1.Controls.Add(lblRetProductTitle, 2, 0);
            tlpReturnsRow1.Controls.Add(cmbReturnProduct, 3, 0);

            StyleBar(tlpReturnsRow2, DockStyle.Top, 64, Padding.Empty, Color.Transparent);
            SetColumns(tlpReturnsRow2, 100F, 140F, 100F, 140F, -100F, 220F);
            SetRows(tlpReturnsRow2, -100F);
            StyleLabel(lblRetQtyTitle, "الكمية:");
            StyleTextBox(txtReturnQty, true);
            StyleLabel(lblRetPriceTitle, "السعر:");
            StyleTextBox(txtReturnPrice, true);
            chkDeductFromDebt.Dock = DockStyle.Fill;
            chkDeductFromDebt.BackColor = Color.Transparent;
            chkDeductFromDebt.ForeColor = BranchTheme.Muted;
            chkDeductFromDebt.Font = BranchTheme.UiFont(12.5F, FontStyle.Bold);
            chkDeductFromDebt.Margin = new Padding(12, 0, 4, 0);
            chkDeductFromDebt.Text = "خصم من المديونية (لا ترد كاش)";
            chkDeductFromDebt.UseVisualStyleBackColor = false;
            StyleButton(btnAddReturnItem, "إضافة للمرتجع ➕", BranchTheme.Accent, 10, 12F);
            tlpReturnsRow2.Controls.Add(lblRetQtyTitle, 0, 0);
            tlpReturnsRow2.Controls.Add(txtReturnQty, 1, 0);
            tlpReturnsRow2.Controls.Add(lblRetPriceTitle, 2, 0);
            tlpReturnsRow2.Controls.Add(txtReturnPrice, 3, 0);
            tlpReturnsRow2.Controls.Add(chkDeductFromDebt, 4, 0);
            tlpReturnsRow2.Controls.Add(btnAddReturnItem, 5, 0);

            StyleBar(pnlReturnsBottom, DockStyle.Bottom, 80, new Padding(0, 8, 0, 8), BranchTheme.Bg);
            SetColumns(pnlReturnsBottom, 350F, 220F, -100F);
            SetRows(pnlReturnsBottom, -100F);
            StyleButton(btnSaveReturn, "تأكيد وحفظ المرتجع ✔", BranchTheme.Green, 4, 14F);
            StyleButton(btnRemoveReturnItem, "حذف الصنف المحدد 🗑", BranchTheme.Red, 4, 12F);
            pnlReturnsBottom.Controls.Add(btnSaveReturn, 0, 0);
            pnlReturnsBottom.Controls.Add(btnRemoveReturnItem, 1, 0);

            // ================= إضافة أعمدة جداول المرتجعات =================
            dgvReturnItems.Columns.AddRange(new DataGridViewColumn[] { colRetSerial, colRetProdID, colRetProdName, colRetQty, colRetPrice });
            SetCol(colRetSerial, "colRetSerial", "م", 30F);
            SetCol(colRetProdID, "colRetProdID", "", 10F, false);
            SetCol(colRetProdName, "colRetProdName", "اسم المنتج المرتجع", 220F);
            SetCol(colRetQty, "colRetQty", "الكمية", 80F);
            SetCol(colRetPrice, "colRetPrice", "السعر", 80F);

            dgvReturnHistory.Columns.AddRange(new DataGridViewColumn[] { colRetHistSerial, colRetHistDate, colRetHistCustomer, colRetHistAmount });
            SetCol(colRetHistSerial, "colRetHistSerial", "م", 30F);
            SetCol(colRetHistDate, "colRetHistDate", "التاريخ والوقت", 100F);
            SetCol(colRetHistCustomer, "colRetHistCustomer", "اسم العميل", 150F);
            SetCol(colRetHistAmount, "colRetHistAmount", "الإجمالي المسترد", 100F);

            // Tab 4: Expenses
            StyleBar(pnlExpensesTop, DockStyle.Top, 90, new Padding(16, 0, 16, 0), BranchTheme.Surface);
            SetColumns(pnlExpensesTop, 110F, 210F, 90F, 150F, 100F, -100F, 220F);
            SetRows(pnlExpensesTop, -100F);
            StyleLabel(lblExpTypeTitle, "نوع الحركة:");
            StyleCombo(cmbExpenseType);
            StyleLabel(lblExpAmountTitle, "المبلغ:");
            StyleTextBox(txtExpenseAmount, true);
            StyleLabel(lblExpNoteTitle, "ملاحظات:");
            StyleTextBox(txtExpenseNotes, false);
            StyleButton(btnAddExpense, "تسجيل على الفرع ➕", BranchTheme.Red, 18, 13F);
            pnlExpensesTop.Controls.Add(lblExpTypeTitle, 0, 0);
            pnlExpensesTop.Controls.Add(cmbExpenseType, 1, 0);
            pnlExpensesTop.Controls.Add(lblExpAmountTitle, 2, 0);
            pnlExpensesTop.Controls.Add(txtExpenseAmount, 3, 0);
            pnlExpensesTop.Controls.Add(lblExpNoteTitle, 4, 0);
            pnlExpensesTop.Controls.Add(txtExpenseNotes, 5, 0);
            pnlExpensesTop.Controls.Add(btnAddExpense, 6, 0);

            // ================= إضافة أعمدة جدول المصروفات =================
            dgvExpenses.Columns.AddRange(new DataGridViewColumn[] { colExpSerial, colExpDate, colExpType, colExpAmount, colExpNote });
            SetCol(colExpSerial, "colExpSerial", "م", 30F);
            SetCol(colExpDate, "colExpDate", "التاريخ والوقت", 110F);
            SetCol(colExpType, "colExpType", "نوع الحركة", 90F);
            SetCol(colExpAmount, "colExpAmount", "المبلغ", 90F);
            SetCol(colExpNote, "colExpNote", "البيان", 220F);

            // Tab 5: Close Shift
            StyleBar(tlpDailyCards, DockStyle.Top, 140, Padding.Empty, BranchTheme.Bg);
            SetColumns(tlpDailyCards, -11.11F, -11.11F, -11.11F, -11.11F, -11.11F, -11.11F, -11.11F, -11.11F, -11.12F);
            SetRows(tlpDailyCards, 46F, 94F);
            SetupCard(lblCardProfitTitle, lblCardProfit, "صافي الربح 🎯", BranchTheme.Purple);
            SetupCard(lblCardOwedTitle, lblCardOwedToUs, "آجل للشركة", Color.FromArgb(59, 130, 246));
            SetupCard(lblCardExpensesTitle, lblCardExpenses, "مصروفات (-)", Color.FromArgb(220, 38, 38));
            SetupCard(lblCardAdvancesTitle, lblCardAdvances, "سلف (-)", Color.FromArgb(234, 120, 10));
            SetupCard(lblCardReturnsTitle, lblCardReturns, "مرتجعات (-)", Color.FromArgb(219, 39, 119));
            SetupCard(lblCardInstaKurdiTitle, lblCardInstaKurdi, "إنستاباي الكردي", Color.FromArgb(13, 148, 136));
            SetupCard(lblCardInstaHakoshTitle, lblCardInstaHakosh, "إنستاباي الحكش", Color.FromArgb(99, 102, 241));
            SetupCard(lblCardSalesTitle, lblCardSales, "مبيعات (كاش)", Color.FromArgb(22, 163, 74));
            SetupCard(lblCardTotalSalesTitle, lblCardTotalSales, "إجمالي اليوم", Color.FromArgb(2, 132, 199));
            AddCard(tlpDailyCards, 0, lblCardProfitTitle, lblCardProfit);
            AddCard(tlpDailyCards, 1, lblCardOwedTitle, lblCardOwedToUs);
            AddCard(tlpDailyCards, 2, lblCardExpensesTitle, lblCardExpenses);
            AddCard(tlpDailyCards, 3, lblCardAdvancesTitle, lblCardAdvances);
            AddCard(tlpDailyCards, 4, lblCardReturnsTitle, lblCardReturns);
            AddCard(tlpDailyCards, 5, lblCardInstaKurdiTitle, lblCardInstaKurdi);
            AddCard(tlpDailyCards, 6, lblCardInstaHakoshTitle, lblCardInstaHakosh);
            AddCard(tlpDailyCards, 7, lblCardSalesTitle, lblCardSales);
            AddCard(tlpDailyCards, 8, lblCardTotalSalesTitle, lblCardTotalSales);

            StyleBar(tlpDailyAction, DockStyle.Top, 100, new Padding(16, 14, 16, 14), BranchTheme.Surface);
            SetColumns(tlpDailyAction, 190F, -33F, 190F, -33F, 120F, -34F, 240F);
            SetRows(tlpDailyAction, -100F);
            StyleLabel(lblExpCash, "الدفتري المفترض:");
            lblExpCash.ForeColor = Color.FromArgb(96, 165, 250);
            StyleLabel(lblActCash, "رصيد الدرج الفعلي:");
            lblActCash.ForeColor = BranchTheme.Gold;
            StyleLabel(lblDiff, "الفرق:");
            StyleTextBox(txtExpectedCash, true);
            txtExpectedCash.ReadOnly = true;
            txtExpectedCash.BackColor = BranchTheme.Field;
            txtExpectedCash.Font = BranchTheme.UiFont(18F, FontStyle.Bold);
            txtExpectedCash.Text = "0.00";
            StyleTextBox(txtActualCash, true);
            txtActualCash.ForeColor = BranchTheme.Gold;
            txtActualCash.Font = BranchTheme.UiFont(20F, FontStyle.Bold);
            txtActualCash.Text = "0.00";
            StyleTextBox(txtShiftDifference, true);
            txtShiftDifference.ReadOnly = true;
            txtShiftDifference.BackColor = BranchTheme.Field;
            txtShiftDifference.Font = BranchTheme.UiFont(18F, FontStyle.Bold);
            txtShiftDifference.Text = "0.00";
            StyleButton(btnActionClose, "تقفيل الوردية 🔒", BranchTheme.Red, 4, 14F);
            btnActionClose.Margin = new Padding(14, 4, 0, 4);
            tlpDailyAction.Controls.Add(lblExpCash, 0, 0);
            tlpDailyAction.Controls.Add(txtExpectedCash, 1, 0);
            tlpDailyAction.Controls.Add(lblActCash, 2, 0);
            tlpDailyAction.Controls.Add(txtActualCash, 3, 0);
            tlpDailyAction.Controls.Add(lblDiff, 4, 0);
            tlpDailyAction.Controls.Add(txtShiftDifference, 5, 0);
            tlpDailyAction.Controls.Add(btnActionClose, 6, 0);

            // Tab 6: Monthly
            StyleBar(tlpMonthlyCards, DockStyle.Top, 140, Padding.Empty, BranchTheme.Bg);
            SetColumns(tlpMonthlyCards, -11.11F, -11.11F, -11.11F, -11.11F, -11.11F, -11.11F, -11.11F, -11.11F, -11.12F);
            SetRows(tlpMonthlyCards, 46F, 94F);
            SetupCard(lblMProfitTitle, lblMonthlyProfit, "صافي الربح 🎯", BranchTheme.Purple);
            SetupCard(lblMOwedTitle, lblMonthlyOwed, "آجل للشركة", Color.FromArgb(59, 130, 246));
            SetupCard(lblMExpTitle, lblMonthlyExpenses, "مصروفات الشهر", Color.FromArgb(220, 38, 38));
            SetupCard(lblMAdvancesTitle, lblMonthlyAdvances, "سلف الشهر (-)", Color.FromArgb(234, 120, 10));
            SetupCard(lblMReturnsTitle, lblMonthlyReturns, "مرتجعات الشهر", Color.FromArgb(219, 39, 119));
            SetupCard(lblMInstaKurdiTitle, lblMonthlyInstaKurdi, "إنستاباي الكردي", Color.FromArgb(13, 148, 136));
            SetupCard(lblMInstaHakoshTitle, lblMonthlyInstaHakosh, "إنستاباي الحكش", Color.FromArgb(99, 102, 241));
            SetupCard(lblMSalesTitle, lblMonthlySales, "مبيعات (كاش)", Color.FromArgb(22, 163, 74));
            SetupCard(lblMTotalSalesTitle, lblMTotalSalesVal, "إجمالي الشهر", Color.FromArgb(2, 132, 199));
            AddCard(tlpMonthlyCards, 0, lblMProfitTitle, lblMonthlyProfit);
            AddCard(tlpMonthlyCards, 1, lblMOwedTitle, lblMonthlyOwed);
            AddCard(tlpMonthlyCards, 2, lblMExpTitle, lblMonthlyExpenses);
            AddCard(tlpMonthlyCards, 3, lblMAdvancesTitle, lblMonthlyAdvances);
            AddCard(tlpMonthlyCards, 4, lblMReturnsTitle, lblMonthlyReturns);
            AddCard(tlpMonthlyCards, 5, lblMInstaKurdiTitle, lblMonthlyInstaKurdi);
            AddCard(tlpMonthlyCards, 6, lblMInstaHakoshTitle, lblMonthlyInstaHakosh);
            AddCard(tlpMonthlyCards, 7, lblMSalesTitle, lblMonthlySales);
            AddCard(tlpMonthlyCards, 8, lblMTotalSalesTitle, lblMTotalSalesVal);

            StyleBar(tlpMonthlyAction, DockStyle.Top, 98, new Padding(16, 10, 16, 10), BranchTheme.Surface);
            SetColumns(tlpMonthlyAction, 310F, -100F, 240F, 270F);
            SetRows(tlpMonthlyAction, -100F);
            StyleButton(btnViewMonthlyArchive, "📂 أرشيف الشهور", BranchTheme.Slate, 10, 12.5F);
            StyleButton(btnActionMonthly, "تقفيل وأرشفة الشهر 🔒", BranchTheme.Green, 10, 13F);
            tlpMonthlyAction.Controls.Add(lblMonthlyTitle, 0, 0);
            tlpMonthlyAction.Controls.Add(lblMonthlyBalanceCard, 1, 0);
            tlpMonthlyAction.Controls.Add(btnViewMonthlyArchive, 2, 0);
            tlpMonthlyAction.Controls.Add(btnActionMonthly, 3, 0);
        }

        private static void SetColumns(TableLayoutPanel t, params float[] widths)
        {
            t.ColumnStyles.Clear();
            t.ColumnCount = widths.Length;
            foreach (float w in widths)
            {
                if (w >= 0) t.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, w));
                else t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, -w));
            }
        }

        private static void SetRows(TableLayoutPanel t, params float[] heights)
        {
            t.RowStyles.Clear();
            t.RowCount = heights.Length;
            foreach (float h in heights)
            {
                if (h >= 0) t.RowStyles.Add(new RowStyle(SizeType.Absolute, h));
                else t.RowStyles.Add(new RowStyle(SizeType.Percent, -h));
            }
        }

        private static void StyleBar(TableLayoutPanel t, DockStyle dock, int height, Padding padding, Color back)
        {
            t.BackColor = back;
            t.Dock = dock;
            t.Height = height;
            t.Padding = padding;
            t.Margin = Padding.Empty;
            t.RightToLeft = RightToLeft.Yes;
        }

        private static void StyleTab(Button b, string text)
        {
            b.Dock = DockStyle.Fill;
            b.Margin = new Padding(4, 0, 4, 0);
            b.BackColor = BranchTheme.Surface;
            b.ForeColor = BranchTheme.Text;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.Font = BranchTheme.UiFont(13F, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            b.Text = text;
            b.UseVisualStyleBackColor = false;
        }

        private static void StyleLabel(Label l, string text)
        {
            l.AutoSize = false;
            l.Dock = DockStyle.Fill;
            l.Margin = new Padding(4, 0, 4, 0);
            l.Font = BranchTheme.UiFont(12.5F, FontStyle.Bold);
            l.ForeColor = BranchTheme.Muted;
            l.Text = text;
            l.TextAlign = ContentAlignment.MiddleLeft;
        }

        private static void StyleTextBox(TextBox t, bool numeric)
        {
            t.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            t.BackColor = BranchTheme.Input;
            t.ForeColor = BranchTheme.Text;
            t.BorderStyle = BorderStyle.FixedSingle;
            t.Font = BranchTheme.UiFont(14F, numeric ? FontStyle.Bold : FontStyle.Regular);
            t.Margin = new Padding(6, 0, 6, 0);
            if (numeric) t.TextAlign = HorizontalAlignment.Center;
        }

        private static void StyleCombo(ComboBox c)
        {
            c.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            c.FlatStyle = FlatStyle.Flat;
            c.BackColor = BranchTheme.Input;
            c.ForeColor = BranchTheme.Text;
            c.Font = BranchTheme.UiFont(13.5F);
            c.Margin = new Padding(6, 0, 6, 0);
        }

        private static void StyleButton(Button b, string text, Color back, int vMargin, float size = 13F)
        {
            b.Dock = DockStyle.Fill;
            b.Margin = new Padding(6, vMargin, 6, vMargin);
            b.BackColor = back;
            b.ForeColor = Color.White;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = ControlPaint.Light(back, 0.15F);
            b.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(back, 0.05F);
            b.Font = BranchTheme.UiFont(size, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            b.Text = text;
            b.UseVisualStyleBackColor = false;
        }

        private static void SetCol(DataGridViewTextBoxColumn c, string name, string header, float weight, bool visible = true)
        {
            c.Name = name;
            c.HeaderText = header;
            c.FillWeight = weight;
            c.Visible = visible;
            c.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private static void SetupCard(Label title, Label value, string titleText, Color accent)
        {
            title.Dock = DockStyle.Fill;
            title.Margin = new Padding(5, 4, 5, 0);
            title.BackColor = accent;
            title.ForeColor = Color.White;
            title.Font = BranchTheme.UiFont(11.5F, FontStyle.Bold);
            title.Text = titleText;
            title.TextAlign = ContentAlignment.MiddleCenter;

            value.Dock = DockStyle.Fill;
            value.Margin = new Padding(5, 0, 5, 4);
            value.BackColor = BranchTheme.Surface;
            value.ForeColor = BranchTheme.Tint(accent, 55);
            value.Font = BranchTheme.UiFont(18F, FontStyle.Bold);
            value.Text = "0.00";
            value.TextAlign = ContentAlignment.MiddleCenter;
        }

        private static void AddCard(TableLayoutPanel t, int col, Label title, Label value)
        {
            t.Controls.Add(title, col, 0);
            t.Controls.Add(value, col, 1);
        }
    }
}