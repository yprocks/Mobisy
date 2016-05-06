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
    public partial class Mobiles : Page
    {
        MyConnection mycon1, mycon2, mycon3;
        MySqlConnection con1, con2, con3;
        ID id;
        MySqlDataAdapter adapter;
        DataSet dataset;
        Mobile mobile;

        string company;

        bool isMobileValid;
        bool compSelected, nameSelected, dealerSelected, validImei, validCP, validSP;


        public Mobiles()
        {
            InitializeComponent();
            mycon1 = new MyConnection();
            mycon2 = new MyConnection();
            mycon3 = new MyConnection();

            id = new ID();
  
            company = "";
            isMobileValid  = false;
            compSelected = nameSelected = dealerSelected = validCP = validImei = validSP = false;

            PopulateCompanies();
  
            mobile = new Mobile();

            tb_search.Text = "";

            PopulateDealerSort();
      
            LoadMobiles(0, 0, tb_search.Text);

        }

        private String GetDate()
        {
            return System.DateTime.Now.Date.ToString("yyyy-MM-dd");
        }

        private void view_mob_click(object sender, RoutedEventArgs e)
        {
            view_mobile_tab.Background = Brushes.LightGray;
            add_mobile_tab.Background = Brushes.Transparent;

            view_panel.Visibility = System.Windows.Visibility.Visible;
            add_mob_panel.Visibility = System.Windows.Visibility.Hidden;

        }

        private void add_mob_click(object sender, RoutedEventArgs e)
        {
            add_mobile_tab.Background = Brushes.LightGray;
            view_mobile_tab.Background = Brushes.Transparent;

            add_mob_panel.Visibility = System.Windows.Visibility.Visible;
            view_panel.Visibility = System.Windows.Visibility.Hidden;

            MobilePanel();
            Reset();
        }

        // popup data in mobile panel

        private void MobilePanel()
        {
            PopulateCompanies();
        }

        // Populates Companies
        private void PopulateCompanies()
        {

            con1 = mycon1.GetConnection();
            try
            {
                con1.Open();

                //MessageBox.Show("Connection Open ! ");
                MySqlCommand cmd = con1.CreateCommand();
                cmd.CommandText = "SELECT company_name from company";

                MySqlDataReader reader = cmd.ExecuteReader();

                cb_company.Items.Clear();

                if (reader.HasRows)
                {
                    cb_company.Items.Add("Select");

                    while (reader.Read())
                    {
                        //companies.Add(reader.GetString(0));
                        cb_company.Items.Add(reader[0]);
                    }
                    //cb_company.ItemsSource = companies;

                    //  cb_company.SelectedValue = cb_company.Items[0];
                }
                else
                {
                    cb_company.Items.Add("No Company found");
                }

                cb_company.SelectedIndex = 0;

                con1.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message);
            }

        }

        // company select event handler
        private void cb_company_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_company.SelectedIndex != 0)
            {
                company = (string)cb_company.SelectedValue;
                PopulateMobiles(company);
                compSelected = true;
                // MessageBox.Show("In");
            }
            else
            {
                cb_family.Items.Clear();
                cb_dealer.Items.Clear();
                compSelected = false;
            }
        }

        //populate mobiles
        private void PopulateMobiles(String c_name)
        {

            int cid = id.GetCompanyID(c_name);

            con2 = mycon2.GetConnection();
            try
            {
                con2.Open();

                //MessageBox.Show("Connection Open ! ");
                MySqlCommand cmd = con2.CreateCommand();
                cmd.CommandText = "SELECT family_name from family where company_id = " + cid;

                MySqlDataReader reader = cmd.ExecuteReader();

                cb_family.Items.Clear();

                if (reader.HasRows)
                {
                    cb_family.Items.Add("Select");

                    while (reader.Read())
                    {
                        //companies.Add(reader.GetString(0));
                        cb_family.Items.Add(reader[0]);

                    }
                    //  cb_family.SelectedValue = cb_family.Items[0];
                    //cb_company.ItemsSource = companies;
                }
                else
                {
                    cb_family.Items.Add("No mobiles found");
                }

                cb_family.SelectedIndex = 0;

                con2.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message);
            }
        }

        // On Mobile name selected
        private void cb_family_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_family.SelectedIndex != 0)
            {
                PopulateDealers();
                nameSelected = true;
            }
            else
            {
                cb_dealer.Items.Clear();
                nameSelected = false;
            }
        }

        // Populate all dealers
        private void PopulateDealers()
        {
            con3 = mycon3.GetConnection();
            try
            {
                con3.Open();

                //MessageBox.Show("Connection Open ! ");
                MySqlCommand cmd = con3.CreateCommand();
                cmd.CommandText = "SELECT dealer_name FROM dealer";

                MySqlDataReader reader = cmd.ExecuteReader();

                cb_dealer.Items.Clear();

                if (reader.HasRows)
                {
                    cb_dealer.Items.Add("Select");
                   
                    while (reader.Read())
                    {
                        //companies.Add(reader.GetString(0));
                        cb_dealer.Items.Add(reader[0]);

                    }
                    //    cb_dealer.SelectedValue = cb_family.Items[0];
                    //cb_company.ItemsSource = companies;
                }
                else
                {
                    cb_dealer.Items.Add("No dealer found");
                }

                cb_dealer.SelectedIndex = 0;
                con3.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message);
            }
        }

        private void tb_imei_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            char c = Convert.ToChar(e.Text);
            if (Char.IsNumber(c) && tb_imei.Text.Length < 16)
                e.Handled = false;
            else
                e.Handled = true;

            base.OnPreviewTextInput(e);

        }

        private void tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private void tb_costprice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char c = Convert.ToChar(e.Text);
            if (Char.IsNumber(c) && tb_costprice.Text.Length < 5)
                e.Handled = false;
            else
                e.Handled = true;

            base.OnPreviewTextInput(e);



        }

        private void tb_sellprice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char c = Convert.ToChar(e.Text);
            if (Char.IsNumber(c) && tb_sellprice.Text.Length < 5)
                e.Handled = false;
            else
                e.Handled = true;

            base.OnPreviewTextInput(e);


        }

        private void cb_dealer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_dealer.SelectedIndex != 0)
            {
                dealerSelected = true;
            }
            else
            {
                dealerSelected = false;
            }
        }

        private void tb_imei_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tb_imei.Text.Length >= 14)
            {
                validImei = true;
            }
            else
            {
                validImei = false;
            }
        }

        private void tb_costprice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_costprice.Text))
            {
                validCP = false;
            }
            else
            {
                validCP = true;
            }
        }

        private void tb_sellprice_TextChanged(object sender, TextChangedEventArgs e)
        {


            if (String.IsNullOrEmpty(tb_sellprice.Text))
            {
                validSP = false;
            }
            else
            {
                validSP = true;
            }

        }

        // Check if all inputs are valid to enter mobile details 
        private bool IsMobileValid()
        {
            if (compSelected && nameSelected && dealerSelected && validCP && validImei && validSP)
            {
                isMobileValid = true;
            }
            else
            {
                isMobileValid = false;
            }

            return isMobileValid;
        }

        private void btn_mob_add_Click(object sender, RoutedEventArgs e)
        {
            if (IsMobileValid())
            {

                if (mobile.AddMobile(
                    cb_family.SelectedValue.ToString(),
                    cb_dealer.SelectedValue.ToString(),
                    tb_imei.Text.ToString(),
                    Convert.ToInt16(tb_costprice.Text.ToString()),
                    Convert.ToInt16(tb_sellprice.Text.ToString()),
                    tb_missingparts.Text.ToString()
                    ))
                    MessageBox.Show("Mobile Added");
                else
                    MessageBox.Show("Something went wrong: \n Check if Already Added");
            }
            else
            {
                MessageBox.Show("Please check all fields");
            }
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void btn_mob_sell_Click(object sender, RoutedEventArgs e)
        {
            // Add to mobile with sell id 1 and add to sales too
            // Make Method Direct Sell instead of sell and call methods add and update mobile then Sell method
            if (IsMobileValid())
            {



                if (mobile.DirectSellMobile(
                    cb_family.SelectedValue.ToString(),
                    cb_dealer.SelectedValue.ToString(),
                    tb_imei.Text.ToString(),
                    Convert.ToInt16(tb_costprice.Text.ToString()),
                    Convert.ToInt16(tb_sellprice.Text.ToString()),
                    tb_missingparts.Text.ToString(),
                    tb_cust_name.Text,
                    tb_cust_phone.Text
                    ))
                    MessageBox.Show("Mobile Added and Sold");
                else
                    MessageBox.Show("Something went wrong: \n Check if Already Sold");
            }
            else
            {
                MessageBox.Show("Please check all fields");
            }

        }

        private void Reset()
        {
            cb_company.SelectedIndex = 0;
            tb_costprice.Text = "";
            tb_imei.Text = "";
            tb_sellprice.Text = "";
            tb_missingparts.Text = "";
        
        }
        
        /**************************View***Mobiles******************************/

        private String DealerSoldAllQuery(string search, int d_id)
        {
            
            if (d_id == 0)
                 return "SELECT mobile_id, family.family_name, dealer.dealer_name, imei, dealer_price, selling_price, " +
                   "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, isExchanged, exchanged_imei, isReturned, problem, isSold " +
                   "from mobile join family on family.family_id = mobile.family_id join dealer ON dealer.dealer_id = mobile.dealer_id " +
                   "where imei LIKE '%" + search + "%' OR family_name LIKE '%" + search + "%' OR date_added LIKE '%" + search + "%' Order by DATE(date_added) DESC";
            else
                return "SELECT mobile_id, family.family_name, dealer.dealer_name, imei, dealer_price, selling_price, " +
                   "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, isExchanged, exchanged_imei, isReturned, problem, isSold " +
                   "from mobile join family on family.family_id = mobile.family_id join dealer ON dealer.dealer_id = mobile.dealer_id " +
                   "where (imei LIKE '%" + search + "%' OR family_name LIKE '%" + search + "%' OR date_added LIKE '%" + search + "%' ) AND mobile.dealer_id = " +  d_id + " Order by DATE(date_added) DESC";
        }

        private String DealersSoldQuery(int d_id, int is_sold, string search)
        {
            if(d_id == 0)
                return "SELECT mobile_id, family.family_name, dealer.dealer_name, imei, dealer_price, selling_price, " +
                   "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, isExchanged, exchanged_imei, isReturned, problem, isSold " +
                   "from mobile join family on family.family_id = mobile.family_id join dealer ON dealer.dealer_id = mobile.dealer_id " +
                   "where isSold = 1 AND (imei LIKE '%" + search + "%' OR family_name LIKE '%" + search + "%' OR date_added LIKE '%" + search + "%' ) Order by DATE(date_added) DESC";
            else
                return "SELECT mobile_id, family.family_name, dealer.dealer_name, imei, dealer_price, selling_price, " +
                    "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, isExchanged, exchanged_imei, isReturned, problem, isSold " +
                    "from mobile join family on family.family_id = mobile.family_id join dealer ON dealer.dealer_id = mobile.dealer_id " +
                    "where isSold = 1 AND (imei LIKE '%" + search + "%' OR family_name LIKE '%" + search + "%' OR date_added LIKE '%" + search + "%' ) AND mobile.dealer_id = " + d_id + " Order by DATE(date_added) DESC";
           
        }

        private String DealersUnsoldQuery(int d_id, int is_sold, string search)
        {
            if (d_id == 0)
                  return "SELECT mobile_id, family.family_name, dealer.dealer_name, imei, dealer_price, selling_price, " +
                     "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, isExchanged, exchanged_imei, isReturned, problem, isSold " +
                     "from mobile join family on family.family_id = mobile.family_id join dealer ON dealer.dealer_id = mobile.dealer_id " +
                     "where isSold = 0 AND ( imei LIKE '%" + search + "%' OR family_name LIKE '%" + search + "%' OR date_added LIKE '%" + search + "%') Order by DATE(date_added) DESC";
            else
                  return "SELECT mobile_id, family.family_name, dealer.dealer_name, imei, dealer_price, selling_price, " +
                     "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, isExchanged, exchanged_imei, isReturned, problem, isSold " +
                     "from mobile join family on family.family_id = mobile.family_id join dealer ON dealer.dealer_id = mobile.dealer_id " +
                     "where isSold = 0 AND (imei LIKE '%" + search + "%' OR family_name LIKE '%" + search + "%' OR date_added LIKE '%" + search + "%') AND mobile.dealer_id = " + d_id + " Order by DATE(date_added) DESC";
           
        }

    

        private void LoadAdapter(string queryString, MySqlConnection con)
        {
            adapter = new MySqlDataAdapter(queryString, con);
            dataset = new DataSet();   
        }

        private void LoadMobiles(int d_value, int s_value, string search)
        {
            mycon2 = new MyConnection();
            con2 = mycon2.GetConnection();
            
            //System.Diagnostics.Debug.WriteLine(" " + d_value + " " + s_value + " " + search );

            if (d_value == 0 && s_value == 0){
                LoadAdapter(DealerSoldAllQuery(search, 0), con2);
            //    System.Diagnostics.Debug.WriteLine("1");
            }
            else if (s_value != 0 && d_value == 0) {
             //   System.Diagnostics.Debug.WriteLine("1");
                if (s_value == 1)
                    LoadAdapter(DealersSoldQuery(0, 1, search), con2);
                else if (s_value == 2)
                    LoadAdapter(DealersUnsoldQuery(0, 0, search), con2);
            }
            else if (s_value != 0 && d_value != 0)
            {
                if (s_value == 1)
                    LoadAdapter(DealersSoldQuery(d_value, 1, search), con2);
                else if (s_value == 2)
                    LoadAdapter(DealersUnsoldQuery(d_value, 0, search), con2);
            }
            else if (s_value == 0 && d_value != 0)
            {
                LoadAdapter(DealerSoldAllQuery(search, d_value), con2);
            }
            else
            {
                LoadAdapter(DealerSoldAllQuery(search, 0), con2);
            }

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

        private void PopulateDealerSort()
        {
            con3 = mycon3.GetConnection();
            try
            {
                con3.Open();

                //MessageBox.Show("Connection Open ! ");
                MySqlCommand cmd = con3.CreateCommand();
                cmd.CommandText = "SELECT dealer_name FROM dealer order by dealer_id";

                MySqlDataReader reader = cmd.ExecuteReader();

                cb_dealerSort.Items.Clear();

                if (reader.HasRows)
                {
                    cb_dealerSort.Items.Add("All");

                    while (reader.Read())
                    {
                        //companies.Add(reader.GetString(0));
                        cb_dealerSort.Items.Add(reader[0]);
                     //   System.Diagnostics.Debug.WriteLine("Dsada + " + reader[0]);
                    }
                    //    cb_dealer.SelectedValue = cb_family.Items[0];
                    //cb_company.ItemsSource = companies;
                }
                else
                {
                    cb_dealerSort.Items.Add("No dealer found");
                }
                
                cb_dealerSort.SelectedIndex = 0;
                con3.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message);
            }
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

        private void dgrid_repairs_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) e.Handled = true;
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView drv = (DataRowView)dgrid_mobiles.SelectedItem;
                String[] mobile_fields = new String[11];

                for (int i = 0; i < 11; i++)
                {
                    mobile_fields[i] = (drv.Row[i]).ToString();
                }

                int family_id = id.GetFamilyID(mobile_fields[1]);
                int dealer_id = id.GetDealerID(mobile_fields[2]);

               // System.Diagnostics.Debug.WriteLine("" + mobile_fields[0] + " " + mobile_fields[2] + " " + mobile_fields[10]);

                mobile.UpdateMobile(
                    Convert.ToInt32(mobile_fields[0]),
                    family_id,
                    dealer_id,
                    mobile_fields[3],
                    Convert.ToInt32(mobile_fields[4]),
                    Convert.ToInt32(mobile_fields[5]),
                    mobile_fields[6],
                    Convert.ToInt32(mobile_fields[7]),
                    mobile_fields[8],
                    Convert.ToInt32(mobile_fields[9]),
                    mobile_fields[10]
                );
 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ ex.Message);

            }
        }

        private void btn_view_reset_Click(object sender, RoutedEventArgs e)
        {
            LoadMobiles(0, 0, "");
            cb_dealerSort.SelectedIndex = 0;
            tb_search.Text = "";
        }

        private void tb_search_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {    
              LoadMobiles(cb_dealerSort.SelectedIndex, cb_soldSort.SelectedIndex, tb_search.Text);
            }
        }

        private void cb_dealerSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if (cb_dealerSort.SelectedIndex > 0)
            {
                LoadMobiles(cb_dealerSort.SelectedIndex, cb_soldSort.SelectedIndex, tb_search.Text);
            }
            else
            {
                LoadMobiles(0, cb_soldSort.SelectedIndex, tb_search.Text);
            }
        }

        private void cb_soldSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_soldSort.SelectedIndex > 0)
            {
                LoadMobiles(cb_dealerSort.SelectedIndex, cb_soldSort.SelectedIndex, tb_search.Text);
            }
            else
            {
                LoadMobiles(cb_dealerSort.SelectedIndex, 0, tb_search.Text);
            }
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