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
using System.Xml.Linq;

namespace GUI
{
    /// <summary>
    /// Interaction logic for ChuyenMonRule.xaml
    /// </summary>
    public partial class ChuyenMonRule : UserControl
    {
        BindingList<DTO_ChuyenMon> members = new BindingList<DTO_ChuyenMon>();
        public ChuyenMonRule()
        {
            InitializeComponent();
            members = BUS_StaticTables.Instance.GetAllDataCMBinding();
            showMember();
        }
        void showMember()
        {
            membersDataGrid.ItemsSource = members;
        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (membersDataGrid.SelectedItem is DTO_ChuyenMon selectedLSK)
            {
                macmText.Text = selectedLSK.MACM;
                tencmText.Text = selectedLSK.TENCM;
                shortText.Text = selectedLSK.INSHORT;
            }
        }

        private void Add_Btn_Click(object sender, RoutedEventArgs e)
        {
            macmText.Text = "";
            tencmText.Text = "";
            shortText.Text = "";
        }

        private void Del_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (macmText.Text == "") return;
            xoaSK(macmText.Text);
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
                    DTO_ChuyenMon? item = row.Item as DTO_ChuyenMon;
                    if (item != null)
                    {
                        xoaSK(item.MACM);

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

        void xoaSK(string macm)
        {
            MessageBoxResult resu = MessageBox.Show("Bạn đang deactive chuyên môn " + macm + ". Bạn không thể thêm nhân viên/ công việc thuộc chuyên môn này trong tương lai, thao tác này không thể quay lại.", "Warning", MessageBoxButton.OKCancel);
            if (resu == MessageBoxResult.OK)
            {

                (bool, string) res = BUS_StaticTables.Instance.DeleteCMByID(macm);
                if (res.Item1 == true)
                {
                    MessageBox.Show(res.Item2);
                    members = BUS_StaticTables.Instance.GetAllDataCMBinding();
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
                    DTO_ChuyenMon? item = row.Item as DTO_ChuyenMon;

                    if (item != null && item.ISDELETED != false)
                    {
                        button.Visibility = Visibility.Collapsed;
                        return;
                    }
                }

            }


        }

        private void Save_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (macmText.Text != "")
            {
                MessageBoxResult resu = MessageBox.Show("Bạn đang chỉnh sửa thông tin về " + macmText.Text + ".", "Warning", MessageBoxButton.OKCancel);
                if (resu == MessageBoxResult.OK)
                {
                    DTO_ChuyenMon newCM = new DTO_ChuyenMon(macmText.Text, tencmText.Text, shortText.Text);
                    (bool, string) res = BUS_StaticTables.Instance.EditCM(newCM);
                    if (res.Item1 == true)
                    {
                        MessageBox.Show(res.Item2);
                        members = BUS_StaticTables.Instance.GetAllDataCMBinding();
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
                DTO_ChuyenMon newCM = new DTO_ChuyenMon("", tencmText.Text, shortText.Text);
                (bool, string) res = BUS_StaticTables.Instance.AddCM(newCM);
                if (res.Item1 == true)
                {
                    MessageBox.Show(res.Item2);
                    members = BUS_StaticTables.Instance.GetAllDataCMBinding();
                    showMember();
                }
                else
                {
                    MessageBox.Show(res.Item2);
                }
            }
        }

    }
}
