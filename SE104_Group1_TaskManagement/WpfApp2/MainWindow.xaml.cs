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
using DTO;
using DAL;
using System.Diagnostics.Metrics;
using System.Data;
using TestBUS;
using System.Diagnostics;

namespace TestGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BUS_NhanVien nvManager = new BUS_NhanVien();
        BUS_ChuyenMon cmManager = new BUS_ChuyenMon();
        BUS_LoaiSK lskManager = new BUS_LoaiSK();
     
        public MainWindow()
        {
            InitializeComponent();
            datagrid.ItemsSource = nvManager.GetAllData();
            //
            //data table => 3 cột tên, viết tắt, mã
                         
            foreach (DataRow row in cmManager.GetAllData().Rows)
            {
                CM.Items.Add(row[0].ToString()) ; 
            }    
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int level;
            level = int.TryParse(LVL.Text, out level) ? level : -1;
            DTO_NhanVien nhanvien = new DTO_NhanVien("", TENNV.Text, EMAIL.Text, PHONE.Text, DOB.Text, level, CM.Text, NOTE.Text);
            nvManager.AddData(nhanvien);
            this.datagrid.ItemsSource = nvManager.GetAllData();
        }

        private void datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagrid.SelectedItems.Count > 0)
            {
                DTO_NhanVien member = nvManager.GetByID((datagrid.SelectedItem as DTO_NhanVien).MANV);
                if (member == null) return; 
                
                MANV.Text = member.MANV;
                TENNV.Text = member.TENNV;
                PHONE.Text = member.PHONE;
                EMAIL.Text = member.EMAIL;
                CM.Text = member.MACM;
                NOTE.Text = member.GHICHU;
                LVL.Text = member.LEVEL.ToString();
                DOB.Text = member.NGAYSINH;

            }
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DAL_NhanVien dal = new DAL_NhanVien();
            (bool, string) res = dal.DeleteByID(MANV.Text);
            if (res.Item1)
                Debug.Write(res.Item2);
            else Debug.Write(res.Item2);

            this.datagrid.ItemsSource = nvManager.GetAllData();

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            int level;
            level = int.TryParse(LVL.Text, out level) ? level : -1;
            DTO_NhanVien nhanvien = new DTO_NhanVien(MANV.Text, TENNV.Text, EMAIL.Text, PHONE.Text, DOB.Text, level, CM.Text, NOTE.Text);
            nvManager.SetData(nhanvien);
            this.datagrid.ItemsSource = nvManager.GetAllData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DangNhap dangnhap = new DangNhap();
            dangnhap.Show();
            this.Close();
        }
    }
}
