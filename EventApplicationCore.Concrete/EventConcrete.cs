using EventApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventApplicationCore.Model;

namespace EventApplicationCore.Concrete
{
    public class EventConcrete : IEvent
    {
        private DatabaseContext _context;

        public EventConcrete(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Event> GetAllEvents()
        {
            return _context.Event.ToList();
        }
    }
}
