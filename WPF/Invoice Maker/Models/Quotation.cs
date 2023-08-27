using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Models
{
    public class Quotation
    {
        public string Quotatoin_ID { get; set; }
        public string Quotation_Date { get; set; }
        public Clinic Quot_Clinic { get; set; }
        public string Sales_Person { get; set; }
        public List<Quotation_Item> ItemList { get; set; }
        public string Remark { get; set; }
        public string Discount { get; set; }
        public string Sub_Total { get; set; }
        public string Total { get; set; }
        public string Due_Date { get; set; }
        public string Quotation_Status { get; set; }

        public new static bool IsAutoCompleteSuggestion(string search, object item)
        {
            Quotation stock = item as Quotation;
            if (stock != null)
            {
                search = search.ToUpper();
                return (stock.Quotatoin_ID.ToUpper().Contains(search) ||
                    stock.Quotation_Date.ToUpper().Contains(search) ||
                    stock.Quot_Clinic.Clinic_Name.ToUpper().Contains(search));
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Quotatoin_ID;
        }
    }

    public class Quotation_Item : Models.Product
    {
        public string Item_ID { get; set; }
        public string Quotatoin_ID { get; set; }
        public string Quotation_Remark { get; set; }
        public string Net_Price { get; set; }
        public string Qty { get; set; }
        public string Total { get; set; }
    }
}
