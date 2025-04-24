using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansFysio.Models.Availability
{
    public class AvailabilitiesViewModel
    {
        public string DayOfWeek { get; set; }
        public bool Available { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
