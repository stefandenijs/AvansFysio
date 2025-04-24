using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Webservice;

namespace Core.DomainServices
{
    public interface IDiagnosisService
    {
        public List<string> GetBodyLocalizations();
        public string GetBodyLocalization(int id);
        public List<Diagnosis> GetDiagnoses();
        public List<Diagnosis> GetDiagnosesByBodyLocalization(string bodyLocalization);
        public Diagnosis GetDiagnosis(int id);
    }
}
