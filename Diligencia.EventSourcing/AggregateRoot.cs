using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Diligencia.EventSourcing
{
    public class AggregateRoot
    {
        public Guid Id { get; set; }

        public void FromHistory(List<Event> events)
        {
            foreach (var @event in events)
            {
                ApplyEvent(@event);
                Id = @event.AggregateRootId;
            }
        }

        private void ApplyEvent(Event @event)
        {
            var methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                var parameter = method.GetParameters().FirstOrDefault();
                if (parameter != null 
                    && parameter.ParameterType == @event.GetType())
                {
                    method.Invoke(this, new object[1] { @event });
                }
            }
        }
    }
}
