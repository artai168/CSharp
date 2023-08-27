namespace TopStar_Invoice_Maker_SQLSever.Models
{
    public class Product
    {
        public string Product_ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Product_Descruibtions { get; set; }
        public string Unit { get; set; }
        public string Unit_Price { get; set; }
        public string Cost { get; set; }

        public static bool IsAutoCompleteSuggestion(string search, object item)
        {
            Product stock = item as Product;
            if (stock != null)
            {
                search = search.ToUpper();
                return (stock.Product_ID.ToUpper().Contains(search) ||
                    stock.Brand.ToUpper().Contains(search) ||
                    stock.Product_Descruibtions.ToUpper().Contains(search) ||
                    stock.Model.ToUpper().Contains(search));
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Product_ID;
        }
    }
}
