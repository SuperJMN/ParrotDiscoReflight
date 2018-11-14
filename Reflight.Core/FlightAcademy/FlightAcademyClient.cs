using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refit;

namespace Reflight.Core.FlightAcademy
{
    public static class FlightAcademyClient
    {
        public static async Task<IFlightAcademyClient> Create(string username, string password, Uri uri)
        {
            return new FlightAcademyWrapper(await CreateInner(username, password, uri));
        }

        private static async Task<IFlightAcademyClient> CreateInner(string username, string password, Uri uri)
        {
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                Credentials = new NetworkCredential(username, password)
            };

            var httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = uri
            };

            byte[] encoded = await GetEncodedCredentials(httpClient, username, password, uri);
            if (username.Contains("@"))
            {

            }

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(encoded));

            return RestService.For<IFlightAcademyClient>(httpClient, new RefitSettings
            {
                JsonSerializerSettings = new JsonSerializerSettings
                {
                    Converters =
                    {
                        new TimeSpanJsonConverter(),
                        new JsonDateTimeOffsetConverter()
                    }
                }
            });
        }

        private static async Task<byte[]> GetEncodedCredentials(HttpClient httpClient, string username, string password, Uri baseUri)
        {
            if (username.Contains("@"))
            {
                var names = new []
                {
                    new KeyValuePair<string, string>("login", "superjmn@outlook.com"),
                    new KeyValuePair<string, string>("password", "blacksun")
                };
                var formUrlEncodedContent = new FormUrlEncodedContent(names);
                var rp = await httpClient.PostAsync(new Uri("https://accounts.parrot.com/V3/logform"), formUrlEncodedContent);
                var uri = rp.RequestMessage.RequestUri;
                var parsed = ParseQueryString(uri.ToString());
                var credentials = parsed["ca"];
                var des = JObject.Parse(credentials);

                username = (string) des["user"];
                password = (string) des["pwd"];
            }

            return Encoding.ASCII.GetBytes($"{username}:{password}");
        }

        public static Dictionary<string, string> ParseQueryString(string requestQueryString)
        {
            Dictionary<string, string> rc = new Dictionary<string, string>();
            string[] ar1 = requestQueryString.Split(new char[] { '&', '?' });
            foreach (string row in ar1)
            {
                if (string.IsNullOrEmpty(row)) continue;
                int index = row.IndexOf('=');
                if (index < 0) continue;
                rc[Uri.UnescapeDataString(row.Substring(0, index))] = Uri.UnescapeDataString(row.Substring(index + 1)); // use Unescape only parts          
            }
            return rc;
        }
    }
}