using System.Collections.Generic;
using ParrotDiscoReflight.Controls;

namespace ParrotDiscoReflight.ViewModels
{
    internal class SamplePlottableViewModel : BasePlottableViewModel
    {
        public SamplePlottableViewModel(IList<double> values, Point currentValue)
        {
            Values = values;
            CurrentValue = currentValue;
        }

        public override IList<double> Values { get; }
        public override Point CurrentValue { get; }
    }
}