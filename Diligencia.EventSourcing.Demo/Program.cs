using Diligencia.EventSourcing.AzureEventStore;
using Diligencia.EventSourcing.Demo.Commands;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Diligencia.EventSourcing.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();

            Guid id = Guid.NewGuid();

            CreateNewPersonCommand newPersonCommand = new CreateNewPersonCommand
            {
                AggregateRootId = id,
                Age = 28,
                Name = "John Doe"
            };

            ChangeNameCommand nameToJane = new ChangeNameCommand
            {
                AggregateRootId = id,
                Name = "Jane Doe"
            };

            ChangeAgeCommand ageTo29Command = new ChangeAgeCommand
            {
                AggregateRootId = id,
                Age = 29
            };

            ChangeAgeCommand ageTo30Command = new ChangeAgeCommand
            {
                AggregateRootId = id,
                Age = 30
            };

            StorageEventStore store = new StorageEventStore(config["storageConnectionString"]);

            StateConnector connector = new StateConnector(store);

            PersonCommandHandler personCommandHandler = new PersonCommandHandler(connector);
            personCommandHandler.Handle(newPersonCommand);
            personCommandHandler.Handle(nameToJane);
            personCommandHandler.Handle(ageTo29Command);
            personCommandHandler.Handle(ageTo30Command);

            var foundPerson = connector.Get<Person>(id);

            Console.WriteLine(foundPerson.ToString());
        }
    }
}
