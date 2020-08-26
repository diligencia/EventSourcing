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
                var commandHandlerTypes = assembly
                    .GetTypes()
                    .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition().IsAssignableFrom(typeof(ICommandHandler<>))))
                    .ToList();

                foreach (var commandHandlerType in commandHandlerTypes)
                {
                    List<Type> allGenericTypes = commandHandlerType.GetInterfaces().SelectMany(i => i.GetGenericArguments()).ToList();

                    foreach (var genericType in allGenericTypes)
                    {
                        var serviceType = typeof(ICommandHandler<>).MakeGenericType(genericType);

                        services.AddTransient(serviceType, commandHandlerType);
                    }
                }
            }

            return services;
        }
    }
}
