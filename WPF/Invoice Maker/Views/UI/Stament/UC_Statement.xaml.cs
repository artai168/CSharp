using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace TopStar_Invoice_Maker_SQLSever.Views.UI.Stament
{
    /// <summary>
    /// UC_Statement.xaml 的互動邏輯
    /// </summary>
    public partial class UC_Statement : UserControl
    {
        private List<Models.Invoice> invoice_List;
        private List<Models.Clinic> clinicList;
        private Models.Clinic currentClinic;

        public UC_Statement()
        {
            InitializeComponent();
            invoice_List = new List<Models.Invoice>();
            currentClinic = new Models.Clinic();
        }

        private string theYear()
        {
            System.Globalization.CultureInfo en = new System.Globalization.CultureInfo("en-US");
            var result = DateTime.Now.ToString("yyyy", en);
            return result.ToString();
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

        private void txtStatementID_LostFocus(object sender, RoutedEventArgs e)
        {
            string result = "";

            if (!string.IsNullOrWhiteSpace(this.txtStatementID.Text))
            {
                if ((this.txtStatementID.Text.Trim().Length > 0) && (this.txtStatementID.Text.Trim().Length <= 3))
                {
                    int temp;
                    if (int.TryParse(this.txtStatementID.Text.Trim(), out temp))
                    {
                        result = string.Format("{0:000}", temp);
                        this.txtStatementID.Text = "ST-S" + theYear() + result;
                    }
                }
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

        private void txtInvoiceDate_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txtInvoiceDate.Text = setDate(this.txtInvoiceDate.Text);
        }

        private void txtClinicID_LostFocus(object sender, RoutedEventArgs e)
        {
            var myLINQResult = from clinic_Detail in clinicList
                               where clinic_Detail.Clinic_ID == this.txtClinicID.Text
                               select clinic_Detail;

            foreach (Models.Clinic temp_clinic_Detail in myLINQResult)
            {
                currentClinic = temp_clinic_Detail;
            }

        }

        private void getInvoiceList()
        {
            if (!string.IsNullOrEmpty(this.txtClinicID.Text))
            {
                Controllers.InvoiceController invoiceController = new Controllers.InvoiceController();
                invoice_List = invoiceController.Get_Invoice_For_Statement(this.txtClinicID.Text);
            }
        }

        private void createStatement()
        {
            getInvoiceList();
            Models.Statement statement = new Models.Statement();
            statement.ID = this.txtStatementID.Text;
            statement.The_Clinic = currentClinic;
            statement.Statement_Date = this.txtInvoiceDate.Text;
            statement.Invoice_List = invoice_List;
            Controllers.Excel.StatementMaker.StatementMaker_EPPlus myStaementMaker = new Controllers.Excel.StatementMaker.StatementMaker_EPPlus(statement);
            myStaementMaker.MakeNewStatement();
            cleanForm();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            createStatement();
        }

        private void cleanForm()
        {
            MessageBox.Show("Statement: " + this.txtStatementID.Text +" had been created! ");
            this.txtClinicID.Text = "";
            this.txtInvoiceDate.Text = "";
            this.txtStatementID.Text = "";
            invoice_List = new List<Models.Invoice>();
            clinicList = new List<Models.Clinic>();
            currentClinic = new Models.Clinic();
        }

    }
}
