using System.Numerics;
using Windows.UI;
using Windows.UI.Xaml;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace ParrotDiscoReflight.Controls
{
    public sealed partial class Plotter
    {
        public Plotter()
        {
            InitializeComponent();
        }

        private void CanvasControl_OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (Point == null)
            {
                return;                
            }

            var x = ActualWidth * Point.X / MaximumWidth;
            var pointY = ActualHeight - ActualHeight * Point.Y / MaximumHeight;

            args.DrawingSession.FillCircle(new Vector2((float) x, (float) pointY), (float)Radius, Color);
        }

        public static readonly DependencyProperty PointProperty = DependencyProperty.Register(
            "Point", typeof(Point), typeof(Plotter), new PropertyMetadata(default(Point), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var plotter = (Plotter) d;
            plotter.CanvasControl.Invalidate();
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(Plotter), new PropertyMetadata(Colors.Black));

        public Color Color
        {
            get { return (Color) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public Point Point
        {
            get { return (Point) GetValue(PointProperty); }
            set { SetValue(PointProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
            "Radius", typeof(double), typeof(Plotter), new PropertyMetadata(1));

        public double Radius
        {
            get { return (double) GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty MaximumWidthProperty = DependencyProperty.Register(
            "MaximumWidth", typeof(double), typeof(Plotter), new PropertyMetadata(default(double)));

        public double MaximumWidth
        {
            get { return (double) GetValue(MaximumWidthProperty); }
            set { SetValue(MaximumWidthProperty, value); }
        }

        public static readonly DependencyProperty MaximumHeightProperty = DependencyProperty.Register(
            "MaximumHeight", typeof(double), typeof(Plotter), new PropertyMetadata(default(double)));

        public double MaximumHeight
        {
            get { return (double) GetValue(MaximumHeightProperty); }
            set { SetValue(MaximumHeightProperty, value); }
        }
    }
}
