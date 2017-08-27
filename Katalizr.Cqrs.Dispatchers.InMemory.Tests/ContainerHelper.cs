using Katalizr.Cqrs.Contracts.Dispatchers;
using Katalizr.Cqrs.Contracts.Handlers.Commands;
using Katalizr.Cqrs.Contracts.Handlers.Queries;
using Katalizr.Cqrs.Dispatchers.InMemory.Tests.Commands;
using Moq;
using StructureMap;

namespace Katalizr.Cqrs.Dispatchers.InMemory.Tests
{
  public class ContainerHelper
  {
    public ContainerHelper()
    {
      MockedSynchronousCommandHandlerWithoutResult = new Mock<ICommandHandler<CommandWithoutResult>>();
      MockedSynchronousCommandHandlerWithResult = new Mock<ICommandHandler<CommandWithResult, string>>();
      MockedSynchronousQueryHandlerWithResult = new Mock<IQueryHandler<QueryWithResult, string>>();
      MockedAsynchronousCommandHandlerWithoutResult = new Mock<IAsynchronousCommandHandler<CommandWithoutResult>>();
      MockedAsynchronousCommandHandlerWithResult = new Mock<IAsynchronousCommandHandler<CommandWithResult, string>>();
      MockedAsynchronousQueryHandlerWithResult = new Mock<IAsynchronousQueryHandler<QueryWithResult, string>>();
      MockedAsynchronousCancellableCommandHandlerWithoutResult = new Mock<IAsynchronousCancellableCommandHandler<CommandWithoutResult>>();
      MockedAsynchronousCancellableCommandHandlerWithResult = new Mock<IAsynchronousCancellableCommandHandler<CommandWithResult, string>>();
      MockedAsynchronousCancellableQueryHandlerWithResult = new Mock<IAsynchronousCancellableQueryHandler<QueryWithResult, string>>();
      Container = new Container(configuration =>
      {
        configuration.For<ICommandHandler<CommandWithoutResult>>().Use(MockedSynchronousCommandHandlerWithoutResult.Object);
        configuration.For<ICommandHandler<CommandWithResult, string>>().Use(MockedSynchronousCommandHandlerWithResult.Object);
        configuration.For<IQueryHandler<QueryWithResult, string>>().Use(MockedSynchronousQueryHandlerWithResult.Object);
        configuration.For<IAsynchronousCommandHandler<CommandWithoutResult>>().Use(MockedAsynchronousCommandHandlerWithoutResult.Object);
        configuration.For<IAsynchronousCommandHandler<CommandWithResult, string>>().Use(MockedAsynchronousCommandHandlerWithResult.Object);
        configuration.For<IAsynchronousQueryHandler<QueryWithResult, string>>().Use(MockedAsynchronousQueryHandlerWithResult.Object);
        configuration.For<IAsynchronousCancellableCommandHandler<CommandWithoutResult>>().Use(MockedAsynchronousCancellableCommandHandlerWithoutResult.Object);
        configuration.For<IAsynchronousCancellableCommandHandler<CommandWithResult, string>>().Use(MockedAsynchronousCancellableCommandHandlerWithResult.Object);
        configuration.For<IAsynchronousCancellableQueryHandler<QueryWithResult, string>>().Use(MockedAsynchronousCancellableQueryHandlerWithResult.Object);
        configuration.For<IDispatcher>().Use<InMemoryDispatcher>();
        configuration.For<IAsynchronousDispatcher>().Use<InMemoryDispatcher>();
        configuration.For<IAsynchronousCancellableDispatcher>().Use<InMemoryDispatcher>();
        configuration.For<SingleInstanceFactory>().Use<SingleInstanceFactory>(context => type => context.GetInstance(type));
      });
    }

    public Container Container { get; }
    public Mock<ICommandHandler<CommandWithoutResult>> MockedSynchronousCommandHandlerWithoutResult { get; }
    public Mock<ICommandHandler<CommandWithResult, string>> MockedSynchronousCommandHandlerWithResult { get; }
    public Mock<IQueryHandler<QueryWithResult, string>> MockedSynchronousQueryHandlerWithResult { get; }
    public Mock<IAsynchronousCommandHandler<CommandWithoutResult>> MockedAsynchronousCommandHandlerWithoutResult { get; }
    public Mock<IAsynchronousCommandHandler<CommandWithResult, string>> MockedAsynchronousCommandHandlerWithResult { get; }
    public Mock<IAsynchronousQueryHandler<QueryWithResult, string>> MockedAsynchronousQueryHandlerWithResult { get; }
    public Mock<IAsynchronousCancellableCommandHandler<CommandWithoutResult>> MockedAsynchronousCancellableCommandHandlerWithoutResult { get; }
    public Mock<IAsynchronousCancellableCommandHandler<CommandWithResult, string>> MockedAsynchronousCancellableCommandHandlerWithResult { get; }
    public Mock<IAsynchronousCancellableQueryHandler<QueryWithResult, string>> MockedAsynchronousCancellableQueryHandlerWithResult { get; }
  }
}
