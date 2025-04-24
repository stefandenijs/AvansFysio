using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvansFysio.Models.Components;
using Core.DomainServices;
using Infrastructure;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace AvansFysio.Components
{
    public class TotalPatientsAppointmentsViewComponent : ViewComponent
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IWorkerService _workerService;

        public TotalPatientsAppointmentsViewComponent(IPatientService patientService, IAppointmentService appointmentService, IWorkerService workerService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _workerService = workerService;
        }

        public IViewComponentResult Invoke(string email)
        {
            var totalCount = _patientService.GetPatients().Count();
            var appointments = GetCountAppointmentsToday(email);
            var model = new TotalPatientsAppointmentsViewModel
            {
                PatientCount = totalCount,
                AppointmentsCount = appointments
            };
            return View(model);
        }

        private int GetCountAppointmentsToday(string email)
        {
            var workerId = _workerService.GetWorkerByEmail(email).Id;
            var count = _appointmentService.GetAllWorkerAppointments(workerId).Count(a => a.StartTime.Date == DateTime.Now.Date && a.StartTime.TimeOfDay > DateTime.Now.TimeOfDay);
            return count;
        }
    }
}
