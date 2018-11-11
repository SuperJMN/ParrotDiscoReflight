using System;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Code.Units;
using ParrotDiscoReflight.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ParrotDiscoReflight.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SuperExportPage : Page
    {
        public SuperExportPage()
        {
            this.InitializeComponent();            
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            

            var exportService = new ControlBasedExporter(RenderControl);
            var videoFile = await StorageFile.GetFileFromPathAsync("E:\\Local\\Parrot\\Disco_20181028174515+0100.mp4");
                
            var video = await Video.Load(videoFile);

            RenderControl.Width = 1920;
            RenderControl.Height = 1080;

            var dataFile = await StorageFile.GetFileFromPathAsync("C:\\Users\\super\\Desktop\\Lluvia.json");
            var flight = await dataFile.ReadFlight();
            var folderFromPathAsync = await StorageFolder.GetFolderFromPathAsync("C:\\Users\\super\\Desktop\\");
            var output = await folderFromPathAsync.CreateFileAsync("output.mp4", CreationCollisionOption.ReplaceExisting);
            await exportService.Export(video, flight, output, status =>
            {
                RenderControl.DataContext = new SimulationSimulationViewModel()
                {
                    Status = new StatusViewModel(status),
                    Units = UnitSource.UnitPacks.FirstOrDefault(),
                };
            });

            base.OnNavigatedTo(e);
        }
    }
}
