using System;
using System.Collections.Generic;

namespace Diligencia.EventSourcing
{
    public class EventPublisher
    {
        private Dictionary<Type, List<Action>> _registrations;
        private object _registrationsLock;

        public EventPublisher()
        {
            _registrations = new Dictionary<Type, List<Action>>();
            _registrationsLock = new object();
        }

        /// <summary>
        /// Publish the occurance of <see cref="event">event</see> to all subscribed components.
        /// </summary>
        /// <param name="event"></param>
        public void Publish(Event @event)
        {
            if (_registrations.ContainsKey(@event.GetType()))
            {
                List<Action> registeredActions = _registrations[@event.GetType()];

                foreach (var registeredAction in registeredActions)
                {
                    registeredAction.Invoke();
                }
            }
        }

        /// <summary>
        /// Allows a component to subscribe for even occurances
        /// </summary>
        /// <param name="event"></param>
        public void Subscribe(Event @event, Action @action)
        {
            lock(_registrationsLock)
            {
                if (!_registrations.ContainsKey(@event.GetType()))
                {
                    _registrations[@event.GetType()] = new List<Action>();
                }

                _registrations[@event.GetType()].Add(@action);
            }
        }
    }
}
