using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansFysio.Models.Appointments
{
    public class AppointmentsViewModel
    {
        public int Id { get; set; }
        public string Practitioner { get; set; }
        public int PatientId { get; set; }
        public string Patient { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Treatment { get; set; }
    }
}
