//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scheduling
{
    using System;
    using System.Collections.Generic;
    
    public partial class Passenger_Flow
    {
        public System.Guid StopID { get; set; }
        public System.Guid RouteID { get; set; }
        public bool Direction { get; set; }
        public string PeriodOfTime { get; set; }
        public int Quantity { get; set; }
    
        public virtual Route Route { get; set; }
    }
}
