//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Drawing;
using NeoKolors.Settings;

namespace GAP.core.image.transformation;

/// <summary>
/// Image Transformer Interface <br/>
/// all image transformer classes must implement this class in order to be properly registered
/// </summary>
public interface IImageTransformer<TSelf> : IImageTransformer where TSelf : class, IImageTransformer<TSelf> {

    /// <summary>
    /// returns copy of settings available for the transformer
    /// </summary>
    public new ISettingsBuilder<TSelf> GetSettings();
}

public interface IImageTransformer {
    
    /// <summary>
    /// main transformation method
    /// </summary>
    /// <returns><see cref="Bitmap"/> object with the final image</returns>
    public Bitmap TransformImage(Bitmap image);
    
    /// <summary>
    /// returns copy of settings available for the transformer
    /// </summary>
    public ISettingsBuilder<IImageTransformer> GetSettings();
}