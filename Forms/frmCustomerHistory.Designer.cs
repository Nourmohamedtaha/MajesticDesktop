using System.Drawing;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    partial class frmCustomerHistory
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlTop = new Panel();
            this.tlpTop = new TableLayoutPanel();
            this.pnlTitle = new Panel();
            this.lblTitle = new Label();
            this.lblSubtitle = new Label();
            this.btnClose = new Button();
            this.pnlLine = new Panel();
            this.pnlFooter = new Panel();
            this.lblTotals = new Label();
            this.dgvHistory = new DataGridView();

            this.pnlTop.SuspendLayout();
            this.tlpTop.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.SuspendLayout();

            // ===== الهيدر =====
            this.pnlTop.BackColor = Color.FromArgb(20, 30, 45);
            this.pnlTop.Dock = DockStyle.Top;
            this.pnlTop.Height = 86;
            this.pnlTop.Padding = new Padding(20, 10, 20, 10);
            this.pnlTop.Controls.Add(this.tlpTop);

            this.tlpTop.Dock = DockStyle.Fill;
            this.tlpTop.ColumnCount = 2;
            this.tlpTop.RowCount = 1;
            this.tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tlpTop.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            this.tlpTop.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tlpTop.Controls.Add(this.pnlTitle, 0, 0);
            this.tlpTop.Controls.Add(this.btnClose, 1, 0);

            this.pnlTitle.Dock = DockStyle.Fill;
            this.pnlTitle.Margin = new Padding(0);
            this.pnlTitle.Controls.Add(this.lblSubtitle);
            this.pnlTitle.Controls.Add(this.lblTitle);

            this.lblTitle.Dock = DockStyle.Top;
            this.lblTitle.Height = 40;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTitle.AutoEllipsis = true;

            this.lblSubtitle.Dock = DockStyle.Fill;
            this.lblSubtitle.Font = new Font("Segoe UI", 11F);
            this.lblSubtitle.ForeColor = Color.DarkGray;
            this.lblSubtitle.TextAlign = ContentAlignment.TopCenter;

            this.btnClose.Anchor = AnchorStyles.None;
            this.btnClose.Size = new Size(110, 44);
            this.btnClose.BackColor = Color.FromArgb(239, 68, 68);
            this.btnClose.ForeColor = Color.White;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.btnClose.Cursor = Cursors.Hand;
            this.btnClose.Text = "✖  إغلاق";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // ===== الخط الملون تحت الهيدر =====
            this.pnlLine.BackColor = Color.FromArgb(59, 130, 246);
            this.pnlLine.Dock = DockStyle.Top;
            this.pnlLine.Height = 3;

            // ===== الفوتر (الإجماليات) =====
            this.pnlFooter.BackColor = Color.FromArgb(30, 41, 59);
            this.pnlFooter.Dock = DockStyle.Bottom;
            this.pnlFooter.Height = 50;
            this.pnlFooter.Controls.Add(this.lblTotals);

            this.lblTotals.Dock = DockStyle.Fill;
            this.lblTotals.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblTotals.ForeColor = Color.WhiteSmoke;
            this.lblTotals.TextAlign = ContentAlignment.MiddleCenter;

            // ===== الجدول =====
            this.dgvHistory.Dock = DockStyle.Fill;
            this.dgvHistory.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvHistory_CellFormatting);

            // ===== الفورم =====
            this.BackColor = Color.FromArgb(15, 23, 42);
            this.ClientSize = new Size(960, 560);
            this.MinimumSize = new Size(760, 420);
            this.Controls.Add(this.dgvHistory);   // الـ Fill الأول
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlLine);
            this.Controls.Add(this.pnlTop);
            this.Font = new Font("Segoe UI", 10F);
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.CancelButton = this.btnClose;
            this.Text = "كشف حساب العميل";

            this.pnlTitle.ResumeLayout(false);
            this.tlpTop.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.ResumeLayout(false);
        }

        private Panel pnlTop;
        private TableLayoutPanel tlpTop;
        private Panel pnlTitle;
        private Label lblTitle;
        private Label lblSubtitle;
        private Button btnClose;
        private Panel pnlLine;
        private Panel pnlFooter;
        private Label lblTotals;
        private DataGridView dgvHistory;
    }
}