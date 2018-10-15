using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using FlightVisualizer.Core;
using ReactiveUI;

namespace SuppaFlight.UWP.Code
{
    public class FileOpenCommands
    {
        public FileOpenCommands()
        {
            OpenDataCommand =
                ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(LoadData)
                    .Where(x => x != null)
                    .SelectMany(ReadDataFromFile));

            OpenVideoCommand =
                ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(LoadVideo).Where(x => x != null));
            SaveFileCommand =
                ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(SaveVideo).Where(x => x != null));
        }

        private static async Task<FlightData> ReadDataFromFile(StorageFile file)
        {
            using (var stream = await file.OpenStreamForReadAsync())
            {
                return FlightDataReader.Read(stream);
            }
        }

        public ReactiveCommand<Unit, StorageFile> SaveFileCommand { get; }

        public ReactiveCommand<Unit, StorageFile> OpenVideoCommand { get; }

        public ReactiveCommand<Unit, FlightData> OpenDataCommand { get; }

        private async Task<StorageFile> SaveVideo()
        {
            var picker = new FileSavePicker
            {
                CommitButtonText = "Export",
                DefaultFileExtension = ".mp4",
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            picker.FileTypeChoices.Add(new KeyValuePair<string, IList<string>>("Videos", new List<string>(){ ".mp4" }));

            return await picker.PickSaveFileAsync();
        }

        private static async Task<StorageFile> LoadData()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                CommitButtonText = "Load",
                FileTypeFilter = {".json"},
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            var file = await picker.PickSingleFileAsync();

            return file;
        }

        private static async Task<StorageFile> LoadVideo()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.List,
                CommitButtonText = "Load",
                FileTypeFilter = {".mp4"},
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            return await picker.PickSingleFileAsync();
        }

        public IObservable<StorageFile> SaveObs()
        {
            return Observable.FromAsync(SaveVideo);
        }
    }

    public interface IVideoExportService
    {
        Task<Unit> Export(ExportInput exportConfig);
    }
}