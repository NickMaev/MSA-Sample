namespace Contracts.Dtos
{
    /// <summary>
    /// Represents the point in geographic coordinate system.
    /// </summary>
    public class GeoPointDto
    {
        /// <summary>
        /// Specifies the north–south position of a point on the Earth's surface.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Specifies the east–west position of a point on the Earth's surface.
        /// </summary>
        public double Longtitude { get; set; }
    }
}