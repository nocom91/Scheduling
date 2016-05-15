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
        public static List<double> GetVehicleSpeedsByStops(string[] stops, bool direction)
        {
            dbContext = new SchedulingEntities();
            var speedsInMeterPerMin = new List<double>();
            foreach (var str in stops)
            {
                var stopID = (dbContext.Stops.FirstOrDefault(y => y.Name == str)).GUID_ID;
                double speed = dbContext.Speeds.Where(x => (x.Start_StopID == stopID) 
                    && (x.Direction == direction)).
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
    }
}
