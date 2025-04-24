using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Webservice;
using Core.DomainServices;

namespace AvansFysioWebservice.Model
{
    public class Query
    {
        private readonly IDiagnosisService _diagnoseService;

        public Query(IDiagnosisService diagnoseService)
        {
            _diagnoseService = diagnoseService;
        }

        public IEnumerable<Diagnosis> Diagnoses => _diagnoseService.GetDiagnoses();
    }
}
