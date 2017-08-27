using System;
using Katalizr.Cqrs.Contracts.Handlers.Queries;

namespace Katalizr.Cqrs.Dispatchers.InMemory
{
  /// <summary>
  /// Factory method for creating single instances. Used to build instances of
  /// <see cref="ICommandHandler{TCommand,TResponse}"/>,
  /// <see cref="IAsynchronousCommandHandler{TRequest,TResponse}"/>,
  /// <see cref="IQueryHandler{TQuery,TResponse}"/>,
  /// <see cref="IAsynchronousQueryHandler{TRequest,TResponse}"/>
  /// </summary>
  /// <param name="serviceType">Type of service to resolve</param>
  /// <returns>An instance of type <paramref name="serviceType" /></returns>
  public delegate object SingleInstanceFactory(Type serviceType);
}
