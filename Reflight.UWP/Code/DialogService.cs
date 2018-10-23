using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace ParrotDiscoReflight.Code
{
    public class DialogService : IDialogService
    {
        public Task ShowError(string title, string message)
        {
            var dialog = new MessageDialog(message) { Title = title };
            return dialog.ShowAsync().AsTask();
        }
    }
}