//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using Kolors;

namespace GAP.util.registries;

/// <summary>
/// registry for references of a class children
/// </summary>
/// <typeparam name="T">type of stored object</typeparam>
public abstract class ClassRegistry<T> {
    
    private static IDictionary<string, Type?> REGISTRY { get; } = new Dictionary<string, Type?>();

    /// <summary>
    /// registers a new class reference 
    /// </summary>
    /// <param name="id">id of the reference</param>
    /// <param name="registeredType">the reference</param>
    /// <returns>the reference</returns>
    /// <exception cref="ArgumentException">when base class of <c>typeof(registeredType)</c> does not match <c>T</c></exception>
    public static Type Register(string id, Type registeredType) {
        if (registeredType.BaseType != typeof(T)) {
            Debug.error($"Registering type {registeredType.FullName} is not a {typeof(T).FullName}");
            throw new ArgumentException();
        }
        
        if (REGISTRY.TryAdd(id, registeredType)) return registeredType;
        Debug.error($"Could not add registry object \'{id}\' of type \'{registeredType}\' to the registry! " +
                    $"Object with the same id already exists.");
        return registeredType;
    }

    /// <summary>
    /// returns a new instance of a type stored behind id
    /// </summary>
    protected static T? GetInstance(string id, params object[] args) {
        T? instance = default;
        
        if (REGISTRY.TryGetValue(id, out Type? value)) {
            if (value != null) instance = (T)Activator.CreateInstance(value, args)!;
        }
        else {
            Debug.error($"Could not find registry object \'{id}\'.");
            return default;
        }
        
        return instance;
    }
}