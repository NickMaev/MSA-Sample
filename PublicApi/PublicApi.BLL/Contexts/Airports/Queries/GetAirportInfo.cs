using AutoMapper;
using Contracts.Dtos;
using Contracts.MessageBus.AirportService;
using FluentValidation;
using MediatR;
using Shared.Models;
using Shared.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

using GetAirportInfoClient =
    Shared.MessageBus.IBusClient<
        Contracts.MessageBus.AirportService.GetAirportInfoRequest,
        Contracts.MessageBus.AirportService.GetAirportInfoResponse
    >;

namespace PublicApi.BLL.Contexts.Airports.Queries
{
    /// <summary>
    /// Get the airport info by its IATA code.
    /// </summary>
    public class GetAirportInfo : IRequest<(AirportInfoDto, Error)>
    {
        /// <summary>
        /// Airport's IATA code.
        /// </summary>
        public string IataCode { get; set; }
    }

    public class GetAirportInfoValidator : AbstractValidator<GetAirportInfo>
    {
        public GetAirportInfoValidator()
        {
            RuleFor(x => x.IataCode)
                .Must(IataCodeValidator.IsValid)
                .WithMessage("IATA code is invalid.");
        }
    }

    /// <summary>
    /// Gets the distance between two airports by their IATA codes.
    /// </summary>
    public class GetAirportInfoHandler : IRequestHandler<GetAirportInfo, (AirportInfoDto, Error)>
    {
        private readonly IMapper _mapper;
        private readonly GetAirportInfoClient _getAirportInfoClient;

        public GetAirportInfoHandler(IMapper mapper, GetAirportInfoClient getAirportInfoClient)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _getAirportInfoClient = getAirportInfoClient ?? throw new ArgumentNullException(nameof(getAirportInfoClient));
        }

        public async Task<(AirportInfoDto, Error)> Handle(GetAirportInfo message, CancellationToken token)
        {
            var request = new GetAirportInfoRequest()
            {
                IataCode = message.IataCode
            };

            var (result, error) = await _getAirportInfoClient.GetResponseAsync(request, token);

            if(error != null)
            {
                return (null, error);
            }

            var dto = _mapper.Map<AirportInfoDto>(result);

            return (dto, null);
        }
    }
}