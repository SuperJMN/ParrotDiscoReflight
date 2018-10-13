namespace SuppaFlight.UWP.Code.Units
{
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
}