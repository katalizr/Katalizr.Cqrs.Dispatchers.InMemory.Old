using System.Threading;
using System.Threading.Tasks;
using Katalizr.Cqrs.Contracts.Dispatchers;
using Katalizr.Cqrs.Dispatchers.InMemory.Tests.Commands;
using Moq;
using NFluent;
using Xunit;

namespace Katalizr.Cqrs.Dispatchers.InMemory.Tests
{
  public class InMemoryCancellableAsynchronousDispatcherTests
  {
    public InMemoryCancellableAsynchronousDispatcherTests()
    {
      ContainerHelper = new ContainerHelper();
    }

    private ContainerHelper ContainerHelper { get; }

    [Fact]
    public async Task ShouldInvokeTheHandlerWhenACommandWithNoExpectedResultIsDispatched()
    {
      // Arranges
      var command = new CommandWithoutResult();
      var dispatcher = ContainerHelper.Container.GetInstance<IAsynchronousCancellableDispatcher>();

      // Acts
      await dispatcher.Dispatch(command, CancellationToken.None);

      // Asserts
      ContainerHelper.MockedAsynchronousCancellableCommandHandlerWithoutResult.Verify(method => method.Handle(command, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ShouldInvokeTheHandlerWhenACommandWithExpectedResultIsDispatched()
    {
      // Arranges
      var expectedString = "test-string";
      var command = new CommandWithResult();
      ContainerHelper.MockedAsynchronousCancellableCommandHandlerWithResult.Setup(method => method.Handle(command, CancellationToken.None)).Returns(Task.FromResult(expectedString));
      var dispatcher = ContainerHelper.Container.GetInstance<IAsynchronousCancellableDispatcher>();

      // Acts
      var result = await dispatcher.Dispatch<CommandWithResult, string>(command, CancellationToken.None);

      // Asserts
      Check.That(result).IsEqualTo(expectedString);
      ContainerHelper.MockedAsynchronousCancellableCommandHandlerWithResult.Verify(method => method.Handle(command, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ShouldInvokeTheHandlerWhenAQueryWithExpectedResultIsDispatched()
    {
      // Arranges
      var expectedString = "test-string";
      var dispatcher = ContainerHelper.Container.GetInstance<IAsynchronousCancellableDispatcher>();
      var query = new QueryWithResult();
      ContainerHelper.MockedAsynchronousCancellableQueryHandlerWithResult.Setup(method => method.Handle(query, CancellationToken.None)).Returns(Task.FromResult(expectedString));

      // Acts
      var result = await dispatcher.Dispatch<QueryWithResult, string>(query, CancellationToken.None);

      // Asserts
      Check.That(result).IsEqualTo(expectedString);
      ContainerHelper.MockedAsynchronousCancellableQueryHandlerWithResult.Verify(method => method.Handle(query, CancellationToken.None), Times.Once);
    }
  }
}
