using Diligencia.EventSourcing.UnitTests.Helpers;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Diligencia.EventSourcing.UnitTests
{
    public class EventPublisherTests
    {
        [Test]
        public void SubscribedOneEvent_EventPublished_ActionInvoked()
        {
            // Arrange
            var publisher = new EventPublisher();
            var testEvent = new TestEvent();
            Event publishedEvent = null;

            publisher.Subscribe(typeof(TestEvent), (Event @event) => { publishedEvent = @event; });

            // Act
            publisher.Publish(testEvent);

            // Assert
            publishedEvent.Should().BeSameAs(testEvent).And.NotBeNull();
        }

        [Test]
        public void SubscribedManyEventsOfSameType_EventPublished_ActionsInvoked()
        {
            // Arrange
            var publisher = new EventPublisher();
            var testEvent = new TestEvent();
            Event publishedEvent = null;
            Event secondPublishedEvent = null;

            publisher.Subscribe(typeof(TestEvent), (Event @event) => { publishedEvent = @event; })
                .Subscribe(typeof(TestEvent), (Event @event) => { secondPublishedEvent = @event; });

            // Act
            publisher.Publish(testEvent);

            // Assert
            publishedEvent.Should().BeSameAs(testEvent).And.NotBeNull();
            secondPublishedEvent.Should().BeSameAs(secondPublishedEvent).And.NotBeNull();
        }

        [Test]
        public void SubscribedDifferentEventTypes_EventPublished_OnlyPublishedTypeInvoked()
        {
            // Arrange
            var publisher = new EventPublisher();
            var testEvent = new TestEvent();
            Event publishedEvent = null;
            Event secondPublishedEvent = null;

            publisher.Subscribe(typeof(TestEvent), (Event @event) => { publishedEvent = @event; })
                .Subscribe(typeof(OtherTestEvent), (Event @event) => { secondPublishedEvent = @event; });

            // Act
            publisher.Publish(testEvent);

            // Assert
            publishedEvent.Should().BeSameAs(testEvent).And.NotBeNull();
            secondPublishedEvent.Should().BeNull();
        }

        [Test]
        public void NoSubscriptions_EventPublished_NoActionAndDoesNotThrow()
        {
            // Arrange
            var publisher = new EventPublisher();
            var testEvent = new TestEvent();
            Event publishedEvent = null;
            
            // Act
            publisher.Publish(testEvent);

            // Assert
            publishedEvent.Should().BeNull();
        }

        [Test]
        public void SubscribeToNonEvent_ThrowsArgumentException()
        {
            // Arrange
            var publisher = new EventPublisher();

            // Act
            Action subscribeAction = () => publisher.Subscribe(typeof(string), (Event @event) => { });

            // Assert
            subscribeAction.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PublishNull_ThrowsArgumentNullException()
        {
            // Arrange
            var publisher = new EventPublisher();

            // Act
            Action publishNullAction = () => publisher.Publish(null);

            // Assert
            publishNullAction.Should().Throw<ArgumentNullException>();
        }
    }
}
