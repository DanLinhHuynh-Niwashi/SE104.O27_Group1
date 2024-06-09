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
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Window
    {
        public DTO_NhanVien crnUser = new DTO_NhanVien();
        
        public UserInfo()
        {
            InitializeComponent();
            crnUser = MainWindow.crnNhanVien;
            manvText.Text = crnUser.MANV;
            tennvText.Text = crnUser.TENNV;
            cmText.Text = crnUser.MACM;
            levelText.Text = crnUser.LEVEL.ToString();
            dobText.Text = crnUser.NGAYSINH != null ? crnUser.NGAYSINH.Value.Date.ToString() : "";
            emailText.Text = crnUser.EMAIL;
            phoneText.Text = crnUser.PHONE;
            noteText.Text = crnUser.GHICHU;
            genderText.Text = crnUser.GENDER;
            qhText.Text = BUS_NhanVien.Instance.GetQuyenHan(crnUser).TENQH;
        }

        private void back_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void changePass_Btn_Click(object sender, RoutedEventArgs e)
        {
            ChangePassword cPWindow = new ChangePassword(1);
            cPWindow.ShowDialog();
        }
    }
}
