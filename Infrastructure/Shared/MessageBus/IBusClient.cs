using MassTransit;
using Shared.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.MessageBus
{
    public interface IBusClient<TRequest, TResponse>
        where TRequest: class
        where TResponse: class
    {
        Task<(TResponse, Error)> GetResponseAsync(
            TRequest request,
            CancellationToken cancellationToken = default,
            RequestTimeout timeout = default
            );
    }
}