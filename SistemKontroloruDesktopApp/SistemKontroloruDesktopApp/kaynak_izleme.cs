using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;
using System.Data.SQLite;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.Data;
using System.Management;
using SistemKontroloruDesktopApp.Model;

namespace SistemKontroloruDesktopApp
{
    public class kaynak_izleme
    {

        // ServiceReference1.Sistem_Kontroloru_Servis_01Client Servis = new ServiceReference1.Sistem_Kontroloru_Servis_01Client();
        //--------------------------------------------------------------------------------------//
        WCF_Servis.Sistem_Kontroloru_Servis_01Client Servis = new WCF_Servis.Sistem_Kontroloru_Servis_01Client();
        //--------------------------------------------------------------------------------------//
        //--------------------------------------------------------------------------------------//
        #region genelkullanımSayacları
        static PerformanceCounter total_cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        static PerformanceCounter total_ram = new PerformanceCounter("Paging File", "% Usage", "_Total");

        //-------------------Network İzleme(İnterface Belirtilmek Zorunda)-------------------//
        static PerformanceCounterCategory pcg = new PerformanceCounterCategory("Network Interface");
        static string instance = pcg.GetInstanceNames()[1];
        static PerformanceCounter pcsent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
        static PerformanceCounter pcreceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
        //-------------------Network İzleme(İnterface Belirtilmek Zorunda)-------------------//
        #endregion genelkullanımSayacları
        //--------------------------------------------------------------------------------------//
        #region uygulamabazliSayaclar
        //static Process[] processes = Process.GetProcesses();
        public static DateTime lastTime;
        public static TimeSpan lastTotalProcessorTime;
        public static DateTime curTime;
        public static TimeSpan curTotalProcessorTime;
        public double CPUUsage;
        #endregion uygulamabazliSayaclar
        //--------------------------------------------------------------------------------------//
        #region sqliteKontrol
        public bool sqliteKontrol()
        {


            if (!File.Exists("locadb.sqlite") || !File.Exists("Banned_App.sqlite") || !File.Exists("RunningTime.sqlite") || !File.Exists("System_Usage.sqlite") || !File.Exists("App_Usage.sqlite"))
            {
                if (!File.Exists("locadb.sqlite"))
                {
                    try
                    {
                        SQLiteConnection.CreateFile("locadb.sqlite");
                        using (SQLiteConnection con = new SQLiteConnection("Data Source=locadb.sqlite;Version=3;"))
                        {
                            //--------------------------------------------------------------------------------------//
                            con.Open();
                            //--------------------------------------------------------------------------------------//
                            string Current_User = @"CREATE TABLE Current_User (
                                        [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                        [User_Id] VARCHAR(100) NULL,
                                        [Hardware_Id] VARCHAR(100) NULL, 
                                        [Status_] BIT NULL,
                                        [Datetime_]
                                            DATETIME NULL
                                    );";
                            SQLiteCommand cmd3 = new SQLiteCommand(Current_User, con);
                            cmd3.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                }
                if (!File.Exists("Banned_App.sqlite"))
                {
                    try
                    {
                        SQLiteConnection.CreateFile("Banned_App.sqlite");
                        using (SQLiteConnection con = new SQLiteConnection("Data Source=Banned_App.sqlite;Version=3;"))
                        {
                            //--------------------------------------------------------------------------------------//
                            con.Open();
                            //--------------------------------------------------------------------------------------//
                            string Banned_App = @"CREATE TABLE Banned_App (
                                        [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                        [Hardware_Id] VARCHAR(100) NULL,
                                        [P_Name] VARCHAR(300) NULL, 
                                        [Limit_] INTEGER NULL, 
                                        [Used_Time] INTEGER NULL, 
                                        [Day_] INTEGER NULL, 
                                        [Status_] BIT NULL,
                                        [Datetime_]
                                            DATETIME NULL
                                       
                                    );";
                            SQLiteCommand cmd4 = new SQLiteCommand(Banned_App, con);
                            cmd4.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                }
                if (!File.Exists("RunningTime.sqlite"))
                {
                    try
                    {

                        SQLiteConnection.CreateFile("RunningTime.sqlite");
                        using (SQLiteConnection con = new SQLiteConnection("Data Source=RunningTime.sqlite;Version=3;"))
                        {

                            //--------------------------------------------------------------------------------------//
                            con.Open();
                            //--------------------------------------------------------------------------------------//
                            int day_ = (int)DateTime.Now.Day;
                            string Banned_App = @"CREATE TABLE RunningTime (
                                        [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                        [Time_Limit] INTEGER NULL,
                                        [Used_Time] INTEGER NULL,
                                        [Day_] INTEGER NULL
                                    );
                                  insert into RunningTime(Time_Limit,Used_Time,Day_)values(999999999,0," + day_ + ");";
                            SQLiteCommand cmd4 = new SQLiteCommand(Banned_App, con);
                            cmd4.ExecuteNonQuery();

                            con.Close();
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                }
                if (!File.Exists("System_Usage.sqlite"))
                {
                    try
                    {
                        SQLiteConnection.CreateFile("System_Usage.sqlite");
                        using (SQLiteConnection con = new SQLiteConnection("Data Source=System_Usage.sqlite;Version=3;"))
                        {
                            //--------------------------------------------------------------------------------------//
                            con.Open();
                            //--------------------------------------------------------------------------------------//
                            string Banned_App = @"CREATE TABLE System_Usage (
                                        [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                        [Hardware_Id] VARCHAR(100) NULL,
                                        [cpu] VARCHAR(6) NULL, 
                                        [memory] VARCHAR(6) NULL, 
                                        [network_rec] VARCHAR(6) NULL, 
                                        [network_out] VARCHAR(6) NULL, 
                                        [io_rec] VARCHAR(6) NULL, 
                                        [io_out] VARCHAR(6) NULL, 
                                        [Datetime_]
                                            TEXT NULL
                                    );";
                            SQLiteCommand cmd4 = new SQLiteCommand(Banned_App, con);
                            cmd4.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                }
                if (!File.Exists("App_Usage.sqlite"))
                {
                    try
                    {
                        SQLiteConnection.CreateFile("App_Usage.sqlite");
                        using (SQLiteConnection con = new SQLiteConnection("Data Source=App_Usage.sqlite;Version=3;"))
                        {
                            //--------------------------------------------------------------------------------------//
                            con.Open();
                            //--------------------------------------------------------------------------------------//
                            string Banned_App = @"CREATE TABLE App_Usage (
                                        [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                        [Hardware_Id] VARCHAR(100) NULL,
                                        [Process_Name] VARCHAR(250) NULL, 
                                        [Process_Windows_Title] VARCHAR(250) NULL, 
                                        [cpu] VARCHAR(6) NULL, 
                                        [memory] VARCHAR(6) NULL, 
                                        [network_rec] VARCHAR(6) NULL, 
                                        [network_out] VARCHAR(6) NULL, 
                                        [io_rec] VARCHAR(6) NULL, 
                                        [io_out] VARCHAR(6) NULL, 
                                        [Datetime_]
                                            TEXT NULL
                                    );";

                            SQLiteCommand cmd4 = new SQLiteCommand(Banned_App, con);
                            cmd4.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                }

            }
            else
            {
                return true;
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////
            return true;

        }
        #endregion sqliteKontrol
        //--------------------------------------------------------------------------------------//
        #region hardwareid
        public string hardwareid()
        {

            string location = @"SOFTWARE\Microsoft\Cryptography";
            string name = "MachineGuid";

            using (RegistryKey localMachineX64View = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (RegistryKey rk = localMachineX64View.OpenSubKey(location))
                {
                    if (rk == null)
                        throw new KeyNotFoundException(
                            string.Format("Key Not Found: {0}", location));

                    object hardwareid = rk.GetValue(name);
                    if (hardwareid == null)
                        throw new IndexOutOfRangeException(
                            string.Format("Index Not Found: {0}", name));

                    return hardwareid.ToString();
                }
            }
        } //Bilgisayar Kimliği
        #endregion hardwareid
        //--------------------------------------------------------------------------------------//
        #region InternetKontrol
        public bool InternetKontrol()
        {
            try
            {
                System.Net.Sockets.TcpClient kontrol_client = new System.Net.Sockets.TcpClient("google.com", 80);
                kontrol_client.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }//Web Site Bağlantısı Kontrol Ediliyor
        #endregion InternetKontrol
        //--------------------------------------------------------------------------------------//
        #region genel_kullanim
        public void genel_kullanim()
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=locadb.sqlite;Version=3;"))
            {
                //Geçerli Kullanıcı BİLGİSİ alınıyor.
                SQLiteCommand cmd = new SQLiteCommand("Select Hardware_Id From Current_User", con);
                con.Open();
                SQLiteDataReader da = cmd.ExecuteReader();
                da.Read();
                string hardwareid = Convert.ToString(da["Hardware_Id"]);
                con.Close();
                int gonderme_sayaci = 0;

                while (true)
                {
                    double cpu = 0;
                    double memory = 0;
                    double network_rec = 0;
                    double network_out = 0;
                    double io_rec = 0;
                    double io_out = 0;
                    while (true)
                    {
                        try
                        {
                            cpu += total_cpu.NextValue();
                            memory += total_ram.NextValue();
                            network_rec += pcreceived.NextValue() / 1024;
                            network_out += pcsent.NextValue() / 1024;
                            io_rec += 0;
                            io_out += 0;

                            if (gonderme_sayaci == 5)//5 dakikada bir senkronizasyon
                            {
                                //ortalama alındı
                                cpu /= gonderme_sayaci;
                                memory /= gonderme_sayaci;
                                network_rec /= gonderme_sayaci;
                                network_out /= gonderme_sayaci;
                                io_rec /= gonderme_sayaci;
                                io_out /= gonderme_sayaci;
                                if (!InternetKontrol())
                                {
                                    using (SQLiteConnection sys_db_conn = new SQLiteConnection("Data Source=System_Usage.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
                                    {
                                        sys_db_conn.Open();
                                        SQLiteCommand ekle_ = new SQLiteCommand("insert into System_Usage(Hardware_Id,cpu,memory,network_rec,network_out,io_rec,io_out,Datetime_) values ('"+ hardwareid + "','" + Math.Round(cpu, 1) + "','" + Math.Round(memory, 2) + "','" + Math.Round(network_rec, 2) + "','" + Math.Round(network_out, 2) + "','" + Math.Round(io_rec, 2) + "','" + Math.Round(io_out, 2) + "','" + DateTime.Now + "')", sys_db_conn);
                                        ekle_.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    //servise kayıt yaptırılır
                                    Servis.genel_kullanim_gonder(hardwareid, Math.Round(cpu, 1), Math.Round(memory, 2), Math.Round(network_rec, 2), Math.Round(network_out, 2), Math.Round(io_rec, 2), Math.Round(io_out, 2), DateTime.Now);

                                }
                                gonderme_sayaci = 0;
                                break;
                            }
                        }
                        catch (Exception xs)
                        {
                            MessageBox.Show(xs.ToString());
                        }
                        Thread.Sleep(1000);
                        gonderme_sayaci++;

                    }

                }
            }


        }
        #endregion genel_kullanim
        //--------------------------------------------------------------------------------------//
        #region uygulama_kullanim
        public void uygulama_kullanim()
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=locadb.sqlite;Version=3;"))
            {
                //Geçerli Kullanıcı BİLGİSİ alınıyor.
                SQLiteCommand cmd = new SQLiteCommand("Select Hardware_Id From Current_User", con);
                con.Open();
                SQLiteDataReader da = cmd.ExecuteReader();
                da.Read();
                string hardwareid = Convert.ToString(da["Hardware_Id"]);
                con.Close();

                List<App_Usage_List_Model> acik_process_adlari = new List<App_Usage_List_Model>();
                int gonderme_sayaci = 1;
                while (true)
                {

                    var tum_processler = Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle)).ToList();//processler alındı
                    PerformanceCounter p_c_lar;
                    PerformanceCounter p_c_memory;
                    PerformanceCounter p_c_Io_r;
                    PerformanceCounter p_c_Io_o;
                    while (true)
                    {
                        foreach (var item in tum_processler)
                        {
                            try
                            {
                                p_c_lar = new PerformanceCounter("Process", "% Processor Time", item.ProcessName.ToString());
                                p_c_memory = new PerformanceCounter("Process", "Working Set", item.ProcessName.ToString());
                                p_c_Io_r = new PerformanceCounter("Process", "IO Read Bytes/sec", item.ProcessName.ToString());
                                p_c_Io_o = new PerformanceCounter("Process", "IO Write Bytes/sec", item.ProcessName.ToString());
                                p_c_lar.NextValue();
                                p_c_memory.NextValue();
                                p_c_Io_r.NextValue();
                                p_c_Io_o.NextValue();
                                Thread.Sleep(15);
                                //Değerler Toplanıyor
                                App_Usage_List_Model veri_burada = new App_Usage_List_Model();
                                veri_burada.Process_Name = item.ProcessName;
                                veri_burada.Process_Windows_Title = item.MainWindowTitle;
                                veri_burada.Cpu = Math.Round(p_c_lar.NextValue(), 2);//Environment.ProcessorCount;
                                veri_burada.Memory = p_c_memory.NextValue() / 1024 / 1024;
                                veri_burada.Network_Rec = 0;
                                veri_burada.Network_Out = 0;
                                veri_burada.IO_Read = p_c_Io_r.NextValue() / 1024 / 1024;//mb donusturuldu
                                veri_burada.IO_Write = p_c_Io_o.NextValue() / 1024 / 1024;//mb donusturuldu

                                acik_process_adlari.Add(veri_burada);
                            }
                            catch (Exception)
                            {
                                tum_processler = Process.GetProcesses().Where(p => !string.IsNullOrEmpty(p.MainWindowTitle)).ToList();//processler alındı
                                continue;
                            }

                        }
                        if (gonderme_sayaci == 5) //verier servis için işlenir
                        {
                            gonderme_sayaci = 0;
                            try
                            {
                                DataTable app_list_table = new DataTable();
                                app_list_table.TableName = "app_list_table";
                                app_list_table.Columns.Add("Process_Name");
                                app_list_table.Columns.Add("Process_Windows_Title");
                                app_list_table.Columns.Add("Cpu");
                                app_list_table.Columns.Add("Memory(MB)");
                                app_list_table.Columns.Add("Network_Rec");
                                app_list_table.Columns.Add("Network_Out");
                                app_list_table.Columns.Add("IO_Read");
                                app_list_table.Columns.Add("IO_Write");

                                var x = from t in acik_process_adlari
                                        group t by new
                                        {
                                            t.Process_Name,

                                        } into g
                                        select new
                                        {
                                            g.Key.Process_Name,
                                            Process_Windows_Title = g.Select(p => p.Process_Windows_Title).FirstOrDefault(),
                                            Cpu = g.Average(p => p.Cpu),
                                            Memory = g.Average(p => p.Memory),
                                            Network_Rec = g.Average(p => p.Network_Rec),
                                            Network_Out = g.Average(p => p.Network_Out),
                                            IO_Read = g.Average(p => p.IO_Read),
                                            IO_Write = g.Average(p => p.IO_Write),
                                        };

                                foreach (var item in x)
                                {
                                    app_list_table.Rows.Add(item.Process_Name, item.Process_Windows_Title, item.Cpu, item.Memory, item.Network_Rec, item.Network_Out, item.IO_Read, item.IO_Write);

                                }
                                if (!InternetKontrol())
                                {
                                    using (SQLiteConnection sys_db_conn = new SQLiteConnection("Data Source=App_Usage.sqlite;charset=utf-8;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory"))
                                    {
                                        sys_db_conn.Open();
                                        foreach (DataRow item in app_list_table.Rows)
                                        {
                                            SQLiteCommand ekle_ = new SQLiteCommand("insert into App_Usage(Hardware_Id,Process_Name,Process_Windows_Title,cpu,memory,network_rec,network_out,io_rec,io_out,Datetime_) values ('" + hardwareid + "','" + item[0] + "','" + item[1] + "','" + Math.Round(Convert.ToDouble(item[2]), 1) + "','" + Math.Round(Convert.ToDouble(item[3]), 2) + "','" + Math.Round(Convert.ToDouble(item[4]), 2) + "','" + Math.Round(Convert.ToDouble(item[5]), 2) + "','" + Math.Round(Convert.ToDouble(item[6]), 2) + "','" + Math.Round(Convert.ToDouble(item[7]), 2) + "','" + DateTime.Now + "')", sys_db_conn);
                                            ekle_.ExecuteNonQuery();
                                        }
                                        sys_db_conn.Close();
                                    }
                                }
                                else
                                {
                                    //----------------servise kayıt yaptırılır---------------------//
                                    Servis.uygulama_kullanim_gonder(hardwareid, app_list_table);
                                }
                                break;
                            }
                            catch (Exception)
                            {
                            }
                        }
                        gonderme_sayaci++;
                    }
                }
            }

            #region eski2
            //using (SQLiteConnection con = new SQLiteConnection("Data Source=locadb.sqlite;Version=3;"))
            //{
            //    List<string> acik_process_adlari = new List<string>();
            //    PerformanceCounter[] p_c_lar = new PerformanceCounter[50];
            //    PerformanceCounter[] p_c_Io_r = new PerformanceCounter[50];
            //    PerformanceCounter[] p_c_Io_o = new PerformanceCounter[50];

            //    //Geçerli Kullanıcı BİLGİSİ alınıyor.
            //    SQLiteCommand cmd = new SQLiteCommand("Select Hardware_Id From Current_User", con);
            //    con.Open();
            //    SQLiteDataReader da = cmd.ExecuteReader();
            //    da.Read();
            //    string hardwareid = Convert.ToString(da["Hardware_Id"]);
            //    con.Close();

            //    while (true)
            //    {
            //        acik_process_adlari.Clear();//adlar silindi.
            //        var tum_processler = Process.GetProcesses();//processler alındı
            //        foreach (var item in tum_processler)
            //        {
            //            if (!String.IsNullOrEmpty(item.MainWindowTitle))
            //            {
            //                if (!acik_process_adlari.Contains(item.ProcessName.ToString()))
            //                {
            //                    acik_process_adlari.Add(item.ProcessName.ToString());

            //                }
            //            }
            //        }
            //        for (int i = 0; i < acik_process_adlari.Count; i++)
            //        {
            //            p_c_lar[i] = new PerformanceCounter("Process", "% Processor Time", acik_process_adlari[i].ToString());
            //            p_c_Io_r[i] = new PerformanceCounter("Process", "IO Read Bytes/sec", acik_process_adlari[i].ToString());
            //            p_c_Io_o[i] = new PerformanceCounter("Process", "IO Write Bytes/sec", acik_process_adlari[i].ToString());
            //            //  p_c_lar[i].CounterType=250;
            //            p_c_lar[i].NextValue();
            //            p_c_Io_r[i].NextValue();
            //            p_c_Io_o[i].NextValue();
            //        }
            //        int sayac = 0;


            //        List<App_Usage_List_Model> app_list = new List<App_Usage_List_Model>();
            //        DataTable app_list_table = new DataTable();
            //        app_list_table.TableName = "app_list_table";
            //        app_list_table.Columns.Add("Process_Name");
            //        app_list_table.Columns.Add("Process_Windows_Title");
            //        app_list_table.Columns.Add("Cpu");
            //        app_list_table.Columns.Add("Memory");
            //        app_list_table.Columns.Add("Network_Rec");
            //        app_list_table.Columns.Add("Network_Out");
            //        app_list_table.Columns.Add("IO_Read");
            //        app_list_table.Columns.Add("IO_Write");

            //        while (true)
            //        {
            //            for (int i = 0; i < acik_process_adlari.Count(); i++)
            //            {
            //                try
            //                {
            //                    double deger = Math.Round((p_c_lar[i].NextValue() / Environment.ProcessorCount), 2);
            //                    //double deger = Math.Round((p_c_lar[i].NextValue()), 2);
            //                    if (deger > 0)
            //                    {
            //                        Process[] pro = Process.GetProcessesByName(p_c_lar[i].InstanceName.ToString());//process ozllıklerı alınıyor
            //                        string MainWindowTitle_ = "";
            //                        for (int x = 0; x < pro.Length; x++)
            //                        {
            //                            if (pro[x].MainWindowTitle.ToString() != "")
            //                            {
            //                                MainWindowTitle_ = pro[x].MainWindowTitle.ToString();
            //                            }
            //                        }
            //                        // table.Rows.Add(pro[0].ProcessName, MainWindowTitle_.ToString(), deger.ToString(), ((pro[0].PrivateMemorySize64) / 1024).ToString(), 0, 0, (p_c_Io_r[i].NextValue() / 1048576).ToString(), (p_c_Io_o[i].NextValue() / 1048576).ToString());
            //                        App_Usage_List_Model app_list_item = new App_Usage_List_Model();
            //                        app_list_item.Process_Name = pro[0].ProcessName;
            //                        app_list_item.Process_Windows_Title = MainWindowTitle_.ToString();
            //                        app_list_item.Cpu = Convert.ToInt64(deger);//cpu
            //                        app_list_item.Memory = pro[0].PrivateMemorySize64 / 1024;
            //                        app_list_item.Network_Rec = p_c_Io_r[i].NextValue() / 1048576;
            //                        app_list_item.Network_Out = p_c_Io_r[i].NextValue() / 1048576;
            //                        app_list_item.IO_Read = p_c_Io_r[i].NextValue() / 1048576;
            //                        app_list_item.IO_Write = p_c_Io_o[i].NextValue() / 1048576;

            //                        app_list.Add(app_list_item);

            //                    }
            //                    else continue;
            //                }
            //                catch (Exception)
            //                {
            //                    sayac = 9;
            //                    break;
            //                    //çalışan uygulama kapatılırsa tekrar program lıstesı cıkartılır
            //                }
            //            }
            //            Thread.Sleep(1000);
            //            sayac++;
            //            if (sayac == 600) //verier servis için işlenir
            //            {
            //                sayac = 0;
            //                try
            //                {

            //                    var x = from t in app_list
            //                            group t by new
            //                            {
            //                                t.Process_Name,

            //                            } into g
            //                            select new
            //                            {
            //                                g.Key.Process_Name,
            //                                Process_Windows_Title = g.Select(p => p.Process_Windows_Title).FirstOrDefault(),
            //                                Cpu = g.Average(p => p.Cpu),
            //                                Memory = g.Average(p => p.Memory),
            //                                Network_Rec = g.Average(p => p.Network_Rec),
            //                                Network_Out = g.Average(p => p.Network_Out),
            //                                IO_Read = g.Average(p => p.IO_Read),
            //                                IO_Write = g.Average(p => p.IO_Write),
            //                            };


            //                    foreach (var item in x)
            //                    {
            //                        app_list_table.Rows.Add(item.Process_Name, item.Process_Windows_Title, item.Cpu, item.Memory, item.Network_Rec, item.Network_Out, item.IO_Read, item.IO_Write);
            //                    }

            //                    //servise kayıt yaptırılır
            //                    Servis.uygulama_kullanim_gonder(hardwareid, app_list_table);
            //                    MessageBox.Show("tamam");
            //                }
            //                catch (Exception)
            //                {

            //                }
            //                break;
            //            }
            //        }

            //    }
            //}

            #endregion eski2
            #region eski_kullanım
            //            Process[] processes = Process.GetProcesses();
            //            while (true)

            //{
            //                string xmlPath = Application.StartupPath + "\\temp\\XmlData\\system_usage\\Uygulama_Kullanim_1000ms_" + hardwareid + ".xml";
            //                XmlTextWriter customer = new XmlTextWriter(xmlPath, UTF8Encoding.UTF8);
            //                customer.Formatting = Formatting.Indented;
            //                customer.WriteStartDocument();
            //                customer.WriteStartElement("Uygulama_Kullanim_1000ms");
            //                foreach (Process p in processes)
            //                {
            //                    if (!String.IsNullOrEmpty(p.MainWindowTitle))
            //                    {
            //                        for (int i = 0; i <= 1; i++)
            //                        {
            //                            Process[] pp = Process.GetProcessesByName(p.ProcessName);
            //                            if (pp.Length == 0)
            //                            {
            //                                Console.WriteLine(p.ProcessName + " does not exist");
            //                            }
            //                            else
            //                            {
            //                                Process p2 = pp[0];
            //                                if (lastTime == null || lastTime == new DateTime())
            //                                {
            //                                    lastTime = DateTime.Now;
            //                                    lastTotalProcessorTime = p2.TotalProcessorTime;
            //                                }
            //                                else
            //                                {
            //                                    curTime = DateTime.Now;
            //                                    curTotalProcessorTime = p2.TotalProcessorTime;
            //                                    CPUUsage = (curTotalProcessorTime.TotalMilliseconds - lastTotalProcessorTime.TotalMilliseconds) / curTime.Subtract(lastTime).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount); Convert.ToDouble(Environment.ProcessorCount);
            //                                    // Console.WriteLine(p.ProcessName + "-" + Math.Round(CPUUsage * 100, 1));
            //                                    lastTime = curTime;
            //                                    lastTotalProcessorTime = curTotalProcessorTime;
            //                                }
            //                            }
            //                            if (i == 0)
            //                            {
            //                                Thread.Sleep(250);
            //                            }
            //                        }
            //                        string ProcessName = p.ProcessName;
            //                        //CPUUsage
            //                        double ram = p.PrivateMemorySize64 / 1000000;
            //                        string pId = p.Id.ToString();
            //                        string MainWindowTitle = p.MainWindowTitle;
            //                        string StartTime = p.StartTime.ToString();
            //                        string TotalProcessorTime = p.TotalProcessorTime.Duration().Hours.ToString() + ":" + p.TotalProcessorTime.Duration().Minutes.ToString() + ":" + p.TotalProcessorTime.Duration().Seconds;
            //                        string PeakWorkingSet64 = (p.PeakWorkingSet64 / 1024).ToString() + "k";
            //                        string HandleCount = p.HandleCount.ToString();
            //                        string Threads = p.Threads.Count.ToString();
            //                        string MachineName = p.MachineName;
            //                        if (Math.Round((CPUUsage * 100), 1) != 0)
            //                        {
            //                            customer.WriteStartElement("Uygulama_Kullanim_User");
            //                            //customer.WriteAttributeString("ID", "1");//hardwareid eklenecek.
            //                            customer.WriteElementString("Process_Name", p.ProcessName.ToString());
            //                            customer.WriteElementString("Process_Windows_Title", p.MainWindowTitle.ToString());
            //                            customer.WriteElementString("Cpu", Math.Round((CPUUsage * 100), 1).ToString());
            //                            customer.WriteElementString("Memory", ram.ToString());
            //                            customer.WriteElementString("Network_Rec", "boş");
            //                            customer.WriteElementString("Network_Out", "boş");
            //                            customer.WriteElementString("IO_Read", "boş");
            //                            customer.WriteElementString("IO_Write", "boş");
            //                            customer.WriteElementString("Datetime_", "Sunucu Saati");
            //                            customer.WriteElementString("Sync", "1");
            //                            customer.WriteEndElement();
            //                            con.Close();
            //                            gonderme_sayaci++;
            //                        }
            //                         Thread.Sleep(1000); 

            //                    }
            //                }

            //                    customer.WriteEndElement();
            //                    customer.Close();
            //                    gonderme_sayaci = 0;


            //            }

            #endregion eski_kullanım
        }
        #endregion uygulama_kullanim
        //--------------------------------------------------------------------------------------//
        #region information
        public void information(string hw_al)
        {

            string Domain_and_user_name = Environment.UserDomainName + @"\" + Environment.UserName.ToString();
            //--İşletim Sistemi ve Ram Bilgileri--//
            ManagementObjectSearcher os_searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            string version = "Alınamadı";
            double memory_size_gb = 0;
            foreach (ManagementObject bilgi in os_searcher.Get())
            {
                version = bilgi.Properties["Caption"].Value.ToString().Trim() + " - " + bilgi.Properties["OSArchitecture"].Value.ToString();
                memory_size_gb = Math.Round(Convert.ToDouble(bilgi["TotalVisibleMemorySize"]) / (1024 * 1024), 2);
            }
            //--İşlemci Bilgileri--//
            ManagementObjectSearcher proc_searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            string processor_name = "Alınamadı";
            foreach (ManagementObject bilgi in proc_searcher.Get())
            {
                processor_name = bilgi.Properties["Name"].Value.ToString();

            }
            //--Anakart Bilgileri--//
            ManagementObjectSearcher arabebeyim = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            string motherboard_name = "Alınamadı";
            foreach (ManagementObject bilgi in arabebeyim.Get())
            {
                motherboard_name = bilgi.Properties["Manufacturer"].Value.ToString() + "- Model:" + bilgi.Properties["Product"].Value.ToString();
            }
            Servis.pc_information(hw_al, Domain_and_user_name, version, memory_size_gb, processor_name, motherboard_name);
        }
        #endregion information
        //--------------------------------------------------------------------------------------//
        public Bitmap Screenshot() // Bitmap türünde olşuturuyoruz  fonksiyonumuzu. 
        {
            Bitmap Screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics GFX = Graphics.FromImage(Screenshot);
            GFX.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size);
            return Screenshot;
        }

        public void ftp_gonder(string yol)
        {
            FileInfo h_dosyabilgisi = new FileInfo(yol);
            string h_ftpAdresi = "ftp://" + "localhost:1111/" + yol;
            FtpWebRequest h_ftpIstek = (FtpWebRequest)FtpWebRequest.Create(new Uri(h_ftpAdresi));
            h_ftpIstek.Credentials = new NetworkCredential("sistemkontrolor", "By.yasin4141");
            h_ftpIstek.KeepAlive = false;
            h_ftpIstek.Method = WebRequestMethods.Ftp.UploadFile;
            h_ftpIstek.UseBinary = true;
            h_ftpIstek.ContentLength = h_dosyabilgisi.Length;
            int h_bufferUzunluğu = 2048;
            byte[] h_buff = new byte[100000000];
            int sayi1;
            FileStream h_stream = h_dosyabilgisi.OpenRead();
            Stream h_str = h_ftpIstek.GetRequestStream();
            sayi1 = h_stream.Read(h_buff, 0, h_bufferUzunluğu);
            while (sayi1 != 0){
                h_str.Write(h_buff, 0, sayi1);
                sayi1 = h_stream.Read(h_buff, 0, h_bufferUzunluğu);
            }
            h_str.Close();
            h_stream.Close();
        }
        public void ftp_al(string istek_dosya)
        {
            string ftpfullpath = "ftp://" + "localhost:1111/" + istek_dosya;
            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential("sistemkontrolor_bg", "By.yasin4141");
                byte[] fileData = request.DownloadData(ftpfullpath);
                using (FileStream file = File.Create("temp/bg/" + istek_dosya)){
                    file.Write(fileData, 0, fileData.Length);
                    file.Close();
                }
            }
        }
    }
}
