﻿using BUS;
using DTO;
using DAL;
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
using static Azure.Core.HttpHeader;
using System.Data;
using System.ComponentModel;

namespace GUI
{
    /// <summary>
    /// Interaction logic for AddAndUpdateTask.xaml
    /// </summary>
    public partial class AddAndUpdateTask : Window
    {
        BUS_CongViec taskManager = new BUS_CongViec();
        private Dictionary<string, DTO_NhanVien> nv;
        private Dictionary<string, DTO_ChuyenMon> cm;
        private List<DTO_PhanCong> phanCongList;
        public AddAndUpdateTask(string mada, DTO_CongViec initializeCV = null)
        {
            InitializeComponent();

            manvTokenizer.TokenMatcher = text =>
            {
                if (text.EndsWith(";"))
                {
                    // Remove the ';'
                    return text.Substring(0, text.Length - 1).Trim().ToUpper();
                }

                return null;
            };

            phanCongList = new List<DTO_PhanCong>();
            BindingDropDown();
            if (initializeCV != null)
            {
                wTitle.Text = "SỬA CONG VIEC";
                ButtonAddNew.Visibility = Visibility.Hidden;
                ButtonUpdate.Visibility = Visibility.Visible;
                tiendoText.IsEnabled = true;
                dadungText.IsEnabled = true;
                macvText.Text = initializeCV.MACV;
                madaText.Text = initializeCV.MADA;
                tencvText.Text = initializeCV.TENCV;
                macmText.Text = initializeCV.MACM;
                TStartPicker.Text = initializeCV.TSTART;
                TEndPicker.Text = initializeCV.TEND;
                ngansachText.Text = initializeCV.NGANSACH.ToString();
                dadungText.Text = initializeCV.DADUNG.ToString();
                tiendoText.Text = initializeCV.TIENDO.ToString();
                ycdkText.Text = initializeCV.YCDK;
                dkText.Text = initializeCV.TEPDK;

                //ham tai maNV
                BindingList<DTO_PhanCong> listPC = taskManager.GetPhanCong(initializeCV);
                foreach (var pc in listPC)
                {
                    addNewToken(pc.MANV);
                }

            }
            else madaText.Text = mada;


        }

        void BindingDropDown()
        {
            nv = BUS_StaticTables.Instance.GetAllDataNV();

            manvText.ItemsSource = nv;
            manvText.DisplayMemberPath = "Value.MANV";
            manvText.SelectedValuePath = "Value.MANV";

            cm = BUS_StaticTables.Instance.GetAllDataCM();

            macmText.ItemsSource = cm;
            macmText.DisplayMemberPath = "Value.TENCM";
            macmText.SelectedValuePath = "Value.MACM";

            macmText.SelectionChanged += MacmText_SelectionChanged;


        }

        private void MacmText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMacm = macmText.SelectedValue as string;
            if (selectedMacm != null)
            {
                var filteredNV = nv.Where(kv => kv.Value.MACM == selectedMacm).ToDictionary(kv => kv.Key, kv => kv.Value);
                manvText.ItemsSource = filteredNV;
            }
        }

        private void ButtonAddNew_Click(object sender, RoutedEventArgs e)
        {
            DTO_CongViec newCV = new DTO_CongViec();
            newCV.MADA = madaText.Text;
            //newCV.MANV = manvText.Text != null ? manvText.Text.ToString() : "";
            newCV.MACV = macvText.Text;
            newCV.TENCV = tencvText.Text;
            newCV.MACM = macmText.SelectedValue != null ? macmText.SelectedValue.ToString() : "";
            //List<string> maNVList = Tokenizer.getAllDataPresented();
            newCV.TSTART = TStartPicker.Text;
            newCV.TEND = TEndPicker.Text;
            newCV.NGANSACH = long.TryParse(ngansachText.Text, out long tempResultNS) ? tempResultNS : 0;
            newCV.DADUNG = long.TryParse(ngansachText.Text, out long tempResultDD) ? tempResultDD : 0;
            newCV.TIENDO = 0;
            newCV.YCDK = ycdkText.Text;
            newCV.TEPDK = dkText.Text;
            (bool, string) res = taskManager.AddData(newCV);


            if (res.Item1 == true)
            {
                List<string> manvList = manvTokenizer.getAllDataPresented();
                for (int i = 0; i < manvList.Count; i++)
                {
                    taskManager.PhanCong(res.Item2, manvList[i]);
                }
                MessageBox.Show("Thêm task thành công!");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(res.Item2);
            }
            //if (macvText.Text != null && manvTokenizer.SelectedItems.Count > 0)
            //{
            //    AddNewPC();
            //    phanCongList.Clear();
            //    manvTokenizer.SelectedItems.Clear();
            //}
        }



        //private void AddNewPC()
        //{
        //    var selectedMACV = macvText.Text;
        //    foreach (var selectedItem in manvTokenizer.SelectedItems)
        //    {
        //        var selectedMANV = ((KeyValuePair<string, string>)selectedItem).Key;
        //        var phanCong = new DTO_PhanCong
        //        {
        //            MACV = selectedMACV,
        //            MANV = selectedMANV
        //        };

        //        phanCongList.Add(phanCong);
        //    }
        //}

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            DTO_CongViec cv = new DTO_CongViec();
            cv.MADA = madaText.Text;
            cv.MACV = macvText.Text;
            cv.TENCV = tencvText.Text;
            cv.MACM = macmText.Text;
            cv.TSTART = TStartPicker.Text;
            cv.TEND = TEndPicker.Text;
            cv.NGANSACH = long.TryParse(ngansachText.Text, out long tempResultNS) ? tempResultNS : -1;
            dadungText.IsEnabled = true;
            tiendoText.IsEnabled = true;
            cv.DADUNG = long.TryParse(dadungText.Text, out long tempResultDD) ? tempResultDD : -1;
            cv.TIENDO = int.Parse(tiendoText.Text.ToString());
            cv.YCDK = ycdkText.Text;
            cv.TEPDK = dkText.Text;
            (bool, string) res = taskManager.EditTask(cv);

            if (res.Item1 == true)
            {
                taskManager.DeletePCByTask(cv);

                List<string> manvList = manvTokenizer.getAllDataPresented();
                for (int i = 0; i < manvList.Count; i++)
                {
                    taskManager.PhanCong(cv.MACV, manvList[i]);
                }

                MessageBox.Show("Sửa task thành công!");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(res.Item2);
            }
            //if (macvText.Text != null && manvTokenizer.SelectedItems.Count > 0)
            //{
            //    EditPC();
            //}
        }

        //private void EditPC()
        //{
        //    var selectedMACV = macvText.Text;

        //    // Xóa các mục phanCong cũ với MACV đã chọn
        //    phanCongList.RemoveAll(pc => pc.MACV == selectedMACV);

        //    // Thêm các mục phanCong mới
        //    foreach (var selectedItem in manvTokenizer.SelectedItems)
        //    {
        //        var selectedMANV = ((KeyValuePair<string, string>)selectedItem).Key;
        //        var phanCong = new DTO_PhanCong
        //        {
        //            MACV = selectedMACV,
        //            MANV = selectedMANV
        //        };

        //        phanCongList.Add(phanCong);
        //    }
        //}

        private void manvText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addNewToken(manvText.SelectedValue.ToString());
        }
        void addNewToken(string content)
        {
            manvTokenizer.AppendText(content);
            manvTokenizer.AppendText(";");

            manvTokenizer.AddToken(content + ";");
        }
    }

}
