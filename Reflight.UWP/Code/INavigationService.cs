namespace ParrotDiscoReflight.Code
{
    public interface INavigationService
    {
        void Navigate<T>(T viewModel);
    }
}