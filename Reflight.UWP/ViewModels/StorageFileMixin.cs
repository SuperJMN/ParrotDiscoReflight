using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Reflight.Core;
using Reflight.Core.Reader;

namespace ParrotDiscoReflight.ViewModels
{
    public static class StorageFileMixin 
    {
        public static async Task<T> GetProperty<T>(this StorageFile storageFile, string name)
        {
            var dateEncodedPropertyName = name;
            var propertyNames = new List<string>
            {
                dateEncodedPropertyName
            };

            var extraProperties =
                await storageFile.Properties.RetrievePropertiesAsync(propertyNames);

            var propValue = (T) extraProperties[dateEncodedPropertyName];
            return propValue;
        }

        public static async Task<byte[]> GetThumbnail(this StorageFile storageFile)
        {
            if (storageFile == null) throw new ArgumentNullException(nameof(storageFile));

            var storageItemThumbnail = await storageFile.GetThumbnailAsync(ThumbnailMode.VideosView);
            if (storageItemThumbnail == null) return null;

            return await storageItemThumbnail.AsStream().ToByteArray();
        }

        public static async Task<Flight> ReadFlight(this StorageFile file)
        {
            using (var stream = await file.OpenStreamForReadAsync())
            {
                return FlightDataReader.Read(stream);
            }
        }
    }
}