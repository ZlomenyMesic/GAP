//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
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
}
