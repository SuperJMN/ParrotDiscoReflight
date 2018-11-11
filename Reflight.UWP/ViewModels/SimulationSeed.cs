using System;
using System.Threading.Tasks;
using ParrotDiscoReflight.Code.Units;
using Reflight.Core;

namespace ParrotDiscoReflight.ViewModels
{
    public abstract class SimulationSeed
    {
        protected SimulationSeed(Video video, Func<PresentationOptions> getPresentationOptions)
        {
            Video = video;
            GetPresentationOptions = getPresentationOptions;
        }

        public Video Video { get;  }
        public abstract Task<Flight> GetFlight();
        public Func<PresentationOptions> GetPresentationOptions {get; }
    }
}