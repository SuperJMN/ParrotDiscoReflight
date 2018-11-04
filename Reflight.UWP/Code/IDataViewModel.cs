using ParrotDiscoReflight.Code.Units;
using ParrotDiscoReflight.ViewModels;

namespace ParrotDiscoReflight.Code
{
    public interface IDataViewModel
    {
        StatusViewModel Status { get; }
        UnitPack Units { get; }
    }
}