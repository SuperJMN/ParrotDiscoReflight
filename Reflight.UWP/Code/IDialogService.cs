using System.Threading.Tasks;
using Windows.UI.Popups;

namespace ParrotDiscoReflight.Code
{
    public interface IDialogService
    {
        Task ShowError(string title, string message);
        Task ShowMessage(string message);
    }
}