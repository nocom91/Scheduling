using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduling
{
    public static class SingleDeviceAlgorith
    {
        private static double timeForDoors = 0.03; //Время открывания и закрывания дверей
        private static double timePassengerInOut = 0.05; //Время на вход или выход из ТС одного пассажира
        public static double[,] CalculateSchedule(int?[] passengersIN, int?[] passengersOUT,
            double[,] timetable, double departureTime, int intervalDefault)
        {
            var times = new double[timetable.GetLength(0), timetable.GetLength(1)];
            double interval;
            for (int j=0; j<timetable.GetLength(0); j++)
            {
                interval = departureTime + j*intervalDefault;
                times[j, 0] = interval;
                for (int i = 1; i < passengersIN.Count(); i++)
                {
                    interval += ((int)(passengersIN[i-1] + passengersOUT[i-1])) * 
                        timePassengerInOut/2 +
                    2 * timeForDoors+timetable[j,i-1] +0.4*timetable[j, i-1];
                    times[j, i] = interval;
                }
            }
            return times;
        }
    }
}
