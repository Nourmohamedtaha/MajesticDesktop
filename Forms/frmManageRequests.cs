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
    public partial class frmManageRequests : Form
    {
        private RequestService requestService;
        private LocationService locationService;
        private ProductService productService;
        private List<Request> pendingRequests;
        private List<Location> locations;
        private List<Product> products;

        public frmManageRequests()
        {
            InitializeComponent();
            ApplyCustomUIStyles(); // بناء العناصر وتنسيقها بأمان بعيداً عن الـ Designer

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            requestService = new RequestService();
            locationService = new LocationService();
            productService = new ProductService();

            EnableDoubleBuffering(dgvRequests);
            EnableDoubleBuffering(dgvDetails);

            dgvRequests.CellFormatting += Global_CellFormatting;
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

        private void frmManageRequests_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("خطأ في تحميل البيانات:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                locations = locationService.GetAllLocations();
                products = productService.GetAllProducts();
                // الداله دي متعدلة في السيرفيس بتجيب كل حاجة بـ Query واحد O(1)
                pendingRequests = requestService.GetAllPendingRequests();

                dgvRequests.Rows.Clear();
                foreach (var req in pendingRequests)
                {
                    string locName = locations.FirstOrDefault(l => l.Id == req.LocationId)?.Name ?? "غير معروف";
                    dgvRequests.Rows.Add("", req.Id, locName, req.RequestDate.ToString("yyyy/MM/dd hh:mm tt"));
                }

                if (dgvRequests.Rows.Count > 0)
                {
                    dgvRequests.Rows[0].Selected = true;
                }
                else
                {
                    dgvRequests.ClearSelection();
                    dgvDetails.Rows.Clear();
                    txtNotes.Clear();
                    pnlAction.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("فشل تحميل الطلبات المعلقة:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvRequests_SelectionChanged(object sender, EventArgs e)
        {
            dgvDetails.Rows.Clear();
            txtNotes.Clear();

            if (dgvRequests.SelectedRows.Count == 0)
            {
                pnlAction.Enabled = false;
                return;
            }

            pnlAction.Enabled = true;
            int reqId = Convert.ToInt32(dgvRequests.SelectedRows[0].Cells["colReqId"].Value);
            var req = pendingRequests.FirstOrDefault(r => r.Id == reqId);

            if (req != null && req.Details != null)
            {
                foreach (var det in req.Details)
                {
                    string prodName = products.FirstOrDefault(p => p.Id == det.ProductId)?.Name ?? "غير معروف";
                    dgvDetails.Rows.Add("", det.ProductId, prodName, det.RequestedQuantity);
                }
            }
        }

        private void btnApproveAll_Click(object sender, EventArgs e)
        {
            if (dgvRequests.SelectedRows.Count == 0) return;

            int reqId = Convert.ToInt32(dgvRequests.SelectedRows[0].Cells["colReqId"].Value);
            var req = pendingRequests.FirstOrDefault(r => r.Id == reqId);
            int locationId = req != null ? req.LocationId : 0;

            // هنبعت كل المنتجات اللي الفرع طالبها عشان تفتح في شاشة الصرف وتتعدل هناك
            List<RequestDetail> detailsToPass = new List<RequestDetail>();
            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.Cells["colDetProductId"].Value == null) continue;

                int pId = Convert.ToInt32(row.Cells["colDetProductId"].Value);
                int reqQty = Convert.ToInt32(row.Cells["colReqQty"].Value);
                detailsToPass.Add(new RequestDetail { ProductId = pId, RequestedQuantity = reqQty, ApprovedQuantity = reqQty });
            }

            if (detailsToPass.Count == 0)
            {
                MessageBox.Show("الطلب لا يحتوي على أصناف صالحة للصرف.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // الشاشة دي لازم تكون متظبطة عندك عشان تستقبل الطلب وتصرفه
            frmIssueStock frm = new frmIssueStock(locationId, reqId, "Approved", "", detailsToPass);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.WindowState = FormWindowState.Maximized;
            frm.ShowDialog();

            LoadData(); // تحديث بعد الصرف
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (dgvRequests.SelectedRows.Count == 0) return;

            string notes = txtNotes.Text.Trim();
            if (string.IsNullOrEmpty(notes))
            {
                MessageBox.Show("برجاء كتابة سبب الرفض في الملاحظات بالأسفل.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNotes.Focus();
                return;
            }

            int reqId = Convert.ToInt32(dgvRequests.SelectedRows[0].Cells["colReqId"].Value);
            List<RequestDetail> processedDetails = new List<RequestDetail>();

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                if (row.Cells["colDetProductId"].Value == null) continue;

                processedDetails.Add(new RequestDetail
                {
                    ProductId = Convert.ToInt32(row.Cells["colDetProductId"].Value),
                    RequestedQuantity = Convert.ToInt32(row.Cells["colReqQty"].Value),
                    ApprovedQuantity = 0
                });
            }

            try
            {
                requestService.ProcessRequestDecision(reqId, "Rejected", notes, processedDetails);
                MessageBox.Show("تم رفض الطلب بنجاح وإشعار الفرع بذلك.", "تم", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء رفض الطلب:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadData();
    }
}