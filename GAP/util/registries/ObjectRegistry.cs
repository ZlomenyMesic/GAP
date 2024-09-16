//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.registries.exceptions;
using Kolors;

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
    protected static T Register(string id, T registeredItem) {
        
        if (REGISTRY.TryAdd(id, registeredItem)) return registeredItem;
        
        throw new RegistryCouldNotAddException(id, registeredItem!.GetType());
        
        // Debug.error($"Could not add registry object \'{id}\' of type \'{registeredItem!.GetType()}\' to a registry! " +
        //             $"Object with the same id already exists.");
        // return registeredItem;
    }

    /// <summary>
    /// returns the registered object of <c>id</c>
    /// </summary>
    protected static T? Get(string id) {
        if (REGISTRY.TryGetValue(id, out T? value)) {
            return value;
        }
        else {
            throw new RegistryItemNotFoundException($"Could not find registry object '{id}'");
            Debug.error($"Could not find registry object \'{id}\'.");
            return default;
        }
    }
}