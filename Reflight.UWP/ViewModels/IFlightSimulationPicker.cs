using System;
using ParrotDiscoReflight.Code;

namespace ParrotDiscoReflight.ViewModels
{
    public interface IFlightSimulationPicker
    {
        IObservable<SimulationUnit> Simulations { get; }
        string Name { get; }
    }
}