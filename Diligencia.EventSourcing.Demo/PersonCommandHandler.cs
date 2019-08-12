using Diligencia.EventSourcing.Demo.Commands;
using Diligencia.EventSourcing.Demo.Events;

namespace Diligencia.EventSourcing.Demo
{
    public class PersonCommandHandler :
        ICommandHandler<CreateNewPersonCommand>,
        ICommandHandler<ChangeAgeCommand>,
        ICommandHandler<ChangeNameCommand>
    {
        private StateConnector _stateConnector;

        public PersonCommandHandler(StateConnector connector)
        {
            _stateConnector = connector;
        }

        public void Handle(CreateNewPersonCommand command)
        {
            var person = _stateConnector.Get<Person>(command.AggregateRootId);

            if (person == null)
            {
                _stateConnector.Save(new PersonCreatedEvent { AggregateRootId = command.AggregateRootId, Age = command.Age, Name = command.Name });
            }
        }

        public void Handle(ChangeAgeCommand command)
        {
            var person = _stateConnector.Get<Person>(command.AggregateRootId);

            if (person != null)
            {
                _stateConnector.Save(new AgeChangedEvent { AggregateRootId = command.AggregateRootId, Age = command.Age });
            }
        }

        public void Handle(ChangeNameCommand command)
        {
            var person = _stateConnector.Get<Person>(command.AggregateRootId);

            if (person != null)
            {
                _stateConnector.Save(new NameChangedEvent { AggregateRootId = command.AggregateRootId, Name = command.Name });
            }
        }
    }
}
