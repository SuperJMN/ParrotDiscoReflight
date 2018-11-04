using Windows.Storage;
using ReactiveUI;

namespace ParrotDiscoReflight.ViewModels
{
    public class UserBasedLogin : ReactiveObject
    {

        private string username;
        private string password;
        private readonly SettingsSaver settings;

        public UserBasedLogin()
        {
            settings = new SettingsSaver(this, ApplicationData.Current.RoamingSettings);
        }

        public string Username
        {
            get => settings.Get<string>();
            set
            {
                settings.Set(value);
                this.RaiseAndSetIfChanged(ref username, value);
            }
        }

        public string Password
        {
            get => settings.Get<string>();
            set
            {
                settings.Set(value);
                this.RaiseAndSetIfChanged(ref password, value);
            }
        }
    }
}