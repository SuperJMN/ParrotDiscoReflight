using Windows.Storage;
using Reflight.Core;

namespace ParrotDiscoReflight.Code
{
    public class ExportInput
    {
        public StorageFile InputFile { get; }
        public Flight Flight { get; }
        public StorageFile OutputFile { get; }

        public ExportInput(StorageFile inputFile, Flight flight, StorageFile outputFile)
        {
            InputFile = inputFile;
            Flight = flight;
            OutputFile = outputFile;
        }
    }
}