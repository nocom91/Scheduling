using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity.Core.Objects;

namespace Scheduling
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SchedulingEntities dbContext;
        private string choosenTime;
        private double[,] finalTimeTable;
        private List<Stop> stops;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dbContext = new SchedulingEntities();
            var query = from route in dbContext.Routes
                        select route.Name;
            routesList.ItemsSource = query.ToList();
            query = from time in dbContext.Times
                    orderby time.Minutes ascending
                    select time.Name;
            timesList.ItemsSource = query.ToList();
        }

        private void routesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dbContext = new SchedulingEntities();
            var selectedValue = ((ComboBox)sender).SelectedValue;
            var temp = dbContext.Stops.OrderBy(x => x.ID).Select(x => x).Where(x => x.Route == (
                  dbContext.Routes.FirstOrDefault(y => y.Name == selectedValue.ToString())
              )).ToList();
            dataGridStops.ItemsSource = temp;
        }

        private void timesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedValue = ((ComboBox)sender).SelectedValue;
            choosenTime = selectedValue.ToString();
        }

        private void calculate_Click(object sender, RoutedEventArgs e)
        {
            StartBuses();
            Results window = new Results();
            window.timetable = finalTimeTable;
            window.selectedStops = stops;
            window.Show();
        }
        private void StartBuses()
        {
            double temp = 0;
            Double.TryParse(fVeolcity.Text, out temp);
            AntColonyClass.feromonVelocity = temp;
            Double.TryParse(fWeight.Text, out temp);
            AntColonyClass.feromonWeight = temp;
            Double.TryParse(vWeight.Text, out temp);
            AntColonyClass.visionWeight = temp;
            AntColonyClass.hoursNumber = 1;
            int departureMinutes = DataConnector.ParseTimeToMinutes(kSectorTime.Text);
            int interval = 0;
            Int32.TryParse(textboxInterval.Text, out interval);
            var tempStops =dataGridStops.SelectedItems;
            stops = new List<Stop>();
            foreach (var stop in tempStops)
            {
                var tempStop = (Stop)stop;
                stops.Add(tempStop);
            }
            var stopsNames = stops.Select(x => x.Name).ToArray();
            var tempSpeed = DataConnector.GetVehicleSpeedsByStops(stopsNames, false, choosenTime);
            var tempDistance = DataConnector.GetDistancesBetweenStops(stopsNames);
            var optTime = new List<double?>();
            for (int i = 0; i < stops.Count; i++)
            {
                optTime.Add(tempDistance[i] / tempSpeed[i]);
            }
            var tempPeopleIn = DataConnector.GetAmountOfPeopleIN(stopsNames, choosenTime);
            var tempPeopleOut = DataConnector.GetAmountOfPeopleOUT(stopsNames, choosenTime);
            int sumPeopleIn = 0, sumPeopleOut = 0;
            for (int i = 0; i < stopsNames.Count(); i++)
            {
                sumPeopleIn += (int)tempPeopleIn[i];
                sumPeopleOut += (int)tempPeopleOut[i];
            }
            var newKeySector = new KeySector(sumPeopleIn, sumPeopleOut, 50, interval);
            var amountofBuses = newKeySector.BusNumber();
            AntColonyClass.vehicleNumber = amountofBuses;
            var choosenInterval = choosenTime.Split('-', ':');
            int startTime = Int32.Parse(choosenInterval[0]) * 60 + Int32.Parse(choosenInterval[1]);
            AntColonyClass.InitializeVariables(stopsNames.Count(), startTime);
            AntColonyClass.InitializeIntervals(optTime.ToArray());
            AntColonyClass.InitializeDepartureTime(interval, startTime);
            AntColonyClass.InitializePeopleInBus(tempPeopleIn.ToArray(), tempPeopleOut.ToArray());
            AntColonyClass.InitializeFeromons();
            var timetable = AntColonyClass.StartBuses();
            finalTimeTable = SingleDeviceAlgorith.CalculateSchedule(tempPeopleIn.ToArray(),
                tempPeopleOut.ToArray(), timetable, departureMinutes, interval);
            
        }
    }
}
