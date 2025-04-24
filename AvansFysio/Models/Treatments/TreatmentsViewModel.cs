using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansFysio.Models.Treatments
{
    public class TreatmentsViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Patient { get; set; }
        public string Code { get; set; }
        public string Practitioner { get; set; }
        public string Room { get; set; }
        public string Description { get; set; }
        public string Particularities { get; set; }
        public DateTime PerformedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
