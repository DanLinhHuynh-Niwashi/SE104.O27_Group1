using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BUS;
using DTO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static DTO_TaiKhoan crnUser = new DTO_TaiKhoan();
        public static DTO_NhanVien nhanVien = new DTO_NhanVien();
        BUS_NhanVien nvManager = new BUS_NhanVien();

        string manv;

        public MainWindow(/*string manv*/)
        {
            InitializeComponent();
            //nhanVien.MANV = manv;
            //if (nhanVien.MANV != "") username.Text = nhanVien.TENNV;
            NavigateTo("Home");
        }

        private void TkBtn_Click(object sender, RoutedEventArgs e)
        {
            UserInfo userinfWindow = new UserInfo();
            userinfWindow.ShowDialog();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.IsEnabled = true;
            ButtonOpenMenu.IsEnabled = false;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.IsEnabled = false;
            ButtonOpenMenu.IsEnabled = true;
        }

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility

            if (ButtonCloseMenu.IsEnabled == true && ButtonOpenMenu.IsEnabled == false)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_report.Visibility = Visibility.Collapsed;
                tt_employee.Visibility = Visibility.Collapsed;
                tt_project.Visibility = Visibility.Collapsed;
            }
            else if (ButtonCloseMenu.IsEnabled == false && ButtonOpenMenu.IsEnabled == true)
            {
                tt_home.Visibility = Visibility.Visible;
                tt_report.Visibility = Visibility.Visible;
                tt_employee.Visibility = Visibility.Visible;
                tt_project.Visibility = Visibility.Visible;
            }
        }

        private bool IsMaximize = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximize)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximize = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;

                    IsMaximize = true;
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void NavigateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string ViewName = button.Tag as string;
                NavigateTo(ViewName);
            }
        }

        public void NavigateTo(string ViewName)
        {
            UserControl view = ViewName switch
            {
                "Home" => new HomeWindow(),
                //"Report" => new ReportWindow(),
                "Employee" => new EmployeeWindow(),
                "Project" => new ProjectWindow(),
                _ => null
            };

            if (view != null)
            {
                MainContent.Content = view;
            }
        }
    }
}