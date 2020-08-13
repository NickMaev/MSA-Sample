using Contracts.Components;
using Contracts.Dtos;
using Contracts.Enums;
using CoordinateSharp;
using System;

namespace Components
{
    /// <summary>
    /// Represents the standard implementation of the geographical distance calculator.
    /// </summary>
    public class StandardGeoDistanceCalculator : IGeoDistanceCalculator
    {
        /// <summary>
        /// Calculates distance between two geographical points.
        /// </summary>
        /// <param name="distanceMeasure">Measure of the result distance.</param>
        /// <param name="fromPoint">Source geographical point.</param>
        /// <param name="toPoint">Destination geographical point.</param>
        /// <returns>Distance between two geographical points.</returns>
        public double Calculate(DistanceMeasure distanceMeasure, GeoPointDto fromPoint, GeoPointDto toPoint)
        {
            if(fromPoint == null)
            {
                throw new ArgumentNullException(nameof(fromPoint));
            }

            if(toPoint == null)
            {
                throw new ArgumentNullException(nameof(toPoint));
            }

            var coordFrom = new Coordinate(fromPoint.Latitude, fromPoint.Longtitude);
            var coordTo = new Coordinate(toPoint.Latitude, toPoint.Longtitude);

            var earthShape = Shape.Sphere;
            var distance = new Distance(coordFrom, coordTo, earthShape);

            double result;

            switch (distanceMeasure)
            {
                case DistanceMeasure.Miles:
                    result = distance.Miles;
                    break;
                case DistanceMeasure.Kilometers:
                    result = distance.Kilometers;
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }
    }
}