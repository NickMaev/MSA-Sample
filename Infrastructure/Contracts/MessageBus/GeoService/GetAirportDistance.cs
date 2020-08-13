using Contracts.Enums;

namespace Contracts.MessageBus.AirportService
{
    /// <summary>
    /// Get the distance between two airports.
    /// </summary>
    public class GetAirportDistanceRequest
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

    /// <summary>
    /// Get the distance between two airports.
    /// </summary>
    public class GetAirportDistanceResponse
    {
        /// <summary>
        /// Distance between two airports in requested measure.
        /// </summary>
        public double Distance { get; set; }
    }
}