using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduling
{
    public class Bus
    {
        public int Capacity { get; set; }
        public int PassengersInside { get; set; }
        public int Number { get; set; }
        public double NextTime { get; set; }
        public int NextStation { get; set; }
        
    }
}
