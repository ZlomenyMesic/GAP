//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using System.Text.Json;
using GAP.util.settings;

namespace GAP.core.image.transformation;

/// <summary>
/// Image Transformer Interface <br/>
/// all image transformer classes must implement this class in order to be properly registered
/// </summary>
public abstract class ImageTransformer {

    /// <summary>
    /// empty constructor, all child classes must have an empty constructor,
    /// otherwise automatic instance creation can fail
    /// </summary>
    public ImageTransformer() {}
    
    /// <summary>
    /// main transformation method
    /// </summary>
    /// <returns><see cref="Bitmap"/> object with the final image</returns>
    public abstract Bitmap TransformImage(Bitmap image);
    
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
    /// returns copy of settings available for the transformer
    /// </summary>
    public abstract SettingsBuilder<T> GetSettings<T>() where T : ImageTransformer;
    
    public override string ToString() => JsonSerializer.Serialize(this);
}