using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MajesticDesktop.Models;
using MajesticDesktop.Services;
using System.Data.SqlClient;
using MajesticDesktop.Database;

namespace MajesticDesktop.Forms
{
    public partial class frmUsers : Form
    {
        private UserService userService;
        private int selectedUserId = 0;
        private bool isInitializing = true;
        private bool _busy;

        // الألوان الموحدة
        private static readonly Color ClrBgDark = Color.FromArgb(15, 23, 42);
        private static readonly Color ClrBgPanel = Color.FromArgb(30, 41, 59);
        private static readonly Color ClrGridHeader = Color.FromArgb(20, 30, 45);
        private static readonly Color ClrGridHeaderText = Color.White;
        private static readonly Color ClrText = Color.FromArgb(241, 245, 249);
        private static readonly Color ClrAccent = Color.FromArgb(59, 130, 246);
        private static readonly Color ClrRed = Color.FromArgb(239, 68, 68);

        public frmUsers()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            userService = new UserService();

            // تفعيل DoubleBuffering للجدول لمنع الرعشة
            this.DoubleBuffered = true;
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, dgvUsers, new object[] { true });
        }

        private async void frmUsers_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                await FixRolesInDatabaseAsync();
                StyleDataGridView();
                await LoadLocationsAsync();
                await LoadRolesAsync();
                await LoadUsersAsync();
                isInitializing = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void SetBusy(bool busy)
        {
            _busy = busy;
            btnSave.Enabled = !busy;
            btnEdit.Enabled = !busy;
            btnClear.Enabled = !busy;
            UseWaitCursor = busy;
        }

        private async Task FixRolesInDatabaseAsync()
        {
            try
            {
                using (SqlConnection con = DbConnection.CreateConnection())
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(@"
                        UPDATE Roles SET RoleName = N'مدير' WHERE RoleName IN (N'مدير نظام', N'سوبر ادمن', 'SuperAdmin', 'Super Admin');
                        UPDATE Roles SET RoleName = N'مسئول فرع' WHERE RoleName IN (N'ادمن', N'أدمن فرع', 'Admin', 'BranchAdmin');
                        
                        IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = N'مدير')
                            INSERT INTO Roles (RoleName) VALUES (N'مدير');
                            
                        IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = N'مسئول فرع')
                            INSERT INTO Roles (RoleName) VALUES (N'مسئول فرع');
                    ", con))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch { }
        }

        private void StyleDataGridView()
        {
            this.BackColor = ClrBgDark;
            pnlHeader.BackColor = ClrGridHeader;
            pnlDataEntry.BackColor = ClrBgPanel;
            lblTitle.ForeColor = ClrText;

            dgvUsers.BackgroundColor = ClrBgDark;
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUsers.GridColor = Color.FromArgb(51, 65, 85);
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvUsers.ColumnHeadersHeight = 55;
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = ClrGridHeader;
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = ClrGridHeaderText;
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 13.5F, FontStyle.Bold);
            dgvUsers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvUsers.ColumnHeadersDefaultCellStyle.SelectionBackColor = ClrGridHeader;

            dgvUsers.DefaultCellStyle.BackColor = ClrBgPanel;
            dgvUsers.DefaultCellStyle.ForeColor = ClrText;
            dgvUsers.DefaultCellStyle.SelectionBackColor = ClrAccent;
            dgvUsers.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvUsers.DefaultCellStyle.Font = new Font("Segoe UI", 12.5F, FontStyle.Bold);
            dgvUsers.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvUsers.DefaultCellStyle.Padding = new Padding(4, 0, 4, 0);

            dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 50, 71);
            dgvUsers.RowTemplate.Height = 50;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AllowUserToResizeRows = false;
            dgvUsers.ReadOnly = true;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.MultiSelect = false;

            // إخفاء الـ Id
            if (dgvUsers.Columns.Contains("colId"))
            {
                dgvUsers.Columns["colId"].Visible = false;
            }
        }

        private async Task LoadLocationsAsync()
        {
            List<Location> locs = (await userService.GetAllLocationsAsync())
                .Where(l => l.Id != 1 && !l.Name.Contains("المركز") && !l.Name.Contains("مخزن"))
                .ToList();

            cmbLocation.DataSource = locs;
            cmbLocation.DisplayMember = "Name";
            cmbLocation.ValueMember = "Id";
            cmbLocation.SelectedIndex = -1;
        }

        private async Task LoadRolesAsync()
        {
            List<Role> roles = (await userService.GetAllRolesAsync())
                .Where(r => r.RoleName == "مدير" || r.RoleName == "مسئول فرع")
                .ToList();

            cmbRole.DataSource = roles;
            cmbRole.DisplayMember = "RoleName";
            cmbRole.ValueMember = "Id";
            cmbRole.SelectedIndex = -1;
        }

        private async Task LoadUsersAsync()
        {
            List<User> users = await userService.GetAllUsersAsync();
            dgvUsers.Rows.Clear();

            int counter = 1;
            foreach (var u in users)
            {
                int rowIndex = dgvUsers.Rows.Add();
                dgvUsers.Rows[rowIndex].Cells["colId"].Value = u.Id;

                // استخدام عمود م للتسلسل لو موجود، أو نعتمد إن العمود الأول هو المسلسل
                if (dgvUsers.Columns.Contains("colM"))
                {
                    dgvUsers.Rows[rowIndex].Cells["colM"].Value = counter++;
                }

                dgvUsers.Rows[rowIndex].Cells["colFullName"].Value = u.FullName;
                dgvUsers.Rows[rowIndex].Cells["colUsername"].Value = u.Username;
                dgvUsers.Rows[rowIndex].Cells["colPassword"].Value = u.Password;
                dgvUsers.Rows[rowIndex].Cells["colRole"].Value = u.RoleName;

                dgvUsers.Rows[rowIndex].Cells["colLocation"].Value = string.IsNullOrEmpty(u.BranchName) ? "المخزن الرئيسي" : u.BranchName;

                dgvUsers.Rows[rowIndex].Cells["colStatus"].Value = u.IsActive ? "نشط" : "موقوف";

                if (!u.IsActive)
                {
                    dgvUsers.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(60, 30, 40);
                    dgvUsers.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(250, 100, 100);
                    dgvUsers.Rows[rowIndex].DefaultCellStyle.SelectionBackColor = Color.FromArgb(80, 40, 50);
                }
            }

            ClearFields();
        }

        private void ClearFields()
        {
            txtFullName.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            cmbRole.SelectedIndex = -1;
            cmbLocation.SelectedIndex = -1;

            // إعادة ضبط الليبل بناءً على الصلاحية الافتراضية
            lblLocation.Text = "الفرع (إجباري لمستوى مسئول الفرع فقط)";
            cmbLocation.Enabled = true;

            chkIsActive.Checked = true;
            selectedUserId = 0;
            dgvUsers.ClearSelection();
            txtFullName.Focus();
        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializing) return;

            if (cmbRole.SelectedIndex != -1)
            {
                if (cmbRole.Text == "مدير")
                {
                    lblLocation.Text = "الفرع: (المخزن الرئيسي - تلقائياً)";
                    cmbLocation.SelectedIndex = -1;
                    cmbLocation.Enabled = false;
                }
                else
                {
                    lblLocation.Text = "الفرع (إجباري لمستوى مسئول الفرع فقط)";
                    cmbLocation.Enabled = true;
                }
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            if (!ValidateInput()) return;

            SetBusy(true);

            User user = new User
            {
                FullName = txtFullName.Text.Trim(),
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                RoleId = Convert.ToInt32(cmbRole.SelectedValue),
                IsActive = chkIsActive.Checked,
                LocationId = cmbLocation.Enabled && cmbLocation.SelectedValue != null ? Convert.ToInt32(cmbLocation.SelectedValue) : (int?)null
            };

            try
            {
                await userService.SaveUserAsync(user, false);
                MessageBox.Show("تم إضافة المستخدم بنجاح.", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                    MessageBox.Show("اسم المستخدم (Username) موجود بالفعل، يرجى اختيار اسم آخر.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                else
                    MessageBox.Show("حدث خطأ أثناء الحفظ.\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private async void btnEdit_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            if (selectedUserId <= 0)
            {
                MessageBox.Show("اختر مستخدماً أولاً للتعديل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return;
            }

            if (!ValidateInput()) return;

            SetBusy(true);

            User user = new User
            {
                Id = selectedUserId,
                FullName = txtFullName.Text.Trim(),
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                RoleId = Convert.ToInt32(cmbRole.SelectedValue),
                IsActive = chkIsActive.Checked,
                LocationId = cmbLocation.Enabled && cmbLocation.SelectedValue != null ? Convert.ToInt32(cmbLocation.SelectedValue) : (int?)null
            };

            try
            {
                await userService.SaveUserAsync(user, true);
                MessageBox.Show("تم تعديل بيانات المستخدم بنجاح.", "تم التعديل", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                await LoadUsersAsync();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("UNIQUE KEY"))
                    MessageBox.Show("اسم المستخدم (Username) الجديد موجود بالفعل لمستخدم آخر.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                else
                    MessageBox.Show("حدث خطأ أثناء التعديل.\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            }
            finally
            {
                SetBusy(false);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("من فضلك أكمل جميع البيانات الأساسية.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return false;
            }

            if (cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("من فضلك اختر مستوى الصلاحية.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return false;
            }

            if (cmbLocation.Enabled && cmbLocation.SelectedIndex == -1)
            {
                MessageBox.Show("من فضلك حدد الفرع الخاص بمسئول الفرع.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return false;
            }

            return true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                selectedUserId = Convert.ToInt32(row.Cells["colId"].Value);
                txtFullName.Text = row.Cells["colFullName"].Value?.ToString();
                txtUsername.Text = row.Cells["colUsername"].Value?.ToString();
                txtPassword.Text = row.Cells["colPassword"].Value?.ToString();

                // ده هيشغل حدث التغيير ويقفل/يفتح بناءً على نوع الصلاحية
                cmbRole.Text = row.Cells["colRole"].Value?.ToString();

                chkIsActive.Checked = row.Cells["colStatus"].Value?.ToString() == "نشط";

                string branch = row.Cells["colLocation"].Value?.ToString();
                if (!string.IsNullOrEmpty(branch) && branch != "المخزن الرئيسي")
                {
                    cmbLocation.Text = branch;
                }
                else
                {
                    cmbLocation.SelectedIndex = -1;
                }
            }
        }

        private void btnPermissions_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "النظام يعتمد على مستويين فقط:\n\n1. (مدير): له حق الإدارة الكاملة للديسكتوب والمخزن.\n2. (مسئول فرع): مخصص للعمل على تطبيق الموبايل لإدارة فرعه فقط.",
                "معلومات الصلاحيات 🔐",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading
            );
        }
    }
}