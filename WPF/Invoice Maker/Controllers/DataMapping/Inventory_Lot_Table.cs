using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.DataMapping
{
    class Inventory_Lot_Table : Models.Inventory_Lot
    {
        public string TableName { get; set; }
        public Inventory_Lot_Table()
        {
            TableName = "Inventory_Lot";
            Lot_ID = "Lot_ID";
            Purchase_ID = "Purchase_ID";
            Supplier_ID = "Supplier_ID";
            Supplier_Ref = "Supplier_Ref";
            Arrival_Date = "Arrived_Date";
            Currency = "Currency";
            ExchangeRate = "ExchangeRate";
        }
    }

    class Inventory_Lot_Detail_Table : Models.Inventory_Lot_Detail
    {
        public string TableName { get; set; }
        public Inventory_Lot_Detail_Table()
        {
            TableName = "Inventory_Lot_Detail";
            Lot_Detial_ID = "Lot_Detail_ID";
            Lot_ID = "Lot_ID";
            Product_ID = "Product_ID";
            Qty = "Qty";
            UnitPrice = "UnitPrice";
            NetPrice = "NetPrice";
            TotalAmount = "TotalAmount";
            Exp_Date = "Exp_Date";
            Unit = "Unit";
            Remark = "Remark";
        }
    }
        class Inventory_Changes_Table : Models.Inventory_Changes
        {
            public string TableName { get; set; }
            public Inventory_Changes_Table()
            {
                TableName = "Inventory_Changes";
                Changes_ID = "Changes_ID";
                Lot_ID = "Lot_ID";
                Lot_Detial_ID = "Lot_Detial_ID";
                Invoce_Detail_ID = "Invoce_Detail_ID";
                Product_ID = "Product_ID";
                Qty = "Qty";
                Changes_Date = "Changes_Date";
                Changes_Status = "Changes_Status";
            }
        }
 }
