using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Entities
{

    public class Building_Address : District_Address, Interfaces.Data.IBuilding_Address<string>
    {

        public string Building_C { get; set; }
        public string Building_E { get; set; }
        public string Street_Num { get; set; }        

    }

    public class T_Building_Address : T_District_Address, Interfaces.Data.IBuilding_Address<string>
    {
        public string Building_C { get; set; }
        public string Building_E { get; set; }
        public string Street_Num { get; set; }

        public T_Building_Address()
        {
            TableName = "Building_Dic";
            Building_C = "Building_C";
            Building_E = "Building_E";
            Street_Num = "Street_Num";
        }
    }
}
