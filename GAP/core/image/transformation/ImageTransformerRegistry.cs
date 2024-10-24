//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.registries;

namespace GAP.core.image.transformation;

/// <summary>
/// Image Transformer Registry <br/>
/// holds references to all registered transformers using the <see cref="ClassRegistry{T}"/> class registry class
/// </summary>
internal abstract class ImageTransformerRegistry : ClassRegistry<IImageTransformer> {
   
    /// <summary>
    /// registers new <see cref="IImageTransformer"/>-implementing class using the
    /// <see cref="ClassRegistry{T}.BaseRegister"/> method
    /// </summary>
    /// <param name="id">id of the class</param>
    /// <param name="type"><c>typeof(&lt;YourClassName&gt;)</c></param>
    internal static void Register(string id, Type type) {
        BaseRegister(id, type);
    }
    
    /// <summary>
    /// returns new instance of the searched <see cref="IImageTransformer"/>-implementing class
    /// </summary>
    /// <param name="id">id of the searched class</param>
    /// <exception cref="NullReferenceException">
    /// registered reference to class is null</exception>
    /// <exception cref="KeyNotFoundException">no class with id of <see cref="id"/> was not found</exception>
    public static Type Get(string id) {
        if (!REGISTRY.TryGetValue(id, out Type? value))
            throw new KeyNotFoundException($"Could not find registry object \'{id}\'.");
        
        if (value != null) {
            return value;
        }

        throw new NullReferenceException(
            "Cannot create an instance of ImageTransformer. Requested type is null.");
    }
}