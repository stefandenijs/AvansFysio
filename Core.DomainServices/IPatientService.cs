using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.People;

namespace Core.DomainServices
{
    public interface IPatientService
    {
        public List<Patient> GetPatients();
        public List<Patient> GetPhysiotherapistPatients(int id);
        public Patient GetPatientById(int id);
        public Patient GetPatientByEmail(string email);
        public Task AddPatient(Patient patient);
        public Task UpdatePatient(Patient patient);
    }
}
