using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduling
{
    public static class AntColonyClass
    {
        static double feromonVelocity { get; set; }        //Скорость испарения феромона
        static double feromonWeight { get; set; }   //Весовой коэффициент для феромона
        static double visionWeight { get; set; } //Весовой коэффициент для видимости
        static double hoursNumber { get; set; } //Временной участок в часах
        static double passengersSpeed { get; set; } //Скорость наполнения остановок, чел./час (чел./мин.)
        private static int PassengersAll;  //Общее количество перевезенных пассажиров
        private static int timeStart = 0;  //Момент начала работы ТС
        private static int timeEnd = (int)(hoursNumber * 60);  //ТС ездят 10 часов
        private static int stopNumber = 29; //Количество остановок на маршруте
        private static int routeNumber = 1; //Количество маршрутов для рассмотрения 
        private static int amountOfPossibleIntervals = 9; //Количество возможных вариантов интервалов
        private static double[,,] possibleIntervals = new double[routeNumber, stopNumber - 1, amountOfPossibleIntervals];
        private static double[,] timeOpt = new double[routeNumber, stopNumber - 1];
        private static double variation = 0.2; //Отклонение времени движения от оптимального
        private static int[] departureTime { get; set; } //Массив моментов выхода ТС с начальной остановки
        private static List<Bus> buses = new List<Bus>(); //Список автобусов, рботающих на маршруте
        //Количество людей, заходящих на остановки в каждый момент времени
        private static int[,] peopleAmount = new int[timeEnd, stopNumber];
        private static double[,] timeTable = new double[departureTime.Length, stopNumber]; //Расписание
        //Количество феромона на остоновках в каждый момент времени
        private static double[,,] feromons = new double[routeNumber, stopNumber, timeEnd];
        static int vehicleNumber { get; set; }
        //Инициализируются массивы, описывающие оптимальные интервалы и возможные интервалы
        static void InitializeIntervals()
        {
            for (int i = 0; i < stopNumber - 1; i++)
            {
                timeOpt[routeNumber - 1, i] = 3; //Задаем оптимальный интервал - 3 минуты
                for (int j = 0; j < amountOfPossibleIntervals; j++)
                {
                    possibleIntervals[routeNumber, i, j] = timeOpt[routeNumber - 1, i] * (1 - variation) +
                        j * 2 * variation * timeOpt[routeNumber - 1, i] / amountOfPossibleIntervals;
                }
            }
        }

        //Инициализируется массив, отвечающий за количество человек, заходящих в ТС в каждый момент времени, на каждой остановке
        static void InitializePeopleInBus()
        {
            for (int i = 0; i < timeEnd; i++)
                for (int j = 0; j < stopNumber; j++)
                    peopleAmount[i, j] = (int)Math.Round(passengersSpeed);
        }

        static void StartBuses()
        {
            for (int time = timeStart; time < timeEnd; time++)
            {
                for (int station = 0; station < stopNumber; station++)
                    if (time != 0)
                        //Испарение феромона
                        feromons[routeNumber - 1, station, time] = feromons[routeNumber - 1, station, time - 1] * (1 - feromonVelocity);
                for (int i = 0; i < departureTime.Count(); i++)
                {
                    if (vehicleNumber > 0 && departureTime[i] == time)
                    {
                        vehicleNumber--;
                        
                    }
            }
            }

        }

    }
}
