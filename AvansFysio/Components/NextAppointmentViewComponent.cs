using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansFysio.Models;
using AvansFysio.Models.Appointments;
using Core.DomainServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvansFysio.Components
{
    public class NextAppointmentViewComponent : ViewComponent
    {

        private readonly IAppointmentService _appointmentService;

        public NextAppointmentViewComponent(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public IViewComponentResult Invoke(int PersonId)
        {
            AppointmentsViewModel appointment;

            if (HttpContext.User.HasClaim(c => c.Type == "Claim.Patient"))
            {
                appointment = GetPatientAppointment(PersonId);
            }
            else
            {
                appointment = GetWorkerAppointment(PersonId);
            }
            return View(appointment);
        }

        private AppointmentsViewModel GetPatientAppointment(int id)
        {
            var appointments = _appointmentService.GetAllPatientAppointments(id)
                .Where(a => a.StartTime >= DateTime.Now).ToViewModel();
            appointments.Sort((x, y) => DateTime.Compare(x.StartTime, y.StartTime));
            AppointmentsViewModel appointment = null;
            if (appointments.Count > 0)
            {
                appointment = appointments[0];
            }
            return appointment;
        }

        private AppointmentsViewModel GetWorkerAppointment(int id)
        {
            var appointments = _appointmentService.GetAllWorkerAppointments(id)
                .Where(a => a.StartTime >= DateTime.Now).ToViewModel();
            appointments.Sort((x, y) => DateTime.Compare(x.StartTime, y.StartTime));
            AppointmentsViewModel appointment = null;
            if (appointments.Count > 0)
            {
                appointment = appointments[0];
            }
            return appointment;
        }
    }
}
