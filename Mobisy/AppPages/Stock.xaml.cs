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
    public partial class Stock : Page
    {
        MyConnection mycon1, mycon2, mycon3;
        MySqlConnection con1, con2, con3;
        ID id;
        Mobile mobile;
        Accessories acc;

        string company;
        
        bool isMobileValid, isAccValid;
        bool compSelected, nameSelected, dealerSelected, validImei, validCP, validSP, validitem, validItemSP, validItemCP;

        
        public Stock()
        {
            InitializeComponent();
            mycon1 = new MyConnection();
            mycon2 = new MyConnection();
            mycon3 = new MyConnection();
            id = new ID();
            mobile = new Mobile();
            acc = new Accessories();

            company = "";
            isMobileValid = isAccValid = false;
            compSelected = nameSelected = dealerSelected = validCP = validImei = validSP = false;

            PopulateCompanies();
        }
        

        private void add_mob_click(object sender, RoutedEventArgs e)
        {
            add_mobile_tab.Background = Brushes.LightGray;
            add_accessories_tab.Background = Brushes.Transparent;

            mobile_panel.Visibility = System.Windows.Visibility.Visible;
            accessories_panel.Visibility = System.Windows.Visibility.Hidden;

            MobilePanel();
            Reset();
        }


        private void add_acc_click(object sender, RoutedEventArgs e)
        {
            add_accessories_tab.Background = Brushes.LightGray;
            add_mobile_tab.Background = Brushes.Transparent;

            accessories_panel.Visibility = System.Windows.Visibility.Visible;
            mobile_panel.Visibility = System.Windows.Visibility.Hidden;
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
            if (cb_company.SelectedIndex != 0) { 
                company =  (string)cb_company.SelectedValue;
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


        private void Reset()
        {
            cb_company.SelectedIndex = 0;
            tb_costprice.Text = "";
            tb_imei.Text = "";
            tb_sellprice.Text = "";
            tb_missingparts.Text = "";
            tb_itemname.Text = "";
            tb_itemCP.Text = "";
            tb_itemSP.Text = "";
        }


        /********************* Completed **** Mobile **** Panel*******************/

        /********************* Start **** Accessories **** Panel*******************/


        private void tb_itemname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_itemname.Text))
            {
                validitem = false;
            }
            else
            {
                validitem = true;
            }
        }


        private void tb_itemCP_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_itemCP.Text))
            {
                validItemCP = false;
            }
            else
            {
                validItemCP = true;
            }
        }


        private void tb_itemSP_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_itemSP.Text))
            {
                validItemSP = false;
            }
            else
            {
                validItemSP = true;
            }
        }


        private void tb_itemSP_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char c = Convert.ToChar(e.Text);
            if (Char.IsNumber(c) && tb_itemSP.Text.Length < 5)
                e.Handled = false;
            else
                e.Handled = true;

            base.OnPreviewTextInput(e);

        }


        private void tb_itemCP_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char c = Convert.ToChar(e.Text);
            if (Char.IsNumber(c) && tb_itemCP.Text.Length < 5)
                e.Handled = false;
            else
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }


        private bool IsAccValid()
        {
            if (validitem && validItemCP && validItemSP)
            {
                isAccValid = true;
            }
            else
            {
                isAccValid = false;
            }

            return isAccValid;
        }


        private void btn_acc_add_Click(object sender, RoutedEventArgs e)
        {
            if (IsAccValid())
            {
                tb_itemname.Text.ToString();
                tb_itemSP.Text.ToString();
                tb_itemCP.Text.ToString();

                MessageBox.Show("Data Added");
            }
            else
            {
                MessageBox.Show("Please check all fields");
            }
        }



        /*****************************Button***Sell***************************/


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
     
    }
}
