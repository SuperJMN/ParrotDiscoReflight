using System.Runtime.CompilerServices;

namespace ParrotDiscoReflight.ViewModels
{
    public interface ISettingsSaver
    {
        T Get<T>([CallerMemberName] string propertyName = null);
        void Set<T>(T value, [CallerMemberName] string propertyName = null);
    }
}