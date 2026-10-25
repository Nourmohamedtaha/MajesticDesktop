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
    public partial class frmIssueStock : Form
    {
        private LocationService locationService;
        private ProductService productService;
        private IssueStockService issueStockService;
        private RequestService requestService;
        private List<Product> availableProducts;
        private int activeRequestId = 0;
        private int _autoLocationId = 0;
        private int _reqId = 0;
        private string _reqStatus = "";
        private string _reqNotes = "";
        private List<RequestDetail> _processedDetails = null;

        public frmIssueStock(int autoLocationId = 0, int reqId = 0, string reqStatus = "", string reqNotes = "", List<RequestDetail> processedDetails = null)
        {
            InitializeComponent();
            ApplyCustomUIStyles(); // الديزاين الآمن

            _autoLocationId = autoLocationId;
            _reqId = reqId;
            _reqStatus = reqStatus;
            _reqNotes = reqNotes;
            _processedDetails = processedDetails;

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            locationService = new LocationService();
            productService = new ProductService();
            issueStockService = new IssueStockService();
            requestService = new RequestService();

            EnableDoubleBuffering(dgvDetails);
            dgvDetails.CellFormatting += Global_CellFormatting;
        }

        // تحسين أداء الجريد
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

        private void frmIssueStock_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                LoadInitialData();
                cmbToLocation.SelectedIndexChanged += CmbToLocation_SelectedIndexChanged;
                dgvDetails.CellClick += DgvDetails_CellClick;

                if (!string.IsNullOrEmpty(_reqNotes))
                {
                    txtIssueNotes.Text = _reqNotes;
                }

                if (_reqId > 0 && _processedDetails != null)
                {
                    activeRequestId = _reqId;
                    dgvDetails.Rows.Clear();
                    foreach (var item in _processedDetails)
                    {
                        if (item.ApprovedQuantity > 0)
                        {
                            int rowIndex = dgvDetails.Rows.Add();
                            dgvDetails.Rows[rowIndex].Cells["colProductId"].Value = item.ProductId;
                            var prod = availableProducts.FirstOrDefault(p => p.Id == item.ProductId);
                            dgvDetails.Rows[rowIndex].Cells["colProductName"].Value = prod != null ? prod.Name : "";
                            dgvDetails.Rows[rowIndex].Cells["colQuantity"].Value = item.ApprovedQuantity;
                        }
                    }
                }
                else if (_autoLocationId > 0)
                {
                    CheckApprovedRequests(_autoLocationId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل الشاشة:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvDetails.Rows.Count) return;

            DataGridViewRow row = dgvDetails.Rows[e.RowIndex];
            if (row.Cells["colProductId"].Value != null)
            {
                int pId = Convert.ToInt32(row.Cells["colProductId"].Value);
                cmbProduct.SelectedValue = pId;
                txtQuantity.Text = row.Cells["colQuantity"].Value?.ToString() ?? "0";
                CheckAvailableQuantity();
                txtQuantity.Focus();
                txtQuantity.SelectAll();
            }
        }

        private void LoadInitialData()
        {
            List<Location> allLocations = locationService.GetAllLocations();
            var mainBranch = allLocations.FirstOrDefault(l => l.Name.Contains("رئيسي")) ?? allLocations.FirstOrDefault();

            cmbFromLocation.DisplayMember = "Name";
            cmbFromLocation.ValueMember = "Id";
            if (mainBranch != null)
            {
                cmbFromLocation.DataSource = new List<Location> { mainBranch };
                cmbFromLocation.SelectedIndex = 0;
                cmbFromLocation.Enabled = false;
            }

            var toLocations = allLocations.Where(l => l.Id != (mainBranch != null ? mainBranch.Id : 0)).ToList();
            cmbToLocation.DisplayMember = "Name";
            cmbToLocation.ValueMember = "Id";
            cmbToLocation.DataSource = toLocations;
            cmbToLocation.SelectedIndex = -1;

            availableProducts = productService.GetAllProducts().Where(p => p.IsActive).ToList();
            cmbProduct.DisplayMember = "Name";
            cmbProduct.ValueMember = "Id";
            cmbProduct.DataSource = availableProducts;
            cmbProduct.SelectedIndex = -1;

            dtpIssueDate.Value = DateTime.Now;

            cmbProduct.DropDownStyle = ComboBoxStyle.DropDown;
            cmbProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbProduct.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbToLocation.DropDownStyle = ComboBoxStyle.DropDown;
            cmbToLocation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbToLocation.AutoCompleteSource = AutoCompleteSource.ListItems;

            if (_autoLocationId > 0)
            {
                cmbToLocation.SelectedValue = _autoLocationId;
                cmbToLocation.Enabled = false;
            }
        }

        private void CheckAvailableQuantity()
        {
            if (cmbFromLocation.SelectedIndex != -1 && cmbProduct.SelectedIndex != -1 && cmbProduct.SelectedValue != null)
            {
                if (int.TryParse(cmbFromLocation.SelectedValue.ToString(), out int locationId) &&
                    int.TryParse(cmbProduct.SelectedValue.ToString(), out int productId))
                {
                    int availableQty = issueStockService.GetAvailableQuantity(locationId, productId);
                    txtAvailableQty.Text = availableQty.ToString();
                    return;
                }
            }
            txtAvailableQty.Text = "0";
        }

        private void cmbFromLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAvailableQuantity();
            if (_reqId == 0)
            {
                dgvDetails.Rows.Clear();
            }
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e) => CheckAvailableQuantity();

        private void CmbToLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_reqId > 0) return;

            if (cmbToLocation.SelectedIndex == -1 || cmbToLocation.SelectedValue == null)
            {
                activeRequestId = 0;
                dgvDetails.Rows.Clear();
                return;
            }

            if (int.TryParse(cmbToLocation.SelectedValue.ToString(), out int targetLocationId))
            {
                CheckApprovedRequests(targetLocationId);
            }
        }

        private void CheckApprovedRequests(int locationId)
        {
            if (_reqId > 0) return;

            try
            {
                var approvedReq = requestService.GetApprovedRequestByLocation(locationId);

                if (approvedReq != null && approvedReq.Details != null && approvedReq.Details.Count > 0)
                {
                    activeRequestId = approvedReq.Id;
                    dgvDetails.Rows.Clear();

                    foreach (var item in approvedReq.Details)
                    {
                        if (item.ApprovedQuantity > 0)
                        {
                            int rowIndex = dgvDetails.Rows.Add();
                            dgvDetails.Rows[rowIndex].Cells["colProductId"].Value = item.ProductId;

                            var prod = availableProducts.FirstOrDefault(p => p.Id == item.ProductId);
                            dgvDetails.Rows[rowIndex].Cells["colProductName"].Value = prod != null ? prod.Name : "";
                            dgvDetails.Rows[rowIndex].Cells["colQuantity"].Value = item.ApprovedQuantity;
                        }
                    }
                }
                else
                {
                    activeRequestId = 0;
                }
            }
            catch { }
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (cmbFromLocation.SelectedIndex == -1 || cmbProduct.SelectedIndex == -1 || cmbProduct.SelectedValue == null)
            {
                MessageBox.Show("من فضلك اختر المنتج من الجدول بالأسفل أو من القائمة بالأعلى.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int inputQty) || inputQty <= 0)
            {
                MessageBox.Show("من فضلك أدخل كمية صحيحة أكبر من الصفر.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int fromLocId = Convert.ToInt32(cmbFromLocation.SelectedValue);
            int prodId = Convert.ToInt32(cmbProduct.SelectedValue);
            int availableQty = issueStockService.GetAvailableQuantity(fromLocId, prodId);

            if (inputQty > availableQty)
            {
                MessageBox.Show($"الرصيد المتاح من هذا الصنف ({availableQty}) لا يكفي لصرف ({inputQty}).", "رصيد غير كافٍ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string productName = cmbProduct.Text;
            bool isUpdated = false;

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.Cells["colProductId"].Value != null && Convert.ToInt32(row.Cells["colProductId"].Value) == prodId)
                {
                    row.Cells["colQuantity"].Value = inputQty;
                    isUpdated = true;
                    break;
                }
            }

            if (!isUpdated)
            {
                int rowIndex = dgvDetails.Rows.Add();
                dgvDetails.Rows[rowIndex].Cells["colProductId"].Value = prodId;
                dgvDetails.Rows[rowIndex].Cells["colProductName"].Value = productName;
                dgvDetails.Rows[rowIndex].Cells["colQuantity"].Value = inputQty;
            }

            ClearProductEntry();
        }

        private void ClearProductEntry()
        {
            cmbProduct.SelectedIndex = -1;
            cmbProduct.Text = "";
            txtQuantity.Clear();
            txtAvailableQty.Text = "0";
            cmbProduct.Focus();
            dgvDetails.ClearSelection();
        }

        private void btnRemoveProduct_Click(object sender, EventArgs e)
        {
            if (dgvDetails.SelectedRows.Count > 0)
            {
                dgvDetails.Rows.Remove(dgvDetails.SelectedRows[0]);
                ClearProductEntry();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            dgvDetails.EndEdit();

            if (cmbFromLocation.SelectedIndex == -1 || cmbToLocation.SelectedIndex == -1)
            {
                MessageBox.Show("من فضلك حدد المخزن المصدر والفرع المستلم.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvDetails.Rows.Count == 0)
            {
                MessageBox.Show("لا توجد أصناف في إذن الصرف.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int fromLocationId = Convert.ToInt32(cmbFromLocation.SelectedValue);
            int toLocationId = Convert.ToInt32(cmbToLocation.SelectedValue);

            if (fromLocationId == toLocationId)
            {
                MessageBox.Show("لا يمكن التحويل من وإلى نفس المكان.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.IsNewRow || row.Cells["colProductId"].Value == null) continue;

                int pId = Convert.ToInt32(row.Cells["colProductId"].Value);
                int qtyToIssue = Convert.ToInt32(row.Cells["colQuantity"].Value);
                string pName = row.Cells["colProductName"].Value.ToString();

                int availableQty = issueStockService.GetAvailableQuantity(fromLocationId, pId);

                if (qtyToIssue > availableQty)
                {
                    MessageBox.Show($"عفواً، لا يوجد رصيد كافٍ من الصنف: {pName}\nالمتاح في المخزن: {availableQty}\nالمراد صرفه: {qtyToIssue}", "لا يمكن إتمام الصرف", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgvDetails.ClearSelection();
                    row.Selected = true;
                    return;
                }
            }

            if (MessageBox.Show("تأكيد خصم البضاعة من المخزن الرئيسي وإضافتها للفرع المستلم؟", "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            IssueStock newIssue = new IssueStock
            {
                FromLocationId = fromLocationId,
                ToLocationId = toLocationId,
                IssueDate = dtpIssueDate.Value
            };

            List<RequestDetail> finalDetails = new List<RequestDetail>();

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.IsNewRow || row.Cells["colProductId"].Value == null) continue;

                int pId = Convert.ToInt32(row.Cells["colProductId"].Value);
                int qty = Convert.ToInt32(row.Cells["colQuantity"].Value);

                newIssue.Details.Add(new IssueStockDetail
                {
                    ProductId = pId,
                    Quantity = qty
                });

                if (_reqId > 0 && _processedDetails != null)
                {
                    var originalReqDet = _processedDetails.FirstOrDefault(d => d.ProductId == pId);
                    int reqQty = originalReqDet != null ? originalReqDet.RequestedQuantity : qty;
                    finalDetails.Add(new RequestDetail
                    {
                        ProductId = pId,
                        RequestedQuantity = reqQty,
                        ApprovedQuantity = qty
                    });
                }
            }

            try
            {
                issueStockService.SaveIssueStock(newIssue);

                string finalNotes = txtIssueNotes.Text.Trim();

                if (_reqId > 0)
                {
                    requestService.ProcessRequestDecision(_reqId, _reqStatus, finalNotes, finalDetails);
                    requestService.UpdateRequestStatus(_reqId, "تم الصرف");
                }
                else if (activeRequestId > 0)
                {
                    requestService.UpdateRequestStatus(activeRequestId, "تم الصرف");
                }

                MessageBox.Show("تم تنفيذ وصرف البضاعة بنجاح.", "تم الصرف", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء تنفيذ إذن الصرف:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
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