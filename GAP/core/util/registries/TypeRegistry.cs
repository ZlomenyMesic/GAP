//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.registries.exceptions;

namespace GAP.util.registries;

/// <summary>
/// registry for references of a class children
/// </summary>
/// <typeparam name="T">type of stored object</typeparam>
public abstract class TypeRegistry<T> {
    
    // ReSharper disable once StaticMemberInGenericType
    protected static readonly IDictionary<string, Type> REGISTRY = new Dictionary<string, Type>();

    /// <summary>
    /// registers a new class reference 
    /// </summary>
    /// <param name="id">id of the reference</param>
    /// <param name="registeredType">the reference</param>
    /// <returns>the reference</returns>
    /// <exception cref="RegistryInvalidTypeException">
    /// base class of <c>typeof(registeredType)</c> does not match <c>T</c>,
    /// registered type does not have an empty constructor
    /// </exception>
    /// <exception cref="RegistryCouldNotAddException">
    /// if failed to add the type due to a type with the same id already registered
    /// </exception>
    protected static Type BaseRegister(string id, Type registeredType) {
        if (registeredType.IsAssignableFrom(typeof(T))) {
            throw new RegistryInvalidTypeException(registeredType, typeof(T));
        }
        
        if (REGISTRY.TryAdd(id, registeredType)) return registeredType;
        
        throw new RegistryCouldNotAddException(id, registeredType);
    }

    /// <summary>
    /// returns a new instance of a type stored behind id
    /// </summary>
    /// <exception cref="RegistryItemNotFoundException">if desired object is not found</exception>
    protected static Type BaseGet(string id) {
        if (!REGISTRY.TryGetValue(id, out Type? value))
            throw new KeyNotFoundException($"Could not find registry object \'{id}\'.");

        return value;
    }

    /// <summary>
    /// returns all registered types
    /// </summary>
    public static (string id, Type type)?[] GetAll() {
        List<(string id, Type type)?> all = new();
        
        foreach (var r in REGISTRY) {
            all.Add((r.Key, r.Value)!);
        }
        
        return all.ToArray();
    }
}