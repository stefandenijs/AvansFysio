using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AvansFysio.Models;
using AvansFysio.Models.Patients;
using Core.Domain;
using Core.Domain.PatientRecord;
using Core.Domain.People;
using Core.Domain.Treatment;
using Core.Domain.Webservice;
using Core.DomainServices;
using Core.DomainServices.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace AvansFysio.Controllers
{

    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatientService _patientService;
        private readonly IWorkerService _workerService;
        private readonly IAgeHelper _ageHelper;
        private readonly IDiagnosisServiceHttp _diagnosisServiceHttp;

        public PatientController(ILogger<PatientController> logger, IPatientService patientService,
            IWorkerService workerService, IAgeHelper ageHelper, IDiagnosisServiceHttp diagnosisServiceHttp)
        {
            _logger = logger;
            _patientService = patientService;
            _workerService = workerService;
            _ageHelper = ageHelper;
            _diagnosisServiceHttp = diagnosisServiceHttp;
        }

        [HttpGet]
        [Authorize(Policy = "RequireWorker")]
        public IActionResult Patients()
        {
            var model = _patientService.GetPatients().ToViewModel();
            return View("PatientsList", model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(int id)
        {
            var patient = _patientService.GetPatientById(id);
            if (patient == null) return NotFound();
            var model = patient.ToViewModel();
            return View("PatientDetails", model);
        }

        [HttpGet]
        [Authorize(Policy = "RequirePhysio")]
        public IActionResult PatientForm()
        {
            var model = new NewPatientViewModel();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Policy = "RequirePhysio")]
        public async Task<IActionResult> PatientForm(NewPatientViewModel patientModel)
        {

            if (!string.IsNullOrEmpty(patientModel.Name) && !patientModel.Name.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c) || c == '\''))
            {
                ModelState.AddModelError(nameof(patientModel.Name), "Patiënt naam mag geen speciale tekens bevatten.");
            }

            if (!string.IsNullOrEmpty(patientModel.Email))
            {
                if (_patientService.GetPatients().FirstOrDefault(p => p.Email.Equals(patientModel.Email)) != null)
                {
                    ModelState.AddModelError(nameof(patientModel.Email), $"Het e-mailadress {patientModel.Email} is al in gebruik.");
                }
            }

            if (patientModel.BirthDate > DateTime.Now)
            {
                ModelState.AddModelError(nameof(patientModel.BirthDate), "Een geboortedatum mag niet in de toekomst zijn.");
            }

            if (patientModel.BirthDate != null)
            {
                if (!_ageHelper.CheckAge((DateTime)patientModel.BirthDate))
                {
                    ModelState.AddModelError(nameof(patientModel.BirthDate), "Een patiënt moet 16 jaar of ouder zijn.");
                }
            }

            if (ModelState.IsValid)
            {
                var image = patientModel.Photo;

                await _patientService.AddPatient(new Patient
                {
                    Name = patientModel.Name.Trim(),
                    Email = patientModel.Email,
                    PhoneNumber = patientModel.PhoneNumber,
                    BirthDate = (DateTime)patientModel.BirthDate,
                    PatientImage = GetByteArrayFromImage(image),
                    Gender = (Gender)patientModel.Gender,
                    Role = (Role)patientModel.Role,
                    IdentificationNumber = patientModel.IdentificationNumber
                });
                var patientId = _patientService.GetPatientByEmail(patientModel.Email).Id;

                return RedirectToAction("Patients", new { id = patientId });
            }

            return View(patientModel);
        }



        [HttpGet]
        [Authorize(Policy = "RequireWorker")]
        public IActionResult UpdatePatientForm(int id)
        {
            var patient = _patientService.GetPatientById(id);
            
            var model = new UpdatePatientsViewModel()
            {
                Name = patient.Name,
                Email = patient.Email,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                IdentificationNumber = patient.IdentificationNumber,
                PhoneNumber = patient.PhoneNumber,
                Role = patient.Role
            };
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Policy = "RequireWorker")]
        public async Task<IActionResult> UpdatePatientForm(UpdatePatientsViewModel patientModel)
        {

            if (!string.IsNullOrEmpty(patientModel.Name) && !patientModel.Name.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c) || c == '\''))
            {
                ModelState.AddModelError(nameof(patientModel.Name), "Patiënt naam mag geen speciale tekens bevatten.");
            }

            if (patientModel.BirthDate > DateTime.Now)
            {
                ModelState.AddModelError(nameof(patientModel.BirthDate), "Een geboortedatum mag niet in de toekomst zijn.");
            }

            if (patientModel.BirthDate != null)
            {
                if (!_ageHelper.CheckAge((DateTime)patientModel.BirthDate))
                {
                    ModelState.AddModelError(nameof(patientModel.BirthDate), "Een patiënt moet 16 jaar of ouder zijn.");
                }
            }

            if (ModelState.IsValid)
            {
                var patient = _patientService.GetPatientById(patientModel.Id);

                patient.Name = patientModel.Name.Trim();
                patient.PhoneNumber = patientModel.PhoneNumber;
                patient.BirthDate = (DateTime)patientModel.BirthDate;
                patient.Gender = (Gender)patientModel.Gender;
                patient.Role = (Role)patientModel.Role;
                patient.IdentificationNumber = patientModel.IdentificationNumber;
                patient.PatientRecord.Age = _ageHelper.Age((DateTime) patientModel.BirthDate);

                if (patientModel.Photo != null)
                {
                    var image = patientModel.Photo;
                    patient.PatientImage = GetByteArrayFromImage(image);
                }

                await _patientService.UpdatePatient(patient);

                return RedirectToAction("Patients", new { id = patientModel.Id });
            }

            return View(patientModel);
        }

        [HttpGet]
        [Authorize(Policy = "RequirePhysio")]
        public IActionResult PatientRecordForm(int id)
        {
            PreFillWorkers();
            PreFillBodyLocalizations();
            var model = new NewPatientRecordsViewModel
            {
                PatientId = id,
                BodyLocalization = "Selecteer een lichaamslocatie"
            };
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Policy = "RequirePhysio")]
        public async Task<IActionResult> PatientRecordForm(NewPatientRecordsViewModel patientRecordModel)
        {
            if (_workerService.GetInternById(patientRecordModel.IntakeHandlerId) != null)
            {
                if (patientRecordModel.IntakeSupervisorId < 1)
                {
                    ModelState.AddModelError(nameof(patientRecordModel.IntakeSupervisorId), "Voer in toezichthouder intake bij een stagiair.");
                }
            }

            if (patientRecordModel.DateOfRegistration > DateTime.Now)
            {
                ModelState.AddModelError(nameof(patientRecordModel.DateOfRegistration), "Registratie datum mag niet in de toekomst liggen.");
            }

            if (patientRecordModel.DateOfDismissal < DateTime.Now)
            {
                ModelState.AddModelError(nameof(patientRecordModel.DateOfDismissal), "Afmeldings datum mag niet in het verleden liggen.");
            }

            if (patientRecordModel.BodyLocalization.Equals("Selecteer een lichaamslocatie"))
            {
                ModelState.AddModelError(nameof(patientRecordModel.BodyLocalization), "Selecteer een lichaamslocatie.");
            }

            if (ModelState.IsValid)
            {
                var diagnose = _diagnosisServiceHttp.GetDiagnosis(patientRecordModel.DiagnoseId);
                var patient = _patientService.GetPatientById(patientRecordModel.PatientId);
                patient.PatientRecord = new PatientRecord
                {
                    Age = _ageHelper.Age(patient.BirthDate),
                    PatientId = patientRecordModel.PatientId,
                    IntakeHandlerId = patientRecordModel.IntakeHandlerId,
                    HeadPractitionerId = patientRecordModel.HeadPractitionerId,
                    DiagnosticBodyLocalization = diagnose.BodyLocalization,
                    DiagnoseId = diagnose.Id,
                    DiagnosticCode = diagnose.DCSPH,
                    DiagnoseDescription = diagnose.Description,
                    MedicalComplaintsDescription = patientRecordModel.ProblemDescription,
                    DateOfRegistration = (DateTime)patientRecordModel.DateOfRegistration,
                    DateOfDismissal = (DateTime)patientRecordModel.DateOfDismissal,
                    TreatmentPlan = new TreatmentPlan
                    {
                        SessionsPerWeek = patientRecordModel.SessionsPerWeek,
                        SessionDuration = patientRecordModel.SessionsDuration
                    }
                };
                if (patientRecordModel.IntakeSupervisorId > 0)
                {
                    patient.PatientRecord.IntakeSupervisorId = patientRecordModel.IntakeSupervisorId;
                }
                await _patientService.UpdatePatient(patient);

                return RedirectToAction("Index", "Home");
            }

            PreFillWorkers();
            PreFillBodyLocalizations();
            return View(patientRecordModel);
        }

        [HttpGet]
        [Authorize(Policy = "RequirePhysio")]
        public IActionResult UpdatePatientRecordForm(int id)
        {
            PreFillWorkers();
            PreFillBodyLocalizations();
            var patientRecord = _patientService.GetPatientById(id).PatientRecord;
            var model = new UpdatePatientRecordsViewModel
            {
                PatientId = patientRecord.PatientId,
                DiagnoseId = patientRecord.DiagnoseId,
                BodyLocalization = patientRecord.DiagnosticBodyLocalization,
                DateOfRegistration = patientRecord.DateOfRegistration,
                DateOfDismissal = patientRecord.DateOfDismissal,
                HeadPractitionerId = patientRecord.HeadPractitionerId,
                IntakeHandlerId = patientRecord.IntakeHandlerId,
                ProblemDescription = patientRecord.MedicalComplaintsDescription,
                SessionsPerWeek = patientRecord.TreatmentPlan.SessionsPerWeek,
                SessionsDuration = patientRecord.TreatmentPlan.SessionDuration
            };
            if (patientRecord.IntakeSupervisorId > 0)
            {
                model.IntakeSupervisorId = (int)patientRecord.IntakeSupervisorId;
            }
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Policy = "RequirePhysio")]
        public async Task<IActionResult> UpdatePatientRecordForm(UpdatePatientRecordsViewModel patientRecordModel)
        {
            if (_workerService.GetInternById(patientRecordModel.IntakeHandlerId) != null)
            {
                if (patientRecordModel.IntakeSupervisorId < 1)
                {
                    ModelState.AddModelError(nameof(patientRecordModel.IntakeSupervisorId), "Voer in toezichthouder intake bij een stagiair.");
                }
            }

            if (patientRecordModel.DateOfRegistration > DateTime.Now)
            {
                ModelState.AddModelError(nameof(patientRecordModel.DateOfRegistration), "Registratie datum mag niet in de toekomst liggen.");
            }

            if (patientRecordModel.DateOfDismissal < DateTime.Now)
            {
                ModelState.AddModelError(nameof(patientRecordModel.DateOfDismissal), "Afmeldings datum mag niet in het verleden liggen.");
            }

            if (patientRecordModel.BodyLocalization.Equals("Selecteer een lichaamslocatie"))
            {
                ModelState.AddModelError(nameof(patientRecordModel.BodyLocalization), "Selecteer een lichaamslocatie.");
            }

            if (ModelState.IsValid)
            {
                var diagnose = _diagnosisServiceHttp.GetDiagnosis(patientRecordModel.DiagnoseId);
                var patient = _patientService.GetPatientById(patientRecordModel.PatientId);

                patient.PatientRecord.PatientId = patientRecordModel.PatientId;
                patient.PatientRecord.IntakeHandlerId = patientRecordModel.IntakeHandlerId;
                patient.PatientRecord.HeadPractitionerId = patientRecordModel.HeadPractitionerId;
                patient.PatientRecord.DiagnosticBodyLocalization = diagnose.BodyLocalization;
                patient.PatientRecord.DiagnosticCode = diagnose.DCSPH;
                patient.PatientRecord.DiagnoseDescription = diagnose.Description;
                patient.PatientRecord.MedicalComplaintsDescription = patientRecordModel.ProblemDescription;
                patient.PatientRecord.DateOfRegistration = (DateTime)patientRecordModel.DateOfRegistration;
                patient.PatientRecord.DateOfDismissal = (DateTime)patientRecordModel.DateOfDismissal;


                patient.PatientRecord.TreatmentPlan.SessionsPerWeek = patientRecordModel.SessionsPerWeek;
                patient.PatientRecord.TreatmentPlan.SessionDuration = patientRecordModel.SessionsDuration;

                if (patientRecordModel.IntakeSupervisorId > 0)
                {
                    patient.PatientRecord.IntakeSupervisorId = patientRecordModel.IntakeSupervisorId;
                }
                await _patientService.UpdatePatient(patient);

                return RedirectToAction("Details", "Patient", new {id = patientRecordModel.PatientId});
            }

            PreFillWorkers();
            PreFillBodyLocalizations();
            return View(patientRecordModel);
        }

        private void PreFillWorkers()
        {
            var workers = _workerService.GetAllWorkers()
                .Prepend(new Worker() { Id = -1, Name = "Selecteer een intakebehandelaar" });
            ViewBag.Workers = new SelectList(workers, "Id", "Name");
            var physiotherapists = _workerService.GetPhysiotherapists()
                .Prepend(new Worker() { Id = -1, Name = "Selecteer een fysiotherapeut" });
            ViewBag.Physiotherapists = new SelectList(physiotherapists, "Id", "Name");
        }

        private void PreFillBodyLocalizations()
        {
            var bodyLocalizations = _diagnosisServiceHttp.GetBodyLocalizations().Prepend("Selecteer een lichaamslocatie");
            ViewBag.bodyLocalizations = new SelectList(bodyLocalizations);
        }

        public SelectList GetPhysiotherapists()
        {
            var physiotherapists = _workerService.GetPhysiotherapists()
                .Prepend(new Worker() { Id = -1, Name = "Selecteer een fysiotherapeut" });
            return new SelectList(physiotherapists, "Id", "Name");
        }

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using var target = new MemoryStream();
            file.CopyTo(target);
            return target.ToArray();
        }

        [HttpGet]
        [Authorize(Policy = "RequirePhysio")]
        public IActionResult CheckWorker(int id)
        {
            ViewBag.Physiotherapists = GetPhysiotherapists();

            var result = _workerService.GetInternById(id) != null;

            var model = new NewPatientRecordsViewModel();
            return result == true ? PartialView("_SupervisorInput", model) : new EmptyResult();
        }

        [HttpGet]
        [Authorize(Policy = "RequirePhysio")]
        public IActionResult CheckBodyLocalization(string location)
        {
            var elementSelected = location != null && !location.Equals("Selecteer een lichaamslocatie");

            if (elementSelected)
            {
                ViewBag.Diagnoses = FillDiagnoses(location);
            }
            var model = new NewPatientRecordsViewModel();
            return elementSelected == true ? PartialView("_DiagnosisInput", model) : new EmptyResult();
        }

        private SelectList FillDiagnoses(string location)
        {
            var diagnoses = _diagnosisServiceHttp.GetDiagnosesByBodyLocalization(location)
                .Prepend(new Diagnosis { Id = -1, DCSPH = "0000", Description = "Selecteer een diagnose", BodyLocalization = "Geen" });
            return new SelectList(diagnoses.Select(c => new { Id = c.Id, Diagnosis = $"{c.DCSPH}  - {c.Description}" }), "Id", "Diagnosis");
        }
    }
}
