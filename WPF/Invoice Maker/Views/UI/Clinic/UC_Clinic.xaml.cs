using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TopStar_Invoice_Maker_SQLSever.Views.UI.Clinic
{
    /// <summary>
    /// UC_Clinic.xaml 的互動邏輯
    /// </summary>
    public partial class UC_Clinic : UserControl
    {
        private List<string> delivery_List;
        private List<string> payment_Terms_List;
        private List<string> district_List;
        private List<Models.Clinic> clinicList;
        private List<string> sales_List;


        public UC_Clinic()
        {
            InitializeComponent();
            load_PaymentTerms_List();
            this.txtPayment.ItemsSource = payment_Terms_List;
            load_Delivery_List();
            this.txtDelivery.ItemsSource = delivery_List;
            delivery_List = new List<string>();
            load_District_List();
            load_Sales_List();
            this.txtSalesPerson.ItemsSource = sales_List;
        }

        public void loadClinic()
        {
            InitializeComponent();
            load_PaymentTerms_List();
            this.txtPayment.ItemsSource = payment_Terms_List;
            load_Delivery_List();
            this.txtDelivery.ItemsSource = delivery_List;
            delivery_List = new List<string>();
            load_District_List();
        }

        private void load_District_List()
        {
            district_List = new List<string>();
            //Controllers.Excel.DistrictController districtController = new Controllers.Excel.DistrictController();

            Controllers.DistrictController districtController = new Controllers.DistrictController();
            districtController.load_String_List();
            this.txtDistrict.ItemsSource = districtController.district_String_List;
        }

        private void load_Sales_List()
        {
            sales_List = new List<string>();
            this.sales_List.Add("Dennis Wong");
            this.sales_List.Add("Kevin Cheng");
        }

        private void load_Delivery_List()
        {
            delivery_List = new List<string>();
            this.delivery_List.Add("BY HAND");
            this.delivery_List.Add("COURIER");
        }

        private void load_PaymentTerms_List()
        {
            payment_Terms_List = new List<string>();
            this.payment_Terms_List.Add("C.O.D");
            this.payment_Terms_List.Add("MONTHLY");
        }

        private void ShipTo_Text_Empty()
        {
            this.txt_ShipTo_Add.Text = "";
            this.txt_ShipTo_Clinic.Text = "";
            this.txt_ShipTo_Contact.Text = "";
            this.txt_ShipTo_Tel.Text = "";
        }

        private void btnCreateShipTo_Click(object sender, RoutedEventArgs e)
        {
            //ShipTo_Editable_Set_True();
            ShipTo_Text_Empty();
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

        private void txtDistrict_GotFocus(object sender, RoutedEventArgs e)
        {

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
                this.txtDiscount.Text = temp_clinic_Detail.Discount;
            }
        }

        private void cleanForm()
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
            this.txtSalesPerson.Text = "";
            this.txtDiscount.Text = "";
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cleanForm();
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            this.txt_ShipTo_Add.Text = this.txt_Clinic_Add.Text;
            this.txt_ShipTo_Clinic.Text = this.txt_Clinic.Text;
            this.txt_ShipTo_Contact.Text = this.txt_Clinic_Contact.Text;
            this.txt_ShipTo_Tel.Text = this.txt_Clinic_Tel.Text;
        }

        /*
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            saveData();
        }*/

        private Models.Clinic GetClinic()
        {
            return new Models.Clinic
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
                SalesPerson = this.txtSalesPerson.Text,
                Discount = this.txtDiscount.Text,
                Clinic_ID = this.txtClinicID.Text
            };
        }

        public void saveData()
        {
            if ((!String.IsNullOrEmpty(this.txtClinicID.Text)) && (!String.IsNullOrEmpty(this.txt_Clinic.Text)) && (!String.IsNullOrEmpty(this.txt_Clinic_Add.Text)))
            {
                Controllers.ClinicController districtController = new Controllers.ClinicController();
                districtController.SaveItem(GetClinic());
                MessageBox.Show("Record Saved!");
                cleanForm();
            }
            else
            {
                MessageBox.Show("Please finish the Form!");
            }
            cleanForm();
        }

        private void txtDiscount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtDiscount.Text))
            {
                this.txtDiscount.Text = "100";
            }
        }
    }
}
