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
            //AntColonyClass.InitializeVariables();
            //AntColonyClass.InitializeIntervals();
            //AntColonyClass.InitializeDepartureTime();
            //AntColonyClass.InitializePeopleInBus();
            //AntColonyClass.InitializeFeromons();
            ////AntColonyClass.StartBuses();
            //AntColonyClass.StartBusesReverse();

            //List<string> stops = new List<string>() {"поселок Лоскутово", "Томское ДРСУ",
            //    "2-е мичуринские","1-е мичуринские", "поселок Апрель", "Пороховые склады",
            //    "поселок Предтеченск", "поселок Ключи", "поселок Просторный", "поселок Геологов",
            //    "2-й переезд", "площадь Южная"
            //};
            //var tempSpeed = DataConnector.GetVehicleSpeedsByStops(stops.ToArray(), false, "6:00-7:00");
            //var tempDistance = DataConnector.GetDistancesBetweenStops(stops.ToArray());
            //var optTime = new List<double?>();
            //for (int i = 0; i < stops.Count; i++)
            //{
            //    optTime.Add(tempDistance[i] / tempSpeed[i]);
            //}
            //var tempPeopleIn = DataConnector.GetAmountOfPeopleIN(stops.ToArray(), "6:00-7:00");
            //var tempPeopleOut = DataConnector.GetAmountOfPeopleOUT(stops.ToArray(), "6:00-7:00");
            //int sumPeopleIn = 0, sumPeopleOut = 0;
            //for (int i = 0; i < stops.Count; i++)
            //{
            //    sumPeopleIn += (int)tempPeopleIn[i];
            //    sumPeopleOut += (int)tempPeopleOut[i];
            //}
            //var newKeySector = new KeySector(sumPeopleIn, sumPeopleOut, 50, 10);
            //var amountofBuses = newKeySector.BusNumber();
            //AntColonyClass.feromonVelocity = 0.8;
            //AntColonyClass.feromonWeight = 3;
            //AntColonyClass.visionWeight = 3;
            //AntColonyClass.hoursNumber = 1;
            //AntColonyClass.vehicleNumber = amountofBuses;
            //AntColonyClass.InitializeVariables(stops.Count);
            //AntColonyClass.InitializeIntervals(optTime.ToArray());

            //AntColonyClass.InitializeDepartureTime();

            //AntColonyClass.InitializePeopleInBus(tempPeopleIn.ToArray(), tempPeopleOut.ToArray());
            //AntColonyClass.InitializeFeromons();
            //var timetable = AntColonyClass.StartBuses();
            //SingleDeviceAlgorith.CalculateSchedule(tempPeopleIn.ToArray(),
            //    tempPeopleOut.ToArray(), timetable, 0);
        }

        private void routesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dbContext = new SchedulingEntities();
            var selectedValue = ((ComboBox)sender).SelectedValue;
            var temp = dbContext.Stops.Select(x => x).Where(x=>x.Route == (
                dbContext.Routes.FirstOrDefault(y=>y.Name == selectedValue.ToString())
            )).ToList();
            dataGridStops.ItemsSource = temp;
            
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
