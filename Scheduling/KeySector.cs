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
        private int Q_in; //Количество вошедших пассажиров
        private int Q_out;//Количество вышедших пассажиров
        private int WorkingHours; // Оборотное время маршрута
        private int Interval;//Предполагаемый интервал выхода

        public KeySector(int qin, int qout, int workingh, int interval)
        {
            Q_in = qin;
            Q_out = qout;
            WorkingHours = workingh;
            Interval = interval;
        }


        private double BetaCoefficient()
        {
            var beta = (double)(this.Q_in - this.Q_out) / capacity;
            return beta;
        }

        public int BusNumber()
        {
            return (int)Math.Ceiling(((this.WorkingHours * BetaCoefficient()) / this.Interval)+1);
        }
    }
}
