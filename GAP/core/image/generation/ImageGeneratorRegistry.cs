//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Reflection;
using GAP.util.registries;
using Kolors;

namespace GAP.core.image.generation;

/// <summary>
/// Image Generator Registry <br/>
/// holds references to all registered generators using the <see cref="ClassRegistry{T}"/> class registry class
/// </summary>
public abstract class ImageGeneratorRegistry : ClassRegistry<ImageGenerator> {

    /// <summary>
    /// registers new <see cref="ImageGenerator"/>-implementing class
    /// </summary>
    /// <param name="id">id of the class</param>
    /// <param name="type"><c>typeof(&lt;YourClassName&gt;)</c></param>
    /// <returns><see cref="type"/></returns>
    internal static Type Register(string id, Type type) {
        return BaseRegister(id, type);
    }
    
    /// <summary>
    /// returns new instance of the searched <see cref="ImageGenerator"/>-implementing class using the
    /// <see cref="jsonSettings"/> 
    /// </summary>
    /// <param name="id">id of the searched class</param>
    /// <param name="jsonSettings">json arguments</param>
    /// <returns>new instance of the searched <see cref="ImageGenerator"/>-implementing class</returns>
    /// <exception cref="NullReferenceException">
    /// if failed to create an instance or
    /// could not find the method <see cref="ImageGenerator.LoadFromJson"/> or
    /// registered reference to class is null</exception>
    /// <exception cref="KeyNotFoundException">no class with id of <see cref="id"/> was not found</exception>
    public static ImageGenerator GetInstance(string id, string jsonSettings) {
        Debug.info(REGISTRY.Count.ToString());
        
        if (REGISTRY.TryGetValue(id, out Type? value)) {
            if (value != null) {
                
                ImageGenerator? instance = (ImageGenerator?)Activator.CreateInstance(value);
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
                "Cannot create an instance of ImageGenerator. Requested type is null.");
        }
        else {
            throw new KeyNotFoundException($"Could not find registry object \'{id}\'.");
        }
    }
}