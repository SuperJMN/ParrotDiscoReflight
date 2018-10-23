using Windows.Storage;
using Reflight.Core;

namespace ParrotDiscoReflight.Code
{
    public class SimulationUnit
    {
        public Flight Flight { get; }
        public StorageFile Video { get; }

        public SimulationUnit(Flight flight, StorageFile video)
        {
            Flight = flight;
            Video = video;
        }
    }
}