using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using DAL;
using System.Windows.Navigation;
using BUS;

namespace TestGUI
{
    /// <summary>
    /// Interaction logic for DangKi.xaml
    /// </summary>
    public partial class DangKi : Window
    {
        public DangKi()
        {
            InitializeComponent();
        }
        BUS_TaiKhoan taotaikhoan = new BUS_TaiKhoan();
        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra xem các trường thông tin đã được điền đầy đủ hay chưa
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPass.Password))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }
            if (txtPass.Password != txtPassAgain.Password)
            {
                MessageBox.Show("Mật khẩu không trùng khớp.");
                return;
            }

            // Tạo một đối tượng DTO_TaiKhoan từ các trường thông tin được nhập vào
            DTO_TaiKhoan taiKhoan = new DTO_TaiKhoan()
            {
                EMAIL = txtEmail.Text,
                PASS = txtPass.Password,
                MAQH = "1",
                // Các thông tin khác có thể được cung cấp tùy thuộc vào yêu cầu
            };

            // Gọi phương thức tạo tài khoản và lưu kết quả
            string result = taotaikhoan.TaoTaiKhoan(taiKhoan);

            // Hiển thị thông báo kết quả
            MessageBox.Show(result);
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Admin admin = new Admin();
            admin.Show();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
