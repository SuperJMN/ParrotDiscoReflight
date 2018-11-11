using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Graphics.Canvas;
using NodaTime;
using NodaTime.Extensions;
using ParrotDiscoReflight.ViewModels;
using Reflight.Core;

namespace ParrotDiscoReflight.Code
{
    public class ControlBasedExporter : IExporter
    {
        private readonly Control control;

        public ControlBasedExporter(Control control)
        {
            this.control = control;
        }

        public async Task Export(Video video, Flight flight, StorageFile output, Action<Status> onNewStatus)
        {
            
            var composition = new MediaComposition();
            var baseClip = await MediaClip.CreateFromFileAsync(video.Source);
            composition.Clips.Add(baseClip);

            var encProps = baseClip.GetVideoEncodingProperties();
            var layer = await CreateOverlay2(video.RecordedInterval.Value, flight, TimeSpan.FromSeconds(0.3),
                onNewStatus, new Rect(0, 0, encProps.Width, encProps.Height));

            composition.OverlayLayers.Add(layer);

            await composition.RenderToFileAsync(output, MediaTrimmingPreference.Fast);
        }

        private async Task<MediaOverlay> CreateOverlay(Interval videoInterval, Flight flight, TimeSpan period,
            Action<Status> onNewStatus)
        {
            var count = videoInterval.Duration.ToTimeSpan().Divide(period);
            var composition = new MediaComposition();

            var bmps = new List<StorageFile>();

            for (var i = 0; i < count; i++)
            {
                var offset = i * period;
                var videoTime = videoInterval.Start.Plus(offset.ToDuration());
                var flightTime = videoTime.Minus(flight.Date.ToInstant());
                var status = flight.Statuses.FirstOrDefault(x => x.TimeElapsed >= flightTime.ToTimeSpan());

                if (status != null) onNewStatus(status);

                var bmp = await CaptureBitmap();
                bmps.Add(bmp);
                composition.Clips.Add(await MediaClip.CreateFromImageFileAsync(bmp, offset));
            }

            var storageFile = await CreateFile();
            await composition.RenderToFileAsync(storageFile, MediaTrimmingPreference.Fast);

            foreach (var bmp in bmps) await bmp.DeleteAsync();

            var mediaClip = await MediaClip.CreateFromFileAsync(storageFile);
            await storageFile.DeleteAsync();

            var mediaOverlay = new MediaOverlay(mediaClip)
            {
                Position = new Rect(0, 0, 200, 200),
                Delay = TimeSpan.Zero
            };

            return mediaOverlay;
        }

        private async Task<MediaOverlayLayer> CreateOverlay2(Interval videoInterval, Flight flight, TimeSpan period,
            Action<Status> onNewStatus, Rect position)
        {
            var count = videoInterval.Duration.ToTimeSpan().Divide(period);
            var mediaLayer = new MediaOverlayLayer();

            for (var i = 0; i < count; i++)
            {
                var offset = i * period;
                var videoTime = videoInterval.Start.Plus(offset.ToDuration());
                var flightTime = videoTime.Minus(flight.Date.ToInstant());
                var status = flight.Statuses.FirstOrDefault(x => x.TimeElapsed >= flightTime.ToTimeSpan());

                if (status != null) onNewStatus(status);

                var clip = await CreateMediaClip(period);
                var mediaOverlay = new MediaOverlay(clip)
                {
                    Position = position,
                    Delay = offset,
                };

                mediaLayer.Overlays.Add(mediaOverlay);
            }

            return mediaLayer;
        }

        private async Task<MediaClip> CreateMediaClip(TimeSpan originalDuration)
        {
            var rendertargetBitmap = new RenderTargetBitmap();
            await rendertargetBitmap.RenderAsync(control);
            var bfr = await rendertargetBitmap.GetPixelsAsync();
            CanvasRenderTarget rendertarget;
            
            using (var canvas = CanvasBitmap.CreateFromBytes(CanvasDevice.GetSharedDevice(), bfr,
                rendertargetBitmap.PixelWidth, rendertargetBitmap.PixelHeight,
                DirectXPixelFormat.B8G8R8A8UIntNormalized))
            {
                rendertarget = new CanvasRenderTarget(CanvasDevice.GetSharedDevice(), canvas.SizeInPixels.Width,
                    canvas.SizeInPixels.Height, 96);
                using (var ds = rendertarget.CreateDrawingSession())
                {
                    ds.DrawImage(canvas);
                }
            }

            return MediaClip.CreateFromSurface(rendertarget, originalDuration);
        }

        private Task<StorageFile> CreateFile()
        {
            return ApplicationData.Current.LocalFolder.CreateFileAsync(Path.GetRandomFileName()).AsTask();
        }

        private async Task<StorageFile> CaptureBitmap()
        {
            var bmp = new RenderTargetBitmap();
            await bmp.RenderAsync(control);
            var pixel = await bmp.GetPixelsAsync();

            var file = await CreateFile();

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var logicalDpi = DisplayInformation.GetForCurrentView().LogicalDpi;
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);

                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Straight,
                    (uint) bmp.PixelWidth,
                    (uint) bmp.PixelHeight,
                    logicalDpi,
                    logicalDpi,
                    pixel.ToArray());

                await encoder.FlushAsync();
                stream.Dispose();
            }

            return file;
        }
    }

    public interface IExporter
    {
        Task Export(Video video, Flight flight, StorageFile output, Action<Status> onNewStatus);
    }
}