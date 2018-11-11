using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParrotDiscoReflight.ViewModels
{
    public interface IVirtualDashboardsRepository
    {
        ICollection<VirtualDashboard> GetAll();
    }
}