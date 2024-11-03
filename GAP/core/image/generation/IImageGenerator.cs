//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using GAP.util;
using GAP.util.math;
using GAP.util.settings;

namespace GAP.core.image.generation;

/// <summary>
/// Image Generator Interface <br/>
/// all image generator classes must implement this class in order to be properly registered
/// </summary>
public interface IImageGenerator {
    
    /// <summary>
    /// main generation method
    /// </summary>
    /// <returns><see cref="Bitmap"/> object with the final image</returns>
    public Bitmap GenerateImage();

    /// <summary>
    /// returns copy of settings available for the generator
    /// </summary>
    /// <typeparam name="T">must be same as the generator class</typeparam>
    public static SettingsBuilder<T> GetSettings<T>() where T : IImageGenerator {
        throw new NotImplementedException();
    }

    private static readonly SettingsGroup UNIVERSAL_SEED_SETTINGS = SettingsGroup
        .New("seed", Context.New(("seed", Arguments.Integer())))
        .Option(SettingsGroupOption
            .New("number")
            .Argument("seed", Arguments.Integer())
            .EnableAutoParse()
        )
        .Option(SettingsGroupOption
            .New("word")
            .Argument("seed", Arguments.String())
            .OnParse((cin, cout) => {
                cout["seed"].SetParsedValue(SeedFormat.SeedFromWord((string)cin["seed"].GetParsedValue()));
            })
        )
        .Option(SettingsGroupOption
            .New("string")
            .Argument("seed", Arguments.String())
            .OnParse((cin, cout) => {
                cout["seed"].SetParsedValue(Hash.GetHashCode((string)cin["seed"].GetParsedValue()));
            })
        )
        .EnableAutoParse();
        
    /// <summary>
    /// returns a clone of the universal seed input group
    /// </summary>
    public static SettingsGroup UniversalSeedInput() {
        return (SettingsGroup)UNIVERSAL_SEED_SETTINGS.Clone();
    } 
}
