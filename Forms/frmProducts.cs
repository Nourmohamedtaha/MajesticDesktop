using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MajesticDesktop.Models;
using MajesticDesktop.Services;

namespace MajesticDesktop.Forms
{
    public partial class frmProducts : Form
    {
        private ProductService productService;
        private CategoryService categoryService;
        private List<Product> products = new List<Product>();
        private List<Category> categories = new List<Category>();
        private int selectedProductId = 0;
        private string imagePath = "";

        public frmProducts()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            productService = new ProductService();
            categoryService = new CategoryService();

            SetupTextBoxPlaceholders();
        }

        private void frmProducts_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            try
            {
                StyleDataGridView();
                LoadCategories();
                LoadProducts();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupTextBoxPlaceholders()
        {
            TextBox[] textboxes = { txtPurchasePrice, txtInitialQty, txtAlertLimit };
            foreach (var txt in textboxes)
            {
                txt.Enter += (s, e) =>
                {
                    if (txt.Text.Trim() == "0" || txt.Text.Trim() == "0.00" || txt.Text.Trim() == "5")
                        txt.Text = "";
                };

                txt.Leave += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txt.Text))
                    {
                        if (txt == txtAlertLimit) txt.Text = "5";
                        else txt.Text = "0";
                    }
                };
            }
        }

        private void StyleDataGridView()
        {
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.AllowUserToResizeRows = false;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.BorderStyle = BorderStyle.None;
            dgvProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProducts.BackgroundColor = Color.FromArgb(30, 41, 59);
            dgvProducts.GridColor = Color.FromArgb(51, 65, 85);
            dgvProducts.MultiSelect = false;
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.RowTemplate.Height = 42;

            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvProducts.ColumnHeadersHeight = 45;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            dgvProducts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvProducts.DefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            dgvProducts.DefaultCellStyle.ForeColor = Color.WhiteSmoke;
            dgvProducts.DefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dgvProducts.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvProducts.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 50, 71);

            dgvProducts.CellFormatting += DgvProducts_CellFormatting;
        }

        private void DgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // تلوين الكمية لو وصلت للحد الخطر
            if (dgvProducts.Columns[e.ColumnIndex].Name == "colQuantity")
            {
                if (decimal.TryParse(e.Value?.ToString(), out decimal qty))
                {
                    // جلب الحد من الصف نفسه
                    int alertLimit = 5;
                    var alertCell = dgvProducts.Rows[e.RowIndex].Cells["colAlertLimit"].Value;
                    if (alertCell != null) int.TryParse(alertCell.ToString(), out alertLimit);

                    if (qty <= alertLimit)
                        e.CellStyle.ForeColor = Color.FromArgb(248, 113, 113); // أحمر
                    else
                        e.CellStyle.ForeColor = Color.WhiteSmoke;
                }
            }
            // تلوين الحالة (نشط / غير نشط)
            else if (dgvProducts.Columns[e.ColumnIndex].Name == "colStatus")
            {
                string status = e.Value?.ToString();
                if (status == "نشط 🟢")
                    e.CellStyle.ForeColor = Color.FromArgb(74, 222, 128); // أخضر
                else if (status == "موقوف 🔴")
                    e.CellStyle.ForeColor = Color.FromArgb(248, 113, 113); // أحمر
            }
        }

        private void LoadCategories()
        {
            categories = categoryService.GetAllCategories();
            List<Category> activeCategories = categories.Where(c => c.IsActive).ToList();

            cmbCategory.DropDownStyle = ComboBoxStyle.DropDown;
            cmbCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbCategory.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbCategory.DataSource = null;
            cmbCategory.DataSource = activeCategories;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Id";
            cmbCategory.SelectedIndex = -1;

            cmbCategoryFilter.DropDownStyle = ComboBoxStyle.DropDown;
            cmbCategoryFilter.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbCategoryFilter.AutoCompleteSource = AutoCompleteSource.ListItems;

            List<CategoryFilterItem> filterItems = new List<CategoryFilterItem> { new CategoryFilterItem { Id = 0, Name = "كل الأقسام" } };
            filterItems.AddRange(activeCategories.Select(c => new CategoryFilterItem { Id = c.Id, Name = c.Name }));

            cmbCategoryFilter.DataSource = null;
            cmbCategoryFilter.DataSource = filterItems;
            cmbCategoryFilter.DisplayMember = "Name";
            cmbCategoryFilter.ValueMember = "Id";
            cmbCategoryFilter.SelectedValue = 0;
        }

        private void LoadProducts()
        {
            try
            {
                products = productService.GetAllProducts();
                ApplyFilters();
                dgvProducts.ClearSelection();
                selectedProductId = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            string search = txtSearch.Text.Trim().ToLower();
            int categoryId = 0;

            if (cmbCategoryFilter.SelectedValue != null && int.TryParse(cmbCategoryFilter.SelectedValue.ToString(), out int parsedId))
                categoryId = parsedId;

            IEnumerable<Product> filteredProducts = products;

            // 💡 البحث الذكي: البحث بأول حرف من أي كلمة في اسم المنتج أو الوصف
            if (!string.IsNullOrWhiteSpace(search))
            {
                filteredProducts = filteredProducts.Where(p =>
                    (!string.IsNullOrWhiteSpace(p.Name) && p.Name.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Any(word => word.StartsWith(search))) ||
                    (!string.IsNullOrWhiteSpace(p.Notes) && p.Notes.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Any(word => word.StartsWith(search)))
                );
            }

            if (categoryId > 0)
                filteredProducts = filteredProducts.Where(p => p.CategoryId == categoryId);

            if (chkLowStock.Checked)
                filteredProducts = filteredProducts.Where(p => p.CurrentQuantity <= p.AlertLimit);

            var displayList = filteredProducts.Select((p, index) => new
            {
                RowNumber = index + 1,
                Id = p.Id,
                Name = p.Name,
                CategoryName = GetCategoryName(p.CategoryId),
                Quantity = p.CurrentQuantity,
                Price = p.PurchasePrice,
                Notes = p.Notes,
                AlertLimit = p.AlertLimit,
                StatusText = p.IsActive ? "نشط 🟢" : "موقوف 🔴",
                IsActive = p.IsActive,
                CategoryId = p.CategoryId
            }).ToList();

            dgvProducts.Rows.Clear();

            foreach (var item in displayList)
            {
                int rowIndex = dgvProducts.Rows.Add();
                dgvProducts.Rows[rowIndex].Cells["colRowNumber"].Value = item.RowNumber;
                dgvProducts.Rows[rowIndex].Cells["colId"].Value = item.Id;
                dgvProducts.Rows[rowIndex].Cells["colProductName"].Value = item.Name;
                dgvProducts.Rows[rowIndex].Cells["colCategory"].Value = item.CategoryName;
                dgvProducts.Rows[rowIndex].Cells["colQuantity"].Value = item.Quantity.ToString("0.00");
                dgvProducts.Rows[rowIndex].Cells["colPurchasePrice"].Value = item.Price.ToString("0.00");
                dgvProducts.Rows[rowIndex].Cells["colNotes"].Value = item.Notes;
                dgvProducts.Rows[rowIndex].Cells["colAlertLimit"].Value = item.AlertLimit;
                dgvProducts.Rows[rowIndex].Cells["colStatus"].Value = item.StatusText;
            }

            dgvProducts.ClearSelection();
        }

        private string GetCategoryName(int categoryId)
        {
            Category category = categories.FirstOrDefault(c => c.Id == categoryId);
            return category != null ? category.Name : "غير محدد";
        }

        private void ClearFields()
        {
            txtProductName.Clear();
            cmbCategory.SelectedIndex = -1;
            txtPurchasePrice.Text = "0";
            txtInitialQty.Text = "0";
            txtInitialQty.Enabled = true; // نفتح الكمية لأننا بنسجل جديد
            txtNotes.Clear();
            txtAlertLimit.Text = "5";
            chkIsActive.Checked = true;
            imagePath = "";

            if (picProductImage.Image != null)
            {
                Image oldImage = picProductImage.Image;
                picProductImage.Image = null;
                oldImage.Dispose();
            }

            selectedProductId = 0;
            dgvProducts.ClearSelection();
            txtProductName.Focus();
        }

        private bool ValidateProductData(out string name, out int categoryId, out int alertLimit, out decimal price, out decimal initialQty)
        {
            name = txtProductName.Text.Trim();
            categoryId = 0;
            alertLimit = 5;
            price = 0;
            initialQty = 0;

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("من فضلك أدخل اسم المنتج.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return false;
            }

            if (cmbCategory.SelectedValue == null || !int.TryParse(cmbCategory.SelectedValue.ToString(), out categoryId) || categoryId <= 0)
            {
                MessageBox.Show("من فضلك اختر القسم الصحيح من القائمة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            decimal.TryParse(txtPurchasePrice.Text, out price);
            decimal.TryParse(txtInitialQty.Text, out initialQty);
            if (!int.TryParse(txtAlertLimit.Text, out alertLimit) || alertLimit < 0) alertLimit = 5;

            if (!string.IsNullOrWhiteSpace(imagePath) && !File.Exists(imagePath))
            {
                imagePath = "";
                if (picProductImage.Image != null)
                {
                    Image oldImage = picProductImage.Image;
                    picProductImage.Image = null;
                    oldImage.Dispose();
                }
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateProductData(out string name, out int categoryId, out int alertLimit, out decimal price, out decimal qty)) return;

            try
            {
                Product product = new Product
                {
                    Name = name,
                    CategoryId = categoryId,
                    ImagePath = imagePath,
                    IsActive = chkIsActive.Checked,
                    AlertLimit = alertLimit,
                    PurchasePrice = price,
                    CurrentQuantity = qty,
                    Notes = txtNotes.Text.Trim()
                };

                productService.AddProduct(product);
                MessageBox.Show("تم حفظ المنتج والكمية الافتتاحية بنجاح.", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProducts();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedProductId <= 0)
            {
                MessageBox.Show("من فضلك اختر منتجًا من القائمة أولًا.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateProductData(out string name, out int categoryId, out int alertLimit, out decimal price, out decimal qty)) return;

            try
            {
                Product selectedProduct = products.FirstOrDefault(p => p.Id == selectedProductId);
                if (selectedProduct == null) return;

                Product product = new Product
                {
                    Id = selectedProductId,
                    Name = name,
                    CategoryId = categoryId,
                    ImagePath = imagePath,
                    IsActive = chkIsActive.Checked,
                    AlertLimit = alertLimit,
                    PurchasePrice = price,
                    Notes = txtNotes.Text.Trim()
                };

                productService.UpdateProduct(product);
                MessageBox.Show("تم تعديل بيانات المنتج بنجاح.\n(ملحوظة: لتعديل رصيد المخزن استخدم شاشة الجرد أو فاتورة المشتريات)", "تم التعديل", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProducts();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedProductId <= 0)
            {
                MessageBox.Show("من فضلك اختر منتجًا من القائمة أولًا.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("هل أنت متأكد من حذف هذا المنتج نهائياً؟", "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    productService.DeleteProduct(selectedProductId);
                    MessageBox.Show("تم حذف المنتج بنجاح.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "مرفوض", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e) { ClearFields(); }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try { LoadCategories(); LoadProducts(); ClearFields(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) { ApplyFilters(); }

        private void cmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsHandleCreated) ApplyFilters();
        }

        private void chkLowStock_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvProducts.Rows.Count) return;

            DataGridViewRow row = dgvProducts.Rows[e.RowIndex];
            if (row.Cells["colId"].Value == null || !int.TryParse(row.Cells["colId"].Value.ToString(), out int productId)) return;

            Product product = products.FirstOrDefault(p => p.Id == productId);
            if (product == null) return;

            selectedProductId = product.Id;
            txtProductName.Text = product.Name;
            cmbCategory.SelectedValue = categories.Any(c => c.Id == product.CategoryId) ? (object)product.CategoryId : -1;
            txtPurchasePrice.Text = product.PurchasePrice.ToString("0.00");
            txtAlertLimit.Text = product.AlertLimit.ToString();
            txtNotes.Text = product.Notes;

            txtInitialQty.Text = product.CurrentQuantity.ToString("0.00");
            txtInitialQty.Enabled = false;

            chkIsActive.Checked = product.IsActive;
            imagePath = product.ImagePath ?? "";

            LoadProductImage(product.ImagePath);
        }

        private void LoadProductImage(string path)
        {
            if (picProductImage.Image != null)
            {
                Image oldImage = picProductImage.Image;
                picProductImage.Image = null;
                oldImage.Dispose();
            }

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return;

            try
            {
                using (Image tempImage = Image.FromFile(path)) { picProductImage.Image = new Bitmap(tempImage); }
            }
            catch { picProductImage.Image = null; }
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog { Title = "اختيار صورة المنتج", Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif", Multiselect = false })
            {
                if (dialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    imagePath = dialog.FileName;
                    LoadProductImage(imagePath);
                }
                catch (Exception ex)
                {
                    imagePath = "";
                    MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.')) e.Handled = true;
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1)) e.Handled = true;
        }

        private void txtAlertLimit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private class CategoryFilterItem { public int Id { get; set; } public string Name { get; set; } }
    }
}