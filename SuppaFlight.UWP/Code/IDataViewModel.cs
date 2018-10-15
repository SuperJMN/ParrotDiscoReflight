using SuppaFlight.UWP.Code.Units;

namespace SuppaFlight.UWP.Code
{
    public interface IDataViewModel
    {
        StatusViewModel Status { get; }
        UnitPack UnitPack { get; }
    }
}