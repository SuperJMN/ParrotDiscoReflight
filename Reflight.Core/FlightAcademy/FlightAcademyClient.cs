using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Refit;
using Reflight.Core.Reader;

namespace Reflight.Core.FlightAcademy
{
    public class FlightAcademyWrapper : IFlightAcademyClient
    {
        private readonly IFlightAcademyClient inner;

        public FlightAcademyWrapper(IFlightAcademyClient inner)
        {
            this.inner = inner;
        }

        public Task<ICollection<FlightSummary>> GetFlights(int page, int paginate_by)
        {
            return SafeCall(() => inner.GetFlights(page, paginate_by));
        }

        public async Task<TOutput> SafeCall<TOutput>(Func<Task<TOutput>> func)
        {
            try
            {
                return await func();
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Forbidden)
                    throw new InvalidCredentialException(
                        "Your Parrot Flight Academy credentials seem to be incorrect. Please check them in the Settings page");

                throw;
            }
        }

        public Task<FlightDetails> GetFlight(int id)
        {
            return SafeCall(() => inner.GetFlight(id));
        }
    }

    public static class FlightAcademyClient
    {
        public static IFlightAcademyClient Create(string username, string password, Uri uri)
        {
            return new FlightAcademyWrapper(CreateInner(username, password, uri));
        }

        private static IFlightAcademyClient CreateInner(string username, string password, Uri uri)
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

            var encoded = Encoding.ASCII.GetBytes($"{username}:{password}");
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
    }
}