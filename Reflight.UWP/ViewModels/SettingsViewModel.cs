using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Windows.Storage;
using Windows.Storage.AccessCache;
using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Code.Settings;
using ParrotDiscoReflight.Code.Units;
using ReactiveUI;

namespace ParrotDiscoReflight.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        private readonly IVirtualDashboardsRepository virtualDashboardsRepository;
        private readonly SettingsSaver settings;
        private readonly ObservableAsPropertyHelper<StorageFolder> videoFolder;
        private UnitPack unitPack;
        private readonly ObservableAsPropertyHelper<string> username;
        private readonly ObservableAsPropertyHelper<string> password;

        public SettingsViewModel(FileOpenCommands commands, IDialogService dialogService, IVirtualDashboardsRepository virtualDashboardsRepository)
        {
            this.virtualDashboardsRepository = virtualDashboardsRepository;
            settings = new SettingsSaver(this, ApplicationData.Current.RoamingSettings);
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

            videoFolder = BrowseFolderCommand.ToProperty(this, x => x.Folder);
            IsAccountConfigured = this.WhenAnyValue(x => x.Username, x => x.Password,
                (u, p) => new[] {u, p}.All(s => !string.IsNullOrEmpty(s)));

            IsVideoFolderFolderConfigured = this.WhenAnyValue(x => x.VideoFolder, x => !string.IsNullOrEmpty(x));

            UnitPack = UnitPacks.FirstOrDefault(pack => pack.Id == StringUnitPack) ?? UnitPacks.First();
            this.WhenAnyValue(x => x.UnitPack).Subscribe(x => StringUnitPack = UnitPack.Id);

            RemoveFolderCommand = ReactiveCommand.Create(() =>
            {
                StorageApplicationPermissions.FutureAccessList.Remove(VideoFolderToken);
                VideoFolder = null;
                VideoFolderToken = null;
            }, this.WhenAnyValue(x => x.VideoFolderToken, selector: s => s != null));

            UserBasedLogin = new UserBasedLogin();
            EmailBasedLogin = new EmailBasedLogin(dialogService);

            var usernamesSequence = GetUsernamesSequence();
            var passwordSequence = GetPasswordsSequence();


            username = usernamesSequence.ToProperty(this, x => x.Username);
            password = passwordSequence.ToProperty(this, x => x.Password);
        }

        private IObservable<string> GetUsernamesSequence()
        {
            var isUserBased = this.WhenAnyValue(x => x.IsUserLogon);
            var userBasedLogon = UserBasedLogin.WhenAnyValue(x => x.Username);
            var emailBasedLogon = EmailBasedLogin.WhenAnyValue(x => x.VerifiedUsername);
            var usernames = userBasedLogon.CombineLatest(emailBasedLogon, (u, m) => new {Username = u, Email = m});
            var usernamesSequence = isUserBased.CombineLatest(usernames, (userBased, user) => userBased ? user.Username : user.Email);
            return usernamesSequence;
        }

        private IObservable<string> GetPasswordsSequence()
        {
            var isUserBased = this.WhenAnyValue(x => x.IsUserLogon);
            var userBasedLogon = UserBasedLogin.WhenAnyValue(x => x.Password);
            var emailBasedLogon = EmailBasedLogin.WhenAnyValue(x => x.VerifiedPassword);
            var usernames = userBasedLogon.CombineLatest(emailBasedLogon, (u, w) => new {Username = u, Email = w});
            var sequence = isUserBased.CombineLatest(usernames, (userBased, user) => userBased ? user.Username : user.Email);
            return sequence;
        }

        public IObservable<bool> IsAccountConfigured { get; }

        public IObservable<bool> IsVideoFolderFolderConfigured { get; }

        public ReactiveCommand<Unit, Unit> RemoveFolderCommand { get; }

        public string StringUnitPack
        {
            get => settings.Get<string>();
            set => settings.Set(value);
        }

        public StorageFolder Folder => videoFolder.Value;

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

        public string Username => username.Value;

        public string Password => password.Value;

        public UnitPack UnitPack
        {
            get => unitPack;
            set => this.RaiseAndSetIfChanged(ref unitPack, value);
        }

        public VirtualDashboard VirtualDashboard
        {
            get
            {
                var name = settings.Get<string>();
                return VirtualDashboards.FirstOrDefault(x => x.Name == name) ?? VirtualDashboards.First();
            }
            set
            {
                settings.Set(value.Name);
                this.RaisePropertyChanged();
            }
        }

        [DefaultSettingValue(true)]
        public bool IsUserLogon
        {
            get => settings.Get<bool>();
            set
            {
                settings.Set(value);
                this.RaisePropertyChanged();
            }
        }

        public UserBasedLogin UserBasedLogin { get; }
        public EmailBasedLogin EmailBasedLogin { get; }

        public ICollection<VirtualDashboard> VirtualDashboards => virtualDashboardsRepository.GetAll();
    }
}