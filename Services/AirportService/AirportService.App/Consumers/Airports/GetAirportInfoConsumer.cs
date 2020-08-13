using AutoMapper;
using Contracts.MessageBus.AirportService;
using AirportService.BLL.Contexts.Airports.Queries;
using MassTransit;
using MediatR;
using System;
using System.Threading.Tasks;

namespace AirportService.App.Consumers.Airports
{
    public class GetAirportInfoConsumer : IConsumer<GetAirportInfoRequest>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public GetAirportInfoConsumer(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task Consume(ConsumeContext<GetAirportInfoRequest> context)
        {
            var query = new GetInfo() { IataCode = context.Message.IataCode };

            var queryResult = await _mediator.Send(query, context.CancellationToken);

            if(queryResult.Item2 != null)
            {
                await context.RespondAsync(queryResult.Item2);
            } 
            else {

                var response = _mapper.Map<GetAirportInfoResponse>(queryResult.Item1);

                await context.RespondAsync(response);
            }
        }
    }
}