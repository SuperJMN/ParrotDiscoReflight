using System.Threading.Tasks;

namespace ParrotDiscoReflight.Code
{
    public interface IDialogService
    {
        Task ShowError(string title, string message);
    }
}