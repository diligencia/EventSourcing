using System;
using System.Collections.Generic;
using System.Text;

namespace Diligencia.EventSourcing
{
    public abstract class Command
    {
        public Guid AggregateRootId { get; set; }
    }
}
