using ScottPlot;
using SkiaSharp;

namespace Core.ScottPlotCustom
{
    public class RainbowPlot(double[] xs, double[] ys) : IPlottable
    {
        // data and customization options
        double[] Xs { get; } = xs; 
        double[] Ys { get; } = ys; 
        public float Radius { get; set; } = 10;
        public IColormap Colormap { get; set; } = new ScottPlot.Colormaps.Turbo();
        
        // items required by IPlottable
        public bool IsVisible { get; set; } = true;
        public IAxes Axes { get; set; } = new Axes();
        public IEnumerable<LegendItem> LegendItems => LegendItem.None;
        public AxisLimits GetAxisLimits() => new(Xs.Min(), Xs.Max(), Ys.Min(), Ys.Max());

        public void Render(RenderPack rp)
        {
            FillStyle FillStyle = new();
            using SKPaint paint = new();
            for (int i = 0; i < Xs.Length; i++)
            {
                // convert coordinate location to screen location
                Coordinates centerCoordinates = new(Xs[i], Ys[i]);
                Pixel centerPixel = Axes.GetPixel(centerCoordinates);

                // draw the colored circle
                FillStyle.Color = Colormap.GetColor(i / (Xs.Length - 1.0));
                ScottPlot.Drawing.DrawCircle(rp.Canvas, centerPixel, Radius, FillStyle, paint);
            }
        }
    }
}
