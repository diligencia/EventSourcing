using System;
using System.Collections.Generic;
using System.Text;

namespace Diligencia.EventSourcing
{
    public class Event
    {
        public Guid AggregateRootId { get; set; }
    }
}
