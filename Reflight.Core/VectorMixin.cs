using System;
using System.Linq;

namespace Reflight.Core
{
    public static class VectorMixin
    {
        public static double L2Norm(this Vector self)
        {
            return Math.Sqrt(self.Coordinates.Sum(arg => Math.Pow(arg, 2)));
        }
    }
}