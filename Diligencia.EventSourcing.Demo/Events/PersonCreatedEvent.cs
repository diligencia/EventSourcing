using System;
using System.Collections.Generic;
using System.Text;

namespace Diligencia.EventSourcing.Demo.Events
{
    public class PersonCreatedEvent : Event
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
