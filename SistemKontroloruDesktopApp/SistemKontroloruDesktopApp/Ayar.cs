using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SistemKontroloruDesktopApp
{
    public partial class Ayar : Form
    {
        public Ayar()
        {
            InitializeComponent();
        }
        public WCF_Servis.Sistem_Kontroloru_Servis_01Client Servis = new WCF_Servis.Sistem_Kontroloru_Servis_01Client();
        SQLiteConnection con = new SQLiteConnection("Data Source=locadb.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
        public kaynak_izleme metot = new kaynak_izleme();
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

        private void Ayar_Load(object sender, EventArgs e)
        {

            comboBox1.SelectedIndex = 0;
            try
            {
                SQLiteCommand cmd = new SQLiteCommand("Select count(id) as kayitsayisi From Current_User", con);
                con.Open();
                SQLiteDataReader da = cmd.ExecuteReader();
                da.Read();
                if (Convert.ToInt32(da["kayitsayisi"]) > 0)
                {
                   
                }
                else
                {

                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        private void Degistirbtn_Click(object sender, EventArgs e)
        {
            LoginMini.asama = 1;
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteConnection con = new SQLiteConnection("Data Source=locadb.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory");
            con.Open();
            if (comboBox1.SelectedItem.ToString() == "Durdur")
            {
                cmd.Connection = con;
                cmd.CommandText = "update Current_User set Status_=0";
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Tekrar Aktif Yapana Kadar Sistem Pasif Durumda Kalacak!");
                    
                    Application.Exit();
                    this.Hide();
                  
                }
                catch (Exception hata)
                {
                    MessageBox.Show("hata:" + hata.ToString());

                }
                finally
                {
                    con.Close();
                }
            }
            else if (comboBox1.SelectedItem.ToString() == "İptal Et")
            {
                cmd.Connection = con;
                cmd.CommandText = "delete from  Current_User";
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bilgisayar İzleme Ağından Çıkarıldı.Eski Kayıtlara Web Panelden Ulaşabilirsiniz!");
                    Application.Exit();
                    this.Hide();
                }
                catch (Exception hata)
                {

                    MessageBox.Show("hata:" + hata.ToString());

                }
                finally
                {
                    con.Close();
                }
            }
            con.Close();

        }
    }
}
