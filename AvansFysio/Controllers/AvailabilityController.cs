using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AvansFysio.Models.Availability;
using Core.Domain.Time;
using Core.DomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AvansFysio.Controllers
{
    [Authorize(Policy = "RequireWorker")]
    public class AvailabilityController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAvailabilityService _availabilityService;
        private readonly IWorkerService _workerService;

        public AvailabilityController(ILogger<HomeController> logger, IAvailabilityService availabilityService, IWorkerService workerService)
        {
            _logger = logger;
            _availabilityService = availabilityService;
            _workerService = workerService;
        }
        
        [HttpGet]
        public IActionResult AvailabilityForm()
        {
            var workerId = _workerService.GetWorkerByEmail(User.FindFirstValue(ClaimTypes.Email)).Id;
            var availabilities = _availabilityService.GetAvailabilities(workerId);
            var model = new NewAvailabilitiesViewModel();
            if (availabilities.Count > 0)
            {
                model.Monday = availabilities[0].Available;
                model.MondayStartTime = availabilities[0].StartTime;
                model.MondayEndTime = availabilities[0].EndTime;
                model.Tuesday = availabilities[1].Available;
                model.TuesdayStartTime = availabilities[1].StartTime;
                model.TuesdayEndTime = availabilities[1].EndTime;
                model.Wednesday = availabilities[2].Available;
                model.WednesdayStartTime = availabilities[2].StartTime;
                model.WednesdayEndTime = availabilities[2].EndTime;
                model.Thursday = availabilities[3].Available;
                model.ThursdayStartTime = availabilities[3].StartTime;
                model.ThursdayEndTime = availabilities[3].EndTime;
                model.Friday = availabilities[4].Available;
                model.FridayStartTime = availabilities[4].StartTime;
                model.FridayEndTime = availabilities[4].EndTime;
            }
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AvailabilityForm(NewAvailabilitiesViewModel availabilitiesModel)
        {
            var wrongTimes = false;
            var requiredInputsFilled = true;
            if (availabilitiesModel.Monday)
            {
                if (availabilitiesModel.MondayStartTime == null)
                {
                    requiredInputsFilled = false;
                    ModelState.AddModelError(nameof(availabilitiesModel.MondayStartTime), "");
                }
                if (availabilitiesModel.MondayEndTime == null)
                {
                    requiredInputsFilled = false;
                    ModelState.AddModelError(nameof(availabilitiesModel.MondayEndTime), "");
                }

                if (availabilitiesModel.MondayEndTime < availabilitiesModel.MondayStartTime)
                {
                    if (!wrongTimes)
                    {
                        wrongTimes = true;
                    }
                    ModelState.AddModelError(nameof(availabilitiesModel.MondayEndTime), "");
                }
            }

            if (availabilitiesModel.Tuesday)
            {
                if (availabilitiesModel.TuesdayStartTime == null)
                {
                    requiredInputsFilled = false;
                    ModelState.AddModelError(nameof(availabilitiesModel.TuesdayStartTime), "");
                }
                if (availabilitiesModel.TuesdayEndTime == null)
                {
                    requiredInputsFilled = false;
                    ModelState.AddModelError(nameof(availabilitiesModel.TuesdayEndTime), "");
                }

                if (availabilitiesModel.TuesdayEndTime < availabilitiesModel.TuesdayStartTime)
                {
                    if (!wrongTimes)
                    {
                        wrongTimes = true;
                    }
                    ModelState.AddModelError(nameof(availabilitiesModel.TuesdayEndTime), "");
                }
            }
            if (availabilitiesModel.Wednesday)
            {
                if (availabilitiesModel.WednesdayStartTime == null)
                {
                    requiredInputsFilled = false;
                    ModelState.AddModelError(nameof(availabilitiesModel.WednesdayStartTime), "");
                }
                if (availabilitiesModel.WednesdayEndTime == null)
                {
                    requiredInputsFilled = false;
                    ModelState.AddModelError(nameof(availabilitiesModel.WednesdayEndTime), "");
                }

                if (availabilitiesModel.WednesdayEndTime < availabilitiesModel.WednesdayStartTime)
                {
                    if (!wrongTimes)
                    {
                        wrongTimes = true;
                    }
                    ModelState.AddModelError(nameof(availabilitiesModel.WednesdayEndTime), "");
                }
            }
            if (availabilitiesModel.Thursday)
            {
                if (availabilitiesModel.ThursdayStartTime == null)
                {
                    requiredInputsFilled = false;
                    ModelState.AddModelError(nameof(availabilitiesModel.ThursdayStartTime), "");
                }
                if (availabilitiesModel.ThursdayEndTime == null)
                {
                    requiredInputsFilled = false;
                    ModelState.AddModelError(nameof(availabilitiesModel.ThursdayEndTime), "");
                }

                if (availabilitiesModel.ThursdayEndTime < availabilitiesModel.ThursdayStartTime)
                {
                    if (!wrongTimes)
                    {
                        wrongTimes = true;
                    }
                    ModelState.AddModelError(nameof(availabilitiesModel.ThursdayEndTime), "");
                }
            }
            if (availabilitiesModel.Friday)
            {
                if (availabilitiesModel.FridayStartTime == null)
                {
                    requiredInputsFilled = false;
                    ModelState.AddModelError(nameof(availabilitiesModel.FridayStartTime), "");
                }
                if (availabilitiesModel.FridayEndTime == null)
                {
                    requiredInputsFilled = false;
                    ModelState.AddModelError(nameof(availabilitiesModel.FridayEndTime), "");
                }

                if (availabilitiesModel.FridayEndTime < availabilitiesModel.FridayStartTime)
                {
                    if (!wrongTimes)
                    {
                        wrongTimes = true;
                    }
                    ModelState.AddModelError(nameof(availabilitiesModel.FridayEndTime), "");
                }
            }

            if (wrongTimes)
            {
                ModelState.AddModelError(string.Empty, "Eind tijd mag niet voor start tijd vallen.");
            }

            if (!requiredInputsFilled)
            {
                ModelState.AddModelError(string.Empty, "Voer in alle tijdsvlakken voor beschikbare dagen.");
            }

            if (ModelState.IsValid)
            {
                var workerId = _workerService.GetWorkerByEmail(User.FindFirstValue(ClaimTypes.Email)).Id;
                var availabilities = _availabilityService.GetAvailabilities(workerId) ?? new List<Availability>() { };

                if (availabilities.Count == 0)
                {
                    var newAvailabilities = new List<Availability>()
                    {
                        new Availability() { Day = DayOfWeek.Monday, WorkerId = workerId },
                        new Availability() { Day = DayOfWeek.Tuesday, WorkerId = workerId },
                        new Availability() { Day = DayOfWeek.Wednesday, WorkerId = workerId },
                        new Availability() { Day = DayOfWeek.Thursday, WorkerId = workerId },
                        new Availability() { Day = DayOfWeek.Friday, WorkerId = workerId }
                    };
                    foreach (var availability in newAvailabilities)
                    {
                        await _availabilityService.AddAvailability(availability);
                    }
                    availabilities = _availabilityService.GetAvailabilities(workerId);
                }

                var monday = availabilities.SingleOrDefault(a => a.Day == DayOfWeek.Monday);
                await UpdateAvailability(monday, availabilitiesModel.Monday,
                    DayOfWeek.Monday, availabilitiesModel.MondayStartTime, availabilitiesModel.MondayEndTime);

                var tuesday = availabilities.SingleOrDefault(a => a.Day == DayOfWeek.Tuesday);
                await UpdateAvailability(tuesday, availabilitiesModel.Tuesday,
                    DayOfWeek.Tuesday, availabilitiesModel.TuesdayStartTime, availabilitiesModel.TuesdayEndTime);

                var wednesday = availabilities.SingleOrDefault(a => a.Day == DayOfWeek.Wednesday);
                await UpdateAvailability(wednesday, availabilitiesModel.Wednesday,
                    DayOfWeek.Wednesday, availabilitiesModel.WednesdayStartTime, availabilitiesModel.WednesdayEndTime);

                var thursday = availabilities.SingleOrDefault(a => a.Day == DayOfWeek.Thursday);
                await UpdateAvailability(thursday, availabilitiesModel.Thursday,
                    DayOfWeek.Thursday, availabilitiesModel.ThursdayStartTime, availabilitiesModel.ThursdayEndTime);

                var friday = availabilities.SingleOrDefault(a => a.Day == DayOfWeek.Friday);
                await UpdateAvailability(friday, availabilitiesModel.Friday,
                    DayOfWeek.Friday, availabilitiesModel.FridayStartTime, availabilitiesModel.FridayEndTime);

                return RedirectToAction("Index", "Home");
            }

            return View(availabilitiesModel);
        }

        private async Task UpdateAvailability(Availability availability, bool available, DayOfWeek dayOfWeek, DateTime? startTime, DateTime? endTime)
        {
            if (!available)
            {
                startTime = null;
                endTime = null;
            }
            availability.Available = available;
            availability.StartTime = startTime;
            availability.EndTime = endTime;
            await _availabilityService.UpdateAvailability(availability);
        }
    }
}
