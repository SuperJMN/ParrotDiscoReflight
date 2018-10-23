using System;
using System.Collections.Generic;
using System.Linq;

namespace Reflight.Core
{
    public class Vector
    {
        public Vector(params double[] list)
        {
            Coordinates = list;
        }

        public IEnumerable<double> Coordinates { get; }
        public static Vector Zero => new Vector(0,0,0);
    }

    public static class VectorMixin
    {
        public static double L2Norm(this Vector self)
        {
            return Math.Sqrt(self.Coordinates.Sum(arg => Math.Pow(arg, 2)));
        }
    }

    
}