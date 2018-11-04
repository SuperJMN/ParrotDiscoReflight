namespace ParrotDiscoReflight.Code.Units
{
    public class MilesUnit : IMeasurementUnit
    {
        public string Name => "Miles";
        public string Abbreviation => "mi";
        public double Convert(double value)
        {
            return value * 0.00062137D;
        }

        public double Maximum { get; }
        public double Tick { get; }
        public string StringFormat => $"{{0:F}} {Abbreviation}";
    }
}