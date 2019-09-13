using System;
using System.Collections.Generic;

namespace Diligencia.EventSourcing
{
    public class EventPublisher : ISubscription
    {
        private Dictionary<Type, List<Action<Event>>> _registrations;
        private object _registrationsLock;

        public EventPublisher()
        {
            _registrations = new Dictionary<Type, List<Action<Event>>>();
            _registrationsLock = new object();
        }

        /// <summary>
        /// Publish the occurance of <see cref="event">event</see> to all subscribed components.
        /// </summary>
        /// <param name="event"></param>
        public void Publish(Event @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event), "Event should not be null");

            if (_registrations.ContainsKey(@event.GetType()))
            {
                var registeredActions = _registrations[@event.GetType()];

                foreach (var registeredAction in registeredActions)
                {
                    registeredAction.Invoke(@event);
                }
            }
        }

        /// <summary>
        /// Allows a component to subscribe for even occurances
        /// </summary>
        /// <param name="event"></param>
        public ISubscription Subscribe(Type eventType, Action<Event> @action)
        {
            if (eventType.BaseType != typeof(Event)) throw new ArgumentException("Can only subscribe to Event types");

            lock(_registrationsLock)
            {
                if (!_registrations.ContainsKey(eventType))
                {
                    _registrations[eventType] = new List<Action<Event>>();
                }

                _registrations[eventType].Add(@action);
            }

            return this;
        }
    }
}
