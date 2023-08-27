using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            this._Invoice_List = new List<Models.Invoice>(); ;
            this.invoiceList_inTextBox = new List<Models.Invoice>();
            this.lstInvoice.ItemsSource = null;
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
        }

        private void txtClinicID_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txtInvoiceID.Text = "";
            if (!string.IsNullOrEmpty(this.txtClinicID.Text.Trim()))
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
            this.txtBankinDate.Text = setDate(this.txtBankinDate.Text);
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
                receivable_Date = this.txtPaymentDate.Text,
                receivable_Method = this.txtPaymentMethod.Text,
                receivable_Amount = this.txtPayment.Text,
                receivable_Customer_ID = this.txtClinicID.Text,
                receivable_Remark = this.txtRemark.Text,
                receivable_Charges = this.txtRemark.Text
            };
        }

        private void setReceivable(Models.Receivable inReceivable)
        {
            this.txtPaymentDate.Text = inReceivable.receivable_Date;
            this.txtPaymentDate.Text = inReceivable.receivable_Method;
            this.txtPayment.Text = inReceivable.receivable_Amount;
            this.txtClinicID.Text = inReceivable.receivable_Customer_ID;
            this.txtRemark.Text = inReceivable.receivable_Remark;
            this.txtRemark.Text = inReceivable.receivable_Remark;
        }

        private void addToList()
        {
            if (!string.IsNullOrEmpty(this.txtInvoiceID.Text.Trim()))
            {
                foreach (Models.Invoice invoiceItem in invoiceList_inTextBox)
                {
                    if (invoiceItem.Invoice_ID == this.txtInvoiceID.Text)
                    {
                        _Invoice_List.Add(invoiceItem);
                        lstInvoice.Items.Refresh();
                        updateBalance();
                        invoiceList_inTextBox.Remove(invoiceItem);
                    }
                }
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private Models.Receivable GetReceivable()
        {
            Models.Receivable theResult = new Models.Receivable
            {
                receivable_Date = this.txtPaymentDate.Text,
                receivable_Method = this.txtPaymentMethod.Text,
                receivable_Amount = this.txtPayment.Text,
                receivable_Customer_ID = this.txtClinicID.Text,
                receivable_Record_ID = this.txtReceivableID.Text,
                receivable_Remark = this.txtRemark.Text,
                receivable_Charges = this.txtCharges.Text
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

        
    }
}
