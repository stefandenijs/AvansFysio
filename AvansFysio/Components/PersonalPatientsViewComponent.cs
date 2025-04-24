using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AvansFysio.Models;
using AvansFysio.Models.Patients;
using Core.Domain.People;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace AvansFysio.Components
{
    public class PersonalPatientsViewComponent : ViewComponent
    {
        private readonly IWorkerService _workerService;
        private readonly IPatientService _patientService;

        public PersonalPatientsViewComponent(IWorkerService workerService, IPatientService patientService)
        {
            _workerService = workerService;
            _patientService = patientService;
        }

        public IViewComponentResult Invoke(int workerId)
        {
            var patients = GetPatients(workerId).ToViewModel();
            return View(patients);
        }

        private List<Patient> GetPatients(int workerId)
        {
            return _patientService.GetPhysiotherapistPatients(workerId);
        }
    }
}
