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
using DTO;

namespace GUI
{
    /// <summary>
    /// Interaction logic for ChuyenMonRule.xaml
    /// </summary>
    public partial class UpdatePercentRule : UserControl
    {
        public UpdatePercentRule()
        {
            InitializeComponent();
            upSlider.Value = BUS_StaticTables.Instance.GetThamSo().PERCENTUP;
        }

        private void Discard_Btn_Click(object sender, RoutedEventArgs e)
        {
            upSlider.Value = BUS_StaticTables.Instance.GetThamSo().PERCENTUP;
        }

        private void Save_Btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(BUS_StaticTables.Instance.SetThamSo(new DTO_ThamSo((float)upSlider.Value)).Item2);
        }
    }
}
