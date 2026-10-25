using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    public partial class frmMain : Form
    {
        private Timer clockTimer;

        public frmMain()
        {
            InitializeComponent();

            // FIX: رندرر مخصص بدل الديفولت عشان لون القائمة يفضل واضح وقت الـHover (شرح فوق)
            mainMenuStrip.Renderer = new MajesticMenuRenderer();
            SetupHoverMenu();

            CreateCalculatorImage();
            InitializeClock();
            SetupArabicDateTime();
            this.Resize += FrmMain_Resize;
        }

        private void WarehouseMainForm_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateDateTime();
                AdjustMenuSpacing();
                LoadDashboard();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmMain_Resize(object sender, EventArgs e)
        {
            AdjustMenuSpacing();
        }

        // القائمة تفتح على مجرد مرور الماوس من غير أي كليك
        private void SetupHoverMenu()
        {
            foreach (ToolStripItem topItem in mainMenuStrip.Items)
            {
                ToolStripMenuItem menuItem = topItem as ToolStripMenuItem;
                if (menuItem == null || !menuItem.HasDropDownItems) continue;
                menuItem.MouseEnter += TopMenuItem_MouseEnter;
            }
        }

        private void TopMenuItem_MouseEnter(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item == null) return;
            if (!item.DropDown.Visible)
                item.ShowDropDown();
        }

        private void AdjustMenuSpacing()
        {
            try
            {
                if (mainMenuStrip.Items.Count == 0) return;
                int widthPerItem = (mainMenuStrip.ClientSize.Width - 30) / mainMenuStrip.Items.Count;
                foreach (ToolStripItem item in mainMenuStrip.Items)
                {
                    item.AutoSize = false;
                    item.Width = widthPerItem;
                    item.TextAlign = ContentAlignment.MiddleCenter;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDashboard()
        {
            try
            {
                pnlMainContent.Controls.Clear();
                uc_Dashboard dashboard = new uc_Dashboard();
                dashboard.TopLevel = false;
                dashboard.FormBorderStyle = FormBorderStyle.None;
                dashboard.Dock = DockStyle.Fill;
                pnlMainContent.Controls.Add(dashboard);
                dashboard.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeClock()
        {
            try
            {
                clockTimer = new Timer();
                clockTimer.Interval = 1000;
                clockTimer.Tick += ClockTimer_Tick;
                clockTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            try
            {
                DateTime now = DateTime.Now;
                lblDay.Text = GetArabicDayName(now.DayOfWeek);
                lblDate.Text = now.ToString("dd / MM / yyyy", CultureInfo.InvariantCulture);
                lblTime.Text = now.ToString("hh:mm:ss tt", new CultureInfo("ar-EG"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetArabicDayName(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Saturday: return "السبت";
                case DayOfWeek.Sunday: return "الأحد";
                case DayOfWeek.Monday: return "الإثنين";
                case DayOfWeek.Tuesday: return "الثلاثاء";
                case DayOfWeek.Wednesday: return "الأربعاء";
                case DayOfWeek.Thursday: return "الخميس";
                case DayOfWeek.Friday: return "الجمعة";
                default: return "";
            }
        }

        private void SetupArabicDateTime()
        {
            lblDay.RightToLeft = RightToLeft.Yes;
            lblDate.RightToLeft = RightToLeft.Yes;
            lblTime.RightToLeft = RightToLeft.Yes;

            lblDay.TextAlign = ContentAlignment.MiddleCenter;
            lblDate.TextAlign = ContentAlignment.MiddleCenter;
            lblTime.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void CreateCalculatorImage()
        {
            try
            {
                int width = 90;
                int height = 90;
                Bitmap calculatorBitmap = new Bitmap(width, height);

                using (Graphics g = Graphics.FromImage(calculatorBitmap))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.Clear(Color.Transparent);

                    Rectangle body = new Rectangle(14, 5, 62, 80);
                    using (GraphicsPath bodyPath = CreateRoundedRectangle(body, 10))
                    {
                        using (Brush bodyBrush = new SolidBrush(Color.FromArgb(235, 242, 250)))
                            g.FillPath(bodyBrush, bodyPath);
                        using (Pen bodyPen = new Pen(Color.FromArgb(75, 105, 135), 2))
                            g.DrawPath(bodyPen, bodyPath);
                    }

                    Rectangle screen = new Rectangle(23, 14, 44, 18);
                    using (GraphicsPath screenPath = CreateRoundedRectangle(screen, 4))
                    using (Brush screenBrush = new SolidBrush(Color.FromArgb(30, 55, 70)))
                        g.FillPath(screenBrush, screenPath);

                    using (Pen linePen = new Pen(Color.FromArgb(80, 210, 190), 2))
                        g.DrawLine(linePen, 53, 25, 62, 25);

                    Color[] buttonColors = {
                        Color.FromArgb(80, 170, 230),
                        Color.FromArgb(100, 190, 150),
                        Color.FromArgb(245, 180, 70),
                        Color.FromArgb(190, 130, 220)
                    };

                    int buttonIndex = 0;
                    for (int row = 0; row < 3; row++)
                    {
                        for (int col = 0; col < 3; col++)
                        {
                            int x = 23 + (col * 14);
                            int y = 38 + (row * 13);
                            Rectangle button = new Rectangle(x, y, 9, 8);
                            using (Brush buttonBrush = new SolidBrush(buttonColors[buttonIndex % buttonColors.Length]))
                                g.FillRectangle(buttonBrush, button);
                            buttonIndex++;
                        }
                    }

                    Rectangle bottomButton = new Rectangle(23, 77, 44, 5);
                    using (Brush bottomBrush = new SolidBrush(Color.FromArgb(80, 150, 200)))
                        g.FillRectangle(bottomBrush, bottomButton);
                }

                if (picCalculator.Image != null)
                    picCalculator.Image.Dispose();

                picCalculator.Image = calculatorBitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle rectangle, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            Rectangle arc = new Rectangle(rectangle.Location, new Size(diameter, diameter));

            path.AddArc(arc, 180, 90);
            arc.X = rectangle.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = rectangle.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = rectangle.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();

            return path;
        }

        private void pnlCalculator_Click(object sender, EventArgs e) { OpenCalculator(); }
        private void picCalculator_Click(object sender, EventArgs e) { OpenCalculator(); }
        private void lblCalculator_Click(object sender, EventArgs e) { OpenCalculator(); }

        private void OpenCalculator()
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "calc.exe",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر فتح الآلة الحاسبة.\n\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("هل تريد بالفعل الخروج من البرنامج؟", "تأكيد الخروج", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (clockTimer != null)
                    {
                        clockTimer.Stop();
                        clockTimer.Dispose();
                    }
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (clockTimer != null)
            {
                clockTimer.Stop();
                clockTimer.Dispose();
                clockTimer = null;
            }

            if (picCalculator != null && picCalculator.Image != null)
            {
                picCalculator.Image.Dispose();
                picCalculator.Image = null;
            }

            base.OnFormClosed(e);
        }

        private void ShowForm(Form frm)
        {
            try
            {
                // 1. إجبار الشاشة الرئيسية على سحب التركيز لإغلاق أي قائمة إكمال تلقائي (AutoComplete) مفتوحة بأمان
                this.Focus();

                // أو يمكنك استخدام هذا السطر كبديل لإلغاء التركيز عن أي TextBox نشط:
                // this.ActiveControl = null;

                // 2. استخدام using لتنظيف الذاكرة (Memory Management) وتجنب تسريب الـ GDI
                using (frm)
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog(this); // إضافة (this) لربط الشاشة المنبثقة بالشاشة الرئيسية
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء فتح الشاشة:\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void الاقسامToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmCategories());
        private void المنتجاتToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmProducts());
        private void فاتورة_مشترياتToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmPurchaseInvoice());
        private void الموردينToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmSuppliers());

        private void لوحة_تحكم_الفرعToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmBranchManagement());
        private void طلبات_الفروعToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmBranchRequests());
        private void إدارة_الطلباتToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmManageRequests());
        private void صرف_بضاعةToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmIssueStock());
        private void العملاءToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmCustomers());
        private void السجلات_المركزيةToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmCentralRecords());

        private void يومية_الخزنةToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmCashRegister());
        private void خزينة_الإدارةToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmMainTreasury());

        private void مصروفات_المخزنToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmWarehouseExpenses());

        private void المستخدمين_والصلاحياتToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmUsers());
        private void إعدادات_النظامToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmSettings());

        private void فاتورة_البيعToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmSalesInvoice frmSales = new frmSalesInvoice();
                frmSales.StartPosition = FormStartPosition.CenterScreen;
                frmSales.WindowState = FormWindowState.Maximized;
                frmSales.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void مرتجع_مبيعاتToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmSalesReturn());
        private void التقارير_الشاملةToolStripMenuItem_Click(object sender, EventArgs e) => ShowForm(new frmReports());
    }
}