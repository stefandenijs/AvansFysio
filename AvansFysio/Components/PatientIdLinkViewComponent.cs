using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace AvansFysio.Components
{
    public class PatientIdLinkViewComponent : ViewComponent
    {
        private readonly IPatientService _patientService;

        public PatientIdLinkViewComponent(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public IViewComponentResult Invoke()
        {
            var id = GetPatientId();
            return View(id);
        }

        private int GetPatientId()
        {
            var id =  _patientService.GetPatients()
                .SingleOrDefault(p => 
                    p.Email.Equals(HttpContext.User.FindFirstValue(ClaimTypes.Email))).Id;
            return id;
        }
    }
}