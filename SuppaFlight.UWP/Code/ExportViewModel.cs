using System;
using Windows.Media.Editing;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace SuppaFlight.UWP.Code
{
    public class ExportViewModel : ReactiveObject
    {
        private ObservableAsPropertyHelper<bool> isBusyOh;
        public IVideoExportService ExportService { get; }

        public ExportViewModel(IVideoExportService exportService, ExportInput exportInput)
        {
            ExportService = exportService;
            ExportCommand = ReactiveCommand.CreateFromTask(() => exportService.Export(exportInput));

            MessageBus.Current.Listen<LoadedMessage>()
                .SelectMany(async _ =>
                {
                    var c = await MediaClip.CreateFromFileAsync(exportInput.InputFile);
                    var p = c.GetVideoEncodingProperties();
                    var s = new SizeMessage(p.Width, p.Height);
                    return s;
                })
                .ObserveOnDispatcher()
                .Do(message => MessageBus.Current.SendMessage(message))
                .Select(_ => Unit.Default)
                .InvokeCommand(this, x => x.ExportCommand);

            isBusyOh = ExportCommand.IsExecuting.ToProperty(this, x => x.IsBusy);
        }

        public bool IsBusy => isBusyOh.Value;

        public ReactiveCommand<Unit, Unit> ExportCommand { get; }
    }
}