using System;

namespace Diligencia.EventSourcing.SqlStore
{
    public class SqlEvent
    {
        public Guid Id { get; set; }

        public Guid AggregateId { get; set; }

        public string EventType { get; set; }

        public int Order { get; set; }

        public string Data { get; set; }
    }
}
