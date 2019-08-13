using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diligencia.EventSourcing
{
    public class MemoryEventStore : IEventStore
    {
        private List<Event> _events;

        public MemoryEventStore()
        {
            _events = new List<Event>();
        }

        public List<Event> Get(Guid aggregate)
        {
            return _events
                .Where(c => c.AggregateRootId == aggregate)
                .OrderBy(e => e.Order)
                .ToList();
        }

        public void Save(Event @event)
        {
            _events.Add(@event);
        }
    }
}
