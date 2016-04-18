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
        public int Q_in; //Количество вошедших пассажиров
        public int Q_out;//Количество вышедших пассажиров
        public int WorkingHouse; // Оборотное время маршрута
        public int Interval;//Предполагаемый интервал выхода

        public KeySector(int qin, int qout, int workingh, int interval)
        {

        }


        private double BetaCoefficient()
        {
            var beta = (double)(this.Q_in - this.Q_out) / capacity;
            return beta;
        }

        public int BusNumber()
        {
            return (int)(this.WorkingHouse * BetaCoefficient()) / this.Interval;
        }
    }
}
