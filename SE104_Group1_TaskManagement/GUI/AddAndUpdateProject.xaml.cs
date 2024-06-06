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
using BUS;
using DTO;
using DAL;
using System.Globalization;

namespace GUI
{
    /// <summary>
    /// Interaction logic for AddAndUpdateProject.xaml
    /// </summary>
    public partial class AddAndUpdateProject : Window
    {
        BUS_DuAn projectManager = new BUS_DuAn();
        public AddAndUpdateProject(DTO_DuAn initializeDA = null)
        {
            InitializeComponent();
            BindingDropDown();
            if (initializeDA != null)
            {
                wTitle.Text = "SỬA DU AN";
                ButtonAddNew.Visibility = Visibility.Hidden;
                ButtonUpdate.Visibility = Visibility.Visible;
                madaText.Text = initializeDA.MADA;
                tendaText.Text = initializeDA.TENDA;
                statText.SelectedValue = initializeDA.STAT;
                TStartPicker.Text = initializeDA.TSTART;
                TEndPicker.Text = initializeDA.TEND;
                ngansachText.Text = initializeDA.NGANSACH.ToString();
                lskText.SelectedValue = initializeDA.MALSK;
                ownerText.SelectedValue = initializeDA.MAOWNER;
            }
        }

        void BindingDropDown()
        {
            Dictionary<string, DTO_NhanVien> nv = BUS_StaticTables.Instance.GetAllDataNV();

            ownerText.ItemsSource = nv;
            ownerText.DisplayMemberPath = "Value.MANV";
            ownerText.SelectedValuePath = "Value.MANV";

            Dictionary<string, DTO_LoaiSK> lsk = BUS_StaticTables.Instance.GetAllDataLSK();

            lskText.ItemsSource = lsk;
            lskText.DisplayMemberPath = "Value.TENLSK";
            lskText.SelectedValuePath = "Value.MALSK";
        }

        private void ButtonAddNew_Click(object sender, RoutedEventArgs e)
        {
            DTO_DuAn newDA = new DTO_DuAn();
            newDA.TENDA = tendaText.Text;
            newDA.STAT = statText.Text != null ? statText.Text.ToString() : "";
            newDA.MADA = madaText.Text;
            newDA.TSTART = TStartPicker.Text;
            newDA.TEND = TEndPicker.Text;
            newDA.NGANSACH = long.TryParse(ngansachText.Text, out long tempResult) ? tempResult : -1;
            newDA.MALSK = lskText.SelectedValue != null ? lskText.SelectedValue.ToString() : "";
            newDA.MAOWNER = ownerText.Text;
            newDA.MAOWNER = ownerText.SelectedValue != null ? ownerText.SelectedValue.ToString() : "";
            (bool, string) res = projectManager.AddData(newDA);

            

            if (res.Item1 == true)
            {
                MessageBox.Show("Thêm du an thành công!");
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
            DTO_DuAn da = new DTO_DuAn();
            da.MADA = madaText.Text;
            da.MALSK = lskText.SelectedValue.ToString();
            //da.MAOWNER = ownerText.Text;
            da.MAOWNER = ownerText.SelectedValue!=null ? ownerText.SelectedValue.ToString():"";
            da.TENDA = tendaText.Text;
            da.STAT = statText.Text.ToString();
            da.NGANSACH = long.TryParse(ngansachText.Text, out long tempResult) ? tempResult : -1;
            da.TSTART = TStartPicker.Text;
            da.TEND = TEndPicker.Text;
            (bool, string) res = projectManager.EditProject(da);

            if (res.Item1 == true)
            {
                MessageBox.Show("Sửa du an thành công!");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(res.Item2);
            }
        }
    }
}
