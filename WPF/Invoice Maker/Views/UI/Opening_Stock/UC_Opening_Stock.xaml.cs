using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TopStar_Invoice_Maker_SQLSever.Views.UI.Opening_Stock
{
    /// <summary>
    /// UC_Opening_Stock.xaml 的互動邏輯
    /// </summary>
    public partial class UC_Opening_Stock : UserControl
    {
        private List<TopStar_Invoice_Maker_SQLSever.Opening_Stock.Opening_Stock_Temp> opening_Stock_Temp_List;
        private TopStar_Invoice_Maker_SQLSever.Opening_Stock.opening_Stock_Processor opening_Stock_Processor;

        public UC_Opening_Stock()
        {
            InitializeComponent();
            opening_Stock_Processor = new TopStar_Invoice_Maker_SQLSever.Opening_Stock.opening_Stock_Processor();
        }

        private void btn_LoadFromExcel_Click(object sender, RoutedEventArgs e)
        {
            opening_Stock_Processor.loadListFromExcel();
            opening_Stock_Temp_List = opening_Stock_Processor.opening_Stock_Temp_List;
            this.lst_Raw_List.ItemsSource = opening_Stock_Temp_List;
        }

        private void btn_LoadFromDB_Click(object sender, RoutedEventArgs e)
        {
            opening_Stock_Processor.loadListFormDB();
            opening_Stock_Temp_List = opening_Stock_Processor.opening_Stock_Temp_List;
            this.lst_Raw_List.ItemsSource = opening_Stock_Temp_List;
        }

        private void lst_Raw_List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            get_Item_From_List();
        }

        private void get_Item_From_List()
        {
            TopStar_Invoice_Maker_SQLSever.Opening_Stock.Opening_Stock_Temp item = this.lst_Raw_List.SelectedItem as TopStar_Invoice_Maker_SQLSever.Opening_Stock.Opening_Stock_Temp;
            if (item != null)
            {
                Opening_Stock.UC_Opening_Stock_Editor uC_Opening_Stock_Editor = new Opening_Stock.UC_Opening_Stock_Editor();
                this.grid_Result.Children.Clear();
                this.grid_Result.Children.Add(uC_Opening_Stock_Editor);
                uC_Opening_Stock_Editor.SetUI(item);

                var index = lst_Raw_List.SelectedIndex;
                opening_Stock_Temp_List.RemoveAt(index);
                lst_Raw_List.Items.Refresh();

            }
        }
    }
}
