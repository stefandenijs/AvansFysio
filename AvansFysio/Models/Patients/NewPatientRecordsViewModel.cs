using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Domain;
using Core.Domain.People;

namespace AvansFysio.Models.Patients
{
    public class NewPatientRecordsViewModel
    {
        public int PatientId { get; set; }
        [Required(ErrorMessage = "Geef een omschrijving klachten.")]
        public string ProblemDescription { get; set; }
        [Required(ErrorMessage = "Kies diagnosecode.")]
        public string BodyLocalization { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Selecteer een diagnose.")]
        public int DiagnoseId { get; set; }
        [Required(ErrorMessage = "Kies medewerker die de intake heeft afgehandeld.")]
        [Range(0, int.MaxValue, ErrorMessage = "Kies medewerker die de intake heeft afgehandeld.")]
        public int IntakeHandlerId { get; set; }
        [Range(0, int.MaxValue)]
        public int IntakeSupervisorId { get; set; }
        [Required(ErrorMessage = "Kies een hoofdbehandelaar.")]
        [Range(0, int.MaxValue, ErrorMessage = "Kies een hoofdbehandelaar.")]
        public int HeadPractitionerId { get; set; }
        [Required(ErrorMessage = "Voer in datum van aanmelding.")]
        public DateTime? DateOfRegistration { get; set; }
        [Required(ErrorMessage = "Voer in datum van afmelding.")]
        public DateTime? DateOfDismissal { get; set; }
        [Required(ErrorMessage = "Voer in maximaal aantal sessies behandeling per week.")]
        [Range(1, 5, ErrorMessage = "Voer in maximaal aantal sessies behandeling per week. Maximaal maar 5 sessies per week.")]
        public int SessionsPerWeek { get; set; }
        [Required(ErrorMessage = "Voer in aantal minuten duratie per sessie.")]
        [Range(15, 60, ErrorMessage = "Voer in aantal minuten duratie per sessie. Een sessie mag duren van 15 t/m 60 minuten.")]
        public int SessionsDuration { get; set; }
    }
}
