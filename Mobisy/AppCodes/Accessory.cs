using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Mobisy.AppCodes
{
    class Accessory
    {
        MyConnection mycon;
        MySqlConnection con;

        public Accessory()
        {
            mycon = new MyConnection();
        }


        public bool AddSales(String s_name, double c_price, int s_price)
        {

            try
            {
                con = mycon.GetConnection();

                string query = "insert into sales (sales_date, sales_name, cost_price, selling_price) values ('" + GetDate() + "', '" + s_name + "', " + c_price + ", "+ s_price + ");";
               // System.Diagnostics.Debug.WriteLine("At Pass1");
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

        private String GetDate()
        {
            return System.DateTime.Now.Date.ToString("yyyy-MM-dd");
        }
      
    }
}
