using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Webservice;
using Core.DomainServices;
using Infrastructure.Data;

namespace Infrastructure
{
    public class TreatmentInfoService : ITreatmentInfoService
    {
        private readonly StamDataDbContext _context;

        public TreatmentInfoService(StamDataDbContext context)
        {
            _context = context;
        }

        public TreatmentInfo GetTreatmentInfo(int id)
        {
            var treatmentInfo = _context.Treatments.SingleOrDefault(t => t.Id == id);
            return treatmentInfo;
        }

        public List<TreatmentInfo> GetAllTreatmentInfo()
        {
            var treatmentInfo = _context.Treatments;
            return treatmentInfo.ToList();
        }
    }
}
