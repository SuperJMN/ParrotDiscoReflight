using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Code.Units;
using ReactiveUI;
using Reflight.Core;

namespace ParrotDiscoReflight.ViewModels
{
    public class DataViewModel : ReactiveObject, IDataViewModel
    {
        public UnitPack UnitPack { get; }
        private readonly ObservableAsPropertyHelper<StatusViewModel> statusHelper;

        public DataViewModel(IEnumerable<Status> statuses, IObservable<TimeSpan> positionObservable,
            UnitPack unitPack)
        {
            UnitPack = unitPack;
            statusHelper = positionObservable
                .Select(pos => statuses.SkipWhile(x => x.TimeElapsed < pos).Take(1).DefaultIfEmpty(Reflight.Core.Status.Zero).First().ConvertTo(unitPack))
                .Select(status => new StatusViewModel(status))
                .ToProperty(this, x => x.Status);
        }

        public StatusViewModel Status => statusHelper.Value;
    }
}