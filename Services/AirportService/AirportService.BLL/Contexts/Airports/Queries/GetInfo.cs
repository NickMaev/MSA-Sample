using Contracts.Components;
using Contracts.Dtos;
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
    /// Get the airport info by its IATA code.
    /// </summary>
    public class GetInfo : IRequest<(AirportInfoDto, Error)>
    {
        /// <summary>
        /// Airport's IATA code.
        /// </summary>
        public string IataCode { get; set; }
    }

    public class GetInfoValidator : AbstractValidator<GetInfo>
    {
        public GetInfoValidator()
        {
            RuleFor(x => x.IataCode)
                .Must(IataCodeValidator.IsValid)
                .WithMessage("IATA code is invalid.");
        }
    }

    /// <summary>
    /// Gets the airport info by its IATA code.
    /// </summary>
    public class GetInfoHandler : IRequestHandler<GetInfo, (AirportInfoDto, Error)>
    {
        private readonly IAirportInfoDataProvider _airportInfoDataProvider;

        public GetInfoHandler(IAirportInfoDataProvider airportInfoDataProvider)
        {
            _airportInfoDataProvider = airportInfoDataProvider ?? throw new ArgumentNullException(nameof(airportInfoDataProvider));
        }

        public async Task<(AirportInfoDto, Error)> Handle(GetInfo message, CancellationToken token)
        {
            try
            {
                var airportInfo = 
                    await _airportInfoDataProvider
                    .GetAirportInfoAsync(message.IataCode, token)
                    .ConfigureAwait(false);

                return (airportInfo, null);
            }
            catch (ArgumentException e)
            {
                return (null, new Error(HttpStatusCode.BadRequest, e.Message));
            }
            catch (EntityNotFoundException e)
            {
                return (null, new Error(HttpStatusCode.NotFound, e.Message));
            }
        }
    }
}