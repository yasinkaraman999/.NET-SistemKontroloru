namespace SistemKontroloruDesktopApp
{
    partial class Main
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.uygulama_kullanim_zamanlayicisi = new System.Windows.Forms.Timer(this.components);
            this.genel_kullanim_zamanlayicisi = new System.Windows.Forms.Timer(this.components);
            this.SistemTepsisi = new System.Windows.Forms.NotifyIcon(this.components);
            this.NI_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ayarlarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cikis = new System.Windows.Forms.Button();
            this.ayar = new System.Windows.Forms.Button();
            this.rapor = new System.Windows.Forms.Button();
            this.dashboard = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.mini_panel_left = new System.Windows.Forms.Panel();
            this.NI_Menu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // uygulama_kullanim_zamanlayicisi
            // 
            this.uygulama_kullanim_zamanlayicisi.Interval = 1000;
            this.uygulama_kullanim_zamanlayicisi.Tick += new System.EventHandler(this.uygulama_kullanim_zamanlayicisi_Tick);
            // 
            // genel_kullanim_zamanlayicisi
            // 
            this.genel_kullanim_zamanlayicisi.Interval = 250;
            this.genel_kullanim_zamanlayicisi.Tick += new System.EventHandler(this.genel_kullanim_zamanlayicisi_Tick);
            // 
            // SistemTepsisi
            // 
            this.SistemTepsisi.ContextMenuStrip = this.NI_Menu;
            this.SistemTepsisi.Icon = ((System.Drawing.Icon)(resources.GetObject("SistemTepsisi.Icon")));
            this.SistemTepsisi.Text = "Sistem Kontrolörü";
            // 
            // NI_Menu
            // 
            this.NI_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ayarlarToolStripMenuItem});
            this.NI_Menu.Name = "NI_Menu";
            this.NI_Menu.Size = new System.Drawing.Size(181, 48);
            this.NI_Menu.Opening += new System.ComponentModel.CancelEventHandler(this.NI_Menu_Opening);
            // 
            // ayarlarToolStripMenuItem
            // 
            this.ayarlarToolStripMenuItem.Name = "ayarlarToolStripMenuItem";
            this.ayarlarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ayarlarToolStripMenuItem.Text = "Ayarlar";
            this.ayarlarToolStripMenuItem.Click += new System.EventHandler(this.ayarlarToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(43)))), ((int)(((byte)(55)))));
            this.panel1.Controls.Add(this.cikis);
            this.panel1.Controls.Add(this.ayar);
            this.panel1.Controls.Add(this.rapor);
            this.panel1.Controls.Add(this.dashboard);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(239, 626);
            this.panel1.TabIndex = 6;
            // 
            // cikis
            // 
            this.cikis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(0)))), ((int)(((byte)(10)))));
            this.cikis.FlatAppearance.BorderSize = 0;
            this.cikis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cikis.Font = new System.Drawing.Font("Century Gothic", 11F);
            this.cikis.ForeColor = System.Drawing.Color.White;
            this.cikis.Location = new System.Drawing.Point(0, 514);
            this.cikis.Name = "cikis";
            this.cikis.Size = new System.Drawing.Size(239, 109);
            this.cikis.TabIndex = 9;
            this.cikis.Text = "Çıkış";
            this.cikis.UseVisualStyleBackColor = false;
            this.cikis.Click += new System.EventHandler(this.cikis_Click);
            // 
            // ayar
            // 
            this.ayar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(51)))));
            this.ayar.FlatAppearance.BorderSize = 0;
            this.ayar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ayar.Font = new System.Drawing.Font("Century Gothic", 11F);
            this.ayar.ForeColor = System.Drawing.Color.White;
            this.ayar.Location = new System.Drawing.Point(0, 319);
            this.ayar.Name = "ayar";
            this.ayar.Size = new System.Drawing.Size(239, 109);
            this.ayar.TabIndex = 9;
            this.ayar.Text = "Ayarlar";
            this.ayar.UseVisualStyleBackColor = false;
            this.ayar.Click += new System.EventHandler(this.ayar_Click);
            // 
            // rapor
            // 
            this.rapor.FlatAppearance.BorderSize = 0;
            this.rapor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rapor.Font = new System.Drawing.Font("Century Gothic", 11F);
            this.rapor.ForeColor = System.Drawing.Color.White;
            this.rapor.Location = new System.Drawing.Point(0, 210);
            this.rapor.Name = "rapor";
            this.rapor.Size = new System.Drawing.Size(239, 109);
            this.rapor.TabIndex = 9;
            this.rapor.Text = "Rapor Al";
            this.rapor.UseVisualStyleBackColor = true;
            this.rapor.Click += new System.EventHandler(this.rapor_Click);
            // 
            // dashboard
            // 
            this.dashboard.FlatAppearance.BorderSize = 0;
            this.dashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dashboard.Font = new System.Drawing.Font("Century Gothic", 11F);
            this.dashboard.ForeColor = System.Drawing.Color.White;
            this.dashboard.Location = new System.Drawing.Point(0, 101);
            this.dashboard.Name = "dashboard";
            this.dashboard.Size = new System.Drawing.Size(239, 109);
            this.dashboard.TabIndex = 9;
            this.dashboard.Text = "İzleme Ekranı";
            this.dashboard.UseVisualStyleBackColor = true;
            this.dashboard.Click += new System.EventHandler(this.dashboard_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(239, 100);
            this.panel3.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.Location = new System.Drawing.Point(28, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 78);
            this.label3.TabIndex = 8;
            this.label3.Text = "<SK>";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(239, 375);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(751, 251);
            this.panel2.TabIndex = 7;
            // 
            // mini_panel_left
            // 
            this.mini_panel_left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.mini_panel_left.Location = new System.Drawing.Point(240, 101);
            this.mini_panel_left.Name = "mini_panel_left";
            this.mini_panel_left.Size = new System.Drawing.Size(12, 109);
            this.mini_panel_left.TabIndex = 8;
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(990, 626);
            this.Controls.Add(this.mini_panel_left);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.giris_Load);
            this.NI_Menu.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon SistemTepsisi;
        private System.Windows.Forms.ContextMenuStrip NI_Menu;
        private System.Windows.Forms.ToolStripMenuItem ayarlarToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button dashboard;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button ayar;
        private System.Windows.Forms.Button rapor;
        private System.Windows.Forms.Panel mini_panel_left;
        public System.Windows.Forms.Timer uygulama_kullanim_zamanlayicisi;
        public System.Windows.Forms.Timer genel_kullanim_zamanlayicisi;
        private System.Windows.Forms.Button cikis;
    }
}

