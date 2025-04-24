using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Core.Domain.Space;

namespace Core.DomainServices
{
    public interface IRoomService
    {
        public List<Room> GetRooms();
    }
}
