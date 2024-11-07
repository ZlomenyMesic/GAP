using System.Drawing;
using GAP.util.exceptions;
using GAP.util.settings;
using Kolors;

namespace GAP.core.image.generation.generators;

public class Rectangles : IImageGenerator {

    private int width { get; set; } = 128;
    private int height { get; set; } = 128;
    private int seed { get; set; } = 0;
    
    public int Width => width;
    public int Height => height;
    public int Seed => seed;

    private int gridWidth { get; set; } = 8;
    private int gridHeight { get; set; } = 8;
    private int pixelsPerUnit { get; set; } = 8;
    private int gapThickness { get; set; } = 4;
    
    public int GridWidth => gridWidth;
    public int GridHeight => gridHeight;
    public int PixelsPerUnit => pixelsPerUnit;
    public int GapThickness => gapThickness;
    
    public Rectangles(int gridWidth, int gridHeight, int pixelsPerUnit, int gapThickness, int seed) {
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.pixelsPerUnit = pixelsPerUnit;
        this.gapThickness = gapThickness;
        this.seed = seed;
        
        width = gridWidth * pixelsPerUnit + (gridWidth - 1) * gapThickness;
        height = gridHeight * pixelsPerUnit + (gridHeight - 1) * gapThickness;
    }
    
    public Rectangles() { }
    
    public Bitmap GenerateImage() {
        
        // create a palette
        ColorPalette palette = ColorPalette.GeneratePalette(seed);

        double minV = int.MaxValue;
        int minI = 0;
        
        for (int i = 0; i < palette.Colors.Length; i++) {
            (_, _, double v) = ColorFormat.ColorToHsv(Color.FromArgb(palette.Colors[i]));

            if (!(v < minV)) continue;
            
            minV = v;
            minI = i;
        }
        
        // choose a background color from the palette
        int background = palette.Colors[minI] + (0xff << 24);
        
        Bitmap output = new Bitmap(width, height);

        // fill it with the 
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                output.SetPixel(x, y, Color.FromArgb(background));
            }
        }

        Random rnd = new Random(seed);
        
        Grid grid = new Grid(gridWidth, gridHeight, seed);

        while (!grid.IsFull()) {
            var rect = grid.GetRandomRectangle();
            Color c = Color.FromArgb(palette.Colors[rnd.Next(0, palette.Colors.Length - 1)] + (0xff << 24));

            for (int x = rect.a.x * (pixelsPerUnit + gapThickness); x < rect.b.x * (pixelsPerUnit + gapThickness) + pixelsPerUnit; x++) {
                for (int y = rect.a.y * (pixelsPerUnit + gapThickness); y < rect.b.y * (pixelsPerUnit + gapThickness) + pixelsPerUnit; y++) {
                    output.SetPixel(x, y, c);
                }
            }
        }
        
        return output;
    }

    private static readonly SettingsBuilder<Rectangles> SETTINGS = SettingsBuilder<Rectangles>.Build("rectangles",
        SettingsNode<Rectangles>.New("rectangles")
            .Group(IImageGenerator.UniversalSeedInput())
            .Argument("grid_width", Arguments.Integer(1))
            .Argument("grid_height", Arguments.Integer(1))
            .Argument("pixels_per_unit", Arguments.Integer(1))
            .Argument("gap_thickness", Arguments.Integer(1))
            .OnParse(cin => new Rectangles(
                (int)cin["grid_width"].GetParsedValue(), 
                (int)cin["grid_height"].GetParsedValue(), 
                (int)cin["pixels_per_unit"].GetParsedValue(), 
                (int)cin["gap_thickness"].GetParsedValue(), 
                (int)cin["seed"].GetParsedValue())
            )
    );
    
    public static SettingsBuilder<T> GetSettings<T>() where T : IImageGenerator {
        if (typeof(T) != typeof(Rectangles)) {
            throw new SettingsBuilderException("Invalid type inputted.");
        }
        
        return SETTINGS.Clone() as SettingsBuilder<T> ?? SettingsBuilder<T>.Empty<T>("rectangles");
    }

    /// <summary>
    /// helper struct
    /// </summary>
    private readonly struct Grid {
        private readonly bool[,] grid;
        
        private readonly int width;
        private readonly int height;
        private readonly int seed;

        private const int END_CHANCE = 8;

        public Grid(int width, int height, int seed) {
            this.width = width;
            this.height = height;
            this.seed = seed;
            grid = new bool[width, height];
        }
        
        public ((int x, int y) a, (int x, int y) b) GetRandomRectangle() {
            ((int x, int y) a, (int x, int y) b) p = ((0, 0), (0, 0));
            p.a = GetRandomFirst();
            p.b = GetRandomSecond(p.a.x, p.a.y);

            (p.a.x, p.b.x) = p.a.x <= p.b.x ? (p.a.x, p.b.x) : (p.b.x, p.a.x);
            (p.a.y, p.b.y) = p.a.y <= p.b.y ? (p.a.y, p.b.y) : (p.b.y, p.a.y);
            
            for (int x = p.a.x; x <= p.b.x; x++) {
                for (int y = p.a.y; y <= p.b.y; y++) {
                    grid[x, y] = true;
                }
            }
            
            return p;
        }

        public bool IsFull() {
            return grid.Cast<bool>().All(b => b);
        }

        private (int x, int y) GetRandomFirst() {
            Random rnd = new Random(seed);
            
            List<(int x, int y)> points = new List<(int x, int y)>();
            
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (grid[x, y] == false) {
                        points.Add((x, y)); 
                    }
                }
            }
            
            var position = points[rnd.Next(0, points.Count - 1)];
            
            return position;
        }

        private (int x, int y) GetRandomSecond(int startX, int startY) {
            Random rnd = new Random(seed + 3);

            return rnd.Next(1, 4) switch {
                1 => GetSecond1(startX, startY),
                2 => GetSecond2(startX, startY),
                3 => GetSecond3(startX, startY),
                4 => GetSecond4(startX, startY),
                _ => GetSecond1(startX, startY)
            };
        }

        private (int x, int y) GetSecond1(int startX, int startY) {
            int maxX = startX;
            Random rnd = new Random(seed + 2);

            while (true) {
                if (grid[maxX, startY] == true) {
                    maxX--;
                    break;
                }

                if (maxX == width - 1 || rnd.Next(0, END_CHANCE) == 0) {
                    break;
                }
                
                maxX++;
            }

            for (int y = startY; y < height; y++) {
                for (int x = startX; x <= maxX; x++) {
                    if (grid[x, y] == true) {
                        return (x, y - 1);
                    }
                }

                if (rnd.Next(0, END_CHANCE) == 0) {
                    return (maxX, y);
                }
            }
            
            return (maxX, height - 1);
        }
        
        private (int x, int y) GetSecond2(int startX, int startY) {
            int maxX = startX;
            Random rnd = new Random(seed + 2);

            while (true) {
                if (grid[maxX, startY] == true) {
                    maxX++;
                    break;
                }

                if (maxX == 0 || rnd.Next(0, END_CHANCE) == 0) {
                    break;
                }
                
                maxX--;
            }

            for (int y = startY; y < height; y++) {
                for (int x = startX; x >= maxX; x--) {
                    if (grid[x, y] == true) {
                        return (x, y - 1);
                    }
                }

                if (rnd.Next(0, END_CHANCE) == 0) {
                    return (maxX, y);
                }
            }
            
            return (maxX, height - 1);
        }
        
        private (int x, int y) GetSecond3(int startX, int startY) {
            int maxX = startX;
            Random rnd = new Random(seed + 2);

            while (true) {
                if (grid[maxX, startY] == true) {
                    maxX--;
                    break;
                }

                if (maxX == width - 1 || rnd.Next(0, END_CHANCE) == 0) {
                    break;
                }
                
                maxX++;
            }

            for (int y = startY; y >= 0; y--) {
                for (int x = startX; x <= maxX; x++) {
                    if (grid[x, y] == true) {
                        return (x, y + 1);
                    }
                }

                if (rnd.Next(0, END_CHANCE) == 0) {
                    return (maxX, y);
                }
            }
            
            return (maxX, 0);
        }
        
        private (int x, int y) GetSecond4(int startX, int startY) {
            int maxX = startX;
            Random rnd = new Random(seed + 2);

            while (true) {
                if (grid[maxX, startY] == true) {
                    maxX++;
                    break;
                }

                if (maxX == 0 || rnd.Next(0, END_CHANCE) == 0) {
                    break;
                }
                
                maxX--;
            }

            for (int y = startY; y >= height; y--) {
                for (int x = startX; x >= maxX; x--) {
                    if (grid[x, y] == true) {
                        return (x, y + 1);
                    }
                }

                if (rnd.Next(0, END_CHANCE) == 0) {
                    return (maxX, y);
                }
            }
            
            return (maxX, 0);
        }
    }
}