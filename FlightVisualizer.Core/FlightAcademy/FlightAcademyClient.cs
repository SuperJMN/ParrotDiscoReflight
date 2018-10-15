using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Refit;

namespace FlightVisualizer.Core.FlightAcademy
{
    public static class FlightAcademyClient
    {
        public static IFlightAcademyClient Create(string username, string password, Uri uri)
        {
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                Credentials = new NetworkCredential(username, password),
            };

            var httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = uri,
            };

            var encoded = Encoding.ASCII.GetBytes($"{username}:{password}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(encoded));

            return RestService.For<IFlightAcademyClient>(httpClient, new RefitSettings()
            {
                JsonSerializerSettings = new JsonSerializerSettings()
                {
                    //20181007111948
                    DateFormatString = "yyyyMMddHHmmss",
                    Converters = { new MyTimeSpanConverter() }
                }
            });
        }
    }

    public class MyTimeSpanConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var timeSpan = TimeSpan.FromMilliseconds(Convert.ToDouble(reader.Value));
            return timeSpan;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(TimeSpan))
            {
                return true;
            }

            return false;
        }
    }
}