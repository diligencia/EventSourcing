using System;
using System.Linq;

namespace Diligencia.EventSourcing
{
    public class StateConnector
    {
        private IEventStore _store;

        public StateConnector(IEventStore store)
        {
            _store = store;
        }

        public T Get<T>(Guid aggregate) where T : AggregateRoot, new()
        {
            T root = null;
            var allEvents = _store.Get(aggregate);

            if (allEvents.Any())
            {
                root = new T();

                root.FromHistory(allEvents);
            }

            return root;
        }

        public void Save(Event @event)
        {
            // Get the other events for this new event to determine the order.
            var otherEvents = _store.Get(@event.AggregateRootId);
            @event.Order = otherEvents.LastOrDefault()?.Order + 1 ?? 1;

            _store.Save(@event);
        }
    }
}
