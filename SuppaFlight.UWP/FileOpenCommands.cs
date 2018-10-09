using System;
using System.IO;
using System.Reactive;
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
            OpenDataCommand = ReactiveCommand.CreateFromTask(Load);
            OpenVideoCommand = ReactiveCommand.CreateFromTask(LoadVideo);           
        }

        public ReactiveCommand<Unit, StorageFile> OpenVideoCommand { get; }

        public ReactiveCommand<Unit, FlightData> OpenDataCommand { get; }

        private async Task<FlightData> Load()
        {
            var picker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.List,
                CommitButtonText = "Seleccionar",
                FileTypeFilter = { ".json" },
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            var file = await picker.PickSingleFileAsync();
            using (var stream = await file.OpenStreamForReadAsync())
            {
                return FlightDataReader.Read(stream);
            }
        }

        private async Task<StorageFile> LoadVideo()
        {
            var picker = new FileOpenPicker()
            {
                ViewMode = PickerViewMode.List,
                CommitButtonText = "Seleccionar",
                FileTypeFilter = { ".mp4" },
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };

            var file = await picker.PickSingleFileAsync();
            return file;
        }
    }
}