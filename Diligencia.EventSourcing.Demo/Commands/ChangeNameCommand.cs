using System;
using System.Collections.Generic;
using System.Text;

namespace Diligencia.EventSourcing.Demo.Commands
{
    public class ChangeNameCommand : Command
    {
        public string Name { get; set; }
    }
}
