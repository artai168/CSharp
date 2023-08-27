using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace TopStar_Invoice_Maker_SQLSever.Views.UI.Invoice
{
    /// <summary>
    /// UC_Invoice.xaml 的互動邏輯
    /// </summary>
    public partial class UC_Invoice : UserControl
    {
        private List<Models.Clinic> clinicList;
        private List<string> delivery_List;
        private List<string> payment_Terms_List;
        private List<string> paymentMethod_List;
        private List<string> salesPersonList;
        private List<Models.Product> productList;
        private List<string> unit_List;
        private List<Models.Invoice_Item> invoiceItem_List;

        public UC_Invoice()
        {
            InitProcedures();
        }

        private void InitProcedures()
        {
            InitializeComponent();
            load_PaymentTerms_List();
            this.txtPayment.ItemsSource = payment_Terms_List;
            load_Delivery_List();
            this.txtDelivery.ItemsSource = delivery_List;
            delivery_List = new List<string>();
            load_PaymentMethod_List();
            this.txtPaymentMethod.ItemsSource = paymentMethod_List;
            load_salesPerson_List();
            this.txtSalesPerson.ItemsSource = salesPersonList;
            invoiceItem_List = new List<Models.Invoice_Item>();
        }

        private void load_PaymentTerms_List()
        {
            payment_Terms_List = new List<string>();
            this.payment_Terms_List.Add("C.O.D");
            this.payment_Terms_List.Add("CASH or CHEQUE");
            this.payment_Terms_List.Add("MONTHLY");
        }

        private void load_Delivery_List()
        {
            delivery_List = new List<string>();
            this.delivery_List.Add("BY HAND");
            this.delivery_List.Add("COURIER");
        }

        private void load_salesPerson_List()
        {
            salesPersonList = new List<string>();
            this.salesPersonList.Add("Dennies Wong");
            this.salesPersonList.Add("Kevin Cheng");
        }

        private void load_PaymentMethod_List()
        {
            paymentMethod_List = new List<string>();
            this.paymentMethod_List.Add("CASH");
            this.paymentMethod_List.Add("CHEQUE");
        }

        public void loadInvoice(Models.Invoice inInvoice)
        {
            loadInvoiceHeader(inInvoice);
            loadInvoice_Detail(inInvoice);
        }

        private void loadInvoiceHeader(Models.Invoice inInvoice)
        {
            this.txt_Clinic.Text = inInvoice.Clinic_Name;
            this.txt_Clinic_Contact.Text = inInvoice.ContactPerson;
            this.txt_Clinic_Add.Text = inInvoice.Address;
            this.txt_Clinic_Tel.Text = inInvoice.Telephone;
            this.txtDistrict.Text = inInvoice.District;
            this.txtPayment.Text = inInvoice.Payment_Terms;
            this.txtDelivery.Text = inInvoice.Delivery;
            this.txt_ShipTo_Clinic.Text = inInvoice.SHIPTO_Clinic_Name;
            this.txt_ShipTo_Contact.Text = inInvoice.SHIPTO_ContactPerson;
            this.txt_ShipTo_Add.Text = inInvoice.SHIPTO_Address;
            this.txt_ShipTo_Tel.Text = inInvoice.SHIPTO_Telephone;
            this.txtClinicID.Text = inInvoice.Clinic_ID;
            this.txtInvoice.Text = inInvoice.Invoice_ID;
            this.txtInvoiceDate.Text = inInvoice.Invoice_Date;
            this.txtSalesPerson.Text = inInvoice.Sales_Person;
            this.txtInvoiceRemark.Text = inInvoice.Remark;
            this.txtDiscount.Text = inInvoice.Discount;
            this.txtCustomerRefCode.Text = inInvoice.CustomerReferenceCode;
            this.txtDueDate.Text = inInvoice.Due_Date;
        }

        private void loadInvoice_Detail(Models.Invoice inInvoice)
        {
            invoiceItem_List = inInvoice.ItemList;
            this.lstInvoiceItem.ItemsSource = invoiceItem_List;
            lstInvoiceItem.Items.Refresh();
            this.lblNum_Of_Record.Content = invoiceItem_List.Count() + "Set(s)";
            totalOrderAmount();
        }

        private void txtClinicID_GotFocus(object sender, RoutedEventArgs e)
        {
            clinicList = new List<Models.Clinic>();
            Controllers.ClinicController clinicController = new Controllers.ClinicController();
            //loadJOBs();
            clinicController.loadList();

            clinicList = clinicController.clinic_List;

            this.txtClinicID.ItemsSource = clinicList;
            txtClinicID.ItemFilter = (txt, i) => Models.Clinic.IsAutoCompleteSuggestion(txt, i);
        }

        private void txtClinicID_LostFocus(object sender, RoutedEventArgs e)
        {
            var myLINQResult = from clinic_Detail in clinicList
                               where clinic_Detail.Clinic_ID == this.txtClinicID.Text
                               select clinic_Detail;

            foreach (Models.Clinic temp_clinic_Detail in myLINQResult)
            {
                this.txt_Clinic.Text = temp_clinic_Detail.Clinic_Name;
                this.txt_Clinic_Contact.Text = temp_clinic_Detail.ContactPerson;
                this.txt_Clinic_Add.Text = temp_clinic_Detail.Address;
                this.txt_Clinic_Tel.Text = temp_clinic_Detail.Telephone;
                this.txtDistrict.Text = temp_clinic_Detail.District;
                this.txtPayment.Text = temp_clinic_Detail.Payment_Terms;
                this.txtDelivery.Text = temp_clinic_Detail.Delivery;
                this.txt_ShipTo_Clinic.Text = temp_clinic_Detail.SHIPTO_Clinic_Name;
                this.txt_ShipTo_Contact.Text = temp_clinic_Detail.SHIPTO_ContactPerson;
                this.txt_ShipTo_Add.Text = temp_clinic_Detail.SHIPTO_Address;
                this.txt_ShipTo_Tel.Text = temp_clinic_Detail.SHIPTO_Telephone;
                this.txtSalesPerson.Text = temp_clinic_Detail.SalesPerson;
            }
        }

        private void cleanForm_Clinic()
        {
            this.txt_Clinic.Text = "";
            this.txt_Clinic_Contact.Text = "";
            this.txt_Clinic_Add.Text = "";
            this.txt_Clinic_Tel.Text = "";
            this.txtDistrict.Text = "";
            this.txtPayment.Text = "";
            this.txtDelivery.Text = "";
            this.txt_ShipTo_Clinic.Text = "";
            this.txt_ShipTo_Contact.Text = "";
            this.txt_ShipTo_Add.Text = "";
            this.txt_ShipTo_Tel.Text = "";
            this.txtClinicID.Text = "";
            this.txtInvoice.Text = "";
            this.txtInvoiceDate.Text = "";
            this.txtSalesPerson.Text = "";
            this.txtRemark.Text = "";
            this.txtCustomerRefCode.Text = "";
            this.txtDiscount.Text = "";
            this.txtDeliveryDate.Text = "";
            this.lblNum_Of_Record.Content = "";
            this.lblTotalAmount.Content = "";
            this.txtDueDate.Text = "";
        }

        private Models.Invoice GetInvoice()
        {
            string str_SubTotal = Convert.ToString(caculate_Total_Order_Amount());
            string str_Total = the_Final_Total(str_SubTotal, this.txtDiscount.Text);

            return new Models.Invoice
            {
                Clinic_Name = this.txt_Clinic.Text,
                ContactPerson = this.txt_Clinic_Contact.Text,
                Address = this.txt_Clinic_Add.Text,
                Telephone = this.txt_Clinic_Tel.Text,
                District = this.txtDistrict.Text,
                Payment_Terms = this.txtPayment.Text,
                Delivery = this.txtDelivery.Text,
                SHIPTO_Clinic_Name = this.txt_ShipTo_Clinic.Text,
                SHIPTO_ContactPerson = this.txt_ShipTo_Contact.Text,
                SHIPTO_Address = this.txt_ShipTo_Add.Text,
                SHIPTO_Telephone = this.txt_ShipTo_Tel.Text,
                Clinic_ID = this.txtClinicID.Text,
                Invoice_ID = this.txtInvoice.Text,
                Invoice_Date = this.txtInvoiceDate.Text,
                Sales_Person = this.txtSalesPerson.Text,
                Remark = this.txtInvoiceRemark.Text,
                Discount = this.txtDiscount.Text,
                Sub_Total = str_SubTotal,
                Total = str_Total,
                Due_Date = this.txtDueDate.Text,
                INV_Status = "OPEN",
                CustomerReferenceCode = this.txtCustomerRefCode.Text
            };
        }

        private void txtInvoice_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtProductID_GotFocus(object sender, RoutedEventArgs e)
        {
            productList = new List<Models.Product>();
            Controllers.ProductController productController = new Controllers.ProductController();

            productController.loadList();

            productList = productController.product_List;

            this.txtProductID.ItemsSource = productList;
            txtProductID.ItemFilter = (txt, i) => Models.Product.IsAutoCompleteSuggestion(txt, i);
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
                    lblCost.Content = tempProductDetail.Cost;
                }

            }
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

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clean_Product_Form();
        }

        private void clean_Invoice_Items()
        {
            this.txtInvoiceRemark.Text = "";
            this.invoiceItem_List = new List<Models.Invoice_Item>(); ;
            this.lstInvoiceItem.ItemsSource = null;
            lstInvoiceItem.Items.Refresh();
        }
        private void clean_Product_Form()
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
            this.lblCost.Content = "";
        }

        private void add_to_InvoiceItem()
        {
            if ((!string.IsNullOrEmpty(this.txtNetPrice.Text)) && (!string.IsNullOrEmpty(this.txtQty.Text)) && (!string.IsNullOrEmpty(this.txtProductID.Text)))
            {
                this.invoiceItem_List.Add(getInvoiceItem());
                this.lstInvoiceItem.ItemsSource = invoiceItem_List;
                lstInvoiceItem.Items.Refresh();
                clean_Product_Form();
                this.lblNum_Of_Record.Content = invoiceItem_List.Count() + "Set(s)";
                totalOrderAmount();
            }
        }


        private Models.Invoice_Item getInvoiceItem()
        {
            return new Models.Invoice_Item
            {
                Item_ID = "",
                Item_Remark = this.txtRemark.Text,
                Net_Price = this.txtNetPrice.Text,
                Qty = this.txtQty.Text,
                Total = Convert.ToString(Convert.ToDouble(this.txtNetPrice.Text) * Convert.ToDouble(this.txtQty.Text)),
                Product_ID = this.txtProductID.Text,
                Brand = this.txtBrand.Text,
                Model = this.txtModel.Text,
                Product_Descruibtions = this.txtDescribtion.Text,
                Unit = this.txtUnit.Text,
                Unit_Price = this.txtUnitPrice.Text,
                Invoice_ID = this.txtInvoice.Text
            };
        }

        private void lstInvoiceItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Models.Invoice_Item item = lstInvoiceItem.SelectedItem as Models.Invoice_Item;

            if (item != null)
            {
                this.txtRemark.Text = item.Item_Remark;
                this.txtNetPrice.Text = item.Net_Price;
                this.txtQty.Text = item.Qty;
                this.txtProductID.Text = item.Product_ID;
                this.txtBrand.Text = item.Brand;
                this.txtModel.Text = item.Model;
                this.txtDescribtion.Text = item.Product_Descruibtions;
                this.txtUnit.Text = item.Unit;
                this.txtUnitPrice.Text = item.Unit_Price;

                var index = lstInvoiceItem.SelectedIndex;
                //MessageBox.Show("The index is: " + index);
                invoiceItem_List.RemoveAt(index);
                lstInvoiceItem.Items.Refresh();

                this.lblNum_Of_Record.Content = invoiceItem_List.Count() + "Set(s)";
                totalOrderAmount();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            add_to_InvoiceItem();
        }

        private void txtInvoiceDate_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txtInvoiceDate.Text = setDate(this.txtInvoiceDate.Text);
        }

        private void txtDeliveryDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtDeliveryDate.Text))
            {
                if (!string.IsNullOrWhiteSpace(this.txtInvoiceDate.Text))
                {
                    this.txtDeliveryDate.Text = this.txtInvoiceDate.Text;
                    this.txtDueDate.Text = converToDate(nextMonth(this.txtInvoiceDate.Text));
                }
            }
            else
            {
                this.txtDeliveryDate.Text = setDate(this.txtDeliveryDate.Text);
                this.txtDueDate.Text = converToDate(nextMonth(this.txtInvoiceDate.Text));
            }

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

        private string nextMonth(string inString)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(inString))
            {
                DateTime date = DateTime.Parse(inString);
                DateTime nextMonth = date.AddDays(1).AddMonths(1).AddDays(-1);
                System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
                result = nextMonth.ToString("yyyyMMdd", en);
            }
            return result;
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
                                System.Windows.MessageBox.Show("您輸入了的年份，是未來" + (dateResult.Year - DateTime.Now.Year) + "年，可能有誤!");
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("您輸入了的年份，是" + (DateTime.Now.Year - dateResult.Year) + "年前，可能有誤!");
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

        private void txtInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Controllers.InvoiceController invoiceController = new Controllers.InvoiceController();
            Controllers.Functional.Invoice_No_Creator invoice_No_Creator = new Controllers.Functional.Invoice_No_Creator(invoiceController.Get_Max_InvoiceNo());

            this.txtInvoice.Text = invoice_No_Creator.NewRef();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveInvoice();
        }

        private void SaveInvoice()
        {
            // && (!String.IsNullOrEmpty(this.txt_Clinic.Text)) && (!String.IsNullOrEmpty(this.txt_Clinic_Add.Text))
            if ((!string.IsNullOrWhiteSpace(this.txtInvoice.Text)) && (this.txtInvoice.Text.Length >= 12))
            {
                if ((!String.IsNullOrEmpty(this.txtClinicID.Text)) && (!String.IsNullOrEmpty(this.txt_Clinic.Text)) && (!String.IsNullOrEmpty(this.txt_Clinic_Add.Text)))
                {
                    //---  NEW LOGIC
                    if ((!string.IsNullOrWhiteSpace(this.txtProductID.Text))
                        && (!string.IsNullOrWhiteSpace(this.txtBrand.Text))
                        && (!string.IsNullOrWhiteSpace(this.txtModel.Text))
                        && (!string.IsNullOrWhiteSpace(this.txtDescribtion.Text))
                        && (!string.IsNullOrWhiteSpace(this.txtUnit.Text))
                        && (!string.IsNullOrWhiteSpace(this.txtUnitPrice.Text))
                        && (!string.IsNullOrWhiteSpace(this.txtNetPrice.Text))
                        && (!string.IsNullOrWhiteSpace(this.txtQty.Text))
                        )
                    {
                        //-- END OF NEW LOGIC
                        if ((invoiceItem_List.Count > 0) || (invoiceItem_List != null))
                        {
                            add_to_InvoiceItem();
                            Controllers.InvoiceController invoiceController = new Controllers.InvoiceController();
                            Models.Invoice theInvoice = new Models.Invoice();
                            theInvoice = GetInvoice();
                            theInvoice.ItemList = invoiceItem_List;

                            invoiceController.SaveInvoice(theInvoice);

                            MessageBox.Show("Record Saved!");
                            clean_Product_Form();
                            cleanForm_Clinic();
                            clean_Invoice_Items();
                        }
                        else
                        {
                            MessageBox.Show("Please finish the Form!");
                        }
                    }
                    else
                    {
                        if ((invoiceItem_List.Count > 0) && (invoiceItem_List != null))
                        {
                            Controllers.InvoiceController invoiceController = new Controllers.InvoiceController();
                            Models.Invoice theInvoice = new Models.Invoice();
                            theInvoice = GetInvoice();
                            theInvoice.ItemList = invoiceItem_List;

                            invoiceController.SaveInvoice(theInvoice);

                            MessageBox.Show("Invoice: " + theInvoice.Invoice_ID + "\r\nClinic: " + theInvoice.Clinic_Name + " \r\n\r\nSaved!");
                            clean_Product_Form();
                            cleanForm_Clinic();
                            clean_Invoice_Items();
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Please finish the Form!");
                }
            }

        }



        private void txtInvoice_LostFocus(object sender, RoutedEventArgs e)
        {
            string result = "";

            if (!string.IsNullOrWhiteSpace(this.txtInvoice.Text))
            {
                if ((this.txtInvoice.Text.Trim().Length > 0) && (this.txtInvoice.Text.Trim().Length <= 3))
                {
                    int temp;
                    if (int.TryParse(this.txtInvoice.Text.Trim(), out temp))
                    {
                        result = string.Format("{0:0000}", temp);
                        this.txtInvoice.Text = "TS-S" + theYear() + result;
                    }
                }
            }

            checkInvoiceExit();
        }

        private void checkInvoiceExit()
        {
            Controllers.InvoiceController invoiceController = new Controllers.InvoiceController();
            Models.Invoice tempInvoice = new Models.Invoice();
            if (invoiceController.invoiceExit(this.txtInvoice.Text))
            {
                tempInvoice = invoiceController.GetInvoice(this.txtInvoice.Text);
                loadInvoice(tempInvoice);
            }
        }

        private void btnNewInvoice_Click(object sender, RoutedEventArgs e)
        {
            InitProcedures();
            clean_Product_Form();
            cleanForm_Clinic();
            clean_Invoice_Items();

        }

        private void txtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.L)
            {
                add_to_InvoiceItem();
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                SaveInvoice();
            }

        }

        private void txtRemark_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.L)
            {
                add_to_InvoiceItem();
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            {
                SaveInvoice();
            }
        }

        private void totalOrderAmount()
        {

            this.lblTotalAmount.Content = string.Format("{0:C2}", caculate_Total_Order_Amount());
        }

        private decimal caculate_Total_Order_Amount()
        {
            decimal totalAmount = 0;
            foreach (Models.Invoice_Item invoiceItem in invoiceItem_List)
            {
                totalAmount += Convert.ToDecimal(invoiceItem.Total);
            }
            return totalAmount;
        }

        private string the_Final_Total(string in_SubTotal, string in_Discount)
        {
            decimal dec_SubTotal = Convert.ToDecimal(in_SubTotal);
            decimal dec_Discount = 0;
            if (!string.IsNullOrEmpty(in_Discount.Trim()))
            {
                dec_Discount = Convert.ToDecimal(in_Discount);
            }

            return Convert.ToString(dec_SubTotal - dec_Discount);
        }

        private string theYear()
        {
            System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
            var result = DateTime.Now.ToString("yyyy", en);
            return result.ToString();
        }

        private void txtInvoiceRemark_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((!string.IsNullOrWhiteSpace(this.txtInvoiceRemark.Text)) && (!string.IsNullOrWhiteSpace(this.txtInvoiceDate.Text)))
            {
                if (this.txtInvoiceRemark.Text.Trim().ToUpper() == "##WARRANTY##")
                {
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    string dateString = this.txtInvoiceDate.Text;
                    string format = "dd-MMM-yyyy";
                    DateTime dateResult = DateTime.ParseExact(dateString, format, provider);
                    dateResult = dateResult.AddYears(1);
                    System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
                    string result = dateResult.ToString("dd-MMM-yyyy", en);

                    this.txtInvoiceRemark.Text = "One year product warranty covered for the following item(s) until " + result;
                }
            }
        }

        private void txtNetPrice_LostFocus(object sender, RoutedEventArgs e)
        {
            //var result2 = CSharpScript.EvaluateAsync(" 5 * 7").Result;
            if (!string.IsNullOrWhiteSpace(this.txtNetPrice.Text))
            {
                string str_Proccessing = this.txtNetPrice.Text;
                if (str_Proccessing.IndexOf("=") == 0)
                {
                    str_Proccessing = str_Proccessing.Replace("=", "");
                    var result2 = CSharpScript.EvaluateAsync(str_Proccessing).Result;
                    this.txtNetPrice.Text = Convert.ToString(result2);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.txtUnitPrice.Text.Trim()))
                {
                    this.txtNetPrice.Text = this.txtUnitPrice.Text;
                }
            }
        }
    }
}
