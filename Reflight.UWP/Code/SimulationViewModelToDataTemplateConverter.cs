using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ParrotDiscoReflight.Code
{
    public class SimulationViewModelToDataTemplateConverter : DataTemplateSelector
    {
        public ResourceDictionary ResourceDictionary { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is ISimulationViewModel vm)
            {
                var dataTemplate = ResourceDictionary[vm.PresentationOptions.Dashboard.Template] as DataTemplate;
                return dataTemplate;
            }
            
            return base.SelectTemplateCore(item, container);
        }
    }
}