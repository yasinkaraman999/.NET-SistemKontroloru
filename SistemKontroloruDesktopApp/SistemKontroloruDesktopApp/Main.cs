using System;
using System.Windows.Forms;
using System.Threading;
using System.Data.SQLite;
using System.Data;
namespace SistemKontroloruDesktopApp
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

        }

        public WCF_Servis.Sistem_Kontroloru_Servis_01Client Servis = new WCF_Servis.Sistem_Kontroloru_Servis_01Client();
        SQLiteConnection con = new SQLiteConnection("Data Source=locadb.sqlite;Version=3;");
        public kaynak_izleme metot = new kaynak_izleme();
        

        private void giris_Load(object sender, EventArgs e)
        {

            if (metot.sqliteKontrol()&&metot.InternetKontrol())
            {

            }
            metot.sqliteKontrol();

            metot.InternetKontrol();
           




            //Thread th1 = new Thread(metot.genel_kullanim);
            //Thread th2 = new Thread(metot.uygulama_kullanim);
            //th1.Start();
            //th2.Start();

            // metot.genel_kullanim();
            // metot.uygulama_kullanim();

        }
       
        private void ayarlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginMini frmGiris = new LoginMini();
            SistemTepsisi.Visible = false;
            frmGiris.Show();
            
        }

        private void dashboard_Click(object sender, EventArgs e)
        {
            mini_panel_left.Height = dashboard.Height;
            mini_panel_left.Top = dashboard.Top;

        }

        private void rapor_Click(object sender, EventArgs e)
        {
            mini_panel_left.Height = rapor.Height;
            mini_panel_left.Top = rapor.Top;
        }

        private void ayar_Click(object sender, EventArgs e)
        {
        mini_panel_left.Height = ayar.Height;
            mini_panel_left.Top = ayar.Top;
        }
        private void genel_kullanim_zamanlayicisi_Tick(object sender, EventArgs e)
        {
            try
            {
                // GirisBtn.Text=  metot.genel_kullanim();
            }
            catch (Exception X)
            {

                MessageBox.Show(X.ToString());
            }


        }
        private void uygulama_kullanim_zamanlayicisi_Tick(object sender, EventArgs e)
        {
            try
            {
                metot.uygulama_kullanim();
            }
            catch (Exception X)
            {

                MessageBox.Show(X.ToString());
            }
        }

        int CurrentUserControl()
        {

            SQLiteCommand comm = new SQLiteCommand("Select *From Current_User", con);

            con.Open();
            SQLiteDataReader rd = comm.ExecuteReader();
            if (rd.Read())
            {
                con.Close();
                return 1;
            }
            else
            {
                con.Close();
                return 0;//KAYIT YOK
            }




        }

        private void cikis_Click(object sender, EventArgs e)
        {
            SistemTepsisi.Visible = true;
            SistemTepsisi.ShowBalloonTip(4000,"Sistem Kontrolörü","İzleme Başlatıldı.",ToolTipIcon.Info);
            this.Hide();

        }

        private void NI_Menu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}