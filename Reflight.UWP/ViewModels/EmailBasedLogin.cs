using System;
using System.Reactive;
using System.Threading.Tasks;
using Windows.Storage;
using ParrotDiscoReflight.Code;
using ReactiveUI;

namespace ParrotDiscoReflight.ViewModels
{
    public class EmailBasedLogin : ReactiveObject
    {
        private readonly CredentialRetriever retriever;
        private readonly ObservableAsPropertyHelper<bool> isLogged;
        private readonly SettingsSaver settings;

        public EmailBasedLogin(IDialogService dialogService)
        {
            settings = new SettingsSaver(this, ApplicationData.Current.RoamingSettings);

            LoginCommand = ReactiveCommand.CreateFromTask(() => RetrieveCredentials(Email, Password));
            LoginCommand.ThrownExceptions.Subscribe(x =>
            {
                VerifiedUsername = null;
                VerifiedPassword = null;
            });

            LoginCommand.ThrownExceptions.MessageOnException(dialogService);

            LoginCommand.Subscribe(x =>
            {
                VerifiedUsername = x.Username;
                VerifiedPassword = x.Password;                
            });

            retriever = new CredentialRetriever(new Uri("https://accounts.parrot.com/V3/logform"));

            isLogged = this.WhenAnyValue(x => x.VerifiedUsername, x => x.VerifiedPassword,
                selector: (a, b) => a != null || b != null).ToProperty(this, x => x.IsLogged);
        }

        public bool IsLogged => isLogged.Value;

        public ReactiveCommand<Unit, Credentials> LoginCommand { get; }

        private Task<Credentials> RetrieveCredentials(string email, string password)
        {
            return retriever.RetrieveCredentials(email, password);
        }
        
        public string VerifiedUsername
        {
            get => settings.Get<string>();
            set
            {
                settings.Set(value);
                this.RaisePropertyChanged();            }
        }

        public string VerifiedPassword
        {
            get => settings.Get<string>();
            set
            {
                settings.Set(value);
                this.RaisePropertyChanged();            }
        }

        public string Email
        {
            get => settings.Get<string>();
            set
            {
                settings.Set(value);
                this.RaisePropertyChanged();            }
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
    }
}