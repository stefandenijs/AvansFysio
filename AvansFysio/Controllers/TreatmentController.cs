using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AvansFysio.Models;
using AvansFysio.Models.Treatments;
using Core.Domain;
using Core.Domain.People;
using Core.Domain.Space;
using Core.Domain.Treatment;
using Core.Domain.Webservice;
using Core.DomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;

namespace AvansFysio.Controllers
{
    public class TreatmentController : Controller
    {
        private readonly ITreatmentInfoServiceHttp _treatmentInfoServiceHttp;
        private readonly IRoomService _roomService;
        private readonly IWorkerService _workerService;
        private readonly ITreatmentService _treatmentService;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;

        public TreatmentController(ITreatmentInfoServiceHttp treatmentInfoServiceHttp, IRoomService roomService, IWorkerService workerService,
            ITreatmentService treatmentService, IPatientService patientService, IAppointmentService appointmentService)
        {
            _treatmentInfoServiceHttp = treatmentInfoServiceHttp;
            _roomService = roomService;
            _workerService = workerService;
            _treatmentService = treatmentService;
            _patientService = patientService;
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public IActionResult TreatmentDetails(int id)
        {
            var model = _treatmentService.GetTreatment(id).ToViewModel();
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "RequireWorker")]
        public IActionResult TreatmentForm(int id, int appointmentId)
        {
            var model = new NewTreatmentsViewModel
            {
                PatientId = id
            };
            if (appointmentId > 0)
            {
                var appointment = _appointmentService.GetAppointment(appointmentId);
                if (appointment != null)
                {
                    model.AppointmentId = appointment.Id;
                    model.TreatmentPerformedDate = appointment.StartTime.Date;
                }
            }

            var intern = _workerService.GetInternByEmail(User.FindFirstValue(ClaimTypes.Email));
            if (intern != null)
            {
                model.IsIntern = true;
                model.TreatmentPerformedById = intern.Id;
            }
            else
            {
                model.IsIntern = false;
            }

            PreFillSelects();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Policy = "RequireWorker")]
        public async Task<IActionResult> TreatmentForm(NewTreatmentsViewModel treatmentModel)
        {
            var patient = _patientService.GetPatientById(treatmentModel.PatientId);

            if (treatmentModel.IsIntern)
            {
                var internId = _workerService.GetInternByEmail(User.FindFirstValue(ClaimTypes.Email)).Id;
                if (treatmentModel.TreatmentPerformedById != internId)
                {
                    ModelState.AddModelError(nameof(treatmentModel.TreatmentPerformedById), "Een stagiair moet zichzelf kiezen bij het toevoegen van een behandeling.");
                }
            }

            if (treatmentModel.TreatmentInfoId > 0)
            {
                var explanationRequired = _treatmentInfoServiceHttp.GetTreatmentInfo(treatmentModel.TreatmentInfoId).ExplanationRequired.ToLower().Contains("ja");
                if (explanationRequired && string.IsNullOrEmpty(treatmentModel.Particularities))
                {
                    ModelState.AddModelError(nameof(treatmentModel.Particularities), "Voer in bijzonderheden behandeling.");
                }
            }

            if (patient.PatientRecord == null)
            {
                ModelState.AddModelError(string.Empty, "Behandelingen mogen niet doorgevoerd worden als patiënt nog niet geregistreerd is");
            }

            if (patient.PatientRecord != null)
            {
                if (treatmentModel.TreatmentPerformedDate > patient.PatientRecord.DateOfDismissal)
                {
                    ModelState.AddModelError(nameof(treatmentModel.TreatmentPerformedDate), "Een behandeling mag niet uitgevoerd worden na datum van afmelding");
                }
            }

            if (ModelState.IsValid)
            {
                var treatmentInfo = _treatmentInfoServiceHttp.GetTreatmentInfo(treatmentModel.TreatmentInfoId);

                patient.PatientRecord.Treatments.Add(new Treatment
                {
                    TreatmentInfoId = treatmentModel.TreatmentInfoId,
                    Code = treatmentInfo.Code,
                    Description = treatmentInfo.Description,
                    RoomId = treatmentModel.RoomId,
                    Particularities = treatmentModel.Particularities,
                    TreatmentPerformedById = treatmentModel.TreatmentPerformedById,
                    TreatmentPerformedDate = (DateTime)treatmentModel.TreatmentPerformedDate
                });

                await _patientService.UpdatePatient(patient);

                if (treatmentModel.AppointmentId > 0)
                {
                    var treatment = _patientService.GetPatientById(treatmentModel.PatientId).PatientRecord.Treatments.Last();
                    var appointment = _appointmentService.GetAppointment(treatmentModel.AppointmentId);
                    treatment.TreatmentPerformedDate = appointment.StartTime.Date;
                    appointment.Treatment = treatment;
                    await _appointmentService.UpdateAppointment(appointment);
                }

                return RedirectToAction("Details", "Patient", new { id = treatmentModel.PatientId});
            }

            PreFillSelects();
            return View(treatmentModel);
        }

        [HttpGet]
        [Authorize(Policy = "RequireWorker")]
        public IActionResult UpdateTreatmentForm(int id, int treatmentId)
        {
            var treatment = _treatmentService.GetTreatment(treatmentId);

            var model = new UpdateTreatmentsViewModel
            {
                PatientId = id,
                RoomId = treatment.RoomId,
                TreatmentInfoId = treatment.TreatmentInfoId,
                TreatmentId = treatment.Id,
                TreatmentPerformedById = treatment.TreatmentPerformedById,
                TreatmentPerformedDate = treatment.TreatmentPerformedDate
            };

            if (treatment.Particularities != null)
            {
                model.Particularities = treatment.Particularities;
            }

            PreFillSelects();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Policy = "RequireWorker")]
        public async Task<IActionResult> UpdateTreatmentForm(UpdateTreatmentsViewModel treatmentModel)
        {
            var patient = _patientService.GetPatientById(treatmentModel.PatientId);

            var treatment = _treatmentService.GetTreatment(treatmentModel.TreatmentId);

            if (DateTime.Now.Date > treatment.CreationDate.Date || DateTime.Now.Date < treatment.CreationDate.Date)
            {
                ModelState.AddModelError(string.Empty, "De mogelijkheid om deze behandeling te wijzigen is al verlopen.");
            }

            if (treatmentModel.TreatmentInfoId > 0)
            {
                var explanationRequired = _treatmentInfoServiceHttp.GetTreatmentInfo(treatmentModel.TreatmentId).ExplanationRequired.ToLower().Contains("ja");
                if (explanationRequired && string.IsNullOrEmpty(treatmentModel.Particularities))
                {
                    ModelState.AddModelError(nameof(treatmentModel.Particularities), "Voer in bijzonderheden behandeling.");
                }
            }

            if (treatmentModel.TreatmentPerformedDate > patient.PatientRecord.DateOfDismissal)
            {
                ModelState.AddModelError(nameof(treatmentModel.TreatmentPerformedDate), "Een behandeling mag niet uitgevoerd worden na datum van afmelding");
            }

            if (ModelState.IsValid)
            {
                var treatmentInfo = _treatmentInfoServiceHttp.GetTreatmentInfo(treatmentModel.TreatmentInfoId);

                treatment.Code = treatmentInfo.Code;
                treatment.Description = treatmentInfo.Description;
                treatment.RoomId = treatmentModel.RoomId;
                treatment.Particularities = treatmentModel.Particularities;
                treatment.TreatmentPerformedById = treatmentModel.TreatmentPerformedById;
                treatment.TreatmentPerformedDate = (DateTime) treatmentModel.TreatmentPerformedDate;

                await _treatmentService.UpdateTreatment(treatment);

                var appointment = _appointmentService.GetAppointmentByTreatment(treatment.Id);

                if (appointment != null)
                {
                    var newDateTime = treatment.TreatmentPerformedDate;
                    newDateTime = newDateTime.AddHours(appointment.StartTime.Hour);
                    appointment.StartTime = newDateTime;
                    appointment.EndTime = newDateTime.AddMinutes(patient.PatientRecord.TreatmentPlan.SessionDuration);

                    await _appointmentService.UpdateAppointment(appointment);
                }

                return RedirectToAction("Details", "Patient", new { id = treatmentModel.PatientId });
            }

            PreFillSelects();
            return View(treatmentModel);
        }

        private void PreFillSelects()
        {
            var treatments = _treatmentInfoServiceHttp.GetAllTreatmentInfo().Prepend(new TreatmentInfo { Id = -1, Code = "0000", Description = "Selecteer een behandeling"});
            ViewBag.Treatments = new SelectList(treatments.Select(t => new {t.Id, Treatment = $"{t.Code}  - {t.Description}" }), "Id", "Treatment");
            var rooms = _roomService.GetRooms().Prepend(new Room { Id = -1, RoomType = "Selecteer een behanderlruimte" });
            ViewBag.Rooms = new SelectList(rooms.Select(r => new {r.Id, Room = $"{r.RoomNumber} - {r.RoomType}"}), "Id", "Room");
            var workers = _workerService.GetAllWorkers()
                .Prepend(new Worker {Id = -1, Name = "Selecteer een behandelaar"});
            ViewBag.Workers = new SelectList(workers, "Id", "Name");
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        [Authorize(Policy = "RequirePhysio")]
        public async Task<bool> DeleteTreatment(int id)
        {
            var treatment = _treatmentService.GetTreatment(id);

            if (DateTime.Now.Date > treatment.CreationDate.Date || DateTime.Now.Date < treatment.CreationDate.Date)
            {
                return false;
            }

            var appointment = _appointmentService.GetAppointmentByTreatment(treatment.Id);
            if (appointment != null)
            {
                await _appointmentService.DeleteAppointment(appointment);
            }

            var task = await _treatmentService.DeleteTreatment(treatment);

            return task;
        }
    }
}
