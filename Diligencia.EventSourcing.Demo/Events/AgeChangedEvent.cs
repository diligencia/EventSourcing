using System;
using System.Collections.Generic;
using System.Text;

namespace Diligencia.EventSourcing.Demo.Events
{
    public class AgeChangedEvent : Event
    {
        public int Age { get; set; }
    }
}
