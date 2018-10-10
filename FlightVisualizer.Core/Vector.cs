using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace FlightVisualizer.Core
{
    public class Vector
    {
        public Vector(params double[] list)
        {
            Coordinates = list;
        }

        public IEnumerable<double> Coordinates { get; }
    }

    public static class VectorMixin
    {
        public static double L2Norm(this Vector self)
        {
            return Sqrt(self.Coordinates.Sum(arg => Pow(arg, 2)));
        }
    }
}