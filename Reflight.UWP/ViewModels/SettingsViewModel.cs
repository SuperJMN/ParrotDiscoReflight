using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Windows.Storage;
using Windows.Storage.AccessCache;
using GeoCoordinatePortable;
using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Code.Settings;
using ParrotDiscoReflight.Code.Units;
using ParrotDiscoReflight.Controls;
using ReactiveUI;
using Reflight.Core;

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
        private readonly ObservableAsPropertyHelper<ISimulationViewModel> dashboardPreview;

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
                (u, p) => new[] { u, p }.All(s => !string.IsNullOrEmpty(s)));

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

            dashboardPreview =
                this.WhenAnyValue(x => x.VirtualDashboard, x => x.UnitPack,
                    (vd, up) => CreatePreviewViewModel(up, vd)).ToProperty(this, x => x.DashboardPreview);
        }

        private static DefaultSimulationViewModel CreatePreviewViewModel(UnitPack up, VirtualDashboard vd)
        {
            var presentationOptions = new PresentationOptions { UnitPack = up, Dashboard = vd, };
            return new DefaultSimulationViewModel(presentationOptions)
            {
                Altitude = new SamplePlottableViewModel(new List<double> { 0, 1, 2, 5, 8, 9, 19, 19, 29, 19, 12, 13, 23, 22, 21, 20, 19, 20, 21 }, new Point(2, 2)),
                Speed = new SamplePlottableViewModel(new List<double> { 0, 11, 12, 15, 18, 20, 22, 22, 22.3, 21, 2, 17.5, 5, 15.5, 18, 19, 19, 17, 18.75, 17, 16 }, new Point(5, 16)),
                Status = new StatusViewModel(new Status
                {
                    TimeElapsed = new TimeSpan(0, 0, 11, 28),
                    Speed = new Vector(1, 2, 4),
                    AnglePhi = 0.3,
                    AnglePsi = 0.2,
                    AngleTheta = 0.4,
                    BatteryLevel = 0.75,
                    PitotSpeed = 9,
                    WifiStregth = -30,
                    TotalDistance = 1234,
                    ControllerPosition = new GeoCoordinate(0, 0, 0),
                    DronePosition = new GeoCoordinate(0, 0, 123),
                })
            };
        }

        public ISimulationViewModel DashboardPreview => dashboardPreview.Value;

        private IObservable<string> GetUsernamesSequence()
        {
            var isUserBased = this.WhenAnyValue(x => x.IsUserLogon);
            var userBasedLogon = UserBasedLogin.WhenAnyValue(x => x.Username);
            var emailBasedLogon = EmailBasedLogin.WhenAnyValue(x => x.VerifiedUsername);
            var usernames = userBasedLogon.CombineLatest(emailBasedLogon, (u, m) => new { Username = u, Email = m });
            var usernamesSequence = isUserBased.CombineLatest(usernames, (userBased, user) => userBased ? user.Username : user.Email);
            return usernamesSequence;
        }

        private IObservable<string> GetPasswordsSequence()
        {
            var isUserBased = this.WhenAnyValue(x => x.IsUserLogon);
            var userBasedLogon = UserBasedLogin.WhenAnyValue(x => x.Password);
            var emailBasedLogon = EmailBasedLogin.WhenAnyValue(x => x.VerifiedPassword);
            var usernames = userBasedLogon.CombineLatest(emailBasedLogon, (u, w) => new { Username = u, Email = w });
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