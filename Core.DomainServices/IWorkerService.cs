using System.Collections.Generic;
using Core.Domain.People;

namespace Core.DomainServices
{
    public interface IWorkerService
    {
        public List<Worker> GetAllWorkers();
        public Worker GetWorkerById(int id);
        public Worker GetWorkerByEmail(string email);
        public Intern GetInternById(int id);
        public Intern GetInternByEmail(string email);
        public Physiotherapist GetPhysiotherapistById(int id);
        public Physiotherapist GetPhysiotherapistByEmail(string email);
        public List<Physiotherapist> GetPhysiotherapists();
    }
}
