using DTO;
using BUS;
using DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;
using System.Xml;
using System.Windows.Controls.Primitives;
using System.Globalization;

namespace GUI
{
    /// <summary>
    /// Interaction logic for ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow : UserControl
    {
        BindingList<DTO_DuAn> members = new BindingList<DTO_DuAn>();

        Dictionary<string, DTO_NhanVien> nv = BUS_StaticTables.Instance.GetAllRawDataNV();
        Dictionary<string, DTO_LoaiSK> lsk = BUS_StaticTables.Instance.GetAllRawDataLSK();




        public ProjectWindow()
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            membersDataGrid.LoadingRow += MembersDataGrid_LoadingRow;
            ownerText.ItemsSource = nv;
            ownerText.DisplayMemberPath = "Value.TENNV";
            ownerText.SelectedValuePath = "Value.MANV";
            lskText.ItemsSource = lsk;
            lskText.DisplayMemberPath = "Value.TENLSK";
            lskText.SelectedValuePath = "Value.MALSK";
            var converter = new BrushConverter();
            members = BUS_DuAn.Instance.GetAllData();

            if (BUS_TaiKhoan.Instance.checkQH(LoginWindow.crnUser, "ThemDA") == false)
            {
                Add_Btn.Visibility = Visibility.Collapsed;
            }

            showMember();
        }
        

        private void MembersDataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            var firstCol = membersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "C");
            var lskCol = membersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "Loại sự kiện");
            var ownerCol = membersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "Quản lý");

            e.Row.Loaded += (s, args) =>
            {
                var row = (DataGridRow)s;
                var item = row.Item;

                DTO_DuAn? da = item as DTO_DuAn;
                if (da != null)
                {
                    if (firstCol != null)
                    {
                        var chBx = firstCol.GetCellContent(row) as CheckBox;
                        if (chBx != null)
                        {
                            chBx.Visibility = Visibility.Visible;
                        }
                    }
                }
                if (lskCol != null)
                {
                    var chBx = lskCol.GetCellContent(row) as TextBlock;
                    DTO_LoaiSK temp = new DTO_LoaiSK();
                    if(temp != null)
                    {
                        lsk.TryGetValue(da.MALSK, out temp);
                        chBx.Text = temp.TENLSK;
                    }    
                }

                if (ownerCol != null)
                {
                    var chBx = ownerCol.GetCellContent(row) as TextBlock;
                    DTO_NhanVien temp = new DTO_NhanVien();
                    nv.TryGetValue(da.MAOWNER, out temp);
                    if (temp != null)
                    {
                        string tenNV = temp.TENNV;
                        chBx.Text = tenNV;
                    }
                }
            };
        }


        void showMember()
        {
            membersDataGrid.ItemsSource = members;
        }

        private void EditButton_Loaded(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (BUS_TaiKhoan.Instance.checkQH(LoginWindow.crnUser, "SuaTBDA") == false)
                {
                    if (BUS_TaiKhoan.Instance.checkQH(LoginWindow.crnUser, "SuaDA") == false)
                    {
                        button.Visibility = Visibility.Collapsed;
                        return;
                    }
                    else
                    {
                        DataGridRow row = FindVisualParent<DataGridRow>(button);
                        if (row != null)
                        {
                            // Access the data item behind the row
                            DTO_DuAn? item = row.Item as DTO_DuAn;

                            if (item != null && item.MAOWNER != LoginWindow.crnUser.MANV)
                            {
                                button.Visibility = Visibility.Collapsed;
                                return;
                            }
                        }
                        
                    }    
                }
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

        private void ButtonEdit_Click(Object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
            if (button != null)
            {
                // Find the DataGridRow that contains the clicked button
                DataGridRow row = FindVisualParent<DataGridRow>(button);
                if (row != null)
                {
                    // Access the data item behind the row
                    DTO_DuAn? item = row.Item as DTO_DuAn;
                    if (item != null)
                    {
                        AddAndUpdateProject updateDialog = new AddAndUpdateProject(item);
                        bool? res = updateDialog.ShowDialog();
                        if (res != null && res == true)
                        {
                            members = BUS_DuAn.Instance.GetAllData();
                            showMember();
                        }
                    }

                    // Do something with the item...
                }
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddAndUpdateProject addDialog = new AddAndUpdateProject();
            bool? res = addDialog.ShowDialog();
            if (res != null && res == true)
            {
                members = BUS_DuAn.Instance.GetAllData();
                membersDataGrid.ItemsSource = members;
            }
        }

        private void ButtonView_Click(object sender, RoutedEventArgs e)
        {
            if (membersDataGrid.SelectedItem is DTO_DuAn selectedDA)
            {
                UserControl view = new TaskWindow(selectedDA);

                if (view != null)
                {
                    ProjectContent.Visibility = Visibility.Collapsed;
                    MainContent.Content = view;
                }
            }
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            long ngsl = -1;
            long ngsh = -1;
            DTO_DuAn filter = new DTO_DuAn();
            filter.MADA = searchText.Text != null ? searchText.Text.ToString() : "";
            filter.TENDA = searchText.Text != null ? searchText.Text.ToString() : "";

            if (malskCheck.IsChecked == true)
            {
                filter.MALSK = lskText.SelectedValue != null ? lskText.SelectedValue.ToString() : "";
            }

            if (maownerCheck.IsChecked == true)
            {
                filter.MAOWNER = ownerText.SelectedValue != null ? ownerText.SelectedValue.ToString() : "";
            }

            if (statCheck.IsChecked == true)
            {
                ComboBoxItem sel = statText.SelectedItem as ComboBoxItem;
                filter.STAT = sel != null ? sel.Content.ToString() : "";
            }
            if (ngansachCheck.IsChecked == true)
            {
                ngsl = long.TryParse(NganSachLTextBox.Text, out long tempLResult) ? tempLResult : -1L;
                ngsh = long.TryParse(NganSachHTextBox.Text, out long tempHResult) ? tempHResult : -1L;
            }
            if (TStartCheck.IsChecked == true)
            {
                filter.TSTART = TStartPicker.SelectedDate != null ? TStartPicker.SelectedDate : null;
            }
            if (TEndCheck.IsChecked == true)
            {
                filter.TEND = TEndPicker.SelectedDate != null ? TEndPicker.SelectedDate : null;
            }
            members = BUS_DuAn.Instance.FindDA(filter, ngsl, ngsh);
            showMember();
        }

    }
}
