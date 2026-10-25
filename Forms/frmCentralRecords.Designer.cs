namespace MajesticDesktop.Forms
{
    partial class frmCentralRecords
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
            this.pnlTopTabs = new System.Windows.Forms.Panel();
            this.tlpTabs = new System.Windows.Forms.TableLayoutPanel();
            this.btnTabPurchases = new System.Windows.Forms.Button();
            this.btnTabSales = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.pnlSpacer1 = new System.Windows.Forms.Panel();
            this.pnlFilter = new System.Windows.Forms.TableLayoutPanel();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblCustomerSearch = new System.Windows.Forms.Label();
            this.txtCustomerSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.pnlSpacer2 = new System.Windows.Forms.Panel();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRemaining = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaymentMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnViewInvoice = new System.Windows.Forms.Button();

            this.pnlTopTabs.SuspendLayout();
            this.tlpTabs.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // 
            // pnlTopTabs
            // 
            this.pnlTopTabs.Controls.Add(this.tlpTabs);
            this.pnlTopTabs.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopTabs.Location = new System.Drawing.Point(0, 0);
            this.pnlTopTabs.Name = "pnlTopTabs";
            this.pnlTopTabs.Size = new System.Drawing.Size(1250, 64);
            this.pnlTopTabs.TabIndex = 0;

            // 
            // tlpTabs
            // 
            this.tlpTabs.ColumnCount = 2;
            this.tlpTabs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTabs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTabs.Controls.Add(this.btnTabSales, 0, 0);
            this.tlpTabs.Controls.Add(this.btnTabPurchases, 1, 0);
            this.tlpTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTabs.Location = new System.Drawing.Point(0, 0);
            this.tlpTabs.Name = "tlpTabs";
            this.tlpTabs.RowCount = 1;
            this.tlpTabs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTabs.Size = new System.Drawing.Size(1250, 64);
            this.tlpTabs.TabIndex = 0;

            // 
            // btnTabPurchases
            // 
            this.btnTabPurchases.Location = new System.Drawing.Point(3, 3);
            this.btnTabPurchases.Name = "btnTabPurchases";
            this.btnTabPurchases.Size = new System.Drawing.Size(619, 58);
            this.btnTabPurchases.TabIndex = 1;
            this.btnTabPurchases.UseVisualStyleBackColor = true;
            this.btnTabPurchases.Click += new System.EventHandler(this.btnTabPurchases_Click);

            // 
            // btnTabSales
            // 
            this.btnTabSales.Location = new System.Drawing.Point(628, 3);
            this.btnTabSales.Name = "btnTabSales";
            this.btnTabSales.Size = new System.Drawing.Size(619, 58);
            this.btnTabSales.TabIndex = 0;
            this.btnTabSales.UseVisualStyleBackColor = true;
            this.btnTabSales.Click += new System.EventHandler(this.btnTabSales_Click);

            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 64);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1250, 76);
            this.pnlHeader.TabIndex = 1;

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(0, 15);
            this.lblTitle.TabIndex = 0;

            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Location = new System.Drawing.Point(0, 0);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(0, 15);
            this.lblSubtitle.TabIndex = 1;

            // 
            // pnlSpacer1
            // 
            this.pnlSpacer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacer1.Location = new System.Drawing.Point(0, 140);
            this.pnlSpacer1.Name = "pnlSpacer1";
            this.pnlSpacer1.Size = new System.Drawing.Size(1250, 12);
            this.pnlSpacer1.TabIndex = 2;

            // 
            // pnlFilter
            // 
            this.pnlFilter.ColumnCount = 9;
            this.pnlFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.pnlFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.pnlFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.pnlFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.pnlFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.pnlFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.pnlFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.pnlFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.pnlFilter.Controls.Add(this.lblLocation, 0, 0);
            this.pnlFilter.Controls.Add(this.cmbLocation, 1, 0);
            this.pnlFilter.Controls.Add(this.lblFrom, 2, 0);
            this.pnlFilter.Controls.Add(this.dtpFrom, 3, 0);
            this.pnlFilter.Controls.Add(this.lblTo, 4, 0);
            this.pnlFilter.Controls.Add(this.dtpTo, 5, 0);
            this.pnlFilter.Controls.Add(this.lblCustomerSearch, 6, 0);
            this.pnlFilter.Controls.Add(this.txtCustomerSearch, 7, 0);
            this.pnlFilter.Controls.Add(this.btnSearch, 8, 0);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 152);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.RowCount = 1;
            this.pnlFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlFilter.Size = new System.Drawing.Size(1250, 76);
            this.pnlFilter.TabIndex = 3;

            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(1198, 0);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(0, 15);
            this.lblLocation.TabIndex = 0;

            // 
            // cmbLocation
            // 
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(998, 3);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(194, 23);
            this.cmbLocation.TabIndex = 1;

            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(958, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(0, 15);
            this.lblFrom.TabIndex = 2;

            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(818, 3);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(134, 23);
            this.dtpFrom.TabIndex = 3;

            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(778, 0);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(0, 15);
            this.lblTo.TabIndex = 4;

            // 
            // dtpTo
            // 
            this.dtpTo.Location = new System.Drawing.Point(638, 3);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(134, 23);
            this.dtpTo.TabIndex = 5;

            // 
            // lblCustomerSearch
            // 
            this.lblCustomerSearch.AutoSize = true;
            this.lblCustomerSearch.Location = new System.Drawing.Point(448, 0);
            this.lblCustomerSearch.Name = "lblCustomerSearch";
            this.lblCustomerSearch.Size = new System.Drawing.Size(0, 15);
            this.lblCustomerSearch.TabIndex = 6;

            // 
            // txtCustomerSearch
            // 
            this.txtCustomerSearch.Location = new System.Drawing.Point(153, 3);
            this.txtCustomerSearch.Name = "txtCustomerSearch";
            this.txtCustomerSearch.Size = new System.Drawing.Size(289, 23);
            this.txtCustomerSearch.TabIndex = 7;

            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(144, 70);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            // 
            // pnlSpacer2
            // 
            this.pnlSpacer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacer2.Location = new System.Drawing.Point(0, 228);
            this.pnlSpacer2.Name = "pnlSpacer2";
            this.pnlSpacer2.Size = new System.Drawing.Size(1250, 12);
            this.pnlSpacer2.TabIndex = 4;

            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.dgvRecords);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 240);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(1250, 476);
            this.pnlGrid.TabIndex = 5;

            // 
            // dgvRecords
            // 
            this.dgvRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colInvoiceNo,
            this.colDate,
            this.colCustomer,
            this.colLocation,
            this.colTotal,
            this.colDiscount,
            this.colNet,
            this.colPaid,
            this.colRemaining,
            this.colPaymentMethod});
            this.dgvRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecords.Location = new System.Drawing.Point(0, 0);
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.Size = new System.Drawing.Size(1250, 476);
            this.dgvRecords.TabIndex = 0;
            this.dgvRecords.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRecords_CellDoubleClick);

            // 
            // colId
            // 
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";

            // 
            // colInvoiceNo
            // 
            this.colInvoiceNo.HeaderText = "InvoiceNo";
            this.colInvoiceNo.Name = "colInvoiceNo";

            // 
            // colDate
            // 
            this.colDate.HeaderText = "Date";
            this.colDate.Name = "colDate";

            // 
            // colCustomer
            // 
            this.colCustomer.HeaderText = "Customer";
            this.colCustomer.Name = "colCustomer";

            // 
            // colLocation
            // 
            this.colLocation.HeaderText = "Location";
            this.colLocation.Name = "colLocation";

            // 
            // colTotal
            // 
            this.colTotal.HeaderText = "Total";
            this.colTotal.Name = "colTotal";

            // 
            // colDiscount
            // 
            this.colDiscount.HeaderText = "Discount";
            this.colDiscount.Name = "colDiscount";

            // 
            // colNet
            // 
            this.colNet.HeaderText = "Net";
            this.colNet.Name = "colNet";

            // 
            // colPaid
            // 
            this.colPaid.HeaderText = "Paid";
            this.colPaid.Name = "colPaid";

            // 
            // colRemaining
            // 
            this.colRemaining.HeaderText = "Remaining";
            this.colRemaining.Name = "colRemaining";

            // 
            // colPaymentMethod
            // 
            this.colPaymentMethod.HeaderText = "PaymentMethod";
            this.colPaymentMethod.Name = "colPaymentMethod";

            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.btnViewInvoice);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 716);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1250, 84);
            this.pnlFooter.TabIndex = 6;

            // 
            // btnViewInvoice
            // 
            this.btnViewInvoice.Location = new System.Drawing.Point(0, 0);
            this.btnViewInvoice.Name = "btnViewInvoice";
            this.btnViewInvoice.Size = new System.Drawing.Size(1250, 84);
            this.btnViewInvoice.TabIndex = 0;
            this.btnViewInvoice.UseVisualStyleBackColor = true;
            this.btnViewInvoice.Click += new System.EventHandler(this.btnViewInvoice_Click);

            // 
            // frmCentralRecords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 800);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlSpacer2);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.pnlSpacer1);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlTopTabs);
            this.Controls.Add(this.pnlFooter);
            this.MinimumSize = new System.Drawing.Size(1150, 650);
            this.Name = "frmCentralRecords";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "السجلات المركزية للنظام";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmCentralRecords_Load);

            this.pnlTopTabs.ResumeLayout(false);
            this.tlpTabs.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlTopTabs;
        private System.Windows.Forms.TableLayoutPanel tlpTabs;
        private System.Windows.Forms.Button btnTabPurchases;
        private System.Windows.Forms.Button btnTabSales;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel pnlSpacer1;
        private System.Windows.Forms.TableLayoutPanel pnlFilter;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblCustomerSearch;
        private System.Windows.Forms.TextBox txtCustomerSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel pnlSpacer2;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvRecords;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRemaining;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentMethod;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnViewInvoice;
    }
}