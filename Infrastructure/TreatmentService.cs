using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.Treatment;
using Core.DomainServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class TreatmentService : ITreatmentService
    {
        private readonly PhysioDbContext _context;

        public TreatmentService(PhysioDbContext context)
        {
            _context = context;
        }

        public Treatment GetTreatment(int treatmentId)
        {
            var treatment = _context.Treatments
                .Include(t => t.TreatmentPerformedBy)
                .Include(t => t.Room)
                .Include(t => t.PatientRecord)
                .ThenInclude(pr => pr.Patient)
                .SingleOrDefault(t => t.Id == treatmentId);
            return treatment;
        }

        public List<Treatment> GetAllTreatments(int patientRecordId)
        {
            var treatments = _context.Treatments.Where(t => t.PatientRecordId == patientRecordId)
                .Include(t => t.TreatmentPerformedBy)
                .Include(t => t.Room)
                .Include(t => t.PatientRecord);
            return treatments.ToList();
        }

        public List<Treatment> GetAllTreatmentsForWorker(int workerId)
        {
            var treatments = _context.Treatments.Where(t => t.TreatmentPerformedById == workerId)
                .Include(t => t.TreatmentPerformedBy)
                .Include(t => t.Room)
                .Include(t => t.PatientRecord)
                .ThenInclude(pr => pr.Patient);
            return treatments.ToList();
        }

        public async Task UpdateTreatment(Treatment treatment)
        {
            _context.Treatments.Update(treatment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteTreatment(Treatment treatment)
        {
            try
            {
                _context.Treatments.Remove(treatment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
