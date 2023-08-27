using System.Collections.Generic;

namespace TopStar_Invoice_Maker_SQLSever.Models
{
    public class Invoice : Models.Clinic
    {
        public string Invoice_ID { get; set; }
        public string Invoice_Date { get; set; }
        public string Sales_Person { get; set; }
        public List<Invoice_Item> ItemList { get; set; }
        public string Remark { get; set; }
        public string CustomerReferenceCode { get; set; }
        public string Discount { get; set; }
        public string Sub_Total { get; set; }
        public string Total { get; set; }
        public string Due_Date { get; set; }
        public string INV_Status { get; set; }

        public new static bool IsAutoCompleteSuggestion(string search, object item)
        {
            Invoice stock = item as Invoice;
            if (stock != null)
            {
                search = search.ToUpper();
                return (stock.Invoice_ID.ToUpper().Contains(search) ||
                    stock.Invoice_Date.ToUpper().Contains(search) ||
                    stock.Total.ToUpper().Contains(search));
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Invoice_ID;
        }
    }

    public class Invoice_Item : Models.Product
    {
        public string Item_ID { get; set; }
        public string Invoice_ID { get; set; }
        public string Item_Remark { get; set; }
        public string Net_Price { get; set; }
        public string Qty { get; set; }
        public string Total { get; set; }
    }
}
