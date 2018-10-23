using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Graphics.Canvas;
using ReactiveUI;
using Reflight.Core;

namespace ParrotDiscoReflight.Code
{
    public class ExportService : IVideoExportService
    {
        private readonly Func<Status, IDataViewModel> convertFunc;
        private Control control;

        public ExportService(Func<Status, IDataViewModel> convertFunc)
        {
            this.convertFunc = convertFunc;
            MessageBus.Current.Listen<Control>()
                .Subscribe(x => control = x);
        }

        public async Task<Unit> Export(ExportInput exportConfig)
        {
            var frames = exportConfig.Flight.Statuses.ToObservable()
                .Buffer(2, 1)
                .SkipLast(1)
                .Select(list => new Frame(list[0].TimeElapsed, list[1].TimeElapsed - list[0].TimeElapsed, list[0]))
                .Where(x => x.Position >= TimeSpan.Zero)
                .ToEnumerable();

            var composition = await CreateComposition(frames, exportConfig.InputFile);
            await composition.RenderToFileAsync(exportConfig.OutputFile, MediaTrimmingPreference.Precise);
            return Unit.Default;
        }

        private async Task<MediaComposition> CreateComposition(IEnumerable<Frame> frames, IStorageFile inputFile)
        {
            var composition = new MediaComposition();
            await AddOverlay(frames, composition);
            var originalVideo = await MediaClip.CreateFromFileAsync(inputFile);
            composition.Clips.Add(originalVideo);

            return composition;
        }

        private async Task AddOverlay(IEnumerable<Frame> frames, MediaComposition composition)
        {
            foreach (var frame in frames.Take(600))
            {
                control.DataContext = convertFunc(frame.Status);
                var rendertargetBitmap = new RenderTargetBitmap();
                await rendertargetBitmap.RenderAsync(control);
                var bfr = await rendertargetBitmap.GetPixelsAsync();

                CanvasRenderTarget rendertarget;
                using (var canvas = CanvasBitmap.CreateFromBytes(CanvasDevice.GetSharedDevice(), bfr,
                    rendertargetBitmap.PixelWidth, rendertargetBitmap.PixelHeight,
                    Windows.Graphics.DirectX.DirectXPixelFormat.B8G8R8A8UIntNormalized))
                {
                    rendertarget = new CanvasRenderTarget(CanvasDevice.GetSharedDevice(), canvas.SizeInPixels.Width,
                        canvas.SizeInPixels.Height, 96);
                    using (var ds = rendertarget.CreateDrawingSession())
                    {
                        ds.DrawImage(canvas);
                    }
                }

                var clip = MediaClip.CreateFromSurface(rendertarget, frame.Duration);
                AddOverlayClip(composition, clip, 0, 0, control.ActualWidth, control.ActualHeight, frame.Position);               
            }
        }

        private void AddOverlayClip(MediaComposition composition, MediaClip overlayMediaClip, double left, double top, double width, double height, TimeSpan delay)
        {
            var overlayPosition = new Rect(left, top, width, height);

            var mediaOverlay = new MediaOverlay(overlayMediaClip)
            {
                Position = overlayPosition,
                Delay = delay,
            };

            var mediaOverlayLayer = new MediaOverlayLayer();
            mediaOverlayLayer.Overlays.Add(mediaOverlay);

            composition.OverlayLayers.Add(mediaOverlayLayer);
        }

        private class Frame
        {
            public TimeSpan Position { get; }
            public TimeSpan Duration { get; }
            public Status Status { get; }

            public Frame(TimeSpan position, TimeSpan duration, Status status)
            {
                Position = position;
                Duration = duration;
                Status = status;
            }
        }
    }
}