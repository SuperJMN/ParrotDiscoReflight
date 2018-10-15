using ReactiveUI;
using SuppaFlight.UWP.Code.Units;

namespace SuppaFlight.UWP.Code
{
    public class SimulationDataViewModel : ReactiveObject, IDataViewModel
    {
        private StatusViewModel status;

        public StatusViewModel Status
        {
            get => status;
            set => this.RaiseAndSetIfChanged(ref status, value);
        }

        public UnitPack UnitPack { get; set; }
    }
}