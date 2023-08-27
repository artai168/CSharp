using System.Windows;
using System.Windows.Controls;

namespace TopStar_Invoice_Maker_SQLSever.Views.UI
{
    /// <summary>
    /// UC_Left_Right_V1.xaml 的互動邏輯
    /// </summary>
    public partial class UC_Left_Right_V1 : UserControl
    {
        public UC_Left_Right_V1()
        {
            InitializeComponent();
        }

        private void btn_Add_Invoice_Click(object sender, RoutedEventArgs e)
        {
            Invoice.UC_Invoice_Maker uC_Invoice_Maker = new Invoice.UC_Invoice_Maker();
            mainPanel.Children.Clear();
            mainPanel.Children.Add(uC_Invoice_Maker);
        }

        private void btn_Add_Statemnt_Click(object sender, RoutedEventArgs e)
        {
            UI.Stament.UC_Statement uC_Statement = new Stament.UC_Statement();
            mainPanel.Children.Clear();
            mainPanel.Children.Add(uC_Statement);
        }

        private void btn_Add_Receivable_Click(object sender, RoutedEventArgs e)
        {
            UI.Receivable.UC_Receiable uC_Receiable = new Receivable.UC_Receiable();
            mainPanel.Children.Clear();
            mainPanel.Children.Add(uC_Receiable);
        }

        private void btn_Add_Supplier_Click(object sender, RoutedEventArgs e)
        {
            UI.Supplier.UC_Supplier uC_Supplier = new Supplier.UC_Supplier();
            mainPanel.Children.Clear();
            mainPanel.Children.Add(uC_Supplier);
        }

        private void btn_Add_Inventory_Click(object sender, RoutedEventArgs e)
        {
            UI.Inventory.UC_Inventory_Maker uC_Inventory = new Inventory.UC_Inventory_Maker();
            mainPanel.Children.Clear();
            mainPanel.Children.Add(uC_Inventory);
        }

        private void btn_Opening_Stock_Click(object sender, RoutedEventArgs e)
        {
            UI.Opening_Stock.UC_Opening_Stock uC_Opening_Stock = new Opening_Stock.UC_Opening_Stock();
            mainPanel.Children.Clear();
            mainPanel.Children.Add(uC_Opening_Stock);
        }
    }
}
