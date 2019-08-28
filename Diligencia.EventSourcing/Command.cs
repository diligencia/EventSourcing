using System;

namespace Diligencia.EventSourcing
{
    public abstract class Command
    {
        public Guid AggregateRootId { get; set; }
    }
}
