using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;
using SuppaFlight.UWP.Code.Units;

namespace SuppaFlight.UWP.Code
{
    public class MainViewModel : ReactiveObject, IDisposable
    {
        public MainViewModel(FileOpenCommands fileOpenCommands, INavigationService navigationService)
        {
            UnitPack = UnitPacks.First();
            FileOpenCommands = fileOpenCommands;

            var flightDataObs = fileOpenCommands.OpenDataCommand;
            var videoObs = fileOpenCommands.OpenVideoCommand;

            var dataBatch = flightDataObs.Select(data => data.Statuses);
            var measurementUnitObs = this.WhenAnyValue(x => x.UnitPack);

            ISubject<Unit> runSubject = new Subject<Unit>();

            dataBatch.CombineLatest(videoObs, measurementUnitObs, (statuses, file, unit) => new {statuses, file, unit})
                .Where(x => x.file != null && x.statuses != null)
                .Zip(runSubject, (x, _) => new { x.statuses, x.file, x.unit})
                .Subscribe(obj =>
                {
                    navigationService.Navigate(new FlightViewModel(obj.statuses, obj.file, unitPack));
                    MessageBus.Current.SendMessage(Unit.Default, "Play");
                })
                .DisposeWith(disposables);

            var canRunObs = FileOpenCommands.OpenDataCommand.Any().CombineLatest(FileOpenCommands.OpenVideoCommand.Any(), (a, b) => a && b);
            RunCommand = ReactiveCommand.Create(() => { }, canRunObs);
            RunCommand
                .Subscribe(runSubject)
                .DisposeWith(disposables);

            hasDataHelper = flightDataObs.Any().ToProperty(this, x => x.HasData);
            var someVideo = videoObs.Any();
            someVideo.Subscribe(_ => { });
            hasVideoHelper = someVideo.ToProperty(this, x => x.HasVideo);
        }

        public bool HasVideo => hasVideoHelper.Value;

        public bool HasData => hasDataHelper.Value;

        public ReactiveCommand<Unit, Unit> RunCommand { get; }

        public FileOpenCommands FileOpenCommands { get; }

        private readonly ObservableAsPropertyHelper<bool> hasDataHelper;
        private readonly ObservableAsPropertyHelper<bool> hasVideoHelper;
        private UnitPack unitPack;
        private readonly CompositeDisposable disposables = new CompositeDisposable();

        public UnitPack UnitPack
        {
            get => unitPack;
            set => this.RaiseAndSetIfChanged(ref unitPack, value);
        }

        public ICollection<UnitPack> UnitPacks => UnitSource.UnitPacks;

        public void Dispose()
        {
            hasDataHelper?.Dispose();
            hasVideoHelper?.Dispose();
            disposables?.Dispose();
            RunCommand?.Dispose();
        }
    }
}