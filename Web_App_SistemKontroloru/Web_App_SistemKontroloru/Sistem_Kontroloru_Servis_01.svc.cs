using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using Web_App_SistemKontroloru.Models;

namespace Web_App_SistemKontroloru
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Sistem_Kontroloru_Servis_01" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Sistem_Kontroloru_Servis_01.svc or Sistem_Kontroloru_Servis_01.svc.cs at the Solution Explorer and start debugging.
    public class Sistem_Kontroloru_Servis_01 : ISistem_Kontroloru_Servis_01
    {
        admin_WebApp_DatabaseEntities db = new admin_WebApp_DatabaseEntities();

        public bool Servis_Kontrol()
        {
            return true;
        }
        public string giris(string username, string password, string Hardware_Id)
        {
            User_Details UserTable = db.User_Details.Where(x => x.User_Name == username && x.User_Pass == password).FirstOrDefault();
            if (UserTable != null)
            {
                PC_Details pc_varmi = db.PC_Details.Where(x => x.Hardware_Id == Hardware_Id).FirstOrDefault();
                if (pc_varmi == null)
                {
                    PC_Details PC_Details = new PC_Details();
                    PC_Details.User_Id = UserTable.id;
                    PC_Details.Hardware_Id = Hardware_Id;
                    PC_Details.Status_ = true;
                    PC_Details.Last_Sync = DateTime.Now;
                    db.PC_Details.Add(PC_Details);
                    db.SaveChanges();
                    return "Pc_Eklendi";//Kullanıcı id si bu olan kayıt giriş yaptı ve bilgisayarı ekledı
                }
                else
                {
                    pc_varmi.Status_ = true;
                    pc_varmi.Last_Sync = DateTime.Now;
                    db.SaveChanges();
                    return "Normal_Giris";//Kullanıcıya Ait Bilgisayar Var Normal Giriş Yap.
                }
            }
            else
            {
                return "Kullanici_Bulunamadi";//Kullanıcı Bulunamadı.
            }
        }
        public string girisSonrasiKontrol(string hardware_id)
        {
            PC_Details PC_Detailss = db.PC_Details.Where(x => x.Hardware_Id == hardware_id).SingleOrDefault();
            if (PC_Detailss != null)
            {
                PC_Detailss.Online = true;
                db.SaveChanges();
                return "Ok";
            }
            else { return "Giris_Ekrani"; }
        }
        public string ayarKontrol(string username, string password, string hardware_id)
        {

            User_Details User_Detailss = db.User_Details.Where(x => x.User_Name == username && x.User_Pass == password).FirstOrDefault();
            if (User_Detailss != null)
            {
                PC_Details PC_DetailsS = db.PC_Details.Where(x => x.Hardware_Id == hardware_id).FirstOrDefault();
                if (PC_DetailsS != null)
                {
                    return "AyarAc";
                }
                else
                {
                    return "PcEslesmedi";
                }
            }
            else
            {
                //bulunamadı
                return "Bulunamadi";
            }
        }
        public List<Work_Order> isEmri(string Hardware_id)
        {
            List<Work_Order> isEmri_ = db.Work_Order.Where(x => x.Hardware_Id == Hardware_id && x.Active_ == true && x.Result_ == false).ToList();
            if (isEmri_ != null)
            {
                return isEmri_;//dolu liste
            }
            else
            {
                return isEmri_;//boş liste
            }
        }
        public string isEmri_Kapat(string Hardware_id, string isemri__, string yol = null, string cmd_output = null)
        {
            Work_Order isEmri_ = db.Work_Order.Where(x => x.Hardware_Id == Hardware_id && x.Work_ == isemri__ && x.Active_ == true && x.Result_ == false).FirstOrDefault();
            try
            {
                switch (isemri__)
                {
                    case "Screen_Capture":
                        isEmri_.Active_ = false;
                        isEmri_.Result_ = true;
                        isEmri_.Datetime_ = DateTime.Now;
                        db.SaveChanges();

                        Screen_Capture image_ = new Screen_Capture();
                        image_.Hardware_Id = Hardware_id;
                        image_.Img_Url = yol;
                        image_.Datetime_ = DateTime.Now;
                        db.Screen_Capture.Add(image_);
                        db.SaveChanges();
                        return "Ok";

                    case "Screen_Record":

                        isEmri_.Active_ = false;
                        isEmri_.Result_ = true;
                        isEmri_.Datetime_ = DateTime.Now;
                        db.SaveChanges();

                        Screen_Record video_ = new Screen_Record();
                        video_.Hardware_Id = Hardware_id;
                        video_.Video_Url = yol;
                        video_.Datetime_ = DateTime.Now;
                        db.Screen_Record.Add(video_);
                        db.SaveChanges();
                        return "Ok";
                    case "App_Run":
                        isEmri_.Active_ = false;
                        isEmri_.Result_ = true;
                        isEmri_.Datetime_ = DateTime.Now;
                        db.SaveChanges();
                        return "Ok";
                    case "App_Stop":
                        isEmri_.Active_ = false;
                        isEmri_.Result_ = true;
                        isEmri_.Datetime_ = DateTime.Now;
                        db.SaveChanges();
                        return "Ok";
                    case "Browser_Histories":
                        isEmri_.Active_ = false;
                        isEmri_.Result_ = true;
                        isEmri_.Datetime_ = DateTime.Now;
                        //Aynı ANDA KAYIT YAPINCA DESKTOPDA BUG OLUYOR DUZELTİLECEK

                        //browser historye yol yaılıyor
                        Browser_Histories bh_check = db.Browser_Histories.Where(x => x.Hardware_Id == Hardware_id).FirstOrDefault();

                        if (bh_check == null)
                        {
                            Browser_Histories b_h = new Browser_Histories();
                            b_h.Hardware_Id = Hardware_id;
                            b_h.Title_ = "";
                            b_h.Url_ = yol;
                            b_h.Sync_Time_ = DateTime.Now;
                            db.Browser_Histories.Add(b_h);
                        }
                        else
                        {
                            string xmlPath = HttpContext.Current.Server.MapPath(bh_check.Url_).Replace("\\Panel", "");
                            File.Delete(xmlPath);
                            bh_check.Hardware_Id = Hardware_id;
                            bh_check.Title_ = "";
                            bh_check.Url_ = yol;
                            bh_check.Sync_Time_ = DateTime.Now;
                        }

                        db.SaveChanges();
                        return "Ok";

                    case "Cmd":
                        isEmri_.Active_ = false;
                        isEmri_.Result_ = true;
                        isEmri_.Datetime_ = DateTime.Now;
                        db.SaveChanges();

                        cmd_output_ cmd_ = new cmd_output_();
                        cmd_.Hardware_Id = Hardware_id;
                        cmd_.Query_ = yol;
                        cmd_.CMD_Output_Text = cmd_output;
                        cmd_.Datetime_ = DateTime.Now;
                        cmd_.Status_ = true;
                        db.cmd_output_.Add(cmd_);
                        db.SaveChanges();
                        return "Ok";
                    case "Banned_App_Add":
                        isEmri_.Active_ = false;
                        isEmri_.Result_ = true;
                        isEmri_.Datetime_ = DateTime.Now;
                        db.SaveChanges();


                        Banned_App banlandin = new Banned_App();
                        banlandin.Hardware_Id = Hardware_id;
                        banlandin.P_Name = yol;
                        var banned_title_ = db.PC_App_Locations.Where(x => x.Process_Name_ == yol).FirstOrDefault();
                        banlandin.Title_ = banned_title_.App_Name;
                        banlandin.Datetime_ = DateTime.Now;
                        db.Banned_App.Add(banlandin);
                        db.SaveChanges();

                        //Banned app için tablo olusturulacak

                        return "Ok";
                    case "Banned_App_Remove":
                        isEmri_.Active_ = false;
                        isEmri_.Result_ = true;
                        isEmri_.Datetime_ = DateTime.Now;
                        db.SaveChanges();


                        Banned_App banned_disable = db.Banned_App.Where(x => x.Hardware_Id == Hardware_id && x.P_Name == yol).FirstOrDefault();
                        db.Banned_App.Remove(banned_disable);
                        db.SaveChanges();

                        return "Ok";
                    case "Wallpaper_Change":
                        isEmri_.Active_ = false;
                        isEmri_.Result_ = true;
                        isEmri_.Datetime_ = DateTime.Now;
                        db.SaveChanges();

                        return "Ok";
                    case "Time_Limit":
                        isEmri_.Active_ = false;
                        isEmri_.Result_ = true;
                        isEmri_.Datetime_ = DateTime.Now;
                        db.SaveChanges();

                        return "Ok";
                    case "Alert_Message":
                        isEmri_.Active_ = false;
                        isEmri_.Result_ = true;
                        isEmri_.Datetime_ = DateTime.Now;
                        db.SaveChanges();

                        return "Ok";
                    case "Install_App":
                        isEmri_.Active_ = false;
                        isEmri_.Result_ = true;
                        isEmri_.Datetime_ = DateTime.Now;
                        db.SaveChanges();

                        return "Ok";
                    default:
                        return "Hatali_Emir";
                }

            }
            catch (Exception xx)
            {
                return "Hata" + xx.ToString();
            }

        }
        public string uygulama_listesi_gonder(string Hardware_id, DataTable tablo)
        {
            db.PC_App_Locations.RemoveRange(db.PC_App_Locations.Where(x => x.Hardware_Id == Hardware_id));
            db.SaveChanges(); //Kullanıcıya ait eski kayıtlar siliniyor 


            foreach (DataRow item in tablo.Rows) //yeni veriler eklenıyor
            {
                PC_App_Locations pcapploc = new PC_App_Locations();
                pcapploc.Hardware_Id = Hardware_id;
                pcapploc.Folder_ = item.Field<string>(0);
                pcapploc.App_Name = item.Field<string>(1);
                pcapploc.Process_Name_ = item.Field<string>(2);
                pcapploc.Datetime_ = DateTime.Now;
                db.PC_App_Locations.Add(pcapploc);
                db.SaveChanges();
            }


            return "";

        }
        public string calisan_uygulama_gonder(string Hardware_id, DataTable tablo)
        {

            db.PC_Running_Apps.RemoveRange(db.PC_Running_Apps.Where(x => x.Hardware_id == Hardware_id));
            db.SaveChanges(); //Kullanıcıya ait eski kayıtlar siliniyor 


            foreach (DataRow item in tablo.Rows)//yeni veriler eklenıyor
            {
                PC_Running_Apps pc_r_apps = new PC_Running_Apps();
                pc_r_apps.Hardware_id = Hardware_id;
                pc_r_apps.Name_ = item.Field<string>(0);
                pc_r_apps.Title_ = item.Field<string>(1);
                db.PC_Running_Apps.Add(pc_r_apps);
                db.SaveChanges();
            }
            return "";

        }


        public string genel_kullanim_gonder(string Hardware_id, double cpu, double memory, double network_rec, double network_out, double io_rec, double io_out, DateTime datetime_)
        {
            try
            {
                System_Usage sys_us = new System_Usage();
                sys_us.Hardware_Id = Hardware_id;
                sys_us.Cpu = cpu;
                sys_us.Memory = memory;
                sys_us.Network_Rec = network_rec;
                sys_us.Network_Out = network_out;
                sys_us.IO_Read = io_rec;
                sys_us.IO_Write = io_out;
                sys_us.Datetime_ = datetime_;
                sys_us.Sync = true;
                db.System_Usage.Add(sys_us);
                db.SaveChanges();
                return "OK";
            }
            catch (Exception x)
            {
                return x.ToString();
            }
        }
        public string uygulama_kullanim_gonder(string Hardware_id, DataTable app_list_table)
        {
            try
            {
                foreach (DataRow row in app_list_table.Rows)
                {
                    App_Usage app_us = new App_Usage();
                    app_us.Hardware_Id = Hardware_id;
                    app_us.Process_Name = row[0].ToString();
                    app_us.Process_Windows_Title = row[1].ToString();
                    app_us.Cpu = Convert.ToDouble(row[2]);
                    app_us.Memory = Convert.ToDouble(row[3]);
                    app_us.Network_Rec = Convert.ToDouble(row[4]);
                    app_us.Network_Out = Convert.ToDouble(row[5]);
                    app_us.IO_Read = Convert.ToDouble(row[6]);
                    app_us.IO_Write = Convert.ToDouble(row[7]);
                    app_us.Datetime_ = DateTime.Now;
                    app_us.Sync = true;
                    db.App_Usage.Add(app_us);

                }
                db.SaveChanges();
                return "OK";
            }
            catch (Exception x)
            {
                return x.ToString();
            }
        }

        public void pc_information(string thardwareid, string tDomain_User_Name, string tVersion_, double tMemory_Size_Gb, string tProcessor_Name, string tMotherboard_Info)
        {
            PC_Details temp_pc_detail = db.PC_Details.Where(x => x.Hardware_Id == thardwareid).FirstOrDefault();
            if (temp_pc_detail != null)
            {
                temp_pc_detail.Domain_User_Name = tDomain_User_Name.Replace("\\", "-");
                temp_pc_detail.Version_ = tVersion_;
                temp_pc_detail.Memory_Size_Gb = tMemory_Size_Gb;
                temp_pc_detail.Processor_Name = tProcessor_Name;
                temp_pc_detail.Motherboard_Info = tMotherboard_Info;
                db.SaveChanges();
            }


        }
        public void kullanimlari_esitle(string thardwareid, DataTable sys_table, DataTable app_table)
        {
            PC_Details temp_pc_detail = db.PC_Details.Where(x => x.Hardware_Id == thardwareid).FirstOrDefault();
            if (temp_pc_detail != null)
            {
                foreach (DataRow item in sys_table.Rows)
                {
                    System_Usage temp_sys_usage_data = new System_Usage();
                    temp_sys_usage_data.Hardware_Id = thardwareid;
                    temp_sys_usage_data.Cpu = Convert.ToDouble(item[2]);
                    temp_sys_usage_data.Memory = Convert.ToDouble(item[3]);
                    temp_sys_usage_data.Network_Rec = Convert.ToDouble(item[4]);
                    temp_sys_usage_data.Network_Out = Convert.ToDouble(item[5]);
                    temp_sys_usage_data.IO_Read = Convert.ToDouble(item[6]);
                    temp_sys_usage_data.IO_Write = Convert.ToDouble(item[7]);
                    temp_sys_usage_data.Datetime_ = Convert.ToDateTime(item[8]);
                    temp_sys_usage_data.Sync = true;
                    db.System_Usage.Add(temp_sys_usage_data);
                }
                foreach (DataRow item in app_table.Rows)
                {
                    App_Usage temp_app_usage_data = new App_Usage();
                    temp_app_usage_data.Hardware_Id = thardwareid;
                    temp_app_usage_data.Process_Name =item[2].ToString();
                    temp_app_usage_data.Process_Windows_Title = item[3].ToString();
                    temp_app_usage_data.Cpu = Convert.ToDouble(item[4]);
                    temp_app_usage_data.Memory = Convert.ToDouble(item[5]);
                    temp_app_usage_data.Network_Rec = Convert.ToDouble(item[6]);
                    temp_app_usage_data.Network_Out = Convert.ToDouble(item[7]);
                    temp_app_usage_data.IO_Read = Convert.ToDouble(item[8]);
                    temp_app_usage_data.IO_Write = Convert.ToDouble(item[9]);
                    temp_app_usage_data.Datetime_ = Convert.ToDateTime(item[10]);
                    temp_app_usage_data.Sync = true;
                    db.App_Usage.Add(temp_app_usage_data);
                }
                db.SaveChanges();
            }
        }
    }

}
