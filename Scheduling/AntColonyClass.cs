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
        static double passengersSpeedOut { get; set; }//Скорость выхода пассажиров из ТС, чел./час (чел./мин.)
        private static int passengersAll;  //Общее количество перевезенных пассажиров
        private static int timeStart = 0;  //Момент начала работы ТС
        private static int timeEnd = (int)(hoursNumber * 60);  //ТС ездят 10 часов
        private static int stopNumber = 29; //Количество остановок на маршруте
        private static int routeNumber = 1; //Количество маршрутов для рассмотрения 
        private static int amountOfPossibleIntervals = 9; //Количество возможных вариантов интервалов
        private static double[,,] possibleIntervals = new double[routeNumber, stopNumber - 1, amountOfPossibleIntervals];
        private static double[,] timeOpt = new double[routeNumber, stopNumber - 1];
        private static double variation = 0.2; //Отклонение времени движения от оптимального
        private static int[] departureTime { get; set; } //Массив моментов выхода ТС с начальной остановки
        private static List<Bus> buses = new List<Bus>(); //Список автобусов, работающих на маршруте
        //Количество людей, заходящих на остановки в каждый момент времени
        private static int[,] peopleAmount = new int[timeEnd, stopNumber];
        //Количество выходящих на остановках людей
        private static int[,] peopleAmountOut = new int[timeEnd, stopNumber];
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
                {
                    peopleAmount[i, j] = (int)Math.Round(passengersSpeed);
                    peopleAmountOut[i, j] = (int)Math.Round(passengersSpeedOut);
                }
        }
        //Инициализируются феромоны. Изначально пассажиры не собраны и феромоны не отложены
        static void InitializeFeromons()
        {
            for (int i = 0; i < vehicleNumber; i++)
                feromons[routeNumber - 1, i, timeStart] = 0;
        }

        static double GetFeromoneAtTime(double startF, double interval)
        {
            return startF * Math.Pow(1 - feromonVelocity, interval);
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
                    if (vehicleNumber > 0 && departureTime[i] == time) //автобус едет, если есть свободное ТС и его время пришло
                    {
                        vehicleNumber--;
                        int prevNumber;
                        if (buses.Count == 0) prevNumber = 0;
                        else prevNumber = buses.Last().Number;
                        var bus = new Bus
                        {
                            Number = prevNumber + 1,
                            NextStation = 1,
                            NextTime = departureTime[i] + timeOpt[0, 1],
                            Capacity = 42
                        };
                        buses.Add(bus);
                    }
                }
                for (int k=0; k<buses.Count; k++)
                {
                    var bus = buses[k];
                    if (time == (int)Math.Round(bus.NextTime)) //Автобус прибыл на остановку
                    {
                        //Пассажиры выходят
                        bus.PassengersInside -= Math.Min(bus.PassengersInside, peopleAmountOut[time, bus.NextStation]);
                        if (bus.PassengersInside + peopleAmount[time, bus.NextStation] <= bus.Capacity)
                        {
                            passengersAll += peopleAmount[time, bus.NextStation];
                            //Увеличиваем количество феромона
                            feromons[routeNumber - 1, bus.NextStation, time] += peopleAmount[time, bus.NextStation];
                            //Люди зашли в автобус
                            bus.PassengersInside += peopleAmount[time, bus.NextStation];
                        }
                        else
                        {
                            //Кто смог, тот и зашел
                            passengersAll += bus.Capacity - bus.PassengersInside;
                            //Феромон увеличился
                            feromons[routeNumber - 1, bus.NextStation, time] += bus.Capacity - bus.PassengersInside;
                            bus.PassengersInside = bus.Capacity; //Автобус заполнен
                        }
                        bus.NextStation++; // Двигаемся к следующей остановке
                        if (bus.NextStation >= stopNumber)
                        {
                            buses.Remove(bus);
                            vehicleNumber++;
                        }
                        else
                        {
                            var possibility = new double[amountOfPossibleIntervals]; //Вероятность поехать с другим интервалом
                            double sum = 0;
                            var currentInterval = new double[amountOfPossibleIntervals];
                            var visibility = new double[amountOfPossibleIntervals];
                            var feromone = new double[amountOfPossibleIntervals];
                            double possibilityMax = 0;
                            double intervalMax = timeOpt[routeNumber-1, bus.NextStation-1];
                            for (int interval = 0; interval < amountOfPossibleIntervals; interval++)
                            {
                                currentInterval[interval] = possibleIntervals[routeNumber - 1, bus.NextStation - 1, interval];
                                if (Math.Abs(currentInterval[interval] - timeOpt[routeNumber - 1, bus.NextStation - 1]) <= 0.1)
                                    currentInterval[interval] = timeOpt[routeNumber - 1, bus.NextStation - 1] + 0.1;
                                visibility[interval] = Math.Pow(1/Math.Abs(timeOpt[routeNumber-1, bus.NextStation-1]-
                                    currentInterval[interval]), visionWeight);
                                feromone[interval] = GetFeromoneAtTime(feromons[routeNumber-1, bus.NextStation,time], currentInterval[interval]);
                                sum += visibility[interval] * Math.Pow(feromone[interval], feromonWeight);
                            }
                            
                            //for(int interval =0; inter)
                        }

                    }
                }

            }

        }

    }
}
