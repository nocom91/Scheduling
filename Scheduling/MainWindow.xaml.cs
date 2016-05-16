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
            //dbContext = new SchedulingEntities();
            //List<string> stops = new List<string>() {
            //    "Кольцевая Лоскутовская", "поселок Лоскутово", "Томское ДРСУ",
            //    "2-е мичуринские","1-е мичуринские", "поселок Апрель", "Пороховые склады",
            //    "поселок Предтеченск", "поселок Ключи", "поселок Просторный", "поселок Геологов",
            //    "2-й переезд", "площадь Южная", "Транспортное кольцо", "Дворец спорта",
            //    "улица Вершинина", "улица Белинского", "улица Учебная", "ТЭМЗ",
            //    "Томский государственный университет", "площадь Ново-Соборная",
            //    "Главпочтамт", "Театр юного зрителя", "ЦУМ", "Речной вокзал", "Центральный рынок",
            //    "улица Дальне-Ключевская", "Дрожжевой завод", "Карандашная фабрика"
            //};
            //int i = 1;
            //foreach (var stop in stops)
            //    dbContext.Stops.Add(new Stop() {
            //        ID = i++,
            //        GUID_ID = Guid.NewGuid(),
            //        Name = stop,
            //        Route_ID = new Guid("96443343-1617-4887-8F23-D1A45D8AAB59") 
            //    });            
            //dbContext.SaveChanges();
            //AntColonyClass.feromonVelocity = 0.2;
            //AntColonyClass.feromonWeight = 2;
            //AntColonyClass.visionWeight = 2;
            //AntColonyClass.hoursNumber = 10;
            //AntColonyClass.vehicleNumber = 29;
            //AntColonyClass.passengersSpeed = 5;
            //AntColonyClass.passengersSpeedOut = 2;
            //AntColonyClass.InitializeFeromons();
            //AntColonyClass.InitializeIntervals();
            //AntColonyClass.InitializePeopleInBus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            //AntColonyClass.InitializeVariables();
            //AntColonyClass.InitializeIntervals();
            //AntColonyClass.InitializeDepartureTime();
            //AntColonyClass.InitializePeopleInBus();
            //AntColonyClass.InitializeFeromons();
            ////AntColonyClass.StartBuses();
            //AntColonyClass.StartBusesReverse();

            List<string> stops = new List<string>() {"поселок Лоскутово", "Томское ДРСУ",
                "2-е мичуринские","1-е мичуринские", "поселок Апрель", "Пороховые склады",
                "поселок Предтеченск", "поселок Ключи", "поселок Просторный", "поселок Геологов",
                "2-й переезд", "площадь Южная"
            };
            var tempSpeed = DataConnector.GetVehicleSpeedsByStops(stops.ToArray(), false);
            var tempDistance = DataConnector.GetDistancesBetweenStops(stops.ToArray());
            var optTime = new List<double?>();
            for (int i = 0; i < stops.Count; i++)
            {
                optTime.Add(tempDistance[i] / tempSpeed[i]);
            }
            AntColonyClass.feromonVelocity = 0.8;
            AntColonyClass.feromonWeight = 3;
            AntColonyClass.visionWeight = 3;
            AntColonyClass.hoursNumber = 1;
            AntColonyClass.passengersSpeed = 4;
            AntColonyClass.passengersSpeedOut = 4;
            AntColonyClass.vehicleNumber = 7;
            AntColonyClass.InitializeVariables(stops.Count-1);
            AntColonyClass.InitializeIntervals(optTime.ToArray());

            AntColonyClass.InitializeDepartureTime();
            AntColonyClass.InitializePeopleInBus();
            AntColonyClass.InitializeFeromons();
            AntColonyClass.StartBuses();
        }
    }
}
