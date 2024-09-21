//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;
using Kolors;

namespace GAP.core.image.transformation.transformers;

public class Pixelize : ImageTransformer {
    public override int seed { get; set; }
    public PixelType pixelType { get; set; }
    
    
    public Pixelize() { }
    
    public Pixelize(int seed, PixelType pixelType) {
        this.seed = seed;
        this.pixelType = pixelType;
    }
    
    public override Bitmap TransformImage(Bitmap image) {
        if (pixelType == PixelType.STRIPES) {
            return PixelizeStripes(image);
        }
        else {
            return PixelizeSquares(image);
        }
    }

    private static Bitmap PixelizeStripes(Bitmap image) {
        Bitmap result = new Bitmap(image.Width * 3, image.Height * 3);
            
        for (int x = 0; x < image.Width; x++) {
            for (int y = 0; y < image.Height; y++) {
                result.SetPixel(x * 3, y * 3, Color.FromArgb(255, image.GetPixel(x, y).R, 0, 0));
                result.SetPixel(x * 3, y * 3 + 1, Color.FromArgb(255, image.GetPixel(x, y).R, 0, 0));
                result.SetPixel(x * 3, y * 3 + 2, Color.FromArgb(255, image.GetPixel(x, y).R, 0, 0));
                result.SetPixel(x * 3 + 1, y * 3, Color.FromArgb(255, 0, image.GetPixel(x, y).G, 0));
                result.SetPixel(x * 3 + 1, y * 3 + 1, Color.FromArgb(255, 0, image.GetPixel(x, y).G, 0));
                result.SetPixel(x * 3 + 1, y * 3 + 2, Color.FromArgb(255, 0, image.GetPixel(x, y).G, 0));
                result.SetPixel(x * 3 + 2, y * 3, Color.FromArgb(255, 0, 0, image.GetPixel(x, y).B));
                result.SetPixel(x * 3 + 2, y * 3 + 1, Color.FromArgb(255, 0, 0, image.GetPixel(x, y).B));
                result.SetPixel(x * 3 + 2, y * 3 + 2, Color.FromArgb(255, 0, 0, image.GetPixel(x, y).B));
            }
        }
            
        return result;
    }

    private static Bitmap PixelizeSquares(Bitmap image) {
        Bitmap result = new Bitmap(image.Width * 2, image.Height * 2);
        
        for (int x = 0; x < image.Width; x++) {
            for (int y = 0; y < image.Height; y++) {
                result.SetPixel(x * 2, y * 2, Color.FromArgb(255, image.GetPixel(x, y).R, 0, 0));
                result.SetPixel(x * 2, y * 2 + 1, Color.FromArgb(255, 0, image.GetPixel(x, y).G * 1/2, 0));
                result.SetPixel(x * 2 + 1, y * 2, Color.FromArgb(255, 0, image.GetPixel(x, y).G * 1/2, 0));
                result.SetPixel(x * 2 + 1, y * 2 + 1, Color.FromArgb(255, 0, 0, image.GetPixel(x, y).B));
            }
        }
        
        return result;
    }
    private void Copy(Pixelize pixelize) {
        seed = pixelize.seed;
        pixelType = pixelize.pixelType;
    }

    public override void LoadFromJson(string settings) {
        Pixelize? pixelize = JsonSerializer.Deserialize<Pixelize>(settings) ?? null;
        
        if (pixelize == null) {
            throw new JsonException("Deserialization of settings of Grid failed");
        }
        
        Copy(pixelize);
    }

    public override string ToString() => JsonSerializer.Serialize(this);
}

public enum PixelType {
    STRIPES,
    SQUARES
}