//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;
using Kolors;

namespace GAP.core.image.generation.generators;

public sealed class WhiteNoise : ImageGenerator {
    public override int width { get; protected set; }
    public override int height { get; protected set; }
    public override int seed { get; protected set; }

    public double hueFactor { get; private set; } = 1d;
    public bool allowHueRandom { get; private set; }
    public double saturationFactor { get; private set; } = 1d;
    public bool allowSaturationRandom { get; private set; }
    public double brightnessFactor { get; private set; } = 1d;
    public bool allowBrightnessRandom { get; private set; }
    
    public WhiteNoise(int width, int height, int seed) {
        this.width = width;
        this.height = height;
        this.seed = seed;
    }

    public WhiteNoise(int width, int height, int seed, WhiteNoisePresets presets) {
        this.width = width;
        this.height = height;
        this.seed = seed;
        switch (presets) {
            case WhiteNoisePresets.GRAYSCALE:
                allowHueRandom = true;
                allowSaturationRandom = false;
                saturationFactor = 0d;
                allowBrightnessRandom = true;
                break;
            case WhiteNoisePresets.RANDOM_HUE:
                allowHueRandom = true;
                allowSaturationRandom = false;
                allowBrightnessRandom = false;
                break;
            case WhiteNoisePresets.RANDOM_HUE_DARKNESS:
                allowHueRandom = true;
                allowSaturationRandom = false;
                allowBrightnessRandom = true;
                break;
            case WhiteNoisePresets.FULL_RANDOM:
                allowHueRandom = true;
                allowSaturationRandom = true;
                allowBrightnessRandom = true;
                break;
            default:
                allowHueRandom = true;
                allowSaturationRandom = true;
                allowBrightnessRandom = true;
                break;
        }
    }

    public WhiteNoise(int width, int height, int seed, double hueFactor, double saturationFactor,
        double brightnessFactor) {
        
        this.width = width;
        this.height = height;
        this.seed = seed;
        this.hueFactor = hueFactor;
        this.saturationFactor = saturationFactor;
        this.brightnessFactor = brightnessFactor;
    }

    private void Copy(WhiteNoise whiteNoise) {
        width = whiteNoise.width;
        height = whiteNoise.height;
        seed = whiteNoise.seed;
        allowHueRandom = whiteNoise.allowHueRandom;
        hueFactor = whiteNoise.hueFactor;
        saturationFactor = whiteNoise.saturationFactor;
        allowSaturationRandom = whiteNoise.allowSaturationRandom;
        brightnessFactor = whiteNoise.brightnessFactor;
        allowBrightnessRandom = whiteNoise.allowBrightnessRandom;
    }

    public override Bitmap GenerateImage() {
        Random random = new Random(seed);
        Bitmap image = new Bitmap(width, height);
        
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                image.SetPixel(x, y, ColorFormat.ColorFromHsv(
                    allowHueRandom ? hueFactor * random.NextDouble() * 360: hueFactor * 360, 
                    allowSaturationRandom ? saturationFactor * random.NextDouble() : saturationFactor, 
                    allowBrightnessRandom ? brightnessFactor * random.NextDouble(): brightnessFactor));
            }
        }
        
        return image;
    }

    public override void LoadFromJson(string settings) {
        WhiteNoise? wiener = JsonSerializer.Deserialize<WhiteNoise>(settings) ?? null;
        
        if (wiener == null) {
            throw new JsonException("Deserialization of settings of WhiteNoise failed");
        }
        
        Copy(wiener);
    }

    public override string ToString() {
        return JsonSerializer.Serialize(this);
    }
}

public enum WhiteNoisePresets {
    FULL_RANDOM,
    RANDOM_HUE,
    RANDOM_HUE_DARKNESS,
    GRAYSCALE
}