namespace SuppaFlight.UWP.Code
{
    public interface IMeasurementUnit
    {
        string Name { get; }
        string Abbreviation { get; }
        double Convert(double value);
        double Maximum { get; }
        double Tick { get; }
        string StringFormat { get; }
    }
}