using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using MajesticDesktop.Models;
using MajesticDesktop.Services;

namespace MajesticDesktop.Forms
{
    public partial class frmSuppliers : Form
    {
        private readonly SupplierService _supplierService;
        private List<Supplier> _suppliersList = new List<Supplier>();

        private const int SYSTEM_CASH_SUPPLIER_ID = 1;

        private int _selectedSupplierId = 0;
        private decimal _currentSupplierInitialBalance = 0;

        public frmSuppliers()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            _supplierService = new SupplierService();
            SetupTextBoxPlaceholders();
            EnableDoubleBuffering(dgvSuppliers);
            EnableDoubleBuffering(dgvLedger);
        }

        private void frmSuppliers_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            try
            {
                StyleDataGridViews();
                SetupGridDataBinding();
                LoadLocations();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        #region UI & Grid Setup

        private void EnableDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, dgv, new object[] { true });
        }

        private void SetupTextBoxPlaceholders()
        {
            txtBalance.Enter += (s, e) => { if (txtBalance.Text.Trim() == "0" || txtBalance.Text.Trim() == "0.00") txtBalance.Text = ""; };
            txtBalance.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txtBalance.Text)) txtBalance.Text = "0"; };

            txtPaymentAmount.Enter += (s, e) => { if (txtPaymentAmount.Text.Trim() == "0" || txtPaymentAmount.Text.Trim() == "0.00") txtPaymentAmount.Text = ""; };
            txtPaymentAmount.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txtPaymentAmount.Text)) txtPaymentAmount.Text = "0"; };
        }

        private void SetupGridDataBinding()
        {
            dgvSuppliers.AutoGenerateColumns = false;
            dgvLedger.AutoGenerateColumns = false;

            colRowNumber.DataPropertyName = "RowNumber";
            colId.DataPropertyName = "Id";
            colName.DataPropertyName = "Name";
            colPhone.DataPropertyName = "Phone";
            colRegion.DataPropertyName = "Region";
            colNotes.DataPropertyName = "Notes";
            colBalance.DataPropertyName = "Balance";

            dgvSuppliers.CellFormatting += DgvSuppliers_CellFormatting;
        }

        private void DgvSuppliers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvSuppliers.Columns[e.ColumnIndex].Name == "colBalance" && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal balance))
                {
                    e.Value = balance.ToString("0.00");
                    e.FormattingApplied = true;

                    if (balance > 0) e.CellStyle.ForeColor = Color.FromArgb(248, 113, 113);
                    else if (balance < 0) e.CellStyle.ForeColor = Color.FromArgb(74, 222, 128);
                    else e.CellStyle.ForeColor = Color.WhiteSmoke;
                }
            }
        }

        private void StyleDataGridViews()
        {
            Action<DataGridView> style = (grid) =>
            {
                grid.AllowUserToAddRows = false;
                grid.AllowUserToDeleteRows = false;
                grid.AllowUserToResizeRows = false;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grid.BorderStyle = BorderStyle.None;
                grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                grid.BackgroundColor = Color.FromArgb(30, 41, 59);
                grid.GridColor = Color.FromArgb(51, 65, 85);
                grid.MultiSelect = false;
                grid.ReadOnly = true;
                grid.RowHeadersVisible = false;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                grid.ColumnHeadersHeight = 45;
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
                grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid.DefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
                grid.DefaultCellStyle.ForeColor = Color.WhiteSmoke;
                grid.DefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
                grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
                grid.DefaultCellStyle.SelectionForeColor = Color.White;
                grid.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 50, 71);
                grid.RowTemplate.Height = 42;
            };

            style(dgvSuppliers);
            style(dgvLedger);
        }

        #endregion

        #region Data Loading & Filtering

        private void LoadLocations()
        {
            DataTable dtLocations = _supplierService.GetAllLocations();
            cmbLocations.DisplayMember = "Name";
            cmbLocations.ValueMember = "Id";
            cmbLocations.DataSource = dtLocations;

            if (dtLocations.Rows.Count > 0) cmbLocations.SelectedIndex = 0;
        }

        private void cmbLocations_SelectedIndexChanged(object sender, EventArgs e) => LoadSuppliers();

        private void LoadSuppliers()
        {
            if (cmbLocations.SelectedValue == null || !int.TryParse(cmbLocations.SelectedValue.ToString(), out int branchId)) return;

            try
            {
                _suppliersList = _supplierService.GetAllSuppliers(branchId);
                PopulateRegionFilter();
                ApplyFilters();
                ClearFields();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void ApplyFilters()
        {
            string search = txtSearch.Text.Trim().ToLower();
            string regionFilter = cmbRegionFilter.Text.Trim();
            bool filterByRegion = !string.IsNullOrWhiteSpace(regionFilter) && regionFilter != "كل المناطق";

            var filtered = _suppliersList.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                filtered = filtered.Where(c =>
                    (!string.IsNullOrEmpty(c.Name) && c.Name.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Any(word => word.StartsWith(search))) ||
                    (!string.IsNullOrEmpty(c.Phone) && c.Phone.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Any(word => word.StartsWith(search))));
            }

            if (filterByRegion)
            {
                filtered = filtered.Where(c => !string.IsNullOrEmpty(c.Region) && c.Region.IndexOf(regionFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            var displayData = filtered.Select((c, index) => new
            {
                RowNumber = index + 1,
                Id = c.Id,
                Name = c.Name,
                Phone = c.Phone,
                Region = c.Region,
                Notes = c.Notes,
                Balance = c.Balance
            }).ToList();

            dgvSuppliers.DataSource = displayData;
            dgvSuppliers.ClearSelection();
        }

        private void PopulateRegionFilter()
        {
            string currentText = cmbRegionFilter.Text;
            var regions = _suppliersList.Where(c => !string.IsNullOrWhiteSpace(c.Region)).Select(c => c.Region.Trim()).Distinct().OrderBy(r => r).ToList();

            cmbRegionFilter.Items.Clear();
            cmbRegionFilter.Items.Add("كل المناطق");
            regions.ForEach(r => cmbRegionFilter.Items.Add(r));

            if (!string.IsNullOrWhiteSpace(currentText) && cmbRegionFilter.Items.Contains(currentText))
                cmbRegionFilter.Text = currentText;
            else
                cmbRegionFilter.SelectedIndex = 0;
        }

        private void LoadLedger(int supplierId)
        {
            dgvLedger.Rows.Clear();
            if (supplierId <= 0) return;

            DataTable dt = _supplierService.GetSupplierLedger(supplierId);
            decimal runningBalance = _currentSupplierInitialBalance;

            if (_currentSupplierInitialBalance != 0)
            {
                int initRow = dgvLedger.Rows.Add();
                dgvLedger.Rows[initRow].Cells["colTransType"].Value = "Init";
                dgvLedger.Rows[initRow].Cells["colTransDate"].Value = "رصيد سابق";
                dgvLedger.Rows[initRow].Cells["colDesc"].Value = "الرصيد الافتتاحي للمورد";
                dgvLedger.Rows[initRow].Cells["colCredit"].Value = _currentSupplierInitialBalance > 0 ? _currentSupplierInitialBalance.ToString("0.00") : "";
                dgvLedger.Rows[initRow].Cells["colDebit"].Value = _currentSupplierInitialBalance < 0 ? Math.Abs(_currentSupplierInitialBalance).ToString("0.00") : "";
                dgvLedger.Rows[initRow].Cells["colRunBalance"].Value = runningBalance.ToString("0.00");
            }

            foreach (DataRow row in dt.Rows)
            {
                decimal debit = Convert.ToDecimal(row["Debit"]);
                decimal credit = Convert.ToDecimal(row["Credit"]);
                runningBalance += (credit - debit);

                dgvLedger.Rows.Add(
                    row["RefId"], row["TransType"], Convert.ToDateTime(row["TransDate"]).ToShortDateString(),
                    row["Description"], debit > 0 ? debit.ToString("0.00") : "", credit > 0 ? credit.ToString("0.00") : "", runningBalance.ToString("0.00")
                );
            }
        }

        #endregion

        #region CRUD Operations

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            decimal.TryParse(txtBalance.Text, out decimal balance);
            var supplier = GetSupplierFromUI(0, balance);

            try
            {
                int branchId = Convert.ToInt32(cmbLocations.SelectedValue);
                _supplierService.AddSupplier(supplier, branchId);
                ShowSuccess("تم حفظ بيانات المورد بنجاح.", "تم الحفظ");
                LoadSuppliers();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (_selectedSupplierId <= 0) { ShowWarning("من فضلك اختر مورداً من القائمة أولاً."); return; }
            if (_selectedSupplierId == SYSTEM_CASH_SUPPLIER_ID) { ShowWarning("لا يمكن تعديل المورد النقدي الأساسي.", "محمي", MessageBoxIcon.Stop); return; }
            if (!ValidateInput()) return;

            decimal.TryParse(txtBalance.Text, out decimal balance);
            var supplier = GetSupplierFromUI(_selectedSupplierId, balance);

            try
            {
                _supplierService.UpdateSupplier(supplier);
                ShowSuccess("تم تعديل بيانات المورد بنجاح.", "تم التعديل");
                LoadSuppliers();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedSupplierId <= 0) { ShowWarning("من فضلك اختر مورداً من القائمة أولاً."); return; }
            if (_selectedSupplierId == SYSTEM_CASH_SUPPLIER_ID) { ShowWarning("مستحيل حذف المورد النقدي.", "محمي", MessageBoxIcon.Stop); return; }

            if (MessageBox.Show("هل أنت متأكد من حذف هذا المورد؟ لن يظهر مرة أخرى.", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    _supplierService.DeleteSupplier(_selectedSupplierId);
                    ShowSuccess("تم حذف المورد بنجاح.", "نجاح");
                    LoadSuppliers();
                }
                catch (Exception ex) { ShowError(ex.Message); }
            }
        }

        private void btnPayDebt_Click(object sender, EventArgs e)
        {
            if (_selectedSupplierId <= 0) { ShowWarning("من فضلك اختر مورداً أولاً."); return; }
            if (!decimal.TryParse(txtPaymentAmount.Text, out decimal amount) || amount <= 0) { ShowWarning("أدخل مبلغًا صحيحًا أكبر من صفر."); return; }
            if (cmbLocations.SelectedValue == null) return;

            // بنحدد وسيلة الدفع اللي اختارها من الليستة الجديدة، عشان الفلوس
            // تتخصم من نفس الوسيلة دي (مش من الكاش النقدي دايمًا زي ما كان بيحصل قبل كده)
            string paymentMethod = cmbPaymentMethod.SelectedItem?.ToString() ?? "نقدي";

            try
            {
                int branchId = Convert.ToInt32(cmbLocations.SelectedValue);
                _supplierService.PaySupplierDebt(_selectedSupplierId, amount, branchId, paymentMethod);
                txtPaymentAmount.Text = "0";
                ReselectSupplierAfterAction();
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }

        #endregion

        #region Export, Share & Print

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.Rows.Count == 0) { ShowWarning("لا توجد بيانات لطباعتها."); return; }
            try
            {
                using (var printer = new DataGridViewPrinter(dgvSuppliers))
                {
                    printer.PrintPreview();
                }
            }
            catch (Exception ex) { ShowError($"خطأ في الطباعة: {ex.Message}"); }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.Rows.Count == 0) { ShowWarning("لا توجد بيانات لتصديرها."); return; }

            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "ملف Excel (CSV)|*.csv", FileName = $"كشف_الموردين_{DateTime.Now:yyyyMMdd_HHmm}.csv" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportGridToCsv(sfd.FileName);
                        ShowSuccess("تم تصدير الملف بنجاح.\nملحوظة: لتصدير الكشف كـ PDF استخدم زر الطباعة ثم اختر (Microsoft Print to PDF).", "تم التصدير");
                    }
                    catch (Exception ex) { ShowError(ex.Message); }
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (cmbLocations.SelectedValue == null) { ShowWarning("برجاء اختيار الفرع المراد استيراد الموردين إليه."); return; }

            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "CSV Files (*.csv)|*.csv", Title = "اختر ملف الموردين" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        int branchId = Convert.ToInt32(cmbLocations.SelectedValue);
                        _supplierService.ImportSuppliersFromCsv(ofd.FileName, branchId);
                        ShowSuccess("تم استيراد الموردين بنجاح.", "تم الاستيراد");
                        LoadSuppliers();
                    }
                    catch (Exception ex) { ShowError(ex.Message); }
                }
            }
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.Rows.Count == 0) { ShowWarning("لا توجد بيانات للمشاركة."); return; }

            try
            {
                string tempPath = Path.Combine(Path.GetTempPath(), $"كشف_الموردين_{DateTime.Now:yyyyMMdd_HHmm}.csv");
                ExportGridToCsv(tempPath);
                Process.Start("explorer.exe", $"/select,\"{tempPath}\"");
                ShowSuccess("تم التجهيز.\nشارك الملف (كليك يمين -> Send To) بلوتوث أو واتساب.", "مشاركة الملف");
            }
            catch (Exception ex) { ShowError(ex.Message); }
        }


        private void ExportGridToCsv(string path)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("م,الاسم,الهاتف,المنطقة,الملحوظة,المديونية");

            foreach (DataGridViewRow row in dgvSuppliers.Rows)
            {
                string line = string.Join(",",
                    EscapeCsv(row.Cells["colRowNumber"].Value),
                    EscapeCsv(row.Cells["colName"].Value),
                    EscapeCsv(row.Cells["colPhone"].Value),
                    EscapeCsv(row.Cells["colRegion"].Value),
                    EscapeCsv(row.Cells["colNotes"].Value),
                    EscapeCsv(row.Cells["colBalance"].Value));
                sb.AppendLine(line);
            }
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }
        private string EscapeCsv(object value)
        {
            string s = value?.ToString() ?? "";
            return (s.Contains(",") || s.Contains("\"") || s.Contains("\n")) ? $"\"{s.Replace("\"", "\"\"")}\"" : s;
        }

        #endregion

        #region UI Events
        private void btnClear_Click(object sender, EventArgs e) => ClearFields();
        private void btnRefresh_Click(object sender, EventArgs e) => LoadSuppliers();
        private void txtSearch_TextChanged(object sender, EventArgs e) => ApplyFilters();
        private void cmbRegionFilter_TextChanged(object sender, EventArgs e) => ApplyFilters();

        private void dgvSuppliers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvSuppliers.Rows.Count) return;
            DataGridViewRow row = dgvSuppliers.Rows[e.RowIndex];
            if (row.Cells["colId"].Value == null) return;

            _selectedSupplierId = Convert.ToInt32(row.Cells["colId"].Value);
            txtSupplierName.Text = row.Cells["colName"].Value?.ToString();
            txtPhone.Text = row.Cells["colPhone"].Value?.ToString();
            txtRegion.Text = row.Cells["colRegion"].Value?.ToString();
            txtNotes.Text = row.Cells["colNotes"].Value?.ToString();

            _currentSupplierInitialBalance = _supplierService.GetSupplierInitialBalance(_selectedSupplierId);
            txtBalance.Text = _currentSupplierInitialBalance.ToString("0.00");
            LoadLedger(_selectedSupplierId);
        }

        private void dgvLedger_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvLedger.Rows[e.RowIndex].Cells["colTransType"].Value?.ToString() == "Invoice")
            {
                if (int.TryParse(dgvLedger.Rows[e.RowIndex].Cells["colRefId"].Value?.ToString(), out int refId))
                    ReselectSupplierAfterAction();
            }
        }

        private void ClearFields()
        {
            txtSupplierName.Clear();
            txtPhone.Clear();
            txtRegion.Clear();
            txtNotes.Clear();
            txtBalance.Text = "0";
            txtPaymentAmount.Text = "0";
            cmbPaymentMethod.SelectedIndex = 0;
            _selectedSupplierId = 0;
            _currentSupplierInitialBalance = 0;
            dgvSuppliers.ClearSelection();
            dgvLedger.Rows.Clear();
            txtSupplierName.Focus();
        }

        private void ReselectSupplierAfterAction()
        {
            int tempId = _selectedSupplierId;
            LoadSuppliers();
            foreach (DataGridViewRow row in dgvSuppliers.Rows)
            {
                if (row.Cells["colId"].Value != null && Convert.ToInt32(row.Cells["colId"].Value) == tempId)
                {
                    row.Selected = true;
                    dgvSuppliers_CellClick(dgvSuppliers, new DataGridViewCellEventArgs(0, row.Index));
                    break;
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtSupplierName.Text)) { ShowWarning("من فضلك أدخل اسم المورد."); txtSupplierName.Focus(); return false; }
            if (cmbLocations.SelectedValue == null) { ShowWarning("برجاء اختيار الفرع أولاً."); return false; }
            return true;
        }

        private Supplier GetSupplierFromUI(int id, decimal balance)
        {
            return new Supplier { Id = id, Name = txtSupplierName.Text.Trim(), Phone = txtPhone.Text.Trim(), Region = txtRegion.Text.Trim(), Notes = txtNotes.Text.Trim(), Balance = balance };
        }

        private void ShowWarning(string msg, string title = "تنبيه", MessageBoxIcon icon = MessageBoxIcon.Warning) => MessageBox.Show(msg, title, MessageBoxButtons.OK, icon);
        private void ShowError(string msg) => MessageBox.Show(msg, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        private void ShowSuccess(string msg, string title = "تم") => MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        #endregion
    }
}