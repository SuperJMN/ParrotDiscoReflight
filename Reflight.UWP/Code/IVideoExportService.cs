using System.Reactive;
using System.Threading.Tasks;

namespace ParrotDiscoReflight.Code
{
    public interface IVideoExportService
    {
        Task<Unit> Export(ExportInput exportConfig);
    }
}