using Diligencia.EventSourcing;
using Moq;
using NUnit.Framework;
using System;

namespace Tests
{
    public class CommandHandlerTests
    {

        [Test]
        public void CommandHandlerAvailable_ExecutesHandleOnce()
        {
            // Arrange
            var mockHandler = new Mock<ICommandHandler<MockCommand1>>();
            var command = new MockCommand1();

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(s => s.GetService(typeof(ICommandHandler<>).MakeGenericType(typeof(MockCommand1))))
                .Returns(mockHandler.Object);

            var commandHandler = new CommandHandler(mockServiceProvider.Object);

            // Act
            commandHandler.Handle(command);

            // Assert
            mockHandler.Verify(m => m.Handle(command), Times.Once);
        }

        [Test]
        public void CommandHandlerNotAvailable_DoesntExecuteHandleAndDoesntThrowException()
        {
            // Arrange
            var mockHandler = new Mock<ICommandHandler<MockCommand1>>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockCommand = new MockCommand1();

            var commandHandler = new CommandHandler(mockServiceProvider.Object);

            // Act
            commandHandler.Handle(mockCommand);

            // Assert
            mockHandler.Verify(h => h.Handle(mockCommand), Times.Never);
        }
    }

    public class MockCommand1 : Command
    {
    }
}
