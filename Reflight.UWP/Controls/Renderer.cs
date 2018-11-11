using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace ParrotDiscoReflight.Controls
{
    class Renderer
    {
        public static void RenderData(CanvasControl canvas, CanvasDrawEventArgs args, Color color, float thickness, List<double> data, bool renderArea)
        {
            using (var cpb = new CanvasPathBuilder(args.DrawingSession))
            {
                var max = data.Max();
                var spacing = canvas.ActualWidth / (data.Count -1);

                for (int i = 0; i < data.Count; i++)
                {
                    var value = data[i];
                    var y = GetY(canvas.ActualHeight, value, max);

                    var x = i * spacing;

                    if (i == 0)
                    {
                        cpb.BeginFigure(new Vector2((float) x, (float)y));
                    }
                    else
                    {
                        
                        cpb.AddLine(new Vector2((float) x, (float)y));
                    }
                }

                if (renderArea)
                {
                    cpb.AddLine(new Vector2(data.Count, (float)canvas.ActualHeight));
                    cpb.AddLine(new Vector2(0, (float)canvas.ActualHeight));
                    cpb.EndFigure(CanvasFigureLoop.Closed);
                    args.DrawingSession.FillGeometry(CanvasGeometry.CreatePath(cpb), Colors.LightGreen);
                }
                else
                {
                    cpb.EndFigure(CanvasFigureLoop.Open);
                    args.DrawingSession.DrawGeometry(CanvasGeometry.CreatePath(cpb), color, thickness);
                }
            }
        }

        private static double GetY(double actualHeight, double value, double max)
        {
            var invertedY = value * actualHeight / max;
            return actualHeight - invertedY;
        }
    }
}