using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Mobisy.AppCodes
{
    class Accessories
    {
        MyConnection mycon;
        MySqlConnection con;

        public Accessories()
        {
            mycon = new MyConnection();
        }


        public bool AddAccessories()
        {

            return true;
        }


        public bool UpdateAccessories(int id)
        {

            return true;
        }

    }
}
