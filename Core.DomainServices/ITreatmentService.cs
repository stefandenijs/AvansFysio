using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.Treatment;

namespace Core.DomainServices
{
    public interface ITreatmentService
    {
        public List<Treatment> GetAllTreatments(int patientRecordId);
        public List<Treatment> GetAllTreatmentsForWorker(int workerId);
        public Treatment GetTreatment(int treatmentId);
        public Task UpdateTreatment(Treatment treatment);
        public Task<bool> DeleteTreatment(Treatment treatment);
    }
}
