using System;
using System.ComponentModel.DataAnnotations;
using Core.Domain.People;

namespace Core.Domain.Time
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PractitionerId { get; set; }
        public Worker Practitioner { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public DateTime EndTime { get; set; }
        public Treatment.Treatment Treatment { get; set; }
    }
}
