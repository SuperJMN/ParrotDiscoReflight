namespace SuppaFlight.UWP
{
    public interface IMeasurementUnit
    {
        string Name { get; }
        string Abbreviation { get; }
        double Convert(double value);
        double Maximum { get; }
        double Tick { get; }
    }

    public class KilometersHourUnit : IMeasurementUnit
    {
        public string Name => "Kilometers/Hour";
        public string Abbreviation => "Km/h";

        public double Convert(double value)
        {
            return value * 3.6;
        }

        public double Maximum => 100;
        public double Tick => 10;
    }

    public class MetersSecondUnit : IMeasurementUnit
    {
        public string Name => "Meters/Second";
        public string Abbreviation => "m/s";

        public double Convert(double value)
        {
            return value;
        }

        public double Maximum => 35;
        public double Tick => 1;
    }
}