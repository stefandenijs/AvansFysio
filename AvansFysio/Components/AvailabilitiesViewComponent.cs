using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AvansFysio.Models;
using Core.Domain.Time;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace AvansFysio.Components
{
    public class AvailabilitiesViewComponent : ViewComponent
    {
        private readonly IAvailabilityService _availabilityService;
        private readonly IWorkerService _workerService;

        public AvailabilitiesViewComponent(IAvailabilityService availabilityService, IWorkerService workerService)
        {
            _availabilityService = availabilityService;
            _workerService = workerService;
        }

        public IViewComponentResult Invoke(int workerId)
        {
            var availabilities = GetAvailabilities(workerId).ToViewModel();
            return View(availabilities);
        }

        private List<Availability> GetAvailabilities(int workerId)
        {
            return _availabilityService.GetAvailabilities(workerId);
        }
    }
}
