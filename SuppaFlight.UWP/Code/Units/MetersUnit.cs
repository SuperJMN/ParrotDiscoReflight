namespace SuppaFlight.UWP.Code.Units
{
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