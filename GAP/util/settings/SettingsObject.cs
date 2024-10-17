//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

namespace GAP.util.settings;

/// <summary>
/// Settings Object Interface <br/>
/// all results of settings parsing must implement this interface  
/// </summary>
public interface SettingsObject {
    
    /// <summary>
    /// sets the settings parsed values to internal properties
    /// </summary>
    /// <param name="input">name-value input</param>
    public void Deserialize((string name, object value)[] input);
}