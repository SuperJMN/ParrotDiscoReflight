using System;
using System.Collections.Generic;
using System.Linq;
using NodaTime;
using NodaTime.Extensions;
using Reflight.Core;

namespace ParrotDiscoReflight.ViewModels
{
    public class Simulation
    {
        public Simulation(Video video, Flight flight, PresentationOptions unitPack)
        {
            Video = video;
            Flight = flight;
            PresentationOptions = unitPack;
        }

        public Duration Duration => Video.RecordedInterval.Value.Duration;

        public Video Video { get; }
        public Flight Flight { get; }
        public PresentationOptions PresentationOptions { get; }
        public TimeSpan Offset => Video.RecordedInterval.Value.Start.ToDateTimeOffset().Subtract(Flight.Date);
        public IEnumerable<Status> Statuses
        {
            get
            {
                var enumerable = Flight.Statuses.Where(s => IsInRange(s.TimeElapsed)).ToList();
                return enumerable;
            }
        }

        private bool IsInRange(TimeSpan zeroBasedTimeSpan)
        {
            var offsetRelativeToVideo = zeroBasedTimeSpan.ToOffset();
            var flightInstant = Flight.Date.ToInstant();

            var finalInstant = flightInstant.Plus(offsetRelativeToVideo.ToTimeSpan().ToDuration());
            
            return Video.RecordedInterval.Value.Contains(finalInstant);

        }
    }
}