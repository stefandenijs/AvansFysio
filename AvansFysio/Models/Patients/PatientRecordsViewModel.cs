using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansFysio.Models.Patients
{
    public class PatientRecordsViewModel
    {
        public int PatientRecordId { get; set; }
        public int Age { get; set; }
        public string MedicalComplaintsDescription { get; set; }
        public string DiagnosticCode { get; set; }
        public string DiagnoseDescription { get; set; }
        public string IntakeHandler { get; set; }
        public string IntakeSupervisor { get; set; }
        public int HeadPractitionerId { get; set; }
        public string HeadPractitioner { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public DateTime DateOfDismissal { get; set; }
        public int SessionsPerWeek { get; set; }
        public int SessionsDuration { get; set; }
    }
}
