using Contracts.Dtos;
using Shared.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Contracts.Components
{
    /// <summary>
    /// Represents the airport information provider.
    /// </summary>
    public interface IAirportInfoDataProvider
    {
        /// <summary>
        /// Gets the <see cref="AirportInfoDto" /> by IATA code.
        /// </summary>
        /// <param name="iataAirportCode">Airport's IATA code.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Airport info.</returns>
        /// <exception cref="ArgumentException">IATA code is null, empty or invalid.</exception>
        /// <exception cref="EntityNotFoundException">Airport not found by requested IATA code.</exception>
        Task<AirportInfoDto> GetAirportInfoAsync(string iataAirportCode, CancellationToken cancellationToken = default);
    }
}