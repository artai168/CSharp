using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.DataMapping
{
    class Product_Table : Models.Product
    {
        public string TableName { get; set; }
        public Product_Table()
        {
            TableName = "Product";
            Product_ID = "Product_ID";
            Brand = "Brand";
            Model = "Model";
            Product_Descruibtions = "Product_Desc";
            Unit = "Unit";
            Unit_Price = "Unit_Price";
            Cost = "Cost";
        }
    }
}
