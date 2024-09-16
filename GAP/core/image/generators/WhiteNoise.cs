//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using GAP.core.input;
using GAP.util;
using Kolors;

namespace GAP.core.image.generators;

public class WhiteNoise : ImageGenerator {
    public override int width { get; }
    public override int height { get; }
    public override int seed { get; }
    public bool grayscale { get; private set; }
    public ColorPalette palette { get; private set; }

    // private static SettingsBuilder SETTING = new SettingsBuilder();
    
    public WhiteNoise(int width, int height, int seed/*, Settings settings*/) {
        this.width = width;
        this.height = height;
        this.seed = seed;
    }

    // public override SettingsBuilder GetSettings() {
    //     return SETTING;
    // }

    public override ImageGenerator GetInstance(int width, int height, int seed/*, Settings settings*/) {
        return new WhiteNoise(width, height, seed/*, settings*/);
    }

    // Todo finish fuckin generation!!!
    public override Bitmap GenerateImage() {
        Random random = new Random(seed);
        Bitmap image = new Bitmap(width, height);
        
        if (grayscale) {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    image.SetPixel(x, y, new Color());
                }
            }
        }
        else {
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    image.SetPixel(x, y, Color.White);
                }
            }
        }
        
        return image;
    }
}