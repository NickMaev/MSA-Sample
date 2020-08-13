using System;
using System.Net;
using System.Threading.Tasks;
using Contracts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PublicApi.BLL.Contexts.Airports.Queries;
using Shared.Models;

namespace PublicApi.App.Controllers
{
    /// <summary>
    /// Information about airports.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AirportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AirportsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Gets the distance between two airports by two IATA codes.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /distance
        ///     {
        ///        "distanceMeasure": 1,
        ///        "sourceIataCode": "AMS",
        ///        "destinationIataCode": "ROV"
        ///     }
        ///
        /// </remarks>
        /// <param name="getDistanceQuery"></param>
        /// <response code="200">Returns the distance between two airports.</response>
        /// <response code="400">Validation errors.</response>
        /// <response code="404">If one of the IATA code doesn't exist.</response>
        /// <returns>Distance between two airports.</returns>
        [HttpPost("distance")]
        [ProducesResponseType(typeof(double), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDistance([FromBody] GetDistanceBetweenAirports getDistanceQuery)
        {
            var result = await _mediator.Send(getDistanceQuery);

            return Result(result);
        }

        /// <summary>
        /// Gets the airport info.
        /// </summary>
        /// <param name="iataCode">Airport's IATA code.</param>
        /// <response code="200">Returns the airport info.</response>
        /// <response code="400">Validation errors.</response>
        /// <response code="404">If airport not found.</response>
        /// <returns>Airport info.</returns>
        [HttpGet("{iataCode}")]
        [ProducesResponseType(typeof(AirportInfoDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Info(string iataCode)
        {
            var query = new GetAirportInfo()
            {
                IataCode = iataCode
            };

            var result = await _mediator.Send(query);

            return Result(result);
        }

        private IActionResult Result<T>((T, Error) result)
        {
            if (result.Item2 != null)
            {
                return StatusCode((int)result.Item2.StatusCode, result.Item2);
            }

            return Ok(result.Item1);
        }
    }
}