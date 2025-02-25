using System.Drawing;
using NeoKolors.Settings;
using ColorPalette = NeoKolors.Common.ColorPalette;

namespace GAP.core.image.generation.generators;

public class DottedLines : IImageGenerator<DottedLines> {
    
    public int Width { get; }
    public int Height { get; }
    public int Seed { get; }
    public int DotSize { get; }
    public int DotSpacing { get; }
    public int PathCount { get; }
    public int ColorCount { get; }
    
    public DottedLines(int width = 512, int height = 512, int seed = 0, int dotSize = 5, int dotSpacing = 8, int pathCount = 10, int colorCount = 10) {
        Width = width;
        Height = height;
        Seed = seed;
        DotSize = dotSize;
        DotSpacing = dotSpacing;
        PathCount = pathCount;
        ColorCount = colorCount; 
    }
    
    public DottedLines() {
        Width = 512;
        Height = 512;
        Seed = 0;
        DotSize = 5;
        DotSpacing = 8;
        PathCount = 10;
        ColorCount = 10;
    }
    
    public Bitmap GenerateImage() {
        var bmp = new Bitmap(Width, Height);
        var g = Graphics.FromImage(bmp);
        var cp = ColorPalette.GeneratePalette(Seed, PathCount);
        var r = new Random(Seed);
        var pen = new Pen(Color.Black, 1);
        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
        
        for (int i = 0; i < PathCount; i++) {
            Color color = Color.FromArgb(255, 255, 255);
            g.FillEllipse(new SolidBrush(color), 0, 0, DotSize, DotSize);
        }
        
        return bmp;
    }

    ISettingsBuilder<IImageGenerator> IImageGenerator.GetSettings() {
        return GetSettings();
    }

    public SettingsBuilder<DottedLines> GetSettings() {
        throw new NotImplementedException();
    }
}