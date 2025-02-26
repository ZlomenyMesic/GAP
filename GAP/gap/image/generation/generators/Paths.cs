//
// GAP
// Copyright (c) 2025 KryKom & ZlomenyMesic
// Licensed under the MIT License
// 

using System.Drawing;
using System.Drawing.Drawing2D;
using GapCore;
using NeoKolors.Common;
using NeoKolors.Settings;
using static System.Double;
using Color = System.Drawing.Color;

namespace GAP.gap.image.generation.generators;

public class Paths : IBatchableGenerator<Paths> {
    
    public int Seed { get; private set; }
    public int Width { get; private set; }
    public int GridWidth { get; private set; }
    public int Height { get; private set; }
    public int GridHeight { get; private set; }
    public int UnitSize { get; private set; }
    

    public int PathLifetime { get; private set; }
    public int PathLifetimeTolerance { get; private set; }
    public int PathThickness { get; private set; }
    public int PathCount { get; private set; }
    

    public Paths(int gridWidth, int gridHeight, int seed, int unitSize = 7, int pathLifetime = 7, 
        int pathLifetimeTolerance = 3, int pathThickness = 2, int pathCount = 5) 
    {
        Seed = seed;
        GridWidth = gridWidth;
        GridHeight = gridHeight;
        UnitSize = unitSize;
        PathLifetime = pathLifetime;
        PathLifetimeTolerance = pathLifetimeTolerance;
        PathThickness = pathThickness;
        PathCount = pathCount;
        Width = gridWidth * unitSize;
        Height = gridHeight * unitSize;
    }
    
    public Paths() {}
    
    public Bitmap GenerateImage() {
        Bitmap bmp = new Bitmap(Width, Height);
        ColorPalette palette = ColorPalette.GeneratePalette(Seed, 6);
        Random rnd = new Random(Seed);
        int offset = UnitSize / 2 + 1;
        
        int minI = 0;
        double minV = MaxValue;
        
        for (int i = 0; i < palette.Colors.Length; i++) {
            (_, _, double v) = ColorFormat.ColorToHsv(Color.FromArgb(palette.Colors[i]));

            if (!(v < minV)) continue;
            
            minV = v;
            minI = i;
        }
        
        // choose a background color from the palette
        int background = palette.Colors[minI] + (0xff << 24);
        
        Graphics g = Graphics.FromImage(bmp);
        g.Clear(Color.FromArgb(background));
        
        Pen pathPen = new Pen(Color.FromArgb(background));
        pathPen.StartCap = LineCap.Square;
        pathPen.EndCap = LineCap.Square;
        pathPen.Width = PathThickness;
        
        Pen erasePen = new Pen(Color.FromArgb(background));
        erasePen.StartCap = LineCap.Square;
        erasePen.EndCap = LineCap.Square;
        erasePen.Width = PathThickness + 1;
        
        for (int i = 0; i < PathCount; i++) {
            var path = GeneratePath(i);
            pathPen.Color = Color.FromArgb(palette.Colors[rnd.Next(0, palette.Colors.Length)] + (0xff << 24));
            
            foreach (var points in path) {
                g.DrawLine(erasePen, 
                    new Point(points.a.x * UnitSize + offset, points.a.y * UnitSize + offset), 
                    new Point(points.b.x * UnitSize + offset, points.b.y * UnitSize + offset));
                g.DrawLine(pathPen, 
                    new Point(points.a.x * UnitSize + offset, points.a.y * UnitSize + offset), 
                    new Point(points.b.x * UnitSize + offset, points.b.y * UnitSize + offset));
            }
        }
        
        return bmp;
    }

    ISettingsBuilder<IImageGenerator> IImageGenerator.GetSettings() => GetSettings();

    public IImageGenerator GetEmptyInstance() => new Paths(0, 0, 0, 0, 0, 0, 0, 0);

    private ((int x, int y) a, (int x, int y) b)[] GeneratePath(int seedOffset) {
        Random rnd = new Random(Seed + 1 + seedOffset);
        int length = PathLifetime + rnd.Next(-PathLifetimeTolerance, PathLifetimeTolerance);
        (int x, int y) start = (rnd.Next(GridWidth - 1), rnd.Next(GridHeight - 1));
        var path = new List<((int x, int y) a, (int x, int y) b)>();
        int lastDirection = 4;

        for (int i = 0; i < length; i++) {
            (var next, lastDirection) = GenerateNext(start, lastDirection, seedOffset);
            path.Add((start, next));
            start = next;
        }
        
        return path.ToArray();
    }

    private ((int x, int y) pos, int direction) GenerateNext((int x, int y) start, int lastDirection, int seedOffset) {
        Random rnd = new Random(Seed + 2 + seedOffset);
        int direction = 0;

        while (direction == lastDirection) {
            direction = rnd.Next(0, 3);
        }

        return direction switch {
            0 => ((start.x, rnd.Next(start.y, GridHeight)), direction),
            1 => ((start.x, rnd.Next(start.y)), direction),
            2 => ((rnd.Next(start.x, GridWidth), start.y), direction),
            3 => ((rnd.Next(start.x), start.y), direction),
            _ => ((start.x, rnd.Next(start.y, GridHeight)), direction),
        };
    }

    private static readonly SettingsBuilder<Paths> SETTINGS = SettingsBuilder<Paths>.Build("paths",
        SettingsNode<Paths>.New("basic")
            .Group(IImageGenerator.UniversalSeedInput())
            .Argument("grid_width", Arguments.Integer(2))
            .Argument("grid_height", Arguments.Integer(2))
            .Constructs(cin => new Paths((int)cin["grid_width"].Get(), 
                (int)cin["grid_height"].Get(), 
                (int)cin["seed"].Get())
            ),
        
        SettingsNode<Paths>.New("advanced")
            .Group(IImageGenerator.UniversalSeedInput())
            .Argument("grid_width", Arguments.Integer(2))
            .Argument("grid_height", Arguments.Integer(2))
            .Argument("path_lifetime", Arguments.Integer(1))
            .Argument("path_lifetime_tolerance", Arguments.Integer(0))
            .Argument("path_thickness", Arguments.Integer(1))
            .Argument("path_count", Arguments.Integer(1))
            .Argument("unit_size", Arguments.Integer(1))
            .Constructs(cin => new Paths((int)cin["grid_width"].Get(), 
                (int)cin["grid_height"].Get(), 
                (int)cin["seed"].Get(), 
                (int)cin["unit_size"].Get(),
                (int)cin["path_lifetime"].Get(),
                (int)cin["path_lifetime_tolerance"].Get(),
                (int)cin["path_thickness"].Get(),
                (int)cin["path_count"].Get())
            )
    );
    
    public SettingsBuilder<Paths> GetSettings() {
        return (SettingsBuilder<Paths>)SETTINGS.Clone();
    }

    public Paths GetNextGenerator(int i) {
        Paths copy = (Paths)MemberwiseClone();
        copy.Seed = i;
        return copy;
    }
    
    IImageGenerator IBatchableGenerator.GetNextGenerator(int i) {
        return GetNextGenerator(i);
    }
}