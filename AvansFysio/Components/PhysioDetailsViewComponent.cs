using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvansFysio.Models;
using AvansFysio.Models.Workers;
using Core.DomainServices;
using Microsoft.AspNetCore.Mvc;

namespace AvansFysio.Components
{
    public class PhysioDetailsViewComponent : ViewComponent
    {
        private readonly IWorkerService _workerService;

        public PhysioDetailsViewComponent(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        public IViewComponentResult Invoke(int workerId)
        {
            var physiotherapist = GetPhysiotherapist(workerId);
            return View(physiotherapist);
        }

        private PhysiotherapistsViewModel GetPhysiotherapist(int workerId)
        {
            var physiotherapist = _workerService.GetPhysiotherapistById(workerId).ToViewModel();
            return physiotherapist;
        }
    }
}
