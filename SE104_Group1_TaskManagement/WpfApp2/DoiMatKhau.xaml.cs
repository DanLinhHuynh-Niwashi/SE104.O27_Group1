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
    /// Interaction logic for DoiMatKhau.xaml
    /// </summary>
    public partial class DoiMatKhau : Window
    {
        public DoiMatKhau()
        {
            InitializeComponent();
        }
        private readonly DoiMatKhauBUS doimatkhau = new DoiMatKhauBUS();
        private void DoiMatKhau_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string oldPassword = txtOldPass.Text;
            string newPassword = txtNewPass.Text;
            string confirmPassword = txtConfirmPass.Text;

            // Kiểm tra xác nhận mật khẩu mới
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu mới và xác nhận mật khẩu không khớp!");
                return;
            }

            // Gọi phương thức đổi mật khẩu từ lớp BUS
            string result = doimatkhau.ChangePassword(email, oldPassword, newPassword);

            // Hiển thị kết quả cho người dùng
            MessageBox.Show(result);
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            DangNhap dangnhap = new DangNhap();
            dangnhap.Show();
        }
    }
}
