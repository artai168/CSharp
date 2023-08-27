namespace TopStar_Invoice_Maker_SQLSever.Models
{
    class Inventory_Lot
    {
        public string Lot_ID { get; set; }
        public string Purchase_ID { get; set; }
        public string Supplier_ID { get; set; }
        public string Supplier_Ref { get; set; }
        public string Arrival_Date { get; set; }
        public string Currency { get; set; }
        public string ExchangeRate { get; set; }
    }

    class Inventory_Lot_Detail
    {
        public string Lot_Detial_ID { get; set; }
        public string Lot_ID { get; set; }
        public string Product_ID { get; set; }
        public string Qty { get; set; }
        public string UnitPrice { get; set; }
        public string NetPrice { get; set; }
        public string TotalAmount { get; set; }
        public string Exp_Date { get; set; }
        public string Unit { get; set; }
        public string Remark { get; set; }
    }

    class Inventory_Changes
    {
        public string Changes_ID { get; set; }
        public string Lot_ID { get; set; }
        public string Lot_Detial_ID { get; set; }
        public string Invoce_ID { get; set; }
        public string Invoce_Detail_ID { get; set; }
        public string Product_ID { get; set; }
        public string Qty { get; set; }
        public string Changes_Date { get; set; }
        public string Changes_Status { get; set; }
    }
}
