using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduling
{
    public class AntColonyClass
    {
        public static double feromonVelocity { get; set; }  //Скорость испарения феромона
        public static double feromonWeight { get; set; }   //Весовой коэффициент для феромона
        public static double visionWeight { get; set; } //весовой коэффициент для видимости
        public static double hoursNumber { get; set; } //временной участок в часах
        private static int passengersAll { get; set; }  //общее количество перевезенных пассажиров
        private static int timeStart;  //Момент начала работы ТС
        private static int timeEnd;  //ТС ездят N часов
        private static int stopNumber; //Количество остановок на маршруте
        private static int routeNumber; //Количество маршрутов для рассмотрения 
        private static int amountOfPossibleIntervals; //Количество возможных вариантов интервалов
        private static double[,,] possibleIntervals;
        private static double[,] timeOpt;
        private static double variation; //Отклонение времени движения от оптимального
        private static int[] departureTime;//Массив моментов выхода ТС с начальной остановки
        private static List<Bus> buses; //Список автобусов, работающих на маршруте
        //Количество людей, заходящих на остановки в каждый момент времени
        private static int[,] peopleAmount;
        //Количество выходящих на остановках людей
        private static int[,] peopleAmountOut;
        private static double[,] timeTable; //Расписание
        //Количество феромона на остоновках в каждый момент времени
        private static double[,,] feromons;
        public static int vehicleNumber { get; set; }

        public static void InitializeVariables(int stopnumber)
        {
            timeEnd = (int)(hoursNumber * 60);
            timeStart = 0;
            stopNumber = stopnumber;
            routeNumber = 1;
            amountOfPossibleIntervals = 9;
            possibleIntervals = new double[routeNumber, stopNumber - 1, amountOfPossibleIntervals];
            timeOpt = new double[routeNumber, stopNumber - 1];
            variation = 0.2;
            departureTime = new int[vehicleNumber];
            buses = new List<Bus>();
            peopleAmount = new int[timeEnd, stopNumber];
            peopleAmountOut = new int[timeEnd, stopNumber];
            timeTable = new double[departureTime.Length, stopNumber];
            feromons = new double[routeNumber, stopNumber, timeEnd];
        }
        //Инициализируются массивы, описывающие оптимальные интервалы и возможные интервалы
        public static void InitializeIntervals(double?[] optTimes)
        {
            for (int i = 0; i < stopNumber - 1; i++)
            {
                timeOpt[routeNumber - 1, i] = (double)optTimes[i]; //Задаем оптимальный интервал - 3 минуты
                for (int j = 0; j < amountOfPossibleIntervals; j++)
                {
                    possibleIntervals[routeNumber-1, i, j] = timeOpt[routeNumber - 1, i] * (1 - variation) +
                        j * 2 * variation * timeOpt[routeNumber - 1, i] / amountOfPossibleIntervals;
                }
            }
        }

        public static void InitializeDepartureTime()
        {
            for (int i = 0; i < departureTime.Length;  i++)
                departureTime[i] = i * 5+5;
        }

        //Инициализируется массив, отвечающий за количество человек, заходящих в ТС в каждый момент времени, на каждой остановке
        public static void InitializePeopleInBus(int?[] peopleIN, int?[] peopleOUT)
        {
            for (int i = 0; i < timeEnd; i++)
                for (int j = 0; j < stopNumber; j++)
                {
                    peopleAmount[i, j] = (int)peopleIN[j];
                    peopleAmountOut[i, j] = (int)peopleOUT[j];
                }
        }
        //Инициализируются феромоны. Изначально пассажиры не собраны и феромоны не отложены
        public static void InitializeFeromons()
        {
            for (int i = 0; i < stopNumber; i++)
                feromons[routeNumber - 1, i, timeStart] = 0;
        }

        private static double GetFeromoneAtTime(double startF, double interval)
        {
            return startF * Math.Pow(1 - feromonVelocity, interval);
        }

        public static void StartBuses()
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
                        timeTable[bus.Number - 1, bus.NextStation - 1] = bus.NextTime;                        
                        buses.Add(bus);
                    }
                }
                for (int k = 0; k < buses.Count; k++)
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
                            //Массив текущих интервалов
                            var currentInterval = new double[amountOfPossibleIntervals];
                            //Массив видимостей
                            var visibility = new double[amountOfPossibleIntervals];
                            //Массив феромонов
                            var feromone = new double[amountOfPossibleIntervals];
                            //Максимальная вероятность
                            double possibilityMax = 0;
                            //Максимальный интервал. По умолчанию равен оптимальному
                            double intervalMax = timeOpt[routeNumber - 1, bus.NextStation - 1];
                            for (int interval = 0; interval < amountOfPossibleIntervals; interval++)
                            {
                                //Берется текущий интервал
                                currentInterval[interval] = possibleIntervals[routeNumber - 1, bus.NextStation - 1, interval];
                                if (Math.Abs(currentInterval[interval] - timeOpt[routeNumber - 1, bus.NextStation - 1]) <= 0.1)
                                    //Зачем прибавлять 0,1?!
                                    currentInterval[interval] = timeOpt[routeNumber - 1, bus.NextStation - 1] + 0.1;
                                //Расчитываем видимость для каждого интервала
                                visibility[interval] = Math.Pow(1 / Math.Abs(timeOpt[routeNumber - 1, bus.NextStation - 1] -
                                    currentInterval[interval]), visionWeight);
                                //Считаем феромоны
                                feromone[interval] = GetFeromoneAtTime(feromons[routeNumber - 1, bus.NextStation-1, time], currentInterval[interval]);
                                //Находим сумму
                                sum += visibility[interval] * Math.Pow(feromone[interval], feromonWeight);
                            }

                            for (int interval = 0; interval < amountOfPossibleIntervals; interval++)
                            {
                                //Расчитываем вероятность для каждого интервала
                                possibility[interval] = visibility[interval] * Math.Pow(feromone[interval],
                                    feromonWeight) / sum;
                                //Находим интервал с максимальной вероятностью и запоминаем
                                if (possibility[interval] > possibilityMax)
                                {
                                    possibilityMax = possibility[interval];
                                    intervalMax = possibleIntervals[routeNumber - 1, bus.NextStation - 1,
                                        interval];
                                }
                            }

                            if ((new Random()).NextDouble() <= possibilityMax)
                                bus.NextTime = time + intervalMax;
                            else
                                bus.NextTime = time + timeOpt[routeNumber - 1, bus.NextStation - 1];
                        }
                        timeTable[bus.Number - 1, bus.NextStation - 1] = bus.NextTime;
                    }
                }

            }

        }

        public static void StartBusesReverse()
        {
            for (int time = timeEnd-1; time >= timeStart; time--)
            {
                for (int station = 0; station < stopNumber; station++)
                {
                    if (time != 0)
                    {
                        feromons[routeNumber - 1, station, time] = feromons[routeNumber - 1, station, time - 1] * (1 - feromonVelocity);
                    }
                }
                for (int i = departureTime.Count()-1; i >= 0; i--)
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
                            NextTime = departureTime[i] - timeOpt[0, 1],
                            Capacity = 42
                        };
                        timeTable[bus.Number - 1, bus.NextStation - 1] = bus.NextTime;
                        buses.Add(bus);
                    }
                }
                for (int k = 0; k < buses.Count; k++)
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
                            //Массив текущих интервалов
                            var currentInterval = new double[amountOfPossibleIntervals];
                            //Массив видимостей
                            var visibility = new double[amountOfPossibleIntervals];
                            //Массив феромонов
                            var feromone = new double[amountOfPossibleIntervals];
                            //Максимальная вероятность
                            double possibilityMax = 0;
                            //Максимальный интервал. По умолчанию равен оптимальному
                            double intervalMax = timeOpt[routeNumber - 1, bus.NextStation - 1];
                            for (int interval = 0; interval < amountOfPossibleIntervals; interval++)
                            {
                                //Берется текущий интервал
                                currentInterval[interval] = possibleIntervals[routeNumber - 1, bus.NextStation - 1, interval];
                                if (Math.Abs(currentInterval[interval] - timeOpt[routeNumber - 1, bus.NextStation - 1]) <= 0.1)
                                    //Зачем прибавлять 0,1?!
                                    currentInterval[interval] = timeOpt[routeNumber - 1, bus.NextStation - 1] + 0.1;
                                //Расчитываем видимость для каждого интервала
                                visibility[interval] = Math.Pow(1 / Math.Abs(timeOpt[routeNumber - 1, bus.NextStation - 1] -
                                    currentInterval[interval]), visionWeight);
                                //Считаем феромоны
                                feromone[interval] = GetFeromoneAtTime(feromons[routeNumber - 1, bus.NextStation-1, time], currentInterval[interval]);
                                //Находим сумму
                                sum += visibility[interval] * Math.Pow(feromone[interval], feromonWeight);
                            }

                            for (int interval = 0; interval < amountOfPossibleIntervals; interval++)
                            {
                                //Расчитываем вероятность для каждого интервала
                                possibility[interval] = visibility[interval] * Math.Pow(feromone[interval],
                                    feromonWeight) / sum;
                                //Находим интервал с максимальной вероятностью и запоминаем
                                if (possibility[interval] > possibilityMax)
                                {
                                    possibilityMax = possibility[interval];
                                    intervalMax = possibleIntervals[routeNumber - 1, bus.NextStation - 1,
                                        interval];
                                }
                            }

                            if ((new Random()).NextDouble() <= possibilityMax)
                                bus.NextTime = time - intervalMax;
                            else
                                bus.NextTime = time - timeOpt[routeNumber - 1, bus.NextStation - 1];
                        }
                        timeTable[bus.Number - 1, bus.NextStation - 1] = bus.NextTime;
                    }
                }
            }
        }
    }
}
