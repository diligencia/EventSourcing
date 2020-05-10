using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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

        protected Event ToEvent(IEventStoreItem item)
        {
            Type eventType = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name.Equals(item.EventType));

            var currentEvent = Activator.CreateInstance(eventType);
            JsonConvert.PopulateObject(item.Data, currentEvent);

            return currentEvent as Event;
        }
    }
}
