using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansFysio.Models;
using AvansFysio.Models.Workers;
using Core.Domain.People;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace AvansFysio.Components
{
    public class InternDetailsViewComponent : ViewComponent
    {
        private readonly IWorkerService _workerService;

        public InternDetailsViewComponent(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        public IViewComponentResult Invoke(int workerId)
        {
            var intern = GetPhysiotherapist(workerId);
            return View(intern);
        }

        private InternsViewModel GetPhysiotherapist(int workerId)
        {
            var intern = _workerService.GetInternById(workerId).ToViewModel();
            return intern;
        }

    }
}
