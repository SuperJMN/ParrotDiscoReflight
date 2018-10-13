using System.Collections.Generic;

namespace SuppaFlight.UWP.Code.Units
{
    public static class UnitSource
    {
        static UnitSource()
        {
            UnitPacks = new List<UnitPack>
            {
                new UnitPack
                {
                    Name = "Metric (speed in Km/h)",
                    Speed = new KilometersPerHourUnit(),
                    Longitude = new MetersUnit()                    
                },
                new UnitPack
                {
                    Name = "Metric (speed in m/s)",
                    Speed = new MetersPerSecondUnit(),
                    Longitude = new MetersUnit()
                },
                new UnitPack
                {
                    Name = "Imperial",
                    Speed = new MilesPerHourUnit(),
                    Longitude = new FeetUnit()
                }
            };
        }

        public static ICollection<UnitPack> UnitPacks { get; }
    }
}