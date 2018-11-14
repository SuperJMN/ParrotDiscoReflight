using System;
using System.Threading.Tasks;
using Windows.Storage;
using ParrotDiscoReflight.ViewModels;
using Reflight.Core;

namespace ParrotDiscoReflight.Code
{
    public interface IExporter
    {
        Task Export(Video video, Flight flight, StorageFile output, Action<Status> onNewStatus);
    }
}