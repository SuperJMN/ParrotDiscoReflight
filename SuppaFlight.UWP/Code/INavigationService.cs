namespace SuppaFlight.UWP.Code
{
    public interface INavigationService
    {
        void Navigate<T>(T viewModel);
    }
}