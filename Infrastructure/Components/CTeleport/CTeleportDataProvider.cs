using AutoMapper;
using Contracts.Components;
using Contracts.Dtos;
using Shared.Exceptions;
using Shared.Validation;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Components.CTeleport
{
    /// <summary>
    /// Represents the informational grabber from "CTeleport" service.
    /// </summary>
    public class CTeleportDataProvider : IAirportInfoDataProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CTeleportDataProviderSettings _settings;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates a new instance of the <see cref="CTeleportDataProvider" />.
        /// </summary>
        /// <param name="httpClientFactory"><see cref="HttpClient" /> factory.</param>
        /// <param name="settings">Represents settings of the CTeleport's data provider.</param>
        /// <param name="mapper">Represents the Automapper instance.</param>
        public CTeleportDataProvider(
            CTeleportDataProviderSettings settings,
            IHttpClientFactory httpClientFactory,
            IMapper mapper
            )
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets the <see cref="AirportInfoDto" /> from "CTeleport" by IATA code.
        /// </summary>
        /// <param name="iataCode">Airport's IATA code.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Airport info.</returns>
        /// <exception cref="ArgumentException">IATA code is null, empty or invalid.</exception>
        /// <exception cref="EntityNotFoundException">Airport not found by requested IATA code.</exception>
        /// <exception cref="HttpRequestException">Unsuccessfull HTTP request.</exception>
        /// <exception cref="InvalidOperationException">Invalid response.</exception>
        public async Task<AirportInfoDto> GetAirportInfoAsync(string iataCode, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(iataCode))
            {
                throw new ArgumentException(nameof(iataCode));
            }

            if (!IataCodeValidator.IsValid(iataCode))
            {
                throw new ArgumentException($"Invalid IATA code.");
            }

            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Clear();

            var uriBuilder = new UriBuilder(_settings.AirportInfoRequestUrl);

            uriBuilder.Path += $"/{iataCode}";

            var response = 
                await httpClient.GetAsync(uriBuilder.Uri, cancellationToken)
                .ConfigureAwait(false);

            if(response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new EntityNotFoundException($"Airport not found by requested IATA code.");
            }

            response.EnsureSuccessStatusCode();

            var responseModel =
                await response
                .Content
                .ReadFromJsonAsync<CTeleportAirportInfoDto>(cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            if (responseModel?.Location == null)
            {
                throw new InvalidOperationException($"Invalid response.");
            }

            var resultModel = _mapper.Map<AirportInfoDto>(responseModel);

            return resultModel;
        }
    }
}