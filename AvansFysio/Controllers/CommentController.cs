using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AvansFysio.Models.Comments;
using Core.Domain;
using Core.Domain.PatientRecord;
using Core.Domain.Treatment;
using Core.DomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AvansFysio.Controllers
{
    [Authorize(Policy = "RequireWorker")]
    public class CommentController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IWorkerService _workerService;
        private readonly ITreatmentService _treatmentService;

        public CommentController(IPatientService patientService, IWorkerService workerService, ITreatmentService treatmentService)
        {
            _patientService = patientService;
            _workerService = workerService;
            _treatmentService = treatmentService;
        }

        [HttpGet]
        public IActionResult CommentForm(int id)
        {
            var model = new NewCommentsViewModel
            {
                PatientId = id
            };
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CommentForm(NewCommentsViewModel commentsModel)
        {
            if (ModelState.IsValid)
            {
                var patient = _patientService.GetPatientById(commentsModel.PatientId);
                var workerId = _workerService.GetWorkerByEmail(User.FindFirstValue(ClaimTypes.Email)).Id;

                patient.PatientRecord.Comments ??= new List<Comment>();

                patient.PatientRecord.Comments.Add(new Comment
                {
                    CommentText = commentsModel.Comment,
                    Visible = commentsModel.Visible,
                    PlacedById = workerId,
                    Date = DateTime.Now
                });

                await _patientService.UpdatePatient(patient);

                return RedirectToAction("Details", "Patient", new { id = commentsModel.PatientId });
            }

            return View(commentsModel);
        }
    }
}
