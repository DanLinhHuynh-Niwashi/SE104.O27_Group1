using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BUS;
using DTO;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MimeKit;

namespace GUI
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : UserControl
    {
        public static DTO_CongViec task = new DTO_CongViec();
        BindingList<DTO_CongViec> members = new BindingList<DTO_CongViec>();
        Dictionary<string, DTO_ChuyenMon> cm = BUS_StaticTables.Instance.GetAllDataCM();
        Dictionary<string, DTO_NhanVien> nv = BUS_StaticTables.Instance.GetAllDataNV();
        CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);

        string crnMADA;

        public TaskWindow(DTO_DuAn da)
        {
            InitializeComponent();
            ci.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            crnMADA = da.MADA;
            MaDa.Text = da.TENDA;
            membersDataGrid.LoadingRow += MembersDataGrid_LoadingRow;
            cmText.ItemsSource = cm;
            cmText.DisplayMemberPath = "Value.TENCM";
            cmText.SelectedValuePath = "Value.MACM";
            nvText.ItemsSource = nv;
            nvText.DisplayMemberPath = "Value.TENNV";
            nvText.SelectedValuePath = "Value.MANV";
            members = BUS_CongViec.Instance.GetByMaDA(crnMADA);

            if (BUS_TaiKhoan.Instance.checkQH(LoginWindow.crnUser, "ThemCV") == false)
            {
                Add_Btn.Visibility = Visibility.Collapsed;
            }

            if (BUS_TaiKhoan.Instance.checkQH(LoginWindow.crnUser, "XoaCV") == false)
            {
                Del_Btn.Visibility = Visibility.Collapsed;
            }
            showMember();
        }

        void showMember()
        {
            membersDataGrid.ItemsSource = members;
        }

        private void MembersDataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            var firstCol = membersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "C");
            var cmCol = membersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "Chuyên môn");
            var pcCol = membersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "Phân công");
            e.Row.Loaded += (s, args) =>
            {
                var row = (DataGridRow)s;
                var item = row.Item;

                DTO_CongViec? cv = item as DTO_CongViec;
                if (cv != null)
                {
                    if (firstCol != null)
                    {
                        var chBx = firstCol.GetCellContent(row) as CheckBox;
                        if (chBx != null)
                        {
                            chBx.Visibility = Visibility.Visible;
                        }
                    }

                    if (cmCol != null)
                    {
                        var chBx = cmCol.GetCellContent(row) as TextBlock;
                        DTO_ChuyenMon temp = new DTO_ChuyenMon();
                        if (temp != null)
                        {
                            cm.TryGetValue(cv.MACM, out temp);
                            chBx.Text = temp.TENCM;
                        }
                    }

                    if (pcCol != null)
                    {
                        var chBx = pcCol.GetCellContent(row) as TextBlock;
                        BindingList<DTO_PhanCong> strings = BUS_CongViec.Instance.GetPhanCong(cv);
                        string pcString = "";
                        foreach (var str in strings)
                        {
                            pcString += str.MANV + "\n";
                        }
                        chBx.Text = pcString;
                    }
                }

                
            };
        
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

        private void Sel_CheckBox_DataContextChanged(object sender, RoutedEventArgs e)
        {
            var chkSelectAll = sender as CheckBox;
            var firstCol = membersDataGrid.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
            if (chkSelectAll == null || firstCol == null || membersDataGrid?.Items == null)
            {
                return;
            }
            foreach (var item in membersDataGrid.Items)
            {
                var chBx = firstCol.GetCellContent(item) as CheckBox;
                if (chBx == null || chBx.Visibility != Visibility.Visible)
                {
                    continue;
                }
                chBx.IsChecked = chkSelectAll.IsChecked;
            }
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            UserControl view = new ProjectWindow();

            if (view != null)
            {
                TaskContent.Visibility = Visibility.Collapsed;
                MainContent.Content = view;
            }
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            long ngsl = 0;
            long ngsh = 0;
            DTO_CongViec filter = new DTO_CongViec();
            filter.MADA = crnMADA;
            filter.MACV = searchText.Text != null ? searchText.Text.ToString() : "";
            filter.TENCV = searchText.Text != null ? searchText.Text.ToString() : "";

            if (cmCheck.IsChecked == true)
            {
                filter.MACM = cmText.SelectedValue != null ? cmText.SelectedValue.ToString() : "";
            }

            if (ngansachCheck.IsChecked == true)
            {
                ngsl = long.TryParse(NganSachLTextBox.Text, out long tempLResult) ? tempLResult : 0L;
                ngsh = long.TryParse(NganSachHTextBox.Text, out long tempHResult) ? tempHResult : 0L;
            }

            if (TStartCheck.IsChecked == true)
            {
                filter.TSTART = TStartPicker.SelectedDate != null ? TStartPicker.SelectedDate : null;
            }
            if (TEndCheck.IsChecked == true)
            {
                filter.TEND = TEndPicker.SelectedDate != null ? TEndPicker.SelectedDate : null;
            }
            if (progressCheck.IsChecked == true)
            {
                filter.TIENDO = int.TryParse(ProgressTextBox.Text, out int tempResult) ? tempResult : 0;
            }
            members = BUS_CongViec.Instance.FindCV(filter, ngsl, ngsh);
            showMember();
        }

        private void ButtonEdit_Click(Object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                // Find the DataGridRow that contains the clicked button
                DataGridRow row = FindVisualParent<DataGridRow>(button);
                if (row != null)
                {
                    // Access the data item behind the row
                    DTO_CongViec? item = row.Item as DTO_CongViec;
                    if (item != null)
                    {
                        AddAndUpdateTask updateDialog = new AddAndUpdateTask(crnMADA, item);
                        bool? res = updateDialog.ShowDialog();
                        if (res != null && res == true)
                        {
                            members = BUS_CongViec.Instance.GetByMaDA(crnMADA);
                            showMember();
                        }
                    }

                    // Do something with the item...
                }
            }
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
                    DTO_CongViec? item = row.Item as DTO_CongViec;
                    if (item != null)
                    {
                        MessageBoxResult resu = MessageBox.Show("Bạn đang xóa task " + item.MACV + ", thao tác này không thể quay lại.", "Warning", MessageBoxButton.OKCancel);
                        if (resu == MessageBoxResult.OK)
                        {
                            (bool, string) res = BUS_CongViec.Instance.DeleteByID(item);
                            if (res.Item1 == true)
                            {
                                MessageBox.Show(res.Item2);
                                members = BUS_CongViec.Instance.GetByMaDA(crnMADA);
                                showMember();
                            }
                            else
                            {
                                MessageBox.Show(res.Item2);
                            }
                        }
                    }
                }

                // Do something with the item...
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddAndUpdateTask addDialog = new AddAndUpdateTask(crnMADA, null);
            bool? res = addDialog.ShowDialog();
            if (res != null && res == true)
            {
                members = BUS_CongViec.Instance.GetByMaDA(crnMADA);
                membersDataGrid.ItemsSource = members;
            }
        }

        private void ButtonDelete_Loaded(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                if (BUS_TaiKhoan.Instance.checkQH(LoginWindow.crnUser, "XoaCV") == false)
                {
                    btn.Visibility = Visibility.Collapsed;
                }
            }    
            
        }
        private void ButtonEdit_Loaded(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button != null)
            {
                if (BUS_TaiKhoan.Instance.checkQH(LoginWindow.crnUser, "SuaCV") == false)
                {
                    if (BUS_TaiKhoan.Instance.checkQH(LoginWindow.crnUser, "CapNhatCV"))
                    {
                        // Find the DataGridRow that contains the clicked button
                        DataGridRow row = FindVisualParent<DataGridRow>(button);
                        if (row != null)
                        {
                            // Access the data item behind the row
                            DTO_CongViec? item = row.Item as DTO_CongViec;
                            if (item != null)
                            {
                                foreach (DTO_PhanCong pc in BUS_CongViec.Instance.GetPhanCong(item))
                                    if (LoginWindow.crnUser.MANV == pc.MANV) return;
                            }      
                            
                        }

                    }
                    button.Visibility = Visibility.Collapsed;

                }

            }



            // Do something with the item...


        }
        private void ButtonDelete_All_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Bạn đang xóa tất cả task đã chọn, thao tác này không thể quay lại.", "Warning", MessageBoxButton.OKCancel);
            if (res == MessageBoxResult.OK)
            {
                var firstCol = membersDataGrid.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
                if (firstCol == null || membersDataGrid?.Items == null)
                {
                    return;
                }
                foreach (var item in membersDataGrid.Items)
                {
                    var chBx = firstCol.GetCellContent(item) as CheckBox;
                    if (chBx == null)
                    {
                        continue;
                    }
                    if (chBx.IsChecked == true)
                    {
                        DTO_CongViec? da = item as DTO_CongViec;
                        if (da != null)
                        {
                            BUS_CongViec.Instance.DeleteByID(da);
                            members = BUS_CongViec.Instance.GetByMaDA(crnMADA);
                            showMember();
                        }
                    }
                }

            }
        }

        private void MacmText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMacm = cmText.SelectedValue as string;
            if (selectedMacm != null)
            {
                var filteredNV = nv.Where(kv => kv.Value.MACM == selectedMacm).ToDictionary(kv => kv.Key, kv => kv.Value);
                nvText.ItemsSource = filteredNV;
            }
        }

        private void Gantt_Btn_Click(object sender, RoutedEventArgs e)
        {
            GanttWindow window = new GanttWindow(members);
            window.ShowDialog();
        }
    }
}



