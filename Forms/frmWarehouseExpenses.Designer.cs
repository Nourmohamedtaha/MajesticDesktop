namespace MajesticDesktop.Forms
{
    partial class frmWarehouseExpenses
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlGridCard = new System.Windows.Forms.Panel();
            this.dgvExpenses = new System.Windows.Forms.DataGridView();
            this.pnlFilterCard = new System.Windows.Forms.Panel();
            this.btnFilter = new System.Windows.Forms.Button();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFilterTitle = new System.Windows.Forms.Label();
            this.pnlInputCard = new System.Windows.Forms.Panel();
            this.btnSaveExpense = new System.Windows.Forms.Button();
            this.pnlNotes = new System.Windows.Forms.Panel();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.pnlPaymentSource = new System.Windows.Forms.Panel();
            this.cmbPaymentSource = new System.Windows.Forms.ComboBox();
            this.lblPaymentSource = new System.Windows.Forms.Label();
            this.pnlAmount = new System.Windows.Forms.Panel();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.lblAmount = new System.Windows.Forms.Label();
            this.pnlBorrower = new System.Windows.Forms.Panel();
            this.txtBorrowerName = new System.Windows.Forms.TextBox();
            this.lblBorrowerName = new System.Windows.Forms.Label();
            this.pnlExpenseCategory = new System.Windows.Forms.Panel();
            this.cmbExpenseType = new System.Windows.Forms.ComboBox();
            this.lblExpenseType = new System.Windows.Forms.Label();
            this.pnlOperationType = new System.Windows.Forms.Panel();
            this.rbBorrow = new System.Windows.Forms.RadioButton();
            this.rbExpense = new System.Windows.Forms.RadioButton();
            this.lblOperationType = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.lblTotal = new System.Windows.Forms.Label();

            this.pnlHeader.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlGridCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpenses)).BeginInit();
            this.pnlFilterCard.SuspendLayout();
            this.pnlInputCard.SuspendLayout();
            this.pnlNotes.SuspendLayout();
            this.pnlPaymentSource.SuspendLayout();
            this.pnlAmount.SuspendLayout();
            this.pnlBorrower.SuspendLayout();
            this.pnlExpenseCategory.SuspendLayout();
            this.pnlOperationType.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(20, 25, 35);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Size = new System.Drawing.Size(1200, 80);

            // lblTitle
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(240, 240, 245);
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Size = new System.Drawing.Size(1200, 80);
            this.lblTitle.Text = "💸 إدارة المصروفات والسلف - المخزن الرئيسي";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // pnlMain
            this.pnlMain.AutoScroll = true;
            this.pnlMain.Controls.Add(this.pnlGridCard);
            this.pnlMain.Controls.Add(this.pnlFilterCard);
            this.pnlMain.Controls.Add(this.pnlInputCard);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 80);
            this.pnlMain.Padding = new System.Windows.Forms.Padding(20);
            this.pnlMain.Size = new System.Drawing.Size(1200, 640);

            // pnlInputCard (تم تصميمه بالمسافات الهندسية الدقيقة لمنع التداخل)
            this.pnlInputCard.BackColor = System.Drawing.Color.FromArgb(30, 38, 50);
            this.pnlInputCard.Controls.Add(this.pnlOperationType);
            this.pnlInputCard.Controls.Add(this.pnlExpenseCategory);
            this.pnlInputCard.Controls.Add(this.pnlBorrower);
            this.pnlInputCard.Controls.Add(this.pnlAmount);
            this.pnlInputCard.Controls.Add(this.pnlPaymentSource);
            this.pnlInputCard.Controls.Add(this.pnlNotes);
            this.pnlInputCard.Controls.Add(this.btnSaveExpense);
            this.pnlInputCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInputCard.Location = new System.Drawing.Point(20, 20);
            this.pnlInputCard.Size = new System.Drawing.Size(1160, 200);

            // pnlOperationType
            this.pnlOperationType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.pnlOperationType.Controls.Add(this.rbBorrow);
            this.pnlOperationType.Controls.Add(this.rbExpense);
            this.pnlOperationType.Controls.Add(this.lblOperationType);
            this.pnlOperationType.Location = new System.Drawing.Point(920, 15);
            this.pnlOperationType.Size = new System.Drawing.Size(220, 80);

            this.lblOperationType.AutoSize = true;
            this.lblOperationType.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblOperationType.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblOperationType.Location = new System.Drawing.Point(125, 5);
            this.lblOperationType.Text = "نوع العملية:";

            this.rbExpense.AutoSize = true;
            this.rbExpense.Checked = true;
            this.rbExpense.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.rbExpense.ForeColor = System.Drawing.Color.White;
            this.rbExpense.Location = new System.Drawing.Point(125, 35);
            this.rbExpense.Text = "مصروف";
            this.rbExpense.CheckedChanged += new System.EventHandler(this.rbOperationType_CheckedChanged);

            this.rbBorrow.AutoSize = true;
            this.rbBorrow.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.rbBorrow.ForeColor = System.Drawing.Color.FromArgb(241, 196, 15);
            this.rbBorrow.Location = new System.Drawing.Point(30, 35);
            this.rbBorrow.Text = "سلفة";
            this.rbBorrow.CheckedChanged += new System.EventHandler(this.rbOperationType_CheckedChanged);

            // pnlExpenseCategory
            this.pnlExpenseCategory.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.pnlExpenseCategory.Controls.Add(this.cmbExpenseType);
            this.pnlExpenseCategory.Controls.Add(this.lblExpenseType);
            this.pnlExpenseCategory.Location = new System.Drawing.Point(650, 15);
            this.pnlExpenseCategory.Size = new System.Drawing.Size(260, 80);

            this.lblExpenseType.AutoSize = true;
            this.lblExpenseType.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblExpenseType.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblExpenseType.Location = new System.Drawing.Point(115, 5);
            this.lblExpenseType.Text = "تصنيف المصروف:";

            this.cmbExpenseType.BackColor = System.Drawing.Color.FromArgb(45, 55, 70);
            this.cmbExpenseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExpenseType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbExpenseType.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.cmbExpenseType.ForeColor = System.Drawing.Color.White;
            this.cmbExpenseType.Location = new System.Drawing.Point(10, 35);
            this.cmbExpenseType.Size = new System.Drawing.Size(240, 31);

            // pnlBorrower (يأخذ نفس مكان الـ Category عند التبديل)
            this.pnlBorrower.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.pnlBorrower.Controls.Add(this.txtBorrowerName);
            this.pnlBorrower.Controls.Add(this.lblBorrowerName);
            this.pnlBorrower.Location = new System.Drawing.Point(650, 15);
            this.pnlBorrower.Size = new System.Drawing.Size(260, 80);
            this.pnlBorrower.Visible = false;

            this.lblBorrowerName.AutoSize = true;
            this.lblBorrowerName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblBorrowerName.ForeColor = System.Drawing.Color.FromArgb(241, 196, 15);
            this.lblBorrowerName.Location = new System.Drawing.Point(145, 5);
            this.lblBorrowerName.Text = "اسم المستلف:";

            this.txtBorrowerName.BackColor = System.Drawing.Color.FromArgb(45, 55, 70);
            this.txtBorrowerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBorrowerName.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.txtBorrowerName.ForeColor = System.Drawing.Color.White;
            this.txtBorrowerName.Location = new System.Drawing.Point(10, 35);
            this.txtBorrowerName.Size = new System.Drawing.Size(240, 31);

            // pnlAmount
            this.pnlAmount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.pnlAmount.Controls.Add(this.txtAmount);
            this.pnlAmount.Controls.Add(this.lblAmount);
            this.pnlAmount.Location = new System.Drawing.Point(400, 15);
            this.pnlAmount.Size = new System.Drawing.Size(240, 80);

            this.lblAmount.AutoSize = true;
            this.lblAmount.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAmount.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblAmount.Location = new System.Drawing.Point(180, 5);
            this.lblAmount.Text = "المبلغ:";

            this.txtAmount.BackColor = System.Drawing.Color.FromArgb(45, 55, 70);
            this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAmount.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.txtAmount.ForeColor = System.Drawing.Color.White;
            this.txtAmount.Location = new System.Drawing.Point(10, 35);
            this.txtAmount.Size = new System.Drawing.Size(220, 32);
            this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAmount_KeyPress);

            // pnlPaymentSource
            this.pnlPaymentSource.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.pnlPaymentSource.Controls.Add(this.cmbPaymentSource);
            this.pnlPaymentSource.Controls.Add(this.lblPaymentSource);
            this.pnlPaymentSource.Location = new System.Drawing.Point(130, 15);
            this.pnlPaymentSource.Size = new System.Drawing.Size(260, 80);

            this.lblPaymentSource.AutoSize = true;
            this.lblPaymentSource.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblPaymentSource.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblPaymentSource.Location = new System.Drawing.Point(165, 5);
            this.lblPaymentSource.Text = "مصدر الدفع:";

            this.cmbPaymentSource.BackColor = System.Drawing.Color.FromArgb(45, 55, 70);
            this.cmbPaymentSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPaymentSource.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.cmbPaymentSource.ForeColor = System.Drawing.Color.White;
            this.cmbPaymentSource.Location = new System.Drawing.Point(10, 35);
            this.cmbPaymentSource.Size = new System.Drawing.Size(240, 31);

            // pnlNotes
            this.pnlNotes.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.pnlNotes.Controls.Add(this.txtNotes);
            this.pnlNotes.Controls.Add(this.lblNotes);
            this.pnlNotes.Location = new System.Drawing.Point(400, 105);
            this.pnlNotes.Size = new System.Drawing.Size(740, 80);

            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblNotes.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblNotes.Location = new System.Drawing.Point(590, 5);
            this.lblNotes.Text = "البيان / الملاحظات:";

            this.txtNotes.BackColor = System.Drawing.Color.FromArgb(45, 55, 70);
            this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.txtNotes.ForeColor = System.Drawing.Color.White;
            this.txtNotes.Location = new System.Drawing.Point(10, 35);
            this.txtNotes.Size = new System.Drawing.Size(720, 31);

            // btnSaveExpense
            this.btnSaveExpense.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnSaveExpense.BackColor = System.Drawing.Color.FromArgb(230, 60, 60);
            this.btnSaveExpense.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveExpense.FlatAppearance.BorderSize = 0;
            this.btnSaveExpense.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveExpense.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnSaveExpense.ForeColor = System.Drawing.Color.White;
            this.btnSaveExpense.Location = new System.Drawing.Point(140, 115);
            this.btnSaveExpense.Size = new System.Drawing.Size(250, 50);
            this.btnSaveExpense.Text = "تأكيد وخصم المبلغ ➖";
            this.btnSaveExpense.UseVisualStyleBackColor = false;
            this.btnSaveExpense.Click += new System.EventHandler(this.btnSaveExpense_Click);

            // pnlFilterCard
            this.pnlFilterCard.BackColor = System.Drawing.Color.FromArgb(24, 30, 40);
            this.pnlFilterCard.Controls.Add(this.btnFilter);
            this.pnlFilterCard.Controls.Add(this.lblTo);
            this.pnlFilterCard.Controls.Add(this.dtpTo);
            this.pnlFilterCard.Controls.Add(this.lblFrom);
            this.pnlFilterCard.Controls.Add(this.dtpFrom);
            this.pnlFilterCard.Controls.Add(this.lblFilterTitle);
            this.pnlFilterCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilterCard.Location = new System.Drawing.Point(20, 220);
            this.pnlFilterCard.Size = new System.Drawing.Size(1160, 80);

            this.lblFilterTitle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblFilterTitle.AutoSize = true;
            this.lblFilterTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblFilterTitle.ForeColor = System.Drawing.Color.FromArgb(200, 210, 220);
            this.lblFilterTitle.Location = new System.Drawing.Point(990, 28);
            this.lblFilterTitle.Text = "تصفية السجلات:";

            this.lblFrom.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblFrom.ForeColor = System.Drawing.Color.LightGray;
            this.lblFrom.Location = new System.Drawing.Point(820, 30);
            this.lblFrom.Text = "من:";

            this.dtpFrom.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(650, 26);
            this.dtpFrom.Size = new System.Drawing.Size(160, 29);

            this.lblTo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTo.ForeColor = System.Drawing.Color.LightGray;
            this.lblTo.Location = new System.Drawing.Point(590, 30);
            this.lblTo.Text = "إلى:";

            this.dtpTo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(420, 26);
            this.dtpTo.Size = new System.Drawing.Size(160, 29);

            this.btnFilter.BackColor = System.Drawing.Color.FromArgb(50, 130, 200);
            this.btnFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnFilter.ForeColor = System.Drawing.Color.White;
            this.btnFilter.Location = new System.Drawing.Point(20, 20);
            this.btnFilter.Size = new System.Drawing.Size(200, 40);
            this.btnFilter.Text = "عرض السجل 📅";
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);

            // pnlGridCard
            this.pnlGridCard.BackColor = System.Drawing.Color.FromArgb(30, 38, 50);
            this.pnlGridCard.Controls.Add(this.dgvExpenses);
            this.pnlGridCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGridCard.Location = new System.Drawing.Point(20, 300);
            this.pnlGridCard.Padding = new System.Windows.Forms.Padding(10);
            this.pnlGridCard.Size = new System.Drawing.Size(1160, 320);

            // dgvExpenses
            this.dgvExpenses.AllowUserToAddRows = false;
            this.dgvExpenses.AllowUserToDeleteRows = false;
            this.dgvExpenses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvExpenses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvExpenses.Location = new System.Drawing.Point(10, 10);
            this.dgvExpenses.ReadOnly = true;
            this.dgvExpenses.Size = new System.Drawing.Size(1140, 300);

            // pnlFooter
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(20, 25, 35);
            this.pnlFooter.Controls.Add(this.lblTotal);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 720);
            this.pnlFooter.Size = new System.Drawing.Size(1200, 80);

            // lblTotal
            this.lblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(46, 204, 113);
            this.lblTotal.Location = new System.Drawing.Point(0, 0);
            this.lblTotal.Size = new System.Drawing.Size(1200, 80);
            this.lblTotal.Text = "إجمالي العمليات للفترة المحددة: 0.00 ج.م";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // frmWarehouseExpenses
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(18, 22, 30);
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1100, 700);
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "مصروفات المخزن الرئيسي";
            this.Load += new System.EventHandler(this.frmWarehouseExpenses_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlGridCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpenses)).EndInit();
            this.pnlFilterCard.ResumeLayout(false);
            this.pnlFilterCard.PerformLayout();
            this.pnlInputCard.ResumeLayout(false);
            this.pnlNotes.ResumeLayout(false);
            this.pnlNotes.PerformLayout();
            this.pnlPaymentSource.ResumeLayout(false);
            this.pnlPaymentSource.PerformLayout();
            this.pnlAmount.ResumeLayout(false);
            this.pnlAmount.PerformLayout();
            this.pnlBorrower.ResumeLayout(false);
            this.pnlBorrower.PerformLayout();
            this.pnlExpenseCategory.ResumeLayout(false);
            this.pnlExpenseCategory.PerformLayout();
            this.pnlOperationType.ResumeLayout(false);
            this.pnlOperationType.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlInputCard;
        private System.Windows.Forms.Panel pnlOperationType;
        private System.Windows.Forms.Label lblOperationType;
        private System.Windows.Forms.RadioButton rbExpense;
        private System.Windows.Forms.RadioButton rbBorrow;
        private System.Windows.Forms.Panel pnlDynamicInput;
        private System.Windows.Forms.Panel pnlExpenseCategory;
        private System.Windows.Forms.Label lblExpenseType;
        private System.Windows.Forms.ComboBox cmbExpenseType;
        private System.Windows.Forms.Panel pnlBorrower;
        private System.Windows.Forms.Label lblBorrowerName;
        private System.Windows.Forms.TextBox txtBorrowerName;
        private System.Windows.Forms.Panel pnlAmount;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Panel pnlPaymentSource;
        private System.Windows.Forms.Label lblPaymentSource;
        private System.Windows.Forms.ComboBox cmbPaymentSource;
        private System.Windows.Forms.Panel pnlNotes;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Button btnSaveExpense;
        private System.Windows.Forms.Panel pnlFilterCard;
        private System.Windows.Forms.Label lblFilterTitle;
        private System.Windows.Forms.Panel pnlFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Panel pnlTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Panel pnlGridCard;
        private System.Windows.Forms.DataGridView dgvExpenses;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblTotal;
    }
}