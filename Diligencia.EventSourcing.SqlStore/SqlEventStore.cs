using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Diligencia.EventSourcing.SqlStore
{
    public class SqlEventStore : EventStore
    {
        private readonly string _connectionString;

        public SqlEventStore(string connectionString, EventPublisher eventPublisher)
            : base(eventPublisher)
        {
            _connectionString = connectionString;
        }

        public override List<Event> Get(Guid aggregateId)
        {
            List<Event> events = new List<Event>();
            List<SqlEvent> sqlEvents = new List<SqlEvent>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string sqlCommand = $"SELECT * FROM events WHERE AggregateId = '{aggregateId}' ORDER BY [Order] ASC";

                using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SqlEvent sqlEvent = new SqlEvent();
                            sqlEvent.Id = new Guid(reader["Id"].ToString());
                            sqlEvent.AggregateId = new Guid(reader["AggregateId"].ToString());
                            sqlEvent.Order = int.Parse(reader["Order"].ToString());
                            sqlEvent.EventType = reader["Type"].ToString();
                            sqlEvent.Data = reader["Data"].ToString();

                            sqlEvents.Add(sqlEvent);
                        }
                    }
                }
            }

            foreach (SqlEvent @event in sqlEvents)
            {
                Type eventType = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.IsDynamic)
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name.Equals(@event.EventType));

                var currentEvent = Activator.CreateInstance(eventType);
                JsonConvert.PopulateObject(@event.Data, currentEvent);
                events.Add(currentEvent as Event);
            }

            return events;
        }

        protected override void SaveInStore(Event @event)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string data = JsonConvert.SerializeObject(@event);
                string query = $"INSERT INTO events ([Id], [AggregateId], [Order], [Type], [Data]) VALUES (NEWID(), '{@event.AggregateRootId}', '{@event.Order}', '{@event.GetType().Name}', '{data}');";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
