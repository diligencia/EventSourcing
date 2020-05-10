using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diligencia.EventSourcing.AzureEventStore
{
    public class StorageEventStore : EventStore
    {
        private CloudTable _cloudTable;

        public StorageEventStore(string connectionString, EventPublisher eventPublisher)
            : base(eventPublisher)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            CloudTableClient client = account.CreateCloudTableClient();
            _cloudTable = client.GetTableReference("events");
            _cloudTable.CreateIfNotExistsAsync();
        }

        public override List<Event> Get(Guid aggregateId)
        {
            TableQuery<TableEvent> query = new TableQuery<TableEvent>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, aggregateId.ToString()));

            List<Event> allEvents = new List<Event>();
            TableContinuationToken token = new TableContinuationToken();

            do
            {
                TableQuerySegment<TableEvent> events = _cloudTable.ExecuteQuerySegmented(query, token);
                token = events.ContinuationToken;
            
                foreach (var @event in events.Results)
                {
                    allEvents.Add(ToEvent(@event));
                }
            } while (token != null);

            return allEvents
                .OrderBy(e => e.Order)
                .ToList();
        }

        protected override void SaveInStore(Event @event)
        {
            TableEvent tableEvent = new TableEvent
            {
                PartitionKey = @event.AggregateRootId.ToString(),
                RowKey = DateTime.UtcNow.Ticks.ToString(),
                AggregateId = @event.AggregateRootId,
                EventType = @event.GetType().Name,
                Order = @event.Order,
                Data = JsonConvert.SerializeObject(@event)
            };

            TableOperation newEvent = TableOperation.Insert(tableEvent);
            _cloudTable.Execute(newEvent);
        }
    }
}
