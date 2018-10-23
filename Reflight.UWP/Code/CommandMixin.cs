using System;

namespace ParrotDiscoReflight.Code
{
    public static class CommandMixin
    {
        public static IDisposable MessageOnException(this IObservable<Exception> obs, IDialogService dialogService)
        {
            return obs.Subscribe(x => dialogService.ShowError("Error", x.Message));
        }
    }
}