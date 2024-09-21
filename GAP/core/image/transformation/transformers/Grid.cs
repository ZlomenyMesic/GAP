//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;
using GAP.util.math;

namespace GAP.core.image.transformation.transformers;

public class Grid : ImageTransformer {
    public sealed override int seed { get; set; }
    public uint scaleFactor { get; set; }
    public InterpolationType interpolationType { get; set; }
    

    public Grid(int seed, uint scaleFactor, InterpolationType interpolationType = InterpolationType.LINEAR) {

        if (scaleFactor == 0) throw new ArgumentException("Scale factor for Grid transform must be greater than zero");

        this.seed = seed;
        this.scaleFactor = scaleFactor;
        this.interpolationType = interpolationType;
    }
    
    public override Bitmap TransformImage(Bitmap image) {

        if (scaleFactor == 1) return image;

        int width = (int)(image.Width * scaleFactor);
        int height = (int)(image.Height * scaleFactor);
        Bitmap result = new Bitmap(width, height);

        for (int x = 0; x < image.Width; x++) {
            for (int y = 0; y < image.Height; y++) {
                result.SetPixel((int)(x * scaleFactor), (int)(y * scaleFactor), image.GetPixel(x, y));
            }
        }
            
        return result;
    }

    private void Copy(Grid grid) {
        seed = grid.seed;
        scaleFactor = grid.scaleFactor;
        interpolationType = grid.interpolationType;
    }

    public override void LoadFromJson(string settings) {
        Grid? grid = JsonSerializer.Deserialize<Grid>(settings) ?? null;
        
        if (grid == null) {
            throw new JsonException("Deserialization of settings of Grid failed");
        }
        
        Copy(grid);
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}