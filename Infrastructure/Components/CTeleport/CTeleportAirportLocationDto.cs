using System.Text.Json.Serialization;

namespace Components.CTeleport
{
    /// <summary>
    /// Represents geographic coordinates.
    /// </summary>
    internal class CTeleportAirportLocationDto
    {
        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("lon")]
        public double Longtitude { get; set; }
    }
}