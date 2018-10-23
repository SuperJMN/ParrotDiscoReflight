using ParrotDiscoReflight.Code;

namespace ParrotDiscoReflight.ViewModels
{
    public class SimulationUnitViewModel
    {
        public SimulationUnit SimulationUnit { get; }
        public byte[] Thumbnail { get; }

        public SimulationUnitViewModel(SimulationUnit simulationUnit, byte[] thumbnail)
        {
            SimulationUnit = simulationUnit;
            Thumbnail = thumbnail;
        }


    }
}