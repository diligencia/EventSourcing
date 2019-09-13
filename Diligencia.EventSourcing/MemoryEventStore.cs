using System;
using System.Collections.Generic;
using System.Linq;

namespace Diligencia.EventSourcing
{
    public class MemoryEventStore : EventStore
    {
        private List<Event> _events;

        public MemoryEventStore(EventPublisher publisher)
            :base (publisher)
        {
            _events = new List<Event>();
        }

        public override List<Event> Get(Guid aggregate)
        {
            return _events
                .Where(c => c.AggregateRootId == aggregate)
                .OrderBy(e => e.Order)
                .ToList();
        }

        protected override void SaveInStore(Event @event)
        {
            _events.Add(@event);
        }
    }
}
