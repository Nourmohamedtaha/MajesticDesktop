using System.Drawing;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    partial class uc_Dashboard
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
            this.components = new System.ComponentModel.Container();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);

            this.lblLastUpdate = new System.Windows.Forms.Label();

            this.tlpCards = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCardSales = new System.Windows.Forms.Panel();
            this.lblCardSalesTitle = new System.Windows.Forms.Label();
            this.lblSalesValue = new System.Windows.Forms.Label();
            this.pnlCardProfit = new System.Windows.Forms.Panel();
            this.lblCardProfitTitle = new System.Windows.Forms.Label();
            this.lblProfitValue = new System.Windows.Forms.Label();
            this.pnlCardExpenses = new System.Windows.Forms.Panel();
            this.lblCardExpensesTitle = new System.Windows.Forms.Label();
            this.lblExpensesValue = new System.Windows.Forms.Label();
            this.pnlCardProducts = new System.Windows.Forms.Panel();
            this.lblCardProductsTitle = new System.Windows.Forms.Label();
            this.lblProductsValue = new System.Windows.Forms.Label();
            this.pnlCardRequests = new System.Windows.Forms.Panel();
            this.lblCardRequestsTitle = new System.Windows.Forms.Label();
            this.lblRequestsValue = new System.Windows.Forms.Label();
            this.pnlCardAvgInvoice = new System.Windows.Forms.Panel();
            this.lblCardAvgInvoiceTitle = new System.Windows.Forms.Label();
            this.lblAvgInvoiceValue = new System.Windows.Forms.Label();

            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnNewSale = new System.Windows.Forms.Button();
            this.btnSupply = new System.Windows.Forms.Button();
            this.btnManageRequests = new System.Windows.Forms.Button();

            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();

            this.pnlShortagesContainer = new System.Windows.Forms.Panel();
            this.lblShortagesTitle = new System.Windows.Forms.Label();
            this.dgvShortages = new System.Windows.Forms.DataGridView();

            this.tlpRight = new System.Windows.Forms.TableLayoutPanel();
            this.pnlRequestsContainer = new System.Windows.Forms.Panel();
            this.lblRequestsTitle = new System.Windows.Forms.Label();
            this.dgvRequests = new System.Windows.Forms.DataGridView();
            this.pnlRecentSalesContainer = new System.Windows.Forms.Panel();
            this.lblRecentSalesTitle = new System.Windows.Forms.Label();
            this.dgvRecentSales = new System.Windows.Forms.DataGridView();

            this.tlpCards.SuspendLayout();
            this.pnlCardSales.SuspendLayout();
            this.pnlCardProfit.SuspendLayout();
            this.pnlCardExpenses.SuspendLayout();
            this.pnlCardProducts.SuspendLayout();
            this.pnlCardRequests.SuspendLayout();
            this.pnlCardAvgInvoice.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.tlpContent.SuspendLayout();
            this.pnlShortagesContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShortages)).BeginInit();
            this.tlpRight.SuspendLayout();
            this.pnlRequestsContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).BeginInit();
            this.pnlRecentSalesContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentSales)).BeginInit();
            this.SuspendLayout();

            // ============================ الكروت (3 × 2) ============================
            this.tlpCards.ColumnCount = 3;
            this.tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            this.tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            this.tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            this.tlpCards.RowCount = 2;
            this.tlpCards.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            this.tlpCards.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            this.tlpCards.Controls.Add(this.pnlCardSales, 0, 0);
            this.tlpCards.Controls.Add(this.pnlCardProfit, 1, 0);
            this.tlpCards.Controls.Add(this.pnlCardExpenses, 2, 0);
            this.tlpCards.Controls.Add(this.pnlCardProducts, 0, 1);
            this.tlpCards.Controls.Add(this.pnlCardRequests, 1, 1);
            this.tlpCards.Controls.Add(this.pnlCardAvgInvoice, 2, 1);
            this.tlpCards.Dock = DockStyle.Top;
            this.tlpCards.Location = new Point(0, 0);
            this.tlpCards.Name = "tlpCards";
            // هوامش أكبر من فوق عشان تعوض غياب الهيدر ويدي شكل مريح
            this.tlpCards.Padding = new Padding(16, 20, 16, 10);
            this.tlpCards.RightToLeft = RightToLeft.Yes;
            this.tlpCards.Size = new Size(1200, 280);

            // ============================ أزرار الإجراءات السريعة + آخر تحديث ============================
            this.pnlActions.BackColor = Color.Transparent;
            this.pnlActions.Controls.Add(this.lblLastUpdate);
            this.pnlActions.Controls.Add(this.btnNewSale);
            this.pnlActions.Controls.Add(this.btnSupply);
            this.pnlActions.Controls.Add(this.btnManageRequests);
            this.pnlActions.Dock = DockStyle.Top;
            this.pnlActions.Location = new Point(0, 280);
            this.pnlActions.Name = "pnlActions";
            // هوامش متوازنة فوق وتحت الزراير
            this.pnlActions.Padding = new Padding(16, 10, 16, 15);
            this.pnlActions.RightToLeft = RightToLeft.Yes;
            this.pnlActions.Size = new Size(1200, 75);

            this.lblLastUpdate.Dock = DockStyle.Left;
            this.lblLastUpdate.Font = new Font("Tahoma", 10.5F, FontStyle.Bold);
            this.lblLastUpdate.ForeColor = Color.FromArgb(148, 163, 184);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Size = new Size(260, 50);
            this.lblLastUpdate.Text = "آخر تحديث: --:--:--";
            this.lblLastUpdate.TextAlign = ContentAlignment.MiddleLeft;

            this.btnNewSale.BackColor = Color.FromArgb(22, 163, 74);
            this.btnNewSale.Cursor = Cursors.Hand;
            this.btnNewSale.Dock = DockStyle.Right;
            this.btnNewSale.FlatAppearance.BorderSize = 0;
            this.btnNewSale.FlatStyle = FlatStyle.Flat;
            this.btnNewSale.Font = new Font("Tahoma", 11.5F, FontStyle.Bold);
            this.btnNewSale.ForeColor = Color.White;
            this.btnNewSale.Margin = new Padding(8, 0, 0, 0);
            this.btnNewSale.Name = "btnNewSale";
            this.btnNewSale.Text = "🛒  فاتورة مبيعات";
            this.btnNewSale.Width = 200;
            this.btnNewSale.UseVisualStyleBackColor = false;
            this.btnNewSale.Click += new System.EventHandler(this.btnNewSale_Click);

            this.btnSupply.BackColor = Color.FromArgb(37, 99, 235);
            this.btnSupply.Cursor = Cursors.Hand;
            this.btnSupply.Dock = DockStyle.Right;
            this.btnSupply.FlatAppearance.BorderSize = 0;
            this.btnSupply.FlatStyle = FlatStyle.Flat;
            this.btnSupply.Font = new Font("Tahoma", 11.5F, FontStyle.Bold);
            this.btnSupply.ForeColor = Color.White;
            this.btnSupply.Margin = new Padding(8, 0, 0, 0);
            this.btnSupply.Name = "btnSupply";
            this.btnSupply.Text = "📥  توريد بضاعة";
            this.btnSupply.Width = 190;
            this.btnSupply.UseVisualStyleBackColor = false;
            this.btnSupply.Click += new System.EventHandler(this.btnSupply_Click);

            this.btnManageRequests.BackColor = Color.FromArgb(217, 119, 6);
            this.btnManageRequests.Cursor = Cursors.Hand;
            this.btnManageRequests.Dock = DockStyle.Right;
            this.btnManageRequests.FlatAppearance.BorderSize = 0;
            this.btnManageRequests.FlatStyle = FlatStyle.Flat;
            this.btnManageRequests.Font = new Font("Tahoma", 11.5F, FontStyle.Bold);
            this.btnManageRequests.ForeColor = Color.White;
            this.btnManageRequests.Margin = new Padding(8, 0, 0, 0);
            this.btnManageRequests.Name = "btnManageRequests";
            this.btnManageRequests.Text = "📋  إدارة الطلبات";
            this.btnManageRequests.Width = 190;
            this.btnManageRequests.UseVisualStyleBackColor = false;
            this.btnManageRequests.Click += new System.EventHandler(this.btnManageRequests_Click);

            // ============================ المحتوى الرئيسي ============================
            this.tlpContent.ColumnCount = 2;
            this.tlpContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            this.tlpContent.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            this.tlpContent.Controls.Add(this.pnlShortagesContainer, 0, 0);
            this.tlpContent.Controls.Add(this.tlpRight, 1, 0);
            this.tlpContent.Dock = DockStyle.Fill;
            this.tlpContent.Name = "tlpContent";
            this.tlpContent.Padding = new Padding(16, 5, 16, 16);
            this.tlpContent.RightToLeft = RightToLeft.Yes;
            this.tlpContent.RowCount = 1;
            this.tlpContent.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // ---- عمود الشمال: النواقص ----
            this.pnlShortagesContainer.BackColor = Color.FromArgb(30, 41, 59);
            this.pnlShortagesContainer.Controls.Add(this.dgvShortages);
            this.pnlShortagesContainer.Controls.Add(this.lblShortagesTitle);
            this.pnlShortagesContainer.Dock = DockStyle.Fill;
            this.pnlShortagesContainer.Margin = new Padding(0, 0, 8, 0);
            this.pnlShortagesContainer.Name = "pnlShortagesContainer";

            this.lblShortagesTitle.BackColor = Color.FromArgb(15, 23, 42);
            this.lblShortagesTitle.Dock = DockStyle.Top;
            this.lblShortagesTitle.Font = new Font("Tahoma", 12.5F, FontStyle.Bold);
            this.lblShortagesTitle.ForeColor = Color.FromArgb(250, 204, 21);
            this.lblShortagesTitle.Height = 44;
            this.lblShortagesTitle.Name = "lblShortagesTitle";
            this.lblShortagesTitle.Padding = new Padding(0, 0, 16, 0);
            this.lblShortagesTitle.Text = "⚠️  نواقص المخزون";
            this.lblShortagesTitle.TextAlign = ContentAlignment.MiddleRight;

            this.dgvShortages.Name = "dgvShortages";
            this.dgvShortages.CellFormatting += new DataGridViewCellFormattingEventHandler(this.dgvShortages_CellFormatting);

            // ---- عمود اليمين: الطلبات + أحدث الفواتير ----
            this.tlpRight.ColumnCount = 1;
            this.tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tlpRight.Controls.Add(this.pnlRequestsContainer, 0, 0);
            this.tlpRight.Controls.Add(this.pnlRecentSalesContainer, 0, 1);
            this.tlpRight.Dock = DockStyle.Fill;
            this.tlpRight.Margin = new Padding(8, 0, 0, 0);
            this.tlpRight.Name = "tlpRight";
            this.tlpRight.RowCount = 2;
            this.tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            this.tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));

            this.pnlRequestsContainer.BackColor = Color.FromArgb(30, 41, 59);
            this.pnlRequestsContainer.Controls.Add(this.dgvRequests);
            this.pnlRequestsContainer.Controls.Add(this.lblRequestsTitle);
            this.pnlRequestsContainer.Dock = DockStyle.Fill;
            this.pnlRequestsContainer.Margin = new Padding(0, 0, 0, 8);
            this.pnlRequestsContainer.Name = "pnlRequestsContainer";

            this.lblRequestsTitle.BackColor = Color.FromArgb(15, 23, 42);
            this.lblRequestsTitle.Dock = DockStyle.Top;
            this.lblRequestsTitle.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            this.lblRequestsTitle.ForeColor = Color.FromArgb(96, 165, 250);
            this.lblRequestsTitle.Height = 40;
            this.lblRequestsTitle.Name = "lblRequestsTitle";
            this.lblRequestsTitle.Padding = new Padding(0, 0, 16, 0);
            this.lblRequestsTitle.Text = "📥  أحدث طلبات الفروع";
            this.lblRequestsTitle.TextAlign = ContentAlignment.MiddleRight;

            this.dgvRequests.Name = "dgvRequests";

            this.pnlRecentSalesContainer.BackColor = Color.FromArgb(30, 41, 59);
            this.pnlRecentSalesContainer.Controls.Add(this.dgvRecentSales);
            this.pnlRecentSalesContainer.Controls.Add(this.lblRecentSalesTitle);
            this.pnlRecentSalesContainer.Dock = DockStyle.Fill;
            this.pnlRecentSalesContainer.Margin = new Padding(0, 8, 0, 0);
            this.pnlRecentSalesContainer.Name = "pnlRecentSalesContainer";

            this.lblRecentSalesTitle.BackColor = Color.FromArgb(15, 23, 42);
            this.lblRecentSalesTitle.Dock = DockStyle.Top;
            this.lblRecentSalesTitle.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            this.lblRecentSalesTitle.ForeColor = Color.FromArgb(74, 222, 128);
            this.lblRecentSalesTitle.Height = 40;
            this.lblRecentSalesTitle.Name = "lblRecentSalesTitle";
            this.lblRecentSalesTitle.Padding = new Padding(0, 0, 16, 0);
            this.lblRecentSalesTitle.Text = "🧾  أحدث فواتير اليوم";
            this.lblRecentSalesTitle.TextAlign = ContentAlignment.MiddleRight;

            this.dgvRecentSales.Name = "dgvRecentSales";

            // ============================ التايمر ============================
            this.refreshTimer.Interval = 20000;
            this.refreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);

            // ============================ الفورم ============================
            this.AutoScaleDimensions = new SizeF(7F, 14F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(18, 25, 38);
            this.ClientSize = new Size(1200, 700);
            this.Controls.Add(this.tlpContent);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.tlpCards);
            this.Font = new Font("Tahoma", 9F);
            this.Name = "uc_Dashboard";
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "uc_Dashboard";

            this.tlpCards.ResumeLayout(false);
            this.pnlCardSales.ResumeLayout(false);
            this.pnlCardProfit.ResumeLayout(false);
            this.pnlCardExpenses.ResumeLayout(false);
            this.pnlCardProducts.ResumeLayout(false);
            this.pnlCardRequests.ResumeLayout(false);
            this.pnlCardAvgInvoice.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.pnlShortagesContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvShortages)).EndInit();
            this.pnlRequestsContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequests)).EndInit();
            this.pnlRecentSalesContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentSales)).EndInit();
            this.tlpRight.ResumeLayout(false);
            this.tlpContent.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.Label lblLastUpdate;
        private System.Windows.Forms.TableLayoutPanel tlpCards;
        private System.Windows.Forms.Panel pnlCardSales;
        private System.Windows.Forms.Label lblCardSalesTitle;
        private System.Windows.Forms.Label lblSalesValue;
        private System.Windows.Forms.Panel pnlCardProfit;
        private System.Windows.Forms.Label lblCardProfitTitle;
        private System.Windows.Forms.Label lblProfitValue;
        private System.Windows.Forms.Panel pnlCardExpenses;
        private System.Windows.Forms.Label lblCardExpensesTitle;
        private System.Windows.Forms.Label lblExpensesValue;
        private System.Windows.Forms.Panel pnlCardProducts;
        private System.Windows.Forms.Label lblCardProductsTitle;
        private System.Windows.Forms.Label lblProductsValue;
        private System.Windows.Forms.Panel pnlCardRequests;
        private System.Windows.Forms.Label lblCardRequestsTitle;
        private System.Windows.Forms.Label lblRequestsValue;
        private System.Windows.Forms.Panel pnlCardAvgInvoice;
        private System.Windows.Forms.Label lblCardAvgInvoiceTitle;
        private System.Windows.Forms.Label lblAvgInvoiceValue;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnNewSale;
        private System.Windows.Forms.Button btnSupply;
        private System.Windows.Forms.Button btnManageRequests;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private System.Windows.Forms.Panel pnlShortagesContainer;
        private System.Windows.Forms.Label lblShortagesTitle;
        private System.Windows.Forms.DataGridView dgvShortages;
        private System.Windows.Forms.TableLayoutPanel tlpRight;
        private System.Windows.Forms.Panel pnlRequestsContainer;
        private System.Windows.Forms.Label lblRequestsTitle;
        private System.Windows.Forms.DataGridView dgvRequests;
        private System.Windows.Forms.Panel pnlRecentSalesContainer;
        private System.Windows.Forms.Label lblRecentSalesTitle;
        private System.Windows.Forms.DataGridView dgvRecentSales;
    }
}