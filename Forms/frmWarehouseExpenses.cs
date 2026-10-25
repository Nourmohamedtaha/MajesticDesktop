using MajesticDesktop.Services;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    public partial class frmWarehouseExpenses : Form
    {
        private WarehouseExpenseService expenseService;
        private bool _busy;

        public frmWarehouseExpenses()
        {
            InitializeComponent();

            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            expenseService = new WarehouseExpenseService();

            // تفعيل DoubleBuffering للجدول لمنع الرعشة
            EnableDoubleBuffering(dgvExpenses);
        }

        private static void EnableDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null,
                dgv,
                new object[] { true });
        }

        private async void frmWarehouseExpenses_Load(object sender, EventArgs e)
        {
            if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                return;

            StyleDataGridView();
            LoadDropdowns();

            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;

            rbExpense.Checked = true; // الافتراضي هو مصروف
            ManageUIState();

            await LoadExpensesAsync();
        }

        private void LoadDropdowns()
        {
            cmbExpenseType.Items.Clear();
            cmbExpenseType.Items.AddRange(WarehouseExpenseService.ExpenseCategories);
            cmbExpenseType.SelectedIndex = 0;
            cmbExpenseType.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbPaymentSource.Items.Clear();
            cmbPaymentSource.Items.AddRange(WarehouseExpenseService.PaymentSources);
            cmbPaymentSource.SelectedIndex = 0;
            cmbPaymentSource.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void ManageUIState()
        {
            if (rbBorrow.Checked)
            {
                pnlBorrower.Visible = true;
                pnlExpenseCategory.Visible = false;
                txtBorrowerName.Focus();
            }
            else
            {
                pnlExpenseCategory.Visible = true;
                pnlBorrower.Visible = false;
                cmbExpenseType.Focus();
                txtBorrowerName.Clear();
            }
        }

        private void rbOperationType_CheckedChanged(object sender, EventArgs e)
        {
            ManageUIState();
        }

        // التعديل الثالث: تنسيق الجدول ليكون Pixel Perfect ومتوافقاً مع RTL
        // التعديل الثالث: تنسيق الجدول ليكون Pixel Perfect ومتوافقاً مع RTL
        private void StyleDataGridView()
        {
            dgvExpenses.BorderStyle = BorderStyle.None;
            dgvExpenses.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvExpenses.BackgroundColor = Color.FromArgb(15, 23, 42); // نفس لون الكرد
            dgvExpenses.GridColor = Color.FromArgb(51, 65, 85);

            dgvExpenses.DefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            dgvExpenses.DefaultCellStyle.ForeColor = Color.WhiteSmoke;
            dgvExpenses.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235); // تم التصحيح هنا ✔️
            dgvExpenses.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvExpenses.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvExpenses.DefaultCellStyle.Font = new Font("Segoe UI", 12.5F, FontStyle.Bold);

            dgvExpenses.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 50, 71);
            dgvExpenses.RowTemplate.Height = 50;

            dgvExpenses.EnableHeadersVisualStyles = false;
            dgvExpenses.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvExpenses.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 30, 45);
            dgvExpenses.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvExpenses.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            dgvExpenses.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvExpenses.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvExpenses.ColumnHeadersHeight = 55;
            dgvExpenses.RowHeadersVisible = false;

            dgvExpenses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExpenses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvExpenses.ReadOnly = true;
            dgvExpenses.AllowUserToAddRows = false;
            dgvExpenses.AllowUserToDeleteRows = false;
            dgvExpenses.AllowUserToResizeRows = false;
        }

        // التعديل الرابع: التحميل بـ Async وضبط أوزان ومحاذاة الأعمدة
        private async Task LoadExpensesAsync()
        {
            if (_busy) return;
            SetBusy(true);

            try
            {
                DataTable dt = await expenseService.GetWarehouseExpensesAsync(dtpFrom.Value, dtpTo.Value);

                dgvExpenses.DataSource = dt;

                if (dgvExpenses.Columns.Contains("Id"))
                    dgvExpenses.Columns["Id"].Visible = false;

                // ضبط مقاسات العواميد للحصول على شكل احترافي (البيان يكون الأوسع)
                if (dgvExpenses.Columns.Contains("البيان"))
                {
                    dgvExpenses.Columns["البيان"].FillWeight = 35F;
                    dgvExpenses.Columns["البيان"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dgvExpenses.Columns.Contains("التاريخ والوقت"))
                {
                    dgvExpenses.Columns["التاريخ والوقت"].FillWeight = 20F;
                }
                if (dgvExpenses.Columns.Contains("نوع العملية"))
                {
                    dgvExpenses.Columns["نوع العملية"].FillWeight = 15F;
                }
                if (dgvExpenses.Columns.Contains("اسم المستلف"))
                {
                    dgvExpenses.Columns["اسم المستلف"].FillWeight = 15F;
                }
                if (dgvExpenses.Columns.Contains("مصدر الدفع"))
                {
                    dgvExpenses.Columns["مصدر الدفع"].FillWeight = 15F;
                }
                if (dgvExpenses.Columns.Contains("المبلغ"))
                {
                    dgvExpenses.Columns["المبلغ"].FillWeight = 15F;
                    dgvExpenses.Columns["المبلغ"].DefaultCellStyle.Format = "N2";
                }

                dgvExpenses.ClearSelection();
                CalculateTotal(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ في جلب البيانات", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void SetBusy(bool busy)
        {
            _busy = busy;
            btnFilter.Enabled = !busy;
            btnSaveExpense.Enabled = !busy;
            UseWaitCursor = busy;
        }

        private async void btnFilter_Click(object sender, EventArgs e)
        {
            await LoadExpensesAsync();
        }

        private void CalculateTotal(DataTable dt)
        {
            decimal total = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (row["المبلغ"] != DBNull.Value)
                {
                    total += Convert.ToDecimal(row["المبلغ"]);
                }
            }
            lblTotal.Text = $"إجمالي العمليات للفترة المحددة: {total.ToString("N2", CultureInfo.InvariantCulture)} ج.م";
        }

        private decimal ParseSafeDecimal(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0;
            string sanitized = text.Replace(",", ".");
            decimal.TryParse(sanitized, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result);
            return result;
        }

        private async void btnSaveExpense_Click(object sender, EventArgs e)
        {
            decimal amount = ParseSafeDecimal(txtAmount.Text);

            if (amount <= 0)
            {
                MessageBox.Show("برجاء إدخال مبلغ صحيح أكبر من الصفر.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAmount.Focus();
                return;
            }

            SetBusy(true);

            try
            {
                string operationType = rbBorrow.Checked ? "سلفة" : "مصروف";
                string expenseCategory = cmbExpenseType.Text;
                string paymentSource = cmbPaymentSource.Text;
                string notes = txtNotes.Text;
                string borrower = txtBorrowerName.Text;

                await expenseService.AddWarehouseExpenseAsync(operationType, expenseCategory, amount, paymentSource, notes, borrower);

                txtAmount.Clear();
                txtNotes.Clear();
                txtBorrowerName.Clear();

                await LoadExpensesAsync();

                string successMsg = $"تم تسجيل {operationType} بنجاح وخصم المبلغ من ({paymentSource}).\nتم الربط بيومية اليوم تلقائياً.";
                MessageBox.Show(successMsg, "عملية ناجحة", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // السماح بالأرقام والنقطة والفاصلة لعدم التقييد بنظام التشغيل
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // منع كتابة الفاصل العشري أكثر من مرة
            TextBox tb = sender as TextBox;
            if (tb != null && (e.KeyChar == '.' || e.KeyChar == ',') && (tb.Text.Contains(".") || tb.Text.Contains(",")))
            {
                e.Handled = true;
            }
        }
    }
}