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
    public partial class Repairs : Page
    {
        MyConnection mycon1, mycon2, mycon3;
        MySqlConnection con1, con2, con3;
        ID id;
        Repair rp;
        MySqlDataAdapter adapter;
        DataSet dataset;
        int r_id;

        bool dateValid, validRpman, validRprice, validMobile, validProblem, validCustphone, validCustname, validCustprice;

        public Repairs()
        {
            InitializeComponent();
            mycon1 = new MyConnection();

            id = new ID();
            LoadRepairsmen();
            dateValid = validRprice = validRpman = validMobile = validProblem = validCustname = validCustprice = false;
            validCustphone = true;
            tb_repair_date.Text = GetDate();
            r_id = 0;
            rp = new Repair();
            LoadRepairs(0);

        }

        private String GetDate()
        {
            return System.DateTime.Now.Date.ToString("yyyy-MM-dd");
        }

        private void add_rep_click(object sender, RoutedEventArgs e)
        {
            add_repair_tab.Background = Brushes.LightGray;
            view_reapir_tab.Background = Brushes.Transparent;

            add_rep_panel.Visibility = System.Windows.Visibility.Visible;
            view_panel.Visibility = System.Windows.Visibility.Hidden;

            LoadRepairsmen();

        }

        private void view_rep_click(object sender, RoutedEventArgs e)
        {
            view_reapir_tab.Background = Brushes.LightGray;
            add_repair_tab.Background = Brushes.Transparent;

            view_panel.Visibility = System.Windows.Visibility.Visible;
            add_rep_panel.Visibility = System.Windows.Visibility.Hidden;

        }

        private void LoadRepairsmen()
        {
            con1 = mycon1.GetConnection();
            try
            {
                con1.Open();

                //MessageBox.Show("Connection Open ! ");

                MySqlCommand cmd = con1.CreateCommand();
                cmd.CommandText = "SELECT repairsman_name from repairsman";

                MySqlDataReader reader = cmd.ExecuteReader();

                cb_rpman.Items.Clear();

                if (reader.HasRows)
                {

                    cb_rpman.Items.Add("Select");

                    while (reader.Read())
                    {
                        //companies.Add(reader.GetString(0));
                        cb_rpman.Items.Add(reader[0]);
                    }
                    //cb_company.ItemsSource = companies;
                }
                else
                {
                    cb_rpman.Items.Add("No Repairsman Found");
                }
                cb_rpman.SelectedIndex = 0;

                con1.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Can not open connection ! " + ex.Message);
            }
        }

        private void tb_mobile_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_mobile.Text))
            {
                validMobile = false;
            }
            else
            {
                validMobile = true;
            }
        }

        private void tb_problem_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_problem.Text))
            {
                validProblem = false;
            }
            else
            {
                validProblem = true;
            }
        }

        private void tb_rpman_price_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_rpman_price.Text))
            {
                validRprice = false;
            }
            else
            {
                validRprice = true;
            }
        }

        private void tb_cust_price_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_cust_price.Text))
            {
                validCustprice = false;
            }
            else
            {
                validCustprice = true;
            }
        }

        private void tb_cust_name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_cust_name.Text))
            {
                validCustname = false;
            }
            else
            {
                validCustname = true;
            }
        }

        private void tb_cust_phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_cust_phone.Text))
            { 
                validCustphone = true;
            }
            else
            {
                if (tb_cust_phone.Text.Length == 10)
                {
                    validCustphone = true;
                }
                else
                {
                    validCustphone = false;
                }
            }
        }

        private void tb_repair_date_TextChanged(object sender, TextChangedEventArgs e)
        {        

            if (tb_repair_date.Text.Length > 7)
            {
                dateValid = true;
            }
            else
            {
                dateValid = false;
            }
        }

        private void btn_rep_reset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void btn_rep_add_Click(object sender, RoutedEventArgs e)
        {
            if (dateValid && validMobile && validProblem && CheckRpmanSelected() && validRprice && validCustprice && validCustname && validCustphone)
            {
                r_id = id.GetRepairsmanID(cb_rpman.Text.ToString());

                if (rp.AddRepair(r_id, tb_mobile.Text, tb_problem.Text, Convert.ToInt32(tb_rpman_price.Text.ToString()), Convert.ToInt32(tb_cust_price.Text.ToString()), tb_cust_name.Text, tb_cust_phone.Text, tb_repair_date.Text)) {
                    MessageBox.Show("Mobile added to Repairs");
                    Reset();
                }else
	            {
                     MessageBox.Show("Something went wrong");
	            }

            }else
	        {
                MessageBox.Show("Please check all fields");
	        }
        }

        private bool CheckRpmanSelected()
        {
            if (cb_rpman.SelectedIndex != 0)
            {
                validRpman = true;
            }
            else
            {
                validRpman = false;
            }

            return validRpman;
        }

        private void Reset()
        {

            cb_rpman.SelectedIndex = 0;

            tb_cust_price.Text = "";
            tb_mobile.Text = "";
            tb_problem.Text = "";
            tb_cust_name.Text = "";
            tb_cust_phone.Text = "";
            tb_repair_date.Text = GetDate();
            tb_rpman_price.Text = "";
        }

        private void tb_repair_date_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Back) e.Handled = false;

            if (tb_repair_date.SelectionStart > 0) { 
            
                if (tb_repair_date.Text[tb_repair_date.SelectionStart - 1] == '/')
                {
                    if (e.Key == Key.Back)
                    {
                        e.Handled = true;
                    }
                }
                
            }
            

            if (e.Key == Key.Delete) e.Handled = true;
            
            if (e.Key == Key.Space) e.Handled = true;

           
        }

        private void tb_number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char c = Convert.ToChar(e.Text);

            if (Char.IsDigit(c))
            {
                e.Handled = false;

            } else
            {
                e.Handled = true;
            }

            base.OnPreviewTextInput(e);

        }
        
        /**************************View***Repairs******************************/

        private void LoadAdapter(string queryString, MySqlConnection con)
        {

            adapter = new MySqlDataAdapter(queryString, con);
            dataset = new DataSet();
            
        }

        private void LoadRepairs(int value)
        {
            mycon2 = new MyConnection();
            con2 = mycon2.GetConnection();
            // MySqlCommand cmd = dbConn.CreateCommand();
            // cmd.CommandText = "SELECT * from demobase";
            if (value == 0)
                LoadAdapter(TodaysRepairsQuery(), con2);
            else if (value == 1)
                LoadAdapter(PendingRepairsQuery(), con2);
            else if (value == 2)
                LoadAdapter(AllRepairsQuery(), con2);
            else if (value == 3)
                LoadAdapter(SearchRepairsQuery(), con2);


            try
            {
                con2.Open();
                adapter.Fill(dataset, "repairs");
              
                dgrid_repairs.ItemsSource = dataset.Tables[0].DefaultView;
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

        private void btn_update_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Error: "+ ex.Message);

            }
        }

        private String TodaysRepairsQuery()
        {
            return "SELECT repairs_id, mobile_name, cust_name, cust_phone, repairsman_name, mobile_problem, repairsman_price, cust_price, " +
                   "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, DATE_FORMAT(fixing_date,'%y-%m-%d') as fixing_date, isRepaired, isPaid, isRemoved " + 
                   "from repairs JOIN repairsman on repairsman.repairsman_id = repairs.repairsman_id where DATE(date_added) = '"+ GetDate() +"' and isRemoved = 0";
        }

        private String PendingRepairsQuery()
        {
            return "SELECT repairs_id, mobile_name, cust_name, cust_phone, repairsman_name, mobile_problem, repairsman_price, cust_price, " +
                   "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, DATE_FORMAT(fixing_date,'%y-%m-%d') as fixing_date, isRepaired, isPaid, isRemoved " +
                   "from repairs JOIN repairsman on repairsman.repairsman_id = repairs.repairsman_id where isPaid = 0 and isRemoved = 0";
        }

        private String AllRepairsQuery()
        {
            return "SELECT repairs_id, mobile_name, cust_name, cust_phone, repairsman_name, mobile_problem, repairsman_price, cust_price, " +
                   "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, DATE_FORMAT(fixing_date,'%y-%m-%d') as fixing_date, isRepaired, isPaid, isRemoved " +
                   "from repairs JOIN repairsman on repairsman.repairsman_id = repairs.repairsman_id";
        }

        private String SearchRepairsQuery()
        {
            return "SELECT repairs_id, mobile_name, cust_name, cust_phone, repairsman_name, mobile_problem, repairsman_price, cust_price, " +
                   "DATE_FORMAT(date_added,'%y-%m-%d') as date_added, DATE_FORMAT(fixing_date,'%y-%m-%d') as fixing_date, isRepaired, isPaid, isRemoved " +
                   "from repairs JOIN repairsman on repairsman.repairsman_id = repairs.repairsman_id " +
                   "where mobile_name LIKE '%" + tb_search.Text + "%' OR cust_name LIKE '%" + tb_search.Text + "%' OR cust_phone = '" + tb_search.Text + "' " +
                   "OR repairsman_name Like '%" + tb_search.Text + "%' OR fixing_date LIKE '%" + tb_search.Text + "%' OR date_added LIKE '%" + tb_search.Text + "%'";
        }

        private void btn_view_reset_Click(object sender, RoutedEventArgs e)
        {
            LoadRepairs(0);
            cb_dateSelector.SelectedIndex = 0;
            tb_search.Text = "";
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
            else if (cb_dateSelector.SelectedIndex == 2)
            {
                LoadRepairs(2);
            }
            else if (cb_dateSelector.SelectedIndex == 0)
            {
                LoadRepairs(0);
            }
        }

        private void tb_search_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if(!String.IsNullOrEmpty(tb_search.Text)){
                    cb_dateSelector.SelectedIndex = 2;
                    LoadRepairs(3);
                }
            }
        }

    }
}