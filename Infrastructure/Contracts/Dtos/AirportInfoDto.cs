namespace Contracts.Dtos
{
    /// <summary>
    /// Represents the airport info.
    /// </summary>
    public class AirportInfoDto
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
        /// Contains the geographic point where the aiport is located.
        /// </summary>
        public GeoPointDto GeoPoint { get; set; }
    }
}