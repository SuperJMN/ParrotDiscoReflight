using System;
using System.Threading.Tasks;
using Windows.Storage;
using NodaTime;
using NodaTime.Extensions;

namespace ParrotDiscoReflight.ViewModels
{
    public class Video
    {
        public StorageFile Source { get; set; }
        public Interval? RecordedInterval { get; set; }

        public byte[] Thumbnail { get; private set; }

        public static async Task<Video> Load(StorageFile source)
        {
            var dateEncoded = await source.GetProperty<DateTimeOffset?>(StorageFileProperty.DateEncoded);
            var encodedDuration = await source.GetProperty<ulong?>(StorageFileProperty.Duration);

            Interval? recordedInterval = null;

            if (dateEncoded.HasValue && encodedDuration.HasValue)
            {
                var duration = ToTimeSpan(encodedDuration.Value);
                var end = dateEncoded.Value.Add(duration);
                recordedInterval = new Interval(dateEncoded.Value.ToInstant(), end.ToInstant());
            }

            return new Video
            {
                RecordedInterval = recordedInterval,
                Source = source,
                Thumbnail = await source.GetThumbnail()
            };
        }

        private static TimeSpan ToTimeSpan(ulong encodedDurationValue)
        {
            return TimeSpan.FromMilliseconds(0.0001 * encodedDurationValue);
        }
    }
}