using System;

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
            var root = new T();

            var allEvents = _store.Get(aggregate);

            root.FromHistory(allEvents);

            return root;
        }

        public void Save(Event @event)
        {
            _store.Save(@event);
        }
    }
}
