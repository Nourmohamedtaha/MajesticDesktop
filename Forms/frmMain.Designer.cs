namespace MajesticDesktop.Forms
{
    partial class frmMain
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
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.productsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.الاقسامToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.المنتجاتToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.فاتورة_مشترياتToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.الموردينToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.branchesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.لوحة_تحكم_الفرعToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.طلبات_الفروعToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.إدارة_الطلباتToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.صرف_بضاعةToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.فاتورة_البيعToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.مرتجع_مبيعاتToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.العملاءToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.السجلات_المركزيةToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.يومية_الخزنةToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.مصروفات_المخزنToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.خزينة_الإدارةToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.التقارير_الشاملةToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.المستخدمين_والصلاحياتToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.إعدادات_النظامToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlBranding = new System.Windows.Forms.Panel();
            this.pnlDateInfo = new System.Windows.Forms.Panel();
            this.lblDay = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.pnlCalculator = new System.Windows.Forms.Panel();
            this.picCalculator = new System.Windows.Forms.PictureBox();
            this.lblCalculator = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.pnlMainContent = new System.Windows.Forms.Panel();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.mainMenuStrip.SuspendLayout();
            this.pnlBranding.SuspendLayout();
            this.pnlDateInfo.SuspendLayout();
            this.pnlCalculator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCalculator)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.AutoSize = false;
            this.mainMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(45)))), ((int)(((byte)(78)))));
            this.mainMenuStrip.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.mainMenuStrip.ForeColor = System.Drawing.Color.White;
            this.mainMenuStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.productsMenuItem,
            this.branchesMenuItem,
            this.salesMenuItem,
            this.accountingMenuItem,
            this.reportsMenuItem,
            this.settingsMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.mainMenuStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mainMenuStrip.Size = new System.Drawing.Size(1029, 42);
            this.mainMenuStrip.TabIndex = 0;
            // 
            // productsMenuItem
            // 
            this.productsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.الاقسامToolStripMenuItem,
            this.المنتجاتToolStripMenuItem,
            this.فاتورة_مشترياتToolStripMenuItem,
            this.الموردينToolStripMenuItem});
            this.productsMenuItem.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.productsMenuItem.Name = "productsMenuItem";
            this.productsMenuItem.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.productsMenuItem.Size = new System.Drawing.Size(114, 42);
            this.productsMenuItem.Text = "📦  المخزن";
            // 
            // الاقسامToolStripMenuItem
            // 
            this.الاقسامToolStripMenuItem.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.الاقسامToolStripMenuItem.Name = "الاقسامToolStripMenuItem";
            this.الاقسامToolStripMenuItem.Size = new System.Drawing.Size(215, 24);
            this.الاقسامToolStripMenuItem.Text = "📂  الأقسام";
            this.الاقسامToolStripMenuItem.Click += new System.EventHandler(this.الاقسامToolStripMenuItem_Click);
            // 
            // المنتجاتToolStripMenuItem
            // 
            this.المنتجاتToolStripMenuItem.ForeColor = System.Drawing.Color.LimeGreen;
            this.المنتجاتToolStripMenuItem.Name = "المنتجاتToolStripMenuItem";
            this.المنتجاتToolStripMenuItem.Size = new System.Drawing.Size(215, 24);
            this.المنتجاتToolStripMenuItem.Text = "📦  المنتجات";
            this.المنتجاتToolStripMenuItem.Click += new System.EventHandler(this.المنتجاتToolStripMenuItem_Click);
            // 
            // فاتورة_مشترياتToolStripMenuItem
            // 
            this.فاتورة_مشترياتToolStripMenuItem.ForeColor = System.Drawing.Color.Orange;
            this.فاتورة_مشترياتToolStripMenuItem.Name = "فاتورة_مشترياتToolStripMenuItem";
            this.فاتورة_مشترياتToolStripMenuItem.Size = new System.Drawing.Size(215, 24);
            this.فاتورة_مشترياتToolStripMenuItem.Text = "📥  فاتورة مشتريات";
            this.فاتورة_مشترياتToolStripMenuItem.Click += new System.EventHandler(this.فاتورة_مشترياتToolStripMenuItem_Click);
            // 
            // الموردينToolStripMenuItem
            // 
            this.الموردينToolStripMenuItem.ForeColor = System.Drawing.Color.MediumTurquoise;
            this.الموردينToolStripMenuItem.Name = "الموردينToolStripMenuItem";
            this.الموردينToolStripMenuItem.Size = new System.Drawing.Size(215, 24);
            this.الموردينToolStripMenuItem.Text = "🤝  حسابات الموردين";
            this.الموردينToolStripMenuItem.Click += new System.EventHandler(this.الموردينToolStripMenuItem_Click);
            // 
            // branchesMenuItem
            // 
            this.branchesMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.لوحة_تحكم_الفرعToolStripMenuItem,
            this.طلبات_الفروعToolStripMenuItem,
            this.إدارة_الطلباتToolStripMenuItem,
            this.صرف_بضاعةToolStripMenuItem});
            this.branchesMenuItem.ForeColor = System.Drawing.Color.MediumPurple;
            this.branchesMenuItem.Name = "branchesMenuItem";
            this.branchesMenuItem.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.branchesMenuItem.Size = new System.Drawing.Size(174, 42);
            this.branchesMenuItem.Text = "🚚  الفروع والعربيات";
            // 
            // لوحة_تحكم_الفرعToolStripMenuItem
            // 
            this.لوحة_تحكم_الفرعToolStripMenuItem.ForeColor = System.Drawing.Color.LightSkyBlue;
            this.لوحة_تحكم_الفرعToolStripMenuItem.Name = "لوحة_تحكم_الفرعToolStripMenuItem";
            this.لوحة_تحكم_الفرعToolStripMenuItem.Size = new System.Drawing.Size(192, 24);
            this.لوحة_تحكم_الفرعToolStripMenuItem.Text = "🖥️  إدارة الفروع";
            this.لوحة_تحكم_الفرعToolStripMenuItem.Click += new System.EventHandler(this.لوحة_تحكم_الفرعToolStripMenuItem_Click);
            // 
            // طلبات_الفروعToolStripMenuItem
            // 
            this.طلبات_الفروعToolStripMenuItem.ForeColor = System.Drawing.Color.LightCoral;
            this.طلبات_الفروعToolStripMenuItem.Name = "طلبات_الفروعToolStripMenuItem";
            this.طلبات_الفروعToolStripMenuItem.Size = new System.Drawing.Size(192, 24);
            this.طلبات_الفروعToolStripMenuItem.Text = "📝  طلبات الفروع";
            this.طلبات_الفروعToolStripMenuItem.Click += new System.EventHandler(this.طلبات_الفروعToolStripMenuItem_Click);
            // 
            // إدارة_الطلباتToolStripMenuItem
            // 
            this.إدارة_الطلباتToolStripMenuItem.ForeColor = System.Drawing.Color.OrangeRed;
            this.إدارة_الطلباتToolStripMenuItem.Name = "إدارة_الطلباتToolStripMenuItem";
            this.إدارة_الطلباتToolStripMenuItem.Size = new System.Drawing.Size(192, 24);
            this.إدارة_الطلباتToolStripMenuItem.Text = "📋  إدارة الطلبات";
            this.إدارة_الطلباتToolStripMenuItem.Click += new System.EventHandler(this.إدارة_الطلباتToolStripMenuItem_Click);
            // 
            // صرف_بضاعةToolStripMenuItem
            // 
            this.صرف_بضاعةToolStripMenuItem.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.صرف_بضاعةToolStripMenuItem.Name = "صرف_بضاعةToolStripMenuItem";
            this.صرف_بضاعةToolStripMenuItem.Size = new System.Drawing.Size(192, 24);
            this.صرف_بضاعةToolStripMenuItem.Text = "📤  صرف بضاعة";
            this.صرف_بضاعةToolStripMenuItem.Click += new System.EventHandler(this.صرف_بضاعةToolStripMenuItem_Click);
            // 
            // salesMenuItem
            // 
            this.salesMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.فاتورة_البيعToolStripMenuItem,
            this.مرتجع_مبيعاتToolStripMenuItem,
            this.العملاءToolStripMenuItem,
            this.السجلات_المركزيةToolStripMenuItem});
            this.salesMenuItem.ForeColor = System.Drawing.Color.LimeGreen;
            this.salesMenuItem.Name = "salesMenuItem";
            this.salesMenuItem.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.salesMenuItem.Size = new System.Drawing.Size(126, 42);
            this.salesMenuItem.Text = "🛒  المبيعات";
            // 
            // فاتورة_البيعToolStripMenuItem
            // 
            this.فاتورة_البيعToolStripMenuItem.ForeColor = System.Drawing.Color.LimeGreen;
            this.فاتورة_البيعToolStripMenuItem.Name = "فاتورة_البيعToolStripMenuItem";
            this.فاتورة_البيعToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
            this.فاتورة_البيعToolStripMenuItem.Text = "🧾  فاتورة البيع";
            this.فاتورة_البيعToolStripMenuItem.Click += new System.EventHandler(this.فاتورة_البيعToolStripMenuItem_Click);
            // 
            // مرتجع_مبيعاتToolStripMenuItem
            // 
            this.مرتجع_مبيعاتToolStripMenuItem.ForeColor = System.Drawing.Color.Crimson;
            this.مرتجع_مبيعاتToolStripMenuItem.Name = "مرتجع_مبيعاتToolStripMenuItem";
            this.مرتجع_مبيعاتToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
            this.مرتجع_مبيعاتToolStripMenuItem.Text = "↩️  مرتجع مبيعات";
            this.مرتجع_مبيعاتToolStripMenuItem.Click += new System.EventHandler(this.مرتجع_مبيعاتToolStripMenuItem_Click);
            // 
            // العملاءToolStripMenuItem
            // 
            this.العملاءToolStripMenuItem.ForeColor = System.Drawing.Color.DodgerBlue;
            this.العملاءToolStripMenuItem.Name = "العملاءToolStripMenuItem";
            this.العملاءToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
            this.العملاءToolStripMenuItem.Text = "👥  حسابات العملاء";
            this.العملاءToolStripMenuItem.Click += new System.EventHandler(this.العملاءToolStripMenuItem_Click);
            // 
            // السجلات_المركزيةToolStripMenuItem
            // 
            this.السجلات_المركزيةToolStripMenuItem.ForeColor = System.Drawing.Color.Crimson;
            this.السجلات_المركزيةToolStripMenuItem.Name = "السجلات_المركزيةToolStripMenuItem";
            this.السجلات_المركزيةToolStripMenuItem.Size = new System.Drawing.Size(216, 24);
            this.السجلات_المركزيةToolStripMenuItem.Text = "📊  السجلات المركزية";
            this.السجلات_المركزيةToolStripMenuItem.Click += new System.EventHandler(this.السجلات_المركزيةToolStripMenuItem_Click);
            // 
            // accountingMenuItem
            // 
            this.accountingMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.يومية_الخزنةToolStripMenuItem,
            this.مصروفات_المخزنToolStripMenuItem,
            this.خزينة_الإدارةToolStripMenuItem});
            this.accountingMenuItem.ForeColor = System.Drawing.Color.Gold;
            this.accountingMenuItem.Name = "accountingMenuItem";
            this.accountingMenuItem.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.accountingMenuItem.Size = new System.Drawing.Size(163, 42);
            this.accountingMenuItem.Text = "💰  الخزن والماليات";
            // 
            // يومية_الخزنةToolStripMenuItem
            // 
            this.يومية_الخزنةToolStripMenuItem.ForeColor = System.Drawing.Color.Gold;
            this.يومية_الخزنةToolStripMenuItem.Name = "يومية_الخزنةToolStripMenuItem";
            this.يومية_الخزنةToolStripMenuItem.Size = new System.Drawing.Size(239, 24);
            this.يومية_الخزنةToolStripMenuItem.Text = "📘  يومية الخزنة";
            this.يومية_الخزنةToolStripMenuItem.Click += new System.EventHandler(this.يومية_الخزنةToolStripMenuItem_Click);
            // 
            // مصروفات_المخزنToolStripMenuItem
            // 
            this.مصروفات_المخزنToolStripMenuItem.ForeColor = System.Drawing.Color.Tomato;
            this.مصروفات_المخزنToolStripMenuItem.Name = "مصروفات_المخزنToolStripMenuItem";
            this.مصروفات_المخزنToolStripMenuItem.Size = new System.Drawing.Size(239, 24);
            this.مصروفات_المخزنToolStripMenuItem.Text = "💸  مصروفات المخزن";
            this.مصروفات_المخزنToolStripMenuItem.Click += new System.EventHandler(this.مصروفات_المخزنToolStripMenuItem_Click);
            // 
            // خزينة_الإدارةToolStripMenuItem
            // 
            this.خزينة_الإدارةToolStripMenuItem.ForeColor = System.Drawing.Color.Gold;
            this.خزينة_الإدارةToolStripMenuItem.Name = "خزينة_الإدارةToolStripMenuItem";
            this.خزينة_الإدارةToolStripMenuItem.Size = new System.Drawing.Size(239, 24);
            this.خزينة_الإدارةToolStripMenuItem.Text = "🏛️  خزينة الإدارة المركزية";
            this.خزينة_الإدارةToolStripMenuItem.Click += new System.EventHandler(this.خزينة_الإدارةToolStripMenuItem_Click);
            // 
            // reportsMenuItem
            // 
            this.reportsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.التقارير_الشاملةToolStripMenuItem});
            this.reportsMenuItem.ForeColor = System.Drawing.Color.Violet;
            this.reportsMenuItem.Name = "reportsMenuItem";
            this.reportsMenuItem.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.reportsMenuItem.Size = new System.Drawing.Size(114, 42);
            this.reportsMenuItem.Text = "📊  التقارير";
            // 
            // التقارير_الشاملةToolStripMenuItem
            // 
            this.التقارير_الشاملةToolStripMenuItem.ForeColor = System.Drawing.Color.Violet;
            this.التقارير_الشاملةToolStripMenuItem.Name = "التقارير_الشاملةToolStripMenuItem";
            this.التقارير_الشاملةToolStripMenuItem.Size = new System.Drawing.Size(203, 24);
            this.التقارير_الشاملةToolStripMenuItem.Text = "📈  التقارير الشاملة";
            this.التقارير_الشاملةToolStripMenuItem.Click += new System.EventHandler(this.التقارير_الشاملةToolStripMenuItem_Click);
            // 
            // settingsMenuItem
            // 
            this.settingsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.المستخدمين_والصلاحياتToolStripMenuItem,
            this.إعدادات_النظامToolStripMenuItem});
            this.settingsMenuItem.ForeColor = System.Drawing.Color.LightPink;
            this.settingsMenuItem.Name = "settingsMenuItem";
            this.settingsMenuItem.Padding = new System.Windows.Forms.Padding(14, 0, 14, 0);
            this.settingsMenuItem.Size = new System.Drawing.Size(131, 42);
            this.settingsMenuItem.Text = "⚙️  الإعدادات";
            // 
            // المستخدمين_والصلاحياتToolStripMenuItem
            // 
            this.المستخدمين_والصلاحياتToolStripMenuItem.ForeColor = System.Drawing.Color.DodgerBlue;
            this.المستخدمين_والصلاحياتToolStripMenuItem.Name = "المستخدمين_والصلاحياتToolStripMenuItem";
            this.المستخدمين_والصلاحياتToolStripMenuItem.Size = new System.Drawing.Size(262, 24);
            this.المستخدمين_والصلاحياتToolStripMenuItem.Text = "👥  المستخدمين والصلاحيات";
            this.المستخدمين_والصلاحياتToolStripMenuItem.Click += new System.EventHandler(this.المستخدمين_والصلاحياتToolStripMenuItem_Click);
            // 
            // إعدادات_النظامToolStripMenuItem
            // 
            this.إعدادات_النظامToolStripMenuItem.ForeColor = System.Drawing.Color.DarkOrange;
            this.إعدادات_النظامToolStripMenuItem.Name = "إعدادات_النظامToolStripMenuItem";
            this.إعدادات_النظامToolStripMenuItem.Size = new System.Drawing.Size(262, 24);
            this.إعدادات_النظامToolStripMenuItem.Text = "⚙️  إعدادات النظام";
            this.إعدادات_النظامToolStripMenuItem.Click += new System.EventHandler(this.إعدادات_النظامToolStripMenuItem_Click);
            // 
            // pnlBranding
            // 
            this.pnlBranding.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.pnlBranding.Controls.Add(this.pnlDateInfo);
            this.pnlBranding.Controls.Add(this.pnlCalculator);
            this.pnlBranding.Controls.Add(this.btnExit);
            this.pnlBranding.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlBranding.Location = new System.Drawing.Point(0, 42);
            this.pnlBranding.Name = "pnlBranding";
            this.pnlBranding.Size = new System.Drawing.Size(163, 473);
            this.pnlBranding.TabIndex = 1;
            // 
            // pnlDateInfo
            // 
            this.pnlDateInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(78)))), ((int)(((byte)(125)))));
            this.pnlDateInfo.Controls.Add(this.lblDay);
            this.pnlDateInfo.Controls.Add(this.lblDate);
            this.pnlDateInfo.Controls.Add(this.lblTime);
            this.pnlDateInfo.Location = new System.Drawing.Point(0, 15);
            this.pnlDateInfo.Name = "pnlDateInfo";
            this.pnlDateInfo.Size = new System.Drawing.Size(163, 115);
            this.pnlDateInfo.TabIndex = 0;
            // 
            // lblDay
            // 
            this.lblDay.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblDay.ForeColor = System.Drawing.Color.Gold;
            this.lblDay.Location = new System.Drawing.Point(0, 0);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(163, 35);
            this.lblDay.TabIndex = 0;
            this.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDate
            // 
            this.lblDate.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDate.ForeColor = System.Drawing.Color.White;
            this.lblDate.Location = new System.Drawing.Point(0, 35);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(163, 35);
            this.lblDate.TabIndex = 1;
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTime
            // 
            this.lblTime.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTime.ForeColor = System.Drawing.Color.LightGreen;
            this.lblTime.Location = new System.Drawing.Point(0, 70);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(163, 35);
            this.lblTime.TabIndex = 2;
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCalculator
            // 
            this.pnlCalculator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(60)))), ((int)(((byte)(105)))));
            this.pnlCalculator.Controls.Add(this.picCalculator);
            this.pnlCalculator.Controls.Add(this.lblCalculator);
            this.pnlCalculator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlCalculator.Location = new System.Drawing.Point(0, 140);
            this.pnlCalculator.Name = "pnlCalculator";
            this.pnlCalculator.Size = new System.Drawing.Size(163, 115);
            this.pnlCalculator.TabIndex = 1;
            this.pnlCalculator.Click += new System.EventHandler(this.pnlCalculator_Click);
            // 
            // picCalculator
            // 
            this.picCalculator.BackColor = System.Drawing.Color.Transparent;
            this.picCalculator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCalculator.Location = new System.Drawing.Point(51, 8);
            this.picCalculator.Name = "picCalculator";
            this.picCalculator.Size = new System.Drawing.Size(60, 60);
            this.picCalculator.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCalculator.TabIndex = 0;
            this.picCalculator.TabStop = false;
            this.picCalculator.Click += new System.EventHandler(this.picCalculator_Click);
            // 
            // lblCalculator
            // 
            this.lblCalculator.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCalculator.ForeColor = System.Drawing.Color.White;
            this.lblCalculator.Location = new System.Drawing.Point(0, 75);
            this.lblCalculator.Name = "lblCalculator";
            this.lblCalculator.Size = new System.Drawing.Size(163, 30);
            this.lblCalculator.TabIndex = 1;
            this.lblCalculator.Text = "الحاسبة";
            this.lblCalculator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCalculator.Click += new System.EventHandler(this.lblCalculator_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(25)))), ((int)(((byte)(38)))));
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(0, 428);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(163, 45);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "⏻  خروج";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // pnlMainContent
            // 
            this.pnlMainContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(35)))), ((int)(((byte)(50)))));
            this.pnlMainContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContent.Location = new System.Drawing.Point(163, 42);
            this.pnlMainContent.Name = "pnlMainContent";
            this.pnlMainContent.Size = new System.Drawing.Size(866, 473);
            this.pnlMainContent.TabIndex = 3;
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(45)))), ((int)(((byte)(78)))));
            this.mainStatusStrip.ForeColor = System.Drawing.Color.White;
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 515);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 12, 0);
            this.mainStatusStrip.Size = new System.Drawing.Size(1029, 22);
            this.mainStatusStrip.TabIndex = 4;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(40)))), ((int)(((byte)(55)))));
            this.ClientSize = new System.Drawing.Size(1029, 537);
            this.Controls.Add(this.pnlMainContent);
            this.Controls.Add(this.pnlBranding);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "frmMain";
            this.Text = "Majestic Warehouse Management System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.WarehouseMainForm_Load);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.pnlBranding.ResumeLayout(false);
            this.pnlDateInfo.ResumeLayout(false);
            this.pnlCalculator.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCalculator)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem productsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem الاقسامToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem المنتجاتToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem فاتورة_مشترياتToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem الموردينToolStripMenuItem; // جديد الموردين
        private System.Windows.Forms.ToolStripMenuItem branchesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem لوحة_تحكم_الفرعToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem طلبات_الفروعToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem إدارة_الطلباتToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem صرف_بضاعةToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem فاتورة_البيعToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem مرتجع_مبيعاتToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem العملاءToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem السجلات_المركزيةToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem يومية_الخزنةToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem مصروفات_المخزنToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem خزينة_الإدارةToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsMenuItem; // جديد التقارير
        private System.Windows.Forms.ToolStripMenuItem التقارير_الشاملةToolStripMenuItem; // جديد التقارير
        private System.Windows.Forms.ToolStripMenuItem settingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem المستخدمين_والصلاحياتToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem إعدادات_النظامToolStripMenuItem;
        private System.Windows.Forms.Panel pnlBranding;
        private System.Windows.Forms.Panel pnlDateInfo;
        private System.Windows.Forms.Label lblDay;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Panel pnlCalculator;
        private System.Windows.Forms.PictureBox picCalculator;
        private System.Windows.Forms.Label lblCalculator;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel pnlMainContent;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
    }
}