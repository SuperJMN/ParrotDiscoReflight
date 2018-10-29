using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using ParrotDiscoReflight.ViewModels;
using ReactiveUI;
using Reflight.Core;
using Reflight.Core.FlightAcademy;
using Reflight.Core.Reader;

namespace ParrotDiscoReflight.Code
{
    public class FileOpenCommands
    {
        public FileOpenCommands()
        {
            OpenDataCommand =
                ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(LoadData).Where(x => x != null));

            OpenVideoCommand =
                ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(LoadVideo).Where(x => x != null)
                    .SelectMany(Video.Load));

            SaveFileCommand =
                ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(SaveVideo).Where(x => x != null));

            BrowseFolderCommand =
                ReactiveCommand.CreateFromObservable(() => Observable.FromAsync(BrowseFolder).Where(x => x != null));
        }

        public ReactiveCommand<Unit, StorageFolder> BrowseFolderCommand { get; }

        public ReactiveCommand<Unit, StorageFile> SaveFileCommand { get; }

        public ReactiveCommand<Unit, Video> OpenVideoCommand { get; }

        public ReactiveCommand<Unit, StorageFile> OpenDataCommand { get; }

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

        private async Task<StorageFolder> BrowseFolder()
        {
            var picker = new FolderPicker
            {
                CommitButtonText = "Select",
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add("*"); 
            return await picker.PickSingleFolderAsync();
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
}