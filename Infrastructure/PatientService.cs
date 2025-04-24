using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.People;
using Core.DomainServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class PatientService : IPatientService
    {
        private readonly PhysioDbContext _context;

        public PatientService(PhysioDbContext context)
        {
            _context = context;
        }

        public List<Patient> GetPatients()
        {
            var patients = _context.Patients
                .Include(p => p.PatientRecord)
                .ThenInclude(pr => pr.TreatmentPlan)
                .Include(p => p.PatientRecord)
                .ThenInclude(pr => pr.IntakeHandler)
                .Include(p => p.PatientRecord)
                .ThenInclude(pr => pr.IntakeSupervisor)
                .Include(p => p.PatientRecord)
                .ThenInclude(pr => pr.HeadPractitioner);
            return patients.ToList();
        }

        public List<Patient> GetPhysiotherapistPatients(int id)
        {
            var patients = _context.Patients.Where(p => p.PatientRecord.HeadPractitionerId == id);
            return patients.ToList();
        }

        public Patient GetPatientById(int id)
        {
            return _context.Patients
                .Include(p => p.PatientRecord)
                .ThenInclude(pr => pr.TreatmentPlan)
                .Include(p => p.PatientRecord)
                .ThenInclude(pr => pr.IntakeHandler)
                .Include(p => p.PatientRecord)
                .ThenInclude(pr => pr.IntakeSupervisor)
                .Include(p => p.PatientRecord)
                .ThenInclude(pr => pr.HeadPractitioner)
                .Include(p => p.PatientRecord)
                .ThenInclude(pr => pr.Treatments)
                .SingleOrDefault(p => p.Id == id);
        }

        public Patient GetPatientByEmail(string email)
        {
            return _context.Patients
                .Include(p => p.PatientRecord).SingleOrDefault(p => p.Email == email);
        }

        public async Task AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePatient(Patient patient)
        {
            _context.Update(patient);
            await _context.SaveChangesAsync();
        }
    }
}
