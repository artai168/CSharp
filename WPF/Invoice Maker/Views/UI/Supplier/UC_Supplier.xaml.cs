using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TopStar_Invoice_Maker_SQLSever.Views.UI.Supplier
{
    /// <summary>
    /// UC_Supplier.xaml 的互動邏輯
    /// </summary>
    public partial class UC_Supplier : UserControl
    {
        private List<Models.Supplier> supplierList;
        //private List<string> brandList;

        public UC_Supplier()
        {
            InitializeComponent();
        }

        private void txtSupplierID_GotFocus(object sender, RoutedEventArgs e)
        {
            supplierList = new List<Models.Supplier>();
            Controllers.Supplier_Controller supplierController = new Controllers.Supplier_Controller();
            //loadJOBs();
            supplierList = supplierController.loadList();

            this.txtSupplierID.ItemsSource = supplierList;
            txtSupplierID.ItemFilter = (txt, i) => Models.Supplier.IsAutoCompleteSuggestion(txt, i);
        }

        private void txtSupplierID_LostFocus(object sender, RoutedEventArgs e)
        {
            var myLINQResult = from supplier in supplierList
                               where supplier.Supplier_ID == this.txtSupplierID.Text
                               select supplier;

            foreach (Models.Supplier temp_supplier_Detail in myLINQResult)
            {
               this.txtCompany.Text  = temp_supplier_Detail.Company;
                this.txtCurrency.Text = temp_supplier_Detail.Currency;
                this.txtExchangeRate.Text = temp_supplier_Detail.Exchange_Rage;
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            SaveSupplier();
           
        }

        private void SaveSupplier()
        {
            if (is_Able_To_Save())
            {
                Save_To_DB();
                MessageBox.Show("儲存完畢!");
                clearForm();
            }
        }

        private bool is_Able_To_Save()
        {
            if ((!string.IsNullOrEmpty(this.txtCompany.Text)) && (!string.IsNullOrEmpty(this.txtSupplierID.Text)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Save_To_DB()
        {
            Controllers.Supplier_Controller supplierController = new Controllers.Supplier_Controller();
            supplierController.ADD_Supplier(GetSupplier());
        }

        private Models.Supplier GetSupplier()
        {
            return new Models.Supplier {
                Supplier_ID = this.txtSupplierID.Text,
                Company = this.txtCompany.Text,
                Currency = this.txtCurrency.Text,
                Exchange_Rage = this.txtExchangeRate.Text
            };
        }

        public void clearForm()
        {
            this.txtCompany.Text = "";
            this.txtSupplierID.Text = "";
            this.txtExchangeRate.Text = "";
            this.txtCurrency.Text = "";
            this.supplierList = null;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clearForm();
        }
        /*
        private void loadBrandList()
        {
            brandList = new List<string>();
            Controllers.ProductController productController = new Controllers.ProductController();
            brandList = productController.LoadBrandList();

        }*/
    }
}
