namespace SuppaFlight.UWP
{
    public class UnitPack
    {
        public string Name { get; set; }
        public IMeasurementUnit Speed { get; set; }
        public IMeasurementUnit Longitude { get; set; }
    }

    public interface IMeasurementUnit
    {
        string Name { get; }
        string Abbreviation { get; }
        double Convert(double value);
        double Maximum { get; }
        double Tick { get; }
        string StringFormat { get; }
    }

    public class KilometersPerHourUnit : IMeasurementUnit
    {
        public string Name => "Kilometers/Hour";
        public string Abbreviation => "Km/h";

        public double Convert(double value)
        {
            return value * 3.6;
        }

        public double Maximum => 100;
        public double Tick => 10;
        public string StringFormat => $"{{0:F}} {Abbreviation}";
    }

    public class MetersPerSecondUnit : IMeasurementUnit
    {
        public string Name => "Meters/Second";
        public string Abbreviation => "m/s";

        public double Convert(double value)
        {
            return value;
        }

        public double Maximum => 35;
        public double Tick => 1;
        public string StringFormat => $"{{0:F}} {Abbreviation}";
    }

    public class MilesPerHourUnit : IMeasurementUnit
    {
        public string Name => "Miles/Hour";
        public string Abbreviation => "mph";

        public double Convert(double value)
        {
            return value * 0.44704;
        }

        public double Maximum => 50;
        public double Tick => 5;
        public string StringFormat => $"{{0:F}} {Abbreviation}";
    }

    public class FeetUnit : IMeasurementUnit
    {
        public string Name => "Feet";
        public string Abbreviation => "ft";

        public double Convert(double value)
        {
            return value * 3.2808399;
        }

        public double Maximum => 500;
        public double Tick => 10;
        public string StringFormat => $"{{0:F}} {Abbreviation}";
    }

    public class MetersUnit : IMeasurementUnit
    {
        public string Name => "Meters";
        public string Abbreviation => "m";

        public double Convert(double value)
        {
            return value;
        }

        public double Maximum => 200;
        public double Tick => 10;
        public string StringFormat => $"{{0:F}} {Abbreviation}";
    }
}