using BUS;
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
using System.Diagnostics;
using System.Windows.Media.Media3D;

namespace GUI
{
    /// <summary>
    /// Interaction logic for AddAndUpdateTask.xaml
    /// </summary>
    public partial class AddAndUpdateTask : Window
    {
        private Dictionary<string, DTO_NhanVien> nv;
        private Dictionary<string, DTO_ChuyenMon> cm;

        DTO_CongViec initializeCV;

        List<string> Deleted = new List<string>();
        List<string> PhanCongList = new List<string>();

        List<string> DeletedDK = new List<string>();
        List<string> DinhKemList = new List<string>();

        public AddAndUpdateTask(string mada, DTO_CongViec? initialized = null)
        {
            InitializeComponent();

            this.initializeCV = BUS_CongViec.Instance.GetByID(initialized.MACV);
            nv = BUS_StaticTables.Instance.GetAllDataNV();

            manvText.ItemsSource = nv;
            manvText.DisplayMemberPath = "Value.TENNV";
            manvText.SelectedValuePath = "Value.MANV";

            cm = BUS_StaticTables.Instance.GetAllDataCM();

            macmText.ItemsSource = cm;
            macmText.DisplayMemberPath = "Value.TENCM";
            macmText.SelectedValuePath = "Value.MACM";

            macmText.SelectionChanged += MacmText_SelectionChanged;

            //BindingDropDown();
            if (initializeCV != null)
            {
                wTitle.Text = "SỬA CÔNG VIỆC";
                ButtonAddNew.Visibility = Visibility.Hidden;
                ButtonUpdate.Visibility = Visibility.Visible;

                if (BUS_TaiKhoan.Instance.checkQH(LoginWindow.crnUser, "CapNhatCV"))
                {
                    tiendoText.IsEnabled = true;
                    dadungText.IsEnabled = true;
                    dkText.IsEnabled = true;

                    tencvText.IsEnabled = false;
                    tencvText.IsEnabled = false;
                    macmText.IsEnabled = false;
                    TStartPicker.IsEnabled = false;
                    TEndPicker.IsEnabled = false;
                    ngansachText.IsEnabled = false;
                    ycdkText.IsEnabled = false;
                    manvText.IsEnabled = false;
                    manvTokenizer.IsEnabled = false;
                }

                macvText.Text = initializeCV.MACV;
                madaText.Text = initializeCV.MADA;
                tencvText.Text = initializeCV.TENCV;
                macmText.SelectedValue = initializeCV.MACM;
                TStartPicker.SelectedDate = initializeCV.TSTART;
                TEndPicker.SelectedDate = initializeCV.TEND;
                ngansachText.Text = initializeCV.NGANSACH.ToString();
                dadungText.Text = initializeCV.DADUNG.ToString();
                tiendoText.Value = initializeCV.TIENDO;
                ycdkText.Text = initializeCV.YCDK;

                loadTepDK(initializeCV);

            }
            else
            {
                madaText.Text = mada;
                tiendoText.Value = 0;
            }


        }

        void loadMaNV(DTO_CongViec initializeCV)
        {
            //ham tai maNV
            if (initializeCV != null)
            {
                BindingList<DTO_PhanCong> listPC = BUS_CongViec.Instance.GetPhanCong(initializeCV);
                foreach (var pc in listPC)
                {
                    PhanCongList.Add(pc.MANV);
                    manvTokenizer.AddToken(pc.MANV);
                }
            }
        }

        void loadTepDK(DTO_CongViec initializeCV)
        {
            //ham tai congViec
            if (initializeCV != null)
            {
                BindingList<DTO_DinhKem> listDK = BUS_CongViec.Instance.GetDKAll(initializeCV);
                foreach (var pc in listDK)
                {
                    DinhKemList.Add(pc.TEP);
                    dkTokenizer.AddToken(pc.TEP);
                }
            }
        }
        //void BindingDropDown()
        //{



        //}

        private void MacmText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            manvTokenizer.Document.Blocks.Clear();
            var selectedMacm = macmText.SelectedValue as string;
            if (selectedMacm != null)
            {
                var filteredNV = nv.Where(kv => kv.Value.MACM == selectedMacm).ToDictionary(kv => kv.Key, kv => kv.Value);
                manvText.ItemsSource = filteredNV;
                
                if (initializeCV != null)
                {
                    List<string> data = manvTokenizer.getAllDataPresented();
                    foreach (var item in data)
                    {
                        Deleted.Add(item);
                    }
                    if (selectedMacm == initializeCV.MACM)
                    {
                        loadMaNV(initializeCV);
                        Deleted.Clear();
                    }
                }    
            }
            manvText.SelectedIndex = -1;
        }

        private void ButtonAddNew_Click(object sender, RoutedEventArgs e)
        {
            DTO_CongViec newCV = new DTO_CongViec();
            newCV.MADA = madaText.Text;
            newCV.MACV = macvText.Text;
            newCV.TENCV = tencvText.Text;
            newCV.MACM = macmText.SelectedValue != null ? macmText.SelectedValue.ToString() : "";
            newCV.TSTART = TStartPicker.SelectedDate;
            newCV.TEND = TEndPicker.SelectedDate;
            newCV.NGANSACH = long.TryParse(ngansachText.Text, out long tempResultNS) ? tempResultNS : 0;
            newCV.DADUNG = long.TryParse(ngansachText.Text, out long tempResultDD) ? tempResultDD : 0;
            newCV.TIENDO = 0;
            newCV.YCDK = ycdkText.Text;
            (bool, string) res = BUS_CongViec.Instance.AddData(newCV);

            if (res.Item1 == true)
            {
                List<string> manvList = manvTokenizer.getAllDataPresented();
                for (int i = 0; i < manvList.Count; i++)
                {
                    BUS_CongViec.Instance.PhanCong(res.Item2, manvList[i]);
                }

                List<string> dkList = dkTokenizer.getAllDataPresented();
                for (int i = 0; i < dkList.Count; i++)
                {
                    BUS_CongViec.Instance.DinhKem(new DTO_DinhKem(MainWindow.crnNhanVien.MANV, newCV.MACV, "", dkList[i]));
                }

                MessageBox.Show("Thêm task thành công!");
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
            DTO_CongViec cv = new DTO_CongViec();
            cv.MADA = madaText.Text;
            cv.MACV = macvText.Text;
            cv.TENCV = tencvText.Text;
            cv.MACM = macmText.SelectedValue != null ? macmText.SelectedValue.ToString() : "";
            cv.TSTART = TStartPicker.SelectedDate;
            cv.TEND = TEndPicker.SelectedDate;
            cv.NGANSACH = long.TryParse(ngansachText.Text, out long tempResultNS) ? tempResultNS : 0;
            cv.DADUNG = long.TryParse(dadungText.Text, out long tempResultDD) ? tempResultDD : 0;
            cv.TIENDO = (int)tiendoText.Value;
            cv.YCDK = ycdkText.Text;
            cv.TEPDK = dkText.Text;
            (bool, string) res = BUS_CongViec.Instance.EditTask(cv);

            if (res.Item1 == true)
            {
                foreach (var item in Deleted)
                {
                    BUS_CongViec.Instance.DeletePC(new DTO_PhanCong(item, cv.MACV));
                }    

                List<string> manvList = manvTokenizer.getAllDataPresented();
                for (int i = 0; i < manvList.Count; i++)
                {
                    if (!PhanCongList.Contains(manvList[i]))
                    {
                        BUS_CongViec.Instance.PhanCong(cv.MACV, manvList[i]);
                    }    
                }

                foreach (var item in DeletedDK)
                {
                    BUS_CongViec.Instance.DeleteDK(new DTO_DinhKem(MainWindow.crnNhanVien.MANV, cv.MACV, "", item));
                }

                List<string> dkList = dkTokenizer.getAllDataPresented();
                for (int i = 0; i < dkList.Count; i++)
                {
                    string temp = dkList[i];
                    if (!DinhKemList.Contains(temp))
                    {
                        BUS_CongViec.Instance.DinhKem(new DTO_DinhKem(MainWindow.crnNhanVien.MANV, cv.MACV, "", temp));
                    }
                }

                MessageBox.Show("Sửa task thành công!");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(res.Item2);
            }
        }

        private void manvText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (manvText.SelectedIndex >= 0)
            {
                if (manvText.SelectedValue == null) return;
                if (manvTokenizer.getAllDataPresented().Contains(manvText.SelectedValue.ToString()))
                {
                    return;
                }
                manvTokenizer.AddToken(manvText.SelectedValue.ToString());
                if (initializeCV != null)
                {
                    if (Deleted.Contains(manvText.SelectedValue.ToString())) 
                        Deleted.Remove(manvText.SelectedValue.ToString());
                }    
            }
        }


        private T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject;
            if (child is Visual || child is Visual3D)
            {
                parentObject = VisualTreeHelper.GetParent(child);
            }
            else
            {
                // If the child is not a visual, use the logical tree
                parentObject = LogicalTreeHelper.GetParent(child);
            }
            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;
            else
                return FindVisualParent<T>(parentObject);
        }

        private T FindLogicalParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject;
            parentObject = LogicalTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;
            else
                return FindLogicalParent<T>(parentObject);
        }

        
        private void Del_PC_Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ContentPresenter contentPresenter = FindVisualParent<ContentPresenter>(btn);
            InlineUIContainer container = FindLogicalParent<InlineUIContainer>(contentPresenter);
            TokenizingControl tokenizer = FindVisualParent<TokenizingControl>(container);
            if (container != null && tokenizer != null)
            {
                if (tokenizer.Name == "manvTokenizer")
                {
                    bool res = manvTokenizer.deleteToken(container);
                    if (res && initializeCV != null && initializeCV.MACM == macmText.SelectedValue as string)
                    {
                        Deleted.Add(contentPresenter.Content.ToString());
                    }
                }    
                else if (tokenizer.Name == "dkTokenizer")
                {
                    bool res = dkTokenizer.deleteToken(container);
                    if (res && initializeCV != null)
                    {
                        DeletedDK.Add(contentPresenter.Content.ToString());
                    }
                }

            }
        }

        private void dkText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Get the key that was pressed
            Key key = e.Key;
            // Optional: Check for specific keys
            if (key == Key.Enter)
            {
                if (dkText.Text.Length > 1) {
                    if (dkTokenizer.getAllDataPresented().Contains(dkText.Text))
                    {
                        return;
                    }
                    dkTokenizer.AddToken(dkText.Text);
                    dkText.Clear();
                }
                
            }

        }

        private void RollBack_Click(object sender, RoutedEventArgs e)
        {
            dkTokenizer.Document.Blocks.Clear();
            loadTepDK(initializeCV);
            DeletedDK.Clear();
        }
    }

}
