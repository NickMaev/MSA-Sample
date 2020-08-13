using Contracts.Dtos;
using Contracts.Enums;

namespace Contracts.Components
{
    /// <summary>
    /// Represents geographical distance calculator.
    /// </summary>
    public interface IGeoDistanceCalculator
    {
        /// <summary>
        /// Calculates distance between two geographical points.
        /// </summary>
        /// <param name="distanceMeasure">Measure of the result distance.</param>
        /// <param name="fromPoint">Source geographical point.</param>
        /// <param name="toPoint">Destination geographical point.</param>
        /// <returns>Distance between two geographical points.</returns>
        double Calculate(DistanceMeasure distanceMeasure, GeoPointDto fromPoint, GeoPointDto toPoint);
    }
}