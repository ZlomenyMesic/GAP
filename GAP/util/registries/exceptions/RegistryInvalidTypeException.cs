//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

namespace GAP.util.registries.exceptions;

public class RegistryInvalidTypeException : Exception {
    public RegistryInvalidTypeException(Type type, Type baseType) : 
        base($"Registered type {type.FullName} is not a {baseType.FullName}") { }
}