using System;

namespace Diligencia.EventSourcing
{
    public class Event
    {
        public Guid AggregateRootId { get; set; }

        internal int Order { get; set; }
    }
}
