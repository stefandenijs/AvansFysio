using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using AvansFysio.Models;
using AvansFysio.Models.Treatments;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace AvansFysio.Components
{
    public class TreatmentsViewComponent : ViewComponent
    {
        private readonly ITreatmentService _treatmentService;

        public TreatmentsViewComponent(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        public IViewComponentResult Invoke(int patientRecordId)
        {
            var treatments = GetTreatments(patientRecordId);
            return View(treatments);
        }

        private List<TreatmentsViewModel> GetTreatments(int patientRecordId)
        {
            var treatments = _treatmentService.GetAllTreatments(patientRecordId).ToViewModel();
            treatments.Sort((x,y) => DateTime.Compare(x.PerformedOn, y.PerformedOn));
            treatments.Reverse();
            return treatments;
        }
    }
}
