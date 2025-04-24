using System;
using System.ComponentModel.DataAnnotations;

namespace AvansFysio.Models.Treatments
{
    public class UpdateTreatmentsViewModel
    {
        public int PatientId { get; set; }
        public int TreatmentId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Selecteer een behandeling.")]
        public int TreatmentInfoId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Selecteer een behandelruimte.")]
        public int RoomId { get; set; }
        public string Particularities { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Voer in behanderlaar van behandeling.")]
        public int TreatmentPerformedById { get; set; }
        [Required(ErrorMessage = "Voer in datum van behandeling.")]
        public DateTime? TreatmentPerformedDate { get; set; }
    }
}
