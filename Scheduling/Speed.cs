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
    
    public partial class Speed
    {
        public System.Guid Start_StopID { get; set; }
        public string PeriodOfTime { get; set; }
        public bool Direction { get; set; }
        public double VehicleSpeed { get; set; }
    
        public virtual Stop Stop { get; set; }
    }
}
