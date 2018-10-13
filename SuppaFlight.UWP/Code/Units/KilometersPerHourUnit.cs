namespace SuppaFlight.UWP.Code.Units
{
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
}