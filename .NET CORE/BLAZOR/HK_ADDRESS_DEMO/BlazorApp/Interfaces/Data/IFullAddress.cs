using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Interfaces.Data
{
    interface IFullAddress <T_String>: IBuilding_Address<T_String>, IDistrict_Address<T_String>
    {

        string Street_No { get; set; }
        string Flat { get; set; }
        string Floor { get; set; }
    }
}
