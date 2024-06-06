using BUS;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace GUI
{
    /// <summary>
    /// Interaction logic for EmployeesWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : UserControl
    {
        public static DTO_NhanVien crnUser = new DTO_NhanVien();
        BUS_NhanVien nvManager = new BUS_NhanVien();
        BUS_TaiKhoan tkManager = new BUS_TaiKhoan();
        BindingList<DTO_NhanVien> members = new BindingList<DTO_NhanVien>();
        Dictionary<string, DTO_ChuyenMon> cm = BUS_StaticTables.Instance.GetAllDataCM();
        public EmployeeWindow()
        {

            InitializeComponent();
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            //setUser();
            membersDataGrid.LoadingRow += MembersDataGrid_LoadingRow;
            cmText.ItemsSource = cm;
            cmText.DisplayMemberPath = "Value.TENCM";
            cmText.SelectedValuePath = "Value.MACM";
            this.WindowState = WindowState.Maximized;

            setUser();
            if (tkManager.checkQH(LoginWindow.crnUser, "ThemNV") == false)
            {
                Add_Btn.Visibility = Visibility.Collapsed;
            }
            if (tkManager.checkQH(LoginWindow.crnUser, "XoaNV") == false)
            {
                Del_Btn.Visibility = Visibility.Collapsed;
            }
            var converter = new BrushConverter();
            members = nvManager.GetAllData();
            showMember();
        }

        //void setUser()
        //{
        //    crnUser = nvManager.GetByID(LoginWindow.crnUser.MANV);
        //}

        private void MembersDataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            var firstCol = membersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "C");
            var cmCol = membersDataGrid.Columns.First(c => c.Header.ToString() == "Chuyên môn");
            e.Row.Loaded += (s, args) =>
            {
                var row = (DataGridRow)s;
                var item = row.Item;

                DTO_NhanVien? nv = item as DTO_NhanVien;
                if (nv != null && nv.MANV == LoginWindow.crnUser.MANV)
                {
                    if (firstCol != null)
                    {
                        var chBx = firstCol.GetCellContent(row) as CheckBox;
                        if (chBx != null)
                        {
                            chBx.Visibility = Visibility.Collapsed;
                        }
                    }
                }

                if (cmCol != null)
                {
                    var chBx = cmCol.GetCellContent(row) as TextBlock;
                    DTO_ChuyenMon temp = new DTO_ChuyenMon();
                    cm.TryGetValue(nv.MACM, out temp);
                    chBx.Text = temp.TENCM;

                }
            };
        }


        void showMember()
        {
            membersDataGrid.ItemsSource = members;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            AddAndUpdateEmployee addDialog = new AddAndUpdateEmployee();
            bool? res = addDialog.ShowDialog();
            if (res != null && res == true)
            {
                members = nvManager.GetAllData();
                membersDataGrid.ItemsSource = members;
            }
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            DTO_NhanVien filter = new DTO_NhanVien();
            filter.MANV = searchText.Text != null ? searchText.Text.ToString() : "";
            filter.TENNV = searchText.Text != null ? searchText.Text.ToString() : "";
            if (cmCheck.IsChecked == true)
            {
                filter.MACM = cmText.SelectedValue != null ? cmText.SelectedValue.ToString() : "";
            }
            if (emailCheck.IsChecked == true)
            {
                filter.EMAIL = emailText.Text != null ? emailText.Text.ToString() : "";
            }
            if (phoneCheck.IsChecked == true)
            {
                filter.PHONE = phoneText.Text != null ? phoneText.Text.ToString() : "";
            }
            if (lvlCheck.IsChecked == true)
            {
                int lvl;
                filter.LEVEL = int.TryParse(lvlText.Text, out lvl) ? lvl : -1;
            }

            members = nvManager.FindNV(filter);
            showMember();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;
            if (button != null)
            {
                // Find the DataGridRow that contains the clicked button
                DataGridRow row = FindVisualParent<DataGridRow>(button);
                if (row != null)
                {
                    // Access the data item behind the row
                    DTO_NhanVien? item = row.Item as DTO_NhanVien;
                    if (item != null)
                    {
                        AddAndUpdateEmployee updateDialog = new AddAndUpdateEmployee(item);
                        bool? res = updateDialog.ShowDialog();
                        if (res != null && res == true)
                        {
                            members = nvManager.GetAllData();
                            showMember();
                        }
                        //if (item.MANV == crnUser.MANV)
                        //{
                        //    setUser();
                        //}
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
                    DTO_NhanVien? item = row.Item as DTO_NhanVien;
                    if (item != null)
                    {
                        MessageBoxResult resu = MessageBox.Show("Bạn đang xóa nhân viên " + item.MANV + ", thao tác này không thể quay lại.", "Warning", MessageBoxButton.OKCancel);
                        if (resu == MessageBoxResult.OK)
                        {
                            (bool, string) res = nvManager.DeleteByID(item);
                            if (res.Item1 == true)
                            {
                                MessageBox.Show(res.Item2);
                                members = nvManager.GetAllData();
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

        private void ButtonDelete_All_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res = MessageBox.Show("Bạn đang xóa tất cả nhân viên đã chọn, thao tác này không thể quay lại.", "Warning", MessageBoxButton.OKCancel);
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
                        DTO_NhanVien? nv = item as DTO_NhanVien;
                        if (nv != null)
                        {
                            nvManager.DeleteByID(nv);
                            members = nvManager.GetAllData();
                            showMember();
                        }
                    }
                }

            }
        }

        private void membersDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            /*var firstCol = membersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "C");
            var operationCol = membersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "Operations");
            foreach (var item in membersDataGrid.Items)
            {
                DTO_NhanVien? nv = item as DTO_NhanVien;
                if (nv != null && nv.MANV == LoginWindow.crnUser.MANV)
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
            }*/
        }

        private void ButtonDelete_Loaded(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            
            if (button != null)
            {
                if (tkManager.checkQH(LoginWindow.crnUser, "XoaNV") == false)
                {
                    button.Visibility = Visibility.Collapsed;
                    return;
                }
                // Find the DataGridRow that contains the clicked button
                DataGridRow row = FindVisualParent<DataGridRow>(button);
                if (row != null)
                {
                    // Access the data item behind the row
                    DTO_NhanVien? item = row.Item as DTO_NhanVien;
                    
                    if (item != null && item.MANV == LoginWindow.crnUser.MANV)
                    {
                        button.Visibility = Visibility.Collapsed;
                    }
                }

                // Do something with the item...
            }
        }

        private void EditButton_Loaded(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (tkManager.checkQH(LoginWindow.crnUser, "SuaNV") == false)
                {
                    button.Visibility = Visibility.Collapsed;
                    return;
                }
            }
            
        }

        private void tk_Btn_Click(object sender, RoutedEventArgs e)
        {
            UserInfo userinfWindow = new UserInfo();
            userinfWindow.ShowDialog();
        }

        private void logout_Btn_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow.crnUser = new DTO_TaiKhoan();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            MessageBox.Show("Đã đăng xuất khỏi hệ thống");
            this.Close();
            
        }
    }
}
