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
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Mobisy.AppCodes;

namespace Mobisy.AppPages
{
    /// <summary>
    /// Interaction logic for Accessories.xaml
    /// </summary>
    public partial class Accessories : Page
    {
        
        MyConnection mycon;
        MySqlConnection con;
        ID id;
        MySqlDataAdapter adapter;
        DataSet dataset;
        MySqlCommandBuilder msqlcmbl;


        public Accessories()
        {

            InitializeComponent();
            mycon = new MyConnection();
            

            id = new ID();
  
            LoadAccessories();
        }

        private void LoadAccessories()
        {

        
           // mycon = new MyConnection();
            con = mycon.GetConnection();

            string queryString = "Select accessories_id , accessories_name, cost_price, selling_price from accessories";

            adapter = new MySqlDataAdapter(queryString, con);
            dataset = new DataSet();
              
            try
            {
                con.Open();
                adapter.Fill(dataset, "accessories");
                dgrid_acc.ItemsSource = dataset.Tables[0].DefaultView;


            }
            catch (MySqlException erro)
            {
                MessageBox.Show("Erro" + erro);
                con.Close();
            }

            //dgrid_acc.Columns[0].Visibility = Visibility.Hidden;

            con.Close();
        }

        private void add_view_acc_Click(object sender, RoutedEventArgs e)
        {
            LoadAccessories();
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                msqlcmbl = new MySqlCommandBuilder(adapter);
                adapter.Update(dataset, "accessories");
                //MessageBox.Show("UU", "UU", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadAccessories();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" +  ex.Message);
            }
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            LoadAccessories();
        }
    }
}