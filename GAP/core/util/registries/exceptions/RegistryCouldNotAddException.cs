//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

namespace GAP.util.registries.exceptions;

public class RegistryCouldNotAddException : Exception {
    public RegistryCouldNotAddException(string id, Type type) : 
        base($"Could not add registry object \'{id}\' of type \'{type}\' to a registry! " +
             $"Object with the same id already exists.") { }
}