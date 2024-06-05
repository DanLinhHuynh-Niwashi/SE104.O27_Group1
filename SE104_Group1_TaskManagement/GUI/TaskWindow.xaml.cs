using BUS;
using DTO;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public static DTO_CongViec task = new DTO_CongViec();
        BUS_CongViec taskManager = new BUS_CongViec();
        BindingList<DTO_CongViec> members = new BindingList<DTO_CongViec>();
        Dictionary<string, DTO_ChuyenMon> cm = BUS_StaticTables.Instance.GetAllDataCM();
        Dictionary<string, DTO_NhanVien> nv = BUS_StaticTables.Instance.GetAllDataNV();

        string crnMADA;

        public TaskWindow(string mada)
        {
            InitializeComponent();
            crnMADA = mada;
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            MaDa.Text = mada;
            membersDataGrid.LoadingRow += MembersDataGrid_LoadingRow;
            cmText.ItemsSource = cm;
            cmText.DisplayMemberPath = "Value.TENCM";
            cmText.SelectedValuePath = "Value.MACM";
            nvText.ItemsSource = nv;
            nvText.DisplayMemberPath = "Value.MANV";
            nvText.SelectedValuePath = "Value.MANV";
            members = taskManager.GetByMaDA(crnMADA);
            showMember();
        }

        private void MembersDataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            var firstCol = membersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "C");
            var cmCol = membersDataGrid.Columns.First(c => c.Header.ToString() == "Mã chuyên môn");
            
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
                }
                if (cmCol != null)
                {
                    var chBx = cmCol.GetCellContent(row) as TextBlock;
                    DTO_ChuyenMon temp = new DTO_ChuyenMon();
                    cm.TryGetValue(cv.MACM, out temp);
                    chBx.Text = temp.TENCM;
                }
            };
        }

        void showMember()
        {
            membersDataGrid.ItemsSource = members;
        }

        private void membersDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var firstCol = membersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "C");
            var operationCol = membersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "Operations");
            foreach (var item in membersDataGrid.Items)
            {
                DTO_CongViec? da = item as DTO_CongViec;
                if (da != null)
                {
                    var chBx = firstCol.GetCellContent(item) as CheckBox;
                    if (chBx == null)
                    {
                        continue;
                    }
                    else
                    {
                        chBx.IsEnabled = false;
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

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            long ngsl = -1;
            long ngsh = -1;
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
                ngsl = long.TryParse(NganSachLTextBox.Text, out long tempLResult) ? tempLResult : -1L;
                ngsh = long.TryParse(NganSachHTextBox.Text, out long tempHResult) ? tempHResult : -1L;
            }
            if (TStartCheck.IsChecked == true)
            {
                filter.TSTART = TStartPicker.Text != null ? TStartPicker.Text.ToString() : "";
            }
            if (TEndCheck.IsChecked == true)
            {
                filter.TEND = TEndPicker.Text != null ? TEndPicker.Text.ToString() : "";
            }
            if (progressCheck.IsChecked == true)
            {
                filter.TIENDO = int.Parse(ProgressTextBox.Text != null ? ProgressTextBox.Text.ToString() : "");
            }
            members = taskManager.FindCV(filter, ngsl, ngsh);
            showMember();
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
                    DTO_CongViec? item = row.Item as DTO_CongViec;  
                    if (item != null)
                    {
                        AddAndUpdateTask updateDialog = new AddAndUpdateTask(MaDa.Text.ToString(), item);
                        bool? res = updateDialog.ShowDialog();
                        if (res != null && res == true)
                        {
                            members = taskManager.GetByMaDA(crnMADA);
                            showMember();
                        }
                    }

                    // Do something with the item...
                }
            }
        }

        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {

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
                            (bool, string) res = taskManager.DeleteByID(item);
                            if (res.Item1 == true)
                            {
                                MessageBox.Show(res.Item2);
                                members = taskManager.GetByMaDA(crnMADA);
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
            string mada = MaDa.Text;
            AddAndUpdateTask addDialog = new AddAndUpdateTask(mada, null);
            bool? res = addDialog.ShowDialog();
            if (res != null && res == true)
            {
                members = taskManager.GetByMaDA(crnMADA);
                membersDataGrid.ItemsSource = members;
            }
        }

        private void ButtonDelete_Loaded(object sender, RoutedEventArgs e)
        {
            
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
                            taskManager.DeleteByID(da);
                            members = taskManager.GetByMaDA(crnMADA);
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




    }
}
