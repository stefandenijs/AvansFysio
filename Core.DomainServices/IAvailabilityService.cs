using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Time;

namespace Core.DomainServices
{
    public interface IAvailabilityService
    {
        public List<Availability> GetAvailabilities(int id);

        public Availability GetAvailability(int id, DayOfWeek dayOfWeek);

        public Task AddAvailability(Availability availability);
        public Task UpdateAvailability(Availability availability);
    }
}
