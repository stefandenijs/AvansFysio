using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Webservice;

namespace Core.DomainServices
{
    public interface IDiagnosisServiceHttp
    {
        public List<string> GetBodyLocalizations();
        public Diagnosis GetDiagnosis(int id);
        public List<Diagnosis> GetDiagnosesByBodyLocalization(string bodyLocalization);
    }
}
