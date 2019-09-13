using System;

namespace Diligencia.EventSourcing
{
    public class Event
    {
        public Guid AggregateRootId { get; set; }

        public int Order { get; set; }
    }
}
