using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
                {
                    throw new InvalidLoginException("Your My.Parrot credentials seem to be incorrect. Please check them in the Settings page");
                }

                throw;

            }
        }

        public Task<FlightDetails> GetFlight(int id)
        {
            return SafeCall(() => inner.GetFlight(id));
        }
    }
}