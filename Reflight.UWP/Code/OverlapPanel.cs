using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static System.Math;

namespace ParrotDiscoReflight.Code
{
    public class OverlapPanel : Panel
    {
        public static readonly DependencyProperty OffsetXProperty = DependencyProperty.Register(
            "OffsetX", typeof(double), typeof(OverlapPanel), new PropertyMetadata(10D));

        public double OffsetX
        {
            get { return (double) GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

        public static readonly DependencyProperty OffsetYProperty = DependencyProperty.Register(
            "OffsetY", typeof(double), typeof(OverlapPanel), new PropertyMetadata(10D));

        public double OffsetY
        {
            get { return (double) GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double width = 0;
            double height = 0;
            double offsetX = 0;
            double offsetY = 0;

            foreach (var child in Children)
            {
                child.Measure(availableSize);
                width = Max(width, child.DesiredSize.Width + offsetX);
                height = Max(height, child.DesiredSize.Height + offsetY);
                offsetX += OffsetX;
                offsetY += OffsetY;
            }

            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int i = 0;

            foreach (var child in Children)
            {
                child.Arrange(new Rect(OffsetX * i, OffsetY * i, child.DesiredSize.Width, child.DesiredSize.Height));
                i++;
            }
            return base.ArrangeOverride(finalSize);
        }
    }
}