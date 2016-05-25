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
using System.Windows.Forms.DataVisualization.Charting;
using System.Data;

namespace Scheduling
{
    /// <summary>
    /// Логика взаимодействия для Results.xaml
    /// </summary>
    public partial class Results : Window
    {
        public double[,] timetable;
        public List<Stop> selectedStops;
        private List<double[]> rows;
        public Results()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dt = new DataTable();
            foreach (var stop in selectedStops)
            {
                dt.Columns.Add(stop.Name);
            }
            for (int i = 0; i < timetable.GetLength(0); i++)
            {
                DataRow row = dt.NewRow();
                for (int j = 0; j < timetable.GetLength(1); j++)
                {
                    row[j] = DataConnector.ParseMinutesToTimeString(timetable[i,j]);
                }
                dt.Rows.Add(row);
            }
            simpleDataGrid.DataContext = dt.DefaultView;
           

        }

    }
}
