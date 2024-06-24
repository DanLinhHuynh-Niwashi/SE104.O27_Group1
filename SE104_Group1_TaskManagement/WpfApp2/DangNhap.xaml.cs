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
using TestBUS;

namespace TestGUI
{
    /// <summary>
    /// Interaction logic for DangNhap.xaml
    /// </summary>
    public partial class DangNhap : Window
    {
        public DangNhap()
        {
            InitializeComponent();
        }
        BUS_TaiKhoan TKBLL = new BUS_TaiKhoan();
        DTO_TaiKhoan taikhoan = new DTO_TaiKhoan();
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            taikhoan.EMAIL = txtTaiKhoan.Text;
            taikhoan.PASS = txtMatKhau.Password;

            string getuser = TKBLL.CheckLogic(taikhoan);

            // Thể hiện trả lại kết quả nếu nghiệp vụ không đúng
            switch (getuser)
            {
                case "requeid_taikhoan":
                    MessageBox.Show("Tài khoản không được để trống");
                    return;

                case "requeid_password":
                    MessageBox.Show("Mật khẩu không được để trống");
                    return;

                case "Tài khoản hoặc mật khẩu không chính xác!":
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!");
                    return;
            }

            MessageBox.Show("Xin chúc mừng bạn đã đăng nhập thành công hệ thống!");
            Admin admin = new Admin();
            admin.Show();
            this.Close();
        }

        private void DangKi_Click(object sender, RoutedEventArgs e)
        {
            DangKi dangki = new DangKi();
            dangki.Show();
            this.Close();
        }


        private void Thoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow mainwindow = new MainWindow();   
            mainwindow.Show();
        }

        private void DoiMK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            DoiMatKhau doimatkhau = new DoiMatKhau();
            doimatkhau.Show();
        }
    }
}
