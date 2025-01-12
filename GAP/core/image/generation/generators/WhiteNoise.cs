//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;
using NeoKolors;
using NeoKolors.Common;
using NeoKolors.Settings;

namespace GAP.core.image.generation.generators;

/// <summary>
/// White Noise Image Generation <br/>
/// generates purely random set of pixels with color properties set manually or with the <see cref="WhiteNoisePresets"/>
/// </summary>
public sealed class WhiteNoise : ICloneable, IBatchableGenerator {
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Seed { get; private set; }
    

    public double HueFactor { get; private set; } = 1d;
    public bool AllowRandomHue { get; private set; }
    public double SaturationFactor { get; private set; } = 1d;
    public bool AllowRandomSaturation { get; private set; }
    public double ValueFactor { get; private set; } = 1d;
    public bool AllowRandomValue { get; private set; }
    
    /// <summary>
    /// default constructor
    /// </summary>
    /// <param name="width">width of the generated image</param>
    /// <param name="height">height of the generated image</param>
    /// <param name="seed">seed used by the <see cref="Random"/></param>
    public WhiteNoise(int width, int height, int seed) {
        Width = width;
        Height = height;
        Seed = seed;
    }
    
    public WhiteNoise() {}

    /// <summary>
    /// constructor using presets
    /// </summary>
    /// <param name="width">width of the generated image</param>
    /// <param name="height">height of the generated image</param>
    /// <param name="seed">seed used by the <see cref="Random"/></param>
    /// <param name="presets">preset used to set color generation parameters</param>
    public WhiteNoise(int width, int height, int seed, WhiteNoisePresets presets) {
        Width = width;
        Height = height;
        Seed = seed;
        (HueFactor, AllowRandomHue,
            SaturationFactor, AllowRandomSaturation,
            ValueFactor, AllowRandomValue) = FromPreset(presets); 
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
    /// <param name="allowRandomValue">
    /// whether random values are used to generate the value part of a color
    /// </param>
    public WhiteNoise(int width, int height, int seed, double hueFactor, double saturationFactor,
        double valueFactor, bool allowRandomHue = true, 
        bool allowRandomSaturation = true, bool allowRandomValue = true) {
        
        Width = width;
        Height = height;
        Seed = seed;
        HueFactor = hueFactor;
        SaturationFactor = saturationFactor;
        ValueFactor = valueFactor;
        AllowRandomHue = allowRandomHue;
        AllowRandomSaturation = allowRandomSaturation;
        AllowRandomValue = allowRandomValue;
    }

    /// <summary>
    /// generates the output image
    /// </summary>
    /// <returns><see cref="Bitmap"/> object with the final image</returns>
    public Bitmap GenerateImage() {
        Random random = new Random(Seed);
        Bitmap image = new Bitmap(Width, Height);
        
        for (int x = 0; x < Width; x++) {
            for (int y = 0; y < Height; y++) {
                image.SetPixel(x, y, ColorFormat.HsvToColor(
                    AllowRandomHue ? HueFactor * random.NextDouble() * 360: HueFactor * 360, 
                    AllowRandomSaturation ? SaturationFactor * random.NextDouble() : SaturationFactor, 
                    AllowRandomValue ? ValueFactor * random.NextDouble(): ValueFactor));
            }
        }
        
        return image;
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
    
    private static readonly ISettingsBuilder<WhiteNoise> SETTINGS = SettingsBuilder<WhiteNoise>.Build("white_noise", 
        SettingsNode<WhiteNoise>.New("advanced")
            .Group(IImageGenerator.UniversalSeedInput())
            .Argument("width", Arguments.Integer(128))
            .Argument("height", Arguments.Integer(128))
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
                    .Argument("presets", Arguments.SingleSelect(WhiteNoisePresets.RANDOM_HUE_AND_DARKNESS))
                    .Merges((cin, cout) => {
                        (double hf, bool hr, double sf, bool sr, double vf, bool vr) s =
                            FromPreset((WhiteNoisePresets)cin["presets"].Get());

                        cout["allow_random_hue"] <<= s.hr;
                        cout["hue_factor"] <<= s.hf;
                        cout["allow_random_saturation"] <<= s.sr;
                        cout["saturation_factor"] <<= s.sf;
                        cout["allow_random_value"] <<= s.vr;
                        cout["value_factor"] <<= s.vf;
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
                    .EnableAutoMerge()
                )
                .EnableAutoMerge()
            )
            .Constructs(context => {
                WhiteNoise whiteNoise = new WhiteNoise(
                    (int)context["width"].Get(),
                    (int)context["height"].Get(),
                    (int)context["seed"].Get(),
                    (double)context["hue_factor"].Get(),
                    (double)context["saturation_factor"].Get(),
                    (double)context["value_factor"].Get(),
                    (bool)context["allow_random_hue"].Get(),
                    (bool)context["allow_random_saturation"].Get(),
                    (bool)context["allow_random_value"].Get());
                    
                return whiteNoise;
            })
        ,
        
        SettingsNode<WhiteNoise>.New("basic")
            .Group(IImageGenerator.UniversalSeedInput())
            .Argument("width", Arguments.Integer(128))
            .Argument("height", Arguments.Integer(128))
            .Constructs(context => new WhiteNoise(
                (int)context["width"].Get(),
                (int)context["height"].Get(),
                (int)context["seed"].Get())
            )
    );

    public static SettingsBuilder<WhiteNoise> GetSettings() {
        return (SettingsBuilder<WhiteNoise>)SETTINGS.Clone();
    }

    public IImageGenerator GetNextGenerator(int i) {
        WhiteNoise copy = (WhiteNoise)MemberwiseClone();
        copy.Seed = i;
        return copy;
    }

    public override string ToString() {
        return JsonSerializer.Serialize(this);
    }

    public object Clone() => MemberwiseClone();
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