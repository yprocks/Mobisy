using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Mobisy.AppCodes
{
    class Repair
    {
        MyConnection mycon;
        MySqlConnection con;
        ID id;

        //bool dateValid, validRpman, validRprice, validMobile, validProblem, validCustphone, validCustname, validCustprice;

        public Repair()
        {
            mycon = new MyConnection();
            id = new ID();
          
        }

        public bool AddRepair(int r_id, string mobile, string problem, int r_price, int cust_price, string cust_name, string cust_phone, string date )
        {
           // int fid = id.GetFamilyID(fname);
            //int did = id.GetDealerID(dname);

            try
            {
                con = mycon.GetConnection();

                string query = "INSERT INTO repairs (repairsman_id, mobile_name, mobile_problem, repairsman_price, cust_price, cust_name, cust_phone, fixing_date, date_added) VALUES ("+ r_id + ", '" + mobile + "', '" + problem + "', " + r_price+ ", " + cust_price + ", " + "'" + cust_name + "', '" + cust_phone + "', '" + date + "', '" +  GetDate() +"' );";
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
            return true;
        }

        private String GetDate()
        {
            return System.DateTime.Now.Date.ToString("yyyy-MM-dd");
        }


        public void UpdateRepair(int r_id, int rpman_id, string mobile_name, string cust_name, string cust_phone, string problem, int rpman_price, int cust_price, string date_added, string fixing_date, int isRepaired, int isPaid, int isRemoved)
        {
           // System.Diagnostics.Debug.WriteLine(" "+ r_id + " " + rpman_id + " " + mobile_name + " " + cust_name + " " + cust_phone + " " + problem + " " + rpman_price + " " + cust_price + " " + date_added + " " + fixing_date + " " + isRepaired + " " + isPaid);
            try
            {
                con = mycon.GetConnection();

                string query = "UPDATE repairs SET repairsman_id = " + rpman_id +", mobile_name = '" + mobile_name + "', mobile_problem = '" +  problem + "', repairsman_price = " + rpman_price +", cust_price = " + cust_price +", cust_name = '" + cust_name + "', cust_phone = '" + cust_phone + "', fixing_date = '" + fixing_date + "', date_added = '" + date_added + "', isPaid = " + isPaid + ", isRepaired = " + isRepaired + ", isRemoved = " + isRemoved + " WHERE repairs_id = " + r_id;
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

    }
}
