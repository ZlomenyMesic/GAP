//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

namespace GAP.util.exceptions;

public class TypeNotEnumException : Exception {
    public TypeNotEnumException(Type type) : base($"Type '{type.Name}' is not a valid enum type.") { }    
}