using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Web_App_SistemKontroloru.Models;

namespace Web_App_SistemKontroloru
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISistem_Kontroloru_Servis_01" in both code and config file together.
    [ServiceContract]
    public interface ISistem_Kontroloru_Servis_01
    {
        [OperationContract]
        bool Servis_Kontrol();
        [OperationContract]
        string giris(string username, string password, string Hardware_Id);

        [OperationContract]
        string girisSonrasiKontrol(string hardware_id);

        [OperationContract]
        string ayarKontrol(string username, string password, string hardware_id);

        [OperationContract]
        List<Work_Order> isEmri(string Hardware_id);


        [OperationContract]
        string isEmri_Kapat(string Hardware_id, string isemri__, string yol = null, string cmd_output = null);

        [OperationContract]
        string uygulama_listesi_gonder(string Hardware_id, DataTable tablo);

        [OperationContract]
        string calisan_uygulama_gonder(string Hardware_id, DataTable tablo);


        [OperationContract]
        string genel_kullanim_gonder(string Hardware_id, double cpu, double memory, double network_rec, double network_out, double io_rec, double io_out, DateTime datetime_);

        [OperationContract]
        string uygulama_kullanim_gonder(string Hardware_id, DataTable app_list_table);

        [OperationContract]
        void pc_information(string hardwareid, string Domain_User_Name,string Version_, double Memory_Size_Gb, string Processor_Name, string Motherboard_Info);

        [OperationContract]
        void kullanimlari_esitle(string thardwareid, DataTable sys_table, DataTable app_table);
        
    }
}
