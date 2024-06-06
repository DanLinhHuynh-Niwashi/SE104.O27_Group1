using BUS;
using DTO;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for ChuyenMonRule.xaml
    /// </summary>
    public partial class LoaiSKRule : UserControl
    {
        BindingList<DTO_LoaiSK> members = new BindingList<DTO_LoaiSK>();
        public LoaiSKRule()
        {
            InitializeComponent();
            members = BUS_StaticTables.Instance.GetAllDataLSKBinding();
            showMember();
        }
        void showMember()
        {
            membersDataGrid.ItemsSource = members;
        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (membersDataGrid.SelectedItem is DTO_LoaiSK selectedLSK)
            {
                malskText.Text = selectedLSK.MALSK;
                tenlskText.Text = selectedLSK.TENLSK;
                shortText.Text = selectedLSK.INSHORT;
                minText.Text = selectedLSK.MIN.ToString();
                maxText.Text = selectedLSK.MAX.ToString();
            }
        }

        private void Add_Btn_Click(object sender, RoutedEventArgs e)
        {
            malskText.Text = "";
            tenlskText.Text = "";
            shortText.Text = "";
            minText.Text = "";
            maxText.Text = "";
        }

        private void Del_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (malskText.Text == "") return;
            xoaSK(malskText.Text);
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;
            if (button != null)
            {
                // Find the DataGridRow that contains the clicked button
                DataGridRow row = FindVisualParent<DataGridRow>(button);
                if (row != null)
                {
                    // Access the data item behind the row
                    DTO_LoaiSK? item = row.Item as DTO_LoaiSK;
                    if (item != null)
                    {
                        xoaSK(item.MALSK);

                    }
                }

                // Do something with the item...
            }
        }

        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;
            else
                return FindVisualParent<T>(parentObject);
        }

        void xoaSK(string malsk)
        {
            MessageBoxResult resu = MessageBox.Show("Bạn đang xóa loại sự kiện " + malsk + ". Bạn không thể thêm dự án thuộc loại sự kiện này trong tương lai, thao tác này không thể quay lại.", "Warning", MessageBoxButton.OKCancel);
            if (resu == MessageBoxResult.OK)
            {
                
                (bool, string) res = BUS_StaticTables.Instance.DeleteLSKByID(malsk);
                if (res.Item1 == true)
                {
                    MessageBox.Show(res.Item2);
                    members = BUS_StaticTables.Instance.GetAllDataLSKBinding();
                    showMember();
                }
                else
                {
                    MessageBox.Show(res.Item2);
                }
            }
        }

        private void Save_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (malskText.Text != "")
            {
                MessageBoxResult resu = MessageBox.Show("Bạn đang chỉnh sửa thông tin về " + malskText.Text + ".", "Warning", MessageBoxButton.OKCancel);
                if (resu == MessageBoxResult.OK)
                {
                    long min, max;
                    if (!(long.TryParse(minText.Text, out min) && min >= 0))
                    {
                        MessageBox.Show("Ngân sách phải là một số nguyên lớn hơn hoặc bằng 0");
                        return;
                    }
                    if (!(long.TryParse(maxText.Text, out max) && max >= 0))
                    {
                        MessageBox.Show("Ngân sách phải là một số nguyên lớn hơn hoặc bằng 0");
                        return;
                    }
                    DTO_LoaiSK newLSK = new DTO_LoaiSK(malskText.Text, tenlskText.Text, min, max, shortText.Text);
                    (bool, string) res = BUS_StaticTables.Instance.EditLSK(newLSK);
                    if (res.Item1 == true)
                    {
                        MessageBox.Show(res.Item2);
                        members = BUS_StaticTables.Instance.GetAllDataLSKBinding();
                        showMember();
                    }
                    else
                    {
                        MessageBox.Show(res.Item2);
                    }
                }
            }
            else
            {
                long min, max;
                if (!(long.TryParse(minText.Text, out min) && min >= 0))
                {
                    MessageBox.Show("Ngân sách phải là một số nguyên lớn hơn hoặc bằng 0");
                    return;
                }
                if (!(long.TryParse(maxText.Text, out max) && max >= 0))
                {
                    MessageBox.Show("Ngân sách phải là một số nguyên lớn hơn hoặc bằng 0");
                    return;
                }
                DTO_LoaiSK newLSK = new DTO_LoaiSK("", tenlskText.Text, min, max, shortText.Text);
                (bool, string) res = BUS_StaticTables.Instance.AddLSK(newLSK);
                if (res.Item1 == true)
                {
                    MessageBox.Show(res.Item2);
                    members = BUS_StaticTables.Instance.GetAllDataLSKBinding();
                    showMember();
                }
                else
                {
                    MessageBox.Show(res.Item2);
                }
            }    
        }

        private void DeleteButton_Loaded(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                DataGridRow row = FindVisualParent<DataGridRow>(button);
                if (row != null)
                {
                    // Access the data item behind the row
                    DTO_LoaiSK? item = row.Item as DTO_LoaiSK;

                    if (item != null && item.ISDELETED != false)
                    {
                        button.Visibility = Visibility.Collapsed;
                        return;
                    }
                }

            }


        }
    }
}
