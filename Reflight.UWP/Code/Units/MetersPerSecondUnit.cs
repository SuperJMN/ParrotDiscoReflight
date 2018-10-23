namespace ParrotDiscoReflight.Code.Units
{
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
}