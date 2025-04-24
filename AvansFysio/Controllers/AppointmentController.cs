using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AvansFysio.Models;
using AvansFysio.Models.Appointments;
using Core.Domain;
using Core.Domain.People;
using Core.Domain.Time;
using Core.DomainServices;
using Core.DomainServices.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace AvansFysio.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IWorkerService _workerService;
        private readonly IAvailabilityService _availabilityService;
        private readonly IAppointmentPlanner _appointmentPlanner;
        private readonly ITreatmentService _treatmentService;

        public AppointmentController(ILogger<AppointmentController> logger, IAppointmentService appointmentService,
            IPatientService patientService, IWorkerService workerService, IAvailabilityService availabilityService,
            IAppointmentPlanner appointmentPlanner, ITreatmentService treatmentService)
        {
            _logger = logger;
            _appointmentService = appointmentService;
            _patientService = patientService;
            _workerService = workerService;
            _availabilityService = availabilityService;
            _appointmentPlanner = appointmentPlanner;
            _treatmentService = treatmentService;
        }

        [HttpGet]
        public IActionResult AppointmentWorkerForm(int id, int patientId)
        {
            var model = new NewAppointmentsViewModel
            {
                PractitionerId = id
            };
            if (patientId > 0)
            {
                model.PatientId = patientId;
            }
            PrefillSelections();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequirePhysio")]
        public async Task<IActionResult> AppointmentWorkerForm(NewAppointmentsViewModel appointmentsViewModel)
        {
            var patientRecord = _patientService.GetPatientById(appointmentsViewModel.PatientId).PatientRecord;

            if (patientRecord == null)
            {
                ModelState.AddModelError(nameof(appointmentsViewModel.PatientId), "Niet ingeschreven patienten mogen geen afspraken maken.");
            }

            if (appointmentsViewModel.StartTime < DateTime.Now)
            {
                ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Een afspraak mag niet in het verleden ingepland worden.");
            }

            if (appointmentsViewModel.StartTime != null && patientRecord != null && appointmentsViewModel.PatientId > 0)
            {
                var patient = _patientService.GetPatientById(appointmentsViewModel.PatientId);
                appointmentsViewModel.EndTime = appointmentsViewModel.StartTime.Value.AddMinutes(patient.PatientRecord.TreatmentPlan.SessionDuration);

                if (patient.PatientRecord != null && (appointmentsViewModel.StartTime < patient.PatientRecord.DateOfRegistration ||
                                                      appointmentsViewModel.EndTime < patient.PatientRecord.DateOfRegistration
                                                      || appointmentsViewModel.StartTime > patient.PatientRecord.DateOfDismissal ||
                                                      appointmentsViewModel.EndTime > patient.PatientRecord.DateOfDismissal))
                {
                    ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Een afspraak mag niet buiten behandelsperiode vallen van patiënt.");
                }

                if (!_appointmentPlanner.CheckForAmountOfAppointments(patient.Id, appointmentsViewModel.StartTime.Value,
                    patient.PatientRecord.TreatmentPlan.SessionsPerWeek))
                {
                    ModelState.AddModelError(string.Empty, "Een nieuwe afspraak mag niet het aantal sessies per week overschrijden.");
                }
                else
                {
                    if (!_appointmentPlanner.CheckAvailability(appointmentsViewModel.PractitionerId,
                        appointmentsViewModel.StartTime.Value, appointmentsViewModel.EndTime.Value))
                    {
                        ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Datum en tijd mogen  niet vallen buiten beschikbaardheid van behandelaar.");
                    }
                    else
                    {
                        if (!_appointmentPlanner.CheckForAppointments(appointmentsViewModel.PractitionerId,
                            appointmentsViewModel.StartTime.Value,
                            appointmentsViewModel.EndTime.Value))
                        {
                            ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Er is al een afspraak gepland bij de behandelaar op deze tijd.");
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                await _appointmentService.AddAppointment(new Appointment
                {
                    PatientId = appointmentsViewModel.PatientId,
                    PractitionerId = appointmentsViewModel.PractitionerId,
                    StartTime = (DateTime)appointmentsViewModel.StartTime,
                    EndTime = (DateTime)appointmentsViewModel.EndTime
                });
                return RedirectToAction("Index", "Home");
            }

            PrefillSelections();
            return View(appointmentsViewModel);
        }
        [HttpGet]
        public IActionResult UpdateAppointmentWorkerForm(int id)
        {
            var appointment = _appointmentService.GetAppointment(id);
            var model = new UpdateAppointmentsViewModel()
            {
                Id = id,
                PractitionerId = appointment.PractitionerId,
                PatientId = appointment.PatientId,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime
            };
            PrefillSelectionsWorkerUpdate(appointment.PatientId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequirePhysio")]
        public async Task<IActionResult> UpdateAppointmentWorkerForm(UpdateAppointmentsViewModel appointmentsViewModel)
        {

            if (appointmentsViewModel.StartTime < DateTime.Now)
            {
                ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Een afspraak mag niet in het verleden ingepland worden.");
            }

            var appointment = _appointmentService.GetAppointment(appointmentsViewModel.Id);

            if (appointment.Treatment != null && appointment.StartTime.Date < appointment.Treatment.TreatmentPerformedDate)
            {
                ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Een afspraak wijziging mag niet voor behandel datum liggen");
            }

            if (DateTime.Now > appointment.StartTime.AddDays(-1))
            {
                ModelState.AddModelError(string.Empty, "Een afspraak mag alleen 24 uur van te voren gewijzigd worden.");
            }

            if (appointmentsViewModel.StartTime != null && appointmentsViewModel.PatientId > 0)
            {
                var patient = _patientService.GetPatientById(appointmentsViewModel.PatientId);
                appointmentsViewModel.EndTime = appointmentsViewModel.StartTime.Value.AddMinutes(patient.PatientRecord.TreatmentPlan.SessionDuration);

                if (patient.PatientRecord == null)
                {
                    ModelState.AddModelError(nameof(appointmentsViewModel.PatientId), "Niet ingeschreven patienten mogen geen afspraken maken.");
                }

                if (patient.PatientRecord != null && (appointmentsViewModel.StartTime < patient.PatientRecord.DateOfRegistration ||
                                                      appointmentsViewModel.EndTime < patient.PatientRecord.DateOfRegistration
                                                      || appointmentsViewModel.StartTime > patient.PatientRecord.DateOfDismissal ||
                                                      appointmentsViewModel.EndTime > patient.PatientRecord.DateOfDismissal))
                {
                    ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Een afspraak mag niet buiten behandelsperiode vallen van patiënt.");
                }

                if (!_appointmentPlanner.CheckForAmountOfAppointmentsUpdate(patient.Id, appointmentsViewModel.StartTime.Value,
                    patient.PatientRecord.TreatmentPlan.SessionsPerWeek))
                {
                    ModelState.AddModelError(string.Empty, "Een nieuwe afspraak mag niet het aantal sessies per week overschrijden.");
                }
                else
                {
                    if (!_appointmentPlanner.CheckAvailability(appointmentsViewModel.PractitionerId,
                        appointmentsViewModel.StartTime.Value, appointmentsViewModel.EndTime.Value))
                    {
                        ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Datum en tijd mogen  niet vallen buiten beschikbaardheid van behandelaar.");
                    }
                    else
                    {
                        if (!_appointmentPlanner.CheckForAppointments(appointmentsViewModel.PractitionerId,
                            appointmentsViewModel.StartTime.Value,
                            appointmentsViewModel.EndTime.Value))
                        {
                            ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Er is al een afspraak gepland bij de behandelaar op deze tijd.");
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                appointment.PatientId = appointmentsViewModel.PatientId;
                appointment.PractitionerId = appointmentsViewModel.PractitionerId;
                appointment.StartTime = (DateTime)appointmentsViewModel.StartTime;
                appointment.EndTime = (DateTime)appointmentsViewModel.EndTime;

                await _appointmentService.UpdateAppointment(appointment);

                return RedirectToAction("Index", "Home");
            }

            PrefillSelectionsWorkerUpdate(appointmentsViewModel.PatientId);
            return View(appointmentsViewModel);
        }

        [HttpGet]
        public IActionResult AppointmentPatientForm(int id)
        {
            var model = new NewAppointmentsViewModel
            {
                PatientId = id
            };

            var practitionerId = -1;
            if (_patientService.GetPatientById(id).PatientRecord != null)
            {
                practitionerId = _patientService.GetPatientById(id).PatientRecord.HeadPractitionerId;
                model.PractitionerId = practitionerId;
            }

            PrefillSelectionsPatientUpdate(practitionerId, id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AppointmentPatientForm(NewAppointmentsViewModel appointmentsViewModel)
        {
            var patientRecord = _patientService.GetPatientById(appointmentsViewModel.PatientId).PatientRecord;

            if (appointmentsViewModel.PatientId > 0)
            {
                if (patientRecord == null)
                {
                    ModelState.AddModelError(nameof(appointmentsViewModel.PatientId), "Niet ingeschreven patienten mogen geen afspraken maken.");
                }

                if (patientRecord != null && (appointmentsViewModel.StartTime < patientRecord.DateOfRegistration ||
                                              appointmentsViewModel.EndTime < patientRecord.DateOfRegistration
                                              || appointmentsViewModel.StartTime > patientRecord.DateOfDismissal ||
                                              appointmentsViewModel.EndTime > patientRecord.DateOfDismissal))
                {
                    ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Een afspraak mag niet buiten behandelsperiode vallen van patiënt.");
                }
            }

            if (appointmentsViewModel.StartTime < DateTime.Now)
            {
                ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Een afspraak mag niet in het verleden ingepland worden.");
            }

            if (appointmentsViewModel.StartTime != null && patientRecord != null && appointmentsViewModel.PatientId > 0)
            {
                var patient = _patientService.GetPatientById(appointmentsViewModel.PatientId);
                appointmentsViewModel.EndTime = appointmentsViewModel.StartTime.Value.AddMinutes(patient.PatientRecord.TreatmentPlan.SessionDuration);

                if (!_appointmentPlanner.CheckForAmountOfAppointments(patient.Id, appointmentsViewModel.StartTime.Value,
                    patient.PatientRecord.TreatmentPlan.SessionsPerWeek))
                {
                    ModelState.AddModelError(string.Empty, "Een nieuwe afspraak mag niet het aantal sessies per week overschrijden.");
                }
                else
                {
                    if (!_appointmentPlanner.CheckAvailability(appointmentsViewModel.PractitionerId,
                        appointmentsViewModel.StartTime.Value, appointmentsViewModel.EndTime.Value))
                    {
                        ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Datum en tijd mogen niet vallen buiten beschikbaardheid van behandelaar.");
                    }
                    else
                    {
                        if (!_appointmentPlanner.CheckForAppointments(appointmentsViewModel.PractitionerId,
                            appointmentsViewModel.StartTime.Value,
                            appointmentsViewModel.EndTime.Value))
                        {
                            ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Er is al een afspraak gepland bij de behandelaar op deze tijd.");
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                await _appointmentService.AddAppointment(new Appointment
                {
                    PatientId = appointmentsViewModel.PatientId,
                    PractitionerId = appointmentsViewModel.PractitionerId,
                    StartTime = (DateTime)appointmentsViewModel.StartTime,
                    EndTime = (DateTime)appointmentsViewModel.EndTime
                });
                return RedirectToAction("Index", "Home");
            }
            PrefillSelectionsPatientUpdate(appointmentsViewModel.PractitionerId, appointmentsViewModel.PatientId);
            return View(appointmentsViewModel);
        }


        [HttpGet]
        public IActionResult UpdateAppointmentPatientForm(int id)
        {
            var appointment = _appointmentService.GetAppointment(id);
            var model = new UpdateAppointmentsViewModel()
            {
                Id = id,
                PractitionerId = appointment.PractitionerId,
                PatientId = appointment.PatientId,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime
            };
            PrefillSelectionsPatientUpdate(appointment.PractitionerId, appointment.PatientId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAppointmentPatientForm(UpdateAppointmentsViewModel appointmentsViewModel)
        {
            if (appointmentsViewModel.StartTime < DateTime.Now)
            {
                ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Een afspraak mag niet in het verleden ingepland worden.");
            }

            var appointment = _appointmentService.GetAppointment(appointmentsViewModel.Id);

            if (appointment.Treatment != null && appointmentsViewModel.StartTime != null && appointmentsViewModel.StartTime.Value.Date < appointment.Treatment.TreatmentPerformedDate)
            {
                ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Een afspraak wijziging mag niet voor behandel datum liggen");
            }

            if (DateTime.Now > appointment.StartTime.AddDays(-1))
            {
                ModelState.AddModelError(string.Empty, "Een afspraak mag alleen 24 uur van te voren gewijzigd worden.");
            }

            if (appointmentsViewModel.StartTime != null && appointmentsViewModel.PatientId > 0)
            {
                var patient = _patientService.GetPatientById(appointmentsViewModel.PatientId);
                appointmentsViewModel.EndTime = appointmentsViewModel.StartTime.Value.AddMinutes(patient.PatientRecord.TreatmentPlan.SessionDuration);

                if (patient.PatientRecord == null)
                {
                    ModelState.AddModelError(nameof(appointmentsViewModel.PatientId), "Niet ingeschreven patienten mogen geen afspraken maken.");
                }

                if (patient.PatientRecord != null && (appointmentsViewModel.StartTime < patient.PatientRecord.DateOfRegistration ||
                                                      appointmentsViewModel.EndTime < patient.PatientRecord.DateOfRegistration
                                                      || appointmentsViewModel.StartTime > patient.PatientRecord.DateOfDismissal ||
                                                      appointmentsViewModel.EndTime > patient.PatientRecord.DateOfDismissal))
                {
                    ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Een afspraak mag niet buiten behandelsperiode vallen van patiënt.");
                }

                if (!_appointmentPlanner.CheckForAmountOfAppointmentsUpdate(patient.Id, appointmentsViewModel.StartTime.Value,
                    patient.PatientRecord.TreatmentPlan.SessionsPerWeek))
                {
                    ModelState.AddModelError(string.Empty, "Een nieuwe afspraak mag niet het aantal sessies per week overschrijden.");
                }
                else
                {
                    if (!_appointmentPlanner.CheckAvailability(appointmentsViewModel.PractitionerId,
                        appointmentsViewModel.StartTime.Value, appointmentsViewModel.EndTime.Value))
                    {
                        ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Datum en tijd mogen niet vallen buiten beschikbaardheid van behandelaar.");
                    }
                    else
                    {
                        if (!_appointmentPlanner.CheckForAppointments(appointmentsViewModel.PractitionerId,
                            appointmentsViewModel.StartTime.Value,
                            appointmentsViewModel.EndTime.Value))
                        {
                            ModelState.AddModelError(nameof(appointmentsViewModel.StartTime), "Er is al een afspraak gepland bij de behandelaar op deze tijd.");
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                appointment.PatientId = appointmentsViewModel.PatientId;
                appointment.PractitionerId = appointmentsViewModel.PractitionerId;
                appointment.StartTime = (DateTime)appointmentsViewModel.StartTime;
                appointment.EndTime = (DateTime)appointmentsViewModel.EndTime;

                if (appointment.Treatment != null)
                {
                    appointment.Treatment.TreatmentPerformedDate = appointment.StartTime.Date;
                }

                await _appointmentService.UpdateAppointment(appointment);

                return RedirectToAction("Index", "Home");
            }
            PrefillSelectionsPatientUpdate(appointmentsViewModel.PractitionerId, appointmentsViewModel.PatientId);
            return View(appointmentsViewModel);
        }

        [HttpGet]
        public IActionResult Appointments()
        {
            var appointments = new List<Appointment>();
            if (User.HasClaim(c => c.Type == "Claim.Physiotherapist" || c.Type == "Claim.Intern"))
            {
                var workerId = _workerService.GetWorkerByEmail(User.FindFirstValue(ClaimTypes.Email)).Id;
                appointments = _appointmentService.GetAllWorkerAppointments(workerId);
                ViewBag.WorkerId = workerId;
            }
            else if (User.HasClaim(c => c.Type == "Claim.Patient"))
            {
                var patientId = _patientService.GetPatientByEmail(User.FindFirstValue(ClaimTypes.Email)).Id;
                appointments = _appointmentService.GetAllPatientAppointments(patientId);
                ViewBag.PatientId = patientId;
            }
            appointments.Sort((x, y) => DateTime.Compare(x.StartTime, y.EndTime));
            appointments.Reverse();
            var model = appointments.ToViewModel();
            return View("AppointmentsList", model);
        }

        private void PrefillSelections()
        {
            var workers = _workerService.GetAllWorkers().Prepend(new Worker { Id = -1, Name = "Selecteer behandelaar" });
            ViewBag.Workers = new SelectList(workers, "Id", "Name");
            var patients = _patientService.GetPatients().Prepend(new Patient { Id = -1, Name = "Selecteer patiënt" });
            ViewBag.Patients = new SelectList(patients, "Id", "Name");
        }

        private void PrefillSelectionsWorkerUpdate(int patientId)
        {
            var workers = _workerService.GetAllWorkers().Prepend(new Worker { Id = -1, Name = "Selecteer behandelaar" });
            ViewBag.Workers = new SelectList(workers, "Id", "Name");
            var patient = _patientService.GetPatientById(patientId);
            var patients = new List<Patient> { patient };
            ViewBag.Patients = new SelectList(patients, "Id", "Name");
        }

        private void PrefillSelectionsPatientUpdate(int practitionerId, int patientId)
        {
            if (practitionerId > 0)
            {
                var worker = _workerService.GetWorkerById(practitionerId);
                var workers = new List<Worker> { worker };
                ViewBag.Workers = new SelectList(workers, "Id", "Name");
            }
            var patient = _patientService.GetPatientById(patientId);
            var patients = new List<Patient> { patient };
            ViewBag.Patients = new SelectList(patients, "Id", "Name");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public object CalculateTime(DateTime dateTime, int patientId)
        {
            var patient = _patientService.GetPatientById(patientId);
            DateTime? endTime;
            if (patient.PatientRecord != null)
            {
                endTime = dateTime.AddMinutes(_patientService.GetPatientById(patientId).PatientRecord.TreatmentPlan.SessionDuration);
            }
            else
            {
                return false;
            }

            return (endTime != null) ? endTime.Value.ToString("yyyy-MM-ddTHH:mm") : "";
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<bool> CancelAppointment(int appointmentId)
        {
            var appointment = _appointmentService.GetAppointment(appointmentId);

            if (DateTime.Now > appointment.StartTime.AddDays(-1) )
            {
                return false;
            }

            var treatment = appointment.Treatment;

            var task = await _appointmentService.DeleteAppointment(appointment);

            if (task)
            {
                await _treatmentService.DeleteTreatment(treatment);
            }

            return task;
        }
    }
}
