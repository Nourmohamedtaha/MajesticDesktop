using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MajesticDesktop.Models;
using MajesticDesktop.Services;
using MajesticDesktop.Database;
using System.Data.SqlClient;

namespace MajesticDesktop.Forms
{
    public partial class frmPurchaseInvoice : Form
    {
        private ProductService productService;
        private PurchaseService purchaseService;
        private List<Product> availableProducts;

        public frmPurchaseInvoice()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            productService = new ProductService();
            purchaseService = new PurchaseService();

            this.ShowInTaskbar = false;
        }

        private void frmPurchaseInvoice_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                StyleDataGridView();
                LoadLocations();
                RefreshProductsList();
                ResetInvoice();
                SetupTextBoxPlaceholders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupTextBoxPlaceholders()
        {
            txtPaidAmount.Enter += (s, e) => { if (txtPaidAmount.Text.Trim() == "0" || txtPaidAmount.Text.Trim() == "0.00") txtPaidAmount.Text = ""; };
            txtPaidAmount.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txtPaidAmount.Text)) txtPaidAmount.Text = "0"; };

            txtQuantity.Enter += (s, e) => { if (txtQuantity.Text.Trim() == "0" || txtQuantity.Text.Trim() == "1") txtQuantity.Text = ""; };
            txtQuantity.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txtQuantity.Text)) txtQuantity.Text = "1"; };
        }

        private void StyleDataGridView()
        {
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.AllowUserToDeleteRows = false;
            dgvDetails.AllowUserToResizeRows = false;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetails.BorderStyle = BorderStyle.None;
            dgvDetails.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDetails.BackgroundColor = Color.FromArgb(30, 41, 59);
            dgvDetails.GridColor = Color.FromArgb(51, 65, 85);
            dgvDetails.MultiSelect = false;
            dgvDetails.ReadOnly = true;
            dgvDetails.RowHeadersVisible = false;
            dgvDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetails.RowTemplate.Height = 42;

            dgvDetails.EnableHeadersVisualStyles = false;
            dgvDetails.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvDetails.ColumnHeadersHeight = 45;
            dgvDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 23, 42);
            dgvDetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            dgvDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvDetails.DefaultCellStyle.BackColor = Color.FromArgb(30, 41, 59);
            dgvDetails.DefaultCellStyle.ForeColor = Color.WhiteSmoke;
            dgvDetails.DefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            dgvDetails.DefaultCellStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            dgvDetails.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvDetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(38, 50, 71);

            // إضافة الترقيم الوهمي
            dgvDetails.RowsAdded += (s, e) => { UpdateRowNumbers(); };
            dgvDetails.RowsRemoved += (s, e) => { UpdateRowNumbers(); };
        }

        private void UpdateRowNumbers()
        {
            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                row.Cells["colRowNumber"].Value = (row.Index + 1).ToString();
            }
        }

        private void LoadLocations()
        {
            DataTable dtLocs = new DataTable();
            dtLocs.Columns.Add("Id", typeof(int));
            dtLocs.Columns.Add("Name", typeof(string));

            using (SqlConnection con = DbConnection.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM Locations WHERE IsActive = 1 ORDER BY Id", con))
                {
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dtLocs.Rows.Add(Convert.ToInt32(reader["Id"]), reader["Name"].ToString());
                            }
                        }
                    }
                    catch { }
                }
            }

            cmbLocation.DataSource = dtLocs;
            cmbLocation.DisplayMember = "Name";
            cmbLocation.ValueMember = "Id";
            cmbLocation.DropDownStyle = ComboBoxStyle.DropDownList;
            if (dtLocs.Rows.Count > 0) cmbLocation.SelectedIndex = 0;
        }

        private void RefreshProductsList()
        {
            availableProducts = productService.GetAllProducts().Where(p => p.IsActive).ToList();
            cmbProduct.DataSource = null;
            cmbProduct.DataSource = availableProducts;
            cmbProduct.DisplayMember = "Name";
            cmbProduct.ValueMember = "Id";
            cmbProduct.SelectedIndex = -1;

            cmbProduct.DropDownStyle = ComboBoxStyle.DropDown;
            cmbProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbProduct.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void btnAddNewProduct_Click(object sender, EventArgs e)
        {
            frmProducts frm = new frmProducts();
            frm.ShowDialog();
            RefreshProductsList();
        }

        private void ResetInvoice()
        {
            txtInvoiceNo.Text = purchaseService.GenerateInvoiceNumber();
            dtpInvoiceDate.Value = DateTime.Now;
            dgvDetails.Rows.Clear();
            txtPaidAmount.Text = "0";

            ClearProductEntry();
            CalculateFinancials();
        }

        private void ClearProductEntry()
        {
            cmbProduct.SelectedIndex = -1;
            cmbProduct.Text = "";
            txtQuantity.Text = "1";
            txtUnitPrice.Clear();
            cmbProduct.Focus();
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedIndex != -1 && cmbProduct.SelectedValue != null)
            {
                if (int.TryParse(cmbProduct.SelectedValue.ToString(), out int productId))
                {
                    Product selectedProduct = availableProducts.FirstOrDefault(p => p.Id == productId);
                    if (selectedProduct != null)
                    {
                        txtUnitPrice.Text = selectedProduct.PurchasePrice.ToString("0.00");
                        txtQuantity.Text = "1";
                    }
                }
            }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (cmbLocation.SelectedValue == null)
            {
                MessageBox.Show("من فضلك اختر الفرع/المخزن المستلم أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbProduct.SelectedIndex == -1 || cmbProduct.SelectedValue == null)
            {
                MessageBox.Show("من فضلك اختر المنتج.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtQuantity.Text, out decimal qty) || qty <= 0)
            {
                MessageBox.Show("الكمية غير صحيحة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtUnitPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("الرجاء إدخال سعر صحيح للمنتج.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = Convert.ToInt32(cmbProduct.SelectedValue);
            string productName = cmbProduct.Text;
            decimal total = qty * price;

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (Convert.ToInt32(row.Cells["colProductId"].Value) == productId)
                {
                    decimal currentQty = Convert.ToDecimal(row.Cells["colQuantity"].Value);
                    row.Cells["colQuantity"].Value = (currentQty + qty).ToString("0.00");
                    row.Cells["colUnitPrice"].Value = price.ToString("0.00");
                    row.Cells["colTotalPrice"].Value = ((currentQty + qty) * price).ToString("0.00");

                    ClearProductEntry();
                    CalculateFinancials();
                    return;
                }
            }

            int rowIndex = dgvDetails.Rows.Add();
            dgvDetails.Rows[rowIndex].Cells["colProductId"].Value = productId;
            dgvDetails.Rows[rowIndex].Cells["colProductName"].Value = productName;
            dgvDetails.Rows[rowIndex].Cells["colQuantity"].Value = qty.ToString("0.00");
            dgvDetails.Rows[rowIndex].Cells["colUnitPrice"].Value = price.ToString("0.00");
            dgvDetails.Rows[rowIndex].Cells["colTotalPrice"].Value = total.ToString("0.00");

            ClearProductEntry();
            CalculateFinancials();
        }

        private void btnRemoveProduct_Click(object sender, EventArgs e)
        {
            if (dgvDetails.SelectedRows.Count > 0)
            {
                dgvDetails.Rows.Remove(dgvDetails.SelectedRows[0]);
                CalculateFinancials();
            }
        }

        private void CalculateFinancials()
        {
            decimal totalAmount = 0;

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                totalAmount += Convert.ToDecimal(row.Cells["colTotalPrice"].Value);
            }

            txtTotalAmount.Text = totalAmount.ToString("0.00");

            decimal paidAmount = 0;
            if (!string.IsNullOrWhiteSpace(txtPaidAmount.Text))
            {
                decimal.TryParse(txtPaidAmount.Text, out paidAmount);
            }

            decimal remainingAmount = totalAmount - paidAmount;
            txtRemainingAmount.Text = remainingAmount.ToString("0.00");

            if (remainingAmount > 0)
            {
                txtRemainingAmount.ForeColor = Color.FromArgb(248, 113, 113); // أحمر
            }
            else
            {
                txtRemainingAmount.ForeColor = Color.FromArgb(74, 222, 128); // أخضر
            }
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            CalculateFinancials();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvDetails.Rows.Count == 0)
            {
                MessageBox.Show("الفاتورة فارغة لا تحتوي على أصناف.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbLocation.SelectedValue == null)
            {
                MessageBox.Show("برجاء اختيار الفرع/المخزن المستلم للبضاعة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int locationId = Convert.ToInt32(cmbLocation.SelectedValue);
            decimal totalAmount = Convert.ToDecimal(txtTotalAmount.Text);
            decimal.TryParse(txtPaidAmount.Text, out decimal paidAmount);
            decimal remainingAmount = totalAmount - paidAmount;

            Purchase purchase = new Purchase
            {
                InvoiceNumber = txtInvoiceNo.Text,
                LocationId = locationId,
                SupplierId = 0, // لو فيه موردين نقدر نضيفها هنا
                InvoiceDate = dtpInvoiceDate.Value,
                TotalAmount = totalAmount,
                PaidAmount = paidAmount,
                RemainingAmount = remainingAmount
            };

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                purchase.Details.Add(new PurchaseDetail
                {
                    ProductId = Convert.ToInt32(row.Cells["colProductId"].Value),
                    Quantity = Convert.ToInt32(Convert.ToDecimal(row.Cells["colQuantity"].Value)),
                    UnitPrice = Convert.ToDecimal(row.Cells["colUnitPrice"].Value),
                    TotalPrice = Convert.ToDecimal(row.Cells["colTotalPrice"].Value)
                });
            }

            try
            {
                purchaseService.SavePurchaseInvoice(purchase);
                MessageBox.Show("تم حفظ فاتورة المشتريات وتحديث التكلفة وأرصدة الفرع المختار بنجاح.", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetInvoice();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.')) e.Handled = true;
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1)) e.Handled = true;
        }
    }
}