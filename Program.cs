using System;
using System.Windows.Forms;
using MajesticDesktop.Forms;
using MajesticDesktop.Services;

namespace MajesticDesktop
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. التأكد من الاتصال بقاعدة البيانات وإنشاء الجداول
            try
            {
                UserService userService = new UserService();
            }
            catch (Exception ex)
            {
                MessageBox.Show("تعذر الاتصال بقاعدة البيانات أو إنشاء الجداول:\n" + ex.Message, "خطأ فادح", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // إنهاء البرنامج فوراً إذا فشل الاتصال
            }

            // 2. التحقق من التفعيل واسم الجهاز (تمت إضافتها هنا)
            bool isActivated = Properties.Settings.Default.IsActivated;
            string registeredPC = Properties.Settings.Default.RegisteredPC;
            string currentPC = Environment.MachineName;

            if (!isActivated || registeredPC != currentPC)
            {
                frmActivation frmAct = new frmActivation();
                if (frmAct.ShowDialog() != DialogResult.OK)
                {
                    return; // إذا قام المستخدم بإغلاق شاشة التفعيل من (X) ينتهي البرنامج
                }
            }

            // 3. عرض شاشة تسجيل الدخول (كودك الأصلي)
            frmLogin loginForm = new frmLogin();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // إذا نجح الدخول وتم تمرير DialogResult.OK، نفتح الشاشة الرئيسية
                Application.Run(new frmMain());
            }
        }
    }
}