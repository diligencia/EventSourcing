﻿using Microsoft.Azure.Cosmos.Table;
using System;

namespace Diligencia.EventSourcing.AzureEventStore
{
    internal class TableEvent : TableEntity
    {
        public Guid Id { get; set; }

        public string EventType { get; set; }

        public int Order { get; set; }

        public string Data { get; set; }
    }
}
