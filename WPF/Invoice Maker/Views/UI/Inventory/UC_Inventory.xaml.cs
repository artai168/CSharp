using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
//using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace TopStar_Invoice_Maker_SQLSever.Views.UI.Inventory
{
    /// <summary>
    /// UC_Inventory.xaml 的互動邏輯
    /// </summary>
    public partial class UC_Inventory : UserControl
    {
        private List<Models.Supplier> supplierList;
        private List<Models.Product> productList;
        private List<string> unit_List;
        private List<Models.Inventory_Lot_Detail> lot_DetailsList;

        public UC_Inventory()
        {
            InitializeComponent();
            lot_DetailsList = new List<Models.Inventory_Lot_Detail>();

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
            if (string.IsNullOrEmpty(this.txtCurrency.Text))
            {
                var myLINQResult = from supplier in supplierList
                                   where supplier.Supplier_ID == this.txtSupplierID.Text
                                   select supplier;

                foreach (Models.Supplier temp_supplier_Detail in myLINQResult)
                {
                    //this.txtCompany.Text = temp_supplier_Detail.Company;
                    this.txtSupplierID.Text = temp_supplier_Detail.Supplier_ID;
                    this.txtCurrency.Text = temp_supplier_Detail.Currency;
                    this.txtExchangeRate.Text = temp_supplier_Detail.Exchange_Rage;
                }
            }
            else
            {
                this.txtProductID.Focusable = true;
                Keyboard.Focus(txtProductID);
            }
        }

# region Save_Data
        private void SaveRecord()
        {
            Models.Inventory_Lot inventory_Lot = new Models.Inventory_Lot();
            List<Models.Inventory_Lot_Detail> inventory_Lot_Details = new List<Models.Inventory_Lot_Detail>();

            if (is_Able_To_Save())
            {
                //-- update unitprices --
                update_NetPrice_from_List();
                //------------
                Save_To_DB();
                MessageBox.Show("儲存完畢!");
                clear_Form();
            }
        }

        private void update_NetPrice_from_List()
        {
            foreach(Models.Inventory_Lot_Detail temp_Lot_Detail in lot_DetailsList)
            {
                temp_Lot_Detail.NetPrice = caculate_NetPrice(this.txtExchangeRate.Text, temp_Lot_Detail.UnitPrice);
            }
        }

        private bool is_Able_To_Save()
        {
            if ((is_Inventory_Lot_Available ()) && (is_Inventory_Detail_Available()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool is_Inventory_Lot_Available()
        {
            bool result = true;
            if ((string.IsNullOrEmpty(this.txtInventoryNo.Text)) || (this.txtInventoryNo.Text.Length < 6))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(this.txtSupplierID.Text)) 
            {
                return false;
            }
            else if (string.IsNullOrEmpty(this.txtArrival.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(this.txtCurrency.Text))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(this.txtExchangeRate.Text))
            {
                return false;
            }
            return result;
        }

        private bool is_Inventory_Detail_Available()
        {
            if (lot_DetailsList.Count() <=0 )
            {
                return false;
            }
            return true;
        }

        private void Save_To_DB()
        {
            Save_Inventory_Lot();
            Save_Inventory_Lot_Detail();
        }

        private void Save_Inventory_Lot()
        {
            Controllers.Inventory_Lot_Controller inventory_Lot_Controller = new Controllers.Inventory_Lot_Controller();
            Models.Inventory_Lot inventory_Lot = new Models.Inventory_Lot();
            inventory_Lot = Get_Inventory_Lot();
            inventory_Lot_Controller.Save_Inventory_Lot(inventory_Lot);
        }

        private void Save_Inventory_Lot_Detail()
        {
            Controllers.Inventory_Lot_Detail_Controller inventory_Lot_Detail_Controller = new Controllers.Inventory_Lot_Detail_Controller();
            List<Models.Inventory_Lot_Detail> inventory_Lot_Details = new List<Models.Inventory_Lot_Detail>();
            inventory_Lot_Details = Get_Inventory_Lot_Detail();
            inventory_Lot_Detail_Controller.Save_LotDetail(inventory_Lot_Details, this.txtArrival.Text);
        }

        #endregion


        private Models.Inventory_Lot Get_Inventory_Lot()
        {
            Models.Inventory_Lot inventory_Lot = new Models.Inventory_Lot {
                Lot_ID = this.txtInventoryNo.Text,
                Purchase_ID = this.txtProductID.Text,
                Supplier_ID = this.txtSupplierID.Text,
                Supplier_Ref = this.txtSupplierRef.Text,
                Arrival_Date = this.txtArrival.Text,
                Currency = this.txtCurrency.Text,
                ExchangeRate = this.txtExchangeRate.Text
            };
            return inventory_Lot;
        }

        private List<Models.Inventory_Lot_Detail> Get_Inventory_Lot_Detail()
        {
            return lot_DetailsList;
        }

        private Models.Supplier GetSupplier()
        {
            return new Models.Supplier
            {
                Supplier_ID = this.txtSupplierID.Text,
                Currency = this.txtCurrency.Text,
                Exchange_Rage = this.txtExchangeRate.Text
            };
        }

        public void clear_Form()
        {
            clear_Inventory_Master_Form();
            clear_Product_Form();
            lot_DetailsList = new List<Models.Inventory_Lot_Detail>();
            this.lstInventory.ItemsSource = lot_DetailsList;
            lstInventory.Items.Refresh();
        }

        private void clear_Inventory_Master_Form()
        {
            
            supplierList = new List<Models.Supplier>();
            productList = new List<Models.Product>();

            this.txtInventoryNo.Text = "";
            this.txtSupplierID.Text = "";
            this.txtSupplierRef.Text = "";
            this.txtCurrency.Text = "";
            this.txtExchangeRate.Text = "";
            this.txtSupplierID.Text = "";
            this.txtOurReference.Text = "";
            this.txtArrival.Text = "";
            this.lblTotal_Items.Content = "";
        }


        private string setDate(string inDate)
        {
            if (!string.IsNullOrWhiteSpace(inDate))
            {
                return converToDate(inDate);
            }
            else
            {
                string strToday = "";
                System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
                strToday = DateTime.Now.ToString("yyyyMMdd", en);
                return converToDate(strToday);
            }
        }

        private string converToDate(string inString)
        {
            string result = "";
            if (Regex.IsMatch(inString, "[0-9]{2}-[a-zA-Z]{3}-[0-9]{4}"))
            {
                result = inString;
            }
            else
            {
                try
                {
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    string dateString = inString;
                    string format = "yyyyMMdd";
                    DateTime dateResult = DateTime.ParseExact(dateString, format, provider);
                    System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
                    result = dateResult.ToString("dd-MMM-yyyy", en);
                    string tempYEAR = dateResult.Year.ToString();

                    if (dateResult.Year != DateTime.Now.Year)
                    {
                        if ((dateResult.Year - 1) != DateTime.Now.Year)
                        {
                            if (dateResult.Year > DateTime.Now.Year)
                            {
                                //System.Windows.MessageBox.Show("您輸入了的年份，是未來" + (dateResult.Year - DateTime.Now.Year) + "年，可能有誤!");
                            }
                            else
                            {
                                //System.Windows.MessageBox.Show("您輸入了的年份，是" + (DateTime.Now.Year - dateResult.Year) + "年前，可能有誤!");
                            }   
                        }
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
            return result;
        }
        
        private void txtArrival_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txtArrival.Text = setDate(this.txtArrival.Text);
        }

        private void txtProductID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtSupplierID.Text))
            {
                productList = new List<Models.Product>();
                Controllers.ProductController productController = new Controllers.ProductController();

                //productController.loadList(this.txtSupplierID.Text);
                if (this.txtSupplierID.Text == "Rayca")
                {
                    productController.loadList();
                }
                else
                {
                    productController.loadList(this.txtSupplierID.Text);
                }

                productList = productController.product_List;
                this.txtProductID.ItemsSource = productList;
                txtProductID.ItemFilter = (txt, i) => Models.Product.IsAutoCompleteSuggestion(txt, i);
            }            
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
                    this.txtNetPrice.Text = tempProductDetail.Cost;
                }

                load_Latest_Price(this.txtProductID.Text);
            }
        }

        private void load_Latest_Price(string inProductID)
        {
            Controllers.Inventory_Lot_Detail_Controller inventory_Lot_Detail_Controller = new Controllers.Inventory_Lot_Detail_Controller();
            Models.Inventory_Lot_Detail inventory_Lot_Detail = inventory_Lot_Detail_Controller.Get_Price_in_Inventory_Lot_Detail(inProductID);
            if (!string.IsNullOrEmpty(inventory_Lot_Detail.NetPrice))
            {
                this.txtNetPrice.Text = inventory_Lot_Detail.NetPrice;
                this.txtUnitPrice.Text = inventory_Lot_Detail.UnitPrice;
            }
        }

        private void txtBrand_KeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtBrand.Text))
            {
                if (e.Key == Key.Enter)
                {
                    if (string.IsNullOrEmpty(this.txtQty.Text))
                    {
                        txtQty.Focusable = true;
                        Keyboard.Focus(txtQty);
                    }
                    else
                    {
                        txtNetPrice.Focusable = true;
                        Keyboard.Focus(txtNetPrice);
                    }
                }
            }
        }

        private void txtNetPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(this.txtNetPrice.Text))
                {
                    SetNetPrice();
                }
                else
                {
                    add_to_InventoryItems();
                }
            }else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                SaveRecord();
            }
        }

        private void txtNetPrice_LostFocus(object sender, RoutedEventArgs e)
        {
            SetNetPrice();
        }

        private void SetNetPrice()
        {
            if ((!string.IsNullOrEmpty(this.txtUnitPrice.Text)) && (!string.IsNullOrEmpty(this.txtExchangeRate.Text)))
            {
                if (string.IsNullOrEmpty(this.txtNetPrice.Text.Trim()))
                {
                    this.txtNetPrice.Text = caculate_NetPrice(this.txtExchangeRate.Text,this.txtUnitPrice.Text);
                }
            }
        }

        private string caculate_NetPrice(string inExchangeRate, string inUnitPrice)
        {
            return Convert.ToString(Math.Ceiling(Convert.ToDecimal(inExchangeRate) * Convert.ToDecimal(inUnitPrice)));
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

        private void txtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtQty.Text))
            {
                if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.L)
                {
                    add_to_InventoryItems();
                }
                else if (e.Key == Key.Enter)
                {
                    if (!string.IsNullOrEmpty(this.txtQty.Text))
                    {
                        //add_to_InventoryItems();
                        this.txtUnitPrice.Focusable = true;
                        Keyboard.Focus(txtUnitPrice);
                    }
                }
            }
        }

        private Models.Inventory_Lot_Detail Get_Lot_Detail_Item()
        {
            return new Models.Inventory_Lot_Detail {
                Lot_ID = this.txtInventoryNo.Text,
                Product_ID = this.txtProductID.Text,
                Qty = this.txtQty.Text,
                UnitPrice = this.txtUnitPrice.Text,
                NetPrice = this.txtNetPrice.Text,
                Unit = this.txtUnit.Text,
                Remark = this.txtRemark.Text,
                Exp_Date = this.txtExpDate.Text
            };
        }

        private void clear_Product_Form()
        {
            this.txtProductID.Text = "";
            this.txtBrand.Text = "";
            this.txtModel.Text = "";
            this.txtDescribtion.Text = "";
            this.txtUnit.Text = "";
            this.txtUnitPrice.Text = "";
            this.txtNetPrice.Text = "";
            this.txtQty.Text = "";
            this.txtRemark.Text = "";
            this.txtExpDate.Text = "";
            this.lbl_TotalAmount.Content = "";
        }

        private void add_to_InventoryItems()
        {
            if ((!string.IsNullOrEmpty(this.txtNetPrice.Text)) && (!string.IsNullOrEmpty(this.txtQty.Text)) && (!string.IsNullOrEmpty(this.txtProductID.Text)))
            {
                this.lot_DetailsList.Add(Get_Lot_Detail_Item());
                this.lstInventory.ItemsSource = lot_DetailsList;
                lstInventory.Items.Refresh();
                to_LastRow_of_LstInventory();
                clear_Product_Form();
                count_Total_Items();
                //this.lblNum_Of_Record.Content = invoiceItem_List.Count() + "Set(s)";
                //totalOrderAmount();

                this.txtProductID.Focusable = true;
                Keyboard.Focus(txtProductID);
            }
        }

        private string theYear()
        {
            System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
            var result = DateTime.Now.ToString("yyyy", en);
            return result.ToString();
        }

        private void txtInventoryNo_LostFocus(object sender, RoutedEventArgs e)
        {
            string result = "";

            if (!string.IsNullOrWhiteSpace(this.txtInventoryNo.Text))
            {
                if ((this.txtInventoryNo.Text.Trim().Length > 0) && (this.txtInventoryNo.Text.Trim().Length <= 3))
                {
                    int temp;
                    if (int.TryParse(this.txtInventoryNo.Text.Trim(), out temp))
                    {
                        result = string.Format("{0:0000}", temp);
                        this.txtInventoryNo.Text = "STO-" + theYear() + result;
                    }
                }
            }
            else
            {
                NewInventoryNumber();
            }
            checkInventoryExit();
        }

        private void NewInventoryNumber()
        {
            Controllers.Inventory_Lot_Controller inventoryController = new Controllers.Inventory_Lot_Controller();
            Controllers.Functional.Inventory_No_Creator inventory_No_Creator = new Controllers.Functional.Inventory_No_Creator(inventoryController.Get_Max_InventoryNo());

            this.txtInventoryNo.Text = inventory_No_Creator.NewRef();
        }

        private void lstInventory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            get_Back_Items_From_List();
        }

        private void lstInventory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                //MessageBox.Show("Delete action activate !");
                Models.Inventory_Lot_Detail item = lstInventory.SelectedItem as Models.Inventory_Lot_Detail;

                if (item != null)
                {
                    var index = lstInventory.SelectedIndex;
                    //MessageBox.Show("The index is: " + index);
                    lot_DetailsList.RemoveAt(index);
                    lstInventory.Items.Refresh();

                    //this.lblNum_Of_Record.Content = invoiceItem_List.Count() + "Set(s)";
                    //totalOrderAmount();
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            add_to_InventoryItems();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clear_Product_Form();
        }

        private void checkInventoryExit()
        {
            Controllers.Inventory_Lot_Controller inventoryController = new Controllers.Inventory_Lot_Controller();
            Models.Inventory_Lot tempInventory = new Models.Inventory_Lot();
            if (inventoryController.checkInventoryExit(this.txtInventoryNo.Text))
            {
                tempInventory = inventoryController.GetInventory(this.txtInventoryNo.Text);
                loadInventory(tempInventory);
            }
        }

        private void loadInventory(Models.Inventory_Lot inInventoryLot)
        {
            loadInventoryHeader(inInventoryLot);
            loadInvoice_Detail(inInventoryLot);
        }

        private void loadInventoryHeader(Models.Inventory_Lot inInventoryLot)
        {
            this.txtInventoryNo.Text = inInventoryLot.Lot_ID;
            this.txtSupplierID.Text = inInventoryLot.Supplier_ID;
            this.txtCurrency.Text = inInventoryLot.Currency;
            this.txtExchangeRate.Text = inInventoryLot.ExchangeRate;
            this.txtSupplierRef.Text = inInventoryLot.Supplier_Ref;
            this.txtOurReference.Text = inInventoryLot.Purchase_ID;
            this.txtArrival.Text = inInventoryLot.Arrival_Date;
        }

        private void loadInvoice_Detail(Models.Inventory_Lot inInventoryLot)
        {
            Controllers.Inventory_Lot_Detail_Controller inventory_Detail_Controller = new Controllers.Inventory_Lot_Detail_Controller();
            lot_DetailsList = inventory_Detail_Controller.Load_ItemList(inInventoryLot.Lot_ID);
            this.lstInventory.ItemsSource = lot_DetailsList;
                        
            this.lstInventory.Items.Refresh();
            to_LastRow_of_LstInventory();
            count_Total_Items();
            //this.lblNum_Of_Record.Content = invoiceItem_List.Count() + "Set(s)";
            //totalOrderAmount();
        }

        private void txtCurrency_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.txtSupplierRef.Focusable = true;
                Keyboard.Focus(txtSupplierRef);
            }
        }

        private void add_to_InventoryItem()
        {
            if ((!string.IsNullOrEmpty(this.txtNetPrice.Text)) && (!string.IsNullOrEmpty(this.txtQty.Text)) && (!string.IsNullOrEmpty(this.txtProductID.Text)))
            {
                this.lot_DetailsList.Add(Get_Lot_Detail_Item());
                this.lstInventory.ItemsSource = lot_DetailsList;
                lstInventory.Items.Refresh();
                to_LastRow_of_LstInventory();
                clear_Product_Form();
                //this.lblNum_Of_Record.Content = invoiceItem_List.Count() + "Set(s)";
                //totalOrderAmount();
                
                txtProductID.Focusable = true;
                Keyboard.Focus(txtProductID);
            }
        }

        private Models.Product get_Product_Item(List<Models.Product> inProducts, string inProductID)
        {
            Models.Product product = new Models.Product();

            var temp_Product =  from theProduct in inProducts
                                where theProduct.Product_ID == inProductID
                                select theProduct;
            //顯示查詢結果
            foreach (Models.Product result_Product in temp_Product)
            {
                product = new Models.Product {
                    Product_ID = result_Product.Product_ID,
                    Brand = result_Product.Brand,
                    Cost = result_Product.Cost,
                    Model =result_Product.Model,
                    Unit = result_Product.Unit,
                    Unit_Price = result_Product.Unit_Price,
                    Product_Descruibtions = result_Product.Product_Descruibtions
                }; 
            }
            return product;
        }

        private void get_Back_Items_From_List()
        {
            Models.Inventory_Lot_Detail item = this.lstInventory.SelectedItem as Models.Inventory_Lot_Detail;
            if (item != null)
            {
                Models.Product product = get_Product_Item(productList, item.Product_ID);
                this.txtRemark.Text = item.Remark;
                this.txtNetPrice.Text = item.NetPrice;
                this.txtQty.Text = item.Qty;
                this.txtProductID.Text = item.Product_ID;
                this.txtBrand.Text = product.Brand;
                this.txtModel.Text = product.Model;
                this.txtDescribtion.Text = product.Product_Descruibtions;
                this.txtUnit.Text = item.Unit;
                this.txtUnitPrice.Text = item.UnitPrice;
                this.txtNetPrice.Text = item.NetPrice;
                this.txtExpDate.Text = item.Exp_Date;

                var index = lstInventory.SelectedIndex;
                //MessageBox.Show("The index is: " + index);
                lot_DetailsList.RemoveAt(index);
                lstInventory.Items.Refresh();

                //this.lblNum_Of_Record.Content = invoiceItem_List.Count() + "Set(s)";
                //totalOrderAmount();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveRecord();
        }

        private void txtRemark_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.L)
            {
                add_to_InventoryItems();
            }
        }

        private void txtSupplierID_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void setExpDate()
        {
            if (!string.IsNullOrEmpty(this.txtExpDate.Text))
            {
                this.txtExpDate.Text = setDate(this.txtExpDate.Text);
            }
        }

        private void txtExpDate_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                setExpDate();
                add_to_InventoryItems();
            }else if(Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                setExpDate();
                SaveRecord();
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            clear_Form();
        }

        private void txtUnitPrice_LostFocus(object sender, RoutedEventArgs e)
        {
            set_UnitPirce();
        }

        private void txtUnitPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) || (e.Key == Key.Tab))
            {
                //set_UnitPirce();
                this.txtNetPrice.Focusable = true;
                Keyboard.Focus(txtNetPrice);
            }
        }

        private void set_UnitPirce()
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
                else if (str_Proccessing.IndexOf("*") == 0)
                {
                    str_Proccessing = str_Proccessing.Replace("*", "");
                    var result2 = Convert.ToDecimal(str_Proccessing) / Convert.ToDecimal(this.txtQty.Text);
                    this.txtUnitPrice.Text = Convert.ToString(result2);
                }
            }
        }

        private void txtProductID_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                SaveRecord();
            }
        }

        private void to_LastRow_of_LstInventory()
        {
            //this.lstInventory.ScrollIntoView(this.lstInventory.Items.Count - 1);
            if (lot_DetailsList.Count > 0)
            {
                this.lstInventory.ScrollIntoView(lot_DetailsList[lot_DetailsList.Count - 1]);
            }
            else
            {
                this.lstInventory.ScrollIntoView(10);
            }

        }

        private void count_Total_Items()
        {
            this.lblTotal_Items.Content = this.lot_DetailsList.Count();
            this.lbl_TotalAmount.Content = detailList_total();
        }

        private string detailList_total()
        {
            decimal numSum = 0;
            foreach (Models.Inventory_Lot_Detail tempInventoryDetail in lot_DetailsList)
            {
                decimal tempSum = Convert.ToDecimal(tempInventoryDetail.Qty) * Convert.ToDecimal(tempInventoryDetail.UnitPrice);
                numSum += tempSum;
            }
            return numSum.ToString();
        }
    }
}
