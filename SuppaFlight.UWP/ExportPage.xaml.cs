using System;
using System.Reactive.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ReactiveUI;
using SuppaFlight.UWP.Code;

namespace SuppaFlight.UWP
{
    public sealed partial class ExportPage
    {
        public ExportPage()
        {
            this.InitializeComponent();

            Loaded += OnLoaded;

            MessageBus.Current.Listen<SizeMessage>()
                .Subscribe(s =>
                {
                    Width = s.Width;
                    Height = s.Height;
                });
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            MessageBus.Current.SendMessage<Control>(RenderControl);
            MessageBus.Current.SendMessage(new LoadedMessage());            
        }
    }
}
