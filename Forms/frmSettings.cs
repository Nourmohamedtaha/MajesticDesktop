using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MajesticDesktop.Models;
using MajesticDesktop.Services;

namespace MajesticDesktop.Forms
{
    public partial class frmSettings : Form
    {
        private SettingsService settingsService;

        public frmSettings()
        {
            InitializeComponent();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            settingsService = new SettingsService();
        }

        private async void frmSettings_Load(object sender, EventArgs e)
        {
            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;
                await LoadCurrentSettingsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private async Task LoadCurrentSettingsAsync()
        {
            SystemSetting setting = await settingsService.GetSettingsAsync();

            txtCompanyName.Text = setting.CompanyName;
            txtPhone.Text = setting.Phone;
            txtAddress.Text = setting.Address;
            txtTaxNumber.Text = setting.TaxNumber;

            if (setting.Logo != null && setting.Logo.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(setting.Logo))
                {
                    picLogo.Image = Image.FromStream(ms);
                }
            }
        }

        private void btnChooseLogo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "اختر شعار الشركة (اللوجو)";
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // تحرير الذاكرة للصورة القديمة قبل استبدالها لمنع الـ Memory Leaks
                    if (picLogo.Image != null) picLogo.Image.Dispose();
                    picLogo.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void btnRemoveLogo_Click(object sender, EventArgs e)
        {
            if (picLogo.Image != null)
            {
                picLogo.Image.Dispose();
                picLogo.Image = null;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
            {
                MessageBox.Show("اسم الشركة مطلوب ولا يمكن تركه فارغاً.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                txtCompanyName.Focus();
                return;
            }

            btnSave.Enabled = false; // حماية ضد الدبل كليك
            Cursor = Cursors.WaitCursor;

            SystemSetting setting = new SystemSetting
            {
                CompanyName = txtCompanyName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                TaxNumber = txtTaxNumber.Text.Trim()
            };

            if (picLogo.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    picLogo.Image.Save(ms, picLogo.Image.RawFormat);
                    setting.Logo = ms.ToArray();
                }
            }
            else
            {
                setting.Logo = null;
            }

            try
            {
                await settingsService.SaveSettingsAsync(setting);
                MessageBox.Show("تم حفظ إعدادات النظام بنجاح.\nسيتم استخدام هذه البيانات في طباعة الفواتير والتقارير.", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("حدث خطأ أثناء حفظ الإعدادات.\n" + ex.Message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                btnSave.Enabled = true;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}