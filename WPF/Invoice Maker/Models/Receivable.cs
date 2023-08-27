namespace TopStar_Invoice_Maker_SQLSever.Models
{
    class Receivable
    {
        public string Receivable_Record_ID { get; set; }
        public string Receivable_Date { get; set; }
        public string Receivable_Bankin_Date { get; set; }
        public string Receivable_Amount { get; set; }
        public string Receivable_Charges { get; set; }
        public string Receivable_Customer_ID { get; set; }
        public string Receivable_Method { get; set; }
        public string Receivable_Remark { get; set; }
    }

    class Receivable_Balance
    {
        public string IB_ID { get; set; }
        public string Receivable_Record_ID { get; set; }
        public string InvoiceID { get; set; }
    }
}
