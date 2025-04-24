using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Domain.Time;
using Core.Domain.Treatment;

namespace Core.Domain.People
{
    public class Worker
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public ICollection<PatientRecord.PatientRecord> Intake { get; set; }
        public ICollection<Availability> Availabilities { get; set; }
    }
}
