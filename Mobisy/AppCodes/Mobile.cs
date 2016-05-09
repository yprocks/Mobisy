using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Mobisy.AppCodes
{
    class Mobile
    {
        MyConnection mycon;
        MySqlConnection con, con1;
        ID id;
        int sp, cp;

        public Mobile()
        {
            mycon = new MyConnection();
            id = new ID();
            sp = 0;
            cp = 0;
        }


        public bool AddMobile (string fname, string dname, string imei, int cp, int sp, string mparts)
        {

            int fid = id.GetFamilyID(fname);
            int did = id.GetDealerID(dname);

            try
            {
                con = mycon.GetConnection();

                string query = "insert into mobile (mobile_id, family_id, dealer_id, imei, dealer_price, selling_price, missing_parts, date_added) values (null, " + fid + ", " + did + ", '"+ imei + "', "+ cp + ", " + sp + ", " + "'" + mparts +"', '" + GetDate() +"' );";
                System.Diagnostics.Debug.WriteLine("At Pass1");
                MySqlCommand cmd = new MySqlCommand(query, con);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                
            }
            catch (MySqlException ex)
            {
                return false;
            }
            return true;
        }

        public bool DirectSellMobile(string fname, string dname, string imei, int cp, int sp, string mparts, string cust_name, string cust_phone)
        {

            int fid = id.GetFamilyID(fname);
            int did = id.GetDealerID(dname);
            int mid = 0;

            try
            {
                con = mycon.GetConnection();

                string query = "insert into mobile (mobile_id, family_id, dealer_id, imei, dealer_price, selling_price, missing_parts, date_added, isSold, date_sold) values (null, " + fid + ", " + did + ", '" + imei + "', " + cp + ", " + sp + ", " + "'" + mparts + "', '" + GetDate() + "', 1, '" + GetDate() + "'); SELECT MAX(mobile_id) FROM mobile;";
               // System.Diagnostics.Debug.WriteLine("At Pass1");
                MySqlCommand cmd = new MySqlCommand(query, con);

                con.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

               
                    while (reader.Read())
                    {
                        //companies.Add(reader.GetString(0));
                        mid = reader.GetInt32(0);
                    }
                

                con.Close();

            }
            catch (MySqlException ex)
            {
                return false;
            }

            AddCustomer(mid, cust_name, cust_phone);

            return true;
        }

        public int GetSellingPrice(string imei)
        {
            con = mycon.GetConnection();
            try
            {
                con.Open();

                //MessageBox.Show("Connection Open ! ");
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT selling_price from mobile where imei = '" + imei + "'";

                MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    //companies.Add(reader.GetString(0));
                    sp = reader.GetInt32(0);
                }

                //cb_company.ItemsSource = companies;

                con.Close();
            }
            catch (Exception ex)
            {
            }

            return sp;

        }

        public int GetCostPrice(string imei)
        {
            con = mycon.GetConnection();
            try
            {
                con.Open();

                //MessageBox.Show("Connection Open ! ");
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT dealer_price from mobile where imei = '" + imei + "'";

                MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    //companies.Add(reader.GetString(0));
                    cp = reader.GetInt32(0);
                }

                //cb_company.ItemsSource = companies;

                con.Close();
            }
            catch (Exception ex)
            {
            }

            return cp;

        }

        public bool SellMobile(string imei, int dealer_price, int selling_price, string cust_name, string cust_phone)
        {

            int mid = id.GetMobieID(imei);

            try
            {
                con = mycon.GetConnection();

                string query = "update Mobile set dealer_price = "+ dealer_price +", selling_price = "+ selling_price +", isSold = 1, date_sold = '"+ GetDate() +"' where mobile_id = " + mid;
                //System.Diagnostics.Debug.WriteLine("At Pass1");
                MySqlCommand cmd = new MySqlCommand(query, con);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (MySqlException ex)
            {
                return false;
            }

            AddCustomer(mid, cust_name, cust_phone);

            return true;
        }

        private void AddCustomer(int m_id, string name, string phone)
        {
            try
            {
                con = mycon.GetConnection();

                string query = "insert into customer (mobile_id, cust_name, cust_phone) values (" + m_id + ", '" + name + "', '" + phone + "');";
                //System.Diagnostics.Debug.WriteLine("At Pass1");
                MySqlCommand cmd = new MySqlCommand(query, con);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (MySqlException ex)
            {
            }
        }

        public void UpdateMobile(int m_id, int f_id, int d_id, string imei, int c_price, int s_price, string date_added, int isExchanged, string exchangedIMEI, int isReturned, string problem)
        {
            try
            {
                con = mycon.GetConnection();

                string query = "UPDATE mobile SET family_id = " + f_id + ", dealer_id = " + d_id + ", imei = '" + imei + "', dealer_price = " + c_price + ", selling_price = " + s_price  + ", date_added = '" + date_added + "', isExchanged = " + isExchanged + ", exchanged_imei = '" + exchangedIMEI + "', isReturned = " + isReturned + ", problem = '" + problem + "' WHERE mobile_id = " + m_id;
                // System.Diagnostics.Debug.WriteLine("At Pass1");

                MySqlCommand cmd = new MySqlCommand(query, con);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex.Message);
            }
        }

        public void UpdateSales(int m_id, string cust_name, string cust_phone, int selling_price)
        {
            try
            {
                con = mycon.GetConnection();

                string query = "UPDATE mobile SET selling_price = " + selling_price + " WHERE mobile_id = " + m_id;
                // System.Diagnostics.Debug.WriteLine("At Pass1");

                MySqlCommand cmd = new MySqlCommand(query, con);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


                con1 = mycon.GetConnection();

                string query1 = "UPDATE customer SET cust_name = " + cust_name + ", cust_phone = " + cust_phone + " WHERE customer.mobile_id = " + m_id;
                // System.Diagnostics.Debug.WriteLine("At Pass1");

                MySqlCommand cmd1 = new MySqlCommand(query1, con1);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine("" + ex.Message);
            }
        }


        private String GetDate()
        {
            return System.DateTime.Now.Date.ToString("yyyy-MM-dd");
        }

    }
}
