using System.Windows;
using System.Windows.Controls;

namespace TopStar_Invoice_Maker_SQLSever.Views.UI.Inventory
{
    /// <summary>
    /// UC_Inventory_Maker.xaml 的互動邏輯
    /// </summary>
    public partial class UC_Inventory_Maker : UserControl
    {
        public UC_Inventory_Maker()
        {
            InitializeComponent();
        }

        private void btnNewProduct_Click(object sender, RoutedEventArgs e)
        {
            if (this.GD_productManager.Visibility == Visibility.Visible)
            {
                this.GD_productManager.Visibility = Visibility.Hidden;
            }
            else
            {
                this.GD_productManager.Visibility = Visibility.Visible;
            }
        }

        private void btnSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            this.uc_Product.SaveData();
            this.GD_productManager.Visibility = Visibility.Hidden;
        }
    }
}
