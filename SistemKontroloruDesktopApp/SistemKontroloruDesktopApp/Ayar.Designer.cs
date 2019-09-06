namespace SistemKontroloruDesktopApp
{
    partial class Ayar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ayar));
            this.label1 = new System.Windows.Forms.Label();
            this.Degistirbtn = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 18F);
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "Durum";
            // 
            // Degistirbtn
            // 
            this.Degistirbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Degistirbtn.Location = new System.Drawing.Point(328, 37);
            this.Degistirbtn.Name = "Degistirbtn";
            this.Degistirbtn.Size = new System.Drawing.Size(119, 39);
            this.Degistirbtn.TabIndex = 3;
            this.Degistirbtn.Text = "Değiştir";
            this.Degistirbtn.UseVisualStyleBackColor = true;
            this.Degistirbtn.Click += new System.EventHandler(this.Degistirbtn_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Durdur",
            "İptal Et"});
            this.comboBox1.Location = new System.Drawing.Point(121, 42);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(189, 29);
            this.comboBox1.TabIndex = 4;
            // 
            // Ayar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(459, 109);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.Degistirbtn);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Ayar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ayar";
            this.Load += new System.EventHandler(this.Ayar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Degistirbtn;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}