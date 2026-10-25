namespace MajesticDesktop.Forms
{
    partial class frmCashRegister
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

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.tlpHeader = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tlpCards = new System.Windows.Forms.TableLayoutPanel();
            this.lblTableTitle = new System.Windows.Forms.Label();
            this.pnlGridHost = new System.Windows.Forms.Panel();
            this.dgvTransactions = new System.Windows.Forms.DataGridView();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.tlpFooter = new System.Windows.Forms.TableLayoutPanel();
            this.tlpNet = new System.Windows.Forms.TableLayoutPanel();
            this.lblNetCashTitle = new System.Windows.Forms.Label();
            this.lblNetCash = new System.Windows.Forms.Label();
            this.btnCloseDay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // frmCashRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1100, 720);
            this.Name = "frmCashRegister";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "يومية المخزن الرئيسي";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCashRegister_KeyDown);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.TableLayoutPanel tlpHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TableLayoutPanel tlpCards;
        private System.Windows.Forms.Label lblMainSales;
        private System.Windows.Forms.Label lblInstaKurdi;
        private System.Windows.Forms.Label lblInstaHakosh;
        private System.Windows.Forms.Label lblBranchReceipts;
        private System.Windows.Forms.Label lblReturns;
        private System.Windows.Forms.Label lblMainExpenses;
        private System.Windows.Forms.Label lblPurchases;
        private System.Windows.Forms.Label lblGrossProfit;
        private System.Windows.Forms.Label lblTableTitle;
        private System.Windows.Forms.Panel pnlGridHost;
        private System.Windows.Forms.DataGridView dgvTransactions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesc;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.TableLayoutPanel tlpFooter;
        private System.Windows.Forms.TableLayoutPanel tlpNet;
        private System.Windows.Forms.Label lblNetCashTitle;
        private System.Windows.Forms.Label lblNetCash;
        private System.Windows.Forms.Button btnCloseDay;
    }
}