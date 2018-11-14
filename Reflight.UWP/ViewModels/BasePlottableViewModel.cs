using System;
using System.Collections.Generic;
using System.Linq;
using ParrotDiscoReflight.Controls;
using ReactiveUI;

namespace ParrotDiscoReflight.ViewModels
{
    public abstract class BasePlottableViewModel : ReactiveObject, IPlottableViewModel
    {
        private const int Resolution = 1500;

        public abstract IList<double> Values { get; }

        public IList<double> SampledValues
        {
            get
            {
                var total = Values.Count;
                var skip = Math.Max(total / Resolution, 1);

                return Values
                    .Select((status, i) => new { Value = status, Index = i })
                    .Where(s => s.Index % skip == 0)
                    .Select(x => x.Value)
                    .ToList();
            }
        }

        public double Maximum => Values.Max();
        public abstract Point CurrentValue { get; }
    }
}