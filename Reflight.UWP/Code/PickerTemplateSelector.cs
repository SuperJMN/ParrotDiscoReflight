using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ParrotDiscoReflight.ViewModels;

namespace ParrotDiscoReflight.Code
{
    public class PickerTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ManualTemplate { get; set; }

        public DataTemplate FlightAcademyTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            switch (item)
            {
                case GalleryPickViewModel _:
                    return FlightAcademyTemplate;
                case ManualPickViewModel _:
                    return ManualTemplate;
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}