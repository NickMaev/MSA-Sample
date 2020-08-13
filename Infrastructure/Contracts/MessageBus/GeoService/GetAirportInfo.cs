namespace Contracts.MessageBus.AirportService
{
    /// <summary>
    /// Get the airport info by its IATA code.
    /// </summary>
    public class GetAirportInfoRequest
    {
        /// <summary>
        /// Airport's IATA code.
        /// </summary>
        public string IataCode { get; set; }
    }

    /// <summary>
    /// Contains airport info by its IATA code.
    /// </summary>
    public class GetAirportInfoResponse
    {
        /// <summary>
        /// Airport location country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Airport location city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Airport's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Hub count.
        /// </summary>
        public int HubCount { get; set; }

        /// <summary>
        /// Geographic part of the airport's location.
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Geographic part of the airport's location.
        /// </summary>
        public double? Longtitude { get; set; }
    }
}