using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.People;
using Core.DomainServices;
using Infrastructure.Data;

namespace Infrastructure
{
    public class WorkerService : IWorkerService
    {
        private readonly PhysioDbContext _context;

        public WorkerService(PhysioDbContext context)
        {
            _context = context;
        }
        
        public List<Worker> GetAllWorkers()
        {
            var workers = _context.Workers;
            return workers.ToList();
        }

        public Worker GetWorkerById(int id)
        {
            var worker = _context.Workers.SingleOrDefault(w => w.Id == id);
            return worker;
        }

        public Worker GetWorkerByEmail(string email)
        {
            var worker = _context.Workers.SingleOrDefault(w => w.Email == email);
            return worker;
        }

        public Intern GetInternById(int id)
        {
            var intern = (Intern) _context.Workers.SingleOrDefault(w => w.Id == id && w is Intern);
            return  intern;
        }

        public Intern GetInternByEmail(string email)
        {
            var intern = (Intern) _context.Workers.SingleOrDefault(w => w.Email == email && w is Intern);
            return intern;
        }

        public Physiotherapist GetPhysiotherapistById(int id)
        {
            var physiotherapist = (Physiotherapist) _context.Workers.SingleOrDefault(w => w.Id == id && w is Physiotherapist);
            return physiotherapist;
        }

        public Physiotherapist GetPhysiotherapistByEmail(string email)
        {
            var physiotherapist = (Physiotherapist) _context.Workers.SingleOrDefault(w => w.Email == email && w is Physiotherapist);
            return physiotherapist;
        }
        public List<Physiotherapist> GetPhysiotherapists()
        {
            var physiotherapists = _context.Workers.OfType<Physiotherapist>();
            return physiotherapists.ToList();
        }
    }
}
