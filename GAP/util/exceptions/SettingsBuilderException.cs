//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

namespace GAP.util.exceptions;

public class SettingsBuilderException : Exception {
    public SettingsBuilderException(string message) : base($"Failed to build settings. {message}") { }
}