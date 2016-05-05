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
using Mobisy.AppCodes;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Mobisy.AppPages
{
    /// <summary>
    /// Interaction logic for Stock.xaml
    /// </summary>
    public partial class Sell : Page
    {
        MyConnection mycon1, mycon2, mycon3;
        MySqlConnection con1, con2, con3;
        ID id;
        Mobile mobile;
        Accessories acc;

        int d_id, f_id;
        string company;
        bool compSelected, nameSelected, dealerSelected, IMEISelected, phoneValid;

        public Sell()
        {
            InitializeComponent();
            mycon1 = new MyConnection();
            mycon2 = new MyConnection();
            mycon3 = new MyConnection();
            id = new ID();
            mobile = new Mobile();
            acc = new Accessories();
            company = "";

            compSelected = false;
            nameSelected = false;
            dealerSelected = false;
            IMEISelected = false;
            phoneValid = true;
            
        }

        private void add_mob_click(object sender, RoutedEventArgs e)
        {
            mobile_tab.Background = Brushes.LightGray;
            accessories_tab.Background = Brushes.Transparent;

            mobile_panel.Visibility = System.Windows.Visibility.Visible;
            accessories_panel.Visibility = System.Windows.Visibility.Hidden;

            MobilePanel();
        }

        private void add_acc_click(object sender, RoutedEventArgs e)
        {
            accessories_tab.Background = Brushes.LightGray;
            mobile_tab.Background = Brushes.Transparent;

            accessories_panel.Visibility = System.Windows.Visibility.Visible;
            mobile_panel.Visibility = System.Windows.Visibility.Hidden;


        }

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
                }
                else
                    cb_company.Items.Add("No Company Found");
                
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
                tb_sellprice.Text = "";
                tb_costprice.Text = "";
                // MessageBox.Show("In");
            }
            else
            {
                cb_family.Items.Clear();
                cb_dealer.Items.Clear();
                cb_IMEI.Items.Clear();
                tb_sellprice.Text = "";
                tb_costprice.Text = "";
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
                }else
                    cb_family.Items.Add("No Mobiles Found");
                
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
                f_id = id.GetFamilyID((string)cb_family.SelectedValue);
                nameSelected = true;
                PopulateDealers();
                tb_sellprice.Text = "";
                tb_costprice.Text = "";
            }
            else
            {
                cb_dealer.Items.Clear();
                cb_IMEI.Items.Clear();
                nameSelected = false;
                tb_sellprice.Text = "";
                tb_costprice.Text = "";
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

                cb_dealer.Items.Add("Select");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //companies.Add(reader.GetString(0));
                        cb_dealer.Items.Add(reader[0]);

                    }
                    //    cb_dealer.SelectedValue = cb_family.Items[0];
                    //cb_company.ItemsSource = companies;
                }else
                    cb_dealer.Items.Add("No dealers found");
                
                cb_dealer.SelectedIndex = 0;


                con3.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message);
            }
        }

        private void cb_dealer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_dealer.SelectedIndex != 0)
            {
                d_id = id.GetDealerID((string)cb_dealer.SelectedValue);
                dealerSelected = true;
                PopulateIMEI();
                tb_sellprice.Text = "";
                tb_costprice.Text = "";
            }
            else
            {
                cb_IMEI.Items.Clear();
                dealerSelected = false;
                tb_sellprice.Text = "";
                tb_costprice.Text = "";
            }
        }

        private void PopulateIMEI()
        {
            con3 = mycon3.GetConnection();
            try
            {
                con3.Open();

                //MessageBox.Show("Connection Open ! ");
                MySqlCommand cmd = con3.CreateCommand();
                cmd.CommandText = "SELECT imei FROM mobile where dealer_id = "+ d_id + " and family_id = " + f_id + " and isSold = 0";

                MySqlDataReader reader = cmd.ExecuteReader();

                cb_IMEI.Items.Clear();

                if (reader.HasRows) { 
                    cb_IMEI.Items.Add("Select");

                    while (reader.Read())
                    {
                        //companies.Add(reader.GetString(0));
                        cb_IMEI.Items.Add(reader.GetString(0));
                        tb_sellprice.Text = "";
                        tb_costprice.Text = "";
                    }
                    //    cb_dealer.SelectedValue = cb_family.Items[0];
                    //cb_company.ItemsSource = companies;
                }
                else
                {
                    cb_IMEI.Items.Add("No mobile Found");
                }

                cb_IMEI.SelectedIndex = 0;

                con3.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message);
            }
        }

        private void cb_IMEI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_IMEI.SelectedIndex != 0)
            {
                int sp = mobile.GetSellingPrice((string)cb_IMEI.SelectedValue);
                int cp = mobile.GetCostPrice((string)cb_IMEI.SelectedValue);
                tb_sellprice.Text = "" + sp;
                tb_costprice.Text = "" + cp;
                IMEISelected = true;
            }
            else
            {
                IMEISelected = false;
                tb_sellprice.Text = "";
                tb_costprice.Text = "";
            }
        }

        private void tb_price_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char c = Convert.ToChar(e.Text);
            if (Char.IsNumber(c))
                e.Handled = false;
            else
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }

        private void tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private void tb_cust_phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_cust_phone.Text))
            {
                phoneValid = true;
            }
            else
            {
                if (tb_cust_phone.Text.Length == 10)
                {
                    phoneValid = true;
                }
                else
                {
                    phoneValid = false;
                }
            }
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void Reset()
        {

            cb_company.SelectedIndex = 0;
            cb_itemname.SelectedIndex = 0;
            cb_family.Items.Clear();
            cb_dealer.Items.Clear();
            cb_IMEI.Items.Clear();

            tb_costprice.Text = "";
            tb_sellprice.Text = "";
            tb_cust_name.Text = "";
            tb_cust_phone.Text = "";
            tb_itemCP.Text = "";
            tb_itemSP.Text = "";
        }

        private void btn_mob_sell_Click(object sender, RoutedEventArgs e)
        {
            // Make Method Sell which updates Mobile and add into relevent tables
            if (compSelected && nameSelected && IMEISelected && dealerSelected && phoneValid && tb_costprice.Text.Length > 0 && tb_sellprice.Text.Length > 0)
            {
                if (mobile.SellMobile(cb_IMEI.Text, Convert.ToInt32(tb_costprice.Text), Convert.ToInt32(tb_sellprice.Text), tb_cust_name.Text, tb_cust_phone.Text))
                {
                    MessageBox.Show("Mobile Sold");
                    Reset();
                }
                else
                {
                    MessageBox.Show("Something went wrong");
                }
            }
            else
            {
                MessageBox.Show("Please Check all fields");
            }
        }

      
    }
}
