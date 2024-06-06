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
using MailKit.Search;
using System.Xml;
using System.Windows.Controls.Primitives;
using System.Globalization;
using MimeKit;

namespace GUI
{
    /// <summary>
    /// Interaction logic for ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow : UserControl
    {
        public static DTO_DuAn project = new DTO_DuAn();
        BUS_DuAn projectManager = new BUS_DuAn();
        BindingList<DTO_DuAn> members = new BindingList<DTO_DuAn>();

        Dictionary<string, DTO_NhanVien> nv = BUS_StaticTables.Instance.GetAllDataNV();
        Dictionary<string, DTO_LoaiSK> lsk = BUS_StaticTables.Instance.GetAllDataLSK();




        public ProjectWindow()
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            membersDataGrid.LoadingRow += MembersDataGrid_LoadingRow;
            ownerText.ItemsSource = nv;
            ownerText.DisplayMemberPath = "Value.MANV";
            ownerText.SelectedValuePath = "Value.MANV";
            lskText.ItemsSource = lsk;
            lskText.DisplayMemberPath = "Value.TENLSK";
            lskText.SelectedValuePath = "Value.MALSK";
            var converter = new BrushConverter();
            members = projectManager.GetAllData();
            showMember();
        }
        

        private void MembersDataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
        {
            var firstCol = membersDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "C");
            var lskCol = membersDataGrid.Columns.First(c => c.Header.ToString() == "Mã LSK");
            var ownerCol = membersDataGrid.Columns.First(c => c.Header.ToString() == "Mã NQL");


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
                //if (lskCol != null)
                //{
                //    var chBx = lskCol.GetCellContent(row) as TextBlock;
                //    DTO_LoaiSK temp = new DTO_LoaiSK();
                //    lsk.TryGetValue(da.MALSK, out temp);
                //    chBx.Text = temp.TENLSK;

                //}

                //if (ownerCol != null)
                //{
                //    var chBx = ownerCol.GetCellContent(row) as TextBlock;
                //    DTO_NhanVien temp = new DTO_NhanVien();
                //    nv.TryGetValue(da.MAOWNER, out temp);
                //    string manv = temp.MANV;
                //    chBx.Text = manv;

                //}
            };
        }


        void showMember()
        {
            membersDataGrid.ItemsSource = members;
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
                            members = projectManager.GetAllData();
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
                members = projectManager.GetAllData();
                membersDataGrid.ItemsSource = members;
            }
        }

        private void ButtonView_Click(object sender, RoutedEventArgs e)
        {
            if (membersDataGrid.SelectedItem is DTO_DuAn selectedDA)
            {
                UserControl view = new TaskWindow(selectedDA.MADA);

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
                filter.TSTART = TStartPicker.Text != null ? TStartPicker.Text.ToString() : "";
            }
            if (TEndCheck.IsChecked == true)
            {
                filter.TEND = TEndPicker.Text != null ? TEndPicker.Text.ToString() : "";
            }
            members = projectManager.FindDA(filter, ngsl, ngsh);
            showMember();
        }

    }
}
