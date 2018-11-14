using System.Collections.Generic;
using ParrotDiscoReflight.Controls;

namespace ParrotDiscoReflight.ViewModels
{
    public interface IPlottableViewModel
    {
        IList<double> Values { get; }
        IList<double> SampledValues { get; }
        double Maximum { get; }
        Point CurrentValue { get; }
    }
}