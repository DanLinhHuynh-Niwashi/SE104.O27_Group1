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

namespace TestGUI
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void ThemNV_Click(object sender, RoutedEventArgs e)
        {
            DangKi dangki = new DangKi();
            dangki.Show();
            this.Close();
        }

        private void XoaNV_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SuaNV_Click(object sender, RoutedEventArgs e)
        {

        }

        private void XemBC_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ThemDA_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SuaDA_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SuaQD_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ThemCV_Click(object sender, RoutedEventArgs e)
        {

        }

        private void XoaCV_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SuaCV_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CapNhatCV_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
