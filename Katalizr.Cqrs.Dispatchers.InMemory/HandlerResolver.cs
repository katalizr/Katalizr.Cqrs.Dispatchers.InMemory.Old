using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Katalizr.Cqrs.Contracts.Handlers.Commons;
using Katalizr.Cqrs.Contracts.Models;

namespace Katalizr.Cqrs.Dispatchers.InMemory
{
  public static class HandlerResolver
  {
    private static readonly ConcurrentDictionary<Type, object> Handlers = new ConcurrentDictionary<Type, object>();

    private static THandler GetHandler<TRequest, THandler>(SingleInstanceFactory singleInstanceFactory, ref Collection<Exception> resolveExceptions)
      where TRequest : IRequest
      where THandler : IBaseRequestHandler<TRequest>
    {
      try
      {
        var handlerType = typeof(THandler);
        return (THandler) Handlers.GetOrAdd(handlerType, singleInstanceFactory(handlerType));
      }
      catch (Exception exception)
      {
        resolveExceptions?.Add(exception);
        return default(THandler);
      }
    }

    private static THandler GetHandler<TRequest, TResponse, THandler>(SingleInstanceFactory singleInstanceFactory, ref Collection<Exception> resolveExceptions)
      where TRequest : IRequest<TResponse>
      where THandler : IBaseRequestHandler<TRequest, TResponse>
    {
      try
      {
        return (THandler) singleInstanceFactory(typeof(THandler));
      }
      catch (Exception exception)
      {
        resolveExceptions?.Add(exception);
        return default(THandler);
      }
    }

    public static THandler GetHandler<TRequest, THandler>(SingleInstanceFactory singleInstanceFactory)
      where TRequest : IRequest
      where THandler : IBaseRequestHandler<TRequest>
    {
      Collection<Exception> swallowedExceptions = null;
      return GetHandler<TRequest, THandler>(singleInstanceFactory, ref swallowedExceptions);
    }

    public static THandler GetHandler<TRequest, TResponse, THandler>(SingleInstanceFactory singleInstanceFactory)
      where TRequest : IRequest<TResponse>
      where THandler : IBaseRequestHandler<TRequest, TResponse>
    {
      Collection<Exception> swallowedExceptions = null;
      return GetHandler<TRequest, TResponse, THandler>(singleInstanceFactory, ref swallowedExceptions);
    }
  }
}
