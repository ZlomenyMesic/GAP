//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;
using GAP.util.exceptions;
using GAP.util.settings;
using Kolors;

namespace GAP.core.image.generation.generators;

/// <summary>
/// White Noise Image Generation <br/>
/// generates purely random set of pixels with color properties set manually or with the <see cref="WhiteNoisePresets"/>
/// </summary>
public sealed class WhiteNoise : ImageGenerator {
    public int width { get; set; }
    public int height { get; set; }
    public int seed { get; set; }

    public double hueFactor { get; set; } = 1d;
    public bool allowRandomHue { get; set; }
    public double saturationFactor { get; set; } = 1d;
    public bool allowRandomSaturation { get; set; }
    public double valueFactor { get; set; } = 1d;
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
        
    }

    /// <summary>
    /// manual constructor
    /// </summary>
    /// <param name="width">width of the generated image</param>
    /// <param name="height">height of the generated image</param>
    /// <param name="seed">seed used by the <see cref="Random"/></param>
    /// <param name="hueFactor">value 0 - 1, defines fullness of the hue part of a color</param>
    /// <param name="saturationFactor">value 0 - 1, defines fullness of the saturation part of a color</param>
    /// <param name="valueFactor">value 0 - 1, defines fullness of the value part of a color</param>
    /// <param name="allowRandomHue">whether random values are used to generate the hue part of a color</param>
    /// <param name="allowRandomSaturation">
    /// whether random values are used to generate the saturation part of a color
    /// </param>
    /// <param name="allowRandomBrightness">
    /// whether random values are used to generate the value part of a color
    /// </param>
    public WhiteNoise(int width, int height, int seed, double hueFactor, double saturationFactor,
        double valueFactor, bool allowRandomHue = true, 
        bool allowRandomSaturation = true, bool allowRandomBrightness = true) {
        
        this.width = width;
        this.height = height;
        this.seed = seed;
        this.hueFactor = hueFactor;
        this.saturationFactor = saturationFactor;
        this.valueFactor = valueFactor;
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
        valueFactor = whiteNoise.valueFactor;
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
                    allowRandomBrightness ? valueFactor * random.NextDouble(): valueFactor));
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

    private static (double hf, bool hr, double sf, bool sr, double vf, bool vr) FromPreset(WhiteNoisePresets preset) {

        (double hf, bool hr, double sf, bool sr, double vf, bool vr) s = (1, true, 1, false, 1, false);

        switch (preset) {
            case WhiteNoisePresets.GRAYSCALE:
                s.sr = false;
                s.sf = 0d;
                s.vr = true;
                break;
            case WhiteNoisePresets.RANDOM_HUE:
                s.sr = false;
                s.vr = false;
                break;
            case WhiteNoisePresets.RANDOM_HUE_AND_DARKNESS:
                s.sr = false;
                s.vr = true;
                break;
            case WhiteNoisePresets.FULL_RANDOM:
                s.sr = true;
                s.vr = true;
                break;
            default:
                s.sr = true;
                s.vr = true;
                break;
        }
        
        return s;
    }
    
    private static readonly SettingsBuilder<WhiteNoise> SETTINGS = SettingsBuilder<WhiteNoise>.Build("white_noise", 
        SettingsNode<WhiteNoise>.New("advanced")
            .Argument("seed", Arguments.Integer())
            .Argument("width", Arguments.Integer(0))
            .Argument("height", Arguments.Integer(0))
            .Group(SettingsGroup
                .New("advanced", Context.New(
                    ("allow_random_hue", Arguments.Bool()),
                    ("hue_factor", Arguments.Double(0, 1)),
                    ("allow_random_saturation", Arguments.Bool()),
                    ("saturation_factor", Arguments.Double(0, 1)),
                    ("allow_random_value", Arguments.Bool()),
                    ("value_factor", Arguments.Double(0, 1))))
                .Option(SettingsGroupOption
                    .New("presets")
                    .Argument("presets", Arguments.SingleSelect<WhiteNoisePresets>())
                    .OnParse((cin, cout) => {
                        (double hf, bool hr, double sf, bool sr, double vf, bool vr) s =
                            FromPreset((WhiteNoisePresets)cin["presets"].GetParsedValue());

                        cout["allow_random_hue"].SetParsedValue(s.hr);
                        cout["hue_factor"].SetParsedValue(s.hf);
                        cout["allow_random_saturation"].SetParsedValue(s.sr);
                        cout["saturation_factor"].SetParsedValue(s.sf);
                        cout["allow_random_value"].SetParsedValue(s.vr);
                        cout["value_factor"].SetParsedValue(s.vf);
                    })                    
                )
                .Option(SettingsGroupOption
                    .New("manual")
                    .Argument("allow_random_hue", Arguments.Bool())
                    .Argument("hue_factor", Arguments.Double(0, 1))
                    .Argument("allow_random_saturation", Arguments.Bool())
                    .Argument("saturation_factor", Arguments.Double(0, 1))
                    .Argument("allow_random_value", Arguments.Bool())
                    .Argument("value_factor", Arguments.Double(0, 1))
                )
                .OnParse((cin, cout) => {
                    cout["allow_random_hue"].SetParsedValue(cin["allow_random_hue"].GetParsedValue());
                    cout["allow_random_saturation"].SetParsedValue(cin["allow_random_saturation"].GetParsedValue());
                    cout["allow_random_value"].SetParsedValue(cin["allow_random_value"].GetParsedValue());
                    cout["hue_factor"].SetParsedValue(cin["hue_factor"].GetParsedValue());
                    cout["saturation_factor"].SetParsedValue(cin["saturation_factor"].GetParsedValue());
                    cout["value_factor"].SetParsedValue(cin["value_factor"].GetParsedValue());
                })
            )
            .OnParse(context => {
                WhiteNoise whiteNoise = new WhiteNoise(
                    (int)context["width"].GetParsedValue(),
                    (int)context["height"].GetParsedValue(),
                    (int)context["seed"].GetParsedValue(),
                    (double)context["hue_factor"].GetParsedValue(),
                    (double)context["saturation_factor"].GetParsedValue(),
                    (double)context["value_factor"].GetParsedValue(),
                    (bool)context["allow_random_hue"].GetParsedValue(),
                    (bool)context["allow_random_saturation"].GetParsedValue(),
                    (bool)context["allow_random_value"].GetParsedValue());
                    
                return whiteNoise;
            })
        ,
        
        SettingsNode<WhiteNoise>.New("basic")
            .Argument("seed", Arguments.Integer())
            .Argument("width", Arguments.Integer())
            .Argument("height", Arguments.Integer())
            .OnParse(context => new WhiteNoise(
                (int)context["width"].GetParsedValue(),
                (int)context["height"].GetParsedValue(),
                (int)context["seed"].GetParsedValue()))
    );

    public override SettingsBuilder<T> GetSettings<T>() {
        if (typeof(T) != typeof(WhiteNoise)) {
            throw new SettingsBuilderException("Invalid type inputted.");
        }
        
        return SETTINGS.Clone() as SettingsBuilder<T> ?? SettingsBuilder<T>.Empty<T>("white_noise");
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