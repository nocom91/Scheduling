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
        public static void CalculateSchedule(int?[] passengersIN, int?[] passengersOUT,
            double[,] timetable, double departureTime)
        {
            var times = new double[timetable.GetLength(0), timetable.GetLength(1)];
            double interval;
            for (int j=0; j<timetable.GetLength(0); j++)
            {
                interval = departureTime;
                for (int i = 0; i < passengersIN.Count(); i++)
                {
                    interval += ((int)(passengersIN[i] + passengersOUT[i])) * timePassengerInOut +
                    2 * timeForDoors+timetable[j,i]+ timetable[j, i]*0.4;
                    times[j, i] = interval;
                }
            }
        }
    }
}
