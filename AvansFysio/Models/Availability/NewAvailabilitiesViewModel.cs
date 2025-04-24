using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AvansFysio.Models.Availability
{
    public class NewAvailabilitiesViewModel
    {
        public bool Monday { get; set; }
        public DateTime? MondayStartTime { get; set; }
        public DateTime? MondayEndTime { get; set; }
        public bool Tuesday { get; set; }
        public DateTime? TuesdayStartTime { get; set; }
        public DateTime? TuesdayEndTime { get; set; }
        public bool Wednesday { get; set; }
        [DataType(DataType.Time)]
        public DateTime? WednesdayStartTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime? WednesdayEndTime { get; set; }
        public bool Thursday { get; set; }
        public DateTime? ThursdayStartTime { get; set; }
        public DateTime? ThursdayEndTime { get; set; }
        public bool Friday { get; set; }
        public DateTime? FridayStartTime { get; set; }
        public DateTime? FridayEndTime { get; set; }
    }
}
