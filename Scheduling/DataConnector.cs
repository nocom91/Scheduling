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
       static SchedulingEntities dbContext;
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

                speedsInMeterPerMin.Add(speed*16.67);
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
    }
}
