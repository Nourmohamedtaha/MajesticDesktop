using System;
using System.Windows.Forms;

namespace MajesticDesktop
{
    public partial class frmActivation : Form
    {
        public frmActivation()
        {
            InitializeComponent();
        }

        private void frmActivation_Load(object sender, EventArgs e)
        {
            // عرض اسم الجهاز للعميل
            txtDeviceName.Text = Environment.MachineName;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtDeviceName.Text);
            MessageBox.Show("تم نسخ رمز الجهاز بنجاح. أرسله للمبرمج.", "نسخ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtActivationKey.Text))
            {
                MessageBox.Show("برجاء إدخال كود التفعيل أولاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string pcName = txtDeviceName.Text.Trim();

            // معادلتك: عدد حروف اسم الجهاز مضروب في 99
            int secretMath = pcName.Length * 99;

            // الكود الصح اللي المفروض العميل يكتبه
            string expectedKey = $"MAJ-{pcName}-{secretMath}";

            if (txtActivationKey.Text.Trim().ToUpper() == expectedKey.ToUpper())
            {
                // حفظ التفعيل واسم الجهاز (عشان لو نقل فولدر البرنامج لجهاز تاني ميشتغلش)
                Properties.Settings.Default.IsActivated = true;
                Properties.Settings.Default.RegisteredPC = Environment.MachineName;
                Properties.Settings.Default.Save();

                MessageBox.Show("تم تفعيل النسخة بنجاح! أهلاً بك في نظام ماجستيك.", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("كود التفعيل غير صحيح! تأكد من الكود أو تواصل مع الدعم الفني.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}