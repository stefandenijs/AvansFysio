using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansFysio.Models.Appointments
{
    public class AppointmentsAddTreatmentViewModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Selecteer een behandeling.")]
        public int TreatmentId { get; set; }
    }
}
