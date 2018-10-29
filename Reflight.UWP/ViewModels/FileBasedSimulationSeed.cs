using System;
using System.Threading.Tasks;
using ParrotDiscoReflight.Code.Units;
using Reflight.Core;

namespace ParrotDiscoReflight.ViewModels
{
    public class FileBasedSimulationSeed : SimulationSeed
    {
        private readonly Flight flight;

        public FileBasedSimulationSeed(Flight flight, Video video, Func<UnitPack> getUnitPack) : base(video, getUnitPack)
        {
            this.flight = flight;
        }

        public override Task<Flight> GetFlight()
        {
            return Task.FromResult(flight);
        }
    }
} 