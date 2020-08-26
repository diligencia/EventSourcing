using System;

namespace Diligencia.EventSourcing
{
    /// <summary>
    /// Abstract base for all commands
    /// </summary>
    public abstract class Command
    {
        public Guid AggregateRootId { get; set; }
    }
}
