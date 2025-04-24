using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AvansFysio.Models;
using AvansFysio.Models.Treatments;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace AvansFysio.Components
{
    public class InternTreatmentsViewComponent : ViewComponent
    {
        private readonly ITreatmentService _treatmentService;

        public InternTreatmentsViewComponent(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        public IViewComponentResult Invoke(int workerId)
        {
            var treatments = GetTreatments(workerId);
            return View(treatments);
        }

        private List<TreatmentsViewModel> GetTreatments(int workerId)
        {
            var treatments = _treatmentService.GetAllTreatmentsForWorker(workerId).ToViewModel();
            return treatments;
        }
    }
}
