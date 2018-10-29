using System;
using System.Threading.Tasks;
using ParrotDiscoReflight.Code.Units;
using Reflight.Core;
using Reflight.Core.Reader;

namespace ParrotDiscoReflight.ViewModels
{
    class SummaryBasedSimulationSeed : SimulationSeed
    {
        private readonly Func<Task<FlightDetails>> factory;

        public SummaryBasedSimulationSeed(Func<Task<FlightDetails>> factory, Video video, Func<UnitPack> getUnitPack) : base(video, getUnitPack)
        {
            this.factory = factory;
        }
        public override async Task<Flight> GetFlight()
        {
            var flightSummary = await factory();
            return flightSummary.ToFlight();
        }
    }
}