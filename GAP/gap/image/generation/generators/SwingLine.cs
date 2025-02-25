//
// Generative Art Program
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

using System.Drawing;
using NeoKolors.Common;
using NeoKolors.Settings;
using Color = System.Drawing.Color;

namespace GAP.core.image.generation.generators;

public class SwingLine : IBatchableGenerator<SwingLine> {
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Seed { get; private set; }
    
    public int LineLifetime { get; private set; }
    public int Step { get; private set; }
    public Color Color { get; }
    
    public double XFactor { get; private set; }
    public double YFactor { get; private set; }


    /// <summary>
    /// 2d SwingLine line image generator
    /// </summary>
    /// <param name="width">image width</param>
    /// <param name="height">image height</param>
    /// <param name="seed">seed</param>
    /// <param name="lineLifetime">how long will the line be</param>
    /// <param name="step">idk what that is</param>
    /// <param name="xFactor">factor for the x coordinate depending on time</param>
    /// <param name="yFactor">factor for the y coordinate depending on time</param>
    /// <param name="color">color of the line</param>
    public SwingLine(int width,
        int height,
        int seed,
        int lineLifetime = 15000,
        int step = 200,
        double? xFactor = null,
        double? yFactor = null,
        Color? color = null) 
    {
        Width = width;
        Height = height;
        Seed = seed;
        LineLifetime = lineLifetime;
        Step = step;
        Color = color ?? Color.FromArgb(ColorPalette.GeneratePalette(seed, 1)[0]);
        var r = new Random(Seed);
        XFactor = xFactor ?? r.NextDouble();
        YFactor = yFactor ?? r.NextDouble();
    }
    
    public SwingLine() {}

    public Bitmap GenerateImage() {
        Bitmap bmp = new Bitmap(Width, Height);
        Random rnd = new Random(Seed);

        int xOffset = Width / 2;
        int yOffset = Height / 2; 
        float factor = Width > Height ? (float)Width * 1/3 : (float)Height * 1/3;
        float startOffset = rnd.NextSingle();
        double xf = rnd.NextDouble();
        double yf = rnd.NextDouble();
        
        float tx = rnd.Next(0, 100) / 10f;
        float ty = rnd.Next(0, 100) / 10f;
        
        Color background = Color.FromArgb(255, 16, 16 ,16);
        var g = Graphics.FromImage(bmp);
        g.Clear(background);
        
        int lx = (int)(xOffset + double.Sin(XFactor * startOffset + tx) * factor * double.Sin(startOffset * xf + tx));
        int ly = (int)(xOffset + double.Sin(XFactor * startOffset + tx) * factor * double.Sin(startOffset * xf + tx));
        
        for (float t = startOffset; t < LineLifetime / (float)Step + startOffset; t += 1 / (float)Step) {
            int nx = (int)(xOffset + double.Sin(XFactor * t + tx) * factor * double.Sin(t * xf + tx));
            int ny = (int)(yOffset + double.Sin(YFactor * t + ty) * factor * double.Sin(t * yf + ty));
            g.DrawLine(new Pen(Color), lx, ly, nx, ny);
            lx = nx;
            ly = ny;
            
            // bmp.SetPixel(
            //     (int)(xOffset + double.Sin(XFactor * t) * factor * double.Sin(t * xf)),
            //     (int)(yOffset + double.Sin(YFactor * t) * factor * double.Sin(t * yf)),
            //     Color);
        }
        
        return bmp;
    }

    ISettingsBuilder<IImageGenerator> IImageGenerator.GetSettings() => GetSettings();
    
    private static readonly ISettingsBuilder<SwingLine> SETTINGS = SettingsBuilder<SwingLine>.Build("swing_line", 
        SettingsNode<SwingLine>.New("swing_line")
            .Group(IImageGenerator.UniversalSeedInput())
            .Argument("width", Arguments.Integer(32))
            .Argument("height", Arguments.Integer(32))
            .Argument("line_lifetime", Arguments.Integer(0))
            .Argument("step", Arguments.Double(0))
            .Constructs(cin => new SwingLine(
                (int)cin["width"].Get(), 
                (int)cin["height"].Get(), 
                (int)cin["seed"].Get(), 
                (int)cin["line_lifetime"].Get(), 
                (int)(1 / (float)cin["step"].Get()))
            )
    );

    public SettingsBuilder<SwingLine> GetSettings() {
        return (SettingsBuilder<SwingLine>)SETTINGS.Clone();
    }

    IImageGenerator IBatchableGenerator.GetNextGenerator(int i) => GetNextGenerator(i);

    public SwingLine GetNextGenerator(int i) {
        SwingLine copy = (SwingLine)MemberwiseClone();
        copy.Seed = i;
        return copy;
    }
}