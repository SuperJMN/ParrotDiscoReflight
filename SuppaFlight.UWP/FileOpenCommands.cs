using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using FlightVisualizer.Core;
using ReactiveUI;

namespace SuppaFlight.UWP
{
    public class FileOpenCommands
    {
        public FileOpenCommands()
        {
            OpenDataCommand = ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(LoadData).Where(x => x != null));
            OpenVideoCommand = ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(LoadVideo).Where(x => x != null));           
        }

        public ReactiveCommand<Unit, StorageFile> OpenVideoCommand { get; }

        public ReactiveCommand<Unit, FlightData> OpenDataCommand { get; }

        private static async Task<FlightData> LoadData()
        {
            var picker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.List,
                CommitButtonText = "Load",
                FileTypeFilter = { ".json" },
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            var file = await picker.PickSingleFileAsync();
            using (var stream = await file.OpenStreamForReadAsync())
            {
                return FlightDataReader.Read(stream);
            }
        }

        private static async Task<StorageFile> LoadVideo()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                CommitButtonText = "Load",
                FileTypeFilter = { ".mp4" },
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            var file = await picker.PickSingleFileAsync();
            return file;
        }
    }
}