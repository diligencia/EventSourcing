using System;

namespace Diligencia.EventSourcing
{
    public interface ISubscription
    {
        ISubscription Subscribe(Type eventType, Action<Event> @action);
    }
}
