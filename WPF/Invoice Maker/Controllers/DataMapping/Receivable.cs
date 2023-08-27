using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.DataMapping
{
    class Receivable_Table: Models.Receivable
    {
        public string TableName { get; set; }

        public Receivable_Table()
        {
            TableName = "Received";
            Receivable_Record_ID = "Receivable_ID";
            Receivable_Date = "Received_Date";
            Receivable_Amount = "Amount";
            Receivable_Charges = "Charges";
            Receivable_Customer_ID = "CustomerID";
            Receivable_Method = "Received_Method";
            Receivable_Remark = "Remark";
            Receivable_Bankin_Date = "BankIn_Date";
        }
    }

    class Receivable_Balance_Table : Models.Receivable_Balance
    {
        public string TableName { get; set; }
        public Receivable_Balance_Table()
        {
            TableName = "Receivable_Detail";
            IB_ID = "Balance_ID";
            Receivable_Record_ID = "Received_ID";
            InvoiceID = "invoice_ID";
        }
    }
}
