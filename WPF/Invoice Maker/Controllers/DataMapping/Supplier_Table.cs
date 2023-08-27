using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.DataMapping
{
    class Supplier_Table : Models.Supplier
    {
        public string TableName { get; set; }
        public Supplier_Table()
        {
            TableName = "Supplier";
            Supplier_ID = "Supplier_ID";
            Company = "Company";
            Currency = "Currency";
            Exchange_Rage = "Exchange_Rate";
        }
    }
}
