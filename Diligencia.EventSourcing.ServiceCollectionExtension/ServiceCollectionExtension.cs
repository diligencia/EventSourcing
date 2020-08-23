using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diligencia.EventSourcing.ServiceCollectionExtension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddEventSourcing(this IServiceCollection services)
        {
            services.AddTransient<StateConnector>();
            services.AddTransient<CommandHandler>();
            services.AddTransient<EventPublisher, EventPublisher>();

            // Scan assemblies for classes that implement ICommandHandler
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> types = assembly.GetTypes().Where(type => type.IsAssignableFrom(typeof(ICommandHandler<>)) && !type.IsAbstract);

                foreach (Type type in types)
                {
                    services.AddTransient(type);
                }
            }

            return services;
        }
    }
}
