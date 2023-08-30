using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Address_Analysis.Interface
{
    interface IRecognizeEngine
    {
        //List<string> _str_AddressList { get; set; }
        //string str_RawData { get; set; }

        bool With_Info();

        string Set_Info();
    }
}
