using System;
using System.Threading.Tasks;
using ParrotDiscoReflight.Code.Units;
using Reflight.Core;

namespace ParrotDiscoReflight.ViewModels
{
    public abstract class SimulationSeed
    {
        protected SimulationSeed(Video video, Func<UnitPack> getUnitPack)
        {
            Video = video;
            GetUnitPack = getUnitPack;
        }

        public Video Video { get;  }
        public abstract Task<Flight> GetFlight();
        public Func<UnitPack> GetUnitPack {get; }
    }
}