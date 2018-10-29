using System;
using Windows.Storage;
using Reflight.Core;

namespace ParrotDiscoReflight.Code
{
    public class SimulationUnit
    {
        public Flight Flight { get; }
        public StorageFile Video { get; }
        public TimeSpan Offset { get; }

        public SimulationUnit(Flight flight, StorageFile video, TimeSpan offset)
        {
            Flight = flight;
            Video = video;
            Offset = offset;
        }
    }
}