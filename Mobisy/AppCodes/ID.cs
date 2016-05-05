using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Mobisy.AppCodes
{
    class ID
    {
        MyConnection mycon;
        MySqlConnection con;
        int c_id;
        int f_id;
        int d_id;
        int r_id;
        int m_id;

        public ID()
        {
            mycon = new MyConnection();
            c_id = 0;
            f_id = 0;
            d_id = 0;
            r_id = 0;
            m_id = 0;
        }

        public int GetCompanyID(String cname)
        {
            con = mycon.GetConnection();
            try
            {
                con.Open();

                //MessageBox.Show("Connection Open ! ");
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT company_id from company where company_name = '" + cname + "'";

                MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    //companies.Add(reader.GetString(0));
                    c_id = reader.GetInt32(0);
                }

                //cb_company.ItemsSource = companies;

                con.Close();
            }
            catch (Exception ex)
            {
            }

            return c_id;

        }

        public int GetFamilyID(String fname)
        {
            con = mycon.GetConnection();
            try
            {
                con.Open();

                //MessageBox.Show("Connection Open ! ");
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT family_id from family where family_name = '" + fname + "'";

                MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    //companies.Add(reader.GetString(0));
                    f_id = reader.GetInt32(0);
                }

                //cb_company.ItemsSource = companies;

                con.Close();
            }
            catch (Exception ex)
            {
            }

            return f_id;

        }

        public int GetDealerID(String dname)
        {
            con = mycon.GetConnection();
            try
            {
                con.Open();

                //MessageBox.Show("Connection Open ! ");
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT dealer_id from dealer where dealer_name = '" + dname + "'";

                MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    //companies.Add(reader.GetString(0));
                    d_id = reader.GetInt32(0);
                }

                //cb_company.ItemsSource = companies;

                con.Close();
            }
            catch (Exception ex)
            {
            }

            return d_id;

        }

        public int GetRepairsmanID(String rname)
        {
            con = mycon.GetConnection();
            try
            {
                con.Open();

                //MessageBox.Show("Connection Open ! ");
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT repairsman_id from repairsman where repairsman_name = '" + rname + "'";

                MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    //companies.Add(reader.GetString(0));
                    r_id = reader.GetInt32(0);
                }

                //cb_company.ItemsSource = companies;

                con.Close();
            }
            catch (Exception ex)
            {
            }

            return r_id;

        }

        public int GetMobieID(String imei)
        {
            con = mycon.GetConnection();
            try
            {
                con.Open();

                //MessageBox.Show("Connection Open ! ");
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT mobile_id from mobile where imei = '" + imei + "'";

                MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    //companies.Add(reader.GetString(0));
                    m_id = reader.GetInt32(0);
                }

                //cb_company.ItemsSource = companies;

                con.Close();
            }
            catch (Exception ex)
            {
            }
            return m_id;
        }

    }
}
