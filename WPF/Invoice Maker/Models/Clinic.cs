namespace TopStar_Invoice_Maker_SQLSever.Models
{
    public class Clinic
    {
        public string Clinic_ID { get; set; }
        public string Clinic_Name { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string Telephone { get; set; }
        public string SHIPTO_Clinic_Name { get; set; }
        public string SHIPTO_Address { get; set; }
        public string SHIPTO_ContactPerson { get; set; }
        public string SHIPTO_Telephone { get; set; }
        public string Delivery { get; set; }
        public string Payment_Terms { get; set; }
        public string District { get; set; }
        public string SalesPerson { get; set; }
        public string Discount { get; set; }


        public static bool IsAutoCompleteSuggestion(string search, object item)
        {
            Clinic stock = item as Clinic;
            if (stock != null)
            {
                search = search.ToUpper();
                return (stock.Clinic_ID.ToUpper().Contains(search) ||
                    stock.Clinic_Name.ToUpper().Contains(search) || 
                    stock.District.ToUpper().Contains(search));
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Clinic_ID;
        }
    }
}
