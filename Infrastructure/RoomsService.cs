using System.Collections.Generic;
using System.Linq;
using Core.Domain;
using Core.Domain.Space;
using Core.DomainServices;
using Infrastructure.Data;

namespace Infrastructure
{
    public class RoomsService : IRoomService
    {
        private readonly PhysioDbContext _context;

        public RoomsService(PhysioDbContext context)
        {
            _context = context;
        }

        public List<Room> GetRooms()
        {
            var rooms = _context.Rooms;
            return rooms.ToList();
        }
    }
}
