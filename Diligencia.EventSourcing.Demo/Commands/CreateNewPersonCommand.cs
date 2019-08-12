using System;
using System.Collections.Generic;
using System.Text;

namespace Diligencia.EventSourcing.Demo.Commands
{
    public class CreateNewPersonCommand : Command
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
