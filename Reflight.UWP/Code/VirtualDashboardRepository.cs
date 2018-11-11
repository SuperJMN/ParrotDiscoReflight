using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using ParrotDiscoReflight.ViewModels;

namespace ParrotDiscoReflight.Code
{
    internal class VirtualDashboardRepository : IVirtualDashboardsRepository
    {
        private readonly ResourceDictionary source;

        public VirtualDashboardRepository(ResourceDictionary source)
        {
            this.source = source;
        }
        public ICollection<VirtualDashboard> GetAll()
        {
            var virtualDashboards = from tuple in source
                where tuple.Value is DataTemplate
                select new VirtualDashboard((string)tuple.Key);

            return virtualDashboards.ToList();
        }        
    }
}