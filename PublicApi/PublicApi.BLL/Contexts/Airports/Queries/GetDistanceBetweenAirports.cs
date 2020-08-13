using Contracts.Enums;
using Contracts.MessageBus.AirportService;
using FluentValidation;
using MediatR;
using Shared.Models;
using Shared.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

using GetAirportDistanceClient =
    Shared.MessageBus.IBusClient<
        Contracts.MessageBus.AirportService.GetAirportDistanceRequest,
        Contracts.MessageBus.AirportService.GetAirportDistanceResponse
    >;

namespace PublicApi.BLL.Contexts.Airports.Queries
{
    /// <summary>
    /// Get the distance between two airports by their IATA codes.
    /// </summary>
    public class GetDistanceBetweenAirports : IRequest<(double, Error)>
    {
        /// <summary>
        /// Result measure.
        /// </summary>
        public DistanceMeasure DistanceMeasure { get; set; }

        /// <summary>
        /// Source airport's IATA code.
        /// </summary>
        public string SourceIataCode { get; set; }

        /// <summary>
        /// Destination airport's IATA code.
        /// </summary>
        public string DestinationIataCode { get; set; }
    }

    public class GetDistanceBetweenAirportsValidator : AbstractValidator<GetDistanceBetweenAirports>
    {
        public GetDistanceBetweenAirportsValidator()
        {
            RuleFor(x => x.SourceIataCode)
                .Must(IataCodeValidator.IsValid)
                .WithMessage("Source IATA code is invalid.");
            RuleFor(x => x.DestinationIataCode)
                .Must(IataCodeValidator.IsValid)
                .WithMessage("Destination IATA code is invalid.");
            RuleFor(x => new { x.SourceIataCode, x.DestinationIataCode })
                .Must(x => !x.SourceIataCode.Equals(x.DestinationIataCode))
                .WithMessage("Source IATA code should not be equal to destination IATA code.");
        }
    }

    /// <summary>
    /// Gets the distance between two airports by their IATA codes.
    /// </summary>
    public class GetDistanceBetweenAirportsHandler : IRequestHandler<GetDistanceBetweenAirports, (double, Error)>
    {
        private readonly GetAirportDistanceClient _getAirportDistanceClient;

        public GetDistanceBetweenAirportsHandler(GetAirportDistanceClient getAirportDistanceClient)
        {
            _getAirportDistanceClient = 
                getAirportDistanceClient ?? throw new ArgumentNullException(nameof(getAirportDistanceClient));
        }

        public async Task<(double, Error)> Handle(GetDistanceBetweenAirports message, CancellationToken token)
        {
            var request = new GetAirportDistanceRequest()
            {
                SourceIataCode = message.SourceIataCode,
                DestinationIataCode = message.DestinationIataCode,
                DistanceMeasure = message.DistanceMeasure
            };

            var (result, error) = await _getAirportDistanceClient.GetResponseAsync(request, token);

            if(error != null)
            {
                return (default, error);
            }

            return (result.Distance, null);
        }
    }
}