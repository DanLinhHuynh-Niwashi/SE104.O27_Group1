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
using BUS;
using DTO;

namespace GUI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static BUS_TaiKhoan taikhoanManager = new BUS_TaiKhoan();
        public static DTO_TaiKhoan crnUser = new DTO_TaiKhoan();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DTO_TaiKhoan user = new DTO_TaiKhoan("", Name.Text, FloatingPasswordBox.Password);
            string str = "";
            (crnUser,str) = taikhoanManager.Login(user);
            if (crnUser != null)
            {
                EmployeesWindow mainWindow = new EmployeesWindow();
                MessageBox.Show(str);
                this.Visibility = Visibility.Collapsed;
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show(str);           
            }

        }
    }
}
