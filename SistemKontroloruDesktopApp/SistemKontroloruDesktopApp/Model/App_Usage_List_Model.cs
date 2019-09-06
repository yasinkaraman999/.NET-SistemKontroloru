using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemKontroloruDesktopApp.Model
{
    public class App_Usage_List_Model
    {

        public string Process_Name { get; set; }
        public string Process_Windows_Title { get; set; }
        public double Cpu { get; set; }
        public float Memory { get; set; }
        public float Network_Rec { get; set; }
        public float Network_Out { get; set; }
        public float IO_Read { get; set; }
        public float IO_Write { get; set; }
    }
}
