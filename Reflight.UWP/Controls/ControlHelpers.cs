using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace ParrotDiscoReflight.Controls
{
    internal static class ControlHelpers
    {
        /// <summary>
        /// Get the visual associated with an UIElement
        /// </summary>
        /// <param name="element">Source UIElement</param>
        /// <returns>ContainerVisual associated with the element</returns>
        public static ContainerVisual GetVisual(this UIElement element)
        {
            var hostVisual = ElementCompositionPreview.GetElementVisual(element);
            var root = hostVisual.Compositor.CreateContainerVisual();
            ElementCompositionPreview.SetElementChildVisual(element, root);
            return root;
        }
    }
}