using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TopStar_Invoice_Maker_SQLSever.Views.UI.Opening_Stock
{
    /// <summary>
    /// UC_Opening_Stock_Editor.xaml 的互動邏輯
    /// </summary>
    public partial class UC_Opening_Stock_Editor : UserControl
    {
        TopStar_Invoice_Maker_SQLSever.Opening_Stock.Opening_Stock_Temp my_Opening_Stock_Temp;

        public UC_Opening_Stock_Editor()
        {
            InitializeComponent();
            my_Opening_Stock_Temp = new TopStar_Invoice_Maker_SQLSever.Opening_Stock.Opening_Stock_Temp();
        }

        public void SetUI(TopStar_Invoice_Maker_SQLSever.Opening_Stock.Opening_Stock_Temp inOpening_Stock_Temp)
        {
            this.txtSupplier.Text = inOpening_Stock_Temp.Supplier;
            this.txtBrandName.Text = inOpening_Stock_Temp.Brand;
            this.txtProductCode.Text = inOpening_Stock_Temp.ProductCode;
            this.txtProductID.Text = inOpening_Stock_Temp.Product_ID;
            this.txtCost.Text = inOpening_Stock_Temp.Cost;
            this.txtQty.Text = inOpening_Stock_Temp.Qty;
            this.txt_Error.Text = inOpening_Stock_Temp.Current_Status;
            my_Opening_Stock_Temp = inOpening_Stock_Temp;
        }

        private void clear_UI()
        {
            this.txtSupplier.Text = "";
            this.txtBrandName.Text = "";
            this.txtProductCode.Text = "";
            this.txtProductID.Text = "";
            this.txtCost.Text = "";
            this.txtQty.Text = "";
            this.txt_Error.Text = "";
        }

        private void btn_Create_Product_Click(object sender, RoutedEventArgs e)
        {
            if (this.grid_Product.Visibility == Visibility.Hidden)
            {
                this.grid_Product.Visibility = Visibility.Visible;
                this.uc_Product.SetUI(this.txtBrandName.Text, this.txtCost.Text, this.txtProductCode.Text, this.txtProductID.Text);
            }
            else
            {
                this.grid_Product.Visibility = Visibility.Hidden;
            }
        }

        private void btnSave_New_Product_Click(object sender, RoutedEventArgs e)
        {
            this.uc_Product.SaveData();
            this.grid_Product.Visibility = Visibility.Hidden;
        }

        private void txtProductID_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        List<Models.Product> productList;

        private void txtProductID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtBrandName.Text))
            {
                productList = new List<Models.Product>();
                Controllers.ProductController productController = new Controllers.ProductController();

                productController.loadList(this.txtBrandName.Text);


                productList = productController.product_List;
                this.txtProductID.ItemsSource = productList;
                txtProductID.ItemFilter = (txt, i) => Models.Product.IsAutoCompleteSuggestion(txt, i);
            }
        }

        private TopStar_Invoice_Maker_SQLSever.Opening_Stock.Opening_Stock_Temp get_Opening_Stock_Temp()
        {
            return  new TopStar_Invoice_Maker_SQLSever.Opening_Stock.Opening_Stock_Temp {
                ID = my_Opening_Stock_Temp.ID,
                Supplier = this.txtSupplier.Text,
                Brand = this.txtBrandName.Text,
                ProductCode = this.txtProductCode.Text,
                Product_ID = this.txtProductID.Text,
                Cost = this.txtCost.Text,
                Qty = this.txtQty.Text,
                Current_Status = this.txt_Error.Text,
                F_Month = my_Opening_Stock_Temp.F_Month,
                F_Year = my_Opening_Stock_Temp.F_Year
            };
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            TopStar_Invoice_Maker_SQLSever.Opening_Stock.opening_Stock_Processor opening_Stock_Processor= new TopStar_Invoice_Maker_SQLSever.Opening_Stock.opening_Stock_Processor();
            opening_Stock_Processor.Save_Item_To_DB(get_Opening_Stock_Temp());
            clear_UI();
        }
    }
    
}
