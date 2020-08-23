using System;
using System.Collections.Generic;

namespace Diligencia.EventSourcing
{
    public interface IEventStore
    {
        List<Event> Get(Guid aggregateId);

        void Save(Event @event);
    }
}
