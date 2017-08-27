using System.Threading.Tasks;
using Katalizr.Cqrs.Contracts.Dispatchers;
using Katalizr.Cqrs.Dispatchers.InMemory.Tests.Commands;
using Moq;
using NFluent;
using Xunit;

namespace Katalizr.Cqrs.Dispatchers.InMemory.Tests
{
  public class InMemoryAsynchronousDispatcherTests
  {
    public InMemoryAsynchronousDispatcherTests()
    {
      ContainerHelper = new ContainerHelper();
    }

    private ContainerHelper ContainerHelper { get; }

    [Fact]
    public async Task ShouldInvokeTheHandlerWhenACommandWithNoExpectedResultIsDispatched()
    {
      // Arranges
      var command = new CommandWithoutResult();
      var dispatcher = ContainerHelper.Container.GetInstance<IAsynchronousDispatcher>();

      // Acts
      await dispatcher.Dispatch(command);

      // Asserts
      ContainerHelper.MockedAsynchronousCommandHandlerWithoutResult.Verify(method => method.Handle(command), Times.Once);
    }

    [Fact]
    public async Task ShouldInvokeTheHandlerWhenACommandWithExpectedResultIsDispatched()
    {
      // Arranges
      var expectedString = "test-string";
      var command = new CommandWithResult();
      ContainerHelper.MockedAsynchronousCommandHandlerWithResult.Setup(method => method.Handle(command)).Returns(Task.FromResult(expectedString));
      var dispatcher = ContainerHelper.Container.GetInstance<IAsynchronousDispatcher>();

      // Acts
      var result = await dispatcher.Dispatch<CommandWithResult, string>(command);

      // Asserts
      Check.That(result).IsEqualTo(expectedString);
      ContainerHelper.MockedAsynchronousCommandHandlerWithResult.Verify(method => method.Handle(command), Times.Once);
    }

    [Fact]
    public async Task ShouldInvokeTheHandlerWhenAQueryWithExpectedResultIsDispatched()
    {
      // Arranges
      var expectedString = "test-string";
      var dispatcher = ContainerHelper.Container.GetInstance<IAsynchronousDispatcher>();
      var query = new QueryWithResult();
      ContainerHelper.MockedAsynchronousQueryHandlerWithResult.Setup(method => method.Handle(query)).Returns(Task.FromResult(expectedString));

      // Acts
      var result = await dispatcher.Dispatch<QueryWithResult, string>(query);

      // Asserts
      Check.That(result).IsEqualTo(expectedString);
      ContainerHelper.MockedAsynchronousQueryHandlerWithResult.Verify(method => method.Handle(query), Times.Once);
    }
  }
}
