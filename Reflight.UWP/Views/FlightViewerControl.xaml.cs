using System;
using System.Reactive;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using ReactiveUI;

namespace ParrotDiscoReflight.Views
{
    public sealed partial class FlightViewerControl
    {
        public FlightViewerControl()
        {
            InitializeComponent();

            MessageBus.Current.Listen<Unit>("Play").Subscribe(x =>
            {
                MediaElement.Play();
            });

            MessageBus.Current.Listen<StorageFile>().Subscribe(file =>
            {
                var mediaSource = MediaSource.CreateFromStorageFile(file);
                MediaElement.SetPlaybackSource(mediaSource);
            });
        }


        public static readonly DependencyProperty VideoProperty = DependencyProperty.Register(
            "Video", typeof(StorageFile), typeof(FlightViewerControl), new PropertyMetadata(default(StorageFile), VideoChanged));

        private static void VideoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flightViewer = (FlightViewerControl)d;
            var file = (IStorageFile)e.NewValue;
            var mediaPlaybackSource = MediaSource.CreateFromStorageFile(file);
            
            flightViewer.MediaElement.SetPlaybackSource(mediaPlaybackSource);
        }

        public StorageFile Video
        {
            get { return (StorageFile)GetValue(VideoProperty); }
            set { SetValue(VideoProperty, value); }
        }
    }
}
