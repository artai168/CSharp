using System;
using System.Windows;

namespace TopStar_Invoice_Maker_SQLSever
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }
        private string tempWorkingEXCEL;

        private void PAYMENT_RECEIVED_Click(object sender, RoutedEventArgs e)
        {
            W_ReceivedPayment w_ReceivedPayment = new W_ReceivedPayment();
            w_ReceivedPayment.Show();
        }

        private void Men_Statement_Click(object sender, RoutedEventArgs e)
        {
            W_Statement w_Statement = new W_Statement();
            w_Statement.Show();
        }

        private void Men_About_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("The machine name is " + Environment.MachineName);
        }

        private void closeMe()
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if (invalidUsing())
            //{
                //closeMe();
            //}
            
        }

        private bool invalidUsing()
        {
            bool result = false;
            DateTime valid_date = new DateTime(2019, 05, 31);
            DateTime today_date = DateTime.Today;
            if(today_date >= valid_date)
            {
                result = true;
            }
            return result;
        }

        
    }
}
