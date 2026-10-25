namespace MajesticDesktop.Forms
{
    partial class frmSalesInvoice
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
            this.SuspendLayout();
            // 
            // frmSalesInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "frmSalesInvoice";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "نظام المبيعات";
            this.Load += new System.EventHandler(this.frmSalesInvoice_Load);
            this.ResumeLayout(false);
        }

        // ==========================================
        // تعريف الأدوات (Variables Declaration)
        // ==========================================
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.ComboBox cmbCustomer;
        private System.Windows.Forms.Button btnAddCustomer;
        private System.Windows.Forms.TextBox txtInvoiceNo;
        private System.Windows.Forms.CheckBox chkManualDate;
        private System.Windows.Forms.DateTimePicker dtpInvoiceDate;

        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.TextBox txtAvailableQty;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.TextBox txtDiscount;
        private System.Windows.Forms.Button btnAddProduct;

        private System.Windows.Forms.DataGridView dgvDetails;

        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.TextBox txtNetAmount;
        private System.Windows.Forms.TextBox txtPaidAmount;
        private System.Windows.Forms.TextBox txtRemainingAmount;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;

        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.Button btnShare;
        private System.Windows.Forms.Button btnSave;
    }
}