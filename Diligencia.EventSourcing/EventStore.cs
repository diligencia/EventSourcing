using System;
using System.Collections.Generic;

namespace Diligencia.EventSourcing
{
    public abstract class EventStore : IEventStore
    {
        private readonly EventPublisher _eventPublisher;

        public EventStore(EventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public abstract List<Event> Get(Guid aggregateId);

        public void Save(Event @event)
        {
            SaveInStore(@event);

            _eventPublisher.Publish(@event);
        }

        protected abstract void SaveInStore(Event @event);
    }
}
