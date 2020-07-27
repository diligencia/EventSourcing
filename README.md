# EventSourcing
Small library used to incorporate event sourcing (and CQRS) in your .NET application

# How to use
First determine where you want to store your events by instantiating or registering an instance of the IEventStore. The IEventStore accepts an EventPublisher parameter, that is used to publish events after they are stored.

The StateConnector is used for interactions with the Event Store. It allows for adding new Events and retrieving AggregateRoot objects, which are beeing re-contructed by replaying all events that happened.
Reconstruction of AggregateRoot will happen by calling private methods on the AggregateRoot. These private methods should be named 'Apply' and accept the Event as a parameter.

Check out the demo project for a real example.
