using System;

namespace Diligencia.EventSourcing
{
    public class CommandHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Handle<T>(T command) where T : Command
        {
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var handler = _serviceProvider.GetService(handlerType);

            (handler as ICommandHandler<T>)?.Handle(command);
        }
    }
}
