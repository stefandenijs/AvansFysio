using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansFysio.Models.Appointments
{
    public class UpdateAppointmentsViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Selecteer een patiënt.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecteer een patiënt.")]
        public int PatientId { get; set; }
        [Required(ErrorMessage = "Selecteer een behandelaar.")]
        [Range(1, int.MaxValue, ErrorMessage = "Selecteer een behandelaar.")]
        public int PractitionerId { get; set; }
        [Required(ErrorMessage = "Kies een begin tijd.")]
        [DataType(DataType.Time)]
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
