using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopStar_Invoice_Maker_SQLSever.Controllers.Report
{


    class Inventory_Items
    {
        public string product_Code { get; set; }
        public string in_Qty { get; set; }
        public string out_Qty { get; set; }
    }
    class Inventory_Report
    {
        /*
            1) Monthly Report
                -- List out all item code and descriptions
                -- Get opening balance
                -- Get In stock Qty
                -- Get out stock Qty
         */

        private List<Models.Product> product_List = new List<Models.Product>();
        private List<Inventory_Items> inventory_Items = new List<Inventory_Items>();

        private void getProductList()
        {
            ProductController productController = new ProductController();
            productController.loadList();
            this.product_List = productController.product_List();
        }
    }
}
