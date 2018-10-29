using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using Reflight.Core.FlightAcademy;

namespace ParrotDiscoReflight.ViewModels
{
    public class VideoPackViewModel : ReactiveObject
    {
        private VideoPackItemViewModel selectedItem;
        public IEnumerable<VideoPackItemViewModel> Items { get; set; }

        public VideoPackItemViewModel SelectedItem
        {
            get => selectedItem;
            set => this.RaiseAndSetIfChanged(ref selectedItem, value);
        }

        public FlightSummary FlightSummary { get; set; }

        public IEnumerable<VideoPackItemViewModel> ReversedItems => Items.Reverse();
    }
}