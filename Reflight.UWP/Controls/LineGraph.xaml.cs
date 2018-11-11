using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace ParrotDiscoReflight.Controls
{
    public sealed partial class LineGraph
    {
        public LineGraph()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(
            "Values", typeof(IEnumerable<double>), typeof(LineGraph), new PropertyMetadata(default(IEnumerable<double>)));

        public IEnumerable<double> Values
        {
            get { return (IEnumerable<double>) GetValue(ValuesProperty); }
            set { SetValue(ValuesProperty, value); }
        }

        private void CanvasControl_OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (Values == null)
            {
                return;                
            }

            Renderer.RenderData(CanvasControl, args, Color, 1, Values.ToList(), false);
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color", typeof(Color), typeof(LineGraph), new PropertyMetadata(Colors.Black));

        public Color Color
        {
            get { return (Color) GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
    }
}
