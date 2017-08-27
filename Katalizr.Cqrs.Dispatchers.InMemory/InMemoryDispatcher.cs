using System;
using System.Threading;
using System.Threading.Tasks;
using Katalizr.Cqrs.Contracts.Dispatchers;
using Katalizr.Cqrs.Contracts.Handlers.Commons;
using Katalizr.Cqrs.Contracts.Models;

namespace Katalizr.Cqrs.Dispatchers.InMemory
{
  public class InMemoryDispatcher : IDispatcher, IAsynchronousDispatcher, IAsynchronousCancellableDispatcher
  {
    private SingleInstanceFactory SingleInstanceFactory { get; }

    public InMemoryDispatcher(SingleInstanceFactory singleInstanceFactory)
    {
      SingleInstanceFactory = singleInstanceFactory;
    }

//        public void Dispatch<TRequest>(TRequest request) where TRequest : IRequest
//        {
//            HandlerResolver
//                .GetHandler<TRequest, ISynchronousRequestHandler<TRequest>>(SingleInstanceFactory)
//                ?.Handle(request);
//        }
//
//        public Task Dispatch<TRequest>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequest
//        {
//            return HandlerResolver
//                .GetHandler<TRequest, IAsynchronousCancellableRequestHandler<TRequest>>(SingleInstanceFactory)
//                ?.Handle(request, cancellationToken);
//        }
//
//        Task<TResponse> IAsynchronousDispatcher.Dispatch<TRequest, TResponse>(TRequest request)
//        {
//            return HandlerResolver
//                .GetHandler<TRequest, TResponse, IAsynchronousRequestHandler<TRequest, TResponse>>(SingleInstanceFactory)
//                ?.Handle(request);
//        }
//
//        public Task<TResponse> Dispatch<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequest
//        {
//            return HandlerResolver
//                .GetHandler<TRequest, TResponse, IAsynchronousCancellableRequestHandler<TRequest, TResponse>>(SingleInstanceFactory)
//                ?.Handle(request, cancellationToken);
//        }
//
//        Task IAsynchronousDispatcher.Dispatch<TRequest>(TRequest request)
//        {
//            return HandlerResolver
//                .GetHandler<TRequest, IAsynchronousRequestHandler<TRequest>>(SingleInstanceFactory)
//                ?.Handle(request);
//        }
//
//        public TResponse Dispatch<TRequest, TResponse>(TRequest request) where TRequest : IRequest
//        {
//            var handler = HandlerResolver
//                .GetHandler<TRequest, TResponse, ISynchronousRequestHandler<TRequest, TResponse>>(SingleInstanceFactory);
//            return handler != null ? handler.Handle(request) : default(TResponse);
//
//        }

    #region Synchronous Dispatcher

    void IDispatcher.Dispatch<TRequest>(TRequest request)
    {
      HandlerResolver.GetHandler<TRequest, ISynchronousRequestHandler<TRequest>>(SingleInstanceFactory)?.Handle(request);
    }

    TResponse IDispatcher.Dispatch<TRequest, TResponse>(TRequest request)
    {
      var handler = HandlerResolver.GetHandler<TRequest, TResponse, ISynchronousRequestHandler<TRequest, TResponse>>(SingleInstanceFactory);
      return handler != null ? handler.Handle(request) : default(TResponse);
    }

    #endregion

    #region Asynchronous Dispatcher

    Task IAsynchronousDispatcher.Dispatch<TRequest>(TRequest request)
    {
      return HandlerResolver
        .GetHandler<TRequest, IAsynchronousRequestHandler<TRequest>>(SingleInstanceFactory)
        ?.Handle(request);
    }

    Task<TResponse> IAsynchronousDispatcher.Dispatch<TRequest, TResponse>(TRequest request)
    {
      return HandlerResolver
        .GetHandler<TRequest, TResponse, IAsynchronousRequestHandler<TRequest, TResponse>>(SingleInstanceFactory)
        ?.Handle(request);
    }

    #endregion

    #region Asynchronous Cancellable Dispatcher

    public Task Dispatch<TRequest>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequest
    {
      return HandlerResolver
        .GetHandler<TRequest, IAsynchronousCancellableRequestHandler<TRequest>>(SingleInstanceFactory)
        ?.Handle(request, cancellationToken);
    }

    public Task<TResponse> Dispatch<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken) where TRequest : IRequest<TResponse>
    {
      return HandlerResolver
        .GetHandler<TRequest, TResponse, IAsynchronousCancellableRequestHandler<TRequest, TResponse>>(SingleInstanceFactory)
        ?.Handle(request, cancellationToken);
    }

    #endregion
  }
}
