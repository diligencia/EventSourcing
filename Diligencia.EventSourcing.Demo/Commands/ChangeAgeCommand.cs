using System;
using System.Collections.Generic;
using System.Text;

namespace Diligencia.EventSourcing.Demo.Commands
{
    public class ChangeAgeCommand : Command
    {
        public int Age { get; set; }
    }
}
