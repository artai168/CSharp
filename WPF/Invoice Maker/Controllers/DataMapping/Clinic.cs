using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.DataMapping
{
    class Clinic_Table : Models.Clinic
    {
        public string TableName { get; set; }
        public Clinic_Table()
        {
            TableName = "Clinic";
            Clinic_ID = "Customer_ID";
            Clinic_Name = "Clinic";
            Address = "ClinicAddress";
            ContactPerson = "Contact";
            Telephone = "Tel";
            SHIPTO_Clinic_Name = "Ship_Clinic";
            SHIPTO_Address = "Ship_Address";
            SHIPTO_ContactPerson = "Ship_Contact";
            SHIPTO_Telephone = "Ship_Tel";
            Delivery = "DELIVERY";
            Payment_Terms = "Terms";
            District = "District";
            SalesPerson = "SalesPerson";
            Discount = "Discount";
        }
    }
}
