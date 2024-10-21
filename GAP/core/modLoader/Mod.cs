//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

namespace GAP.core.modLoader;

/// <summary>
/// Mod Interface <br/>
/// an interface all mod libraries must implement
/// </summary>
public interface Mod {

    /// <summary>
    /// method called at mod gathering
    /// </summary>
    /// <returns>mod id</returns>
    public string Register();
    
    /// <summary>
    /// method called at mod initialization
    /// </summary>
    public void Initialize();

    /// <summary>
    /// returns the description of the mod
    /// </summary>
    public string GetInfo();
}