namespace SistemKontroloruDesktopApp
{
    partial class LoginMini
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginMini));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMetro_Ad = new MetroFramework.Controls.MetroTextBox();
            this.txtMetro_Pass = new MetroFramework.Controls.MetroTextBox();
            this.Btn_Giris = new System.Windows.Forms.Button();
            this.NI_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ayarlarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SistemTepsisi = new System.Windows.Forms.NotifyIcon(this.components);
            this.isKontrol = new System.Windows.Forms.Timer(this.components);
            this.VideoRecord = new System.Windows.Forms.Timer(this.components);
            this.RecordStop = new System.Windows.Forms.Timer(this.components);
            this.Banned_App_Timer = new System.Windows.Forms.Timer(this.components);
            this.Running_Time_Timer = new System.Windows.Forms.Timer(this.components);
            this.Calisan_uygulama_gonder = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.NI_Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kullanıcı Adı";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Şifre";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(this.label3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 63);
            this.panel1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 28F);
            this.label3.Location = new System.Drawing.Point(100, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 44);
            this.label3.TabIndex = 9;
            this.label3.Text = "<SK>";
            // 
            // txtMetro_Ad
            // 
            // 
            // 
            // 
            this.txtMetro_Ad.CustomButton.Image = null;
            this.txtMetro_Ad.CustomButton.Location = new System.Drawing.Point(133, 1);
            this.txtMetro_Ad.CustomButton.Name = "";
            this.txtMetro_Ad.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtMetro_Ad.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtMetro_Ad.CustomButton.TabIndex = 1;
            this.txtMetro_Ad.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtMetro_Ad.CustomButton.UseSelectable = true;
            this.txtMetro_Ad.CustomButton.Visible = false;
            this.txtMetro_Ad.Lines = new string[0];
            this.txtMetro_Ad.Location = new System.Drawing.Point(126, 107);
            this.txtMetro_Ad.MaxLength = 32767;
            this.txtMetro_Ad.Name = "txtMetro_Ad";
            this.txtMetro_Ad.PasswordChar = '\0';
            this.txtMetro_Ad.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtMetro_Ad.SelectedText = "";
            this.txtMetro_Ad.SelectionLength = 0;
            this.txtMetro_Ad.SelectionStart = 0;
            this.txtMetro_Ad.ShortcutsEnabled = true;
            this.txtMetro_Ad.Size = new System.Drawing.Size(155, 23);
            this.txtMetro_Ad.TabIndex = 3;
            this.txtMetro_Ad.UseSelectable = true;
            this.txtMetro_Ad.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtMetro_Ad.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txtMetro_Pass
            // 
            this.txtMetro_Pass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            // 
            // 
            // 
            this.txtMetro_Pass.CustomButton.Image = null;
            this.txtMetro_Pass.CustomButton.Location = new System.Drawing.Point(133, 1);
            this.txtMetro_Pass.CustomButton.Name = "";
            this.txtMetro_Pass.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtMetro_Pass.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtMetro_Pass.CustomButton.TabIndex = 1;
            this.txtMetro_Pass.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtMetro_Pass.CustomButton.UseSelectable = true;
            this.txtMetro_Pass.CustomButton.Visible = false;
            this.txtMetro_Pass.Lines = new string[0];
            this.txtMetro_Pass.Location = new System.Drawing.Point(126, 173);
            this.txtMetro_Pass.MaxLength = 32767;
            this.txtMetro_Pass.Name = "txtMetro_Pass";
            this.txtMetro_Pass.PasswordChar = '*';
            this.txtMetro_Pass.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtMetro_Pass.SelectedText = "";
            this.txtMetro_Pass.SelectionLength = 0;
            this.txtMetro_Pass.SelectionStart = 0;
            this.txtMetro_Pass.ShortcutsEnabled = true;
            this.txtMetro_Pass.Size = new System.Drawing.Size(155, 23);
            this.txtMetro_Pass.TabIndex = 4;
            this.txtMetro_Pass.UseSelectable = true;
            this.txtMetro_Pass.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtMetro_Pass.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // Btn_Giris
            // 
            this.Btn_Giris.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Giris.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            this.Btn_Giris.Location = new System.Drawing.Point(21, 220);
            this.Btn_Giris.Name = "Btn_Giris";
            this.Btn_Giris.Size = new System.Drawing.Size(260, 68);
            this.Btn_Giris.TabIndex = 5;
            this.Btn_Giris.Text = "Giriş";
            this.Btn_Giris.UseVisualStyleBackColor = true;
            this.Btn_Giris.Click += new System.EventHandler(this.Btn_Giris_Click);
            // 
            // NI_Menu
            // 
            this.NI_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ayarlarToolStripMenuItem});
            this.NI_Menu.Name = "NI_Menu";
            this.NI_Menu.Size = new System.Drawing.Size(112, 26);
            // 
            // ayarlarToolStripMenuItem
            // 
            this.ayarlarToolStripMenuItem.Name = "ayarlarToolStripMenuItem";
            this.ayarlarToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.ayarlarToolStripMenuItem.Text = "Ayarlar";
            this.ayarlarToolStripMenuItem.Click += new System.EventHandler(this.ayarlarToolStripMenuItem_Click);
            // 
            // SistemTepsisi
            // 
            this.SistemTepsisi.ContextMenuStrip = this.NI_Menu;
            this.SistemTepsisi.Icon = ((System.Drawing.Icon)(resources.GetObject("SistemTepsisi.Icon")));
            this.SistemTepsisi.Text = "Sistem Kontrolörü";
            // 
            // isKontrol
            // 
            this.isKontrol.Interval = 10000;
            this.isKontrol.Tick += new System.EventHandler(this.isKontrol_Tick);
            // 
            // VideoRecord
            // 
            this.VideoRecord.Interval = 15;
            this.VideoRecord.Tick += new System.EventHandler(this.VideoRecord_Tick);
            // 
            // RecordStop
            // 
            this.RecordStop.Interval = 30000;
            this.RecordStop.Tick += new System.EventHandler(this.RecordStop_Tick);
            // 
            // Banned_App_Timer
            // 
            this.Banned_App_Timer.Interval = 5000;
            this.Banned_App_Timer.Tick += new System.EventHandler(this.Banned_App_Timer_Tick);
            // 
            // Running_Time_Timer
            // 
            this.Running_Time_Timer.Interval = 5000;
            this.Running_Time_Timer.Tick += new System.EventHandler(this.Running_Time_Timer_Tick);
            // 
            // Calisan_uygulama_gonder
            // 
            this.Calisan_uygulama_gonder.Interval = 10000;
            this.Calisan_uygulama_gonder.Tick += new System.EventHandler(this.Calisan_uygulama_gonder_Tick);
            // 
            // LoginMini
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.Btn_Giris);
            this.Controls.Add(this.txtMetro_Pass);
            this.Controls.Add(this.txtMetro_Ad);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "LoginMini";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "2";
            this.Activated += new System.EventHandler(this.LoginMini_Activated);
            this.Load += new System.EventHandler(this.LoginMini_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.NI_Menu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private MetroFramework.Controls.MetroTextBox txtMetro_Ad;
        private MetroFramework.Controls.MetroTextBox txtMetro_Pass;
        private System.Windows.Forms.Button Btn_Giris;
        private System.Windows.Forms.ContextMenuStrip NI_Menu;
        private System.Windows.Forms.ToolStripMenuItem ayarlarToolStripMenuItem;
        private System.Windows.Forms.Timer isKontrol;
        private System.Windows.Forms.Timer VideoRecord;
        private System.Windows.Forms.Timer RecordStop;
        private System.Windows.Forms.Timer Banned_App_Timer;
        public System.Windows.Forms.NotifyIcon SistemTepsisi;
        private System.Windows.Forms.Timer Running_Time_Timer;
        private System.Windows.Forms.Timer Calisan_uygulama_gonder;
    }
}