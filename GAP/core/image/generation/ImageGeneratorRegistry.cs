//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Reflection;
using GAP.util.exceptions;
using GAP.util.registries;
using GAP.util.settings;
using Kolors;

namespace GAP.core.image.generation;

/// <summary>
/// Image Generator Registry <br/>
/// holds references to all registered generators using the <see cref="ClassRegistry{T}"/> class registry class
/// </summary>
internal abstract class ImageGeneratorRegistry : ClassRegistry<IImageGenerator> {

    /// <summary>
    /// registers new <see cref="IImageGenerator"/>-implementing class using the
    /// <see cref="ClassRegistry{T}.BaseRegister"/> method
    /// </summary>
    /// <param name="id">id of the class</param>
    /// <param name="type"><c>typeof(&lt;YourClassName&gt;)</c></param>
    internal static void Register(string id, Type type) {
        BaseRegister(id, type);
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
        Debug.Info(REGISTRY.Count.ToString());

        if (!REGISTRY.TryGetValue(id, out Type? value))
            throw new KeyNotFoundException($"Could not find registry object \'{id}\'.");
        
        if (value != null) {
            return value;
        }

        throw new NullReferenceException(
            "Cannot create an instance of ImageGenerator. Requested type is null.");

    }

    /// <summary>
    /// returns the settings of the desired generator
    /// </summary>
    /// <param name="id">id of the generator</param>
    /// <returns>the settings builder of the generator</returns>
    /// <exception cref="SettingsBuilderException">could not get the settings</exception>
    /// <exception cref="NullReferenceException">
    /// registered reference to class is null</exception>
    /// <exception cref="KeyNotFoundException">no class with id of <see cref="id"/> was not found</exception>
    public static object GetSettings(string id) {
        MethodInfo? mf = GetType(id).GetMethod("GetSettings");
        
        object? result;
        
        if (mf != null) {
            result = mf.Invoke(null, null);
        }
        else {
            throw new SettingsBuilderException($"Cannot get settings of {GetType(id).FullName}. Method does not exist.");
        }

        if (result == null) {
            throw new SettingsBuilderException($"Cannot get settings of {GetType(id).FullName}.");
        }
        
        if (typeof(SettingsBuilder<>).Name == result.GetType().Name) {
            return result;
        }
        
        throw new SettingsBuilderException($"Cannot get settings of {GetType(id).FullName}.");
    }
}