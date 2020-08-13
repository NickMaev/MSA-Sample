using System;
using System.Collections.Generic;

namespace Components.Tests
{
    public class DoubleMeasurementError : IEqualityComparer<double>
    {
        private readonly int _error;

        public DoubleMeasurementError(int error) { 
            _error = error; 
        }

        public bool Equals(double x, double y) {
            return Math.Abs(x - y) <= _error;
        }

        public int GetHashCode(double obj)
        {
            return obj.GetHashCode();
        }
    }
}