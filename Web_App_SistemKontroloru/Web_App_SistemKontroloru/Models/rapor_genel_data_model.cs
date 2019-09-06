using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Web_App_SistemKontroloru.Models
{
    public class rapor_genel_data_model
    {
        public string process_name { get; set; }
        public double? sys_cpu { get; set; }
        public double? sys_ram { get; set; }
        public double? sys_net { get; set; }
        public double? sys_disk { get; set; }
    }
}