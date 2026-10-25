using System.Drawing;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    partial class frmMainTreasury
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
            this.pnlHeader = new Panel();
            this.lblTitle = new Label();
            this.lblSubtitle = new Label();
            this.pnlSpacer1 = new Panel();
            this.pnlCards = new TableLayoutPanel();

            this.pnlCashCard = new Panel();
            this.lblCashTitle = new Label();
            this.lblCashVal = new Label();
            this.pnlInstaKurdiCard = new Panel();
            this.lblInstaKurdiTitle = new Label();
            this.lblInstaKurdiVal = new Label();
            this.pnlInstaHakoshCard = new Panel();
            this.lblInstaHakoshTitle = new Label();
            this.lblInstaHakoshVal = new Label();
            this.pnlProfitCard = new Panel();
            this.lblProfitTitle = new Label();
            this.lblProfitVal = new Label();
            this.pnlTotalSalesCard = new Panel();
            this.lblTotalSalesTitle = new Label();
            this.lblTotalSalesVal = new Label();
            this.pnlDebtsCard = new Panel();
            this.lblDebtsTitle = new Label();
            this.lblDebtsVal = new Label();
            this.pnlHQInCard = new Panel();
            this.lblHQInTitle = new Label();
            this.lblHQInVal = new Label();
            this.pnlHQOutCard = new Panel();
            this.lblHQOutTitle = new Label();
            this.lblHQOutVal = new Label();

            this.pnlSpacer2 = new Panel();
            this.pnlAction = new Panel();
            this.lblFrom = new Label();
            this.dtpFrom = new DateTimePicker();
            this.lblTo = new Label();
            this.dtpTo = new DateTimePicker();
            this.btnSearch = new Button();
            this.btnAddIn = new Button();
            this.btnAddOut = new Button();
            this.pnlSpacer3 = new Panel();
            this.dgvTransactions = new DataGridView();
            this.colId = new DataGridViewTextBoxColumn();
            this.colDate = new DataGridViewTextBoxColumn();
            this.colType = new DataGridViewTextBoxColumn();
            this.colAmount = new DataGridViewTextBoxColumn();
            this.colPaymentMethod = new DataGridViewTextBoxColumn();
            this.colDesc = new DataGridViewTextBoxColumn();
            this.colRef = new DataGridViewTextBoxColumn();
            this.pnlFooter = new Panel();
            this.tlpFooter = new TableLayoutPanel();
            this.lblNetTotalTitle = new Label();
            this.lblNetTotal = new Label();
            this.btnCloseMonth = new Button();

            this.pnlHeader.SuspendLayout();
            this.pnlCards.SuspendLayout();
            this.pnlCashCard.SuspendLayout();
            this.pnlInstaKurdiCard.SuspendLayout();
            this.pnlInstaHakoshCard.SuspendLayout();
            this.pnlProfitCard.SuspendLayout();
            this.pnlTotalSalesCard.SuspendLayout();
            this.pnlDebtsCard.SuspendLayout();
            this.pnlHQInCard.SuspendLayout();
            this.pnlHQOutCard.SuspendLayout();
            this.pnlAction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.tlpFooter.SuspendLayout();
            this.SuspendLayout();

            // ===== الهيدر: العنوان والوصف في نص الشاشة (Dock بدل إحداثيات ثابتة) =====
            this.pnlHeader.BackColor = Color.FromArgb(20, 30, 45);
            this.pnlHeader.Controls.Add(this.lblSubtitle);   // الـ Fill الأول
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Size = new Size(1200, 90);

            this.lblTitle.Dock = DockStyle.Top;
            this.lblTitle.Font = new Font("Tahoma", 22F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Height = 52;
            this.lblTitle.Text = "الخزينة المركزية (الإدارة)";
            this.lblTitle.TextAlign = ContentAlignment.BottomCenter;

            this.lblSubtitle.Dock = DockStyle.Fill;
            this.lblSubtitle.Font = new Font("Tahoma", 11F);
            this.lblSubtitle.ForeColor = Color.FromArgb(170, 190, 210);
            this.lblSubtitle.Text = "متابعة شاملة للمبيعات والأرصدة لجميع الفروع والمخزن والإدارة";
            this.lblSubtitle.TextAlign = ContentAlignment.TopCenter;

            this.pnlSpacer1.BackColor = Color.FromArgb(30, 40, 55);
            this.pnlSpacer1.Dock = DockStyle.Top;
            this.pnlSpacer1.Size = new Size(1200, 15);

            // ===== الكروت 2×4 =====
            this.pnlCards.BackColor = Color.FromArgb(30, 40, 55);
            this.pnlCards.ColumnCount = 4;
            for (int i = 0; i < 4; i++)
                this.pnlCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            this.pnlCards.Controls.Add(this.pnlCashCard, 0, 0);
            this.pnlCards.Controls.Add(this.pnlInstaKurdiCard, 1, 0);
            this.pnlCards.Controls.Add(this.pnlInstaHakoshCard, 2, 0);
            this.pnlCards.Controls.Add(this.pnlProfitCard, 3, 0);
            this.pnlCards.Controls.Add(this.pnlTotalSalesCard, 0, 1);
            this.pnlCards.Controls.Add(this.pnlDebtsCard, 1, 1);
            this.pnlCards.Controls.Add(this.pnlHQInCard, 2, 1);
            this.pnlCards.Controls.Add(this.pnlHQOutCard, 3, 1);

            this.pnlCards.Dock = DockStyle.Top;
            this.pnlCards.Padding = new Padding(15, 0, 15, 0);
            this.pnlCards.RowCount = 2;
            this.pnlCards.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            this.pnlCards.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            this.pnlCards.Size = new Size(1200, 240);

            SetupCard(this.pnlCashCard, this.lblCashTitle, this.lblCashVal, "الرصيد الحالي - كاش", Color.FromArgb(218, 165, 32), Color.Gold);
            SetupCard(this.pnlInstaKurdiCard, this.lblInstaKurdiTitle, this.lblInstaKurdiVal, "إنستاباي الكردي", Color.FromArgb(13, 148, 136), Color.FromArgb(94, 234, 212));
            SetupCard(this.pnlInstaHakoshCard, this.lblInstaHakoshTitle, this.lblInstaHakoshVal, "إنستاباي الحكش", Color.FromArgb(99, 102, 241), Color.FromArgb(165, 180, 252));
            SetupCard(this.pnlProfitCard, this.lblProfitTitle, this.lblProfitVal, "صافي الربح 🎯", Color.FromArgb(168, 85, 247), Color.FromArgb(216, 180, 254));
            SetupCard(this.pnlTotalSalesCard, this.lblTotalSalesTitle, this.lblTotalSalesVal, "إجمالي المبيعات (المخزن + الفروع)", Color.FromArgb(40, 167, 69), Color.FromArgb(100, 255, 150));
            SetupCard(this.pnlDebtsCard, this.lblDebtsTitle, this.lblDebtsVal, "إجمالي مديونيات العملاء", Color.FromArgb(253, 126, 20), Color.FromArgb(255, 200, 150));
            SetupCard(this.pnlHQInCard, this.lblHQInTitle, this.lblHQInVal, "إيرادات/تمويل خارجي", Color.FromArgb(32, 120, 100), Color.FromArgb(140, 220, 180));
            SetupCard(this.pnlHQOutCard, this.lblHQOutTitle, this.lblHQOutVal, "سحوبات ومصروفات الإدارة", Color.FromArgb(220, 53, 69), Color.FromArgb(255, 140, 140));

            this.pnlSpacer2.BackColor = Color.FromArgb(30, 40, 55);
            this.pnlSpacer2.Dock = DockStyle.Top;
            this.pnlSpacer2.Size = new Size(1200, 15);

            // ===== شريط الفلتر والأزرار (زي ما هو) =====
            this.pnlAction.BackColor = Color.FromArgb(35, 45, 60);
            this.pnlAction.BorderStyle = BorderStyle.FixedSingle;
            this.pnlAction.Controls.Add(this.lblFrom);
            this.pnlAction.Controls.Add(this.dtpFrom);
            this.pnlAction.Controls.Add(this.lblTo);
            this.pnlAction.Controls.Add(this.dtpTo);
            this.pnlAction.Controls.Add(this.btnSearch);
            this.pnlAction.Controls.Add(this.btnAddIn);
            this.pnlAction.Controls.Add(this.btnAddOut);
            this.pnlAction.Dock = DockStyle.Top;
            this.pnlAction.Size = new Size(1200, 70);

            this.lblFrom.Anchor = AnchorStyles.Right;
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            this.lblFrom.ForeColor = Color.WhiteSmoke;
            this.lblFrom.Location = new Point(1140, 25);
            this.lblFrom.Text = "من:";
            this.dtpFrom.Anchor = AnchorStyles.Right;
            this.dtpFrom.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            this.dtpFrom.Format = DateTimePickerFormat.Short;
            this.dtpFrom.Location = new Point(980, 21);
            this.dtpFrom.RightToLeft = RightToLeft.Yes;
            this.dtpFrom.RightToLeftLayout = true;
            this.dtpFrom.Size = new Size(150, 25);

            this.lblTo.Anchor = AnchorStyles.Right;
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            this.lblTo.ForeColor = Color.WhiteSmoke;
            this.lblTo.Location = new Point(920, 25);
            this.lblTo.Text = "إلى:";
            this.dtpTo.Anchor = AnchorStyles.Right;
            this.dtpTo.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            this.dtpTo.Format = DateTimePickerFormat.Short;
            this.dtpTo.Location = new Point(760, 21);
            this.dtpTo.RightToLeft = RightToLeft.Yes;
            this.dtpTo.RightToLeftLayout = true;
            this.dtpTo.Size = new Size(150, 25);

            this.btnSearch.Anchor = AnchorStyles.Right;
            this.btnSearch.BackColor = Color.FromArgb(35, 105, 175);
            this.btnSearch.Cursor = Cursors.Hand;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = FlatStyle.Flat;
            this.btnSearch.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            this.btnSearch.ForeColor = Color.White;
            this.btnSearch.Location = new Point(630, 18);
            this.btnSearch.Size = new Size(100, 33);
            this.btnSearch.Text = "بحث";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            this.btnAddOut.Anchor = AnchorStyles.Left;
            this.btnAddOut.BackColor = Color.FromArgb(190, 50, 60);
            this.btnAddOut.Cursor = Cursors.Hand;
            this.btnAddOut.FlatAppearance.BorderSize = 0;
            this.btnAddOut.FlatStyle = FlatStyle.Flat;
            this.btnAddOut.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            this.btnAddOut.ForeColor = Color.White;
            this.btnAddOut.Location = new Point(20, 15);
            this.btnAddOut.Size = new Size(190, 38);
            this.btnAddOut.Text = "- إذن صرف من إدارة";
            this.btnAddOut.UseVisualStyleBackColor = false;
            this.btnAddOut.Click += new System.EventHandler(this.btnAddOut_Click);

            this.btnAddIn.Anchor = AnchorStyles.Left;
            this.btnAddIn.BackColor = Color.FromArgb(35, 145, 90);
            this.btnAddIn.Cursor = Cursors.Hand;
            this.btnAddIn.FlatAppearance.BorderSize = 0;
            this.btnAddIn.FlatStyle = FlatStyle.Flat;
            this.btnAddIn.Font = new Font("Tahoma", 11F, FontStyle.Bold);
            this.btnAddIn.ForeColor = Color.White;
            this.btnAddIn.Location = new Point(230, 15);
            this.btnAddIn.Size = new Size(190, 38);
            this.btnAddIn.Text = "+ إذن قبض لإدارة";
            this.btnAddIn.UseVisualStyleBackColor = false;
            this.btnAddIn.Click += new System.EventHandler(this.btnAddIn_Click);

            this.pnlSpacer3.BackColor = Color.FromArgb(30, 40, 55);
            this.pnlSpacer3.Dock = DockStyle.Top;
            this.pnlSpacer3.Size = new Size(1200, 15);

            // ===== الجدول =====
            this.dgvTransactions.AllowUserToAddRows = false;
            this.dgvTransactions.AllowUserToDeleteRows = false;
            this.dgvTransactions.AllowUserToResizeRows = false;
            this.dgvTransactions.Dock = DockStyle.Fill;
            this.dgvTransactions.ReadOnly = true;
            this.dgvTransactions.RowHeadersVisible = false;
            this.dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvTransactions.Size = new Size(1200, 240);
            this.dgvTransactions.Columns.AddRange(new DataGridViewColumn[] {
                this.colId, this.colDate, this.colType, this.colAmount, this.colPaymentMethod, this.colDesc, this.colRef });

            this.colId.HeaderText = "م";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colDate.HeaderText = "تاريخ الحركة";
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            this.colType.HeaderText = "نوع الحركة";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colAmount.HeaderText = "المبلغ";
            this.colAmount.Name = "colAmount";
            this.colAmount.ReadOnly = true;
            this.colPaymentMethod.HeaderText = "طريقة الدفع";
            this.colPaymentMethod.Name = "colPaymentMethod";
            this.colPaymentMethod.ReadOnly = true;
            this.colDesc.HeaderText = "البيان / الوصف";
            this.colDesc.Name = "colDesc";
            this.colDesc.ReadOnly = true;
            this.colRef.HeaderText = "رقم مرجعي";
            this.colRef.Name = "colRef";
            this.colRef.ReadOnly = true;

            // ===== الفوتر =====
            this.pnlFooter.BackColor = Color.FromArgb(20, 30, 45);
            this.pnlFooter.BorderStyle = BorderStyle.FixedSingle;
            this.pnlFooter.Controls.Add(this.tlpFooter);
            this.pnlFooter.Dock = DockStyle.Bottom;
            this.pnlFooter.Size = new Size(1200, 100);

            this.tlpFooter.ColumnCount = 3;
            this.tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            this.tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 350F));
            this.tlpFooter.Controls.Add(this.lblNetTotalTitle, 0, 0);
            this.tlpFooter.Controls.Add(this.lblNetTotal, 1, 0);
            this.tlpFooter.Controls.Add(this.btnCloseMonth, 2, 0);
            this.tlpFooter.Dock = DockStyle.Fill;
            this.tlpFooter.RowCount = 1;
            this.tlpFooter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            this.lblNetTotalTitle.AutoSize = true;
            this.lblNetTotalTitle.Dock = DockStyle.Fill;
            this.lblNetTotalTitle.Font = new Font("Tahoma", 20F, FontStyle.Bold);
            this.lblNetTotalTitle.ForeColor = Color.White;
            this.lblNetTotalTitle.TextAlign = ContentAlignment.MiddleRight;
            this.lblNetTotalTitle.Text = "إجمالي الخزنة والأرصدة:";
            this.lblNetTotalTitle.Padding = new Padding(0, 0, 20, 0);

            this.lblNetTotal.Dock = DockStyle.Fill;
            this.lblNetTotal.Font = new Font("Tahoma", 30F, FontStyle.Bold);
            this.lblNetTotal.ForeColor = Color.Gold;
            this.lblNetTotal.TextAlign = ContentAlignment.MiddleLeft;
            this.lblNetTotal.Text = "0.00 ج.م";

            this.btnCloseMonth.BackColor = Color.FromArgb(220, 53, 69);
            this.btnCloseMonth.Cursor = Cursors.Hand;
            this.btnCloseMonth.Dock = DockStyle.Fill;
            this.btnCloseMonth.FlatAppearance.BorderSize = 0;
            this.btnCloseMonth.FlatStyle = FlatStyle.Flat;
            this.btnCloseMonth.Font = new Font("Tahoma", 16F, FontStyle.Bold);
            this.btnCloseMonth.ForeColor = Color.White;
            this.btnCloseMonth.Margin = new Padding(10, 15, 20, 15);
            this.btnCloseMonth.Text = "🔒 تصفية وأرشفة شاملة";
            this.btnCloseMonth.UseVisualStyleBackColor = false;
            this.btnCloseMonth.Click += new System.EventHandler(this.btnCloseMonth_Click);

            // ===== الفورم =====
            this.AutoScaleDimensions = new SizeF(7F, 14F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(30, 40, 55);
            this.ClientSize = new Size(1200, 800);
            this.MinimumSize = new Size(1100, 700);
            this.Controls.Add(this.dgvTransactions);
            this.Controls.Add(this.pnlSpacer3);
            this.Controls.Add(this.pnlAction);
            this.Controls.Add(this.pnlSpacer2);
            this.Controls.Add(this.pnlCards);
            this.Controls.Add(this.pnlSpacer1);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlFooter);
            this.Font = new Font("Tahoma", 9F);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.WindowState = FormWindowState.Maximized;
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "الخزينة المركزية";
            this.Load += new System.EventHandler(this.frmMainTreasury_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlCards.ResumeLayout(false);
            this.pnlCashCard.ResumeLayout(false);
            this.pnlInstaKurdiCard.ResumeLayout(false);
            this.pnlInstaHakoshCard.ResumeLayout(false);
            this.pnlProfitCard.ResumeLayout(false);
            this.pnlTotalSalesCard.ResumeLayout(false);
            this.pnlDebtsCard.ResumeLayout(false);
            this.pnlHQInCard.ResumeLayout(false);
            this.pnlHQOutCard.ResumeLayout(false);
            this.pnlAction.ResumeLayout(false);
            this.pnlAction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransactions)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.tlpFooter.ResumeLayout(false);
            this.tlpFooter.PerformLayout();
            this.ResumeLayout(false);
        }

        private void SetupCard(Panel card, Label title, Label value, string titleText, Color titleBack, Color valueColor)
        {
            card.BackColor = Color.FromArgb(35, 45, 60);
            card.BorderStyle = BorderStyle.FixedSingle;
            card.Controls.Add(value);   // الـ Fill الأول عشان الـ title (Top) ياخد مكانه فوق والقيمة تتوسط في الباقي
            card.Controls.Add(title);
            card.Dock = DockStyle.Fill;
            card.Margin = new Padding(10);

            title.BackColor = titleBack;
            title.Dock = DockStyle.Top;
            title.Font = new Font("Tahoma", 12F, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.Height = 35;
            title.Text = titleText;
            title.TextAlign = ContentAlignment.MiddleCenter;

            value.Dock = DockStyle.Fill;
            value.Font = new Font("Tahoma", 18F, FontStyle.Bold);
            value.ForeColor = valueColor;
            value.Text = "0.00";
            value.TextAlign = ContentAlignment.MiddleCenter;
        }

        private Panel pnlHeader;
        private Label lblTitle;
        private Label lblSubtitle;
        private Panel pnlSpacer1;
        private TableLayoutPanel pnlCards;
        private Panel pnlCashCard;
        private Label lblCashTitle;
        private Label lblCashVal;
        private Panel pnlInstaKurdiCard;
        private Label lblInstaKurdiTitle;
        private Label lblInstaKurdiVal;
        private Panel pnlInstaHakoshCard;
        private Label lblInstaHakoshTitle;
        private Label lblInstaHakoshVal;
        private Panel pnlProfitCard;
        private Label lblProfitTitle;
        private Label lblProfitVal;
        private Panel pnlTotalSalesCard;
        private Label lblTotalSalesTitle;
        private Label lblTotalSalesVal;
        private Panel pnlDebtsCard;
        private Label lblDebtsTitle;
        private Label lblDebtsVal;
        private Panel pnlHQInCard;
        private Label lblHQInTitle;
        private Label lblHQInVal;
        private Panel pnlHQOutCard;
        private Label lblHQOutTitle;
        private Label lblHQOutVal;
        private Panel pnlSpacer2;
        private Panel pnlAction;
        private Label lblFrom;
        private DateTimePicker dtpFrom;
        private Label lblTo;
        private DateTimePicker dtpTo;
        private Button btnSearch;
        private Button btnAddIn;
        private Button btnAddOut;
        private Panel pnlSpacer3;
        private DataGridView dgvTransactions;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn colType;
        private DataGridViewTextBoxColumn colAmount;
        private DataGridViewTextBoxColumn colPaymentMethod;
        private DataGridViewTextBoxColumn colDesc;
        private DataGridViewTextBoxColumn colRef;
        private Panel pnlFooter;
        private TableLayoutPanel tlpFooter;
        private Label lblNetTotalTitle;
        private Label lblNetTotal;
        private Button btnCloseMonth;
    }
}