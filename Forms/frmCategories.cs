using MajesticDesktop.Models;
using MajesticDesktop.Services;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    public partial class frmCategories : Form
    {
        private CategoryService categoryService;
        private int selectedCategoryId = 0;

        public frmCategories()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            categoryService = new CategoryService();
        }

        private void frmCategories_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            StyleDataGridView();
            LoadCategories();
        }

        private void StyleDataGridView()
        {
            dgvCategories.AllowUserToAddRows = false;
            dgvCategories.AllowUserToDeleteRows = false;
            dgvCategories.AllowUserToResizeRows = false;
            dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCategories.BorderStyle = BorderStyle.None;
            dgvCategories.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvCategories.BackgroundColor = Color.FromArgb(30, 41, 59);
            dgvCategories.GridColor = Color.FromArgb(51, 65, 85);
            dgvCategories.MultiSelect = false;
            dgvCategories.ReadOnly = true;
            dgvCategories.RowHeadersVisible = false;
            dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCategories.RowTemplate.Height = 42; // كبرنا الارتفاع سنة عشان الخط الجديد

            dgvCategories.EnableHeadersVisualStyles = false;
            dgvCategories.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvCategories.ColumnHeadersHeight = 45;
            dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // استخدام خط Segoe UI المودرن للجدول
            dgvCategories.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvCategories.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCategories.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            dgvCategories.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvCategories.DefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            dgvCategories.DefaultCellStyle.ForeColor = Color.WhiteSmoke;
            dgvCategories.DefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            dgvCategories.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dgvCategories.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvCategories.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvCategories.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 50, 71);

            // ربط حدث التلوين
            dgvCategories.CellFormatting += DgvCategories_CellFormatting;
        }

        private void DgvCategories_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCategories.Columns[e.ColumnIndex].Name == "StatusText")
            {
                string status = e.Value?.ToString();
                if (status == "نشط 🟢")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(74, 222, 128); // أخضر
                }
                else if (status == "موقوف 🔴")
                {
                    e.CellStyle.ForeColor = Color.FromArgb(248, 113, 113); // أحمر
                }
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categories = categoryService.GetAllCategories();

                var displayData = categories.Select((c, index) => new
                {
                    RowNumber = index + 1,
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    StatusText = c.IsActive ? "نشط 🟢" : "موقوف 🔴",
                    IsActive = c.IsActive
                }).ToList();

                dgvCategories.DataSource = displayData;

                if (dgvCategories.Columns["Id"] != null) dgvCategories.Columns["Id"].Visible = false;
                if (dgvCategories.Columns["IsActive"] != null) dgvCategories.Columns["IsActive"].Visible = false;

                if (dgvCategories.Columns["RowNumber"] != null)
                {
                    dgvCategories.Columns["RowNumber"].HeaderText = "م";
                    dgvCategories.Columns["RowNumber"].Width = 60;
                }
                if (dgvCategories.Columns["Name"] != null)
                    dgvCategories.Columns["Name"].HeaderText = "اسم القسم";

                if (dgvCategories.Columns["Description"] != null)
                    dgvCategories.Columns["Description"].HeaderText = "الوصف";

                if (dgvCategories.Columns["StatusText"] != null)
                    dgvCategories.Columns["StatusText"].HeaderText = "الحالة";

                dgvCategories.ClearSelection();
                selectedCategoryId = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تحميل الأقسام.\n\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                Category category = new Category
                {
                    Name = txtCategoryName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    IsActive = chkIsActive.Checked
                };

                categoryService.AddCategory(category);
                MessageBox.Show("تم إضافة القسم بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedCategoryId <= 0)
            {
                MessageBox.Show("الرجاء تحديد قسم من الجدول لتعديله.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                Category category = new Category
                {
                    Id = selectedCategoryId,
                    Name = txtCategoryName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    IsActive = chkIsActive.Checked
                };

                categoryService.UpdateCategory(category);
                MessageBox.Show("تم التعديل بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedCategoryId <= 0)
            {
                MessageBox.Show("الرجاء تحديد قسم من الجدول لحذفه.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("هل أنت متأكد من حذف هذا القسم نهائياً؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    categoryService.DeleteCategory(selectedCategoryId);
                    MessageBox.Show("تم حذف القسم بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadCategories();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "لا يمكن الحذف", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                MessageBox.Show("من فضلك أدخل اسم القسم.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoryName.Focus();
                return false;
            }
            return true;
        }

        private void ClearFields()
        {
            txtCategoryName.Clear();
            txtDescription.Clear();
            chkIsActive.Checked = true;
            selectedCategoryId = 0;
            dgvCategories.ClearSelection();
            txtCategoryName.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvCategories.Rows.Count)
                return;

            DataGridViewRow row = dgvCategories.Rows[e.RowIndex];

            if (row.Cells["Id"].Value != null)
                int.TryParse(row.Cells["Id"].Value.ToString(), out selectedCategoryId);

            if (row.Cells["Name"].Value != null)
                txtCategoryName.Text = row.Cells["Name"].Value.ToString();

            if (row.Cells["Description"].Value != null)
                txtDescription.Text = row.Cells["Description"].Value.ToString();

            if (row.Cells["IsActive"].Value != null)
                chkIsActive.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value);
        }

        private void ClearSelection_Click(object sender, EventArgs e)
        {
            dgvCategories.ClearSelection();
            selectedCategoryId = 0;
        }
    }
}