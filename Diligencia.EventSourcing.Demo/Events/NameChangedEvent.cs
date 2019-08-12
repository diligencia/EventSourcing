using System;
using System.Collections.Generic;
using System.Text;

namespace Diligencia.EventSourcing.Demo.Events
{
    public class NameChangedEvent : Event
    {
        public string Name { get; set; }
    }
}
