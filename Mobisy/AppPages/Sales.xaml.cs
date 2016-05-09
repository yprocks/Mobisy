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
    /// Interaction logic for Stock.xaml
    /// </summary>
    public partial class Sales : Page
    {
        MyConnection mycon1, mycon2, mycon3;
        MySqlConnection con1, con2, con3;
        ID id;
        MySqlDataAdapter adapter, r_adapter;
        DataSet dataset, r_dataset;
        Mobile mobile;
        Repair rp;


        public Sales()
        {
            InitializeComponent();
            mycon1 = new MyConnection();
            mycon2 = new MyConnection();
            mycon3 = new MyConnection();

            id = new ID();

  
            mobile = new Mobile();
            rp = new Repair();
            tb_mob_search.Text = "";

 
      
            LoadMobiles(0, tb_mob_search.Text);

        }

        private String GetDate()
        {
            return System.DateTime.Now.Date.ToString("yyyy-MM-dd");
        }

        private void view_mob_click(object sender, RoutedEventArgs e)
        {
            view_mobile_sales.Background = Brushes.LightGray;
            view_repair_sales.Background = Brushes.Transparent;

            mobile_view_panel.Visibility = System.Windows.Visibility.Visible;
            repair_view_panel.Visibility = System.Windows.Visibility.Hidden;

        }

        private void view_repair_click(object sender, RoutedEventArgs e)
        {
            view_mobile_sales.Background = Brushes.LightGray;
            view_repair_sales.Background = Brushes.Transparent;

            repair_view_panel.Visibility = System.Windows.Visibility.Visible;
            mobile_view_panel.Visibility = System.Windows.Visibility.Hidden;

         
        }

        /**************************View***Repairs******************************/

        private void LoadRepairAdapter(string queryString, MySqlConnection con)
        {
            r_adapter = new MySqlDataAdapter(queryString, con);
            r_dataset = new DataSet();
        }

        private void LoadRepairs(int value)
        {
            mycon2 = new MyConnection();
            con2 = mycon2.GetConnection();
            // MySqlCommand cmd = dbConn.CreateCommand();
            // cmd.CommandText = "SELECT * from demobase";
            if (value == 0)
                LoadRepairAdapter(TodaysRepairsQuery(), con2);
            else if(value == 1)
                LoadRepairAdapter(AllRepairsQuery(), con2);
            else 
                LoadRepairAdapter(SearchRepairsQuery(), con2);


            try
            {
                con2.Open();
                r_adapter.Fill(r_dataset, "repairs");

                dgrid_repairs.ItemsSource = r_dataset.Tables[0].DefaultView;
                cb_rprman.ItemsSource = GetReparismanList();

            }
            catch (MySqlException erro)
            {
                MessageBox.Show("Erro" + erro);
                con2.Close();
            }

            con2.Close();
        }

        private List<String> GetReparismanList()
        {
            List<String> rp_list = new List<String>();

            mycon3 = new MyConnection();
            con3 = mycon3.GetConnection();
            try
            {
                con3.Open();

                //MessageBox.Show("Connection Open ! ");

                MySqlCommand cmd = con3.CreateCommand();
                cmd.CommandText = "SELECT repairsman_name from repairsman";

                MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    //companies.Add(reader.GetString(0));
                    rp_list.Add(reader.GetString(0));
                }
                //cb_company.ItemsSource = companies;

                con3.Close();
            }
            catch (Exception e)
            {

            }

            return rp_list;
        }

        private void dgrid_repairs_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) e.Handled = true;
        }

        private void btn_update_rep_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView drv = (DataRowView)dgrid_repairs.SelectedItem;
                String[] repair_fields = new String[13];

                for (int i = 0; i < 13; i++)
                {
                    repair_fields[i] = (drv.Row[i]).ToString().Trim();
                }

                int rpman_id = id.GetRepairsmanID(repair_fields[4]);

                rp.UpdateRepair(
                    Convert.ToInt32(repair_fields[0]),
                    rpman_id,
                    repair_fields[1],
                    repair_fields[2],
                    repair_fields[3],
                    repair_fields[5],
                    Convert.ToInt32(repair_fields[6]),
                    Convert.ToInt32(repair_fields[7]),
                    repair_fields[8],
                    repair_fields[9],
                    Convert.ToInt32(repair_fields[10]),
                    Convert.ToInt32(repair_fields[11]),
                    Convert.ToInt32(repair_fields[12])
                );

                LoadRepairs(cb_dateSelector.SelectedIndex);

                //  MessageBox.Show(rpman_id + " " + repair_fields[4]);
                //  msqlcmbl = new MySqlCommandBuilder(adapter);
                //  adapter.Update(dataset, "repairsToday");    

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);

            }
        }

        private String TodaysRepairsQuery()
        {
            return "SELECT repairs_id, mobile_name, cust_name, cust_phone, repairsman_name, mobile_problem, repairsman_price, cust_price, " +
                   "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, DATE_FORMAT(fixing_date,'%y-%m-%d') as fixing_date, isRepaired, isPaid, isRemoved " +
                   "from repairs JOIN repairsman on repairsman.repairsman_id = repairs.repairsman_id where DATE(date_added) = '" + GetDate() + "' and isPaid = 1 and isRemoved = 0";
        }

        private String AllRepairsQuery()
        {
            return "SELECT repairs_id, mobile_name, cust_name, cust_phone, repairsman_name, mobile_problem, repairsman_price, cust_price, " +
                   "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, DATE_FORMAT(fixing_date,'%y-%m-%d') as fixing_date, isRepaired, isPaid, isRemoved " +
                   "from repairs JOIN repairsman on repairsman.repairsman_id = repairs.repairsman_id where isPaid = 1 and isRemoved = 0";
        }

        private void btn_rep_reset_Click(object sender, RoutedEventArgs e)
        {
            LoadRepairs(0);
            cb_dateSelector.SelectedIndex = 0;
            tb_rep_search.Text = "";
        }

        // Sorting date and fate
        private void cb_dateSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //  LoadData();
            // LoadRepairs();
            if (cb_dateSelector.SelectedIndex == 1)
            {
                LoadRepairs(1);
            }
         
            else if (cb_dateSelector.SelectedIndex == 0)
            {
                LoadRepairs(0);
            }
        }

        private void tb_rep_search_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!String.IsNullOrEmpty(tb_rep_search.Text))
                {
                    cb_dateSelector.SelectedIndex = 1;
                    LoadRepairs(2);
                }
            }
        }

        private String SearchRepairsQuery()
        {
            return "SELECT repairs_id, mobile_name, cust_name, cust_phone, repairsman_name, mobile_problem, repairsman_price, cust_price, " +
                   "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, DATE_FORMAT(fixing_date,'%y-%m-%d') as fixing_date, isRepaired, isPaid, isRemoved " +
                   "from repairs JOIN repairsman on repairsman.repairsman_id = repairs.repairsman_id " +
                   "where (mobile_name LIKE '%" + tb_rep_search.Text + "%' OR cust_name LIKE '%" + tb_rep_search.Text + "%' OR cust_phone = '" + tb_rep_search.Text + "' " +
                   "OR repairsman_name Like '%" + tb_rep_search.Text + "%' OR fixing_date LIKE '%" + tb_rep_search.Text + "%' OR date_added LIKE '%" + tb_rep_search.Text + "%')" + 
                   " AND isPaid = 1";
        }


        /**************************View***Mobiles******************************/

        private String MobileSoldQuery(string search, int d_id)
        {

            if (d_id == 0)
                return "SELECT mobile.mobile_id as mobile_id, family.family_name, dealer.dealer_name, imei, customer.cust_name as cust_name, customer.cust_phone as cust_phone, selling_price, " +
                  "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, isExchanged, exchanged_imei " +
                  "from mobile join family on family.family_id = mobile.family_id join dealer ON dealer.dealer_id = mobile.dealer_id join customer on customer.mobile_id = mobile.mobile_id " +
                  "where isSold = 1 AND ( imei LIKE '%" + search + "%' OR family_name LIKE '%" + search + "%' ) AND date_added = '" + GetDate() +"'";
            else
                return "SELECT mobile.mobile_id as mobile_id, family.family_name, dealer.dealer_name, imei, customer.cust_name as cust_name, customer.cust_phone as cust_phone, selling_price, " +
                   "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, isExchanged, exchanged_imei " +
                   "from mobile join family on family.family_id = mobile.family_id join dealer ON dealer.dealer_id = mobile.dealer_id join customer on customer.mobile_id = mobile.mobile_id " +
                   "where isSold = 1 AND ( imei LIKE '%" + search + "%' OR family_name LIKE '%" + search + "%' OR date_added LIKE '%" + search + "%' )";
        }

        private void LoadAdapter(string queryString, MySqlConnection con)
        {
            adapter = new MySqlDataAdapter(queryString, con);
            dataset = new DataSet();   
        }

        private void LoadMobiles(int d_value, string search)
        {
            mycon2 = new MyConnection();
            con2 = mycon2.GetConnection();
            
            //System.Diagnostics.Debug.WriteLine(" " + d_value + " " + s_value + " " + search );

            LoadAdapter(MobileSoldQuery(search, d_value), con2);
          
            try
            {
                con2.Open();
                 adapter.Fill(dataset, "mobiles");
                
                dgrid_mobiles.ItemsSource = dataset.Tables[0].DefaultView;
                cb_dealerName.ItemsSource = GetDealersList();
                cb_mobileName.ItemsSource = GetFamilyName();

            }
            catch (MySqlException erro)
            {
                MessageBox.Show("Erro" + erro);
                con2.Close();
            }

            con2.Close();
        }

        private List<String> GetDealersList()
        {
            List<String> dl_list = new List<String>();

            mycon3 = new MyConnection();
            con3 = mycon3.GetConnection();
            try
            {
                con3.Open();

                //MessageBox.Show("Connection Open ! ");

                MySqlCommand cmd = con3.CreateCommand();
                cmd.CommandText = "SELECT dealer_name from dealer";

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                        //companies.Add(reader.GetString(0));
                         dl_list.Add(reader.GetString(0));
                }
                    //cb_company.ItemsSource = companies;

                con3.Close();
            }
            catch (Exception e)
            {

            }

            return dl_list;
        }

        private List<String> GetFamilyName()
        {
            List<String> fname_list = new List<String>();

            mycon3 = new MyConnection();
            con3 = mycon3.GetConnection();
            try
            {
                con3.Open();

                //MessageBox.Show("Connection Open ! ");

                MySqlCommand cmd = con3.CreateCommand();
                cmd.CommandText = "SELECT family_name from family";

                MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    //companies.Add(reader.GetString(0));
                    fname_list.Add(reader.GetString(0));
                }
                //cb_company.ItemsSource = companies;

                con3.Close();
            }
            catch (Exception e)
            {

            }

            return fname_list;
        }

        private void btn_update_mob_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView drv = (DataRowView)dgrid_mobiles.SelectedItem;
                String[] mobile_fields = new String[10];

                for (int i = 0; i < 10; i++)
                {
                    mobile_fields[i] = (drv.Row[i]).ToString();
                }

               // System.Diagnostics.Debug.WriteLine("" + mobile_fields[0] + " " + mobile_fields[2] + " " + mobile_fields[10]);

                mobile.UpdateSales(
                    Convert.ToInt32(mobile_fields[0]),
                    mobile_fields[4],
                    mobile_fields[5],
                    Convert.ToInt32(mobile_fields[6])
                );
 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ ex.Message);

            }
        }

        private void btn_mob_reset_Click(object sender, RoutedEventArgs e)
        {
            LoadMobiles(0, "");
            cb_mdateSort.SelectedIndex = 0;
            tb_mob_search.Text = "";
        }

        private void tb_mob_search_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoadMobiles(cb_mdateSort.SelectedIndex, tb_mob_search.Text);
            }
        }

        private void cb_mdateSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadMobiles(cb_mdateSort.SelectedIndex, tb_mob_search.Text);
        }

        private void btn_print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(dgrid_mobiles, "My First Print Job");
            }
        }

        private void btn_savePDF_Click(object sender, RoutedEventArgs e)
        {
            
        }

    }
}