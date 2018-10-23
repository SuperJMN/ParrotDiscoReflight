using System;
using System.Collections.Generic;

namespace Reflight.Core
{
    public class Flight
    {
        public ICollection<Status> Statuses { get; set; }
        public DateTimeOffset Date { get; set; }
        public TimeSpan RunTime { get; set; }
        public TimeSpan TotalRunTime { get; set; }
    }
}