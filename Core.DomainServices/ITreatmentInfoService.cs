using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Webservice;

namespace Core.DomainServices
{
    public interface ITreatmentInfoService
    {
        public TreatmentInfo GetTreatmentInfo(int id);
        public List<TreatmentInfo> GetAllTreatmentInfo();
    }
}
