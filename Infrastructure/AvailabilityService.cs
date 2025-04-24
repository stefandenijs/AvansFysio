using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Time;
using Core.DomainServices;
using Infrastructure.Data;

namespace Infrastructure
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly PhysioDbContext _context;

        public AvailabilityService(PhysioDbContext context)
        {
            _context = context;
        }

        public List<Availability> GetAvailabilities(int id)
        {
            var availabilities = _context.Availabilities.Where(a => a.WorkerId == id);

            return availabilities.ToList();
        }

        public Availability GetAvailability(int id, DayOfWeek dayOfWeek)
        {
            var availability = _context.Availabilities.Where(a => a.Day == dayOfWeek)
                .SingleOrDefault(a => a.WorkerId == id);
            return availability;
        }

        public async Task AddAvailability(Availability availability)
        {
            _context.Availabilities.Add(availability);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAvailability(Availability availability)
        {
            _context.Availabilities.Update(availability);
            await _context.SaveChangesAsync();
        }
    }
}
