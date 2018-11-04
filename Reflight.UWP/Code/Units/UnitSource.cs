using System.Collections.Generic;

namespace ParrotDiscoReflight.Code.Units
{
    public static class UnitSource
    {
        static UnitSource()
        {
            UnitPacks = new List<UnitPack>
            {
                new UnitPack
                {
                    Id = "metrickms",
                    Name = "Metric (speed in Km/h)",
                    Speed = new KilometersPerHourUnit(),
                    Longitude = new KilometersUnit(),
                    Altitude = new MetersUnit(),
                },
                new UnitPack
                {
                    Id = "metricms",
                    Name = "Metric (speed in m/s)",
                    Speed = new MetersPerSecondUnit(),
                    Longitude = new MetersUnit(),
                    Altitude = new MetersUnit(),
                },
                new UnitPack
                {
                    Id = "imperial",
                    Name = "Imperial",
                    Speed = new MilesPerHourUnit(),
                    Longitude = new MilesUnit(),
                    Altitude = new FeetUnit()
                }
            };
        }

        public static ICollection<UnitPack> UnitPacks { get; }
    }
}