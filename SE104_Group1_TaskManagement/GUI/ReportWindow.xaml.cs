
using Aspose.Pdf.Drawing;
using Aspose.Pdf.Operators;
using Aspose.Words;
using Aspose.Words.Tables;
using BUS;
using DAL;
using DTO;
using Microsoft.Data.SqlClient;
//using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
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
using Page = Aspose.Pdf.Page;

namespace GUI
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>


    public partial class ReportWindow : Window
    {
        public static DTO_NhanVien crnUser = new DTO_NhanVien();
        BUS_NhanVien nvManager = new BUS_NhanVien();
        BUS_TaiKhoan tkManager = new BUS_TaiKhoan();
        BUS_DuAn daManager = new BUS_DuAn();
        BindingList<DTO_DuAn> projects = new BindingList<DTO_DuAn>();
        Dictionary<string, DTO_ChuyenMon> cm = BUS_StaticTables.Instance.GetAllDataCM();
        public ReportWindow()
        {
            InitializeComponent();
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            projectsDataGrid.LoadingRow += ProjectsDataGrid_LoadingRow;
            this.WindowState = WindowState.Maximized;

            setUser();
            projects = daManager.GetAllData();

            showProjects();
        }

        void setUser()
        {
            crnUser = nvManager.GetByID(LoginWindow.crnUser.MANV);
            if (crnUser.MANV != "") username.Text = crnUser.TENNV;
        }

        private void ProjectsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var firstCol = projectsDataGrid.Columns.FirstOrDefault(c => c.Header.ToString() == "C");

            e.Row.Loaded += (s, args) =>
            {
                var row = (DataGridRow)s;
                var item = row.Item;

                DTO_DuAn? da = item as DTO_DuAn;
                if (da != null && da.MADA == LoginWindow.crnUser.MANV)
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
            };
        }
        void showProjects()
        {
            projectsDataGrid.ItemsSource = projects;
            TotalEvents.Text = projects.Count.ToString();
            TotalMoney.Text = daManager.CalTongNganSach(projects).ToString() + " VND";
            LeftMoney.Text = (daManager.CalTongNganSach(projects) - daManager.CalTongDaDung(projects)).ToString() + " VND";
        }
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            AddAndUpdateProject addDialog = new AddAndUpdateProject();
            bool? res = addDialog.ShowDialog();
            if (res != null && res == true)
            {
                projects = daManager.GetAllData();
                projectsDataGrid.ItemsSource = projects;
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

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set tooltip visibility
            if (ButtonCloseMenu.IsEnabled == true && ButtonOpenMenu.IsEnabled == false)
            {
                tt_home.Visibility = Visibility.Collapsed;
                tt_project.Visibility = Visibility.Collapsed;
                tt_task.Visibility = Visibility.Collapsed;
            }
            else if (ButtonCloseMenu.IsEnabled == false && ButtonOpenMenu.IsEnabled == true)
            {
                tt_home.Visibility = Visibility.Visible;
                tt_project.Visibility = Visibility.Visible;
                tt_task.Visibility = Visibility.Visible;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
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
            var firstCol = projectsDataGrid.Columns.OfType<DataGridCheckBoxColumn>().FirstOrDefault(c => c.DisplayIndex == 0);
            if (chkSelectAll == null || firstCol == null || projectsDataGrid?.Items == null)
            {
                return;
            }
            foreach (var item in projectsDataGrid.Items)
            {
                var chBx = firstCol.GetCellContent(item) as CheckBox;
                if (chBx == null || chBx.Visibility != Visibility.Visible)
                {
                    continue;
                }
                chBx.IsChecked = chkSelectAll.IsChecked;
            }
        }

        private void projectsDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            // Implement logic if needed
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void projectsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DTO_DuAn filter = new DTO_DuAn();
            filter.TSTART = StartDayPicker.Text;
            filter.TEND = EndDayPicker.Text;
            projects = daManager.FindDA(filter, -1, -1);
            showProjects();
        }



        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int columnPosition = 20;
            int rowPosition = 25;

            DrawInfo(e.Graphics, sender as PrintDocument, ref rowPosition);
            
            // run function to draw headers
            DrawHeader(new System.Drawing.Font("Times New Roman", 12, System.Drawing.FontStyle.Bold), e.Graphics, ref columnPosition, ref rowPosition, projectsDataGrid); // runs the DrawHeader function

            rowPosition += 35; // sets the distance below the header text and the next black line (ruler)

            // run function to draw each row
            DrawGridBody(e.Graphics, ref columnPosition, ref rowPosition, projectsDataGrid);

            // run function to draw each row
            DrawSign(e.Graphics, sender as PrintDocument, ref rowPosition);
        }
        int[] width = { 90, 300, 100, 120, 120, 100, 100, 100, 100, 100, 100, 100 };

        private void DrawSign(Graphics g, PrintDocument p, ref int rowPosition)
        {
            rowPosition = rowPosition + 30;
            DateTime now = DateTime.Now;
            g.DrawString("Ngày " + now.Day.ToString() + " tháng " + now.Month.ToString() + " năm " + now.Year.ToString(), new System.Drawing.Font("Times New Roman", 14, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black,
                new System.Drawing.Rectangle(new System.Drawing.Point(p.DefaultPageSettings.PaperSize.Height / 2, rowPosition), new System.Drawing.Size(p.DefaultPageSettings.PaperSize.Height/2, 0)),
                new StringFormat { Alignment = StringAlignment.Center });

            rowPosition += 40;
            g.DrawString("Ký và ghi rõ họ tên", new System.Drawing.Font("Times New Roman", 12), System.Drawing.Brushes.Black,
                new System.Drawing.Rectangle(new System.Drawing.Point( p.DefaultPageSettings.PaperSize.Height / 2, rowPosition), new System.Drawing.Size(p.DefaultPageSettings.PaperSize.Height / 2, 0)),
                new StringFormat { Alignment = StringAlignment.Center });

        }
        private void DrawInfo(Graphics g, PrintDocument p, ref int rowPosition)
        {
            g.DrawString("BÁO CÁO DỰ ÁN", new System.Drawing.Font("Times New Roman", 16, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Red,
                new System.Drawing.Rectangle(System.Drawing.Point.Empty, new System.Drawing.Size(p.DefaultPageSettings.PaperSize.Height, 2 * rowPosition + 40)),
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

            rowPosition += 40;

            g.DrawString("Từ ngày: " + StartDayPicker.Text, new System.Drawing.Font("Times New Roman", 12, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black,
                20, rowPosition);
            rowPosition += 30;

            g.DrawString("Đến ngày: " + EndDayPicker.Text, new System.Drawing.Font("Times New Roman", 12, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.Black,
                 20, rowPosition);
            rowPosition += 30;

            g.DrawString("Số dự án: " + TotalEvents.Text, new System.Drawing.Font("Times New Roman", 12), System.Drawing.Brushes.Black,
                 20, rowPosition);
            rowPosition += 30;

            g.DrawString("Còn lại/Tổng ngân sách: " + LeftMoney.Text + "/" + TotalMoney.Text, new System.Drawing.Font("Times New Roman", 12), System.Drawing.Brushes.Black,
                20, rowPosition);
            rowPosition += 50;
        }
        // DrawHeader will draw the column title, move over, draw the next column title, move over, and continue.
        private int DrawHeader(System.Drawing.Font boldFont, Graphics g, ref int columnPosition, ref int rowPosition, DataGrid dataGridView)
        {
            int i = 0;
            foreach (DataGridColumn dc in dataGridView.Columns)
            {

                //MessageBox.Show("dc = " + dc);

                g.DrawString(dc.Header.ToString(), boldFont, System.Drawing.Brushes.Black, (float)columnPosition, (float)rowPosition);
                columnPosition += width[i++]; ; // adds to colPos. value the width value of the column + 5. 
            }

            return columnPosition;
        }

        /* DrawGridBody will loop though each row and draw it on the screen. It starts by drawing a solid line on the screen, 
         * then it moves down a row and draws the data from the first grid column, then it moves over, then draws the data from the next column,
         * moves over, draws the data from the next column, and continus this pattern. When the entire row is drawn it starts over and draws
         * a solid line then the row data, then the next solid line and then row data, etc.
        */
        
        private void DrawGridBody(Graphics g, ref int columnPosition, ref int rowPosition, DataGrid dataGridView)
            {
            // loop through each row and draw the data to the graphics surface.\
            DataTable dt = ToDataTable(projectsDataGrid);

            
            foreach (DataRow dr in (dt.Rows))
                {
                    columnPosition = 20;

                    // draw a line to separate the rows 
                    g.DrawLine(Pens.Black, new System.Drawing.Point(0, rowPosition), new System.Drawing.Point((int)this.Width, rowPosition));
                int i = 0;
                // loop through each column in the row, and draw the individual data item
                foreach (DataGridColumn dc in dataGridView.Columns)
                    {

                        // draw string in the column
                        string text = dr[dc.Header.ToString()].ToString();
                        g.DrawString(text, new System.Drawing.Font("Times New Roman", 12), System.Drawing.Brushes.Black, (float)columnPosition, (float)rowPosition + 10f); // the last number (10f) sets the space between the black line (ruler) and the text below it.

                        // go to the next column position
                        columnPosition += width[i++];
                    }

                    // go to the next row position
                    rowPosition = rowPosition + 30; // this sets the space between the row text and the black line below it (ruler).
                }
            }



        System.Windows.Forms.PrintPreviewDialog printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog(); // instantiate new print preview dialog
        private void doc_EndPrint(object sender, PrintEventArgs e) {

            if (printPreviewDialog1 != null)
            {
                printPreviewDialog1.Close();
            }
            
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {

                PrintDocument prDoc = new PrintDocument();
                printPreviewDialog1.Document = prDoc;

                prDoc.PrintPage += new PrintPageEventHandler
                   (this.printDocument_PrintPage);

                System.Windows.Forms.PrintDialog printDlg = new System.Windows.Forms.PrintDialog();


                if (printDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    prDoc.PrinterSettings = printDlg.PrinterSettings;
                    prDoc.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 826, 1169);
                    prDoc.DefaultPageSettings.Landscape = true;
                    printPreviewDialog1.ShowDialog();
                }


                /*DataTable dt = new DataTable();
                dt = ToDataTable(projectsDataGrid);
                // Create Document instance
                Document doc = new Document();
                // We can position where we want the table to be inserted and specify any extra formatting to the table.
                DocumentBuilder builder = new DocumentBuilder(doc);

                // We want to rotate the page landscape as we expect a wide table.
                doc.FirstSection.PageSetup.Orientation = Aspose.Words.Orientation.Landscape;

                Aspose.Words.Tables.Table table = ImportTableFromDataTable(builder, dt, true);

                // We can apply a table style as a very quick way to apply formatting to the entire table.
                table.StyleIdentifier = StyleIdentifier.MediumList2Accent1;
                table.StyleOptions = TableStyleOptions.FirstRow | TableStyleOptions.RowBands | TableStyleOptions.LastColumn;



                // Save updated PDF
                Random n = new Random();
                string temp = "D:\\tempDocument" + n.Next(1000000, 9999999).ToString() + ".doc";
                doc.Save(temp);

                System.Windows.Forms.PrintDialog printDlg = new System.Windows.Forms.PrintDialog();

                // Initialize the print dialog with the number of pages in the document.
                printDlg.AllowSomePages = true;
                printDlg.PrinterSettings.MinimumPage = 1;
                printDlg.PrinterSettings.MaximumPage = doc.PageCount;
                printDlg.PrinterSettings.FromPage = 1;
                printDlg.PrinterSettings.ToPage = doc.PageCount;

                // Сheck if the user accepted the print settings and whether to proceed to document preview.
                if (printDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;




                StreamReader streamToPrint = new StreamReader(temp);
                try
                {
                    PrintDocument pd = new PrintDocument();
                    pd.PrinterSettings = printDlg.PrinterSettings;
                    System.Windows.Forms.PrintPreviewDialog previewDialog = new System.Windows.Forms.PrintPreviewDialog();
                    previewDialog.Document = pd;
                    if (previewDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        pd.Print();
                    }

                }
                finally
                {
                    streamToPrint.Close();
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public Aspose.Words.Tables.Table ImportTableFromDataTable(DocumentBuilder builder, DataTable dataTable,bool importColumnHeadings)
        {
            Aspose.Words.Tables.Table table = builder.StartTable();

            // Check if the columns' names from the data source are to be included in a header row.
            if (importColumnHeadings)
            {
                // Store the original values of these properties before changing them.
                bool boldValue = builder.Font.Bold;
                ParagraphAlignment paragraphAlignmentValue = builder.ParagraphFormat.Alignment;

                // Format the heading row with the appropriate properties.
                builder.Font.Bold = true;
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;

                // Create a new row and insert the name of each column into the first row of the table.
                foreach (DataColumn column in dataTable.Columns)
                {
                    builder.InsertCell();
                    builder.Writeln(column.ColumnName);
                }

                builder.EndRow();

                // Restore the original formatting.
                builder.Font.Bold = boldValue;
                builder.ParagraphFormat.Alignment = paragraphAlignmentValue;
            }

            foreach (DataRow dataRow in dataTable.Rows)
            {
                foreach (object item in dataRow.ItemArray)
                {
                    // Insert a new cell for each object.
                    builder.InsertCell();

                    switch (item.GetType().Name)
                    {
                        case "DateTime":
                            // Define a custom format for dates and times.
                            DateTime dateTime = (DateTime)item;
                            builder.Write(dateTime.ToString("MMMM d, yyyy"));
                            break;
                        default:
                            // By default any other item will be inserted as text.
                            builder.Write(item.ToString());
                            break;
                    }
                }

                // After we insert all the data from the current record, we can end the table row.
                builder.EndRow();
            }

            // We have finished inserting all the data from the DataTable, we can end the table.
            builder.EndTable();

            return table;
        }

        private DataTable ToDataTable(DataGrid dataGridView)
        {
            DataTable dt = new DataTable();
            foreach (DataGridColumn dataGridViewColumn in dataGridView.Columns)
            {
                dt.Columns.Add(dataGridViewColumn.Header.ToString());
            }
            var cell = new object[dataGridView.Columns.Count];
            int i = 0;
            foreach (DTO_DuAn temp in dataGridView.ItemsSource)
            {
                cell[0] = temp.MADA.ToString();
                cell[1] = temp.TENDA.ToString();
                cell[2] = temp.MALSK.ToString();
                cell[3] = temp.TSTART.ToString();
                cell[4] = temp.TEND.ToString();
                cell[5] = temp.NGANSACH.ToString();
                cell[6] = temp.DADUNG.ToString();
                cell[7] = temp.STAT.ToString();
                cell[8] = temp.MAOWNER.ToString();

                DataRow row = dt.NewRow();
                row[dt.Columns[0].ColumnName] = cell[0];
                row[dt.Columns[1].ColumnName] = cell[1];
                row[dt.Columns[2].ColumnName] = cell[2];
                row[dt.Columns[3].ColumnName] = cell[3];
                row[dt.Columns[4].ColumnName] = cell[4];
                row[dt.Columns[5].ColumnName] = cell[5];
                row[dt.Columns[6].ColumnName] = cell[6];
                row[dt.Columns[7].ColumnName] = cell[7];
                row[dt.Columns[8].ColumnName] = cell[8];
                dt.Rows.Add(row);
                i++;
            }
            return dt;
        }
    }
}
