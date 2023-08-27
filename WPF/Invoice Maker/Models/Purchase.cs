using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Models
{
    class Purchase
    {
        public string Purchase_ID { get; set; }
        public List<Purchase_Item> ItemList { get; set; }
    }

    class Purchase_Item : Models.Product
    {
        public string Item_ID { get; set; }
        public string Purchase_ID { get; set; }
        public string Discount { get; set; }
        public string Net_Price { get; set; }
        public string Qty { get; set; }
        public string Total { get; set; }
    }
}
