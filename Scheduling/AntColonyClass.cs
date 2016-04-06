using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduling
{
    public class AntColonyClass
    {
        double feromonVelocity { get; set; }        //Скорость испарения феромона
        double feromonWeight { get; set; }   //Весовой коэффициент для феромона
        private double visionWeight { get; set; } //Весовой коэффициент для видимости
        private int PassengersAll;  //Общее количество перевезенных пассажиров
        private int timeStart = 0;  //Момент начала работы ТС
        private int timeEnd = 10 * 60;  //ТС ездят 10 часов
        
                   
    }
}
