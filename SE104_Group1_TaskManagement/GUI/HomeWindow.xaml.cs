using BUS;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : UserControl
    {
        public HomeWindow()
        {
            InitializeComponent();
            if (BUS_TaiKhoan.Instance.checkQH(LoginWindow.crnUser, "XemBC") == false)
            {
                ReportBtn.Visibility = Visibility.Collapsed;
            }
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
                "Report" => new ReportWindow(),
                "Employee" => new EmployeeWindow(),
                "Project" => new ProjectWindow(),
                _ => null
            };

            if (view != null)
            {
                HomeContent.Visibility = Visibility.Collapsed;
                MainContent.Content = view;
            }
        }
    }
}
