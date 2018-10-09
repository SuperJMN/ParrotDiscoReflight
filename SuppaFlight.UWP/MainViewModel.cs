using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage.Pickers;
using FlightVisualizer.Core;
using Microsoft.Toolkit.Uwp.Helpers;
using ReactiveUI;

namespace SuppaFlight.UWP
{
    public class MainViewModel : ReactiveObject
    {
        private IList<StatusViewModel> data;

        public MainViewModel()
        {
            OpenFileCommand = ReactiveCommand.CreateFromTask(Load);
            OpenVideoCommand = ReactiveCommand.CreateFromTask(LoadVideo);

            statuses = OpenFileCommand
                .Select(x => x.Statuses.Select(y => new StatusViewModel(y)))
                .ToProperty(this, x => x.Statuses);

            var positionObs = this.WhenAnyValue(x => x.Position);

            var hasDataObs = OpenFileCommand.Any();

            var statusObs = positionObs
                .CombineLatest(hasDataObs, OpenVideoCommand.Any(), (position, dataLoaded, videoLoaded) => new { position, dataLoaded, videoLoaded })
                .Where(x => x.dataLoaded && x.videoLoaded)
                .Select(x => x.position)
                .Select(GetStatus);

            statusOh = statusObs
                .ToProperty(this, x => x.CurrentStatus);

            statusObs.Subscribe(x => MessageBus.Current.SendMessage(new BasicGeoposition()
            {
                Altitude = x.Altitude,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
            }));
        }

        public StatusViewModel CurrentStatus => statusOh.Value;

        private StatusViewModel GetStatus(TimeSpan pos)
        {
            return Statuses.SkipWhile(x => x.TimeElapsed < pos).Take(1).First();            
        }

        public ReactiveCommand<Unit, Unit> OpenVideoCommand { get; }

        public IEnumerable<StatusViewModel> Statuses => statuses.Value;
        private readonly ObservableAsPropertyHelper<IEnumerable<StatusViewModel>> statuses;
        private TimeSpan position;
        private ObservableAsPropertyHelper<StatusViewModel> statusOh;

        public ReactiveCommand<Unit, FlightData> OpenFileCommand { get; }

        public TimeSpan Position
        {
            get => position;
            set => this.RaiseAndSetIfChanged(ref position, value);
        }

        public Uri Uri { get; set; }

        private async Task<FlightData> Load()
        {
            var picker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.List,
                CommitButtonText = "Seleccionar",
                FileTypeFilter = { ".json"},
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            var file = await picker.PickSingleFileAsync();
            using (var stream = await file.OpenStreamForReadAsync())
            {
                return FlightDataReader.Read(stream);                
            }           
        }

        private async Task LoadVideo()
        {
            var picker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.List,
                CommitButtonText = "Seleccionar",
                FileTypeFilter = { ".mp4" },
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            var file = await picker.PickSingleFileAsync();
            MessageBus.Current.SendMessage(file);
        }
    }
}