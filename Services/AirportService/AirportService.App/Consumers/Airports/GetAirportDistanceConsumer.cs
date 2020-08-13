using Contracts.MessageBus.AirportService;
using AirportService.BLL.Contexts.Airports.Queries;
using MassTransit;
using MediatR;
using System;
using System.Threading.Tasks;

namespace AirportService.App.Consumers.Airports
{
    public class GetAirportDistanceConsumer : IConsumer<GetAirportDistanceRequest>
    {
        private readonly IMediator _mediator;

        public GetAirportDistanceConsumer(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Consume(ConsumeContext<GetAirportDistanceRequest> context)
        {
            var query = new GetDistance() {
                SourceIataCode = context.Message.SourceIataCode,
                DestinationIataCode = context.Message.DestinationIataCode,
                DistanceMeasure = context.Message.DistanceMeasure
            };

            var queryResult = await _mediator.Send(query, context.CancellationToken);

            if (queryResult.Item2 != null)
            {
                await context.RespondAsync(queryResult.Item2);
            }
            else
            {
                var response = new GetAirportDistanceResponse
                {
                    Distance = queryResult.Item1
                };

                await context.RespondAsync(response);
            }
        }
    }
}