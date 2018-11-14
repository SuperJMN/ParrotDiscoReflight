using System.Collections.Generic;

namespace ParrotDiscoReflight.ViewModels
{
    public interface IVirtualDashboardsRepository
    {
        ICollection<VirtualDashboard> GetAll();
    }
}