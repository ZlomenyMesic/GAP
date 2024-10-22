//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;
using GAP.util.settings;

namespace GAP.core.image.generation;

/// <summary>
/// Image Generator Interface <br/>
/// all image generator classes must implement this class in order to be properly registered
/// </summary>
public abstract class ImageGenerator {

    /// <summary>
    /// empty constructor, all child classes must have an empty constructor,
    /// otherwise automatic instance creation can fail
    /// </summary>
    public ImageGenerator() { }
    
    /// <summary>
    /// main generation method
    /// </summary>
    /// <returns><see cref="Bitmap"/> object with the final image</returns>
    public abstract Bitmap GenerateImage();
    
    /// <summary>
    /// loads settings from a json string into an instance that called this method
    /// </summary>
    /// <param name="settings">json string</param>
    /// <exception cref="JsonException">
    /// if inputted json is invalid or if value returned by the
    /// <see cref="JsonSerializer.Deserialize{TValue}(System.IO.Stream,System.Text.Json.JsonSerializerOptions?)"/>
    /// returns null
    /// </exception>
    /// <exception cref="ArgumentException">if <see cref="settings"/> is null</exception>
    /// <exception cref="NotSupportedException">if no deserializer is available</exception>
    public abstract void LoadFromJson(string settings);

    /// <summary>
    /// returns copy of settings available for the generator
    /// </summary>
    public abstract SettingsBuilder<T> GetSettings<T>() where T : ImageGenerator;
}