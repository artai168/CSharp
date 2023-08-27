using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.DataMapping
{
    class Invoice_Table : Models.Invoice
    {
        public string TableName { get; set; }
        public Invoice_Table()
        {
            TableName = "INV_Data";
            Invoice_ID = "INVOICE_NO";
            Invoice_Date = "INVOICE_DATE";
            Sales_Person = "Sales_Person";
            Remark = "Remark";
            Clinic_ID = "Customer_ID";
            Clinic_Name = "Clinic";
            Address = "ClinicAddress";
            ContactPerson = "Contact";
            Telephone = "Tel";
            SHIPTO_Clinic_Name = "Ship_Clinic";
            SHIPTO_Address = "Ship_Address";
            SHIPTO_ContactPerson = "Ship_Contact";
            SHIPTO_Telephone = "Ship_Tel";
            District = "District";
            Delivery = "DELIVERY";
            Payment_Terms = "Terms";
            CustomerReferenceCode = "Customer_Ref_No";
            Discount = "Discount";
            Sub_Total = "Sub_Total";
            Due_Date = "Due_Date";
            INV_Status = "INV_Status";
            Total = "Total";
        }
    }

    class Invoice_Item_Table : Models.Invoice_Item
    {
        public string TableName { get; set; }
        public Invoice_Item_Table()
        {
            TableName = "INV_Product_Data";
            Item_ID = "Item_ID";
            Product_ID = "Product_ID";
            Brand = "Brand";
            Model = "Model";
            Product_Descruibtions = "Product_Desc";
            Unit = "Unit";
            Qty = "Qty";
            Unit_Price = "Unit_Price";
            Item_Remark = "Remark";
            Net_Price = "Net_Price";
            Total = "Total";
            Invoice_ID = "Invoice_ID";
        }

    }
}
