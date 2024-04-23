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

namespace GUI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private string _username;
        private string _password;
        public string Username { get { return _username; } set {  _username = value; } }
        public string Password { get { return _password;} set { _password = value; } }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dang nhap thanh cong");

        }
    }
}