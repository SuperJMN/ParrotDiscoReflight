using Windows.Storage;
using FlightVisualizer.Core;

namespace SuppaFlight.UWP.Code
{
    public class ExportInput
    {
        public StorageFile InputFile { get; }
        public FlightData FlightData { get; }
        public StorageFile OutputFile { get; }

        public ExportInput(StorageFile inputFile, FlightData flightData, StorageFile outputFile)
        {
            InputFile = inputFile;
            FlightData = flightData;
            OutputFile = outputFile;
        }
    }
}