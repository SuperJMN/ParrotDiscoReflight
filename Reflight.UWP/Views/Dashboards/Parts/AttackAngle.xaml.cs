using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Graphics.Canvas.UI.Xaml;

// The Blank UserControl item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ParrotDiscoReflight.Views.Dashboards.Parts
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AttackAngle : UserControl
    {
        public AttackAngle()
        {
            this.InitializeComponent();
        }

        private void CanvasControl_OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var start = new Vector2(0, Value);
            var end = new Vector2((float) ActualWidth, Value);

            //args.DrawingSession.DrawLine(start, () );
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(float), typeof(AttackAngle), new PropertyMetadata(default(float)));

        public float Value
        {
            get { return (float) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }       
    }
}
