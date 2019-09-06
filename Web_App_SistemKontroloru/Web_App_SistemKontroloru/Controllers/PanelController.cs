using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_App_SistemKontroloru.Models;

namespace Web_App_SistemKontroloru.Controllers
{
    public class PanelController : Controller
    {
        admin_WebApp_DatabaseEntities db = new admin_WebApp_DatabaseEntities();


        // GET:POST Panel
        public ActionResult Index(string baslangic, string bitis, string baslangic_app, string bitis_app)//DASHBOARD
        {
            if (Session["User_Details"] == null)
            {
                return Redirect("Giris");
            }
            bilgisayari_varmi();
            User_Details uye_temp = Session["User_Details"] as User_Details;
            if (Session["Current_PC_Hw_id"] == null)
            {
             
                PC_Details Current_PC = db.PC_Details.FirstOrDefault();
                if (Current_PC != null)
                {
                    Session["Current_PC_Hw_id"] = Current_PC.Hardware_Id;
                }
                
            }
          
            if (Session["Current_PC_Hw_id"]!=null)
            {
                string hwid = Session["Current_PC_Hw_id"].ToString();

                //Tarih Aralığı Ayarlanıyor
                DateTime q_baslangic = Convert.ToDateTime("01/01/1970 00:00:00");
                DateTime q_bitis = DateTime.Now;
                DateTime q_baslangic_app = Convert.ToDateTime("01/01/1970 00:00:00");
                DateTime q_bitis_app = DateTime.Now;


                if (!string.IsNullOrEmpty(baslangic) && !string.IsNullOrEmpty(bitis))
                {
                    q_baslangic = Convert.ToDateTime(baslangic);
                    q_bitis = Convert.ToDateTime(bitis);
                    TempData["Gosterilen_Aralik_System"] = q_baslangic.ToString() + " ile " + q_bitis.ToString();
                }
                else
                {
                    q_baslangic = Convert.ToDateTime("01/01/1970 00:00:00");
                    q_bitis = DateTime.Now;
                    TempData["Gosterilen_Aralik_System"] = "Tüm Zamanlar";
                }
                if (!string.IsNullOrEmpty(baslangic_app) && !string.IsNullOrEmpty(bitis_app))
                {
                    q_baslangic_app = Convert.ToDateTime(baslangic_app);
                    q_bitis_app = Convert.ToDateTime(bitis_app);
                    TempData["Gosterilen_Aralik_App"] = q_baslangic_app.ToString() + " ile " + q_bitis_app.ToString();
                }
                else
                {
                    q_baslangic_app = Convert.ToDateTime("01/01/1970 00:00:00");
                    q_bitis_app = DateTime.Now;
                    TempData["Gosterilen_Aralik_App"] = "Tüm Zamanlar";
                }


                //--Genel CPU-RAM-DİSK KULLANIMI--//
                int item_count = 1;
                #region sistem_kullanim_cpu


                List<JS_Chart_Data_Model> js_data_list_cpu = new List<JS_Chart_Data_Model>();
                List<JS_Chart_Data_Model> js_data_list_ram = new List<JS_Chart_Data_Model>();
                List<JS_Chart_Data_Model> js_data_list_disk = new List<JS_Chart_Data_Model>();
                foreach (var item in db.System_Usage.Where(x => x.Hardware_Id == hwid && x.Datetime_ >= q_baslangic && x.Datetime_ <= q_bitis).ToList())
                {

                    JS_Chart_Data_Model cpu = new JS_Chart_Data_Model();
                    cpu.x = item_count;
                    cpu.y = Math.Round(Convert.ToDouble(item.Cpu), 2);
                    cpu.label = Convert.ToString(String.Format("{0:T}", item.Datetime_));
                    js_data_list_cpu.Add(cpu);

                    JS_Chart_Data_Model ram = new JS_Chart_Data_Model();
                    ram.x = item_count;
                    ram.y = Math.Round(Convert.ToDouble(item.Memory), 2);
                    ram.label = Convert.ToString(String.Format("{0:T}", item.Datetime_));
                    js_data_list_ram.Add(ram);

                    JS_Chart_Data_Model disk = new JS_Chart_Data_Model();
                    disk.x = item_count;
                    disk.y = Math.Round(Convert.ToDouble(item.IO_Read + item.IO_Write), 2);
                    disk.label = Convert.ToString(String.Format("{0:T}", item.Datetime_));
                    js_data_list_disk.Add(disk);

                    item_count++;
                }
                TempData["js_data__sys_cpu"] = JsonConvert.SerializeObject(js_data_list_cpu);
                TempData["js_data__sys_ram"] = JsonConvert.SerializeObject(js_data_list_ram);
                TempData["js_data__sys_disk"] = JsonConvert.SerializeObject(js_data_list_disk);
                item_count = 0;
                //--Max Genel Verileri Hazırlanıyor--//
                TempData["max_sys_cpu"] = js_data_list_cpu.Sum(x => x.y) / js_data_list_cpu.Count();
                TempData["max_sys_ram"] = js_data_list_ram.Sum(x => x.y) / js_data_list_ram.Count();
                TempData["max_sys_disk"] = js_data_list_disk.Sum(x => x.y) / js_data_list_disk.Count();

                #endregion sistem_kullanim_cpu

                #region uygulama_kullanim_cpu
                List<JS_Chart_Data_Model> js_data_list_cpu_app = new List<JS_Chart_Data_Model>();
                List<JS_Chart_Data_Model> js_data_list_ram_app = new List<JS_Chart_Data_Model>();
                List<JS_Chart_Data_Model> js_data_list_disk_app = new List<JS_Chart_Data_Model>();

                var app_usage_list_query = (from cc in db.App_Usage.Where(x => x.Hardware_Id == hwid && x.Datetime_ >= q_baslangic_app && x.Datetime_ <= q_bitis_app).ToList()
                                            group cc by cc.Process_Name into newgroup
                                            //add where clause
                                            select new
                                            {
                                                process_name = newgroup.Key,
                                                app_cpu = newgroup.Sum(c => c.Cpu) / newgroup.Count(),
                                                app_ram = newgroup.Sum(c => c.Memory) / newgroup.Count(),
                                                app_disk = (newgroup.Sum(c => c.IO_Read) + newgroup.Sum(c => c.IO_Write)) / newgroup.Count(),
                                            }).ToList();
                foreach (var item in app_usage_list_query.OrderByDescending(x => x.app_cpu))
                {
                    JS_Chart_Data_Model app_cpu_item = new JS_Chart_Data_Model();
                    app_cpu_item.x = item_count;
                    app_cpu_item.y = Math.Round(Convert.ToDouble(item.app_cpu), 2);
                    app_cpu_item.label = item.process_name;
                    js_data_list_cpu_app.Add(app_cpu_item);
                    item_count++;
                }
                item_count = 1;
                foreach (var item in app_usage_list_query.OrderByDescending(x => x.app_ram))
                {
                    JS_Chart_Data_Model app_ram_item = new JS_Chart_Data_Model();
                    app_ram_item.x = item_count;
                    app_ram_item.y = Math.Round(Convert.ToDouble(item.app_ram), 1);
                    app_ram_item.label = item.process_name;
                    js_data_list_ram_app.Add(app_ram_item);
                    item_count++;
                }
                item_count = 1;
                foreach (var item in app_usage_list_query.OrderByDescending(x => x.app_disk))
                {
                    JS_Chart_Data_Model app_disk_item = new JS_Chart_Data_Model();
                    app_disk_item.x = item_count;
                    app_disk_item.y = Convert.ToDouble(item.app_disk);
                    app_disk_item.label = item.process_name;
                    js_data_list_disk_app.Add(app_disk_item);
                    item_count++;
                }



                TempData["js_data__app_cpu"] = JsonConvert.SerializeObject(js_data_list_cpu_app);
                TempData["js_data__app_ram"] = JsonConvert.SerializeObject(js_data_list_ram_app);
                TempData["js_data__app_disk"] = JsonConvert.SerializeObject(js_data_list_disk_app);
                //--Max Uygulama Verileri Hazırlanıyor--//

                var temp_cpu = app_usage_list_query.Where(x => x.app_cpu == (app_usage_list_query.Max(c => c.app_cpu))).FirstOrDefault();
                if (temp_cpu != null)
                {
                    TempData["max_app_cpu"] = temp_cpu.app_cpu;
                    TempData["max_app_cpu_process_name"] = temp_cpu.process_name;
                }


                var temp_ram = app_usage_list_query.Where(x => x.app_ram == (app_usage_list_query.Max(c => c.app_ram))).FirstOrDefault();
                if (temp_ram != null)
                {
                    TempData["max_app_ram"] = temp_ram.app_cpu;
                    TempData["max_app_ram_process_name"] = temp_ram.process_name;
                }
                var temp_disk = app_usage_list_query.Where(x => x.app_disk == (app_usage_list_query.Max(c => c.app_disk))).FirstOrDefault();
                if (temp_disk != null)
                {
                    TempData["max_app_disk"] = temp_disk.app_cpu;
                    TempData["max_app_disk_process_name"] = temp_disk.process_name;
                }


                #endregion uygulama_kullanim_cpu
            }

            return View();
        }
        public ActionResult TemelKontrol(int? saat, int? dakika, HttpPostedFileBase file = null)
        {
            if (Session["User_Details"] == null)
            {
                return Redirect("Giris");
            }
            bilgisayari_varmi();
            string hwid = Session["Current_PC_Hw_id"].ToString();
            PC_Details Current_PC = db.PC_Details.Where(x => x.Hardware_Id == hwid).FirstOrDefault();
            ViewData["bg"] = Current_PC.Bg_Url;
            //img process
            if (file != null)
            {

                decimal filesize = 0;
                try
                {

                    var supportedTypes = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExt = System.IO.Path.GetExtension(file.FileName);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        ViewData["mesaj"] = "File Extension Is InValid - Only Upload WORD/PDF/EXCEL/TXT File";

                    }
                    else if (file.ContentLength / 1024 / 1024 > 5)
                    {
                        ViewData["mesaj"] = "File size Should Be UpTo " + filesize + "KB";
                    }
                    else
                    {

                        string filename = Session["Current_PC_Hw_id"].ToString() + DateTime.Now.Millisecond.ToString() + fileExt;
                        string savepath = Path.Combine(Server.MapPath("~/temp/bg"), filename);
                        file.SaveAs(savepath);
                        Current_PC.Bg_Url = filename;

                        Work_Order wo_varmi = db.Work_Order.Where(x => x.Hardware_Id == hwid && x.Work_ == "Wallpaper_Change").FirstOrDefault();
                        if (wo_varmi != null)
                        {
                            wo_varmi.Query_ = filename;
                            wo_varmi.Result_ = false;
                            wo_varmi.Active_ = true;
                            wo_varmi.Datetime_ = DateTime.Now;
                        }
                        else
                        {
                            Work_Order wo = new Work_Order();
                            wo.Hardware_Id = hwid;
                            wo.Work_ = "Wallpaper_Change";
                            wo.Query_ = filename;
                            wo.Result_ = false;
                            wo.Active_ = true;
                            wo.Datetime_ = DateTime.Now;
                            db.Work_Order.Add(wo);
                        }
                        db.SaveChanges();

                        return RedirectToAction("TemelKontrol");
                    }
                }
                catch (Exception ex)
                {
                    ViewData["mesaj"] = "Upload Container Should Not Be Empty or Contact Admin" + ex.Message;

                }
            }
            if (saat != null && dakika != null)
            {

                Current_PC.Time_Limit_Minutes = (saat * 60) + dakika;

                Work_Order wo_varmi = db.Work_Order.Where(x => x.Hardware_Id == hwid && x.Work_ == "Time_Limit").FirstOrDefault();
                if (wo_varmi != null)
                {
                    wo_varmi.Query_ = ((saat * 60) + dakika).ToString();
                    wo_varmi.Result_ = false;
                    wo_varmi.Active_ = true;
                    wo_varmi.Datetime_ = DateTime.Now;
                }
                else
                {
                    Work_Order wo = new Work_Order();
                    wo.Hardware_Id = hwid;
                    wo.Work_ = "Time_Limit";
                    wo.Query_ = ((saat * 60) + dakika).ToString();
                    wo.Result_ = false;
                    wo.Active_ = true;
                    wo.Datetime_ = DateTime.Now;
                    db.Work_Order.Add(wo);
                }
                db.SaveChanges();
            }

            return View();
        }
        public ActionResult Yasaklar()
        {
            if (Session["User_Details"] == null)
            {
                return Redirect("Giris");
            }
            bilgisayari_varmi();
            if (Convert.ToByte(Session["bilgisayari_varmi"]) != 0)
            {
                string hwid = Session["Current_PC_Hw_id"].ToString();
                var pc_app_loc = db.PC_App_Locations.Where(x => x.Hardware_Id == hwid).ToList<PC_App_Locations>();
                var banned_app = db.Banned_App.Where(x => x.Hardware_Id == hwid).ToList<Banned_App>();
                return View(Tuple.Create(pc_app_loc, banned_app));
            }
            else return View();
        }
        public ActionResult EkranYakala()
        {
            if (Session["User_Details"] == null)
            {
                return Redirect("Giris");
            }
            bilgisayari_varmi();
            return View();
        }
        public ActionResult UygulamaBaslat()
        {
            if (Session["User_Details"] == null)
            {
                return Redirect("Giris");
            }
            bilgisayari_varmi();
            if (Convert.ToByte(Session["bilgisayari_varmi"]) != 0)
            {
                string hwid = Session["Current_PC_Hw_id"].ToString();


                var pc_app_loc = db.PC_App_Locations.Where(x => x.Hardware_Id == hwid).ToList<PC_App_Locations>();
                var pc_runn_app = db.PC_Running_Apps.Where(x => x.Hardware_id == hwid).ToList<PC_Running_Apps>();

                return View(Tuple.Create(pc_app_loc, pc_runn_app));
            }
            else return View();
        }
        [HttpGet]
        public ActionResult TarayiciGecmisi(int? adet)
        {
            if (Session["User_Details"] == null)
            {
                return Redirect("Giris");
            }
            bilgisayari_varmi();
            if (Convert.ToByte(Session["bilgisayari_varmi"]) != 0)
            {
                string hwid = Session["Current_PC_Hw_id"].ToString();
                Browser_Histories current_history = db.Browser_Histories.Where(x => x.Hardware_Id == hwid).FirstOrDefault();
                if (current_history != null)
                {
                    DataSet browser_h_set = new DataSet();
                    string xmlPath = HttpContext.Server.MapPath(current_history.Url_).Replace("\\Panel", "");
                    browser_h_set.ReadXml(xmlPath);
                    List<DataRow> history_list = new List<DataRow>();
                    if (adet == null)
                    {
                        history_list.AddRange(browser_h_set.Tables[0].Rows.Cast<DataRow>().Take(25));
                    }
                    else
                    {
                        history_list.AddRange(browser_h_set.Tables[0].Rows.Cast<DataRow>().Take((int)adet));
                    }
                    return View(history_list);
                }
                else
                {
                    return View();
                }

            }
            else
            {
                return View();
            }
        }
        public ActionResult Gelismis()
        {
            if (Session["User_Details"] == null)
            {
                return Redirect("Giris");
            }
            bilgisayari_varmi();
            return View();
        }
        [HttpGet]
        public ActionResult Rapor(int? id)
        {
            if (Session["User_Details"] == null)
            {
                return Redirect("Giris");
            }
            bilgisayari_varmi();
            string hwid = Session["Current_PC_Hw_id"].ToString();

            //-Tarih Hesaplanıyor-//
            DateTime aralik = DateTime.Now;
            if (id != null && id != 0 && id > 0) aralik = aralik.AddDays(-Convert.ToInt32(id));
            else aralik = Convert.ToDateTime("01.01.1970 00:00:00");


            //--Genel-APP CPU-RAM--NETWORK-DİSK KULLANIMI--//
            int item_count = 1;
            #region sistem_kullanim_cpu_chart
            List<JS_Chart_Data_Model> js_data_list_cpu = new List<JS_Chart_Data_Model>();
            List<JS_Chart_Data_Model> js_data_list_ram = new List<JS_Chart_Data_Model>();
            List<JS_Chart_Data_Model> js_data_list_disk = new List<JS_Chart_Data_Model>();
            foreach (var item in db.System_Usage.Where(x => x.Hardware_Id == hwid && x.Datetime_ > aralik).ToList())
            {
                JS_Chart_Data_Model cpu = new JS_Chart_Data_Model();
                cpu.x = item_count;
                cpu.y = Math.Round(Convert.ToDouble(item.Cpu), 2);
                cpu.label = Convert.ToString(String.Format("{0:T}", item.Datetime_));
                js_data_list_cpu.Add(cpu);

                JS_Chart_Data_Model ram = new JS_Chart_Data_Model();
                ram.x = item_count;
                ram.y = Math.Round(Convert.ToDouble(item.Memory), 2);
                ram.label = Convert.ToString(String.Format("{0:T}", item.Datetime_));
                js_data_list_ram.Add(ram);

                JS_Chart_Data_Model disk = new JS_Chart_Data_Model();
                disk.x = item_count;
                disk.y = Math.Round(Convert.ToDouble(item.IO_Read + item.IO_Write), 2);
                disk.label = Convert.ToString(String.Format("{0:T}", item.Datetime_));
                js_data_list_disk.Add(disk);

                item_count++;
            }
            //--Chart Genel Verileri Hazırlanıyor--//
            TempData["js_data__sys_cpu"] = JsonConvert.SerializeObject(js_data_list_cpu);
            TempData["js_data__sys_ram"] = JsonConvert.SerializeObject(js_data_list_ram);
            TempData["js_data__sys_disk"] = JsonConvert.SerializeObject(js_data_list_disk);
            item_count = 0;
            //--Max Genel Verileri Hazırlanıyor--//
            TempData["max_sys_cpu"] = js_data_list_cpu.Sum(x => x.y) / js_data_list_cpu.Count();
            TempData["max_sys_ram"] = js_data_list_ram.Sum(x => x.y) / js_data_list_ram.Count();
            TempData["max_sys_disk"] = js_data_list_disk.Sum(x => x.y) / js_data_list_disk.Count();
            #endregion sistem_kullanim_cpu_chart

            #region uygulama_kullanim_cpu_chart
            List<JS_Chart_Data_Model> js_data_list_cpu_app = new List<JS_Chart_Data_Model>();
            List<JS_Chart_Data_Model> js_data_list_ram_app = new List<JS_Chart_Data_Model>();
            List<JS_Chart_Data_Model> js_data_list_disk_app = new List<JS_Chart_Data_Model>();
            //--Uygulama Verileri Hzırlanıyor--//
            var app_usage_list_query = (from cc in db.App_Usage.Where(x => x.Hardware_Id == hwid && x.Datetime_ > aralik).ToList()
                                        group cc by cc.Process_Name into newgroup
                                        //add where clause
                                        select new
                                        {
                                            process_name = newgroup.Key,
                                            app_cpu = newgroup.Sum(c => c.Cpu) / newgroup.Count(),
                                            app_ram = newgroup.Sum(c => c.Memory) / newgroup.Count(),
                                            app_disk = (newgroup.Sum(c => c.IO_Read) + newgroup.Sum(c => c.IO_Write)) / newgroup.Count(),
                                        }).ToList();
            foreach (var item in app_usage_list_query.OrderByDescending(x => x.app_cpu))
            {
                JS_Chart_Data_Model app_cpu_item = new JS_Chart_Data_Model();
                app_cpu_item.x = item_count;
                app_cpu_item.y = Math.Round(Convert.ToDouble(item.app_cpu), 2);
                app_cpu_item.label = item.process_name;
                js_data_list_cpu_app.Add(app_cpu_item);
                item_count++;
            }
            item_count = 1;
            foreach (var item in app_usage_list_query.OrderByDescending(x => x.app_ram))
            {
                JS_Chart_Data_Model app_ram_item = new JS_Chart_Data_Model();
                app_ram_item.x = item_count;
                app_ram_item.y = Math.Round(Convert.ToDouble(item.app_ram), 1);
                app_ram_item.label = item.process_name;
                js_data_list_ram_app.Add(app_ram_item);
                item_count++;
            }
            item_count = 1;
            foreach (var item in app_usage_list_query.OrderByDescending(x => x.app_disk))
            {
                JS_Chart_Data_Model app_disk_item = new JS_Chart_Data_Model();
                app_disk_item.x = item_count;
                app_disk_item.y = Convert.ToDouble(item.app_disk);
                app_disk_item.label = item.process_name;
                js_data_list_disk_app.Add(app_disk_item);
                item_count++;
            }



            TempData["js_data__app_cpu"] = JsonConvert.SerializeObject(js_data_list_cpu_app);
            TempData["js_data__app_ram"] = JsonConvert.SerializeObject(js_data_list_ram_app);
            TempData["js_data__app_disk"] = JsonConvert.SerializeObject(js_data_list_disk_app);
            //--Max Uygulama Verileri Hazırlanıyor--//

            var temp_cpu = app_usage_list_query.Where(x => x.app_cpu == (app_usage_list_query.Max(c => c.app_cpu))).FirstOrDefault();

            if (temp_cpu != null)
            {
                TempData["max_app_cpu"] = temp_cpu.app_cpu;
                TempData["max_app_cpu_process_name"] = temp_cpu.process_name;
            }
            var temp_ram = app_usage_list_query.Where(x => x.app_ram == (app_usage_list_query.Max(c => c.app_ram))).FirstOrDefault();
            if (temp_ram != null)
            {
                TempData["max_app_ram"] = temp_ram.app_cpu;
                TempData["max_app_ram_process_name"] = temp_ram.process_name;
            }
            var temp_disk = app_usage_list_query.Where(x => x.app_disk == (app_usage_list_query.Max(c => c.app_disk))).FirstOrDefault();
            if (temp_disk != null)
            {
                TempData["max_app_disk"] = temp_disk.app_cpu;
                TempData["max_app_disk_process_name"] = temp_disk.process_name;
            }


            #endregion uygulama_kullanim_cpu_chart

            #region genel_istatistik_veriler
            //double rapor_genel_data_cpuxx = Convert.ToDouble(db.System_Usage.Where(x => x.Hardware_Id == hwid).Sum(x => x.Cpu) / db.System_Usage.Where(x => x.Hardware_Id == hwid).Count());
            var rapor_genel_data = (from cc in db.System_Usage.Where(x => x.Hardware_Id == hwid && x.Datetime_ > aralik).ToList()
                                    group cc by cc.Hardware_Id into newgroup
                                    //add where clause
                                    select new
                                    {
                                        process_name = newgroup.Key,
                                        sys_cpu = newgroup.Sum(c => c.Cpu) / newgroup.Count(),
                                        sys_ram = newgroup.Sum(c => c.Memory) / newgroup.Count(),
                                        sys_net = (newgroup.Sum(c => c.Network_Rec) + newgroup.Sum(c => c.Network_Out)) / newgroup.Count(),
                                        sys_disk = (newgroup.Sum(c => c.IO_Read) + newgroup.Sum(c => c.IO_Write)) / newgroup.Count(),
                                    }).FirstOrDefault();

            if (rapor_genel_data != null)
            {
                rapor_genel_data_model rapor_genel_temp_data_item = new rapor_genel_data_model();
                rapor_genel_temp_data_item.sys_cpu = rapor_genel_data.sys_cpu;
                rapor_genel_temp_data_item.sys_ram = rapor_genel_data.sys_ram;
                rapor_genel_temp_data_item.sys_net = rapor_genel_data.sys_net;
                rapor_genel_temp_data_item.sys_disk = rapor_genel_data.sys_disk;
                TempData["rapor_genel_temp_data_item"] = rapor_genel_temp_data_item;

            }

            #endregion genel_istatistik_veriler

            #region app_istatistik_veriler
            List<rapor_genel_data_model> rapor_data_app_list = new List<rapor_genel_data_model>();
            var rapor_app_data = (from cc in db.App_Usage.Where(x => x.Hardware_Id == hwid && x.Datetime_ > aralik).ToList()
                                  group cc by cc.Process_Name into newgroup
                                  //add where clause
                                  select new
                                  {
                                      process_name = newgroup.Key,
                                      app_cpu = newgroup.Sum(c => c.Cpu) / newgroup.Count(),
                                      app_ram = newgroup.Sum(c => c.Memory) / newgroup.Count(),
                                      app_net = (newgroup.Sum(c => c.Network_Rec) + newgroup.Sum(c => c.Network_Out)) / newgroup.Count(),
                                      app_disk = (newgroup.Sum(c => c.IO_Read) + newgroup.Sum(c => c.IO_Write)) / newgroup.Count()

                                  }).ToList();

            if (rapor_app_data != null)
            {
                foreach (var item in rapor_app_data)
                {
                    rapor_genel_data_model temp_data = new rapor_genel_data_model();
                    temp_data.process_name = item.process_name;
                    temp_data.sys_cpu = item.app_cpu;
                    temp_data.sys_ram = item.app_ram;
                    temp_data.sys_net = item.app_net;
                    temp_data.sys_disk = item.app_disk;
                    rapor_data_app_list.Add(temp_data);

                }
                TempData["rapor_data_app_list"] = rapor_data_app_list;
            }
            #endregion app_istatistik_veriler
            #region pc_information
            PC_Details rapor_pc_inf_temp_data = db.PC_Details.Where(x => x.Hardware_Id == hwid).FirstOrDefault();
            if (rapor_pc_inf_temp_data != null)
            {
                TempData["rapor_pc_inf_temp_data"] = rapor_pc_inf_temp_data;
            }

            #endregion pc_information
            return View();
        }
        public ActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Giris(User_Details uye)
        {
            uye.Last_Login = DateTime.Now;
            uye.Status_ = true;
            db.User_Details.Add(uye);
            db.SaveChanges();
            return View();
        }
        [HttpPost]
        public ActionResult GirisIslemi(User_Details uye)
        {
            User_Details uye_temp = db.User_Details.Where(x => x.User_Name == uye.User_Name && x.User_Pass == uye.User_Pass).FirstOrDefault();
            if (uye_temp != null)
            {
                PC_Details Current_PC = db.PC_Details.Where(x => x.User_Id == uye_temp.id).FirstOrDefault();
                if (Current_PC != null)
                {
                    Session["Current_PC_Hw_id"] = Current_PC.Hardware_Id;
                    Session["PC_Details"] = Current_PC;

                }

                Session["User_Details"] = uye_temp;
                bilgisayari_varmi();
                return Redirect("/Panel");
            }
            else
            {
                return Redirect("/Panel/Giris");
            }
        }

        public ActionResult KayitOl()
        {
            return View();
        }
        public ActionResult cikis()
        {
            Session.Clear();
            Response.Redirect("/Panel/Giris");
            return View();
        }


        //----------------------------------Tıklanma Olayları----------------------------//
        [HttpGet]
        public ActionResult is_kaydi(int? gorev, string yol, int? limit_dakika)
        {
            string hw_id = Session["Current_PC_Hw_id"].ToString();
            if (gorev == 1)//screen capture
            {
                try
                {

                    Work_Order kayit_ = db.Work_Order.Where(x => x.Hardware_Id == hw_id && x.Work_ == "Screen_Capture").FirstOrDefault();
                    if (kayit_ == null)
                    {
                        Work_Order yeni_is = new Work_Order();
                        yeni_is.Hardware_Id = hw_id;
                        yeni_is.Work_ = "Screen_Capture";
                        yeni_is.Result_ = false;
                        yeni_is.Active_ = true;
                        yeni_is.Datetime_ = DateTime.Now;
                        db.Work_Order.Add(yeni_is);
                        db.SaveChanges();
                    }
                    else
                    {
                        kayit_.Result_ = false;
                        kayit_.Active_ = true;
                        kayit_.Datetime_ = DateTime.Now;
                        db.SaveChanges();
                    }
                }
                catch { }
            }
            else if (gorev == 2)//screen record
            {
                try
                {
                    Work_Order kayit_ = db.Work_Order.Where(x => x.Hardware_Id == hw_id && x.Work_ == "Screen_Record").FirstOrDefault();
                    if (kayit_ == null)
                    {
                        Work_Order yeni_is = new Work_Order();
                        yeni_is.Hardware_Id = hw_id;
                        yeni_is.Work_ = "Screen_Record";
                        yeni_is.Result_ = false;
                        yeni_is.Active_ = true;
                        yeni_is.Datetime_ = DateTime.Now;
                        db.Work_Order.Add(yeni_is);
                        db.SaveChanges();
                    }
                    else
                    {
                        kayit_.Result_ = false;
                        kayit_.Active_ = true;
                        kayit_.Datetime_ = DateTime.Now;
                        db.SaveChanges();
                    }
                }
                catch { }
            }
            else if (gorev == 3)//uygulama baslat
            {
                try
                {
                    PC_Details pc_det = db.PC_Details.Where(x => x.Hardware_Id == hw_id).FirstOrDefault();
                    if (pc_det != null && pc_det.Online == true)
                    {
                        Work_Order kayit_ = db.Work_Order.Where(x => x.Hardware_Id == hw_id && x.Work_ == "App_Run").FirstOrDefault();

                        if (kayit_ == null)
                        {
                            Work_Order yeni_is = new Work_Order();
                            yeni_is.Hardware_Id = hw_id;
                            yeni_is.Work_ = "App_Run";
                            yeni_is.Query_ = yol;
                            yeni_is.Result_ = false;
                            yeni_is.Active_ = true;
                            yeni_is.Datetime_ = DateTime.Now;
                            db.Work_Order.Add(yeni_is);
                            db.SaveChanges();
                        }
                        else
                        {
                            kayit_.Query_ = yol;
                            kayit_.Result_ = false;
                            kayit_.Active_ = true;
                            kayit_.Datetime_ = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        //PC  KAPALI UYASIRISI
                    }

                }
                catch { }
            }
            else if (gorev == 4)//uygulama sonlandir
            {
                try
                {
                    PC_Details pc_det = db.PC_Details.Where(x => x.Hardware_Id == hw_id).FirstOrDefault();
                    if (pc_det != null && pc_det.Online == true)
                    {
                        Work_Order kayit_ = db.Work_Order.Where(x => x.Hardware_Id == hw_id && x.Work_ == "App_Stop").FirstOrDefault();

                        if (kayit_ == null)
                        {
                            Work_Order yeni_is = new Work_Order();
                            yeni_is.Hardware_Id = hw_id;
                            yeni_is.Work_ = "App_Stop";
                            yeni_is.Query_ = yol;
                            yeni_is.Result_ = false;
                            yeni_is.Active_ = true;
                            yeni_is.Datetime_ = DateTime.Now;
                            db.Work_Order.Add(yeni_is);
                            db.SaveChanges();
                        }
                        else
                        {
                            kayit_.Query_ = yol;
                            kayit_.Result_ = false;
                            kayit_.Active_ = true;
                            kayit_.Datetime_ = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        //PC  KAPALI UYASIRISI
                    }

                }
                catch { }
            }
            else if (gorev == 5)//uygulama engelle
            {
                try
                {
                    PC_Details pc_det = db.PC_Details.Where(x => x.Hardware_Id == hw_id).FirstOrDefault();
                    if (pc_det != null)
                    {
                        Work_Order kayit_ = db.Work_Order.Where(x => x.Hardware_Id == hw_id && x.Work_ == "Banned_App_Add").FirstOrDefault();

                        if (kayit_ == null)
                        {
                            Work_Order yeni_is = new Work_Order();
                            yeni_is.Hardware_Id = hw_id;
                            yeni_is.Work_ = "Banned_App_Add";
                            yeni_is.Query_ = yol;
                            if (limit_dakika != -1)
                            {
                                yeni_is.Temp_ = limit_dakika;
                            }
                            else yeni_is.Temp_ = -1;
                            yeni_is.Result_ = false;
                            yeni_is.Active_ = true;
                            yeni_is.Datetime_ = DateTime.Now;
                            db.Work_Order.Add(yeni_is);
                            db.SaveChanges();
                        }
                        else
                        {
                            kayit_.Query_ = yol;
                            if (limit_dakika != -1)
                            {
                                kayit_.Temp_ = limit_dakika;
                            }
                            else kayit_.Temp_ = -1;
                            kayit_.Result_ = false;
                            kayit_.Active_ = true;
                            kayit_.Datetime_ = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        //PC  KAPALI UYASIRISI
                    }

                }
                catch { }
            }
            else if (gorev == 6)//uygulama engel kaldır
            {
                try
                {
                    PC_Details pc_det = db.PC_Details.Where(x => x.Hardware_Id == hw_id).FirstOrDefault();
                    if (pc_det != null)
                    {
                        Work_Order kayit_ = db.Work_Order.Where(x => x.Hardware_Id == hw_id && x.Work_ == "Banned_App_Remove").FirstOrDefault();

                        if (kayit_ == null)
                        {
                            Work_Order yeni_is = new Work_Order();
                            yeni_is.Hardware_Id = hw_id;
                            yeni_is.Work_ = "Banned_App_Remove";
                            yeni_is.Query_ = yol;
                            yeni_is.Result_ = false;
                            yeni_is.Active_ = true;
                            yeni_is.Datetime_ = DateTime.Now;
                            db.Work_Order.Add(yeni_is);
                            db.SaveChanges();
                        }
                        else
                        {
                            kayit_.Query_ = yol;
                            kayit_.Result_ = false;
                            kayit_.Active_ = true;
                            kayit_.Datetime_ = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        //PC  KAPALI UYASIRISI
                    }

                }
                catch { }
            }
            else if (gorev == 7)// cmd execute query
            {
                try
                {
                    PC_Details pc_det = db.PC_Details.Where(x => x.Hardware_Id == hw_id).FirstOrDefault();
                    if (pc_det != null)
                    {
                        Work_Order kayit_ = db.Work_Order.Where(x => x.Hardware_Id == hw_id && x.Work_ == "Cmd").FirstOrDefault();

                        if (kayit_ == null)
                        {
                            Work_Order yeni_is = new Work_Order();
                            yeni_is.Hardware_Id = hw_id;
                            yeni_is.Work_ = "Cmd";
                            yeni_is.Query_ = yol;
                            yeni_is.Result_ = false;
                            yeni_is.Active_ = true;
                            yeni_is.Datetime_ = DateTime.Now;
                            db.Work_Order.Add(yeni_is);
                            db.SaveChanges();
                        }
                        else
                        {
                            kayit_.Query_ = yol;
                            kayit_.Result_ = false;
                            kayit_.Active_ = true;
                            kayit_.Datetime_ = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        //PC  KAPALI UYASIRISI
                    }

                }
                catch { }
            }
            else if (gorev == 8)// cmd execute query
            {
                try
                {
                    PC_Details pc_det = db.PC_Details.Where(x => x.Hardware_Id == hw_id).FirstOrDefault();
                    if (pc_det != null)
                    {
                        Work_Order kayit_ = db.Work_Order.Where(x => x.Hardware_Id == hw_id && x.Work_ == "Browser_Histories").FirstOrDefault();

                        if (kayit_ == null)
                        {
                            Work_Order yeni_is = new Work_Order();
                            yeni_is.Hardware_Id = hw_id;
                            yeni_is.Work_ = "Browser_Histories";
                            yeni_is.Result_ = false;
                            yeni_is.Active_ = true;
                            yeni_is.Datetime_ = DateTime.Now;
                            db.Work_Order.Add(yeni_is);
                            db.SaveChanges();
                        }
                        else
                        {
                            kayit_.Result_ = false;
                            kayit_.Active_ = true;
                            kayit_.Datetime_ = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        //PC  KAPALI UYASIRISI
                    }

                }
                catch { }
            }

            else if (gorev == 9)//Uygulama Kur
            {
                try
                {
                    PC_Details pc_det = db.PC_Details.Where(x => x.Hardware_Id == hw_id).FirstOrDefault();
                    if (pc_det != null)
                    {
                        Work_Order kayit_ = db.Work_Order.Where(x => x.Hardware_Id == hw_id && x.Work_ == "Install_App").FirstOrDefault();

                        if (kayit_ == null)
                        {
                            Work_Order yeni_is = new Work_Order();
                            yeni_is.Hardware_Id = hw_id;
                            yeni_is.Work_ = "Install_App";
                            yeni_is.Query_ = yol;
                            yeni_is.Result_ = false;
                            yeni_is.Active_ = true;
                            yeni_is.Datetime_ = DateTime.Now;
                            db.Work_Order.Add(yeni_is);
                            db.SaveChanges();
                        }
                        else
                        {
                            kayit_.Query_ = yol;
                            kayit_.Result_ = false;
                            kayit_.Active_ = true;
                            kayit_.Datetime_ = DateTime.Now;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        //PC  KAPALI UYASIRISI
                    }

                }
                catch { }
            }
            else
            {

            }

            return RedirectToAction("EkranYakala", "Panel");
        }
        public ActionResult yasak_tablo_doldur()
        {
            if (Session["User_Details"] == null)
            {
                Response.Redirect("Giris");
            }
            string hw_id = "";
            try
            {
                hw_id = Session["Current_PC_Hw_id"].ToString();
            }
            catch (Exception)
            {
                //Veri Gonderilmez.
            }



            var model = db.Banned_App.Where(x => x.Hardware_Id == hw_id).ToList<Banned_App>();

            return Json(
                new
                {
                    Result = (from obj in model
                              select new
                              {
                                  Islem_adi = obj.P_Name,
                                  Islem_Basligi = obj.Title_,
                                  Eklenme_Tarihi = obj.Datetime_
                              })
                }, JsonRequestBehavior.AllowGet);





        }
        public JsonResult uygulama_sonlandir_list()
        {

            string hwid = Session["Current_PC_Hw_id"].ToString();
            var pc_runn_app = db.PC_Running_Apps.Where(x => x.Hardware_id == hwid).ToList<PC_Running_Apps>();
            return Json(
               new
               {
                   Result = (from obj in pc_runn_app
                             select new
                             {
                                 P_name = obj.Name_,
                                 Islem_Basligi = obj.Title_
                             })
               }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult sorgu_doldur()
        {
            string cikti_metni;
            if (Session["User_Details"] == null || Session["Current_PC_Hw_id"] == null)
            {
                Response.Redirect("Giris");
                cikti_metni = "relogin";
            }
            else
            {
                string hw_id = Session["Current_PC_Hw_id"].ToString();
                var model = db.cmd_output_.Where(x => x.Hardware_Id == hw_id && x.Status_ == true).OrderBy(x => x.Datetime_).FirstOrDefault();


                if (model != null)
                {
                    model.Status_ = false;
                    db.SaveChanges();
                    cikti_metni = model.CMD_Output_Text;
                }
                else
                {
                    cikti_metni = "Bekleniyor!";
                }
            }



            return Json(cikti_metni, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult pc_degistir(string pc_hw_id = null)
        {
            if (pc_hw_id != null)
            {
                Session["Current_PC_Hw_id"] = pc_hw_id;

            }
            return RedirectToAction("Index");
        }
        public void bilgisayari_varmi()
        {
            User_Details temp_ = Session["User_Details"] as User_Details;
            List<PC_Details> temp_pc = db.PC_Details.Where(x => x.User_Id == temp_.id).ToList();

            if (temp_pc.Count == 0)
                Session["bilgisayari_varmi"] = 0;
            else
            {
                Session["bilgisayari_varmi"] = 1;
                Session["PC_Detail"] = temp_pc;
               
            }
        }
        /* Chart Grafik Verileri  */
        public ActionResult deneme()
        {
            return View();
        }

        public JsonResult TopluMesajGonder(string mesaj)
        {
            try
            {
                User_Details yonetici_bilgileri = Session["User_Details"] as User_Details;
                List<PC_Details> ekli_bilgisayarlar = db.PC_Details.Where(x => x.User_Id == yonetici_bilgileri.id).ToList();
                foreach (var item in ekli_bilgisayarlar)
                {
                    Work_Order wo_varmi = db.Work_Order.Where(x => x.Hardware_Id == item.Hardware_Id && x.Work_ == "Alert_Message").FirstOrDefault();
                    if (wo_varmi != null)
                    {

                        wo_varmi.Query_ = mesaj;
                        wo_varmi.Result_ = false;
                        wo_varmi.Active_ = true;
                        wo_varmi.Datetime_ = DateTime.Now;
                        db.SaveChanges();
                    }
                    else
                    {
                        Work_Order is_emri_mesaj = new Work_Order();
                        is_emri_mesaj.Hardware_Id = item.Hardware_Id;
                        is_emri_mesaj.Work_ = "Alert_Message";
                        is_emri_mesaj.Query_ = mesaj;
                        is_emri_mesaj.Result_ = false;
                        is_emri_mesaj.Active_ = true;
                        is_emri_mesaj.Datetime_ = DateTime.Now;
                        db.Work_Order.Add(is_emri_mesaj);
                        db.SaveChanges();
                    }
                }

                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }
    }

}