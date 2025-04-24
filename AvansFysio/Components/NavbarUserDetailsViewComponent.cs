using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AvansFysio.Models.Components;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace AvansFysio.Components
{
    public class NavbarUserDetailsViewComponent : ViewComponent
    {
        private readonly IWorkerService _workerService;
        private readonly IPatientService _patientService;

        public NavbarUserDetailsViewComponent(IPatientService patientService , IWorkerService workerService)
        {
            _patientService = patientService;
            _workerService = workerService;
        }

        public IViewComponentResult Invoke()
        {
            var details = GetDetails();

            return View(details);
        }

        private NavUserDetailsViewModel GetDetails()
        {
            var details = new NavUserDetailsViewModel { Name = "Not found", Email = "Not found"};

            if (HttpContext.User.HasClaim(c => c.Type is "Claim.Physiotherapist" or "Claim.Intern"))
            {
                var temp = _workerService.GetAllWorkers()
                    .SingleOrDefault(w => w.Email.Equals(HttpContext.User.FindFirstValue(ClaimTypes.Email)));
                details.Name = temp.Name;
                details.Email = temp.Email;
            } else if (HttpContext.User.HasClaim(c => c.Type is "Claim.Patient"))
            {
                var temp = _patientService.GetPatientByEmail(HttpContext.User.FindFirstValue(ClaimTypes.Email));
                details.Name = temp.Name;
                details.Email = temp.Email;
            }

            return details;
        }
    }
}
