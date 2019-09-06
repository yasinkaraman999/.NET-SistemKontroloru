using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Web_App_SistemKontroloru.Models
{
    public class JS_Chart_Data_Model
    {
        public int x { get; set; }
        public double y { get; set; }

        public string label { get; set; }
    }
}