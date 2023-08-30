using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Entities
{
    public class Raw_Address : Interfaces.Data.IRaw_Address<int, string>
    {
        public int ID { get; set; }
        public string Raw_Address_C { get; set; }
        public string Raw_Address_E { get; set; }
        public string Dentist_ID { get; set; }
    }

    public class T_Raw_Address : T_DB_Table, Interfaces.Data.IRaw_Address<string, string>
    {
        public string ID { get; set; }
        public string Raw_Address_C { get; set; }
        public string Raw_Address_E { get; set; }
        public string Dentist_ID { get; set; }

        public T_Raw_Address()
        {
            TableName = "Clinic_Address_Dic";
            ID = "Clinic_Address_LibraryId";
            Raw_Address_C = "Raw_Address_C";
            Raw_Address_E = "Raw_Address_E";
            Dentist_ID = "Dentist_ID";
        }
    }

}
