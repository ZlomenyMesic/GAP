//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.registries.exceptions;

namespace GAP.util.registries;

/// <summary>
/// registry for large amounts of instances of a class
/// </summary>
/// <typeparam name="T">type of stored object</typeparam>
public abstract class ObjectRegistry<T> {
    
    protected static IDictionary<string, T> REGISTRY { get; } = new Dictionary<string, T>();
    
    /// <summary>
    /// registers new object
    /// </summary>
    /// <param name="id">id of the instance</param>
    /// <param name="registeredItem">the instance</param>
    /// <returns>the instance with the id</returns>
    /// <exception cref="RegistryCouldNotAddException">
    /// if an object with the same id has already been registered
    /// </exception>
    protected static T Register(string id, T registeredItem) {
        
        if (REGISTRY.TryAdd(id, registeredItem)) return registeredItem;
        
        throw new RegistryCouldNotAddException(id, registeredItem!.GetType());
    }

    /// <summary>
    /// returns the registered object of <c>id</c>
    /// </summary>
    /// <exception cref="RegistryItemNotFoundException">if desired object is not found</exception>
    protected static T Get(string id) {
        if (REGISTRY.TryGetValue(id, out T? value)) {
            return value;
        }

        throw new RegistryItemNotFoundException($"Could not find registry object '{id}'");
    }
    
    /// <summary>
    /// returns all registered types
    /// </summary>
    public static (string id, T type)?[] GetAll() {
        List<(string id, T type)?> all = new();
        
        foreach (var r in REGISTRY) {
            all.Add((r.Key, r.Value));
        }
        
        return all.ToArray();
    }
}
