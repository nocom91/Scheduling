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

namespace Scheduling
{
    /// <summary>
    /// Логика взаимодействия для Results.xaml
    /// </summary>
    public partial class Results : Window
    {
        public double[,] timetable;
        public List<Stop> selectedStops; 
        public Results()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chart.ChartAreas.Add(new ChartArea("Timetables"));
            for (int i = 0; i < timetable.GetLength(0); i++)
            {
                chart.Series.Add(new Series('s'+i.ToString()));
                chart.Series['s' + i.ToString()].ChartArea = "Timetables";
                chart.Series['s' + i.ToString()].ChartType = SeriesChartType.Line;
                var axisXdata = selectedStops.Select(x=>x.Name).ToArray();
                var axisYdata = new double[timetable.GetLength(1)];
                for (int j = 0; j < timetable.GetLength(1); j++)
                {
                    axisYdata[j] = timetable[i, j];
                }
                chart.Series['s' + i.ToString()].Points.DataBindXY(axisXdata, axisYdata);
            }
            //chart.Series["s1"].ChartArea = "Timetables";
            //chart.Series["s1"].ChartType = SeriesChartType.Line;
            //string[] axisXData = new string[] { "a", "b", "c" };
            //double[] axisYData = new double[] { 0.1, 1.5, 1.9 };
            //chart.Series["s1"].Points.DataBindXY(axisXData, axisYData);
        }
    }
}
