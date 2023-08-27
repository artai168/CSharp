using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
//using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace TopStar_Invoice_Maker_SQLSever.Views.UI.Product
{
    /// <summary>
    /// UC_Product.xaml 的互動邏輯
    /// </summary>
    public partial class UC_Product : UserControl
    {
        private List<Models.Product> productList;
        private List<string> unit_List;
        private List<string> model_List;
        private List<string> brand_List;
        
        public UC_Product()
        {
            InitializeComponent();
            this.txtUnit.ItemsSource = unit_List;
            load_Product_List();
        }

        public void load_Product()
        {
            InitializeComponent();
            this.txtUnit.ItemsSource = unit_List;
        }

        private void load_Product_List()
        {
            if (this.txtBrand.Text == "")
            {
                productList = new List<Models.Product>();
                Controllers.ProductController productController = new Controllers.ProductController();

                productController.loadList();

                productList = productController.product_List;

                this.txtProductID.ItemsSource = productList;
                txtProductID.ItemFilter = (txt, i) => Models.Product.IsAutoCompleteSuggestion(txt, i);
            }
            else
            {
                productList = new List<Models.Product>();
                Controllers.ProductController productController = new Controllers.ProductController();

                productController.loadList(this.txtBrand.Text);

                productList = productController.product_List;
                this.txtProductID.ItemsSource = productList;
                txtProductID.ItemFilter = (txt, i) => Models.Product.IsAutoCompleteSuggestion(txt, i);
            }
        }

        public void SetUI(string inBrand)
        {
            this.txtBrand.Text = inBrand;
        }

        public void SetUI(string inBrand, string inCost, string inProductCode, string inProductID)
        {
            this.txtBrand.Text = inBrand;
            this.txtModel.Text = inProductCode;
            this.txtUnitCost.Text = inCost;
            this.txtProductID.Text = inProductID;
        }

        private void txtProductID_GotFocus(object sender, RoutedEventArgs e)
        {
            load_Product_List();
        }

        private void txtUnit_GotFocus(object sender, RoutedEventArgs e)
        {
            unit_List = new List<string>();
            this.unit_List.Add("BOX");
            this.unit_List.Add("BOTTLE");
            this.unit_List.Add("PACK");
            this.unit_List.Add("PC");
            this.unit_List.Add("SET");
            this.unit_List.Add("UNIT");
            this.txtUnit.ItemsSource = unit_List;
        }

        private void txtBrand_GotFocus(object sender, RoutedEventArgs e)
        {
            brand_List = new List<string>();
            Controllers.ProductController productController = new Controllers.ProductController();
            brand_List = productController.LoadBrandList();
            this.txtBrand.ItemsSource = brand_List;
        }

        private void txtModel_GotFocus(object sender, RoutedEventArgs e)
        {
            model_List = new List<string>();
            if (!string.IsNullOrEmpty(this.txtBrand.Text))
            {
                Controllers.ProductController productController = new Controllers.ProductController();
                model_List = productController.LoadModelList(this.txtBrand.Text);
                this.txtModel.ItemsSource = model_List;
            }

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cleanForm();
        }

        /*
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
        }*/

        private Models.Product GetProduct()
        {
            return new Models.Product
            {
                Product_ID = this.txtProductID.Text,
                Brand = this.txtBrand.Text,
                Model = this.txtModel.Text,
                Product_Descruibtions = this.txtDescribtion.Text,
                Unit = this.txtUnit.Text,
                Cost = this.txtUnitCost.Text,
                Unit_Price = this.txtUnitPrice.Text
            };
        }

        public void SaveData()
        {
            if ((!String.IsNullOrEmpty(this.txtProductID.Text))
                && (!String.IsNullOrEmpty(this.txtModel.Text))
                && (!String.IsNullOrEmpty(this.txtBrand.Text))
                && (!String.IsNullOrEmpty(this.txtDescribtion.Text))
                && (!String.IsNullOrEmpty(this.txtUnit.Text))
                && (!String.IsNullOrEmpty(this.txtUnitPrice.Text)))
            {
                Controllers.ProductController productController = new Controllers.ProductController();
                productController.SaveItem(GetProduct());
                MessageBox.Show("Record Saved!");
                cleanForm();
            }
            else
            {
                MessageBox.Show("Please finish the Form!");
            }
            cleanForm();
        }

        private void cleanForm()
        {
            this.txtProductID.Text = "";
            this.txtBrand.Text = "";
            this.txtModel.Text = "";
            this.txtDescribtion.Text = "";
            this.txtUnit.Text = "";
            this.txtUnitPrice.Text = "";
            this.txtUnitCost.Text = "";
        }

        private void txtProductID_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtProductID.Text))
            {
                var myLINQResult = from product_Detail in productList
                                   where product_Detail.Product_ID == this.txtProductID.Text
                                   select product_Detail;

                foreach (Models.Product tempProductDetail in myLINQResult)
                {
                    this.txtProductID.Text = tempProductDetail.Product_ID;
                    this.txtBrand.Text = tempProductDetail.Brand;
                    this.txtModel.Text = tempProductDetail.Model;
                    this.txtDescribtion.Text = tempProductDetail.Product_Descruibtions;
                    this.txtUnit.Text = tempProductDetail.Unit;
                    this.txtUnitPrice.Text = tempProductDetail.Unit_Price;
                    this.txtUnitCost.Text = tempProductDetail.Cost;
                }
            }
        }

        private void txtUnitPrice_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtUnitPrice.Text))
            {
                string str_Proccessing = this.txtUnitPrice.Text;
                if (str_Proccessing.IndexOf("=") == 0)
                {
                    str_Proccessing = str_Proccessing.Replace("=", "");
                    //var result2 = CSharpScript.EvaluateAsync(str_Proccessing).Result;
                    Decimal value = Convert.ToDecimal(new System.Data.DataTable().Compute(str_Proccessing, null).ToString());
                    int result2 = (int)Math.Ceiling(Convert.ToDecimal(value));
                    this.txtUnitPrice.Text = Convert.ToString(result2);
                }
            }
        }

        private void txtUnitCost_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtUnitCost.Text))
            {
                string str_Proccessing = this.txtUnitCost.Text;
                if (str_Proccessing.IndexOf("=") == 0)
                {
                    str_Proccessing = str_Proccessing.Replace("=", "");
                    //var result2 = CSharpScript.EvaluateAsync(str_Proccessing).Result;
                    Decimal value = Convert.ToDecimal(new System.Data.DataTable().Compute(str_Proccessing, null).ToString());
                    int result2 = (int)Math.Ceiling(Convert.ToDecimal(value));
                    this.txtUnitCost.Text = Convert.ToString(result2);
                }
            }
        }
    }
}
