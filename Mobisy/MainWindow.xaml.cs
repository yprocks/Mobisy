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
using Mobisy.AppPages;
using MySql.Data.MySqlClient;

namespace Mobisy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       

        public MainWindow()
        {
            InitializeComponent();
            main_content.Content = new Sell();
           
        }

        private void sell_click(object sender, RoutedEventArgs e)
        {
            page_title.Content = "Sell";
            main_content.Content = null;
            main_content.Content = new Sell(); ;
        }

        private void sales_click(object sender, RoutedEventArgs e)
        {
            page_title.Content = "Sales";
            main_content.Content = null;
        }

        private void repairs_click(object sender, RoutedEventArgs e)
        {
            page_title.Content = "Repairs";
            main_content.Content = null;
            main_content.Content = new Repairs(); ;
        }

        private void dealers_click(object sender, RoutedEventArgs e)
        {
            page_title.Content = "Dealers";
            main_content.Content = null;
        }

        private void repairsmen_click(object sender, RoutedEventArgs e)
        {
            page_title.Content = "Repairsmen"; 
            main_content.Content = null;
        }

        private void stock_click(object sender, RoutedEventArgs e)
        {
            page_title.Content = "Stock";
            main_content.Content = null;
            main_content.Content = new Stock();
        }

        private void orderlist_click(object sender, RoutedEventArgs e)
        {
            page_title.Content = "Order List";        
            main_content.Content = null;

        }

        private void company_click(object sender, RoutedEventArgs e)
        {
            page_title.Content = "Company";
            main_content.Content = null;
        }

        private void notes_click(object sender, RoutedEventArgs e)
        {
            page_title.Content = "Notes";
            main_content.Content = null;
        }

        private void search_click(object sender, RoutedEventArgs e)
        {
            page_title.Content = "Search";
            main_content.Content = null;
        }

        private void logout_click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void btn_mobiles_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
