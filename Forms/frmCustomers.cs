using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using MajesticDesktop.Models;
using MajesticDesktop.Services;

namespace MajesticDesktop.Forms
{
    public partial class frmCustomers : Form
    {
        private static readonly Color ClrDebt = Color.FromArgb(255, 110, 110);
        private static readonly Color ClrCredit = Color.FromArgb(110, 255, 160);
        private static readonly Font GridFont = new Font("Segoe UI", 11F, FontStyle.Bold);
        private static readonly Font GridHeaderFont = new Font("Segoe UI", 12F, FontStyle.Bold);
        private const string AllRegionsText = "كل المناطق";
        private const int SYSTEM_CASH_CUSTOMER_ID = CustomerService.SystemCashCustomerId;

        private CustomerService _customerService;
        private List<Customer> _customersList = new List<Customer>();
        private List<Customer> _viewList = new List<Customer>();

        private int _selectedCustomerId;
        private decimal _currentCustomerInitialBalance;
        private bool _suppressSelection;
        private bool _suppressFilter;
        private int _sortCol = -1;
        private bool _sortAsc = true;
        private int _printRowIndex;
        private int _printPage;

        private readonly Timer _filterTimer = new Timer { Interval = 250 };

        public frmCustomers()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            SetupGrids();
            SetupInputs();
            EnableDoubleBuffering(dgvCustomers);
            EnableDoubleBuffering(dgvLedger);

            _filterTimer.Tick += (s, e) => { _filterTimer.Stop(); ApplyFilters(); };
            FormClosed += (s, e) => _filterTimer.Dispose();
        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                // الاتصال بالداتابيز جوه try عشان أي مشكلة تظهر برسالة بدل إيرور غير معالج
                _customerService = new CustomerService();

                try { splitContainer1.SplitterDistance = Math.Max(300, (int)(splitContainer1.Width * 0.56)); }
                catch (InvalidOperationException) { /* الحجم لسه مش جاهز */ }

                LoadLocations();
            }
            catch (Exception ex)
            {
                ShowError("تعذر فتح شاشة العملاء", ex);
            }
        }

        // ======================= رسائل (RTL) =======================
        private DialogResult Msg(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
            MessageBoxDefaultButton def = MessageBoxDefaultButton.Button1)
        {
            return MessageBox.Show(this, text, caption, buttons, icon, def,
                MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void ShowWarning(string msg, string title = "تنبيه", MessageBoxIcon icon = MessageBoxIcon.Warning)
        {
            Msg(msg, title, MessageBoxButtons.OK, icon);
        }

        private void ShowSuccess(string msg, string title = "تم")
        {
            Msg(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowError(string msg)
        {
            Msg(msg, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowError(string title, Exception ex)
        {
            Msg(title + ":\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ======================= تجهيز الواجهة =======================
        private static void EnableDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, dgv, new object[] { true });
        }

        private void SetupInputs()
        {
            txtCustomerName.MaxLength = 150;
            txtPhone.MaxLength = 50;
            txtRegion.MaxLength = 150;
            txtNotes.MaxLength = 500;

            AttachNumericFilter(txtBalance, true);
            AttachNumericFilter(txtPaymentAmount, false);

            txtBalance.Enter += (s, e) => { string t = txtBalance.Text.Trim(); if (t == "0" || t == "0.00") txtBalance.Text = ""; };
            txtBalance.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txtBalance.Text)) txtBalance.Text = "0"; };
            txtPaymentAmount.Enter += (s, e) => { string t = txtPaymentAmount.Text.Trim(); if (t == "0" || t == "0.00") txtPaymentAmount.Text = ""; };
            txtPaymentAmount.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txtPaymentAmount.Text)) txtPaymentAmount.Text = "0"; };
        }

        private static void AttachNumericFilter(TextBox tb, bool allowNegative)
        {
            tb.KeyPress += (s, e) =>
            {
                char c = e.KeyChar;
                if (char.IsControl(c) || char.IsDigit(c)) return;
                if (c == '.' && tb.Text.IndexOf('.') < 0) return;
                if (allowNegative && c == '-' && tb.SelectionStart == 0 && tb.Text.IndexOf('-') < 0) return;
                e.Handled = true;
            };
        }

        private void StyleGrid(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.BackgroundColor = DzPanel;
            grid.GridColor = DzGridLine;
            grid.DefaultCellStyle.BackColor = DzPanel;
            grid.DefaultCellStyle.ForeColor = Color.WhiteSmoke;
            grid.DefaultCellStyle.SelectionBackColor = DzSelect;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.DefaultCellStyle.Font = GridFont;
            grid.AlternatingRowsDefaultCellStyle.BackColor = DzGridAlt;
            grid.RowTemplate.Height = 42;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 46;
            grid.ColumnHeadersDefaultCellStyle.BackColor = DzBar;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = DzBar;
            grid.ColumnHeadersDefaultCellStyle.Font = GridHeaderFont;
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void SetupGrids()
        {
            StyleGrid(dgvCustomers);
            StyleGrid(dgvLedger);

            colBalance.DefaultCellStyle.Format = "N2";
            colTransDate.DefaultCellStyle.Format = "yyyy/MM/dd";
            colDebit.DefaultCellStyle.Format = "N2";
            colCredit.DefaultCellStyle.Format = "N2";
            colRunBalance.DefaultCellStyle.Format = "N2";
        }

        private void dgvCustomers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != colBalance.Index || !(e.Value is decimal)) return;
            decimal v = (decimal)e.Value;
            if (v > 0) e.CellStyle.ForeColor = ClrDebt;
            else if (v < 0) e.CellStyle.ForeColor = ClrCredit;
        }

        private void dgvLedger_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || !(e.Value is decimal)) return;
            decimal v = (decimal)e.Value;
            int c = e.ColumnIndex;

            if (c == colDebit.Index && v > 0) e.CellStyle.ForeColor = ClrDebt;
            else if (c == colCredit.Index && v > 0) e.CellStyle.ForeColor = ClrCredit;
            else if (c == colRunBalance.Index)
            {
                if (v > 0) e.CellStyle.ForeColor = ClrDebt;
                else if (v < 0) e.CellStyle.ForeColor = ClrCredit;
            }
        }

        // ======================= تحميل البيانات والفلترة =======================
        private bool TryGetBranchId(out int branchId)
        {
            branchId = 0;
            return cmbLocations.SelectedValue != null &&
                   int.TryParse(Convert.ToString(cmbLocations.SelectedValue), out branchId);
        }

        private void LoadLocations()
        {
            DataTable dt = _customerService.GetAllLocations();
            cmbLocations.DisplayMember = "Name";
            cmbLocations.ValueMember = "Id";
            cmbLocations.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                cmbLocations.SelectedIndex = 0;
                LoadCustomers();
            }
        }

        // SelectionChangeCommitted بيتنفذ من المستخدم بس (مفيش تحميل مزدوج وقت التشغيل)
        private void cmbLocations_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void LoadCustomers(int reselectId = 0)
        {
            int branchId;
            if (!TryGetBranchId(out branchId)) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                _customersList = _customerService.GetAllCustomers(branchId);
                PopulateRegionFilter();
                ApplyFilters();
                ClearFields();
                if (reselectId > 0) TrySelectRow(reselectId);
            }
            catch (Exception ex)
            {
                ShowError("تعذر تحميل العملاء", ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private IEnumerable<Customer> SortCustomers(IEnumerable<Customer> q)
        {
            if (_sortCol < 0) return q;

            Func<Customer, object> key;
            if (_sortCol == colId.Index) key = c => c.Id;
            else if (_sortCol == colName.Index) key = c => c.Name ?? "";
            else if (_sortCol == colPhone.Index) key = c => c.Phone ?? "";
            else if (_sortCol == colRegion.Index) key = c => c.Region ?? "";
            else if (_sortCol == colNotes.Index) key = c => c.Notes ?? "";
            else if (_sortCol == colBalance.Index) key = c => c.Balance;
            else return q;

            return _sortAsc ? q.OrderBy(key) : q.OrderByDescending(key);
        }

        private void ApplyFilters()
        {
            string search = txtSearch.Text.Trim();
            string region = cmbRegionFilter.Text.Trim();
            bool byRegion = region.Length > 0 && region != AllRegionsText;

            IEnumerable<Customer> q = _customersList;

            if (search.Length > 0)
            {
                q = q.Where(c =>
                    (!string.IsNullOrEmpty(c.Name) && c.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (!string.IsNullOrEmpty(c.Phone) && c.Phone.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0));
            }

            if (byRegion)
            {
                q = q.Where(c => !string.IsNullOrEmpty(c.Region) &&
                                 c.Region.IndexOf(region, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            _viewList = SortCustomers(q).ToList();

            int keepId = _selectedCustomerId;
            _suppressSelection = true;
            try
            {
                dgvCustomers.DataSource = _viewList;
                dgvCustomers.ClearSelection();
                if (keepId > 0) TrySelectRow(keepId);   // يفضل العميل المحدد محدد لو لسه ظاهر
            }
            finally
            {
                _suppressSelection = false;
            }

            lblCustomersList.Text = "قائمة العملاء (" + _viewList.Count + ")   |   صافي الأرصدة المعروضة: "
                                    + _viewList.Sum(c => c.Balance).ToString("N2") + " ج.م";
        }

        private void PopulateRegionFilter()
        {
            string current = cmbRegionFilter.Text;
            object[] regions = _customersList
                .Where(c => !string.IsNullOrWhiteSpace(c.Region))
                .Select(c => c.Region.Trim())
                .Distinct()
                .OrderBy(r => r)
                .Cast<object>()
                .ToArray();

            _suppressFilter = true;
            try
            {
                cmbRegionFilter.BeginUpdate();
                cmbRegionFilter.Items.Clear();
                cmbRegionFilter.Items.Add(AllRegionsText);
                cmbRegionFilter.Items.AddRange(regions);
                cmbRegionFilter.EndUpdate();

                if (!string.IsNullOrWhiteSpace(current) && cmbRegionFilter.Items.Contains(current))
                    cmbRegionFilter.Text = current;
                else
                    cmbRegionFilter.SelectedIndex = 0;
            }
            finally
            {
                _suppressFilter = false;
            }
        }

        private void RestartFilterTimer()
        {
            if (_suppressFilter) return;
            _filterTimer.Stop();
            _filterTimer.Start();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) { RestartFilterTimer(); }
        private void cmbRegionFilter_TextChanged(object sender, EventArgs e) { RestartFilterTimer(); }

        private void dgvCustomers_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (_sortCol == e.ColumnIndex) _sortAsc = !_sortAsc;
            else { _sortCol = e.ColumnIndex; _sortAsc = true; }

            foreach (DataGridViewColumn col in dgvCustomers.Columns)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            dgvCustomers.Columns[_sortCol].HeaderCell.SortGlyphDirection = _sortAsc ? SortOrder.Ascending : SortOrder.Descending;

            ApplyFilters();
        }

        // ======================= اختيار العميل والكشف =======================
        private bool TrySelectRow(int id)
        {
            foreach (DataGridViewRow row in dgvCustomers.Rows)
            {
                Customer c = row.DataBoundItem as Customer;
                if (c != null && c.Id == id)
                {
                    row.Selected = true;
                    dgvCustomers.CurrentCell = row.Cells[colName.Index];   // بيعمل scroll للصف كمان
                    return true;
                }
            }
            return false;
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (_suppressSelection || _customerService == null) return;
            if (dgvCustomers.SelectedRows.Count == 0) return;

            Customer c = dgvCustomers.SelectedRows[0].DataBoundItem as Customer;
            if (c != null) ShowCustomer(c);
        }

        private void ShowCustomer(Customer c)
        {
            _selectedCustomerId = c.Id;
            txtCustomerName.Text = c.Name;
            txtPhone.Text = c.Phone;
            txtRegion.Text = c.Region;
            txtNotes.Text = c.Notes;

            try
            {
                _currentCustomerInitialBalance = _customerService.GetCustomerInitialBalance(c.Id);
                txtBalance.Text = _currentCustomerInitialBalance.ToString("0.00", CultureInfo.InvariantCulture);
                LoadLedger(c.Id);
            }
            catch (Exception ex)
            {
                ShowError("تعذر تحميل كشف الحساب", ex);
            }
        }

        private static DataTable BuildLedgerTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RefId", typeof(int));
            dt.Columns.Add("TransType", typeof(string));
            dt.Columns.Add("TransDate", typeof(DateTime));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Debit", typeof(decimal));
            dt.Columns.Add("Credit", typeof(decimal));
            dt.Columns.Add("Balance", typeof(decimal));
            return dt;
        }

        private void LoadLedger(int customerId)
        {
            DataTable dt = BuildLedgerTable();

            if (customerId > 0)
            {
                DataTable src = _customerService.GetCustomerLedger(customerId);
                decimal running = _currentCustomerInitialBalance;

                dt.BeginLoadData();
                if (running != 0)
                {
                    dt.Rows.Add(DBNull.Value, "Init", DBNull.Value, "الرصيد الافتتاحي",
                        running > 0 ? (object)running : DBNull.Value,
                        running < 0 ? (object)Math.Abs(running) : DBNull.Value,
                        running);
                }

                foreach (DataRow r in src.Rows)
                {
                    decimal debit = Convert.ToDecimal(r["Debit"]);
                    decimal credit = Convert.ToDecimal(r["Credit"]);
                    running += debit - credit;

                    dt.Rows.Add(r["RefId"], r["TransType"], r["TransDate"], r["Description"],
                        debit > 0 ? (object)debit : DBNull.Value,
                        credit > 0 ? (object)credit : DBNull.Value,
                        running);
                }
                dt.EndLoadData();
            }

            dgvLedger.DataSource = dt;

            if (dgvLedger.RowCount > 0)
            {
                // يعرض آخر حركة (الأحدث) تحت
                dgvLedger.CurrentCell = dgvLedger.Rows[dgvLedger.RowCount - 1].Cells[colDesc.Index];
            }
            dgvLedger.ClearSelection();
        }

        private void dgvLedger_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvLedger.Rows[e.RowIndex];
            if (Convert.ToString(row.Cells[colTransType.Index].Value) != "Invoice") return;

            int refId;
            if (!int.TryParse(Convert.ToString(row.Cells[colRefId.Index].Value), out refId)) return;

            int keepId = _selectedCustomerId;
            using (frmSalesInvoice frm = new frmSalesInvoice(refId, true))
            {
                frm.ShowDialog(this);
            }
            LoadCustomers(keepId);
        }

        // ======================= إضافة / تعديل / حذف =======================
        private bool ValidateInput(out decimal balance)
        {
            balance = 0;

            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                ShowWarning("من فضلك أدخل اسم العميل.");
                txtCustomerName.Focus();
                return false;
            }
            if (cmbLocations.SelectedValue == null)
            {
                ShowWarning("برجاء اختيار الفرع أولاً.");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtBalance.Text) && !CustomerService.TryParseAmount(txtBalance.Text, out balance))
            {
                ShowWarning("الرصيد الافتتاحي غير صحيح، اكتب رقم فقط.");
                txtBalance.Focus();
                return false;
            }
            balance = Math.Round(balance, 2);
            return true;
        }

        private Customer GetCustomerFromUI(int id, decimal balance)
        {
            return new Customer
            {
                Id = id,
                Name = txtCustomerName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Region = txtRegion.Text.Trim(),
                Notes = txtNotes.Text.Trim(),
                Balance = balance
            };
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            decimal balance;
            if (!ValidateInput(out balance)) return;

            int branchId;
            if (!TryGetBranchId(out branchId)) return;

            string name = txtCustomerName.Text.Trim();
            if (_customersList.Any(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase)))
            {
                if (Msg("يوجد عميل بنفس الاسم \"" + name + "\" في هذا الفرع.\nهل تريد إضافة عميل جديد بنفس الاسم؟",
                        "اسم مكرر", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return;
            }

            try
            {
                btnSave.Enabled = false;
                _customerService.AddCustomer(GetCustomerFromUI(0, balance), branchId);
                ShowSuccess("تم حفظ بيانات العميل بنجاح.", "تم الحفظ");
                LoadCustomers();
            }
            catch (Exception ex)
            {
                ShowError("تعذر حفظ العميل", ex);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId <= 0)
            {
                ShowWarning("من فضلك اختر عميلًا من القائمة أولًا.");
                return;
            }
            if (_selectedCustomerId == SYSTEM_CASH_CUSTOMER_ID)
            {
                ShowWarning("لا يمكن تعديل بيانات العميل النقدي الأساسي للسيستم.", "محمي", MessageBoxIcon.Stop);
                return;
            }

            decimal balance;
            if (!ValidateInput(out balance)) return;

            if (balance != Math.Round(_currentCustomerInitialBalance, 2))
            {
                string msg = "هتغيّر الرصيد الافتتاحي من " + _currentCustomerInitialBalance.ToString("N2") + " إلى " + balance.ToString("N2") +
                             ".\nمديونية العميل الحالية هتتغير بنفس الفرق (" + (balance - _currentCustomerInitialBalance).ToString("N2") + ").\n\nمتأكد؟";
                if (Msg(msg, "تعديل الرصيد الافتتاحي", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return;
            }

            try
            {
                int keepId = _selectedCustomerId;
                _customerService.UpdateCustomer(GetCustomerFromUI(keepId, balance));
                ShowSuccess("تم تعديل بيانات العميل بنجاح.", "تم التعديل");
                LoadCustomers(keepId);
            }
            catch (Exception ex)
            {
                ShowError("تعذر تعديل العميل", ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId <= 0)
            {
                ShowWarning("من فضلك اختر عميلًا من القائمة أولًا.");
                return;
            }
            if (_selectedCustomerId == SYSTEM_CASH_CUSTOMER_ID)
            {
                ShowWarning("مستحيل حذف العميل النقدي لأنه يمثل كافة المبيعات النقدية للسيستم.", "محمي", MessageBoxIcon.Stop);
                return;
            }

            Customer c = _customersList.FirstOrDefault(x => x.Id == _selectedCustomerId);
            if (c != null && c.Balance != 0)
            {
                ShowWarning("العميل عليه رصيد " + c.Balance.ToString("N2") + " ج.م.\nسدّد الرصيد أو صفّره الأول، لأن حذفه بيشيل المديونية من التقارير والخزينة.",
                    "لا يمكن الحذف", MessageBoxIcon.Stop);
                return;
            }

            if (Msg("هل أنت متأكد من حذف هذا العميل؟ لن يظهر مرة أخرى.", "تأكيد الحذف",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            try
            {
                _customerService.DeleteCustomer(_selectedCustomerId);
                ShowSuccess("تم حذف العميل بنجاح.", "نجاح");
                LoadCustomers();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        // ======================= سداد الديون =======================
        private void btnPayDebt_Click(object sender, EventArgs e)
        {
            if (_selectedCustomerId <= 0)
            {
                ShowWarning("من فضلك اختر عميلًا أولًا.");
                return;
            }
            if (_selectedCustomerId == SYSTEM_CASH_CUSTOMER_ID)
            {
                ShowWarning("العميل النقدي ليس عليه مديونية، لا يمكن تسجيل سداد له.", "محمي", MessageBoxIcon.Stop);
                return;
            }

            decimal amount;
            if (!CustomerService.TryParseAmount(txtPaymentAmount.Text, out amount) || amount <= 0)
            {
                ShowWarning("من فضلك أدخل مبلغًا صحيحًا أكبر من صفر.");
                txtPaymentAmount.Focus();
                return;
            }
            amount = Math.Round(amount, 2);

            int branchId;
            if (!TryGetBranchId(out branchId)) return;

            Customer c = _customersList.FirstOrDefault(x => x.Id == _selectedCustomerId);
            if (c == null)
            {
                ShowWarning("العميل مش ظاهر في القائمة الحالية، اضغط تحديث وحاول تاني.");
                return;
            }

            string method = Convert.ToString(cmbPayMethod.SelectedItem);
            if (string.IsNullOrEmpty(method)) method = CustomerService.PaymentMethods[0];

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("العميل: " + c.Name);
            sb.AppendLine("المبلغ: " + amount.ToString("N2") + " ج.م");
            sb.AppendLine("طريقة الدفع: " + method);
            sb.AppendLine();
            sb.AppendLine("المديونية الحالية: " + c.Balance.ToString("N2") + " ج.م");
            sb.AppendLine("المديونية بعد السداد: " + (c.Balance - amount).ToString("N2") + " ج.م");
            if (amount > c.Balance)
            {
                sb.AppendLine();
                sb.AppendLine(c.Balance <= 0
                    ? "⚠ العميل مفيش عليه مديونية، السداد هيتسجل كرصيد دائن (مقدّم) للعميل."
                    : "⚠ المبلغ أكبر من المديونية، الفرق هيتسجل كرصيد دائن للعميل.");
            }
            sb.AppendLine();
            sb.Append("تأكيد تسجيل السداد؟");

            if (Msg(sb.ToString(), "تأكيد السداد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            btnPayDebt.Enabled = false;   // يمنع الضغط المزدوج
            try
            {
                _customerService.PayCustomerDebt(c.Id, amount, branchId, method);
                txtPaymentAmount.Text = "0";
                LoadCustomers(c.Id);
                ShowSuccess("تم تسجيل السداد بنجاح.", "تم السداد");
            }
            catch (Exception ex)
            {
                ShowError("تعذر تسجيل السداد", ex);
            }
            finally
            {
                btnPayDebt.Enabled = true;
            }
        }

        // ======================= استيراد =======================
        private void btnImport_Click(object sender, EventArgs e)
        {
            int branchId;
            if (!TryGetBranchId(out branchId))
            {
                ShowWarning("برجاء اختيار الفرع المراد استيراد العملاء إليه.");
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "CSV Files (*.csv)|*.csv", Title = "اختر ملف العملاء" })
            {
                if (ofd.ShowDialog(this) != DialogResult.OK) return;

                if (Msg("هيتم استيراد العملاء على فرع: " + cmbLocations.Text + "\nالعملاء المكررين (نفس الاسم والهاتف) هيتم تخطيهم.\n\nمتابعة؟",
                        "تأكيد الاستيراد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                try
                {
                    Cursor = Cursors.WaitCursor;
                    int skipped;
                    int added = _customerService.ImportCustomersFromCsv(ofd.FileName, branchId, out skipped);
                    LoadCustomers();
                    ShowSuccess("تم استيراد " + added + " عميل.\nتم تخطي " + skipped + " (مكرر أو بدون اسم).", "تم الاستيراد");
                }
                catch (Exception ex)
                {
                    ShowError("تعذر الاستيراد", ex);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        // ======================= أدوات =======================
        private void btnClear_Click(object sender, EventArgs e) { ClearFields(); }
        private void btnRefresh_Click(object sender, EventArgs e) { LoadCustomers(_selectedCustomerId); }

        private void ClearFields()
        {
            txtCustomerName.Clear();
            txtPhone.Clear();
            txtRegion.Clear();
            txtNotes.Clear();
            txtBalance.Text = "0";
            txtPaymentAmount.Text = "0";
            cmbPayMethod.SelectedIndex = 0;
            _selectedCustomerId = 0;
            _currentCustomerInitialBalance = 0;

            _suppressSelection = true;
            try
            {
                dgvCustomers.ClearSelection();
                dgvLedger.DataSource = null;
            }
            finally
            {
                _suppressSelection = false;
            }
            txtCustomerName.Focus();
        }

        // ======================= طباعة / تصدير / مشاركة =======================
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (_viewList.Count == 0) { ShowWarning("لا توجد بيانات لطباعتها."); return; }

            try
            {
                using (PrintDocument doc = new PrintDocument())
                {
                    // BeginPrint بيتنفذ مع كل re-render للمعاينة (zoom/refresh) فلازم نصفّر هنا
                    doc.BeginPrint += (s, ev) => { _printRowIndex = 0; _printPage = 0; };
                    doc.PrintPage += PrintCustomersPage;

                    using (PrintPreviewDialog preview = new PrintPreviewDialog
                    {
                        Document = doc,
                        Width = 1000,
                        Height = 750,
                        StartPosition = FormStartPosition.CenterScreen
                    })
                    {
                        preview.ShowDialog(this);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("خطأ في الطباعة", ex);
            }
        }

        private void PrintCustomersPage(object sender, PrintPageEventArgs e)
        {
            _printPage++;

            using (Font headerFont = new Font("Segoe UI", 16F, FontStyle.Bold))
            using (Font subFont = new Font("Segoe UI", 10F))
            using (Font cellFont = new Font("Segoe UI", 11F))
            using (Font boldCell = new Font("Segoe UI", 11F, FontStyle.Bold))
            using (StringFormat sfRight = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft, Trimming = StringTrimming.EllipsisCharacter })
            using (StringFormat sfCenter = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.DirectionRightToLeft, Trimming = StringTrimming.EllipsisCharacter })
            {
                Graphics g = e.Graphics;
                Rectangle m = e.MarginBounds;
                int y = m.Top;

                if (_printRowIndex == 0)
                {
                    g.DrawString("كشف بيانات العملاء - فرع: " + cmbLocations.Text, headerFont, Brushes.Black, new RectangleF(m.Left, y, m.Width, 30), sfCenter);
                    y += 35;
                    g.DrawString("تاريخ الطباعة: " + DateTime.Now.ToString("yyyy/MM/dd HH:mm"), subFont, Brushes.Gray, new RectangleF(m.Left, y, m.Width, 20), sfCenter);
                    y += 40;
                }

                float[] colWidths = { m.Width * 0.08f, m.Width * 0.35f, m.Width * 0.20f, m.Width * 0.22f, m.Width * 0.15f };
                string[] headers = { "م", "اسم العميل", "الهاتف", "المنطقة", "المديونية" };

                float x = m.Right;
                for (int i = 0; i < headers.Length; i++)
                {
                    RectangleF rect = new RectangleF(x - colWidths[i], y, colWidths[i], 30);
                    g.FillRectangle(Brushes.LightGray, rect);
                    g.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
                    g.DrawString(headers[i], boldCell, Brushes.Black, rect, sfCenter);
                    x -= colWidths[i];
                }
                y += 30;

                while (_printRowIndex < _viewList.Count)
                {
                    if (y > m.Bottom - 70)   // نسيب مكان للإجمالي ورقم الصفحة
                    {
                        g.DrawString("صفحة " + _printPage, subFont, Brushes.Gray, new RectangleF(m.Left, m.Bottom - 20, m.Width, 20), sfCenter);
                        e.HasMorePages = true;
                        return;
                    }

                    Customer c = _viewList[_printRowIndex];
                    string[] values = { c.Id.ToString(), c.Name ?? "", c.Phone ?? "", c.Region ?? "", c.Balance.ToString("N2") };

                    x = m.Right;
                    for (int i = 0; i < values.Length; i++)
                    {
                        RectangleF rect = new RectangleF(x - colWidths[i], y, colWidths[i], 30);
                        g.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);
                        g.DrawString(values[i], cellFont, Brushes.Black, rect, i == 1 ? sfRight : sfCenter);
                        x -= colWidths[i];
                    }
                    y += 30;
                    _printRowIndex++;
                }

                // الإجمالي في آخر صفحة
                y += 8;
                RectangleF totalRect = new RectangleF(m.Left, y, m.Width, 30);
                g.DrawString("صافي الأرصدة: " + _viewList.Sum(c => c.Balance).ToString("N2") + " ج.م", boldCell, Brushes.Black, totalRect, sfCenter);
                g.DrawString("صفحة " + _printPage, subFont, Brushes.Gray, new RectangleF(m.Left, m.Bottom - 20, m.Width, 20), sfCenter);
                e.HasMorePages = false;
            }
        }

        private static string Csv(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return (s.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0) ? "\"" + s.Replace("\"", "\"\"") + "\"" : s;
        }

        // العناوين دي هي نفس اللي الاستيراد بيتعرف عليها
        private void ExportToCsv(string path)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("الاسم,الهاتف,المنطقة,الملحوظة,المديونية");
            foreach (Customer c in _viewList)
            {
                sb.AppendLine(string.Join(",", Csv(c.Name), Csv(c.Phone), Csv(c.Region), Csv(c.Notes),
                    c.Balance.ToString("0.00", CultureInfo.InvariantCulture)));
            }
            File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_viewList.Count == 0) { ShowWarning("لا توجد بيانات لتصديرها."); return; }

            using (SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "ملف Excel (CSV)|*.csv",
                FileName = "كشف_العملاء_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv"
            })
            {
                if (sfd.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    ExportToCsv(sfd.FileName);
                    ShowSuccess("تم تصدير الملف بنجاح.\nملحوظة: لتصدير الكشف كـ PDF استخدم زر الطباعة ثم اختر طابعة (Microsoft Print to PDF).", "تم التصدير");
                }
                catch (Exception ex)
                {
                    ShowError("تعذر التصدير", ex);
                }
            }
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            if (_viewList.Count == 0) { ShowWarning("لا توجد بيانات للمشاركة."); return; }

            try
            {
                string tempPath = Path.Combine(Path.GetTempPath(), "كشف_العملاء_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv");
                ExportToCsv(tempPath);
                Process.Start("explorer.exe", "/select,\"" + tempPath + "\"");
                ShowSuccess("تم إنشاء الملف وفتح مكانه في مستكشف الملفات.\n\nلمشاركته عبر البلوتوث أو واتساب:\nاضغط بزر الفأرة الأيمن على الملف واختر (إرسال إلى) ثم اختر وسيلة المشاركة المطلوبة.", "مشاركة الملف");
            }
            catch (Exception ex)
            {
                ShowError("تعذرت المشاركة", ex);
            }
        }
    }
}