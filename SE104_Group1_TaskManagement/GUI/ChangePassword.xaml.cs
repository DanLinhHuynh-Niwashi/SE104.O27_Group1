using BUS;
using DTO;
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
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        int mode = -1;
        BUS_TaiKhoan tkManager = new BUS_TaiKhoan();
        public ChangePassword(int mode)
        {
            InitializeComponent();
            if (mode == 1)
            {
                codeLabel.Visibility = Visibility.Collapsed;
                codeText.Visibility = Visibility.Collapsed;
                resend_Btn.Visibility = Visibility.Collapsed;
            }    
            else if (mode == 2)
            {
                recoverLabel.Text = "Email";
            }
            this.mode = mode;
        }

        private void confirm_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (mode == 1)
            {
                (string, DTO_TaiKhoan) newTk = tkManager.ChangePassWord(LoginWindow.crnUser.EMAIL, recoverText.Text, newPassText.Password);
                if (newTk.Item2 != null && newTk.Item2.MANV!="")
                {
                    LoginWindow.crnUser = newTk.Item2;
                    MessageBox.Show(newTk.Item1);
                    this.Close();
                }   
                else
                {
                    MessageBox.Show(newTk.Item1);
                }    
                
            }    
        }

        private void resend_Btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
