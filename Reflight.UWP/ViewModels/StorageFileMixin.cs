using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

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
    }

    public class StorageFileProperty
    {
        public static string DateEncoded = "System.Media.DateEncoded";
        public static string Duration = "System.Media.Duration";
    }
}