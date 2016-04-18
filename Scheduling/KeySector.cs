using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduling
{
    public class KeySector
    {
        private static int capacity = 36; //Номинальная вместимость ПАЗ-3205
        public static int Q_in { get; set; } //Количество вошедших пассажиров
        public static int Q_out { get; set; } //Количество вышедших пассажиров
        public static int WorkingHouse { get; set; } // Оборотное время маршрута
        public static int Interval { get; set; } //Предполагаемый интервал выхода


        private static double BetaCoefficient()
        {
            return (double)(Q_in - Q_out) / capacity;
        }

        public static int BusNumber()
        {
            return (int)(WorkingHouse * BetaCoefficient()) / Interval;
        }
    }
}
