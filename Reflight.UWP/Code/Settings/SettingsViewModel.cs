using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Windows.Storage;
using Windows.Storage.AccessCache;
using ParrotDiscoReflight.Code.Units;
using ReactiveUI;

namespace ParrotDiscoReflight.Code.Settings
{
    public class SettingsViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<bool> isVideoFolderFolderConfiguredOh;
        private readonly ObservableSettings settings;
        private readonly ObservableAsPropertyHelper<StorageFolder> videoFolderOh;

        private UnitPack unitPack;
        private readonly ObservableAsPropertyHelper<bool> isAccountConfiguredOh;

        public SettingsViewModel(FileOpenCommands commands)
        {
            settings = new ObservableSettings(this, ApplicationData.Current.RoamingSettings);
            BrowseFolderCommand = commands.BrowseFolderCommand;
            BrowseFolderCommand.Subscribe(x =>
            {
                if (VideoFolderToken != null)
                {
                    StorageApplicationPermissions.FutureAccessList.Remove(VideoFolderToken);
                }

                var token = StorageApplicationPermissions.FutureAccessList.Add(x);
                VideoFolder = x.Path;
                VideoFolderToken = token;
            });

            videoFolderOh = BrowseFolderCommand.ToProperty(this, x => x.Folder);
            isAccountConfiguredOh = this.WhenAnyValue(x => x.Username, x => x.Password,
                (u, p) => new[] {u, p}.All(s => !string.IsNullOrEmpty(s))).ToProperty(this, x => x.IsAccountConfigured);

            isVideoFolderFolderConfiguredOh = this.WhenAnyValue(x => x.VideoFolder, x => !string.IsNullOrEmpty(x))
                .ToProperty(this, x => x.IsVideoFolderConfigured);

            UnitPack = UnitPacks.FirstOrDefault(pack => pack.Id == StringUnitPack) ?? UnitPacks.First();
            this.WhenAnyValue(x => x.UnitPack).Subscribe(x => StringUnitPack = UnitPack.Id);

            RemoveFolderCommand = ReactiveCommand.Create(() =>
            {
                StorageApplicationPermissions.FutureAccessList.Remove(VideoFolderToken);
                VideoFolder = null;
                VideoFolderToken = null;
            }, this.WhenAnyValue(x => x.VideoFolderToken, selector: s => s != null));
        }

        public ReactiveCommand<Unit, Unit> RemoveFolderCommand { get; }

        public bool IsVideoFolderConfigured => isVideoFolderFolderConfiguredOh.Value;

        public string StringUnitPack
        {
            get => settings.Get<string>();
            set => settings.Set(value);
        }

        public StorageFolder Folder => videoFolderOh.Value;

        public ReactiveCommand<Unit, StorageFolder> BrowseFolderCommand { get; }

        public string VideoFolder
        {
            get => settings.Get<string>();
            set
            {
                settings.Set(value);
                this.RaisePropertyChanged();
            }
        }

        public string VideoFolderToken
        {
            get => settings.Get<string>();
            set
            {
                settings.Set(value);
                this.RaisePropertyChanged();
            }
        }

        public ICollection<UnitPack> UnitPacks => UnitSource.UnitPacks;

        public string Username
        {
            get => settings.Get<string>();
            set
            {
                settings.Set(value);
                this.RaisePropertyChanged();
            }
        }

        public string Password
        {
            get => settings.Get<string>();
            set
            {
                settings.Set(value); 
                this.RaisePropertyChanged();
            }
        }

        public UnitPack UnitPack
        {
            get => unitPack;
            set => this.RaiseAndSetIfChanged(ref unitPack, value);
        }

        public bool IsAccountConfigured => isAccountConfiguredOh.Value;
    }
}