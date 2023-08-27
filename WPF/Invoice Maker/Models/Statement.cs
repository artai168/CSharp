using System.Collections.Generic;

namespace TopStar_Invoice_Maker_SQLSever.Models
{
    class Statement
    {
        public Clinic The_Clinic { get; set; }
        public string ID { get; set; }
        public string Statement_Date { get; set; }
        public List<Invoice> Invoice_List { get; set; }
    }
}
