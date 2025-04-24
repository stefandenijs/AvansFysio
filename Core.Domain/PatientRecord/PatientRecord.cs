using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Domain.People;
using Core.Domain.Treatment;

namespace Core.Domain.PatientRecord
{
    public class PatientRecord
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [Required]
        public int Age { get; set; }
        [Required]
        public string MedicalComplaintsDescription { get; set; }
        [Required]
        public int DiagnoseId { get; set; }
        [Required]
        public string DiagnosticCode { get; set; }
        [Required]
        public string DiagnosticBodyLocalization { get; set; }
        [Required]
        public string DiagnoseDescription { get; set; }

        public int IntakeHandlerId { get; set; }
        public Worker IntakeHandler { get; set; }

        public int? IntakeSupervisorId { get; set; }
        public Physiotherapist IntakeSupervisor { get; set; }

        public int HeadPractitionerId { get; set; }
        public Physiotherapist HeadPractitioner { get; set; }

        [Required]
        public DateTime DateOfRegistration { get; set; }
        [Required]
        public DateTime DateOfDismissal { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public int? TreatmentPlanId { get; set; }
        public TreatmentPlan TreatmentPlan { get; set; }
        public ICollection<Treatment.Treatment> Treatments { get; set; }
    }
}
