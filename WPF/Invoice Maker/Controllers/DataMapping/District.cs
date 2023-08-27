using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.DataMapping
{
    class District_Table : Models.District
    {
        public string TableName { get; set; }
        public District_Table()
        {
            TableName = "District";
            City = "City";
            Area = "Area";
            DistrictName = "District";
            Sub_District = "Sub_Districts";
            SD_Chinese = "SD_Chinese";
        }
    }
}
