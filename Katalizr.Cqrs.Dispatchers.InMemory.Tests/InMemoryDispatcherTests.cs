using Katalizr.Cqrs.Contracts.Dispatchers;
using Katalizr.Cqrs.Dispatchers.InMemory.Tests.Commands;
using Moq;
using NFluent;
using Xunit;

namespace Katalizr.Cqrs.Dispatchers.InMemory.Tests
{
  public class InMemoryDispatcherTests
  {
    private ContainerHelper ContainerHelper { get; }

    public InMemoryDispatcherTests()
    {
      ContainerHelper = new ContainerHelper();
    }

    [Fact]
    public void ShouldInvokeTheHandlerWhenACommandWithNoExpectedResultIsDispatched()
    {
      // Arranges
      var command = new CommandWithoutResult();
      var dispatcher = ContainerHelper.Container.GetInstance<IDispatcher>();

      // Acts
      dispatcher.Dispatch(command);

      // Asserts
      ContainerHelper.MockedSynchronousCommandHandlerWithoutResult.Verify(method => method.Handle(command), Times.Once);
    }

    [Fact]
    public void ShouldInvokeTheHandlerWhenACommandWithExpectedResultIsDispatched()
    {
      // Arranges
      var expectedString = "test-string";
      var command = new CommandWithResult();
      ContainerHelper.MockedSynchronousCommandHandlerWithResult.Setup(method => method.Handle(command)).Returns(expectedString);
      var dispatcher = ContainerHelper.Container.GetInstance<IDispatcher>();

      // Acts
      var result = dispatcher.Dispatch<CommandWithResult, string>(command);

      // Asserts
      Check.That(result).IsEqualTo(expectedString);
      ContainerHelper.MockedSynchronousCommandHandlerWithResult.Verify(method => method.Handle(command), Times.Once);
    }
  }
}
