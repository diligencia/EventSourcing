using Diligencia.EventSourcing.Demo.Commands;
using Diligencia.EventSourcing.ServiceCollectionExtension;
using Diligencia.EventSourcing.SqlStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddEventSourcing()
                .AddTransient<IEventStore>(s => new SqlEventStore(config["connectionstring"], s.GetService<EventPublisher>()));

            var serviceProvider = serviceCollection.BuildServiceProvider();

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

            CommandHandler commandHandler = new CommandHandler(serviceProvider);
            commandHandler.Handle(newPersonCommand);
            commandHandler.Handle(nameToJane);
            commandHandler.Handle(ageTo29Command);
            commandHandler.Handle(ageTo30Command);

            StateConnector connector = serviceProvider.GetService<StateConnector>();
            var foundPerson = connector.Get<Person>(id);

            Console.WriteLine(foundPerson.ToString());
        }
    }
}
