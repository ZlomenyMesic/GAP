//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.registries.exceptions;
using Kolors;

namespace GAP.util.registries;

/// <summary>
/// registry for references of a class children
/// </summary>
/// <typeparam name="T">type of stored object</typeparam>
public abstract class ClassRegistry<T> {
    
    protected static IDictionary<string, Type?> REGISTRY { get; } = new Dictionary<string, Type?>();

    /// <summary>
    /// registers a new class reference 
    /// </summary>
    /// <param name="id">id of the reference</param>
    /// <param name="registeredType">the reference</param>
    /// <returns>the reference</returns>
    /// <exception cref="RegistryInvalidTypeException">
    /// when base class of <c>typeof(registeredType)</c> does not match <c>T</c>
    /// </exception>
    /// <exception cref="RegistryCouldNotAddException">
    /// if failed to add the type due to a type with the same id already registered
    /// </exception>
    protected static Type BaseRegister(string id, Type registeredType) {
        if (registeredType.BaseType != typeof(T)) {
            throw new RegistryInvalidTypeException(registeredType, typeof(T));
        }
        
        if (REGISTRY.TryAdd(id, registeredType)) return registeredType;
        
        throw new RegistryCouldNotAddException(id, registeredType);
    }

    /// <summary>
    /// returns a new instance of a type stored behind id
    /// </summary>
    /// <exception cref="RegistryItemNotFoundException">if desired object is not found</exception>
    protected static T? BaseGetInstance(string id, params object[] args) {
        T? instance = default;
        
        if (REGISTRY.TryGetValue(id, out Type? value)) {
            if (value != null) instance = (T)Activator.CreateInstance(value, args)!;
        }
        else {
            throw new RegistryItemNotFoundException($"Could not find registry object \'{id}\'.");
        }
        
        return instance;
    }
}