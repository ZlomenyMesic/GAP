//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Reflection;
using GAP.core.image.transformation.transformers;
using GAP.util.registries;
using Kolors;

namespace GAP.core.image.transformation;

/// <summary>
/// Image Transformer Registry <br/>
/// holds references to all registered transformers using the <see cref="ClassRegistry{T}"/> class registry class
/// </summary>
public class ImageTransformerRegistry : ClassRegistry<ImageTransformer> {
   
    /// <summary>
    /// registers new <see cref="ImageTransformer"/>-implementing class using the
    /// <see cref="ClassRegistry{T}.BaseRegister"/> method
    /// </summary>
    /// <param name="id">id of the class</param>
    /// <param name="type"><c>typeof(&lt;YourClassName&gt;)</c></param>
    /// <returns><see cref="type"/></returns>
    internal static Type Register(string id, Type type) {
        return BaseRegister(id, type);
    }
    
    /// <summary>
    /// returns new instance of the searched <see cref="ImageTransformer"/>-implementing class using the
    /// <see cref="jsonSettings"/> 
    /// </summary>
    /// <param name="id">id of the searched class</param>
    /// <param name="jsonSettings">json arguments</param>
    /// <returns>new instance of the searched <see cref="ImageTransformer"/>-implementing class</returns>
    /// <exception cref="NullReferenceException">
    /// if failed to create an instance or
    /// could not find the method <see cref="ImageTransformer.LoadFromJson"/> or
    /// registered reference to class is null</exception>
    /// <exception cref="KeyNotFoundException">no class with id of <see cref="id"/> was not found</exception>
    public static ImageTransformer GetInstance(string id, string jsonSettings) {
        Debug.info(REGISTRY.Count.ToString());
        
        if (REGISTRY.TryGetValue(id, out Type? value)) {
            if (value != null) {
                
                ImageTransformer? instance = (ImageTransformer?)Activator.CreateInstance(value);
                if (instance == null) {
                    throw new NullReferenceException($"Failed to create instance of type {value}");
                }
                
                MethodInfo? method = value.GetMethod("LoadFromJson");
                if (method == null) {
                    throw new NullReferenceException($"Can't find method LoadFromJson in type {value}"); 
                }
                
                instance.LoadFromJson(jsonSettings);
                return instance;
            }
            else throw new NullReferenceException(
                "Cannot create an instance of ImageTransformer. Requested type is null.");
        }
        else {
            throw new KeyNotFoundException($"Could not find registry object \'{id}\'.");
        }
    }
}