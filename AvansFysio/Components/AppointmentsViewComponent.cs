using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansFysio.Models;
using AvansFysio.Models.Appointments;
using Core.Domain.People;
using Core.Domain.Time;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace AvansFysio.Components
{
    public class AppointmentsViewComponent : ViewComponent
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsViewComponent(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public IViewComponentResult Invoke(int PersonId)
        {
            var appointments = new List<AppointmentsViewModel>();

            if (HttpContext.User.HasClaim(c => c.Type == "Claim.Patient"))
            {
                appointments = GetPatientAppointments(PersonId);
            }
            else
            {
                appointments = GetWorkerAppointments(PersonId);
            }
            return View(appointments);
        }

        private List<AppointmentsViewModel> GetPatientAppointments(int id)
        {
            var appointments = _appointmentService.GetAllPatientAppointments(id)
                .Where(a => a.StartTime >= DateTime.Now).ToViewModel();
            appointments.Sort((x, y) => DateTime.Compare(x.StartTime, y.StartTime));
            return appointments;
        }

        private List<AppointmentsViewModel> GetWorkerAppointments(int id)
        {
            var appointments = _appointmentService.GetAllWorkerAppointments(id)
                .Where(a => a.StartTime >= DateTime.Now).ToViewModel();
            appointments.Sort((x, y) => DateTime.Compare(x.StartTime, y.StartTime));
            return appointments;
        }
    }
}
