using BUS;
using DTO;
using Syncfusion.Windows.Controls.Gantt;
using Syncfusion.Windows.Controls.Gantt.Chart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for GanttWindow.xaml
    /// </summary>
    public partial class GanttWindow : Window
    {

        BindingList<DTO_CongViec> memberlist = new BindingList<DTO_CongViec>();
        ObservableCollection<TaskDetails> TaskCollection = new ObservableCollection<TaskDetails>();
        public GanttWindow()
        {
            InitializeComponent();
            int i = 1;
            foreach (DTO_CongViec cv in memberlist)
            {
                TaskDetails details = new TaskDetails
                {
                    TaskId = i++,
                    TaskName = cv.TENCV,
                    StartDate = cv.TSTART.Value,
                    FinishDate = cv.TEND.Value,
                    Progress = cv.TIENDO
                };
                TaskCollection.Add(details);
            }
            ganttChart.ItemsSource = TaskCollection;
            ganttChart.ScheduleType = ScheduleType.MonthWithDays;
            
        }    
        public GanttWindow(BindingList<DTO_CongViec>? members = null)
        {
            InitializeComponent();
            if (members != null)
                memberlist = members;
            
            int i = 1;
            foreach (DTO_CongViec cv in memberlist)
            {
                TaskDetails details = new TaskDetails
                {
                    TaskId = i++,
                    TaskName = cv.TENCV,
                    StartDate = cv.TSTART.Value.Date,
                    FinishDate = cv.TEND.Value.Date,
                    Progress = cv.TIENDO,

                };
                TaskCollection.Add(details);
            }
            TaskCollection = new ObservableCollection<TaskDetails>(TaskCollection.OrderBy(i => i.StartDate));
        }

        float[] rate = { 0.06f, 0.5f, 0.11f, 0.11f, 0.11f, 0.11f }; 
        private void ganttChart_Loaded(object sender, RoutedEventArgs e)
        {
            ganttChart.GanttGrid.Columns.Clear();
            ganttChart.GanttGrid.Columns.Add(new Syncfusion.Windows.Controls.Grid.GridTreeColumn("TaskID",0));
            ganttChart.GanttGrid.Columns.Add(new Syncfusion.Windows.Controls.Grid.GridTreeColumn("TaskName", "Name", 0));
            ganttChart.GanttGrid.Columns.Add(new Syncfusion.Windows.Controls.Grid.GridTreeColumn("StartDate", "Start", 0));
            ganttChart.GanttGrid.Columns.Add(new Syncfusion.Windows.Controls.Grid.GridTreeColumn("FinishDate", "End", 0));
            ganttChart.GanttGrid.Columns.Add(new Syncfusion.Windows.Controls.Grid.GridTreeColumn("Duration", "Duration", 0));
            ganttChart.GanttGrid.Columns.Add(new Syncfusion.Windows.Controls.Grid.GridTreeColumn("Progress", "Progress", 0));

            int i = 0;
            foreach (var columns in ganttChart.GanttGrid.Columns)
            {
                columns.Width = (ganttChart.GridWidth.Value) * rate[i++];
            }

            ganttChart.ItemsSource = TaskCollection;
            ganttChart.ScheduleType = ScheduleType.MonthWithDays;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            int i = 0;
            foreach (var columns in ganttChart.GanttGrid.Columns)
            {
                columns.Width = (ganttChart.GridWidth.Value) * rate[i++];
            }
        }
    }
}
