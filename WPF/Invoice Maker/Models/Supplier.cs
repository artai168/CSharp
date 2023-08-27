namespace TopStar_Invoice_Maker_SQLSever.Models
{
    public class Supplier
    {
        public string Supplier_ID { get; set; }
        public string Company { get; set; }
        public string Currency { get; set; }
        public string Exchange_Rage { get; set; }

        public static bool IsAutoCompleteSuggestion(string search, object item)
        {
            Supplier stock = item as Supplier;
            if (stock != null)
            {
                search = search.ToUpper();
                return (stock.Supplier_ID.ToUpper().Contains(search) ||
                    stock.Company.ToUpper().Contains(search));
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Supplier_ID;
        }
    }
}
