//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;
using Kolors;

namespace GAP.core.image.generation.generators;

/// <summary>
/// White Noise Image Generation <br/>
/// generates purely random set of pixels with color properties set manually or with the <see cref="WhiteNoisePresets"/>
/// </summary>
public class WhiteNoise : ImageGenerator {
    public override int width { get; set; }
    public override int height { get; set; }
    public override int seed { get; set; }

    public double hueFactor { get; set; } = 1d;
    public bool allowRandomHue { get; set; }
    public double saturationFactor { get; set; } = 1d;
    public bool allowRandomSaturation { get; set; }
    public double brightnessFactor { get; set; } = 1d;
    public bool allowRandomBrightness { get; set; }
    
    /// <summary>
    /// default constructor
    /// </summary>
    /// <param name="width">width of the generated image</param>
    /// <param name="height">height of the generated image</param>
    /// <param name="seed">seed used by the <see cref="Random"/></param>
    public WhiteNoise(int width, int height, int seed) {
        this.width = width;
        this.height = height;
        this.seed = seed;
    }
    
    /// <summary>
    /// empty constructor
    /// </summary>
    public WhiteNoise() { }

    /// <summary>
    /// constructor using presets
    /// </summary>
    /// <param name="width">width of the generated image</param>
    /// <param name="height">height of the generated image</param>
    /// <param name="seed">seed used by the <see cref="Random"/></param>
    /// <param name="presets">preset used to set color generation parameters</param>
    public WhiteNoise(int width, int height, int seed, WhiteNoisePresets presets) {
        this.width = width;
        this.height = height;
        this.seed = seed;
        switch (presets) {
            case WhiteNoisePresets.GRAYSCALE:
                allowRandomHue = true;
                allowRandomSaturation = false;
                saturationFactor = 0d;
                allowRandomBrightness = true;
                break;
            case WhiteNoisePresets.RANDOM_HUE:
                allowRandomHue = true;
                allowRandomSaturation = false;
                allowRandomBrightness = false;
                break;
            case WhiteNoisePresets.RANDOM_HUE_AND_DARKNESS:
                allowRandomHue = true;
                allowRandomSaturation = false;
                allowRandomBrightness = true;
                break;
            case WhiteNoisePresets.FULL_RANDOM:
                allowRandomHue = true;
                allowRandomSaturation = true;
                allowRandomBrightness = true;
                break;
            default:
                allowRandomHue = true;
                allowRandomSaturation = true;
                allowRandomBrightness = true;
                break;
        }
    }

    /// <summary>
    /// manual constructor
    /// </summary>
    /// <param name="width">width of the generated image</param>
    /// <param name="height">height of the generated image</param>
    /// <param name="seed">seed used by the <see cref="Random"/></param>
    /// <param name="hueFactor">value 0 - 1, defines fullness of the hue part of a color</param>
    /// <param name="saturationFactor">value 0 - 1, defines fullness of the saturation part of a color</param>
    /// <param name="brightnessFactor">value 0 - 1, defines fullness of the brightness part of a color</param>
    /// <param name="allowRandomHue">whether random values are used to generate the hue part of a color</param>
    /// <param name="allowRandomSaturation">
    /// whether random values are used to generate the saturation part of a color
    /// </param>
    /// <param name="allowRandomBrightness">
    /// whether random values are used to generate the brightness part of a color
    /// </param>
    public WhiteNoise(int width, int height, int seed, double hueFactor, double saturationFactor,
        double brightnessFactor, bool allowRandomHue = true, 
        bool allowRandomSaturation = true, bool allowRandomBrightness = true) {
        
        this.width = width;
        this.height = height;
        this.seed = seed;
        this.hueFactor = hueFactor;
        this.saturationFactor = saturationFactor;
        this.brightnessFactor = brightnessFactor;
        this.allowRandomHue = allowRandomHue;
        this.allowRandomSaturation = allowRandomSaturation;
        this.allowRandomBrightness = allowRandomBrightness;
    }

    private void Copy(WhiteNoise whiteNoise) {
        width = whiteNoise.width;
        height = whiteNoise.height;
        seed = whiteNoise.seed;
        allowRandomHue = whiteNoise.allowRandomHue;
        hueFactor = whiteNoise.hueFactor;
        saturationFactor = whiteNoise.saturationFactor;
        allowRandomSaturation = whiteNoise.allowRandomSaturation;
        brightnessFactor = whiteNoise.brightnessFactor;
        allowRandomBrightness = whiteNoise.allowRandomBrightness;
    }

    /// <summary>
    /// generates the output image
    /// </summary>
    /// <returns><see cref="Bitmap"/> object with the final image</returns>
    public override Bitmap GenerateImage() {
        Random random = new Random(seed);
        Bitmap image = new Bitmap(width, height);
        
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                image.SetPixel(x, y, ColorFormat.ColorFromHsv(
                    allowRandomHue ? hueFactor * random.NextDouble() * 360: hueFactor * 360, 
                    allowRandomSaturation ? saturationFactor * random.NextDouble() : saturationFactor, 
                    allowRandomBrightness ? brightnessFactor * random.NextDouble(): brightnessFactor));
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

/// <summary>
/// White Noise Image Generation Presets <br/>
/// preset settings used in the <see cref="WhiteNoise"/> generator
/// </summary>
public enum WhiteNoisePresets {
    FULL_RANDOM,
    RANDOM_HUE,
    RANDOM_HUE_AND_DARKNESS,
    GRAYSCALE
}