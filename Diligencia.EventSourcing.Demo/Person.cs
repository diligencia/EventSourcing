using Diligencia.EventSourcing.Demo.Events;

namespace Diligencia.EventSourcing.Demo
{
    public class Person : AggregateRoot
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public override string ToString()
        {
            return $"{Name} is {Age} years old.";
        }

        private void Apply(PersonCreatedEvent @event)
        {
            Id = @event.AggregateRootId;
            Name = @event.Name;
            Age = @event.Age;
        }

        private void Apply(NameChangedEvent @event)
        {
            Name = @event.Name;
        }

        private void Apply(AgeChangedEvent @event)
        {
            Age = @event.Age;
        }
    }
}
