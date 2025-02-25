using System.Drawing;
using NeoKolors.Common;
using NeoKolors.Settings;
using Color = System.Drawing.Color;

namespace GAP.core.image.generation.generators;

public class CurlyLines : IBatchableGenerator<CurlyLines> {
    
    public int Width { get; }
    public int Height { get; }
    public int Seed { get; private set; }
    public int LineCount { get; }
    
    public CurlyLines(int width = 512, int height = 512, int seed = 0, int lineCount = 10) {
        Width = width;
        Height = height;
        Seed = seed;
        LineCount = lineCount;
    }
    
    public CurlyLines() {
        Width = 512;
        Height = 512;
        Seed = 0;
        LineCount = 10;
    }
    
    public CurlyLines GetNextGenerator(int i) {
        var @new = (CurlyLines)MemberwiseClone();
        @new.Seed++;
        return @new;
    }

    public Bitmap GenerateImage() {
        var bmp = new Bitmap(Width, Height);
        var g = Graphics.FromImage(bmp);
        var cp = ColorPalette.GeneratePalette(Seed, 5);
        var r = new Random(Seed);
        g.Clear(Color.FromArgb(16, 16, 16));

        var middle = new Point(Width / 2, Height / 2);
        
        for (int i = 0; i < LineCount; i++) {

            Point end;
            var c = Color.FromArgb((255 << 24) | cp[r.Next(0, cp.Colors.Length)]);
            var p = new Pen(c);
            var rad = r.Next(5, 20);
            var rad2 = r.Next(40, 100);
            var f = r.Next(20, 130);
            
            switch (r.Next(0, 3)) {
                case 0:
                    end = new Point(0, r.Next(0, Height));
                    break;
                case 1:
                    end = new Point(Width, r.Next(0, Height));
                    break;
                case 2:
                    end = new Point(r.Next(0, Width), 0);
                    break;
                default:
                    end = new Point(r.Next(0, Width), Height);
                    break;
            }
            
            for (float t = 0; t < Width * 1.5; t += 0.05f) {
                var l = Interpolate(middle, end, (float)(t * f / (Width * 1.5)));
                DrawCircle(g, p, l.X + float.Sin(t) * rad2, l.Y + float.Cos(t) * rad2, rad);
            }
        }

        return bmp;
    }
    
    public static void DrawCircle(Graphics g, Pen pen, float centerX, float centerY, float radius) {
        float diameter = radius * 2;
        g.FillEllipse(pen.Brush, centerX - radius, centerY - radius, diameter, diameter);
    }
    
    public static Point Interpolate(Point p1, Point p2, float t) {
        float x = p1.X + t * (p2.X - p1.X);
        float y = p1.Y + t * (p2.Y - p1.Y);
        return new Point((int)x, (int)y);
    }
    
    private static readonly SettingsBuilder<CurlyLines> SETTING = SettingsBuilder<CurlyLines>.Build("curly_lines", 
        SettingsNode<CurlyLines>
            .New("default")
            .Argument("width", Arguments.Integer(0, Int32.MaxValue, 1000))
            .Argument("height", Arguments.Integer(0, Int32.MaxValue, 1000))
            .Group(IImageGenerator.UniversalSeedInput())
            .Argument("line_count", Arguments.Integer(0, Int32.MaxValue, 10))
            .Constructs(cin => new CurlyLines((int)cin["width"].Get(), (int)cin["height"].Get(), (int)cin["seed"].Get(), (int)cin["line_count"].Get()))
    );
    
    public SettingsBuilder<CurlyLines> GetSettings() {
        return (SettingsBuilder<CurlyLines>)SETTING.Clone();
    }
    
    IImageGenerator IBatchableGenerator.GetNextGenerator(int i) {
        return GetNextGenerator(i);
    }

    ISettingsBuilder<IImageGenerator> IImageGenerator.GetSettings() {
        return GetSettings();
    }
}