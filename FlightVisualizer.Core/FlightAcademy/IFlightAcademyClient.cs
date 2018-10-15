using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace FlightVisualizer.Core.FlightAcademy
{
    public interface IFlightAcademyClient
    {
        [Get("/runs")]
        Task<ICollection<Flight>> GetFlights(int page, int paginate_by);

        [Get("/runs/{id}/details")]
        Task<IntermediateData> GetFlight([AliasAs("id")] int id);
    }
}