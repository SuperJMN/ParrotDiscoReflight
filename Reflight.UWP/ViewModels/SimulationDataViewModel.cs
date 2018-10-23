using ParrotDiscoReflight.Code;
using ParrotDiscoReflight.Code.Units;
using ReactiveUI;

namespace ParrotDiscoReflight.ViewModels
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