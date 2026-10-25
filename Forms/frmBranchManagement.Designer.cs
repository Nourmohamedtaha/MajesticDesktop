using System.Drawing;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    internal static class BranchTheme
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
        public static readonly Color Orange = Color.FromArgb(217, 119, 6);
        public static readonly Color Slate = Color.FromArgb(100, 116, 139);
        public static readonly Color Purple = Color.FromArgb(139, 92, 246);
        public static readonly Color Gold = Color.FromArgb(250, 204, 21);
        public static readonly Color GreenText = Color.FromArgb(74, 222, 128);
        public static readonly Color RedText = Color.FromArgb(248, 113, 113);
        public static readonly Color OrangeText = Color.FromArgb(251, 191, 36);

        public static Font UiFont(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font("Segoe UI", size, style);
        }

        public static Color Tint(Color c, int percent)
        {
            return Color.FromArgb(
                c.R + (255 - c.R) * percent / 100,
                c.G + (255 - c.G) * percent / 100,
                c.B + (255 - c.B) * percent / 100);
        }
    }

    partial class frmBranchManagement
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
            this.lblScreenTitle = new System.Windows.Forms.Label();
            this.pnlHeaderRight = new System.Windows.Forms.Panel();
            this.lblLocation = new System.Windows.Forms.Label();
            this.cmbLocations = new System.Windows.Forms.ComboBox();
            this.pnlHeaderLine = new System.Windows.Forms.Panel();
            this.pnlTabs = new System.Windows.Forms.Panel();
            this.tlpTabs = new System.Windows.Forms.TableLayoutPanel();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.pnlAddBranch = new System.Windows.Forms.Panel();
            this.dgvBranches = new System.Windows.Forms.DataGridView();
            this.pnlSpacerAdd = new System.Windows.Forms.Panel();
            this.pnlAddBranchTop = new System.Windows.Forms.TableLayoutPanel();
            this.pnlProducts = new System.Windows.Forms.Panel();
            this.dgvBranchProducts = new System.Windows.Forms.DataGridView();
            this.pnlProductsBottom = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSpacerProd = new System.Windows.Forms.Panel();
            this.pnlProductsTop = new System.Windows.Forms.TableLayoutPanel();
            this.pnlReturns = new System.Windows.Forms.Panel();
            this.pnlReturnHistoryContainer = new System.Windows.Forms.Panel();
            this.dgvReturnHistory = new System.Windows.Forms.DataGridView();
            this.lblReturnHistoryTitle = new System.Windows.Forms.Label();
            this.pnlNewReturnContainer = new System.Windows.Forms.Panel();
            this.dgvReturnItems = new System.Windows.Forms.DataGridView();
            this.pnlReturnsBottom = new System.Windows.Forms.TableLayoutPanel();
            this.pnlReturnsTop = new System.Windows.Forms.Panel();
            this.tlpReturnsRow2 = new System.Windows.Forms.TableLayoutPanel();
            this.tlpReturnsRow1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlExpenses = new System.Windows.Forms.Panel();
            this.dgvExpenses = new System.Windows.Forms.DataGridView();
            this.pnlExpensesBottom = new System.Windows.Forms.Panel();
            this.lblExpensesSummary = new System.Windows.Forms.Label();
            this.pnlSpacerExp = new System.Windows.Forms.Panel();
            this.pnlExpensesTop = new System.Windows.Forms.TableLayoutPanel();
            this.pnlCloseShift = new System.Windows.Forms.Panel();
            this.dgvDailyShift = new System.Windows.Forms.DataGridView();
            this.pnlSpacerDaily2 = new System.Windows.Forms.Panel();
            this.pnlDailyTools = new System.Windows.Forms.Panel();
            this.lblCashierDeficit = new System.Windows.Forms.Label();
            this.btnPayDeficit = new System.Windows.Forms.Button();
            this.tlpDailyAction = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSpacerDaily1 = new System.Windows.Forms.Panel();
            this.tlpDailyCards = new System.Windows.Forms.TableLayoutPanel();
            this.pnlMonthly = new System.Windows.Forms.Panel();
            this.dgvMonthlyShift = new System.Windows.Forms.DataGridView();
            this.pnlSpacerMonth2 = new System.Windows.Forms.Panel();
            this.tlpMonthlyAction = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSpacerMonth1 = new System.Windows.Forms.Panel();
            this.tlpMonthlyCards = new System.Windows.Forms.TableLayoutPanel();
            this.btnTabAddBranch = new System.Windows.Forms.Button();
            this.btnTabProducts = new System.Windows.Forms.Button();
            this.btnTabReturns = new System.Windows.Forms.Button();
            this.btnTabExpenses = new System.Windows.Forms.Button();
            this.btnTabClose = new System.Windows.Forms.Button();
            this.btnTabMonthly = new System.Windows.Forms.Button();
            this.colBranchSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBranchID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBranchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBranchType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBranchStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblBranchNameTitle = new System.Windows.Forms.Label();
            this.lblBranchTypeTitle = new System.Windows.Forms.Label();
            this.txtBranchName = new System.Windows.Forms.TextBox();
            this.cmbBranchType = new System.Windows.Forms.ComboBox();
            this.btnSaveNewBranch = new System.Windows.Forms.Button();
            this.btnEditBranch = new System.Windows.Forms.Button();
            this.btnClearBranch = new System.Windows.Forms.Button();
            this.colProdSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProdID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProdName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPurchasePrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblProdNameTitle = new System.Windows.Forms.Label();
            this.lblProdPriceTitle = new System.Windows.Forms.Label();
            this.lblProdQtyTitle = new System.Windows.Forms.Label();
            this.lblTotalInventoryValue = new System.Windows.Forms.Label();
            this.txtNewProdName = new System.Windows.Forms.TextBox();
            this.txtNewPurchasePrice = new System.Windows.Forms.TextBox();
            this.txtNewQty = new System.Windows.Forms.TextBox();
            this.btnAddBranchProduct = new System.Windows.Forms.Button();
            this.btnSaveBranchProducts = new System.Windows.Forms.Button();
            this.btnExportProducts = new System.Windows.Forms.Button();
            this.colRetSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetProdID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetProdName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetHistSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetHistDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetHistCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRetHistAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblRetCustomerTitle = new System.Windows.Forms.Label();
            this.lblRetProductTitle = new System.Windows.Forms.Label();
            this.lblRetQtyTitle = new System.Windows.Forms.Label();
            this.lblRetPriceTitle = new System.Windows.Forms.Label();
            this.cmbReturnCustomer = new System.Windows.Forms.ComboBox();
            this.cmbReturnProduct = new System.Windows.Forms.ComboBox();
            this.txtReturnQty = new System.Windows.Forms.TextBox();
            this.txtReturnPrice = new System.Windows.Forms.TextBox();
            this.chkDeductFromDebt = new System.Windows.Forms.CheckBox();
            this.btnAddReturnItem = new System.Windows.Forms.Button();
            this.btnRemoveReturnItem = new System.Windows.Forms.Button();
            this.btnSaveReturn = new System.Windows.Forms.Button();
            this.lblExpTypeTitle = new System.Windows.Forms.Label();
            this.lblExpAmountTitle = new System.Windows.Forms.Label();
            this.lblExpNoteTitle = new System.Windows.Forms.Label();
            this.colExpSerial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbExpenseType = new System.Windows.Forms.ComboBox();
            this.txtExpenseAmount = new System.Windows.Forms.TextBox();
            this.txtExpenseNotes = new System.Windows.Forms.TextBox();
            this.btnAddExpense = new System.Windows.Forms.Button();
            this.lblExpCash = new System.Windows.Forms.Label();
            this.lblActCash = new System.Windows.Forms.Label();
            this.lblDiff = new System.Windows.Forms.Label();
            this.txtExpectedCash = new System.Windows.Forms.TextBox();
            this.txtActualCash = new System.Windows.Forms.TextBox();
            this.txtShiftDifference = new System.Windows.Forms.TextBox();
            this.btnActionClose = new System.Windows.Forms.Button();
            this.lblCardProfitTitle = new System.Windows.Forms.Label();
            this.lblCardProfit = new System.Windows.Forms.Label();
            this.lblCardOwedTitle = new System.Windows.Forms.Label();
            this.lblCardOwedToUs = new System.Windows.Forms.Label();
            this.lblCardExpensesTitle = new System.Windows.Forms.Label();
            this.lblCardExpenses = new System.Windows.Forms.Label();
            this.lblCardAdvancesTitle = new System.Windows.Forms.Label();
            this.lblCardAdvances = new System.Windows.Forms.Label();
            this.lblCardReturnsTitle = new System.Windows.Forms.Label();
            this.lblCardReturns = new System.Windows.Forms.Label();
            this.lblCardInstaKurdiTitle = new System.Windows.Forms.Label();
            this.lblCardInstaKurdi = new System.Windows.Forms.Label();
            this.lblCardInstaHakoshTitle = new System.Windows.Forms.Label();
            this.lblCardInstaHakosh = new System.Windows.Forms.Label();
            this.lblCardSalesTitle = new System.Windows.Forms.Label();
            this.lblCardSales = new System.Windows.Forms.Label();
            this.lblCardTotalSalesTitle = new System.Windows.Forms.Label();
            this.lblCardTotalSales = new System.Windows.Forms.Label();
            this.lblMonthlyTitle = new System.Windows.Forms.Label();
            this.lblMonthlyBalanceCard = new System.Windows.Forms.Label();
            this.btnViewMonthlyArchive = new System.Windows.Forms.Button();
            this.btnActionMonthly = new System.Windows.Forms.Button();
            this.lblMProfitTitle = new System.Windows.Forms.Label();
            this.lblMonthlyProfit = new System.Windows.Forms.Label();
            this.lblMOwedTitle = new System.Windows.Forms.Label();
            this.lblMonthlyOwed = new System.Windows.Forms.Label();
            this.lblMExpTitle = new System.Windows.Forms.Label();
            this.lblMonthlyExpenses = new System.Windows.Forms.Label();
            this.lblMAdvancesTitle = new System.Windows.Forms.Label();
            this.lblMonthlyAdvances = new System.Windows.Forms.Label();
            this.lblMReturnsTitle = new System.Windows.Forms.Label();
            this.lblMonthlyReturns = new System.Windows.Forms.Label();
            this.lblMInstaKurdiTitle = new System.Windows.Forms.Label();
            this.lblMonthlyInstaKurdi = new System.Windows.Forms.Label();
            this.lblMInstaHakoshTitle = new System.Windows.Forms.Label();
            this.lblMonthlyInstaHakosh = new System.Windows.Forms.Label();
            this.lblMSalesTitle = new System.Windows.Forms.Label();
            this.lblMonthlySales = new System.Windows.Forms.Label();
            this.lblMTotalSalesTitle = new System.Windows.Forms.Label();
            this.lblMTotalSalesVal = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlHeaderRight.SuspendLayout();
            this.pnlTabs.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            this.pnlAddBranch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBranches)).BeginInit();
            this.pnlProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBranchProducts)).BeginInit();
            this.pnlReturns.SuspendLayout();
            this.pnlReturnHistoryContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturnHistory)).BeginInit();
            this.pnlNewReturnContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturnItems)).BeginInit();
            this.pnlReturnsTop.SuspendLayout();
            this.pnlExpenses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpenses)).BeginInit();
            this.pnlExpensesBottom.SuspendLayout();
            this.pnlCloseShift.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyShift)).BeginInit();
            this.pnlDailyTools.SuspendLayout();
            this.pnlMonthly.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonthlyShift)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(14)))), ((int)(((byte)(28)))));
            this.pnlHeader.Controls.Add(this.lblScreenTitle);
            this.pnlHeader.Controls.Add(this.pnlHeaderRight);
            this.pnlHeader.Controls.Add(this.pnlHeaderLine);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1200, 90);
            this.pnlHeader.TabIndex = 2;
            // 
            // lblScreenTitle
            // 
            this.lblScreenTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblScreenTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblScreenTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.lblScreenTitle.Location = new System.Drawing.Point(520, 0);
            this.lblScreenTitle.Name = "lblScreenTitle";
            this.lblScreenTitle.Padding = new System.Windows.Forms.Padding(0, 0, 24, 0);
            this.lblScreenTitle.Size = new System.Drawing.Size(680, 86);
            this.lblScreenTitle.TabIndex = 0;
            this.lblScreenTitle.Text = "إدارة الفروع والسيارات 🏢";
            this.lblScreenTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlHeaderRight
            // 
            this.pnlHeaderRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(14)))), ((int)(((byte)(28)))));
            this.pnlHeaderRight.Controls.Add(this.lblLocation);
            this.pnlHeaderRight.Controls.Add(this.cmbLocations);
            this.pnlHeaderRight.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlHeaderRight.Location = new System.Drawing.Point(0, 0);
            this.pnlHeaderRight.Name = "pnlHeaderRight";
            this.pnlHeaderRight.Size = new System.Drawing.Size(520, 86);
            this.pnlHeaderRight.TabIndex = 1;
            // 
            // lblLocation
            // 
            this.lblLocation.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.lblLocation.Location = new System.Drawing.Point(420, 26);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(80, 40);
            this.lblLocation.TabIndex = 0;
            this.lblLocation.Text = "الفرع:";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbLocations
            // 
            this.cmbLocations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cmbLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbLocations.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.cmbLocations.ForeColor = System.Drawing.Color.White;
            this.cmbLocations.Location = new System.Drawing.Point(20, 28);
            this.cmbLocations.Name = "cmbLocations";
            this.cmbLocations.Size = new System.Drawing.Size(390, 33);
            this.cmbLocations.TabIndex = 1;
            this.cmbLocations.SelectedIndexChanged += new System.EventHandler(this.cmbLocations_SelectedIndexChanged);
            // 
            // pnlHeaderLine
            // 
            this.pnlHeaderLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(130)))), ((int)(((byte)(246)))));
            this.pnlHeaderLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlHeaderLine.Location = new System.Drawing.Point(0, 86);
            this.pnlHeaderLine.Name = "pnlHeaderLine";
            this.pnlHeaderLine.Size = new System.Drawing.Size(1200, 4);
            this.pnlHeaderLine.TabIndex = 2;
            // 
            // pnlTabs
            // 
            this.pnlTabs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(14)))), ((int)(((byte)(28)))));
            this.pnlTabs.Controls.Add(this.tlpTabs);
            this.pnlTabs.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTabs.Location = new System.Drawing.Point(0, 90);
            this.pnlTabs.Name = "pnlTabs";
            this.pnlTabs.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.pnlTabs.Size = new System.Drawing.Size(1200, 60);
            this.pnlTabs.TabIndex = 1;
            // 
            // tlpTabs
            // 
            this.tlpTabs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTabs.Location = new System.Drawing.Point(16, 8);
            this.tlpTabs.Name = "tlpTabs";
            this.tlpTabs.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tlpTabs.Size = new System.Drawing.Size(1168, 44);
            this.tlpTabs.TabIndex = 0;
            // 
            // pnlContainer
            // 
            this.pnlContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlContainer.Controls.Add(this.pnlAddBranch);
            this.pnlContainer.Controls.Add(this.pnlProducts);
            this.pnlContainer.Controls.Add(this.pnlReturns);
            this.pnlContainer.Controls.Add(this.pnlExpenses);
            this.pnlContainer.Controls.Add(this.pnlCloseShift);
            this.pnlContainer.Controls.Add(this.pnlMonthly);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Location = new System.Drawing.Point(0, 150);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(20, 16, 20, 20);
            this.pnlContainer.Size = new System.Drawing.Size(1200, 618);
            this.pnlContainer.TabIndex = 0;
            // 
            // pnlAddBranch
            // 
            this.pnlAddBranch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlAddBranch.Controls.Add(this.dgvBranches);
            this.pnlAddBranch.Controls.Add(this.pnlSpacerAdd);
            this.pnlAddBranch.Controls.Add(this.pnlAddBranchTop);
            this.pnlAddBranch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAddBranch.Location = new System.Drawing.Point(20, 16);
            this.pnlAddBranch.Name = "pnlAddBranch";
            this.pnlAddBranch.Size = new System.Drawing.Size(1160, 582);
            this.pnlAddBranch.TabIndex = 0;
            // 
            // dgvBranches
            // 
            this.dgvBranches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBranches.Location = new System.Drawing.Point(0, 12);
            this.dgvBranches.Name = "dgvBranches";
            this.dgvBranches.Size = new System.Drawing.Size(1160, 570);
            this.dgvBranches.TabIndex = 0;
            this.dgvBranches.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBranches_CellClick);
            // 
            // pnlSpacerAdd
            // 
            this.pnlSpacerAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacerAdd.Location = new System.Drawing.Point(0, 0);
            this.pnlSpacerAdd.Name = "pnlSpacerAdd";
            this.pnlSpacerAdd.Size = new System.Drawing.Size(1160, 12);
            this.pnlSpacerAdd.TabIndex = 1;
            // 
            // pnlAddBranchTop
            // 
            this.pnlAddBranchTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlAddBranchTop.Location = new System.Drawing.Point(0, 0);
            this.pnlAddBranchTop.Name = "pnlAddBranchTop";
            this.pnlAddBranchTop.Size = new System.Drawing.Size(200, 100);
            this.pnlAddBranchTop.TabIndex = 2;
            // 
            // pnlProducts
            // 
            this.pnlProducts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlProducts.Controls.Add(this.dgvBranchProducts);
            this.pnlProducts.Controls.Add(this.pnlProductsBottom);
            this.pnlProducts.Controls.Add(this.pnlSpacerProd);
            this.pnlProducts.Controls.Add(this.pnlProductsTop);
            this.pnlProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlProducts.Location = new System.Drawing.Point(20, 16);
            this.pnlProducts.Name = "pnlProducts";
            this.pnlProducts.Size = new System.Drawing.Size(1160, 582);
            this.pnlProducts.TabIndex = 1;
            // 
            // dgvBranchProducts
            // 
            this.dgvBranchProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBranchProducts.Location = new System.Drawing.Point(0, 12);
            this.dgvBranchProducts.Name = "dgvBranchProducts";
            this.dgvBranchProducts.Size = new System.Drawing.Size(1160, 570);
            this.dgvBranchProducts.TabIndex = 0;
            // 
            // pnlProductsBottom
            // 
            this.pnlProductsBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlProductsBottom.Location = new System.Drawing.Point(0, 0);
            this.pnlProductsBottom.Name = "pnlProductsBottom";
            this.pnlProductsBottom.Size = new System.Drawing.Size(200, 100);
            this.pnlProductsBottom.TabIndex = 1;
            // 
            // pnlSpacerProd
            // 
            this.pnlSpacerProd.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacerProd.Location = new System.Drawing.Point(0, 0);
            this.pnlSpacerProd.Name = "pnlSpacerProd";
            this.pnlSpacerProd.Size = new System.Drawing.Size(1160, 12);
            this.pnlSpacerProd.TabIndex = 2;
            // 
            // pnlProductsTop
            // 
            this.pnlProductsTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlProductsTop.Location = new System.Drawing.Point(0, 0);
            this.pnlProductsTop.Name = "pnlProductsTop";
            this.pnlProductsTop.Size = new System.Drawing.Size(200, 100);
            this.pnlProductsTop.TabIndex = 3;
            // 
            // pnlReturns
            // 
            this.pnlReturns.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlReturns.Controls.Add(this.pnlReturnHistoryContainer);
            this.pnlReturns.Controls.Add(this.pnlNewReturnContainer);
            this.pnlReturns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlReturns.Location = new System.Drawing.Point(20, 16);
            this.pnlReturns.Name = "pnlReturns";
            this.pnlReturns.Size = new System.Drawing.Size(1160, 582);
            this.pnlReturns.TabIndex = 2;
            // 
            // pnlReturnHistoryContainer
            // 
            this.pnlReturnHistoryContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlReturnHistoryContainer.Controls.Add(this.dgvReturnHistory);
            this.pnlReturnHistoryContainer.Controls.Add(this.lblReturnHistoryTitle);
            this.pnlReturnHistoryContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlReturnHistoryContainer.Location = new System.Drawing.Point(0, 390);
            this.pnlReturnHistoryContainer.Name = "pnlReturnHistoryContainer";
            this.pnlReturnHistoryContainer.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.pnlReturnHistoryContainer.Size = new System.Drawing.Size(1160, 192);
            this.pnlReturnHistoryContainer.TabIndex = 0;
            // 
            // dgvReturnHistory
            // 
            this.dgvReturnHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReturnHistory.Location = new System.Drawing.Point(0, 58);
            this.dgvReturnHistory.Name = "dgvReturnHistory";
            this.dgvReturnHistory.Size = new System.Drawing.Size(1160, 134);
            this.dgvReturnHistory.TabIndex = 0;
            // 
            // lblReturnHistoryTitle
            // 
            this.lblReturnHistoryTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblReturnHistoryTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblReturnHistoryTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblReturnHistoryTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.lblReturnHistoryTitle.Location = new System.Drawing.Point(0, 12);
            this.lblReturnHistoryTitle.Name = "lblReturnHistoryTitle";
            this.lblReturnHistoryTitle.Size = new System.Drawing.Size(1160, 46);
            this.lblReturnHistoryTitle.TabIndex = 1;
            this.lblReturnHistoryTitle.Text = "سجل المرتجعات السابقة للفرع";
            this.lblReturnHistoryTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlNewReturnContainer
            // 
            this.pnlNewReturnContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlNewReturnContainer.Controls.Add(this.dgvReturnItems);
            this.pnlNewReturnContainer.Controls.Add(this.pnlReturnsBottom);
            this.pnlNewReturnContainer.Controls.Add(this.pnlReturnsTop);
            this.pnlNewReturnContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNewReturnContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlNewReturnContainer.Name = "pnlNewReturnContainer";
            this.pnlNewReturnContainer.Size = new System.Drawing.Size(1160, 390);
            this.pnlNewReturnContainer.TabIndex = 1;
            // 
            // dgvReturnItems
            // 
            this.dgvReturnItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReturnItems.Location = new System.Drawing.Point(0, 140);
            this.dgvReturnItems.Name = "dgvReturnItems";
            this.dgvReturnItems.Size = new System.Drawing.Size(1160, 250);
            this.dgvReturnItems.TabIndex = 0;
            // 
            // pnlReturnsBottom
            // 
            this.pnlReturnsBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlReturnsBottom.Location = new System.Drawing.Point(0, 0);
            this.pnlReturnsBottom.Name = "pnlReturnsBottom";
            this.pnlReturnsBottom.Size = new System.Drawing.Size(200, 100);
            this.pnlReturnsBottom.TabIndex = 1;
            // 
            // pnlReturnsTop
            // 
            this.pnlReturnsTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlReturnsTop.Controls.Add(this.tlpReturnsRow2);
            this.pnlReturnsTop.Controls.Add(this.tlpReturnsRow1);
            this.pnlReturnsTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlReturnsTop.Location = new System.Drawing.Point(0, 0);
            this.pnlReturnsTop.Name = "pnlReturnsTop";
            this.pnlReturnsTop.Padding = new System.Windows.Forms.Padding(16, 6, 16, 6);
            this.pnlReturnsTop.Size = new System.Drawing.Size(1160, 140);
            this.pnlReturnsTop.TabIndex = 2;
            // 
            // tlpReturnsRow2
            // 
            this.tlpReturnsRow2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpReturnsRow2.Location = new System.Drawing.Point(0, 0);
            this.tlpReturnsRow2.Name = "tlpReturnsRow2";
            this.tlpReturnsRow2.Size = new System.Drawing.Size(200, 100);
            this.tlpReturnsRow2.TabIndex = 0;
            // 
            // tlpReturnsRow1
            // 
            this.tlpReturnsRow1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpReturnsRow1.Location = new System.Drawing.Point(0, 0);
            this.tlpReturnsRow1.Name = "tlpReturnsRow1";
            this.tlpReturnsRow1.Size = new System.Drawing.Size(200, 100);
            this.tlpReturnsRow1.TabIndex = 1;
            // 
            // pnlExpenses
            // 
            this.pnlExpenses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlExpenses.Controls.Add(this.dgvExpenses);
            this.pnlExpenses.Controls.Add(this.pnlExpensesBottom);
            this.pnlExpenses.Controls.Add(this.pnlSpacerExp);
            this.pnlExpenses.Controls.Add(this.pnlExpensesTop);
            this.pnlExpenses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExpenses.Location = new System.Drawing.Point(20, 16);
            this.pnlExpenses.Name = "pnlExpenses";
            this.pnlExpenses.Size = new System.Drawing.Size(1160, 582);
            this.pnlExpenses.TabIndex = 3;
            // 
            // dgvExpenses
            // 
            this.dgvExpenses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvExpenses.Location = new System.Drawing.Point(0, 12);
            this.dgvExpenses.Name = "dgvExpenses";
            this.dgvExpenses.Size = new System.Drawing.Size(1160, 512);
            this.dgvExpenses.TabIndex = 0;
            // 
            // pnlExpensesBottom
            // 
            this.pnlExpensesBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlExpensesBottom.Controls.Add(this.lblExpensesSummary);
            this.pnlExpensesBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlExpensesBottom.Location = new System.Drawing.Point(0, 524);
            this.pnlExpensesBottom.Name = "pnlExpensesBottom";
            this.pnlExpensesBottom.Size = new System.Drawing.Size(1160, 58);
            this.pnlExpensesBottom.TabIndex = 1;
            // 
            // lblExpensesSummary
            // 
            this.lblExpensesSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExpensesSummary.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblExpensesSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(204)))), ((int)(((byte)(21)))));
            this.lblExpensesSummary.Location = new System.Drawing.Point(0, 0);
            this.lblExpensesSummary.Name = "lblExpensesSummary";
            this.lblExpensesSummary.Padding = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.lblExpensesSummary.Size = new System.Drawing.Size(1160, 58);
            this.lblExpensesSummary.TabIndex = 0;
            this.lblExpensesSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSpacerExp
            // 
            this.pnlSpacerExp.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacerExp.Location = new System.Drawing.Point(0, 0);
            this.pnlSpacerExp.Name = "pnlSpacerExp";
            this.pnlSpacerExp.Size = new System.Drawing.Size(1160, 12);
            this.pnlSpacerExp.TabIndex = 2;
            // 
            // pnlExpensesTop
            // 
            this.pnlExpensesTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.pnlExpensesTop.Location = new System.Drawing.Point(0, 0);
            this.pnlExpensesTop.Name = "pnlExpensesTop";
            this.pnlExpensesTop.Size = new System.Drawing.Size(200, 100);
            this.pnlExpensesTop.TabIndex = 3;
            // 
            // pnlCloseShift
            // 
            this.pnlCloseShift.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlCloseShift.Controls.Add(this.dgvDailyShift);
            this.pnlCloseShift.Controls.Add(this.pnlSpacerDaily2);
            this.pnlCloseShift.Controls.Add(this.pnlDailyTools);
            this.pnlCloseShift.Controls.Add(this.tlpDailyAction);
            this.pnlCloseShift.Controls.Add(this.pnlSpacerDaily1);
            this.pnlCloseShift.Controls.Add(this.tlpDailyCards);
            this.pnlCloseShift.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCloseShift.Location = new System.Drawing.Point(20, 16);
            this.pnlCloseShift.Name = "pnlCloseShift";
            this.pnlCloseShift.Size = new System.Drawing.Size(1160, 582);
            this.pnlCloseShift.TabIndex = 4;
            // 
            // dgvDailyShift
            // 
            this.dgvDailyShift.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDailyShift.Location = new System.Drawing.Point(0, 96);
            this.dgvDailyShift.Name = "dgvDailyShift";
            this.dgvDailyShift.Size = new System.Drawing.Size(1160, 486);
            this.dgvDailyShift.TabIndex = 0;
            // 
            // pnlSpacerDaily2
            // 
            this.pnlSpacerDaily2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacerDaily2.Location = new System.Drawing.Point(0, 84);
            this.pnlSpacerDaily2.Name = "pnlSpacerDaily2";
            this.pnlSpacerDaily2.Size = new System.Drawing.Size(1160, 12);
            this.pnlSpacerDaily2.TabIndex = 1;
            // 
            // pnlDailyTools
            // 
            this.pnlDailyTools.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.pnlDailyTools.Controls.Add(this.lblCashierDeficit);
            this.pnlDailyTools.Controls.Add(this.btnPayDeficit);
            this.pnlDailyTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDailyTools.Location = new System.Drawing.Point(0, 12);
            this.pnlDailyTools.Name = "pnlDailyTools";
            this.pnlDailyTools.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.pnlDailyTools.Size = new System.Drawing.Size(1160, 72);
            this.pnlDailyTools.TabIndex = 2;
            // 
            // lblCashierDeficit
            // 
            this.lblCashierDeficit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCashierDeficit.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblCashierDeficit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(222)))), ((int)(((byte)(128)))));
            this.lblCashierDeficit.Location = new System.Drawing.Point(272, 8);
            this.lblCashierDeficit.Name = "lblCashierDeficit";
            this.lblCashierDeficit.Padding = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.lblCashierDeficit.Size = new System.Drawing.Size(876, 56);
            this.lblCashierDeficit.TabIndex = 0;
            this.lblCashierDeficit.Text = "عجز الكاشير المستحق: 0.00 ج.م";
            this.lblCashierDeficit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPayDeficit
            // 
            this.btnPayDeficit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(119)))), ((int)(((byte)(6)))));
            this.btnPayDeficit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPayDeficit.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPayDeficit.FlatAppearance.BorderSize = 0;
            this.btnPayDeficit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPayDeficit.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnPayDeficit.ForeColor = System.Drawing.Color.White;
            this.btnPayDeficit.Location = new System.Drawing.Point(12, 8);
            this.btnPayDeficit.Name = "btnPayDeficit";
            this.btnPayDeficit.Size = new System.Drawing.Size(260, 56);
            this.btnPayDeficit.TabIndex = 1;
            this.btnPayDeficit.Text = "💵 سداد عجز كاشير";
            this.btnPayDeficit.UseVisualStyleBackColor = false;
            this.btnPayDeficit.Click += new System.EventHandler(this.btnPayDeficit_Click);
            // 
            // tlpDailyAction
            // 
            this.tlpDailyAction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpDailyAction.Location = new System.Drawing.Point(0, 0);
            this.tlpDailyAction.Name = "tlpDailyAction";
            this.tlpDailyAction.Size = new System.Drawing.Size(200, 100);
            this.tlpDailyAction.TabIndex = 3;
            // 
            // pnlSpacerDaily1
            // 
            this.pnlSpacerDaily1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacerDaily1.Location = new System.Drawing.Point(0, 0);
            this.pnlSpacerDaily1.Name = "pnlSpacerDaily1";
            this.pnlSpacerDaily1.Size = new System.Drawing.Size(1160, 12);
            this.pnlSpacerDaily1.TabIndex = 4;
            // 
            // tlpDailyCards
            // 
            this.tlpDailyCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpDailyCards.Location = new System.Drawing.Point(0, 0);
            this.tlpDailyCards.Name = "tlpDailyCards";
            this.tlpDailyCards.Size = new System.Drawing.Size(200, 100);
            this.tlpDailyCards.TabIndex = 5;
            // 
            // pnlMonthly
            // 
            this.pnlMonthly.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlMonthly.Controls.Add(this.dgvMonthlyShift);
            this.pnlMonthly.Controls.Add(this.pnlSpacerMonth2);
            this.pnlMonthly.Controls.Add(this.tlpMonthlyAction);
            this.pnlMonthly.Controls.Add(this.pnlSpacerMonth1);
            this.pnlMonthly.Controls.Add(this.tlpMonthlyCards);
            this.pnlMonthly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMonthly.Location = new System.Drawing.Point(20, 16);
            this.pnlMonthly.Name = "pnlMonthly";
            this.pnlMonthly.Size = new System.Drawing.Size(1160, 582);
            this.pnlMonthly.TabIndex = 5;
            // 
            // dgvMonthlyShift
            // 
            this.dgvMonthlyShift.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMonthlyShift.Location = new System.Drawing.Point(0, 24);
            this.dgvMonthlyShift.Name = "dgvMonthlyShift";
            this.dgvMonthlyShift.Size = new System.Drawing.Size(1160, 558);
            this.dgvMonthlyShift.TabIndex = 0;
            // 
            // pnlSpacerMonth2
            // 
            this.pnlSpacerMonth2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacerMonth2.Location = new System.Drawing.Point(0, 12);
            this.pnlSpacerMonth2.Name = "pnlSpacerMonth2";
            this.pnlSpacerMonth2.Size = new System.Drawing.Size(1160, 12);
            this.pnlSpacerMonth2.TabIndex = 1;
            // 
            // tlpMonthlyAction
            // 
            this.tlpMonthlyAction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMonthlyAction.Location = new System.Drawing.Point(0, 0);
            this.tlpMonthlyAction.Name = "tlpMonthlyAction";
            this.tlpMonthlyAction.Size = new System.Drawing.Size(200, 100);
            this.tlpMonthlyAction.TabIndex = 2;
            // 
            // pnlSpacerMonth1
            // 
            this.pnlSpacerMonth1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpacerMonth1.Location = new System.Drawing.Point(0, 0);
            this.pnlSpacerMonth1.Name = "pnlSpacerMonth1";
            this.pnlSpacerMonth1.Size = new System.Drawing.Size(1160, 12);
            this.pnlSpacerMonth1.TabIndex = 3;
            // 
            // tlpMonthlyCards
            // 
            this.tlpMonthlyCards.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMonthlyCards.Location = new System.Drawing.Point(0, 0);
            this.tlpMonthlyCards.Name = "tlpMonthlyCards";
            this.tlpMonthlyCards.Size = new System.Drawing.Size(200, 100);
            this.tlpMonthlyCards.TabIndex = 4;
            // 
            // btnTabAddBranch
            // 
            this.btnTabAddBranch.Location = new System.Drawing.Point(0, 0);
            this.btnTabAddBranch.Name = "btnTabAddBranch";
            this.btnTabAddBranch.Size = new System.Drawing.Size(75, 23);
            this.btnTabAddBranch.TabIndex = 0;
            this.btnTabAddBranch.Click += new System.EventHandler(this.btnTabAddBranch_Click);
            // 
            // btnTabProducts
            // 
            this.btnTabProducts.Location = new System.Drawing.Point(0, 0);
            this.btnTabProducts.Name = "btnTabProducts";
            this.btnTabProducts.Size = new System.Drawing.Size(75, 23);
            this.btnTabProducts.TabIndex = 0;
            this.btnTabProducts.Click += new System.EventHandler(this.btnTabProducts_Click);
            // 
            // btnTabReturns
            // 
            this.btnTabReturns.Location = new System.Drawing.Point(0, 0);
            this.btnTabReturns.Name = "btnTabReturns";
            this.btnTabReturns.Size = new System.Drawing.Size(75, 23);
            this.btnTabReturns.TabIndex = 0;
            this.btnTabReturns.Click += new System.EventHandler(this.btnTabReturns_Click);
            // 
            // btnTabExpenses
            // 
            this.btnTabExpenses.Location = new System.Drawing.Point(0, 0);
            this.btnTabExpenses.Name = "btnTabExpenses";
            this.btnTabExpenses.Size = new System.Drawing.Size(75, 23);
            this.btnTabExpenses.TabIndex = 0;
            this.btnTabExpenses.Click += new System.EventHandler(this.btnTabExpenses_Click);
            // 
            // btnTabClose
            // 
            this.btnTabClose.Location = new System.Drawing.Point(0, 0);
            this.btnTabClose.Name = "btnTabClose";
            this.btnTabClose.Size = new System.Drawing.Size(75, 23);
            this.btnTabClose.TabIndex = 0;
            this.btnTabClose.Click += new System.EventHandler(this.btnTabClose_Click);
            // 
            // btnTabMonthly
            // 
            this.btnTabMonthly.Location = new System.Drawing.Point(0, 0);
            this.btnTabMonthly.Name = "btnTabMonthly";
            this.btnTabMonthly.Size = new System.Drawing.Size(75, 23);
            this.btnTabMonthly.TabIndex = 0;
            this.btnTabMonthly.Click += new System.EventHandler(this.btnTabMonthly_Click);
            // 
            // colBranchSerial
            // 
            this.colBranchSerial.Name = "colBranchSerial";
            // 
            // colBranchID
            // 
            this.colBranchID.Name = "colBranchID";
            // 
            // colBranchName
            // 
            this.colBranchName.Name = "colBranchName";
            // 
            // colBranchType
            // 
            this.colBranchType.Name = "colBranchType";
            // 
            // colBranchStatus
            // 
            this.colBranchStatus.Name = "colBranchStatus";
            // 
            // lblBranchNameTitle
            // 
            this.lblBranchNameTitle.Location = new System.Drawing.Point(0, 0);
            this.lblBranchNameTitle.Name = "lblBranchNameTitle";
            this.lblBranchNameTitle.Size = new System.Drawing.Size(100, 23);
            this.lblBranchNameTitle.TabIndex = 0;
            // 
            // lblBranchTypeTitle
            // 
            this.lblBranchTypeTitle.Location = new System.Drawing.Point(0, 0);
            this.lblBranchTypeTitle.Name = "lblBranchTypeTitle";
            this.lblBranchTypeTitle.Size = new System.Drawing.Size(100, 23);
            this.lblBranchTypeTitle.TabIndex = 0;
            // 
            // txtBranchName
            // 
            this.txtBranchName.Location = new System.Drawing.Point(0, 0);
            this.txtBranchName.Name = "txtBranchName";
            this.txtBranchName.Size = new System.Drawing.Size(100, 20);
            this.txtBranchName.TabIndex = 0;
            // 
            // cmbBranchType
            // 
            this.cmbBranchType.Location = new System.Drawing.Point(0, 0);
            this.cmbBranchType.Name = "cmbBranchType";
            this.cmbBranchType.Size = new System.Drawing.Size(121, 21);
            this.cmbBranchType.TabIndex = 0;
            // 
            // btnSaveNewBranch
            // 
            this.btnSaveNewBranch.Location = new System.Drawing.Point(0, 0);
            this.btnSaveNewBranch.Name = "btnSaveNewBranch";
            this.btnSaveNewBranch.Size = new System.Drawing.Size(75, 23);
            this.btnSaveNewBranch.TabIndex = 0;
            this.btnSaveNewBranch.Click += new System.EventHandler(this.btnSaveNewBranch_Click);
            // 
            // btnEditBranch
            // 
            this.btnEditBranch.Location = new System.Drawing.Point(0, 0);
            this.btnEditBranch.Name = "btnEditBranch";
            this.btnEditBranch.Size = new System.Drawing.Size(75, 23);
            this.btnEditBranch.TabIndex = 0;
            this.btnEditBranch.Click += new System.EventHandler(this.btnEditBranch_Click);
            // 
            // btnClearBranch
            // 
            this.btnClearBranch.Location = new System.Drawing.Point(0, 0);
            this.btnClearBranch.Name = "btnClearBranch";
            this.btnClearBranch.Size = new System.Drawing.Size(75, 23);
            this.btnClearBranch.TabIndex = 0;
            this.btnClearBranch.Click += new System.EventHandler(this.btnClearBranch_Click);
            // 
            // colProdSerial
            // 
            this.colProdSerial.Name = "colProdSerial";
            // 
            // colProdID
            // 
            this.colProdID.Name = "colProdID";
            // 
            // colProdName
            // 
            this.colProdName.Name = "colProdName";
            // 
            // colPurchasePrice
            // 
            this.colPurchasePrice.Name = "colPurchasePrice";
            // 
            // colStockQty
            // 
            this.colStockQty.Name = "colStockQty";
            // 
            // lblProdNameTitle
            // 
            this.lblProdNameTitle.Location = new System.Drawing.Point(0, 0);
            this.lblProdNameTitle.Name = "lblProdNameTitle";
            this.lblProdNameTitle.Size = new System.Drawing.Size(100, 23);
            this.lblProdNameTitle.TabIndex = 0;
            // 
            // lblProdPriceTitle
            // 
            this.lblProdPriceTitle.Location = new System.Drawing.Point(0, 0);
            this.lblProdPriceTitle.Name = "lblProdPriceTitle";
            this.lblProdPriceTitle.Size = new System.Drawing.Size(100, 23);
            this.lblProdPriceTitle.TabIndex = 0;
            // 
            // lblProdQtyTitle
            // 
            this.lblProdQtyTitle.Location = new System.Drawing.Point(0, 0);
            this.lblProdQtyTitle.Name = "lblProdQtyTitle";
            this.lblProdQtyTitle.Size = new System.Drawing.Size(100, 23);
            this.lblProdQtyTitle.TabIndex = 0;
            // 
            // lblTotalInventoryValue
            // 
            this.lblTotalInventoryValue.Location = new System.Drawing.Point(0, 0);
            this.lblTotalInventoryValue.Name = "lblTotalInventoryValue";
            this.lblTotalInventoryValue.Size = new System.Drawing.Size(100, 23);
            this.lblTotalInventoryValue.TabIndex = 0;
            // 
            // txtNewProdName
            // 
            this.txtNewProdName.Location = new System.Drawing.Point(0, 0);
            this.txtNewProdName.Name = "txtNewProdName";
            this.txtNewProdName.Size = new System.Drawing.Size(100, 20);
            this.txtNewProdName.TabIndex = 0;
            // 
            // txtNewPurchasePrice
            // 
            this.txtNewPurchasePrice.Location = new System.Drawing.Point(0, 0);
            this.txtNewPurchasePrice.Name = "txtNewPurchasePrice";
            this.txtNewPurchasePrice.Size = new System.Drawing.Size(100, 20);
            this.txtNewPurchasePrice.TabIndex = 0;
            // 
            // txtNewQty
            // 
            this.txtNewQty.Location = new System.Drawing.Point(0, 0);
            this.txtNewQty.Name = "txtNewQty";
            this.txtNewQty.Size = new System.Drawing.Size(100, 20);
            this.txtNewQty.TabIndex = 0;
            // 
            // btnAddBranchProduct
            // 
            this.btnAddBranchProduct.Location = new System.Drawing.Point(0, 0);
            this.btnAddBranchProduct.Name = "btnAddBranchProduct";
            this.btnAddBranchProduct.Size = new System.Drawing.Size(75, 23);
            this.btnAddBranchProduct.TabIndex = 0;
            this.btnAddBranchProduct.Click += new System.EventHandler(this.btnAddBranchProduct_Click);
            // 
            // btnSaveBranchProducts
            // 
            this.btnSaveBranchProducts.Location = new System.Drawing.Point(0, 0);
            this.btnSaveBranchProducts.Name = "btnSaveBranchProducts";
            this.btnSaveBranchProducts.Size = new System.Drawing.Size(75, 23);
            this.btnSaveBranchProducts.TabIndex = 0;
            this.btnSaveBranchProducts.Click += new System.EventHandler(this.btnSaveBranchProducts_Click);
            // 
            // btnExportProducts
            // 
            this.btnExportProducts.Location = new System.Drawing.Point(0, 0);
            this.btnExportProducts.Name = "btnExportProducts";
            this.btnExportProducts.Size = new System.Drawing.Size(75, 23);
            this.btnExportProducts.TabIndex = 0;
            this.btnExportProducts.Click += new System.EventHandler(this.btnExportProducts_Click);
            // 
            // colRetSerial
            // 
            this.colRetSerial.Name = "colRetSerial";
            // 
            // colRetProdID
            // 
            this.colRetProdID.Name = "colRetProdID";
            // 
            // colRetProdName
            // 
            this.colRetProdName.Name = "colRetProdName";
            // 
            // colRetQty
            // 
            this.colRetQty.Name = "colRetQty";
            // 
            // colRetPrice
            // 
            this.colRetPrice.Name = "colRetPrice";
            // 
            // colRetHistSerial
            // 
            this.colRetHistSerial.Name = "colRetHistSerial";
            // 
            // colRetHistDate
            // 
            this.colRetHistDate.Name = "colRetHistDate";
            // 
            // colRetHistCustomer
            // 
            this.colRetHistCustomer.Name = "colRetHistCustomer";
            // 
            // colRetHistAmount
            // 
            this.colRetHistAmount.Name = "colRetHistAmount";
            // 
            // lblRetCustomerTitle
            // 
            this.lblRetCustomerTitle.Location = new System.Drawing.Point(0, 0);
            this.lblRetCustomerTitle.Name = "lblRetCustomerTitle";
            this.lblRetCustomerTitle.Size = new System.Drawing.Size(100, 23);
            this.lblRetCustomerTitle.TabIndex = 0;
            // 
            // lblRetProductTitle
            // 
            this.lblRetProductTitle.Location = new System.Drawing.Point(0, 0);
            this.lblRetProductTitle.Name = "lblRetProductTitle";
            this.lblRetProductTitle.Size = new System.Drawing.Size(100, 23);
            this.lblRetProductTitle.TabIndex = 0;
            // 
            // lblRetQtyTitle
            // 
            this.lblRetQtyTitle.Location = new System.Drawing.Point(0, 0);
            this.lblRetQtyTitle.Name = "lblRetQtyTitle";
            this.lblRetQtyTitle.Size = new System.Drawing.Size(100, 23);
            this.lblRetQtyTitle.TabIndex = 0;
            // 
            // lblRetPriceTitle
            // 
            this.lblRetPriceTitle.Location = new System.Drawing.Point(0, 0);
            this.lblRetPriceTitle.Name = "lblRetPriceTitle";
            this.lblRetPriceTitle.Size = new System.Drawing.Size(100, 23);
            this.lblRetPriceTitle.TabIndex = 0;
            // 
            // cmbReturnCustomer
            // 
            this.cmbReturnCustomer.Location = new System.Drawing.Point(0, 0);
            this.cmbReturnCustomer.Name = "cmbReturnCustomer";
            this.cmbReturnCustomer.Size = new System.Drawing.Size(121, 21);
            this.cmbReturnCustomer.TabIndex = 0;
            this.cmbReturnCustomer.SelectedIndexChanged += new System.EventHandler(this.cmbReturnCustomer_SelectedIndexChanged);
            // 
            // cmbReturnProduct
            // 
            this.cmbReturnProduct.Location = new System.Drawing.Point(0, 0);
            this.cmbReturnProduct.Name = "cmbReturnProduct";
            this.cmbReturnProduct.Size = new System.Drawing.Size(121, 21);
            this.cmbReturnProduct.TabIndex = 0;
            // 
            // txtReturnQty
            // 
            this.txtReturnQty.Location = new System.Drawing.Point(0, 0);
            this.txtReturnQty.Name = "txtReturnQty";
            this.txtReturnQty.Size = new System.Drawing.Size(100, 20);
            this.txtReturnQty.TabIndex = 0;
            // 
            // txtReturnPrice
            // 
            this.txtReturnPrice.Location = new System.Drawing.Point(0, 0);
            this.txtReturnPrice.Name = "txtReturnPrice";
            this.txtReturnPrice.Size = new System.Drawing.Size(100, 20);
            this.txtReturnPrice.TabIndex = 0;
            // 
            // chkDeductFromDebt
            // 
            this.chkDeductFromDebt.Location = new System.Drawing.Point(0, 0);
            this.chkDeductFromDebt.Name = "chkDeductFromDebt";
            this.chkDeductFromDebt.Size = new System.Drawing.Size(104, 24);
            this.chkDeductFromDebt.TabIndex = 0;
            this.chkDeductFromDebt.CheckedChanged += new System.EventHandler(this.chkDeductFromDebt_CheckedChanged);
            // 
            // btnAddReturnItem
            // 
            this.btnAddReturnItem.Location = new System.Drawing.Point(0, 0);
            this.btnAddReturnItem.Name = "btnAddReturnItem";
            this.btnAddReturnItem.Size = new System.Drawing.Size(75, 23);
            this.btnAddReturnItem.TabIndex = 0;
            this.btnAddReturnItem.Click += new System.EventHandler(this.btnAddReturnItem_Click);
            // 
            // btnRemoveReturnItem
            // 
            this.btnRemoveReturnItem.Location = new System.Drawing.Point(0, 0);
            this.btnRemoveReturnItem.Name = "btnRemoveReturnItem";
            this.btnRemoveReturnItem.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveReturnItem.TabIndex = 0;
            this.btnRemoveReturnItem.Click += new System.EventHandler(this.btnRemoveReturnItem_Click);
            // 
            // btnSaveReturn
            // 
            this.btnSaveReturn.Location = new System.Drawing.Point(0, 0);
            this.btnSaveReturn.Name = "btnSaveReturn";
            this.btnSaveReturn.Size = new System.Drawing.Size(75, 23);
            this.btnSaveReturn.TabIndex = 0;
            this.btnSaveReturn.Click += new System.EventHandler(this.btnSaveReturn_Click);
            // 
            // lblExpTypeTitle
            // 
            this.lblExpTypeTitle.Location = new System.Drawing.Point(0, 0);
            this.lblExpTypeTitle.Name = "lblExpTypeTitle";
            this.lblExpTypeTitle.Size = new System.Drawing.Size(100, 23);
            this.lblExpTypeTitle.TabIndex = 0;
            // 
            // lblExpAmountTitle
            // 
            this.lblExpAmountTitle.Location = new System.Drawing.Point(0, 0);
            this.lblExpAmountTitle.Name = "lblExpAmountTitle";
            this.lblExpAmountTitle.Size = new System.Drawing.Size(100, 23);
            this.lblExpAmountTitle.TabIndex = 0;
            // 
            // lblExpNoteTitle
            // 
            this.lblExpNoteTitle.Location = new System.Drawing.Point(0, 0);
            this.lblExpNoteTitle.Name = "lblExpNoteTitle";
            this.lblExpNoteTitle.Size = new System.Drawing.Size(100, 23);
            this.lblExpNoteTitle.TabIndex = 0;
            // 
            // colExpSerial
            // 
            this.colExpSerial.Name = "colExpSerial";
            // 
            // colExpDate
            // 
            this.colExpDate.Name = "colExpDate";
            // 
            // colExpType
            // 
            this.colExpType.Name = "colExpType";
            // 
            // colExpAmount
            // 
            this.colExpAmount.Name = "colExpAmount";
            // 
            // colExpNote
            // 
            this.colExpNote.Name = "colExpNote";
            // 
            // cmbExpenseType
            // 
            this.cmbExpenseType.Location = new System.Drawing.Point(0, 0);
            this.cmbExpenseType.Name = "cmbExpenseType";
            this.cmbExpenseType.Size = new System.Drawing.Size(121, 21);
            this.cmbExpenseType.TabIndex = 0;
            // 
            // txtExpenseAmount
            // 
            this.txtExpenseAmount.Location = new System.Drawing.Point(0, 0);
            this.txtExpenseAmount.Name = "txtExpenseAmount";
            this.txtExpenseAmount.Size = new System.Drawing.Size(100, 20);
            this.txtExpenseAmount.TabIndex = 0;
            // 
            // txtExpenseNotes
            // 
            this.txtExpenseNotes.Location = new System.Drawing.Point(0, 0);
            this.txtExpenseNotes.Name = "txtExpenseNotes";
            this.txtExpenseNotes.Size = new System.Drawing.Size(100, 20);
            this.txtExpenseNotes.TabIndex = 0;
            // 
            // btnAddExpense
            // 
            this.btnAddExpense.Location = new System.Drawing.Point(0, 0);
            this.btnAddExpense.Name = "btnAddExpense";
            this.btnAddExpense.Size = new System.Drawing.Size(75, 23);
            this.btnAddExpense.TabIndex = 0;
            this.btnAddExpense.Click += new System.EventHandler(this.btnAddExpense_Click);
            // 
            // lblExpCash
            // 
            this.lblExpCash.Location = new System.Drawing.Point(0, 0);
            this.lblExpCash.Name = "lblExpCash";
            this.lblExpCash.Size = new System.Drawing.Size(100, 23);
            this.lblExpCash.TabIndex = 0;
            // 
            // lblActCash
            // 
            this.lblActCash.Location = new System.Drawing.Point(0, 0);
            this.lblActCash.Name = "lblActCash";
            this.lblActCash.Size = new System.Drawing.Size(100, 23);
            this.lblActCash.TabIndex = 0;
            // 
            // lblDiff
            // 
            this.lblDiff.Location = new System.Drawing.Point(0, 0);
            this.lblDiff.Name = "lblDiff";
            this.lblDiff.Size = new System.Drawing.Size(100, 23);
            this.lblDiff.TabIndex = 0;
            // 
            // txtExpectedCash
            // 
            this.txtExpectedCash.Location = new System.Drawing.Point(0, 0);
            this.txtExpectedCash.Name = "txtExpectedCash";
            this.txtExpectedCash.Size = new System.Drawing.Size(100, 20);
            this.txtExpectedCash.TabIndex = 0;
            // 
            // txtActualCash
            // 
            this.txtActualCash.Location = new System.Drawing.Point(0, 0);
            this.txtActualCash.Name = "txtActualCash";
            this.txtActualCash.Size = new System.Drawing.Size(100, 20);
            this.txtActualCash.TabIndex = 0;
            this.txtActualCash.TextChanged += new System.EventHandler(this.txtActualCash_TextChanged);
            // 
            // txtShiftDifference
            // 
            this.txtShiftDifference.Location = new System.Drawing.Point(0, 0);
            this.txtShiftDifference.Name = "txtShiftDifference";
            this.txtShiftDifference.Size = new System.Drawing.Size(100, 20);
            this.txtShiftDifference.TabIndex = 0;
            // 
            // btnActionClose
            // 
            this.btnActionClose.Location = new System.Drawing.Point(0, 0);
            this.btnActionClose.Name = "btnActionClose";
            this.btnActionClose.Size = new System.Drawing.Size(75, 23);
            this.btnActionClose.TabIndex = 0;
            this.btnActionClose.Click += new System.EventHandler(this.btnActionClose_Click);
            // 
            // lblCardProfitTitle
            // 
            this.lblCardProfitTitle.Location = new System.Drawing.Point(0, 0);
            this.lblCardProfitTitle.Name = "lblCardProfitTitle";
            this.lblCardProfitTitle.Size = new System.Drawing.Size(100, 23);
            this.lblCardProfitTitle.TabIndex = 0;
            // 
            // lblCardProfit
            // 
            this.lblCardProfit.Location = new System.Drawing.Point(0, 0);
            this.lblCardProfit.Name = "lblCardProfit";
            this.lblCardProfit.Size = new System.Drawing.Size(100, 23);
            this.lblCardProfit.TabIndex = 0;
            // 
            // lblCardOwedTitle
            // 
            this.lblCardOwedTitle.Location = new System.Drawing.Point(0, 0);
            this.lblCardOwedTitle.Name = "lblCardOwedTitle";
            this.lblCardOwedTitle.Size = new System.Drawing.Size(100, 23);
            this.lblCardOwedTitle.TabIndex = 0;
            // 
            // lblCardOwedToUs
            // 
            this.lblCardOwedToUs.Location = new System.Drawing.Point(0, 0);
            this.lblCardOwedToUs.Name = "lblCardOwedToUs";
            this.lblCardOwedToUs.Size = new System.Drawing.Size(100, 23);
            this.lblCardOwedToUs.TabIndex = 0;
            // 
            // lblCardExpensesTitle
            // 
            this.lblCardExpensesTitle.Location = new System.Drawing.Point(0, 0);
            this.lblCardExpensesTitle.Name = "lblCardExpensesTitle";
            this.lblCardExpensesTitle.Size = new System.Drawing.Size(100, 23);
            this.lblCardExpensesTitle.TabIndex = 0;
            // 
            // lblCardExpenses
            // 
            this.lblCardExpenses.Location = new System.Drawing.Point(0, 0);
            this.lblCardExpenses.Name = "lblCardExpenses";
            this.lblCardExpenses.Size = new System.Drawing.Size(100, 23);
            this.lblCardExpenses.TabIndex = 0;
            // 
            // lblCardAdvancesTitle
            // 
            this.lblCardAdvancesTitle.Location = new System.Drawing.Point(0, 0);
            this.lblCardAdvancesTitle.Name = "lblCardAdvancesTitle";
            this.lblCardAdvancesTitle.Size = new System.Drawing.Size(100, 23);
            this.lblCardAdvancesTitle.TabIndex = 0;
            // 
            // lblCardAdvances
            // 
            this.lblCardAdvances.Location = new System.Drawing.Point(0, 0);
            this.lblCardAdvances.Name = "lblCardAdvances";
            this.lblCardAdvances.Size = new System.Drawing.Size(100, 23);
            this.lblCardAdvances.TabIndex = 0;
            // 
            // lblCardReturnsTitle
            // 
            this.lblCardReturnsTitle.Location = new System.Drawing.Point(0, 0);
            this.lblCardReturnsTitle.Name = "lblCardReturnsTitle";
            this.lblCardReturnsTitle.Size = new System.Drawing.Size(100, 23);
            this.lblCardReturnsTitle.TabIndex = 0;
            // 
            // lblCardReturns
            // 
            this.lblCardReturns.Location = new System.Drawing.Point(0, 0);
            this.lblCardReturns.Name = "lblCardReturns";
            this.lblCardReturns.Size = new System.Drawing.Size(100, 23);
            this.lblCardReturns.TabIndex = 0;
            // 
            // lblCardInstaKurdiTitle
            // 
            this.lblCardInstaKurdiTitle.Location = new System.Drawing.Point(0, 0);
            this.lblCardInstaKurdiTitle.Name = "lblCardInstaKurdiTitle";
            this.lblCardInstaKurdiTitle.Size = new System.Drawing.Size(100, 23);
            this.lblCardInstaKurdiTitle.TabIndex = 0;
            // 
            // lblCardInstaKurdi
            // 
            this.lblCardInstaKurdi.Location = new System.Drawing.Point(0, 0);
            this.lblCardInstaKurdi.Name = "lblCardInstaKurdi";
            this.lblCardInstaKurdi.Size = new System.Drawing.Size(100, 23);
            this.lblCardInstaKurdi.TabIndex = 0;
            // 
            // lblCardInstaHakoshTitle
            // 
            this.lblCardInstaHakoshTitle.Location = new System.Drawing.Point(0, 0);
            this.lblCardInstaHakoshTitle.Name = "lblCardInstaHakoshTitle";
            this.lblCardInstaHakoshTitle.Size = new System.Drawing.Size(100, 23);
            this.lblCardInstaHakoshTitle.TabIndex = 0;
            // 
            // lblCardInstaHakosh
            // 
            this.lblCardInstaHakosh.Location = new System.Drawing.Point(0, 0);
            this.lblCardInstaHakosh.Name = "lblCardInstaHakosh";
            this.lblCardInstaHakosh.Size = new System.Drawing.Size(100, 23);
            this.lblCardInstaHakosh.TabIndex = 0;
            // 
            // lblCardSalesTitle
            // 
            this.lblCardSalesTitle.Location = new System.Drawing.Point(0, 0);
            this.lblCardSalesTitle.Name = "lblCardSalesTitle";
            this.lblCardSalesTitle.Size = new System.Drawing.Size(100, 23);
            this.lblCardSalesTitle.TabIndex = 0;
            // 
            // lblCardSales
            // 
            this.lblCardSales.Location = new System.Drawing.Point(0, 0);
            this.lblCardSales.Name = "lblCardSales";
            this.lblCardSales.Size = new System.Drawing.Size(100, 23);
            this.lblCardSales.TabIndex = 0;
            // 
            // lblCardTotalSalesTitle
            // 
            this.lblCardTotalSalesTitle.Location = new System.Drawing.Point(0, 0);
            this.lblCardTotalSalesTitle.Name = "lblCardTotalSalesTitle";
            this.lblCardTotalSalesTitle.Size = new System.Drawing.Size(100, 23);
            this.lblCardTotalSalesTitle.TabIndex = 0;
            // 
            // lblCardTotalSales
            // 
            this.lblCardTotalSales.Location = new System.Drawing.Point(0, 0);
            this.lblCardTotalSales.Name = "lblCardTotalSales";
            this.lblCardTotalSales.Size = new System.Drawing.Size(100, 23);
            this.lblCardTotalSales.TabIndex = 0;
            // 
            // lblMonthlyTitle
            // 
            this.lblMonthlyTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMonthlyTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblMonthlyTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.lblMonthlyTitle.Location = new System.Drawing.Point(0, 0);
            this.lblMonthlyTitle.Name = "lblMonthlyTitle";
            this.lblMonthlyTitle.Size = new System.Drawing.Size(100, 23);
            this.lblMonthlyTitle.TabIndex = 0;
            this.lblMonthlyTitle.Text = "رصيد الكاش المتاح للترحيل:";
            this.lblMonthlyTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMonthlyBalanceCard
            // 
            this.lblMonthlyBalanceCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMonthlyBalanceCard.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            this.lblMonthlyBalanceCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(222)))), ((int)(((byte)(128)))));
            this.lblMonthlyBalanceCard.Location = new System.Drawing.Point(0, 0);
            this.lblMonthlyBalanceCard.Name = "lblMonthlyBalanceCard";
            this.lblMonthlyBalanceCard.Size = new System.Drawing.Size(100, 23);
            this.lblMonthlyBalanceCard.TabIndex = 0;
            this.lblMonthlyBalanceCard.Text = "0.00";
            this.lblMonthlyBalanceCard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnViewMonthlyArchive
            // 
            this.btnViewMonthlyArchive.Location = new System.Drawing.Point(0, 0);
            this.btnViewMonthlyArchive.Name = "btnViewMonthlyArchive";
            this.btnViewMonthlyArchive.Size = new System.Drawing.Size(75, 23);
            this.btnViewMonthlyArchive.TabIndex = 0;
            this.btnViewMonthlyArchive.Click += new System.EventHandler(this.btnViewMonthlyArchive_Click);
            // 
            // btnActionMonthly
            // 
            this.btnActionMonthly.Location = new System.Drawing.Point(0, 0);
            this.btnActionMonthly.Name = "btnActionMonthly";
            this.btnActionMonthly.Size = new System.Drawing.Size(75, 23);
            this.btnActionMonthly.TabIndex = 0;
            this.btnActionMonthly.Click += new System.EventHandler(this.btnActionMonthly_Click);
            // 
            // lblMProfitTitle
            // 
            this.lblMProfitTitle.Location = new System.Drawing.Point(0, 0);
            this.lblMProfitTitle.Name = "lblMProfitTitle";
            this.lblMProfitTitle.Size = new System.Drawing.Size(100, 23);
            this.lblMProfitTitle.TabIndex = 0;
            // 
            // lblMonthlyProfit
            // 
            this.lblMonthlyProfit.Location = new System.Drawing.Point(0, 0);
            this.lblMonthlyProfit.Name = "lblMonthlyProfit";
            this.lblMonthlyProfit.Size = new System.Drawing.Size(100, 23);
            this.lblMonthlyProfit.TabIndex = 0;
            // 
            // lblMOwedTitle
            // 
            this.lblMOwedTitle.Location = new System.Drawing.Point(0, 0);
            this.lblMOwedTitle.Name = "lblMOwedTitle";
            this.lblMOwedTitle.Size = new System.Drawing.Size(100, 23);
            this.lblMOwedTitle.TabIndex = 0;
            // 
            // lblMonthlyOwed
            // 
            this.lblMonthlyOwed.Location = new System.Drawing.Point(0, 0);
            this.lblMonthlyOwed.Name = "lblMonthlyOwed";
            this.lblMonthlyOwed.Size = new System.Drawing.Size(100, 23);
            this.lblMonthlyOwed.TabIndex = 0;
            // 
            // lblMExpTitle
            // 
            this.lblMExpTitle.Location = new System.Drawing.Point(0, 0);
            this.lblMExpTitle.Name = "lblMExpTitle";
            this.lblMExpTitle.Size = new System.Drawing.Size(100, 23);
            this.lblMExpTitle.TabIndex = 0;
            // 
            // lblMonthlyExpenses
            // 
            this.lblMonthlyExpenses.Location = new System.Drawing.Point(0, 0);
            this.lblMonthlyExpenses.Name = "lblMonthlyExpenses";
            this.lblMonthlyExpenses.Size = new System.Drawing.Size(100, 23);
            this.lblMonthlyExpenses.TabIndex = 0;
            // 
            // lblMAdvancesTitle
            // 
            this.lblMAdvancesTitle.Location = new System.Drawing.Point(0, 0);
            this.lblMAdvancesTitle.Name = "lblMAdvancesTitle";
            this.lblMAdvancesTitle.Size = new System.Drawing.Size(100, 23);
            this.lblMAdvancesTitle.TabIndex = 0;
            // 
            // lblMonthlyAdvances
            // 
            this.lblMonthlyAdvances.Location = new System.Drawing.Point(0, 0);
            this.lblMonthlyAdvances.Name = "lblMonthlyAdvances";
            this.lblMonthlyAdvances.Size = new System.Drawing.Size(100, 23);
            this.lblMonthlyAdvances.TabIndex = 0;
            // 
            // lblMReturnsTitle
            // 
            this.lblMReturnsTitle.Location = new System.Drawing.Point(0, 0);
            this.lblMReturnsTitle.Name = "lblMReturnsTitle";
            this.lblMReturnsTitle.Size = new System.Drawing.Size(100, 23);
            this.lblMReturnsTitle.TabIndex = 0;
            // 
            // lblMonthlyReturns
            // 
            this.lblMonthlyReturns.Location = new System.Drawing.Point(0, 0);
            this.lblMonthlyReturns.Name = "lblMonthlyReturns";
            this.lblMonthlyReturns.Size = new System.Drawing.Size(100, 23);
            this.lblMonthlyReturns.TabIndex = 0;
            // 
            // lblMInstaKurdiTitle
            // 
            this.lblMInstaKurdiTitle.Location = new System.Drawing.Point(0, 0);
            this.lblMInstaKurdiTitle.Name = "lblMInstaKurdiTitle";
            this.lblMInstaKurdiTitle.Size = new System.Drawing.Size(100, 23);
            this.lblMInstaKurdiTitle.TabIndex = 0;
            // 
            // lblMonthlyInstaKurdi
            // 
            this.lblMonthlyInstaKurdi.Location = new System.Drawing.Point(0, 0);
            this.lblMonthlyInstaKurdi.Name = "lblMonthlyInstaKurdi";
            this.lblMonthlyInstaKurdi.Size = new System.Drawing.Size(100, 23);
            this.lblMonthlyInstaKurdi.TabIndex = 0;
            // 
            // lblMInstaHakoshTitle
            // 
            this.lblMInstaHakoshTitle.Location = new System.Drawing.Point(0, 0);
            this.lblMInstaHakoshTitle.Name = "lblMInstaHakoshTitle";
            this.lblMInstaHakoshTitle.Size = new System.Drawing.Size(100, 23);
            this.lblMInstaHakoshTitle.TabIndex = 0;
            // 
            // lblMonthlyInstaHakosh
            // 
            this.lblMonthlyInstaHakosh.Location = new System.Drawing.Point(0, 0);
            this.lblMonthlyInstaHakosh.Name = "lblMonthlyInstaHakosh";
            this.lblMonthlyInstaHakosh.Size = new System.Drawing.Size(100, 23);
            this.lblMonthlyInstaHakosh.TabIndex = 0;
            // 
            // lblMSalesTitle
            // 
            this.lblMSalesTitle.Location = new System.Drawing.Point(0, 0);
            this.lblMSalesTitle.Name = "lblMSalesTitle";
            this.lblMSalesTitle.Size = new System.Drawing.Size(100, 23);
            this.lblMSalesTitle.TabIndex = 0;
            // 
            // lblMonthlySales
            // 
            this.lblMonthlySales.Location = new System.Drawing.Point(0, 0);
            this.lblMonthlySales.Name = "lblMonthlySales";
            this.lblMonthlySales.Size = new System.Drawing.Size(100, 23);
            this.lblMonthlySales.TabIndex = 0;
            // 
            // lblMTotalSalesTitle
            // 
            this.lblMTotalSalesTitle.Location = new System.Drawing.Point(0, 0);
            this.lblMTotalSalesTitle.Name = "lblMTotalSalesTitle";
            this.lblMTotalSalesTitle.Size = new System.Drawing.Size(100, 23);
            this.lblMTotalSalesTitle.TabIndex = 0;
            // 
            // lblMTotalSalesVal
            // 
            this.lblMTotalSalesVal.Location = new System.Drawing.Point(0, 0);
            this.lblMTotalSalesVal.Name = "lblMTotalSalesVal";
            this.lblMTotalSalesVal.Size = new System.Drawing.Size(100, 23);
            this.lblMTotalSalesVal.TabIndex = 0;
            // 
            // frmBranchManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(1200, 768);
            this.Controls.Add(this.pnlContainer);
            this.Controls.Add(this.pnlTabs);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.MinimumSize = new System.Drawing.Size(1100, 700);
            this.Name = "frmBranchManagement";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Text = "إدارة الفروع والسيارات";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmBranchManagement_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeaderRight.ResumeLayout(false);
            this.pnlTabs.ResumeLayout(false);
            this.pnlContainer.ResumeLayout(false);
            this.pnlAddBranch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBranches)).EndInit();
            this.pnlProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBranchProducts)).EndInit();
            this.pnlReturns.ResumeLayout(false);
            this.pnlReturnHistoryContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturnHistory)).EndInit();
            this.pnlNewReturnContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturnItems)).EndInit();
            this.pnlReturnsTop.ResumeLayout(false);
            this.pnlExpenses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExpenses)).EndInit();
            this.pnlExpensesBottom.ResumeLayout(false);
            this.pnlCloseShift.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDailyShift)).EndInit();
            this.pnlDailyTools.ResumeLayout(false);
            this.pnlMonthly.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonthlyShift)).EndInit();
            this.ResumeLayout(false);

        }

        private Panel pnlHeader, pnlHeaderRight, pnlHeaderLine, pnlTabs, pnlContainer;
        private Label lblScreenTitle, lblLocation;
        private ComboBox cmbLocations;
        private TableLayoutPanel tlpTabs;
        private Button btnTabAddBranch, btnTabProducts, btnTabReturns, btnTabExpenses, btnTabClose, btnTabMonthly;
        private Panel pnlSpacerAdd, pnlSpacerProd, pnlSpacerExp, pnlSpacerDaily1, pnlSpacerDaily2, pnlSpacerMonth1, pnlSpacerMonth2;

        private Panel pnlAddBranch;
        private TableLayoutPanel pnlAddBranchTop;
        private DataGridView dgvBranches;
        private DataGridViewTextBoxColumn colBranchSerial, colBranchID, colBranchName, colBranchType, colBranchStatus;
        private Label lblBranchNameTitle, lblBranchTypeTitle;
        private TextBox txtBranchName;
        private ComboBox cmbBranchType;
        private Button btnSaveNewBranch, btnEditBranch, btnClearBranch;

        private Panel pnlProducts;
        private TableLayoutPanel pnlProductsTop, pnlProductsBottom;
        private DataGridView dgvBranchProducts;
        private DataGridViewTextBoxColumn colProdSerial, colProdID, colProdName, colPurchasePrice, colStockQty;
        private Label lblProdNameTitle, lblProdPriceTitle, lblProdQtyTitle, lblTotalInventoryValue;
        private TextBox txtNewProdName, txtNewPurchasePrice, txtNewQty;
        private Button btnAddBranchProduct, btnSaveBranchProducts, btnExportProducts;

        private Panel pnlReturns, pnlNewReturnContainer, pnlReturnsTop, pnlReturnHistoryContainer;
        private TableLayoutPanel tlpReturnsRow1, tlpReturnsRow2, pnlReturnsBottom;
        private DataGridView dgvReturnItems, dgvReturnHistory;
        private DataGridViewTextBoxColumn colRetSerial, colRetProdID, colRetProdName, colRetQty, colRetPrice;
        private DataGridViewTextBoxColumn colRetHistSerial, colRetHistDate, colRetHistCustomer, colRetHistAmount;
        private Label lblRetCustomerTitle, lblRetProductTitle, lblRetQtyTitle, lblRetPriceTitle, lblReturnHistoryTitle;
        private ComboBox cmbReturnCustomer, cmbReturnProduct;
        private TextBox txtReturnQty, txtReturnPrice;
        private CheckBox chkDeductFromDebt;
        private Button btnAddReturnItem, btnRemoveReturnItem, btnSaveReturn;

        private Panel pnlExpenses, pnlExpensesBottom;
        private TableLayoutPanel pnlExpensesTop;
        private Label lblExpensesSummary, lblExpTypeTitle, lblExpAmountTitle, lblExpNoteTitle;
        private DataGridView dgvExpenses;
        private DataGridViewTextBoxColumn colExpSerial, colExpDate, colExpType, colExpAmount, colExpNote;
        private ComboBox cmbExpenseType;
        private TextBox txtExpenseAmount, txtExpenseNotes;
        private Button btnAddExpense;

        private Panel pnlCloseShift, pnlDailyTools;
        private DataGridView dgvDailyShift;
        private TableLayoutPanel tlpDailyAction, tlpDailyCards;
        private Label lblExpCash, lblActCash, lblDiff;
        private TextBox txtExpectedCash, txtActualCash, txtShiftDifference;
        private Button btnActionClose, btnPayDeficit;
        private Label lblCashierDeficit;
        private Label lblCardProfitTitle, lblCardProfit, lblCardOwedTitle, lblCardOwedToUs;
        private Label lblCardExpensesTitle, lblCardExpenses, lblCardAdvancesTitle, lblCardAdvances;
        private Label lblCardReturnsTitle, lblCardReturns, lblCardInstaKurdiTitle, lblCardInstaKurdi;
        private Label lblCardInstaHakoshTitle, lblCardInstaHakosh, lblCardSalesTitle, lblCardSales;
        private Label lblCardTotalSalesTitle, lblCardTotalSales;

        private Panel pnlMonthly;
        private DataGridView dgvMonthlyShift;
        private TableLayoutPanel tlpMonthlyAction, tlpMonthlyCards;
        private Label lblMonthlyTitle, lblMonthlyBalanceCard;
        private Button btnViewMonthlyArchive, btnActionMonthly;
        private Label lblMProfitTitle, lblMonthlyProfit, lblMOwedTitle, lblMonthlyOwed;
        private Label lblMExpTitle, lblMonthlyExpenses, lblMAdvancesTitle, lblMonthlyAdvances;
        private Label lblMReturnsTitle, lblMonthlyReturns, lblMInstaKurdiTitle, lblMonthlyInstaKurdi;
        private Label lblMInstaHakoshTitle, lblMonthlyInstaHakosh, lblMSalesTitle, lblMonthlySales;
        private Label lblMTotalSalesTitle, lblMTotalSalesVal;

        #endregion
    }
}