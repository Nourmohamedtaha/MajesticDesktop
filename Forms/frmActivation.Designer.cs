namespace MajesticDesktop
{
    partial class frmActivation
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblInstruction;
        private System.Windows.Forms.TextBox txtDeviceName;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.TextBox txtActivationKey;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.Button btnExit;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmActivation));
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblInstruction = new System.Windows.Forms.Label();
            this.txtDeviceName = new System.Windows.Forms.TextBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.txtActivationKey = new System.Windows.Forms.TextBox();
            this.btnActivate = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(204)))), ((int)(((byte)(21)))));
            this.lblTitle.Location = new System.Drawing.Point(73, 17);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(255, 37);
            this.lblTitle.TabIndex = 6;
            this.lblTitle.Text = "تفعيل نظام ماجستيك";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInstruction
            // 
            this.lblInstruction.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblInstruction.ForeColor = System.Drawing.Color.White;
            this.lblInstruction.Location = new System.Drawing.Point(17, 61);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Size = new System.Drawing.Size(343, 43);
            this.lblInstruction.TabIndex = 5;
            this.lblInstruction.Text = "برجاء نسخ (رمز الجهاز) وإرساله للمبرمج للحصول على (كود التفعيل) الخاص بنسختك.";
            this.lblInstruction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDeviceName
            // 
            this.txtDeviceName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.txtDeviceName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeviceName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.txtDeviceName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.txtDeviceName.Location = new System.Drawing.Point(86, 113);
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.ReadOnly = true;
            this.txtDeviceName.Size = new System.Drawing.Size(275, 32);
            this.txtDeviceName.TabIndex = 4;
            this.txtDeviceName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnCopy.FlatAppearance.BorderSize = 0;
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopy.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCopy.ForeColor = System.Drawing.Color.White;
            this.btnCopy.Location = new System.Drawing.Point(17, 113);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(60, 28);
            this.btnCopy.TabIndex = 3;
            this.btnCopy.Text = "نسخ";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // txtActivationKey
            // 
            this.txtActivationKey.BackColor = System.Drawing.Color.White;
            this.txtActivationKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtActivationKey.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.txtActivationKey.ForeColor = System.Drawing.Color.Black;
            this.txtActivationKey.Location = new System.Drawing.Point(17, 156);
            this.txtActivationKey.Name = "txtActivationKey";
            this.txtActivationKey.Size = new System.Drawing.Size(343, 36);
            this.txtActivationKey.TabIndex = 2;
            this.txtActivationKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnActivate
            // 
            this.btnActivate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnActivate.FlatAppearance.BorderSize = 0;
            this.btnActivate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivate.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnActivate.ForeColor = System.Drawing.Color.White;
            this.btnActivate.Location = new System.Drawing.Point(17, 208);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(223, 39);
            this.btnActivate.TabIndex = 1;
            this.btnActivate.Text = "تفعيل النظام ✔️";
            this.btnActivate.UseVisualStyleBackColor = false;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(249, 208);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(111, 39);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "خروج";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // frmActivation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(377, 269);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnActivate);
            this.Controls.Add(this.txtActivationKey);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.txtDeviceName);
            this.Controls.Add(this.lblInstruction);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmActivation";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تفعيل النسخة";
            this.Load += new System.EventHandler(this.frmActivation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}