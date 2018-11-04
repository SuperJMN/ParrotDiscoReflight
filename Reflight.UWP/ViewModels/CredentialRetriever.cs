using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Reflight.Core.FlightAcademy;

namespace ParrotDiscoReflight.ViewModels
{
    public class CredentialRetriever 
    {
        private readonly HttpClient httpClient;

        public CredentialRetriever(Uri uri)
        {
            httpClient = new HttpClient()
            {
                BaseAddress = uri,
            };
        }

        public async Task<Credentials> RetrieveCredentials(string email, string password)
        {
            var names = new []
            {
                new KeyValuePair<string, string>("login", email),
                new KeyValuePair<string, string>("password", password)
            };
            var formUrlEncodedContent = new FormUrlEncodedContent(names);
            var rp = await httpClient.PostAsync(new Uri("https://accounts.parrot.com/V3/logform"), formUrlEncodedContent);
            var uri = rp.RequestMessage.RequestUri;
            var parsed = ParseQueryString(uri.ToString());

            if (!parsed.TryGetValue("ca", out var credentials))
            {
                throw new InvalidLoginException("Invalid credentials. Cannot log to My.Parrot");
            }

            var des = JObject.Parse(credentials);

            var givenUsername = (string) des["user"];
            var givenPassword = (string) des["pwd"];
            return new Credentials(givenUsername, givenPassword);
        }

        public static Dictionary<string, string> ParseQueryString(string requestQueryString)
        {
            Dictionary<string, string> rc = new Dictionary<string, string>();
            string[] ar1 = requestQueryString.Split('&', '?');
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