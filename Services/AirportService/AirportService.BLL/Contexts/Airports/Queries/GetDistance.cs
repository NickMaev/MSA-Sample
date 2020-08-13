using Contracts.Components;
using Contracts.Enums;
using FluentValidation;
using MediatR;
using Shared.Exceptions;
using Shared.Models;
using Shared.Validation;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AirportService.BLL.Contexts.Airports.Queries
{
    /// <summary>
    /// Get the distance between two airports by their IATA codes.
    /// </summary>
    public class GetDistance : IRequest<(double, Error)>
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

    public class GetDistanceValidator : AbstractValidator<GetDistance>
    {
        public GetDistanceValidator()
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
    public class GetDistanceHandler : IRequestHandler<GetDistance, (double, Error)>
    {
        private readonly IAirportInfoDataProvider _airportInfoDataProvider;
        private readonly IGeoDistanceCalculator _geoDistanceCalculator;

        public GetDistanceHandler(
            IGeoDistanceCalculator geoDistanceCalculator,
            IAirportInfoDataProvider airportInfoDataProvider
            )
        {
            _geoDistanceCalculator = geoDistanceCalculator ?? throw new ArgumentNullException(nameof(geoDistanceCalculator));
            _airportInfoDataProvider = airportInfoDataProvider ?? throw new ArgumentNullException(nameof(airportInfoDataProvider));
        }

        public async Task<(double, Error)> Handle(GetDistance message, CancellationToken token)
        {
            try
            {
                var sourceAirportInfoTask =
                    _airportInfoDataProvider
                    .GetAirportInfoAsync(message.SourceIataCode, token);

                var destinationAirportInfoTask =
                    _airportInfoDataProvider
                    .GetAirportInfoAsync(message.DestinationIataCode, token);

                await Task
                    .WhenAll(sourceAirportInfoTask, destinationAirportInfoTask)
                    .ConfigureAwait(false);

                var sourcePoint = await sourceAirportInfoTask;

                var destinationPoint = await destinationAirportInfoTask;

                var result =
                    _geoDistanceCalculator
                    .Calculate(
                        message.DistanceMeasure,
                        sourcePoint.GeoPoint,
                        destinationPoint.GeoPoint
                        );

                return (result, null);
            }
            catch (ArgumentException e)
            {
                return (default, new Error(HttpStatusCode.BadRequest, e.Message));
            }
            catch (EntityNotFoundException e)
            {
                return (default, new Error(HttpStatusCode.NotFound, e.Message));
            }
        }
    }
}