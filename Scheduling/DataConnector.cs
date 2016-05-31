using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop;

namespace Scheduling
{

    public static class DataConnector
    {
        private static SchedulingEntities dbContext;
        public static List<double> GetVehicleSpeedsByStops(string[] stops, bool direction, string time)
        {
            dbContext = new SchedulingEntities();
            var speedsInMeterPerMin = new List<double>();
            foreach (var str in stops)
            {
                var stopID = (dbContext.Stops.FirstOrDefault(y => y.Name == str)).GUID_ID;
                double speed = dbContext.Speeds.Where(x => (x.Start_StopID == stopID)
                    && (x.Direction == direction)
                    && (x.PeriodOfTime == time)).
                    FirstOrDefault().VehicleSpeed;

                speedsInMeterPerMin.Add(speed * 16.67);
            }
            return speedsInMeterPerMin;
        }

        public static List<int?> GetDistancesBetweenStops(string[] stops)
        {
            dbContext = new SchedulingEntities();
            var distancesBetweenStops = new List<int?>();
            foreach (var str in stops)
            {
                distancesBetweenStops.Add(dbContext.Stops.FirstOrDefault(x => x.Name == str).Distance);
            }
            return distancesBetweenStops;
        }
        public static List<int?> GetAmountOfPeopleIN(string[] stops, string time)
        {
            dbContext = new SchedulingEntities();
            var peopleAmountIn = new List<int?>();
            foreach (var str in stops)
            {
                var stopID = (dbContext.Stops.FirstOrDefault(y => y.Name == str)).GUID_ID;
                peopleAmountIn.Add(
                    dbContext.Passenger_Flows.Where(x => (x.StopID == stopID)
                    && (x.PeriodOfTime == time)).FirstOrDefault().QuantityIN
                    );
            }
            return peopleAmountIn;
        }
        public static List<int?> GetAmountOfPeopleOUT(string[] stops, string time)
        {
            dbContext = new SchedulingEntities();
            var peopleAmountOut = new List<int?>();
            foreach (var str in stops)
            {
                var stopID = (dbContext.Stops.FirstOrDefault(y => y.Name == str)).GUID_ID;
                peopleAmountOut.Add(
                    dbContext.Passenger_Flows.Where(x => (x.StopID == stopID)
                    && (x.PeriodOfTime == time)).FirstOrDefault().QuantityOUT
                    );
            }
            return peopleAmountOut;
        }

        public static List<String> GetRoutesList()
        {
            dbContext = new SchedulingEntities();
            var query = from route in dbContext.Routes
                        select route.Name;
            return query.ToList();
        }
        public static List<string> GetTimesList()
        {
            dbContext = new SchedulingEntities();
            var query = from time in dbContext.Times
                    orderby time.Minutes ascending
                    select time.Name;
            return query.ToList();
        }

        public static List<Stop> GetStopsOfARoute(object selectedValue)
        {
            dbContext = new SchedulingEntities();
            var temp = dbContext.Stops.OrderBy(x => x.ID).Select(x => x).Where(x => x.Route == (
                  dbContext.Routes.FirstOrDefault(y => y.Name == selectedValue.ToString())
              )).ToList();
            return temp;
        }

        public static int ParseTimeToMinutes(string time)
        {
            int timeInMinutes = 0;
            var hoursminutes = time.Split(':');
            timeInMinutes = Int32.Parse(hoursminutes[0]) * 60 + Int32.Parse(hoursminutes[1]);
            return timeInMinutes;
        }
        public static string ParseMinutesToTimeString(double minutes)
        {
            string time = "";
            int tempMinutes = (int)Math.Round(minutes);
            time = ((int)(tempMinutes / 60)).ToString() + ':';
            if ((int)tempMinutes % 60 == 0)
            {
                time += "00";
            }
            else if ((int)tempMinutes % 60 < 10)
            {
                time+='0'+ ((int)tempMinutes % 60).ToString();
            }
            else
            {
                time += ((int)tempMinutes % 60).ToString();
            }

            return time;
        }
    }
}
