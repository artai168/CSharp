using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TopStar_Invoice_Maker_SQLSever.Views.UI.Receivable
{
    /// <summary>
    /// UC_Receiable.xaml 的互動邏輯
    /// </summary>
    public partial class UC_Receiable : UserControl
    {
        private List<Models.Clinic> clinicList;
        protected List<Models.Invoice> invoiceList_inTextBox; //txtInvoiceID item source
        protected List<Models.Invoice> _Invoice_List; // List invoice item source
        //private List<Models.Invoice> _Invoice_List_inListItems;
        private List<string> paymentMethod_List;

        public UC_Receiable()
        {
            InitializeComponent();

            load_PaymentMethod_List();
            this.txtPaymentMethod.ItemsSource = paymentMethod_List;
            invoiceList_inTextBox = new List<Models.Invoice>();
            _Invoice_List = new List<Models.Invoice>();
            lstInvoice.ItemsSource = _Invoice_List;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cleanForm();
        }

        private void cleanForm()
        {
            this.txtPaymentDate.Text = "";
            this.txtPaymentMethod.Text = "";
            this.txtPayment.Text = "";
            this.txtClinicID.Text = "";
            this.txtInvoiceID.Text = "";
            this.txtRemark.Text = "";
            this.txtCharges.Text = "";
            this.lblTotal.Content = "";
            this.txtReceivableID.Text = "";
            this.txtBankinDate.Text = "";
            this._Invoice_List = new List<Models.Invoice>(); ;
            this.invoiceList_inTextBox = new List<Models.Invoice>();
            this.lstInvoice.ItemsSource = null;
        }

        private string setDate(string inDate)
        {
            if (!string.IsNullOrWhiteSpace(inDate))
            {
                string pattern = @"(\d{2}-[a-z]{3}-\d{4})";
                Match match = Regex.Match(inDate, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    return inDate;
                }
                else
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
            try
            {
                System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                string dateString = inString;
                string format = "yyyyMMdd";
                DateTime dateResult = DateTime.ParseExact(dateString, format, provider);
                System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
                result = dateResult.ToString("dd-MMM-yyyy", en);
                string tempYEAR = dateResult.Year.ToString();
                return result;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            return result;
        }

        private void load_PaymentMethod_List()
        {
            paymentMethod_List = new List<string>();
            this.paymentMethod_List.Add("CASH");
            this.paymentMethod_List.Add("CHEQUE");
            this.paymentMethod_List.Add("BANK TRANSFER");
            this.paymentMethod_List.Add("TT");
        }

        private void txtInvoiceID_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void txtInvoiceID_LostFocus(object sender, RoutedEventArgs e)
        {
            addToList();
            this.txtInvoiceID.Text = "";
        }

        private void txtClinicID_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txtInvoiceID.Text = "";
            if (!string.IsNullOrEmpty(this.txtClinicID.Text))
            {
                invoiceList_inTextBox = new List<Models.Invoice>();
                Controllers.InvoiceController invoiceController = new Controllers.InvoiceController();
                invoiceList_inTextBox = invoiceController.get_opened_InvoiceItems(this.txtClinicID.Text);

                this.txtInvoiceID.ItemsSource = invoiceList_inTextBox;
                txtInvoiceID.ItemFilter = (txt, i) => Models.Invoice.IsAutoCompleteSuggestion(txt, i);
            }
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

        private void txtPaymentDate_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txtPaymentDate.Text = setDate(this.txtPaymentDate.Text);
        }

        private void txtBankinDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtBankinDate.Text))
            {
                this.txtBankinDate.Text = this.txtPaymentDate.Text;
            }
            else
            {
                this.txtBankinDate.Text = setDate(this.txtBankinDate.Text);
            }
        }

        private void txtBankinDate_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void lstInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Models.Invoice item = lstInvoice.SelectedItem as Models.Invoice;

            if (item != null)
            {
                this.txtInvoiceID.Text = item.Invoice_ID;

                var index = lstInvoice.SelectedIndex;
                //MessageBox.Show("The index is: " + index);
                _Invoice_List.RemoveAt(index);
                lstInvoice.Items.Refresh();
                updateBalance();
                invoiceList_inTextBox.Add(item);
                this.txtInvoiceID.ItemsSource = null;
                this.txtInvoiceID.ItemsSource = invoiceList_inTextBox;
            }
        }

        private void updateBalance()
        {
            this.lblTotal.Content = Convert.ToString(caculate_Total_Order_Amount());
        }

        private decimal caculate_Total_Order_Amount()
        {
            decimal totalAmount = 0;
            foreach (Models.Invoice invoiceItem in _Invoice_List)
            {
                totalAmount += Convert.ToDecimal(invoiceItem.Total);
            }
            return totalAmount;
        }

        private Models.Receivable getReceivable()
        {
            return new Models.Receivable
            {
                Receivable_Date = this.txtPaymentDate.Text,
                Receivable_Method = this.txtPaymentMethod.Text,
                Receivable_Amount = this.txtPayment.Text,
                Receivable_Customer_ID = this.txtClinicID.Text,
                Receivable_Remark = this.txtRemark.Text,
                Receivable_Bankin_Date = this.txtBankinDate.Text,
                Receivable_Record_ID = this.txtReceivableID.Text,
                Receivable_Charges = this.txtCharges.Text
            };
        }

        private void setReceivable(Models.Receivable inReceivable)
        {
            this.txtPaymentDate.Text = inReceivable.Receivable_Date;
            this.txtPaymentMethod.Text = inReceivable.Receivable_Method;
            this.txtPayment.Text = inReceivable.Receivable_Amount;
            this.txtClinicID.Text = inReceivable.Receivable_Customer_ID;
            this.txtRemark.Text = inReceivable.Receivable_Remark;
            this.txtBankinDate.Text = inReceivable.Receivable_Bankin_Date;
            this.txtReceivableID.Text = inReceivable.Receivable_Record_ID;
            this.txtCharges.Text = inReceivable.Receivable_Charges;
        }

        private void addToList()
        {
            Models.Invoice invoiceItem_to_Remove = new Models.Invoice();

            if (!string.IsNullOrEmpty(this.txtInvoiceID.Text))
            {
                foreach (Models.Invoice invoiceItem in invoiceList_inTextBox)
                {
                    if (invoiceItem.Invoice_ID == this.txtInvoiceID.Text)  
                    {
                        _Invoice_List.Add(invoiceItem);
                        this.lstInvoice.ItemsSource = _Invoice_List;
                        lstInvoice.Items.Refresh();
                        updateBalance();
                        invoiceItem_to_Remove = invoiceItem;
                    }
                }
                invoiceList_inTextBox.Remove(invoiceItem_to_Remove);
                txtInvoiceID.ItemsSource = null;
                txtInvoiceID.ItemsSource = invoiceList_inTextBox;
            }
        }

        private void btnAddToList_Click(object sender, RoutedEventArgs e)
        {
            addToList();
        }

        private void txtPaymentDate_GotFocus(object sender, RoutedEventArgs e)
        {
            //this.txtPaymentDate.Text = "";
        }

        private List<Models.Receivable_Balance> GetReceivable_Balances_List()
        {
            List<Models.Receivable_Balance> result_List = new List<Models.Receivable_Balance>();
            foreach (Models.Invoice tempInvoice in _Invoice_List)
            {
                result_List.Add(new Models.Receivable_Balance {
                    InvoiceID = tempInvoice.Invoice_ID,
                    Receivable_Record_ID = this.txtReceivableID.Text
                });
            }
            return result_List;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveForm();
            cleanForm();
        }

        private void loadReceivable_Record(string inReceivableID)
        {
            Controllers.ReceivableController receivableController = new Controllers.ReceivableController();
            Models.Receivable receivable = new Models.Receivable();
            receivable = receivableController.GetReceivableRecord(inReceivableID);
            setReceivable(receivable);
        }

        private void loadReceivable_Detail(string inReceivableID)
        {
            Controllers.ReceivableDetailController receivableDetailController = new Controllers.ReceivableDetailController();
            List<string> invoices = new List<string>();
            invoices = receivableDetailController.GetInvoicesIncluded(inReceivableID);
            load_Invoices_To_List(invoices);
        }

        private void load_Invoices_To_List(List<string> inInvoiceIDs)
        {
            Controllers.InvoiceController invoiceController = new Controllers.InvoiceController();
            _Invoice_List = new List<Models.Invoice>();
            foreach (string tempInvoiceID in inInvoiceIDs)
            {
                _Invoice_List.Add(invoiceController.get_the_Invoice(tempInvoiceID));
            }
            this.lstInvoice.ItemsSource = _Invoice_List;
            lstInvoice.Items.Refresh();
        }

        private void SaveForm()
        {
           if (!string.IsNullOrEmpty(this.txtPayment.Text))
           {
                if (string.IsNullOrEmpty(this.txtCharges.Text))
                {
                    this.txtCharges.Text = "0";
                }
           }

            // 1) Get Receivable Data from the Form
            Models.Receivable tempReceivable = new Models.Receivable();
            tempReceivable = getReceivable();

            // 2) Save Receivable Data
            Controllers.ReceivableController receivableController = new Controllers.ReceivableController();
            receivableController.SaveItem(tempReceivable);

            // 3) Get Receivable Detail Data from the List
            List<Models.Receivable_Balance> temp_Receivable_Balances_List = new List<Models.Receivable_Balance>();
            temp_Receivable_Balances_List = GetReceivable_Balances_List();

            // 4) Save Receivable Detail Data
            Controllers.ReceivableDetailController receivableDetailController = new Controllers.ReceivableDetailController();
            foreach (Models.Receivable_Balance temp_Receivable_Balance in temp_Receivable_Balances_List)
            {
                receivableDetailController.SaveItem(temp_Receivable_Balance);
            }
        }

        private Models.Receivable GetReceivable()
        {
            Models.Receivable theResult = new Models.Receivable
            {
                Receivable_Date = this.txtPaymentDate.Text,
                Receivable_Method = this.txtPaymentMethod.Text,
                Receivable_Amount = this.txtPayment.Text,
                Receivable_Customer_ID = this.txtClinicID.Text,
                Receivable_Remark = this.txtRemark.Text,
                Receivable_Bankin_Date = this.txtBankinDate.Text,
                Receivable_Record_ID = this.txtReceivableID.Text,
                Receivable_Charges = this.txtCharges.Text
            };

            return theResult;
        }

        private void addRecord(Models.Receivable inReceivable, Models.Receivable_Balance inReceivable_Balance)
        {
            //addReceivable(inReceivable);
            //addReceivableBalance(inReceivable_Balance);
        }
        
        private void addReceivable(Models.Receivable inReceivable)
        {
            Controllers.ReceivableController receivableController = new Controllers.ReceivableController();
            receivableController.SaveItem(inReceivable);
        }

        private void addReceivableBalance(Models.Receivable_Balance inReceivable_Balance)
        {
            Controllers.ReceivableDetailController receivableDetailController = new Controllers.ReceivableDetailController();
            receivableDetailController.SaveItem(inReceivable_Balance);
        }

        private void txtReceivableID_GotFocus(object sender, RoutedEventArgs e)
        {
            //GetNewReceivableID();
        }


        private void txtReceivableID_LostFocus(object sender, RoutedEventArgs e)
        {
            /*
            if (!string.IsNullOrEmpty(this.txtReceivableID.Text))
            {
                 if ((this.txtReceivableID.Text.Length == 10) && (string.IsNullOrEmpty(txtPaymentDate.Text)))
                {
                    //search result
                }
            }*/

            if (string.IsNullOrEmpty(this.txtReceivableID.Text))
            {
                if (!string.IsNullOrEmpty(this.txtPaymentDate.Text))
                {
                    setNewID();
                }
            }
            else
            {
                //Load perviouse record
                loadReceivable_Record(this.txtReceivableID.Text);
                loadReceivable_Detail(this.txtReceivableID.Text);
            }
        }

        private void setNewID()
        {
            if (!string.IsNullOrEmpty(txtPaymentDate.Text))
            {
                if (string.IsNullOrEmpty(this.txtReceivableID.Text))
                {
                    Controllers.Functional.Receivable_ID_Creator receivable_ID_Creator = new Controllers.Functional.Receivable_ID_Creator(this.txtPaymentDate.Text);
                    this.txtReceivableID.Text = receivable_ID_Creator.getNewID();
                }
            }
        }

        private void txtReceivableID_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            setNewID();
        }

        private void txtPaymentMethod_LostFocus(object sender, RoutedEventArgs e)
        {
            if((!string.IsNullOrEmpty(this.txtClinicID.Text)) && (!string.IsNullOrEmpty(this.txtReceivableID.Text)) && (!string.IsNullOrEmpty(this.txtPaymentDate.Text)))
            {
                if (!string.IsNullOrEmpty(this.txtPaymentMethod.Text))
                {
                    setChequeRemark();
                }
                else
                {
                    this.txtPaymentMethod.Text = "CHEQUE";
                    setChequeRemark();
                }
            }
        }

        private void setChequeRemark()
        {
            if (this.txtPaymentMethod.Text == "CHEQUE")
            {
                this.txtRemark.Text = "Bank of Cheque : \n" + "Cheque Number: \n" + "Cheque Date: ";
                this.txtCharges.Text = "0";
            }
        }

        private void txtPayment_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void txtPaymentMethod_GotFocus(object sender, RoutedEventArgs e)
        {
            this.txtPaymentMethod.Text = "";
            this.txtRemark.Text = "";
        }

        private void txtReceivableID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                setNewID();
            }
        }

        private void txtCharges_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtPayment.Text))
            {
                if (string.IsNullOrEmpty(this.txtCharges.Text))
                {
                    this.txtCharges.Text = "0";
                }
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                this.txtPaymentDate.Focusable = true;
                Keyboard.Focus(txtPaymentDate);
            }
        }
    }
}
