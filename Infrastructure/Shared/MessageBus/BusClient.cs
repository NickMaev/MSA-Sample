using MassTransit;
using Shared.Extensions;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.MessageBus
{
    public class BusClient<TRequest, TResponse> : IBusClient<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        private readonly IRequestClient<TRequest> _requestClient;

        public BusClient(IRequestClient<TRequest> requestClient)
        {
            _requestClient = requestClient ?? throw new ArgumentNullException(nameof(requestClient));
        }

        public async Task<(TResponse, Error)> GetResponseAsync(
            TRequest request,
            CancellationToken cancellationToken = default, 
            RequestTimeout timeout = default
            )
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var (resultTask, errorTask) = 
                await _requestClient
                .GetResponse<TResponse, Error>(request, cancellationToken, timeout);
            
            if(resultTask.IsCompletedSuccessfully())
            {
                var result = await resultTask;
                return (result.Message, null);
            }

            var error = await errorTask;
            return (null, error.Message);
        }
    }
}