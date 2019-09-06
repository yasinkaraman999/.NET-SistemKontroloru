using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Accord.Video.FFMPEG;
using ScreenRecord;
using Shell32;
using System.Runtime.InteropServices; //bg

namespace SistemKontroloruDesktopApp
{
    public partial class LoginMini : Form
    {

        ScreenRecorder screenRec = new ScreenRecorder(new Rectangle(), "");
        //Arkaplan Bileşenleri
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 action, UInt32 uParam, String vParam, UInt32 winIni);

        public LoginMini()
        {
            InitializeComponent();
        }


        WCF_Servis.Sistem_Kontroloru_Servis_01Client Servis = new WCF_Servis.Sistem_Kontroloru_Servis_01Client();
        kaynak_izleme kaynak = new kaynak_izleme();
        Ayar ayarfrm = new Ayar();
        double running_Time_Sayac = 0;
        // public SQLiteConnection con = new SQLiteConnection("Data Source=locadb.sqlite;Count Changes=off;Journal Mode=off;Pooling=true;Cache Size=10000;Page Size=4096;Synchronous=off");

        bool goster = true;
        public static int asama = 1;
        public static string video_Name = "";
        Thread genel_kull;
        Thread uygulama_kull;
        string fullName = "";
        public static string hardwareId = "";
        string uygulama_adi_ = "";//Uzak Program Kurulumu için Uygulama adı
        bool internet_durum;
        //-------------------------------------FORM LOAD-------------------------------------------------//

        public void LoginMini_Load(object sender, EventArgs e)
        {

            if (kaynak.sqliteKontrol())//Sqlite Veri Tabanları Kontrol Ediliyor!
            {
                //Gün Eşitleniyor
                using (SQLiteConnection con = new SQLiteConnection("Data Source=RunningTime.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
                {
                    SQLiteCommand cmd = new SQLiteCommand("Select * From RunningTime", con);
                    con.Open();
                    SQLiteDataReader da = cmd.ExecuteReader();
                    if (da.Read())
                    {
                        if (Convert.ToInt32(da["Day_"].ToString()) != DateTime.Now.Day)
                        {
                            try
                            {
                                SQLiteCommand banned_app_add_command = new SQLiteCommand("update  RunningTime set Used_Time=0,Day_=" + DateTime.Now.Day + "", con);
                                banned_app_add_command.ExecuteNonQuery();
                            }
                            catch (Exception X)
                            {
                                //hatamesajıı servise gonderilecek.MES
                                MessageBox.Show(X.Message);
                            }
                        }

                    }
                }
                //Running Time Başlatılıyor | Bilgisayar Açık Kalma Süresi Toplanıyor
                Running_Time_Timer.Start();
                //Servisleri Başlat
                using (SQLiteConnection con = new SQLiteConnection("Data Source=locadb.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
                {

                    SQLiteCommand cmd = new SQLiteCommand("Select Hardware_Id From Current_User", con);
                    con.Open();
                    SQLiteDataReader da = cmd.ExecuteReader();
                    if (da.Read())
                    {
                        hardwareId = Convert.ToString(da["Hardware_Id"]);
                        con.Close();
                        if (kaynak.InternetKontrol())
                        {
                            try
                            {
                                if (Servis.Servis_Kontrol())
                                {
                                    kullanimlari_esitle();
                                    Banned_App_Timer.Start();
                                    isKontrol.Start();//İş emirleri Başlatılıyor
                                    last_login_check();
                                    kaynak.information(hardwareId);
                                }
                            }
                            catch (Exception)
                            {
                                Application.Restart();
                            }
                        }
                        else
                        {
                            internet_durum = false;
                            SistemTepsisi.Visible = true;
                            SistemTepsisi.ShowBalloonTip(10000, "Sistem Kontrolörü - Kontrol", "İnternet bağlantısı kurulamadı.", ToolTipIcon.Warning);
                            // offline_last_login_check();   //gerek kalmadı.
                            Banned_App_Timer.Start();
                            kullanim_gonder_thread_baslat();

                        }
                    }
                    else
                    {
                        //giris ekranı geliyor
                        SistemTepsisi.Visible = true;
                        SistemTepsisi.ShowBalloonTip(5000, "Sistem Kontrolörü - Giriş", "Kayıtlı kullanıcı yok .Lütfen giriş yapınız.", ToolTipIcon.Info);
                    }
                }
            }
        }

        //-----------------------------------------İnternetsiz Giriş Sonrası Kontrol---------------------------------------------//
        void offline_last_login_check()
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=locadb.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
            {
                try
                {
                    SQLiteCommand cmd = new SQLiteCommand("Select count(id) as kayitsayisi,Hardware_Id From Current_User", con);
                    con.Open();
                    SQLiteDataReader da = cmd.ExecuteReader();
                    da.Read();
                    if (Convert.ToInt32(da["kayitsayisi"]) > 0)
                    {
                        //izleme başlatılır locak dbye yazılır
                        MessageBox.Show("uye var");
                    }
                    else
                    {
                        MessageBox.Show("uye yok");
                        //giriş ekranı getirilir

                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show("SQL HATASI" + ee.ToString());
                }
                finally
                {
                    con.Close();
                }
            }


        }

        //-----------------------------------------Giriş Sonrası Kontrol---------------------------------------------//
        void last_login_check()
        {

            using (SQLiteConnection con = new SQLiteConnection("Data Source=locadb.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
            {
                try
                {
                    SQLiteCommand cmd = new SQLiteCommand("Select count(id) as kayitsayisi,Hardware_Id From Current_User", con);
                    con.Open();
                    SQLiteDataReader da = cmd.ExecuteReader();
                    da.Read();
                    if (Convert.ToInt32(da["kayitsayisi"]) > 0)
                    {
                        //Servise bilgiler gonderilecek kullanıcı check edilip online kısmı 1 yapılacak geriye gelen stringe gore işlem devam edecek
                        //Form icon olacak anaform gizlenecek
                        try
                        {
                            if (Servis.girisSonrasiKontrol(da["Hardware_Id"].ToString()) == "Ok")
                            {
                                calisan_uyg_gonder(da["Hardware_Id"].ToString());
                                Calisan_uygulama_gonder.Start();
                                kullanim_gonder_thread_baslat();
                                isKontrol.Start();

                                SistemTepsisi.Visible = true;
                                SistemTepsisi.ShowBalloonTip(4000, "Sistem Kontrolörü", "İzleme Başlatıldı.", ToolTipIcon.Info);
                                goster = false;
                                //yoksa giriş ekranı gelir
                            }
                            else
                            {
                                //giriş ekranı getirilir
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Servise bağlanamıyorum :(\n" + "\n" + e.Message);
                        }
                    }
                    da.Close();
                    cmd.Dispose();
                    con.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show("SQL HATASI" + ee.ToString());
                }
                finally
                {
                    con.Close();

                }
            }


        }

        //-----------------------------------------Btn_Giris_Click---------------------------------------------//
        public void Btn_Giris_Click(object sender, EventArgs e)
        {
            hardwareId = kaynak.hardwareid() + txtMetro_Ad.Text;
            if (kaynak.InternetKontrol())
            {
                if (asama == 1)
                {
                    //servise bilgiler gonderilir
                    //serviste kontrol yapılır 
                    //kullanıcı varsa once pc detail varmı kontrol edilir yoksa webdbye eklenır olumlu sonuc doner
                    //gelen sonuc ile locadbye current user eklenır-icon acılıp form gızlenır sısten baslar yada kullanıcı yok hata verdirilir
                    string check = Servis.giris(txtMetro_Ad.Text, txtMetro_Pass.Text, hardwareId);
                    if (check == "Normal_Giris")
                    {
                        MessageBox.Show("Normal Giriş : Başarılı");
                        GirisSonrasiLokalKayit(kaynak.hardwareid() + txtMetro_Ad.Text, DateTime.Now);
                        //SİSTEM TEPSİSİNE İNECEK
                        SistemTepsisi.Visible = true;
                        SistemTepsisi.ShowBalloonTip(4000, "Sistem Kontrolörü", "İzleme Başlatıldı.", ToolTipIcon.Info);
                        //ayarformu açılıyor
                        Banned_App_Timer.Start();
                        isKontrol.Start();//İş emirleri Başlatılıyor
                        last_login_check();
                        Hide();
                    }
                    else if (check == "Pc_Eklendi")
                    {
                        kaynak.information(hardwareId);
                        SistemTepsisi.Visible = true;
                        SistemTepsisi.ShowBalloonTip(4000, "Sistem Kontrolörü - Yeni Kayıt", "Kullanıcı başarı ile eklendi. Sistem başlatıldı.", ToolTipIcon.Info);
                        GirisSonrasiLokalKayit(hardwareId, DateTime.Now);
                        //SİSTEM TEPSİSİNE İNECEK
                        SistemTepsisi.Visible = true;
                        SistemTepsisi.ShowBalloonTip(4000, "Sistem Kontrolörü", "İzleme Başlatıldı.", ToolTipIcon.Info);
                        Banned_App_Timer.Start();
                        isKontrol.Start();//İş emirleri Başlatılıyor
                        last_login_check();
                        Hide();
                        //LASTLOGİN ÇALISACAK
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı Bulunamadı.Tekrar Deneyiniz!");
                    }

                }
                else if (asama == 2)
                {
                    string check = Servis.ayarKontrol(txtMetro_Ad.Text, txtMetro_Pass.Text, (kaynak.hardwareid() + txtMetro_Ad.Text));
                    if (check == "AyarAc")
                    {
                        ayarfrm.Show();
                    }
                    else if (check == "PcEslesmedi")
                    {
                        MessageBox.Show("Girilen Kullanıcı Yetkili Değil!");
                    }
                    else if (check == "Bulunamadi")
                    {
                        MessageBox.Show("Kullanıcı Adı veya Şifre yanlış!");
                    }
                }

            }
            else
            {
                MessageBox.Show("İlk kurulum için internet bağlantısı gereklidir!");
                Thread.Sleep(5000);
                Application.Exit();
            }

        }

        //-----------------------------------------Giriş Sonraso Local Kayıt---------------------------------------------//
        void GirisSonrasiLokalKayit(string hardwareid, DateTime datetime_)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=locadb.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
            {
                SQLiteCommand sil = new SQLiteCommand("delete from Current_User", con);
                SQLiteCommand ekle_ = new SQLiteCommand("insert into Current_User(Hardware_Id,Status_,Datetime_) values ('" + hardwareid + "',1,'" + datetime_ + "')", con);
                con.Open();
                try
                {
                    sil.ExecuteNonQuery();
                    ekle_.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    SistemTepsisi.ShowBalloonTip(4000, "Sistem Kontrolörü - Hata", "SQLite  Düzgün Çalışmadı.", ToolTipIcon.Error);
                    Thread.Sleep(4000);
                    Application.Restart();
                }
                finally
                {
                    con.Close();
                }
            }

        }

        private void ayarlarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            asama = 2;
            goster = true;
            Show();
        }
        private void LoginMini_Activated(object sender, EventArgs e)
        {
            if (goster == false)
            {
                Hide();
            }
        }

        private void isKontrol_Tick(object sender, EventArgs e)
        {

            isKontrol.Stop();


            foreach (var x in Servis.isEmri(hardwareId).ToList())//try catch eklenecek
            {

                switch (x.Work_)
                {
                    case "Screen_Capture":
                        try
                        {
                            //goruntu alınır 
                            string yoll = "temp/ss/" + hardwareId + DateTime.Now.Millisecond.ToString() + ".jpg"; //dosya yolu
                            kaynak.Screenshot().Save(yoll, ImageFormat.Jpeg);

                            //ftp ile yolla
                            kaynak.ftp_gonder(yoll);
                            
                            Servis.isEmri_Kapat(hardwareId, "Screen_Capture", yoll, "");
                        }
                        catch (Exception)
                        {
                            //hata mesajı gonderilecek
                        }
                        break;
                    case "Screen_Record":
                        Rectangle bounds = Screen.FromControl(this).Bounds;
                        screenRec = new ScreenRecorder(bounds, "temp//sr");
                        RecordStop.Start();
                        VideoRecord.Start();//30 sn sayar
                                            
                        break;
                    case "App_Run":
                        try
                        {
                            Process.Start(x.Query_);
                            Servis.isEmri_Kapat(hardwareId, "App_Run", fullName, "");
                        }
                        catch (Exception)
                        {

                          
                        }
                        break;
                    case "App_Stop":
                        try
                        {
                            var processName = x.Query_;
                            Process[] processes = Process.GetProcessesByName(processName.ToString());
                            foreach (Process process in processes)
                            {
                                process.Kill();
                            }

                            //  MessageBox.Show("Browser_History Running");
                            Servis.isEmri_Kapat(hardwareId, "App_Stop", processName.ToString(), "");
                        }
                        catch (Exception)
                        {

                            //Uygulama Bulunamadı Hatası dondurulecek
                        }


                        break;
                    case "Browser_Histories":
                        try
                        {
                            //Tarayıcı Geçmişi Yollanıyor

                            // Temp Siliniyor
                            var dosyalar = Directory.GetFiles(Application.StartupPath + "\\temp\\XmlData\\browser_histories\\sqlite");
                            foreach (var item in dosyalar)
                                try
                                {
                                    File.Delete(item);
                                }
                                catch (Exception)
                                {
                                }
                            // Temp Siliniyor
                            //Geçmiş Alınıyor
                            string google = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\History";
                            string fileName = DateTime.Now.Ticks.ToString();
                            File.Copy(google, Application.StartupPath + "\\temp\\XmlData\\browser_histories\\sqlite\\" + fileName);
                            string b_history_yol = "temp/XmlData/browser_histories/xml/" + hardwareId + DateTime.Now.Millisecond.ToString() + ".xml";
                            //Geçmiş Alınıyor
                            using (SQLiteConnection con_xml = new SQLiteConnection("DataSource = " + Application.StartupPath + "\\temp\\XmlData\\browser_histories\\sqlite\\" + fileName + ";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;"))
                            {
                                con_xml.Open();
                                SQLiteDataAdapter da_b_h = new SQLiteDataAdapter("select url,title,last_visit_time from urls ", con_xml);
                                // SQLiteDataAdapter da = new SQLiteDataAdapter("select * from urls order by last_visit_time desc", con);
                                DataTable ds = new DataTable();
                                da_b_h.Fill(ds);
                                ds.TableName = "x";
                                ds.WriteXml(b_history_yol);
                                con_xml.Close();
                            geri:
                                try
                                {

                                    kaynak.ftp_gonder(b_history_yol);
                                    Servis.isEmri_Kapat(hardwareId, "Browser_Histories", b_history_yol, "");
                                }
                                catch (Exception xe)
                                {
                                    MessageBox.Show(xe.Message);
                                    goto geri;
                                }

                                ////////////////////////////////////////
                            }
                            //İşlem Tamamlandı.Dosya Ftp ile aktarılıp tabloya bilgisi eklendi
                        }
                        catch (Exception)
                        {
                            //hata mesajı dondurulecek
                        }
                        break;
                    case "Cmd":
                        if (x.Query_ != "" && x.Query_ != null)
                        {
                            try
                            {
                                Process p = new Process();
                                p.StartInfo.FileName = "cmd";
                                p.StartInfo.Arguments = "/c " + x.Query_;
                                p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                                p.StartInfo.CreateNoWindow = true;
                                p.StartInfo.RedirectStandardOutput = true;
                                p.StartInfo.UseShellExecute = false;
                                p.Start();
                                Servis.isEmri_Kapat(hardwareId, "Cmd", x.Query_, p.StandardOutput.ReadToEnd().ToString());
                            }
                            catch (Exception)
                            {

                            }

                        }
                        // MessageBox.Show("Secrenn Capture Running");
                        break;
                    case "Banned_App_Add":
                        using (SQLiteConnection con = new SQLiteConnection("Data Source=Banned_App.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
                        {
                            try
                            {
                                con.Open();
                                SQLiteCommand banned_app_add_command = new SQLiteCommand("insert into Banned_App(Hardware_Id,P_Name,Limit_,Used_Time,Day_,Status_,Datetime_) values ('" + hardwareId + "','" + x.Query_ + "'," + x.Temp_ + ",0," + DateTime.Now.Day + ",1,'" + DateTime.Now + "')", con);
                                banned_app_add_command.ExecuteNonQuery();
                                con.Close();
                                Servis.isEmri_Kapat(hardwareId, "Banned_App_Add", x.Query_, "");
                            }
                            catch (Exception X)
                            {
                                //hatamesajıı servise gonderilecek.MES
                                MessageBox.Show(X.Message);
                            }
                        }
                        break;
                    case "Banned_App_Remove":
                        using (SQLiteConnection con = new SQLiteConnection("Data Source=Banned_App.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
                        {
                            try
                            {
                                con.Open();
                                SQLiteCommand banned_app_remove_command = new SQLiteCommand("DELETE FROM Banned_App WHERE Hardware_Id='" + hardwareId + "' and P_Name='" + x.Query_ + "'", con);
                                banned_app_remove_command.ExecuteNonQuery();
                                con.Close();
                                Servis.isEmri_Kapat(hardwareId, "Banned_App_Remove", x.Query_, "");
                            }
                            catch (Exception)
                            {
                                //hatamesajıı servise gonderilecek.
                            }
                        }
                        break;
                    case "Wallpaper_Change":
                        using (SQLiteConnection con = new SQLiteConnection("Data Source=Banned_App.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
                        {
                            try
                            {
                                kaynak.ftp_al(x.Query_);
                                //Arkaplan Ayarlanıyor
                                string arkaplan_full_yol = Application.StartupPath + @"\temp\bg\" + x.Query_;
                                SystemParametersInfo(0x14, 0, arkaplan_full_yol, 0x01 | 0x02);
                                Servis.isEmri_Kapat(hardwareId, "Wallpaper_Change", x.Query_, "");
                            }
                            catch (Exception)
                            {
                                //hatamesajıı servise gonderilecek.
                            }
                        }
                        break;
                    case "Time_Limit":
                        using (SQLiteConnection con = new SQLiteConnection("Data Source=RunningTime.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
                        {
                            try
                            {
                                con.Open();
                                SQLiteCommand time_limit_cmd = new SQLiteCommand("update  RunningTime set Time_Limit=" + x.Query_ + "", con);
                                time_limit_cmd.ExecuteNonQuery();
                                con.Close();

                                Servis.isEmri_Kapat(hardwareId, "Time_Limit", "", "");
                            }
                            catch (Exception)
                            {
                                //hatamesajıı servise gonderilecek.
                            }
                        }
                        break;
                    case "Alert_Message":
                        SistemTepsisi.Visible = true;
                        SistemTepsisi.ShowBalloonTip(10000, "Sistem Kontrolörü", "Genel Mesaj!  Yönetici:" + x.Query_, ToolTipIcon.Info);
                        Servis.isEmri_Kapat(hardwareId, "Alert_Message", "", "");
                        break;
                    case "Install_App":
                        //Theread Üzerinde  dosya ftp ile alınıyor ve cmd ile kuruluyor.
                        uygulama_adi_ = x.Query_;
                        Thread uygulama_kur_thread = new Thread(new ThreadStart(program_kur));
                        uygulama_kur_thread.Start();
                        SistemTepsisi.Visible = true;
                        SistemTepsisi.ShowBalloonTip(10000, "Sistem Kontrolörü - Program Yükleyici", x.Query_ + " Yükleniyor... ", ToolTipIcon.Info);
                        Servis.isEmri_Kapat(hardwareId, "Install_App", "", "");
                        break;
                    default:
                        break;
                }
            }


            isKontrol.Start();
        }

        private void VideoRecord_Tick(object sender, EventArgs e)
        {
            screenRec.RecordVideo();

        }
        private void RecordStop_Tick(object sender, EventArgs e)
        {
            VideoRecord.Stop();
            string video_adi = hardwareId + DateTime.Now.Millisecond.ToString() + ".mp4";
            screenRec.Stop(video_adi);
            RecordStop.Stop();
        //------------------FTP GONDER----------------//

        tekrar_gonder:
            try
            {
                string local_dosya = "temp/sr/" + video_adi;
                //ftp ile yolla
                kaynak.ftp_gonder(local_dosya);
                Servis.isEmri_Kapat(hardwareId, "Screen_Record", local_dosya, "");
            }
            catch (Exception)
            {

                goto tekrar_gonder;
            }
        }
        private void Banned_App_Timer_Tick(object sender, EventArgs e)
        {

            List<string> calisan_uygulamalar = new List<string>();
            Process[] Memory = Process.GetProcesses();
            foreach (Process prc in Memory)
            {
                if (!String.IsNullOrEmpty(prc.MainWindowTitle))
                {
                    calisan_uygulamalar.Add(prc.ProcessName);
                }
            }

            using (SQLiteConnection con = new SQLiteConnection("Data Source=Banned_App.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
            {
                con.Open();
                foreach (var pname_ in calisan_uygulamalar)
                {
                    SQLiteDataReader esleme_reader = new SQLiteCommand("Select * From Banned_App where Hardware_Id='" + hardwareId + "' and P_Name='"+pname_+"'", con).ExecuteReader();
                    if (esleme_reader.Read())
                    {
                        //kullanılan sure ekleniyor(açık olan uygulama listesi alınacak ve sadece onlara ekleme yapılacak.)
                        SQLiteCommand banned_app_time_add = new SQLiteCommand("update Banned_App set Used_Time=Used_Time+" + 5 + " where Limit_> Used_Time and P_Name='" + pname_ + "'", con);
                        banned_app_time_add.ExecuteNonQuery();
                    }


                }
                con.Close();
            }

            using (SQLiteConnection con = new SQLiteConnection("Data Source=Banned_App.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
            {
                con.Open();
                SQLiteCommand banned_app_command = new SQLiteCommand("Select * From Banned_App where Hardware_Id='" + hardwareId + "'", con);
                SQLiteDataReader data_r = banned_app_command.ExecuteReader();
                while (data_r.Read())
                {
                    //Gün Kontrolu
                    if (Convert.ToInt32(data_r["Day_"]) != DateTime.Now.Day)
                    {
                        SQLiteCommand banned_app_Day_Check = new SQLiteCommand("update Banned_App set Day_=" + DateTime.Now.Day + " , Used_Time=0 where Limit_>=-1", con);
                        banned_app_Day_Check.ExecuteNonQuery();
                    }
                    //Limit konulan uygulamalar ayrılıyor digerler kapatılıyor
                    if (Convert.ToInt32(data_r["Limit_"].ToString()) > -1)
                    {
                        if (Convert.ToInt32(data_r["Limit_"].ToString()) <= Convert.ToInt32(data_r["Used_Time"].ToString()))
                        {
                            var processName = data_r["P_Name"].ToString();
                            Process[] processes = Process.GetProcessesByName(processName.ToString());
                            foreach (Process process in processes)
                            {
                                process.Kill();
                                SistemTepsisi.Visible = true;
                                SistemTepsisi.ShowBalloonTip(4000, "Sistem Kontrolörü", "Uygulama Kullanım Süresi Dolmuştur. Kod:" + data_r["P_Name"].ToString() + "", ToolTipIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        //-1 ise program tamamen engellenmiştir.
                        var processName = data_r["P_Name"].ToString();
                        Process[] processes = Process.GetProcessesByName(processName.ToString());
                        foreach (Process process in processes)
                        {
                            process.Kill();
                            SistemTepsisi.Visible = true;
                            SistemTepsisi.ShowBalloonTip(4000, "Sistem Kontrolörü", "Yasaklı Uygulama Kapatıldı. Kod:" + data_r["P_Name"].ToString() + "", ToolTipIcon.Error);
                        }
                    }

                }

            }
        }
        private void Running_Time_Timer_Tick(object sender, EventArgs e)
        {
            running_Time_Sayac += 5;
            double limit_ = 0;
            if (running_Time_Sayac == 20)
            {
                //db kayıt
                using (SQLiteConnection con = new SQLiteConnection("Data Source=RunningTime.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
                {
                    try
                    {
                        double used_new = 0;
                        con.Open();
                        SQLiteCommand used_time_cmd = new SQLiteCommand("Select * From RunningTime", con);
                        SQLiteDataReader da = used_time_cmd.ExecuteReader();
                        if (da.Read())
                        {
                            used_new = Convert.ToDouble(da["Used_Time"].ToString()) + running_Time_Sayac;
                            limit_ = Convert.ToDouble(da["Time_Limit"].ToString());
                        }
                        con.Close();

                        con.Open();
                        SQLiteCommand banned_app_add_command = new SQLiteCommand("update  RunningTime set Used_Time=" + used_new + "", con);
                        banned_app_add_command.ExecuteNonQuery();
                        con.Close();
                        if (used_new >= limit_)
                        {
                            //cmd shutdown komutu yazılacak
                        }

                    }
                    catch (Exception X)
                    {
                        //hatamesajıı servise gonderilecek.MES
                        MessageBox.Show(X.Message);
                    }
                }
                running_Time_Sayac = 0;

            }

        }
        public void program_kur()
        {
            //ftp ile program alınıyor
            string ftpfullpath = "ftp://" + "localhost:1111/" + "temp/app/" + uygulama_adi_;
            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential("sistemkontrolor", "By.yasin4141");
                byte[] fileData = request.DownloadData(ftpfullpath);

                using (FileStream file = File.Create("temp/app/" + uygulama_adi_))
                {
                    file.Write(fileData, 0, fileData.Length);
                    file.Close();
                }
            }

            //cmd ile program kuruluyor.
            Process.Start((Application.StartupPath + @"\\temp\\app\\" + uygulama_adi_));
            Servis.isEmri_Kapat(hardwareId, "Install_App", "", "");
        }
        public void calisan_uyg_gonder(string hwid)
        {
            //uygulamalar ve yolları alınıyor
            DataTable uyg_tablo = new DataTable();
            uyg_tablo.Columns.Add("uyg_dizin");
            uyg_tablo.Columns.Add("uyg_adi");
            uyg_tablo.Columns.Add("uyg_p_name");

            //List<string> data = new List<string>();
            //List<string> data2 = new List<string>();

            string dizin = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs";
            string[] dizindekiKlasorler = Directory.GetDirectories(dizin);
            string[] dizindekiDosyalar = Directory.GetFiles(dizin);
            Shell shell = new Shell();
            Folder folder;
            FolderItem folderItem;

            Shell32.ShellLinkObject link;
            foreach (string dosya in dizindekiDosyalar)
            {
                FileInfo fileInfo2 = new FileInfo(dosya);
                string dosyaAdi = fileInfo2.Name;
                long byteBoyut = fileInfo2.Length;
                DateTime olsTarihi2 = fileInfo2.CreationTime;
                ///////////////////////PROCESS NAME ALINIYOR//////////////////////////

                if (dosyaAdi != "Immersive Control Panel.lnk")
                {
                    string pathOnly = System.IO.Path.GetDirectoryName(dosya);
                    string filenameOnly = System.IO.Path.GetFileName(dosya);
                    string process_name_ = "";
                    folder = shell.NameSpace(pathOnly);
                    folderItem = folder.ParseName(filenameOnly);
                    if (folderItem != null && fileInfo2.Extension == ".lnk")
                    {
                        link = (ShellLinkObject)folderItem.GetLink;
                        process_name_ = link.Target.Name;
                        if (process_name_.Length >= 5)
                        {
                            process_name_ = process_name_.Substring(0, process_name_.Length - 4);
                        }

                        uyg_tablo.Rows.Add(dosya.ToString(), dosyaAdi.ToString(), process_name_);
                    }
                }
            }
            foreach (string klasor in dizindekiKlasorler)
            {
                string[] dizindekidosyalar2 = Directory.GetFiles(klasor);
                foreach (string dosya in dizindekidosyalar2)
                {
                    FileInfo fileInfo2 = new FileInfo(dosya);
                    string dosyaAdi = fileInfo2.Name;
                    long byteBoyut = fileInfo2.Length;
                    DateTime olsTarihi2 = fileInfo2.CreationTime;
                    ///////////////////////PROCESS NAME ALINIYOR//////////////////////////
                    if (dosyaAdi != "Immersive Control Panel.lnk")
                    {
                        string pathOnly = System.IO.Path.GetDirectoryName(dosya);
                        string filenameOnly = System.IO.Path.GetFileName(dosya);
                        string process_name_ = "";

                        folder = shell.NameSpace(pathOnly);
                        folderItem = folder.ParseName(filenameOnly);
                        if (folderItem != null && fileInfo2.Extension == ".lnk")
                        {
                            link = (Shell32.ShellLinkObject)folderItem.GetLink;
                            process_name_ = link.Target.Name;
                            if (process_name_.Length >= 5)
                            {
                                process_name_ = process_name_.Substring(0, process_name_.Length - 4);
                            }
                            uyg_tablo.Rows.Add(dosya.ToString(), dosyaAdi.ToString(), process_name_);
                        }

                    }
                }
            }
            uyg_tablo.TableName = "deneme";
            Servis.uygulama_listesi_gonder(hwid, uyg_tablo);//tamamlandı.
                                                            //Uygulama Adları Toplanıyor
            DataTable uyg_calisan = new DataTable();
            uyg_calisan.Columns.Add("Name_");
            uyg_calisan.Columns.Add("Title_");
            uyg_calisan.TableName = "uyg_calisan";

            Process[] Memory = Process.GetProcesses();
            foreach (Process prc in Memory)
            {
                if (!String.IsNullOrEmpty(prc.MainWindowTitle))
                {
                    uyg_calisan.Rows.Add(prc.ProcessName, prc.MainWindowTitle);
                }
            }

            Servis.calisan_uygulama_gonder(hwid, uyg_calisan);

        }
        public void kullanim_gonder_thread_baslat()
        {
            genel_kull = new Thread(new ThreadStart(kaynak.genel_kullanim));
            uygulama_kull = new Thread(new ThreadStart(kaynak.uygulama_kullanim));

            genel_kull.Start();//thread fonk. çalışır
            uygulama_kull.Start();//thread fonk. çalışır
        }

        private void Internet_Kontrol_Tick(object sender, EventArgs e)
        {

        }

        public void kullanimlari_esitle()
        {
            if (kaynak.InternetKontrol())
            {
                DataTable sys_table = new DataTable();
                DataTable app_table = new DataTable();
                using (SQLiteConnection con = new SQLiteConnection("Data Source=System_Usage.sqlite;Version=3;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;"))
                {
                    //--------------------------------------------------------------------------------------//
                    con.Open();
                    //veriler sunucuya aktarılıyor.
                    SQLiteDataAdapter sys_data_adapter = new SQLiteDataAdapter("select * from System_Usage where Hardware_Id='" + hardwareId.ToString() + "'", con);

                    sys_table.TableName = "sys_usage_table";
                    sys_data_adapter.Fill(sys_table);
                    //eskileri sil
                    SQLiteCommand sys_sil_cmd = new SQLiteCommand("delete from System_Usage where Hardware_Id='" + hardwareId.ToString() + "'", con);
                    con.Close();
                }
                using (SQLiteConnection con = new SQLiteConnection("Data Source=App_Usage.sqlite;Version=3;"))
                {
                    //--------------------------------------------------------------------------------------//
                    con.Open();
                    //veriler sunucuya aktarılıyor.
                    SQLiteDataAdapter da = new SQLiteDataAdapter("select * from App_Usage where Hardware_Id='" + hardwareId.ToString() + "'", con);

                    app_table.TableName = "app_table";
                    da.Fill(app_table);
                    con.Close();
                }
                //servise gonder
                Servis.kullanimlari_esitle(hardwareId, sys_table, app_table);
            }
        }

        private void Calisan_uygulama_gonder_Tick(object sender, EventArgs e)
        {
            calisan_uyg_gonder(hardwareId);
        }
    }
}
