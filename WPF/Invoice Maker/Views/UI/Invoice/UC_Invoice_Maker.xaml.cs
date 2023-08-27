using System.Windows;
using System.Windows.Controls;

namespace TopStar_Invoice_Maker_SQLSever.Views.UI.Invoice
{
    /// <summary>
    /// UC_Invoice_Maker.xaml 的互動邏輯
    /// </summary>
    public partial class UC_Invoice_Maker : UserControl
    {
        public UC_Invoice_Maker()
        {
            InitializeComponent();
        }

        private void setClinic_Visible()
        {
            this.uc_Clinic.Visibility = Visibility.Visible;
            this.btnSaveClinic.Visibility = Visibility.Visible;
        }

        private void setClinic_Hide()
        {
            this.uc_Clinic.Visibility = Visibility.Hidden;
            this.btnSaveClinic.Visibility = Visibility.Hidden;
        }

        private void btnNewClinic_Click(object sender, RoutedEventArgs e)
        {
            if (this.uc_Clinic.Visibility == Visibility.Hidden)
            {
                this.uc_Clinic.loadClinic();
                setClinic_Visible();
            }
            else
            {
                setClinic_Hide();
            }

        }

        private void btnSaveClinic_Click(object sender, RoutedEventArgs e)
        {
            this.uc_Clinic.saveData();
            setClinic_Hide();
        }

        private void setProduct_Visible()
        {
            if (this.uc_Product.Visibility == Visibility.Hidden)
            {
                this.uc_Product.Visibility = Visibility.Visible;
                this.btnSaveProduct.Visibility = Visibility.Visible;
            }
            else
            {
                setProduct_Hide();
            }

        }

        private void setProduct_Hide()
        {
            this.uc_Product.Visibility = Visibility.Hidden;
            this.btnSaveProduct.Visibility = Visibility.Hidden;
        }

        private void btnNewProduct_Click(object sender, RoutedEventArgs e)
        {
            this.uc_Product.load_Product();
            setProduct_Visible();
        }

        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            this.uc_Product.SaveData();
            setProduct_Hide();
        }


    }
}
