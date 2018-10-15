using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using FlightVisualizer.Core;
using Microsoft.Graphics.Canvas;
using ReactiveUI;
using SuppaFlight.UWP.Code;

namespace SuppaFlight.UWP
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
            var frames = exportConfig.FlightData.Statuses.ToObservable()
                .Buffer(2, 1)
                .Select(list =>
                {
                    var duration = list.Count == 2 ? list[1].TimeElapsed - list[0].TimeElapsed : TimeSpan.FromMilliseconds(200);
                    return new Frame(duration, list[0]);
                }).ToEnumerable();

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
            foreach (var frame in frames.Take(500))
            {
                control.DataContext = convertFunc(frame.Status);
                RenderTargetBitmap rendertargetBitmap = new RenderTargetBitmap();
                await rendertargetBitmap.RenderAsync(control, 400, 0);
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
                        ds.Clear(Colors.Black);
                        ds.DrawImage(canvas);
                    }
                }

                var clip = MediaClip.CreateFromSurface(rendertarget, frame.Duration);
                AddOverlayClip(composition, clip, 0, 0, control.ActualWidth, control.ActualHeight);               
            }
        }

        private void AddOverlayClip(MediaComposition composition, MediaClip overlayMediaClip, double left, double top, double width, double height)
        {
            var overlayPosition = new Rect(left, top, width, height);

            var mediaOverlay = new MediaOverlay(overlayMediaClip)
            {
                Position = overlayPosition,
            };

            var mediaOverlayLayer = new MediaOverlayLayer();
            mediaOverlayLayer.Overlays.Add(mediaOverlay);

            composition.OverlayLayers.Add(mediaOverlayLayer);
        }

        private class Frame
        {
            public TimeSpan Duration { get; }
            public Status Status { get; }

            public Frame(TimeSpan duration, Status status)
            {
                Duration = duration;
                Status = status;
            }
        }
    }
}