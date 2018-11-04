namespace ParrotDiscoReflight.Code.Units
{
    public class KilometersUnit : IMeasurementUnit
    {
        public string Name => "Kilometers";
        public string Abbreviation => "Km";
        public double Convert(double value)
        {
            return value / 1000D;
        }

        public double Maximum { get; }
        public double Tick { get; }
        public string StringFormat => $"{{0:F}} {Abbreviation}";
    }
}