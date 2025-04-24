using System;
using System.ComponentModel.DataAnnotations;
using Core.Domain.People;

namespace Core.Domain.Time
{
    public class Availability
    {
        public int Id { get; set; }
        [Required]
        public DayOfWeek Day { get; set; }
        [Required]
        public bool Available { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
    }
}
