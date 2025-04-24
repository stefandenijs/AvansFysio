using System.Collections.Generic;
using System.Linq;
using Core.Domain.Webservice;
using Core.DomainServices;
using Infrastructure.Data;

namespace Infrastructure
{
    public class DiagnosisService : IDiagnosisService
    {
        private readonly StamDataDbContext _context;

        public DiagnosisService(StamDataDbContext context)
        {
            _context = context;
        }

        public List<string> GetBodyLocalizations()
        {
            var bodyLocalizations = _context.Diagnoses;
            var strings = bodyLocalizations.ToList().Select(l => l.BodyLocalization).Distinct();
            return strings.ToList();
        }
        public string GetBodyLocalization(int id)
        {
            var bodyLocalization = _context.Diagnoses.SingleOrDefault(d => d.Id == id)?.BodyLocalization;
            return bodyLocalization;
        }

        public List<Diagnosis> GetDiagnoses()
        {
            var diagnoses = _context.Diagnoses;
            return diagnoses.ToList();
        }

        public List<Diagnosis> GetDiagnosesByBodyLocalization(string bodyLocalization)
        {
            var diagnoses = _context.Diagnoses.Where(d => d.BodyLocalization == bodyLocalization);
            return diagnoses.ToList();
        }

        public Diagnosis GetDiagnosis(int id)
        {
            var diagnose = _context.Diagnoses.SingleOrDefault(d => d.Id == id);
            return diagnose;
        }
    }
}
