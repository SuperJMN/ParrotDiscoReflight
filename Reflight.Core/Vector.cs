using System.Collections.Generic;

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
}