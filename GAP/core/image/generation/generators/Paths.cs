using System.Drawing;
using System.Drawing.Drawing2D;
using GAP.util.settings;
using Kolors;
using static System.Double;

namespace GAP.core.image.generation.generators;

public class Paths : IImageGenerator, IBatchableGenerator {
    
    private int seed { get; set; }
    private int width { get; set; }
    private int gridWidth { get; set; }
    private int height { get; set; }
    private int gridHeight { get; set; }
    private int unitSize { get; set; }
    
    public int Seed => seed;
    public int Width => width;
    public int GridWidth => gridWidth;
    public int Height => height;
    public int GridHeight => gridHeight;
    public int UnitSize => unitSize;

    private int pathLifetime { get; set; }
    private int pathLifetimeTolerance { get; set; }
    private int pathThickness { get; set; }
    private int pathCount { get; set; }
    
    public int PathCount => pathCount;
    public int PathLifetime => pathLifetime;
    public int PathLifetimeTolerance => pathLifetimeTolerance;
    public int PathThickness => pathThickness;

    public Paths(int gridWidth, int gridHeight, int seed, int unitSize = 7, int pathLifetime = 7, 
        int pathLifetimeTolerance = 3, int pathThickness = 2, int pathCount = 5) 
    {
        this.seed = seed;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.unitSize = unitSize;
        this.pathLifetime = pathLifetime;
        this.pathLifetimeTolerance = pathLifetimeTolerance;
        this.pathThickness = pathThickness;
        this.pathCount = pathCount;
        width = gridWidth * unitSize;
        height = gridHeight * unitSize;
    }
    
    public Bitmap GenerateImage() {
        Bitmap bmp = new Bitmap(width, height);
        ColorPalette palette = ColorPalette.GeneratePalette(seed, 6);
        Random rnd = new Random(seed);
        int offset = unitSize / 2 + 1;
        
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
        pathPen.Width = pathThickness;
        
        Pen erasePen = new Pen(Color.FromArgb(background));
        erasePen.StartCap = LineCap.Square;
        erasePen.EndCap = LineCap.Square;
        erasePen.Width = pathThickness + 1;
        
        for (int i = 0; i < pathCount; i++) {
            var path = GeneratePath(i);
            pathPen.Color = Color.FromArgb(palette.Colors[rnd.Next(0, palette.Colors.Length)] + (0xff << 24));
            
            foreach (var points in path) {
                g.DrawLine(erasePen, 
                    new Point(points.a.x * unitSize + offset, points.a.y * unitSize + offset), 
                    new Point(points.b.x * unitSize + offset, points.b.y * unitSize + offset));
                g.DrawLine(pathPen, 
                    new Point(points.a.x * unitSize + offset, points.a.y * unitSize + offset), 
                    new Point(points.b.x * unitSize + offset, points.b.y * unitSize + offset));
            }
        }
        
        return bmp;
    }

    private ((int x, int y) a, (int x, int y) b)[] GeneratePath(int seedOffset) {
        Random rnd = new Random(seed + 1 + seedOffset);
        int length = pathLifetime + rnd.Next(-pathLifetimeTolerance, pathLifetimeTolerance);
        (int x, int y) start = (rnd.Next(gridWidth - 1), rnd.Next(gridHeight - 1));
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
        Random rnd = new Random(seed + 2 + seedOffset);
        int direction = 0;

        while (direction == lastDirection) {
            direction = rnd.Next(0, 3);
        }

        return direction switch {
            0 => ((start.x, rnd.Next(start.y, gridHeight)), direction),
            1 => ((start.x, rnd.Next(start.y)), direction),
            2 => ((rnd.Next(start.x, gridWidth), start.y), direction),
            3 => ((rnd.Next(start.x), start.y), direction),
            _ => ((start.x, rnd.Next(start.y, gridHeight)), direction),
        };
    }

    private static readonly ISettingsBuilder<Paths, Paths> SETTINGS = SettingsBuilder<Paths>.Build("paths",
        SettingsNode<Paths>.New("basic")
            .Group(IImageGenerator.UniversalSeedInput())
            .Argument("grid_width", Arguments.Integer(2))
            .Argument("grid_height", Arguments.Integer(2))
            .OnParse(cin => new Paths((int)cin["grid_width"].GetParsedValue(), 
                (int)cin["grid_height"].GetParsedValue(), 
                (int)cin["seed"].GetParsedValue())
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
            .OnParse(cin => new Paths((int)cin["grid_width"].GetParsedValue(), 
                (int)cin["grid_height"].GetParsedValue(), 
                (int)cin["seed"].GetParsedValue(), 
                (int)cin["unit_size"].GetParsedValue(),
                (int)cin["path_lifetime"].GetParsedValue(),
                (int)cin["path_lifetime_tolerance"].GetParsedValue(),
                (int)cin["path_thickness"].GetParsedValue(),
                (int)cin["path_count"].GetParsedValue())
            )
    );

    public static ISettingsBuilder<Paths, Paths> GetSettings() {
        return SETTINGS.Clone();
    }

    public IImageGenerator GetNextGenerator(int i) {
        Paths copy = (Paths)MemberwiseClone();
        copy.seed = i;
        return copy;
    }
}