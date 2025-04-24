using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain.People
{
    public class Physiotherapist : Worker
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string EmployeeNumber { get; set; }
        [Required]
        public string BigNumber { get; set; }
        public ICollection<PatientRecord.PatientRecord> HeadPractitioner { get; set; }
        public ICollection<PatientRecord.PatientRecord> Supervisor { get; set; }
    }
}
