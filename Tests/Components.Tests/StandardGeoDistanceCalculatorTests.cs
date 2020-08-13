using Contracts.Dtos;
using Contracts.Enums;
using Xunit;

namespace Components.Tests
{
    public class StandardGeoDistanceCalculatorTests
    {
        // https://www.daftlogic.com/projects-google-maps-distance-calculator.htm
        // https://places-dev.cteleport.com/airports/SYD

        [Theory]
        // AMS - PEK
        [InlineData(52.309069, 4.763385, 40.078538, 116.587095, 4869.238)]
        // MIA - SYD
        [InlineData(25.796, -80.278234, -33.932922, 151.179898, 9345.154)]
        // ALA - DXB
        [InlineData(43.346652, 77.011455, 24.892845, 55.162467, 1776.610)]
        public void CalculateDistanceBetweenAirports(
            double sourceLatitude,
            double sourceLongtitude,
            double destinationLatitude,
            double destinationLongtitude,
            double expectedDistance
            )
        {
            var sourcePoint = new GeoPointDto()
            {
                Latitude = sourceLatitude,
                Longtitude = sourceLongtitude
            };

            var destinationPoint = new GeoPointDto()
            {
                Latitude = destinationLatitude,
                Longtitude = destinationLongtitude
            };

            var calculator = new StandardGeoDistanceCalculator();

            var result = 
                calculator
                .Calculate(DistanceMeasure.Miles, sourcePoint, destinationPoint);

            Assert.Equal(expectedDistance, result, new DoubleMeasurementError(11));
        }
    }
}
