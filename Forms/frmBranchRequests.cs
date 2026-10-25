using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using MajesticDesktop.Models;
using MajesticDesktop.Services;

namespace MajesticDesktop.Forms
{
    public partial class frmBranchRequests : Form
    {
        private LocationService locationService;
        private ProductService productService;
        private RequestService requestService;
        private List<Product> availableProducts;

        public frmBranchRequests()
        {
            InitializeComponent();
            ApplyCustomUIStyles(); // بناء العناصر والأعمدة بأمان

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            locationService = new LocationService();
            productService = new ProductService();
            requestService = new RequestService();

            EnableDoubleBuffering(dgvDetails);
            dgvDetails.CellFormatting += Global_CellFormatting;
        }

        // تحسين أداء الجريد وتقليل التقطيع (Flickering)
        private void EnableDoubleBuffering(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, dgv, new object[] { true });
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

        private void frmBranchRequests_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                LoadInitialData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل البيانات الأساسية:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadInitialData()
        {
            // استبعاد المخزن الرئيسي من القائمة
            List<Location> locations = locationService.GetAllLocations()
                .Where(l => l.IsActive && !l.Name.Contains("رئيسي") && !l.Type.Contains("رئيسي") && l.Id != 1)
                .ToList();

            cmbLocation.DisplayMember = "Name";
            cmbLocation.ValueMember = "Id";
            cmbLocation.DataSource = locations;
            cmbLocation.SelectedIndex = -1;

            EnableComboBoxFilter(cmbLocation);

            availableProducts = productService.GetAllProducts().Where(p => p.IsActive).ToList();
            cmbProduct.DisplayMember = "Name";
            cmbProduct.ValueMember = "Id";
            cmbProduct.DataSource = availableProducts;
            cmbProduct.SelectedIndex = -1;

            EnableComboBoxFilter(cmbProduct);

            dtpRequestDate.Value = DateTime.Now;
        }

        private void EnableComboBoxFilter(ComboBox cmb)
        {
            cmb.DropDownStyle = ComboBoxStyle.DropDown;
            cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedIndex == -1 || cmbProduct.SelectedValue == null)
            {
                MessageBox.Show("من فضلك اختر المنتج المطلوب من القائمة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("من فضلك أدخل كمية صحيحة أكبر من صفر.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = Convert.ToInt32(cmbProduct.SelectedValue);
            string productName = cmbProduct.Text;

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.Cells["colProductId"].Value != null && Convert.ToInt32(row.Cells["colProductId"].Value) == productId)
                {
                    int currentQty = Convert.ToInt32(row.Cells["colQuantity"].Value);
                    row.Cells["colQuantity"].Value = currentQty + qty;
                    ClearProductEntry();
                    return;
                }
            }

            dgvDetails.Rows.Add("", productId, productName, qty);

            ClearProductEntry();
        }

        private void ClearProductEntry()
        {
            cmbProduct.SelectedIndex = -1;
            cmbProduct.Text = "";
            txtQuantity.Clear();
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
            }
            else
            {
                MessageBox.Show("من فضلك حدد المنتج المراد حذفه من الجدول أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbLocation.SelectedIndex == -1 || cmbLocation.SelectedValue == null)
            {
                MessageBox.Show("من فضلك اختر الفرع الطالب.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvDetails.Rows.Count == 0)
            {
                MessageBox.Show("لا يمكن حفظ طلب فارغ. قم بإضافة منتجات أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Request newRequest = new Request
            {
                LocationId = Convert.ToInt32(cmbLocation.SelectedValue),
                RequestDate = dtpRequestDate.Value
            };

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.IsNewRow || row.Cells["colProductId"].Value == null) continue;

                newRequest.Details.Add(new RequestDetail
                {
                    ProductId = Convert.ToInt32(row.Cells["colProductId"].Value),
                    RequestedQuantity = Convert.ToInt32(row.Cells["colQuantity"].Value)
                });
            }

            try
            {
                requestService.SaveRequest(newRequest);
                MessageBox.Show("تم إرسال الطلب بنجاح وهو الآن قيد الانتظار لموافقة الإدارة.", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                dgvDetails.Rows.Clear();
                cmbLocation.SelectedIndex = -1;
                cmbLocation.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء إرسال الطلب.\n\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}