//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.registries;
using GAP.util.registries.exceptions;
using NeoKolors.Settings;

namespace GAP.core.image.generation;

/// <summary>
/// Image Generator Registry <br/>
/// holds references to all registered generators using the <see cref="TypeRegistry{T}"/> class registry class
/// </summary>
internal abstract class ImageGeneratorRegistry : TypeRegistry<IImageGenerator> {

    /// <summary>
    /// registers new <see cref="IImageGenerator"/>-implementing class using the
    /// <see cref="TypeRegistry{T}.BaseRegister"/> method
    /// </summary>
    /// <param name="id">id of the class</param>
    /// <param name="type"><c>typeof(&lt;YourClassName&gt;)</c></param>
    internal static void Register(string id, Type type) {
        var c = type.GetConstructor([]);
        if (c == null) {
            throw new RegistryInvalidTypeException(type);
        }
        
        BaseRegister(id, type);
    }
    
    /// <summary>
    /// registers new <see cref="IImageGenerator"/>-implementing class using the
    /// <see cref="TypeRegistry{T}.BaseRegister"/> method
    /// </summary>
    /// <param name="id">id of the class</param>
    internal static void Register<T>(string id) {
        var c = typeof(T).GetConstructor([]);
        if (c == null) {
            throw new RegistryInvalidTypeException(typeof(T));
        }
        
        BaseRegister(id, typeof(T));
    }
    
    /// <summary>
    /// returns type of the searched <see cref="IImageGenerator"/>-implementing class
    /// </summary>
    /// <param name="id">id of the searched class</param>
    /// <returns>new instance of the searched <see cref="IImageGenerator"/>-implementing class</returns>
    /// <exception cref="NullReferenceException">
    /// registered reference to class is null</exception>
    /// <exception cref="KeyNotFoundException">no class with id of <see cref="id"/> was not found</exception>
    public static Type GetType(string id) {
        if (!REGISTRY.TryGetValue(id, out Type? value))
            throw new KeyNotFoundException($"Could not find registry object \'{id}\'.");

        return value;
    }

    /// <summary>
    /// returns the settings of the desired generator
    /// </summary>
    /// <param name="id">id of the generator</param>
    /// <returns>the settings builder of the generator</returns>
    /// <exception cref="NullReferenceException">
    /// registered reference to class is null</exception>
    /// <exception cref="KeyNotFoundException">no class with id of <see cref="id"/> was not found</exception>
    public static ISettingsBuilder<IImageGenerator> GetSettings(string id) {
        var obj = (IImageGenerator)REGISTRY[id].GetConstructor([])!.Invoke([]);
        return obj.GetSettings();
    }
}