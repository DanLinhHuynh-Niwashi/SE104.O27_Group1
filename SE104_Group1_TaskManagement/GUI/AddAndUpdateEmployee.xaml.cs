﻿using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for AddAndUpdateEmployee.xaml
    /// </summary>
    public partial class AddAndUpdateEmployee : Window
    {
        BUS_NhanVien nvManager = new BUS_NhanVien();
        BUS_TaiKhoan tkManager = new BUS_TaiKhoan();
        
        public AddAndUpdateEmployee(DTO_NhanVien initializeNV = null)
        {
            
            InitializeComponent();

            BindingDropDown();
            if (initializeNV != null)
            {
                wTitle.Text = "SỬA NHÂN VIÊN";
                ButtonAddNew.Visibility = Visibility.Hidden;
                ButtonUpdate.Visibility = Visibility.Visible;
                manvText.Text = initializeNV.MANV;
                tennvText.Text = initializeNV.TENNV;
                cmText.SelectedValue = initializeNV.MACM;
                levelText.Text = initializeNV.LEVEL.ToString();
                dobText.Text = initializeNV.NGAYSINH;
                emailText.Text = initializeNV.EMAIL;
                phoneText.Text = initializeNV.PHONE;
                noteText.Text = initializeNV.GHICHU;
                qhText.SelectedValue = nvManager.GetQuyenHan(initializeNV).MAQH;

                if (initializeNV.MANV == LoginWindow.crnUser.MANV)
                    qhText.IsEnabled = false;
                
            }    
        }
        void BindingDropDown()
        {
            Dictionary<string, DTO_QuyenHan> qh = BUS_StaticTables.Instance.GetAllDataQH();
 
            qhText.ItemsSource = qh;
            qhText.DisplayMemberPath = "Value.TENQH";
            qhText.SelectedValuePath = "Value.MAQH";

            Dictionary<string, DTO_ChuyenMon> cm = BUS_StaticTables.Instance.GetAllDataCM();

            cmText.ItemsSource = cm;
            cmText.DisplayMemberPath = "Value.TENCM";
            cmText.SelectedValuePath = "Value.MACM";
        }    
        private void ButtonAddNew_Click(object sender, RoutedEventArgs e)
        {
            DTO_NhanVien newNV = new DTO_NhanVien();
            newNV.TENNV = tennvText.Text;
            newNV.MACM = cmText.SelectedValue != null ? cmText.SelectedValue.ToString() : "";
            int level = -1;
            int.TryParse(levelText.Text, out level);
            newNV.LEVEL=level;
            newNV.NGAYSINH = dobText.Text;
            newNV.EMAIL = emailText.Text;
            newNV.PHONE = phoneText.Text;
            newNV.GHICHU = noteText.Text;
            (bool, string) res = nvManager.AddData(newNV);
            
            if (res.Item1 == true)
            {
                DTO_TaiKhoan tk = new DTO_TaiKhoan(qhText.SelectedValue.ToString(), newNV.EMAIL, newNV.PHONE, res.Item2);
                tkManager.TaoMoiTaiKhoan(tk);
                MessageBox.Show("Thêm nhân viên thành công!");
                this.DialogResult = true;
                this.Close();
            }    
            else
            {
                MessageBox.Show(res.Item2);
            }    
            
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            DTO_NhanVien nv = new DTO_NhanVien();
            nv.MANV = manvText.Text;
            nv.TENNV = tennvText.Text;
            nv.MACM = cmText.SelectedValue.ToString();
            int level = -1;
            int.TryParse(levelText.Text, out level);
            nv.LEVEL = level;
            nv.NGAYSINH = dobText.Text;
            nv.EMAIL = emailText.Text;
            nv.PHONE = phoneText.Text;
            nv.GHICHU = noteText.Text;
            (bool, string) res = nvManager.SuaNhanVien(nv);
            string result = nvManager.DoiQuyenHan(LoginWindow.crnUser, nv, qhText.SelectedValue.ToString());
            if (res.Item1 == true && result == "")
            {
                MessageBox.Show("Sửa nhân viên thành công!");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(res.Item2 + "\n" + result);
            }
        }
    }
}
