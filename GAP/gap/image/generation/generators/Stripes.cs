//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using NeoKolors;
using NeoKolors.Common;
using NeoKolors.Console;
using NeoKolors.Settings;
using Color = System.Drawing.Color;

namespace GAP.core.image.generation.generators;

public class Stripes : IBatchableGenerator<Stripes> {

    public int Width { get; private set; } = 128;
    public int Height { get; private set; } = 128;
    public int Seed { get; private set; }

    public int GridWidth { get; private set; } = 8;
    public int GridHeight { get; private set; } = 8;
    public int PixelsPerUnit { get; private set; } = 8;
    public int GapThickness { get; private set; } = 4;
    
    public Stripes(int gridWidth, int gridHeight, int pixelsPerUnit, int gapThickness, int seed) {
        GridWidth = gridWidth;
        GridHeight = gridHeight;
        PixelsPerUnit = pixelsPerUnit;
        GapThickness = gapThickness;
        Seed = seed;
        
        Width = gridWidth * pixelsPerUnit + (gridWidth - 1) * gapThickness;
        Height = gridHeight * pixelsPerUnit + (gridHeight - 1) * gapThickness;
    }
    
    public Stripes() { }
    
    public Bitmap GenerateImage() {
        
        // create a palette
        ColorPalette palette = ColorPalette.GeneratePalette(Seed);

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
        
        Bitmap output = new Bitmap(Width, Height);

        // fill it with the 
        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                output.SetPixel(x, y, Color.FromArgb(background));
            }
        }

        Random rnd = new Random(Seed);
        
        Grid grid = new Grid(GridWidth, GridHeight, Seed);

        while (!grid.IsFull()) {
            var rect = grid.GetRandomRectangle();
            Color c = Color.FromArgb(palette.Colors[rnd.Next(0, palette.Colors.Length - 1)] + (0xff << 24));

            for (int x = rect.a.x * (PixelsPerUnit + GapThickness); x < rect.b.x * (PixelsPerUnit + GapThickness) + PixelsPerUnit; x++) {
                for (int y = rect.a.y * (PixelsPerUnit + GapThickness); y < rect.b.y * (PixelsPerUnit + GapThickness) + PixelsPerUnit; y++) {
                    output.SetPixel(x, y, c);
                }
            }
        }
        
        return output;
    }

    ISettingsBuilder<IImageGenerator> IImageGenerator.GetSettings() => GetSettings();

    IImageGenerator IBatchableGenerator.GetNextGenerator(int i) => GetNextGenerator(i);

    private static readonly SettingsBuilder<Stripes> SETTINGS = SettingsBuilder<Stripes>.Build("rectangles",
        SettingsNode<Stripes>.New("stripes")
            .Group(IImageGenerator.UniversalSeedInput())
            .Argument("grid_width", Arguments.Integer(2))
            .Argument("grid_height", Arguments.Integer(2))
            .Argument("pixels_per_unit", Arguments.Integer(1))
            .Argument("gap_thickness", Arguments.Integer(1))
            .Constructs(cin => new Stripes(
                (int)cin["grid_width"].Get(), 
                (int)cin["grid_height"].Get(), 
                (int)cin["pixels_per_unit"].Get(), 
                (int)cin["gap_thickness"].Get(),
                (int)cin["seed"].Get())
            )
    );
    
    public SettingsBuilder<Stripes> GetSettings() {
        return (SettingsBuilder<Stripes>)SETTINGS.Clone();
    }

    public Stripes GetNextGenerator(int i) {
        Stripes copy = (Stripes)MemberwiseClone();
        copy.Seed = i;
        return copy;
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