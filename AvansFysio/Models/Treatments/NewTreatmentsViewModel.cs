using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.People;

namespace AvansFysio.Models.Treatments
{
    public class NewTreatmentsViewModel
    {
        public int PatientId { get; set; }
        public int AppointmentId { get; set; }
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
        public bool IsIntern { get; set; }
    }
}
