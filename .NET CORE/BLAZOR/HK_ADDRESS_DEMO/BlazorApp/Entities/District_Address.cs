using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Entities
{
    public class District_Address :  Interfaces.Data.IDistrict_Address<string>
    {
        public string Street_C { get; set; }
        public string District_C { get; set; }
        public string Area_C { get; set; }
        public string Street_E { get; set; }
        public string District_E { get; set; }
        public string Area_E { get; set; }
    }

    public class T_District_Address : T_DB_Table, Interfaces.Data.IDistrict_Address<string>
    {
        public string Street_C { get; set; }
        public string District_C { get; set; }
        public string Area_C { get; set; }
        public string Street_E { get; set; }
        public string District_E { get; set; }
        public string Area_E { get; set; }

        public T_District_Address()
        {
            TableName = "DISTRICT_Dic";
            Street_C = "Street_C";
            District_C = "District_C";
            Area_C = "Area_C";
            Street_E = "Street_E";
            District_E = "District_E";
            Area_E = "Area_E";
        }
    }
 }
