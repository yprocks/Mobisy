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

        }
        


        private void add_acc_click(object sender, RoutedEventArgs e)
        {
         
        }

        // popup data in mobile panel



        /********************* Completed **** Mobile **** Panel*******************/
        /********************* Start **** Accessories **** Panel*******************/
        private void tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        private void Reset()
        {
 
            tb_itemname.Text = "";
            tb_itemCP.Text = "";
            tb_itemSP.Text = "";
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }



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


       
     
    }
}
