using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Reflight.Core.Reader;

namespace Reflight.Core.FlightAcademy
{
    public interface IFlightAcademyClient
    {
        [Get("/runs")]
        Task<ICollection<FlightSummary>> GetFlights(int page, int paginate_by);

        [Get("/runs/{id}/details")]
        Task<FlightDetails> GetFlight([AliasAs("id")] int id);
    }
}