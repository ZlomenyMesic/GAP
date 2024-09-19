//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

namespace GAP.util.registries.exceptions;

public class RegistryItemNotFoundException : Exception {
    public RegistryItemNotFoundException(string id) : base($"Could not find registry object \'{id}\'.") { }
}