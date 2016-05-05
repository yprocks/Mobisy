using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Mobisy.AppCodes
{
    class MyConnection
    {
        MySqlConnection cnn;

        public MyConnection()
        {}

        public MySqlConnection GetConnection(){
            string myConnectionString = "server=localhost;user id=root;database=mobisy;allowuservariables=True";
            cnn = new MySqlConnection (myConnectionString);
            return cnn;
        }
    }
}