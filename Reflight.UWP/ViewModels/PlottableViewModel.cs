using System;
using System.Collections.Generic;
using System.Linq;
using ParrotDiscoReflight.Controls;
using ReactiveUI;

namespace ParrotDiscoReflight.ViewModels
{
    public class PlottableViewModel : ReactiveObject
    {
        private const int Resolution = 1500;

        private readonly ObservableAsPropertyHelper<Point> currentValue;

        public PlottableViewModel(IObservable<Point> valueObs, IList<double> values)
        {
            Values = values;
            currentValue = valueObs
                .ToProperty(this, x => x.CurrentValue);
        }

        public IList<double> Values { get; }

        public IList<double> SampledValues
        {
            get
            {
                var total = Values.Count;
                var skip = Math.Max(total / Resolution, 1);

                return Values
                    .Select((status, i) => new {Value = status, Index = i})
                    .Where(s => s.Index % skip == 0)
                    .Select(x => x.Value)
                    .ToList();
            }
        }

        public double Maximum => Values.Max();

        public Point CurrentValue => currentValue.Value;
    }
}