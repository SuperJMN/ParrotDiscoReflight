namespace ParrotDiscoReflight.Code.Units
{
    public class FeetUnit : IMeasurementUnit
    {
        public string Name => "Feet";
        public string Abbreviation => "ft";

        public double Convert(double value)
        {
            return value * 3.2808399;
        }

        public double Maximum => 500;
        public double Tick => 25;
        public string StringFormat => $"{{0:F}} {Abbreviation}";
    }
}