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
        DTO_DuAn initializeDA = new DTO_DuAn();
        public AddAndUpdateProject(DTO_DuAn inputDA = null)
        {
            InitializeComponent();
            BindingDropDown();
            statText.Text = "On-going";
            if (inputDA != null)
            {
                wTitle.Text = "SỬA DU AN";
                ButtonAddNew.Visibility = Visibility.Hidden;
                ButtonUpdate.Visibility = Visibility.Visible;
                initializeDA = BUS_DuAn.Instance.GetByID(inputDA.MADA);
                if (initializeDA.STAT != "On-going")
                {
                    madaText.IsEnabled = false;
                    tendaText.IsEnabled = false;
                    TStartPicker.IsEnabled = false;
                    TEndPicker.IsEnabled = false;
                    ngansachText.IsEnabled = false;
                    lskText.IsEnabled = false;
                    ownerText.IsEnabled = false;
                }

                if (BUS_TaiKhoan.Instance.checkQH(LoginWindow.crnUser, "SuaTBDA") == false)
                {
                    if (BUS_TaiKhoan.Instance.checkQH(LoginWindow.crnUser, "SuaDA"))
                    {
                        ownerText.IsEnabled = false;
                    }
                }
                madaText.Text = initializeDA.MADA;
                tendaText.Text = initializeDA.TENDA;
                statText.Text = initializeDA.STAT;
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
            if (initializeDA.STAT != "On-going")
            {
                (bool, string) res1 = projectManager.SetStatus(initializeDA.MADA, statText.Text.ToString());
                if (res1.Item1 == true)
                {
                    MessageBox.Show("Sửa du an thành công!");
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(res1.Item2);
                }
                return;
            }    


            initializeDA.MADA = madaText.Text;
            initializeDA.MALSK = lskText.SelectedValue.ToString();
            //da.MAOWNER = ownerText.Text;
            initializeDA.MAOWNER = ownerText.SelectedValue!=null ? ownerText.SelectedValue.ToString():"";
            initializeDA.TENDA = tendaText.Text;
            initializeDA.STAT = statText.Text.ToString();
            initializeDA.NGANSACH = long.TryParse(ngansachText.Text, out long tempResult) ? tempResult : -1;
            initializeDA.TSTART = TStartPicker.Text;
            initializeDA.TEND = TEndPicker.Text;
            (bool, string) res = projectManager.EditProject(initializeDA);

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
