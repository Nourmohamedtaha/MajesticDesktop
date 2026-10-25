using System.Drawing;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    // ألوان الثيم الموحدة
    internal static class BranchThemeRequestsManage
    {
        public static readonly Color Bg = Color.FromArgb(15, 23, 42);
        public static readonly Color Header = Color.FromArgb(9, 14, 28);
        public static readonly Color Surface = Color.FromArgb(30, 41, 59);
        public static readonly Color Field = Color.FromArgb(24, 34, 51);
        public static readonly Color Input = Color.FromArgb(15, 23, 42);
        public static readonly Color Border = Color.FromArgb(51, 65, 85);
        public static readonly Color Text = Color.FromArgb(241, 245, 249);
        public static readonly Color Muted = Color.FromArgb(148, 163, 184);
        public static readonly Color Accent = Color.FromArgb(59, 130, 246);
        public static readonly Color Green = Color.FromArgb(22, 163, 74);
        public static readonly Color Red = Color.FromArgb(220, 38, 38);

        public static Font UiFont(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font("Segoe UI", size, style);
        }
    }

    partial class frmManageRequests
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlHeaderLine = new System.Windows.Forms.Panel();
            this.pnlTitleContainer = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.pnlRefreshContainer = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();

            this.tblMain = new System.Windows.Forms.TableLayoutPanel();
            this.dgvRequests = new System.Windows.Forms.DataGridView();
            this.colReqSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReqId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReqLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReqDate = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.pnlDivider = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();

            this.dgvDetails = new System.Windows.Forms.DataGridView();
            this.colDetSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetProductId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDetProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReqQty = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.pnlAction = new System.Windows.Forms.Panel();
            this.pnlNotesTop = new System.Windows.Forms.Panel();
            this.lblNotes = new System.Windows.Forms.Label();
            this.pnlNotesMiddle = new System.Windows.Forms.Panel();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.pnlActionButtons = new System.Windows.Forms.Panel();
            this.btnApproveAll = new System.Windows.Forms.Button();
            this.btnReject = new System.Windows.Forms.Button();

            this.pnlHeader.SuspendLayout();
            this.pnlTitleContainer.SuspendLayout();
            this.pnlRefreshContainer.SuspendLayout();
            this.tblMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).BeginInit();
            this.pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
            this.pnlAction.SuspendLayout();
            this.pnlNotesTop.SuspendLayout();
            this.pnlNotesMiddle.SuspendLayout();
            this.pnlActionButtons.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.pnlTitleContainer);
            this.pnlHeader.Controls.Add(this.pnlRefreshContainer);
            this.pnlHeader.Controls.Add(this.pnlHeaderLine);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1200, 90);
            this.pnlHeader.TabIndex = 0;
            // 
            // pnlHeaderLine
            // 
            this.pnlHeaderLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlHeaderLine.Height = 4;
            this.pnlHeaderLine.Name = "pnlHeaderLine";
            this.pnlHeaderLine.TabIndex = 2;
            //
            // pnlTitleContainer
            //
            this.pnlTitleContainer.Dock = DockStyle.Fill;
            this.pnlTitleContainer.Controls.Add(this.lblSubtitle);
            this.pnlTitleContainer.Controls.Add(this.lblTitle);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = false;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Height = 45;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "إدارة طلبات الفروع 📦";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = false;
            this.lblSubtitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubtitle.Height = 35;
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "مراجعة الطلبات الواردة من الفروع (الرفض أو التحويل للصرف)";
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //
            // pnlRefreshContainer
            //
            this.pnlRefreshContainer.Dock = DockStyle.Left;
            this.pnlRefreshContainer.Width = 200;
            this.pnlRefreshContainer.Controls.Add(this.btnRefresh);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            this.btnRefresh.Location = new System.Drawing.Point(20, 20);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(160, 45);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "تحديث الطلبات 🔄";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tblMain
            // 
            this.tblMain.ColumnCount = 3;
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tblMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tblMain.Controls.Add(this.dgvRequests, 0, 0);
            this.tblMain.Controls.Add(this.pnlDivider, 1, 0);
            this.tblMain.Controls.Add(this.pnlRight, 2, 0);
            this.tblMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblMain.Location = new System.Drawing.Point(0, 90);
            this.tblMain.Name = "tblMain";
            this.tblMain.Padding = new System.Windows.Forms.Padding(15);
            this.tblMain.RowCount = 1;
            this.tblMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblMain.TabIndex = 1;
            // 
            // dgvRequests
            // 
            this.dgvRequests.AllowUserToAddRows = false;
            this.dgvRequests.AllowUserToDeleteRows = false;
            this.dgvRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRequests.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRequests.Location = new System.Drawing.Point(735, 18);
            this.dgvRequests.Margin = new System.Windows.Forms.Padding(5, 3, 10, 3);
            this.dgvRequests.Name = "dgvRequests";
            this.dgvRequests.ReadOnly = true;
            this.dgvRequests.TabIndex = 0;
            this.dgvRequests.SelectionChanged += new System.EventHandler(this.dgvRequests_SelectionChanged);
            // 
            // pnlDivider
            // 
            this.pnlDivider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDivider.Location = new System.Drawing.Point(720, 18);
            this.pnlDivider.Margin = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.TabIndex = 2;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.dgvDetails);
            this.pnlRight.Controls.Add(this.pnlAction);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(18, 18);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.pnlRight.TabIndex = 1;
            // 
            // dgvDetails
            // 
            this.dgvDetails.AllowUserToAddRows = false;
            this.dgvDetails.AllowUserToDeleteRows = false;
            this.dgvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetails.Location = new System.Drawing.Point(10, 0);
            this.dgvDetails.Name = "dgvDetails";
            this.dgvDetails.ReadOnly = true;
            this.dgvDetails.TabIndex = 1;
            // 
            // pnlAction
            // 
            this.pnlAction.Controls.Add(this.pnlNotesMiddle);
            this.pnlAction.Controls.Add(this.pnlNotesTop);
            this.pnlAction.Controls.Add(this.pnlActionButtons);
            this.pnlAction.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAction.Location = new System.Drawing.Point(10, 480);
            this.pnlAction.Name = "pnlAction";
            this.pnlAction.Size = new System.Drawing.Size(692, 220);
            this.pnlAction.TabIndex = 0;
            //
            // pnlNotesTop
            //
            this.pnlNotesTop.Dock = DockStyle.Top;
            this.pnlNotesTop.Height = 40;
            this.pnlNotesTop.Controls.Add(this.lblNotes);
            // 
            // lblNotes
            // 
            this.lblNotes.Dock = DockStyle.Fill;
            this.lblNotes.AutoSize = false;
            this.lblNotes.Location = new System.Drawing.Point(0, 0);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.TabIndex = 0;
            this.lblNotes.Text = "ملاحظات / سبب الرفض:";
            // التعديل هنا: استخدام BottomLeft لكي تظهر الكلمة في اليمين للمستخدم العربي (RTL Flip)
            this.lblNotes.TextAlign = ContentAlignment.BottomLeft;
            this.lblNotes.Padding = new Padding(0, 0, 20, 5);
            //
            // pnlNotesMiddle
            //
            this.pnlNotesMiddle.Dock = DockStyle.Fill;
            this.pnlNotesMiddle.Padding = new Padding(20, 0, 20, 10);
            this.pnlNotesMiddle.Controls.Add(this.txtNotes);
            // 
            // txtNotes
            // 
            this.txtNotes.Dock = DockStyle.Fill;
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.TabIndex = 1;
            //
            // pnlActionButtons
            //
            this.pnlActionButtons.Dock = DockStyle.Bottom;
            this.pnlActionButtons.Height = 80;
            this.pnlActionButtons.Controls.Add(this.btnReject);
            this.pnlActionButtons.Controls.Add(this.btnApproveAll);
            // 
            // btnApproveAll
            // 
            this.btnApproveAll.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            this.btnApproveAll.Location = new System.Drawing.Point(230, 10);
            this.btnApproveAll.Name = "btnApproveAll";
            this.btnApproveAll.Size = new System.Drawing.Size(260, 55);
            this.btnApproveAll.TabIndex = 2;
            this.btnApproveAll.Text = "موافقة وتحويل للصرف ✅";
            this.btnApproveAll.UseVisualStyleBackColor = false;
            this.btnApproveAll.Click += new System.EventHandler(this.btnApproveAll_Click);
            // 
            // btnReject
            // 
            this.btnReject.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            this.btnReject.Location = new System.Drawing.Point(20, 10);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(200, 55);
            this.btnReject.TabIndex = 4;
            this.btnReject.Text = "رفض الطلب ❌";
            this.btnReject.UseVisualStyleBackColor = false;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // frmManageRequests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.tblMain);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "frmManageRequests";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إدارة طلبات الفروع";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmManageRequests_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlTitleContainer.ResumeLayout(false);
            this.pnlRefreshContainer.ResumeLayout(false);
            this.tblMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).EndInit();
            this.pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
            this.pnlAction.ResumeLayout(false);
            this.pnlNotesTop.ResumeLayout(false);
            this.pnlNotesMiddle.ResumeLayout(false);
            this.pnlNotesMiddle.PerformLayout();
            this.pnlActionButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void ApplyCustomUIStyles()
        {
            this.Font = BranchThemeRequestsManage.UiFont(12F);
            this.BackColor = BranchThemeRequestsManage.Bg;

            pnlHeader.BackColor = BranchThemeRequestsManage.Header;
            pnlHeaderLine.BackColor = BranchThemeRequestsManage.Accent;

            lblTitle.Font = BranchThemeRequestsManage.UiFont(22F, FontStyle.Bold);
            lblTitle.ForeColor = BranchThemeRequestsManage.Text;

            lblSubtitle.Font = BranchThemeRequestsManage.UiFont(11.5F);
            lblSubtitle.ForeColor = BranchThemeRequestsManage.Muted;

            btnRefresh.BackColor = BranchThemeRequestsManage.Surface;
            btnRefresh.ForeColor = BranchThemeRequestsManage.Text;
            btnRefresh.Font = BranchThemeRequestsManage.UiFont(13F, FontStyle.Bold);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 1;
            btnRefresh.FlatAppearance.BorderColor = BranchThemeRequestsManage.Border;
            btnRefresh.Cursor = Cursors.Hand;

            tblMain.BackColor = BranchThemeRequestsManage.Bg;
            pnlRight.BackColor = BranchThemeRequestsManage.Bg;
            pnlDivider.BackColor = BranchThemeRequestsManage.Border;

            pnlAction.BackColor = BranchThemeRequestsManage.Surface;

            lblNotes.Font = BranchThemeRequestsManage.UiFont(13.5F, FontStyle.Bold);
            lblNotes.ForeColor = BranchThemeRequestsManage.Text;

            txtNotes.BackColor = BranchThemeRequestsManage.Input;
            txtNotes.ForeColor = BranchThemeRequestsManage.Text;
            txtNotes.Font = BranchThemeRequestsManage.UiFont(14F);
            txtNotes.BorderStyle = BorderStyle.FixedSingle;

            btnApproveAll.BackColor = BranchThemeRequestsManage.Green;
            btnApproveAll.ForeColor = Color.White;
            btnApproveAll.Font = BranchThemeRequestsManage.UiFont(14F, FontStyle.Bold);
            btnApproveAll.FlatStyle = FlatStyle.Flat;
            btnApproveAll.FlatAppearance.BorderSize = 0;
            btnApproveAll.Cursor = Cursors.Hand;

            btnReject.BackColor = BranchThemeRequestsManage.Red;
            btnReject.ForeColor = Color.White;
            btnReject.Font = BranchThemeRequestsManage.UiFont(14F, FontStyle.Bold);
            btnReject.FlatStyle = FlatStyle.Flat;
            btnReject.FlatAppearance.BorderSize = 0;
            btnReject.Cursor = Cursors.Hand;

            // Grid Styles
            StyleModernGrid(dgvRequests);
            StyleModernGrid(dgvDetails);

            dgvRequests.Columns.AddRange(new DataGridViewColumn[] { colReqSerial, colReqId, colReqLocation, colReqDate });

            colReqSerial.Name = "colReqSerial";
            colReqSerial.HeaderText = "م";
            colReqSerial.FillWeight = 25F;

            colReqId.Name = "colReqId";
            colReqId.HeaderText = "رقم الطلب";
            colReqId.Visible = false;

            colReqLocation.Name = "colReqLocation";
            colReqLocation.HeaderText = "الفرع الطالب";
            colReqLocation.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            colReqDate.Name = "colReqDate";
            colReqDate.HeaderText = "تاريخ وتوقت الطلب";
            colReqDate.FillWeight = 140F;

            dgvDetails.Columns.AddRange(new DataGridViewColumn[] { colDetSerial, colDetProductId, colDetProductName, colReqQty });

            colDetSerial.Name = "colDetSerial";
            colDetSerial.HeaderText = "م";
            colDetSerial.FillWeight = 20F;

            colDetProductId.Name = "colDetProductId";
            colDetProductId.HeaderText = "رقم المنتج";
            colDetProductId.Visible = false;

            colDetProductName.Name = "colDetProductName";
            colDetProductName.HeaderText = "المنتج المطلوب";
            colDetProductName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            colReqQty.Name = "colReqQty";
            colReqQty.HeaderText = "الكمية المطلوبة";
            colReqQty.FillWeight = 110F;
        }

        private void StyleModernGrid(DataGridView dgv)
        {
            dgv.BackgroundColor = BranchThemeRequestsManage.Bg;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.EnableHeadersVisualStyles = false;
            dgv.GridColor = BranchThemeRequestsManage.Border;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RightToLeft = RightToLeft.Yes;
            dgv.ReadOnly = true;

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            headerStyle.BackColor = BranchThemeRequestsManage.Header;
            headerStyle.ForeColor = BranchThemeRequestsManage.Muted;
            headerStyle.SelectionBackColor = BranchThemeRequestsManage.Header;
            headerStyle.SelectionForeColor = BranchThemeRequestsManage.Muted;
            headerStyle.Font = BranchThemeRequestsManage.UiFont(13F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle = headerStyle;
            dgv.ColumnHeadersHeight = 50;

            DataGridViewCellStyle defaultStyle = new DataGridViewCellStyle();
            defaultStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            defaultStyle.BackColor = BranchThemeRequestsManage.Surface;
            defaultStyle.ForeColor = BranchThemeRequestsManage.Text;
            defaultStyle.SelectionBackColor = Color.FromArgb(37, 99, 235);
            defaultStyle.SelectionForeColor = Color.White;
            defaultStyle.Font = BranchThemeRequestsManage.UiFont(12.5F, FontStyle.Bold);
            dgv.DefaultCellStyle = defaultStyle;

            DataGridViewCellStyle altStyle = new DataGridViewCellStyle();
            altStyle.BackColor = BranchThemeRequestsManage.Field;
            dgv.AlternatingRowsDefaultCellStyle = altStyle;

            dgv.RowTemplate.Height = 48;
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlHeaderLine;
        private System.Windows.Forms.Panel pnlTitleContainer;
        private System.Windows.Forms.Panel pnlRefreshContainer;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TableLayoutPanel tblMain;
        private System.Windows.Forms.DataGridView dgvRequests;
        private System.Windows.Forms.Panel pnlDivider;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlAction;
        private System.Windows.Forms.Panel pnlNotesTop;
        private System.Windows.Forms.Panel pnlNotesMiddle;
        private System.Windows.Forms.Panel pnlActionButtons;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Button btnApproveAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReqSerial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReqId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReqLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReqDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetSerial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetProductId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDetProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReqQty;

        #endregion
    }
}