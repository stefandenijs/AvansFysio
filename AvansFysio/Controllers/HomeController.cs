using AvansFysio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Core.DomainServices;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace AvansFysio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPatientService _patientService;
        private readonly IWorkerService _workerService;

        public HomeController(ILogger<HomeController> logger, IPatientService patientService, IWorkerService workerService)
        {
            _logger = logger;
            _patientService = patientService;
            _workerService = workerService;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (User.HasClaim(c => c.Type == "Claim.Patient"))
            {
                @ViewBag.UserId = _patientService.GetPatients()
                    .SingleOrDefault(p => p.Email.Equals(User.FindFirstValue(ClaimTypes.Email))).Id;
            }

            if (User.HasClaim(c => c.Type.Equals("Claim.Physiotherapist") || c.Type.Equals("Claim.Intern")))
            {
                var worker = _workerService.GetWorkerByEmail(User.FindFirstValue(ClaimTypes.Email)).ToViewModel();
                return View("~/Areas/Employee/Views/Home/Index.cshtml", worker);
            }
            else
            {
                var patient = _patientService.GetPatientByEmail(User.FindFirstValue(ClaimTypes.Email)).ToViewModel();
                return View(patient);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
