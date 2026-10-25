namespace MajesticDesktop.Forms
{
    partial class frmSettings
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.txtCompanyName = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblTaxNumber = new System.Windows.Forms.Label();
            this.txtTaxNumber = new System.Windows.Forms.TextBox();
            this.groupBoxLogo = new System.Windows.Forms.GroupBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.btnChooseLogo = new System.Windows.Forms.Button();
            this.btnRemoveLogo = new System.Windows.Forms.Button();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.groupBoxLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(20, 30, 45); // Dark Theme Header
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(800, 85);

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(25, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(188, 31);
            this.lblTitle.Text = "⚙️ إعدادات النظام";

            // lblSubtitle
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.DarkGray;
            this.lblSubtitle.Location = new System.Drawing.Point(40, 51);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(260, 16);
            this.lblSubtitle.Text = "تكوين بيانات الشركة الأساسية وطباعة التقارير";

            // pnlContent
            this.pnlContent.BackColor = System.Drawing.Color.FromArgb(15, 23, 42); // Dark Theme Background
            this.pnlContent.Controls.Add(this.lblCompanyName);
            this.pnlContent.Controls.Add(this.txtCompanyName);
            this.pnlContent.Controls.Add(this.lblPhone);
            this.pnlContent.Controls.Add(this.txtPhone);
            this.pnlContent.Controls.Add(this.lblAddress);
            this.pnlContent.Controls.Add(this.txtAddress);
            this.pnlContent.Controls.Add(this.lblTaxNumber);
            this.pnlContent.Controls.Add(this.txtTaxNumber);
            this.pnlContent.Controls.Add(this.groupBoxLogo);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 85);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(800, 315);

            // Text Inputs & Labels
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCompanyName.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblCompanyName.Location = new System.Drawing.Point(40, 30);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Text = "اسم الشركة أو المؤسسة:";

            this.txtCompanyName.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.txtCompanyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCompanyName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.txtCompanyName.ForeColor = System.Drawing.Color.White;
            this.txtCompanyName.Location = new System.Drawing.Point(40, 55);
            this.txtCompanyName.Name = "txtCompanyName";
            this.txtCompanyName.Size = new System.Drawing.Size(350, 29);

            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPhone.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblPhone.Location = new System.Drawing.Point(40, 95);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Text = "أرقام الهواتف (للتواصل):";

            this.txtPhone.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.txtPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPhone.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPhone.ForeColor = System.Drawing.Color.White;
            this.txtPhone.Location = new System.Drawing.Point(40, 120);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(350, 29);

            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblAddress.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblAddress.Location = new System.Drawing.Point(40, 160);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Text = "عنوان الشركة:";

            this.txtAddress.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAddress.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtAddress.ForeColor = System.Drawing.Color.White;
            this.txtAddress.Location = new System.Drawing.Point(40, 185);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(350, 29);

            this.lblTaxNumber.AutoSize = true;
            this.lblTaxNumber.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTaxNumber.ForeColor = System.Drawing.Color.FromArgb(241, 245, 249);
            this.lblTaxNumber.Location = new System.Drawing.Point(40, 225);
            this.lblTaxNumber.Name = "lblTaxNumber";
            this.lblTaxNumber.Text = "الرقم الضريبي (إن وجد):";

            this.txtTaxNumber.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.txtTaxNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTaxNumber.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtTaxNumber.ForeColor = System.Drawing.Color.White;
            this.txtTaxNumber.Location = new System.Drawing.Point(40, 250);
            this.txtTaxNumber.Name = "txtTaxNumber";
            this.txtTaxNumber.Size = new System.Drawing.Size(350, 29);

            // groupBoxLogo
            this.groupBoxLogo.Controls.Add(this.picLogo);
            this.groupBoxLogo.Controls.Add(this.btnChooseLogo);
            this.groupBoxLogo.Controls.Add(this.btnRemoveLogo);
            this.groupBoxLogo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.groupBoxLogo.ForeColor = System.Drawing.Color.DarkGray;
            this.groupBoxLogo.Location = new System.Drawing.Point(450, 30);
            this.groupBoxLogo.Name = "groupBoxLogo";
            this.groupBoxLogo.Size = new System.Drawing.Size(300, 249);
            this.groupBoxLogo.Text = "شعار الشركة (اللوجو)";

            this.picLogo.BackColor = System.Drawing.Color.FromArgb(30, 41, 59);
            this.picLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLogo.Location = new System.Drawing.Point(50, 35);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(200, 150);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            this.btnChooseLogo.BackColor = System.Drawing.Color.FromArgb(59, 130, 246); // Accent Blue
            this.btnChooseLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChooseLogo.FlatAppearance.BorderSize = 0;
            this.btnChooseLogo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChooseLogo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnChooseLogo.ForeColor = System.Drawing.Color.White;
            this.btnChooseLogo.Location = new System.Drawing.Point(155, 195);
            this.btnChooseLogo.Name = "btnChooseLogo";
            this.btnChooseLogo.Size = new System.Drawing.Size(95, 35);
            this.btnChooseLogo.Text = "اختيار صورة";
            this.btnChooseLogo.UseVisualStyleBackColor = false;
            this.btnChooseLogo.Click += new System.EventHandler(this.btnChooseLogo_Click);

            this.btnRemoveLogo.BackColor = System.Drawing.Color.FromArgb(239, 68, 68); // Red
            this.btnRemoveLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveLogo.FlatAppearance.BorderSize = 0;
            this.btnRemoveLogo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveLogo.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRemoveLogo.ForeColor = System.Drawing.Color.White;
            this.btnRemoveLogo.Location = new System.Drawing.Point(50, 195);
            this.btnRemoveLogo.Name = "btnRemoveLogo";
            this.btnRemoveLogo.Size = new System.Drawing.Size(95, 35);
            this.btnRemoveLogo.Text = "حذف";
            this.btnRemoveLogo.UseVisualStyleBackColor = false;
            this.btnRemoveLogo.Click += new System.EventHandler(this.btnRemoveLogo_Click);

            // pnlFooter
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(20, 30, 45); // Dark Footer
            this.pnlFooter.Controls.Add(this.btnSave);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 400);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(800, 80);

            this.btnSave.BackColor = System.Drawing.Color.FromArgb(34, 197, 94); // Green Success
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(40, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(200, 50);
            this.btnSave.Text = "حفظ الإعدادات ✔";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // frmSettings
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إعدادات النظام";
            this.Load += new System.EventHandler(this.frmSettings_Load);

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.pnlContent.PerformLayout();
            this.groupBoxLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Label lblCompanyName;
        private System.Windows.Forms.TextBox txtCompanyName;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblTaxNumber;
        private System.Windows.Forms.TextBox txtTaxNumber;
        private System.Windows.Forms.GroupBox groupBoxLogo;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Button btnChooseLogo;
        private System.Windows.Forms.Button btnRemoveLogo;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnSave;
    }
}