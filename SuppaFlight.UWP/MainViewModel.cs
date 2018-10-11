using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using FlightVisualizer.Core;
using ReactiveUI;

namespace SuppaFlight.UWP
{
    public class MainViewModel : ReactiveObject
    {
        public MainViewModel(FileOpenCommands fileOpenCommands)
        {
            MeasurementUnit = MeasurementUnits.First(x => x.Abbreviation.Equals("Km/h", StringComparison.InvariantCultureIgnoreCase));
            FileOpenCommands = fileOpenCommands;

            var flightDataObs = fileOpenCommands.OpenDataCommand;
            var dataBatch = flightDataObs.Select(data => data.Statuses);
            var fileObs = fileOpenCommands.OpenVideoCommand
                .Do(sf => MessageBus.Current.SendMessage(sf));

            var positionObs = this.WhenAnyValue(x => x.Position);

            var isRunningObs = this.WhenAnyValue(x => x.IsRunning);
            var measurementUnitObs = this.WhenAnyValue(x => x.MeasurementUnit);

            var statusObs = dataBatch
                .CombineLatest(fileObs, positionObs, isRunningObs, measurementUnitObs, (statuses, file, pos, isRunning, unit) => new { statuses, file, pos, isRunning, unit })
                .Where(x => x.file != null && x.statuses != null && isRunning)
                .Select(arg => arg.statuses.SkipWhile(x => x.TimeElapsed < arg.pos).Take(1).First().ConvertTo(arg.unit));

            dataBatch
                .Where(x => x != null)
                .Subscribe(x => MessageBus.Current.SendMessage(x.First().Geoposition(), "First"));

            var canRunObs = FileOpenCommands.OpenDataCommand.Any().CombineLatest(FileOpenCommands.OpenVideoCommand.Any(), (a, b) => a && b);
            RunCommand = ReactiveCommand.Create(() => { }, canRunObs);

            statusHelper = statusObs
                .Select(x => new StatusViewModel(x))
                .ToProperty(this, x => x.Status);

            statusObs.Subscribe(x => MessageBus.Current.SendMessage(x.Geoposition()));

            RunCommand.Subscribe(unit =>
            {
                MessageBus.Current.SendMessage(Unit.Default, "Play");
                IsRunning = true;
            });

            hasDataHelper = flightDataObs.Any().ToProperty(this, x => x.HasData);
            hasVideoHelper = fileObs.Any().ToProperty(this, x => x.HasVideo);
        }

        public bool HasVideo => hasVideoHelper.Value;

        public bool HasData => hasDataHelper.Value;

        public StatusViewModel Status =>statusHelper.Value;

        public ReactiveCommand<Unit, Unit> RunCommand { get; }

        public FileOpenCommands FileOpenCommands { get; }

        public TimeSpan Position
        {
            get => position;
            set => this.RaiseAndSetIfChanged(ref position, value);
        }

        private bool isRunning;
        private TimeSpan position;
        private readonly ObservableAsPropertyHelper<StatusViewModel> statusHelper;
        private readonly ObservableAsPropertyHelper<bool> hasDataHelper;
        private readonly ObservableAsPropertyHelper<bool> hasVideoHelper;
        private IMeasurementUnit measurementUnit;

        public IMeasurementUnit MeasurementUnit
        {
            get => measurementUnit;
            set => this.RaiseAndSetIfChanged(ref measurementUnit, value);
        }

        public bool IsRunning
        {
            get => isRunning;
            set => this.RaiseAndSetIfChanged(ref isRunning, value);
        }

        public ICollection<IMeasurementUnit> MeasurementUnits { get; } = new List<IMeasurementUnit>()
        {
            new MetersSecondUnit(),
            new KilometersHourUnit(),
        };
    }
}