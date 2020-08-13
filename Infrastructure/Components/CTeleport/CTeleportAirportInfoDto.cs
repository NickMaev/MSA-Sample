using System.Text.Json.Serialization;

namespace Components.CTeleport
{
    /// <summary>
    /// Represents the airport info response of the CTeleport Web API endpoint.
    /// </summary>
    internal class CTeleportAirportInfoDto
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("city_iata")]
        public string CityIataCode { get; set; }

        [JsonPropertyName("iata")]
        public string IataCode { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("timezone_region_name")]
        public string TimezoneRegionName { get; set; }

        [JsonPropertyName("country_iata")]
        public string CountryIataCode { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("hubs")]
        public int HubCount { get; set; }

        [JsonPropertyName("location")]
        public CTeleportAirportLocationDto Location { get; set; }
    }
}