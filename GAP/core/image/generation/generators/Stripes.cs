//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using GAP.util.exceptions;
using GAP.util.settings;
using Kolors;

namespace GAP.core.image.generation.generators;

public class Stripes : IImageGenerator {

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
    
    public Stripes(int gridWidth, int gridHeight, int pixelsPerUnit, int gapThickness, int seed) {
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.pixelsPerUnit = pixelsPerUnit;
        this.gapThickness = gapThickness;
        this.seed = seed;
        
        width = gridWidth * pixelsPerUnit + (gridWidth - 1) * gapThickness;
        height = gridHeight * pixelsPerUnit + (gridHeight - 1) * gapThickness;
    }
    
    public Stripes() { }
    
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
        ConsoleColors.PrintlnColoredB("   ", background);
        palette.PrintPalette();
        
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
            Console.Write($"[{rect.a.x}; {rect.a.y}] [{rect.b.x}, {rect.b.y}]");
            ConsoleColors.PrintlnColoredB("   ", c.ToArgb());

            for (int x = rect.a.x * (pixelsPerUnit + gapThickness); x < rect.b.x * (pixelsPerUnit + gapThickness) + pixelsPerUnit; x++) {
                for (int y = rect.a.y * (pixelsPerUnit + gapThickness); y < rect.b.y * (pixelsPerUnit + gapThickness) + pixelsPerUnit; y++) {
                    output.SetPixel(x, y, c);
                }
            }
        }
        
        return output;
    }

    private static readonly SettingsBuilder<Stripes> SETTINGS = SettingsBuilder<Stripes>.Build("rectangles",
        SettingsNode<Stripes>.New("stripes")
            .Group(IImageGenerator.UniversalSeedInput())
            .Argument("grid_width", Arguments.Integer(2))
            .Argument("grid_height", Arguments.Integer(2))
            .Argument("pixels_per_unit", Arguments.Integer(1))
            .Argument("gap_thickness", Arguments.Integer(1))
            .OnParse(cin => new Stripes(
                (int)cin["grid_width"].GetParsedValue(), 
                (int)cin["grid_height"].GetParsedValue(), 
                (int)cin["pixels_per_unit"].GetParsedValue(), 
                (int)cin["gap_thickness"].GetParsedValue(),
                (int)cin["seed"].GetParsedValue())
            )
    );
    
    public static object GetSettings() {
        return SETTINGS.Clone();
    }

    /// <summary>
    /// helper struct
    /// </summary>
    private readonly struct Grid {
        private readonly bool[,] grid;
        
        private readonly int width;
        private readonly int height;
        private readonly int seed;

        public Grid(int width, int height, int seed) {
            this.width = width;
            this.height = height;
            this.seed = seed;
            grid = new bool[width, height];
        }
        
        public ((int x, int y) a, (int x, int y) b) GetRandomRectangle() {
            ((int x, int y) a, (int x, int y) b) positions = ((0, 0), (0, 0));
            positions.a = GetRandomFirst();
            positions.b = GetRandomSecond(positions.a.x, positions.a.y);

            for (int x = positions.a.x; x <= positions.b.x; x++) {
                for (int y = positions.a.y; y <= positions.b.y; y++) {
                    grid[x, y] = true;
                }
            }
            
            return positions;
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
            int maxX = startX;
            Random rnd = new Random(seed + 2);
            
            while (true) {
                if (grid[maxX, startY]) {
                    maxX--;
                    break;
                }
                
                if (maxX == width - 1 || rnd.Next(0, 5) == 0) break;
                
                maxX++;
            }

            
            for (int y = startY; y < height; y++) {
                for (int x = startX; x < maxX; x++) {
                    if (grid[x, y]) {
                        return (x, y - 1);
                    }

                    if (rnd.Next(0, 5) == 0) {
                        return (x, y);
                    }
                }
            }
            
            return (maxX, height - 1);
        } 
        
        
    }
}