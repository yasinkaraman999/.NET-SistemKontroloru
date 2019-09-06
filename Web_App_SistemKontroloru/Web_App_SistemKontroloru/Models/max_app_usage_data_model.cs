using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Web_App_SistemKontroloru.Models
{
    public class max_app_usage_data_model
    {
       public string process_name { get; set; }
       public double app_cpu { get; set; }
       public double app_ram { get; set; }
       public double app_disk { get; set; }
    }
}