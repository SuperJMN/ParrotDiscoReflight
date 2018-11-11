using System;
using System.Threading.Tasks;
using Reflight.Core;
using Reflight.Core.Reader;

namespace ParrotDiscoReflight.ViewModels
{
    internal class SummaryBasedSimulationSeed : SimulationSeed
    {
        private readonly Func<Task<FlightDetails>> factory;

        public SummaryBasedSimulationSeed(Func<Task<FlightDetails>> factory, Video video,
            Func<PresentationOptions> getPresentationOptions) : base(video, getPresentationOptions)
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