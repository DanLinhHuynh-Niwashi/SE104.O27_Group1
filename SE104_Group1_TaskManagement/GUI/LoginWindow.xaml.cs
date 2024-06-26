﻿using System;
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
        public static DTO_TaiKhoan crnUser = new DTO_TaiKhoan();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DTO_TaiKhoan user = new DTO_TaiKhoan("", LgName.Text, Password.Password);
            string str = "";
            (crnUser, str) = BUS_TaiKhoan.Instance.Login(user);
            if (crnUser == null)
            {
                MessageBox.Show("Mat khau hoac email sai, moi nhap lai");
            }
            else
            {
                MainWindow mainWindow = new MainWindow(/*crnUser.MANV*/);
                this.Visibility = Visibility.Collapsed;
                mainWindow.Show();
                this.Close();
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
