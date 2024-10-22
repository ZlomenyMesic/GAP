//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

namespace GAP.util.settings.argumentType;

/// <summary>
/// Argument Type Interface <br/>
/// used in settings
/// </summary>
public interface ArgumentType {
    
    /// <summary>
    /// returns a string with the type's name
    /// </summary>
    public string GetInputType();

    /// <summary>
    /// returns the stringified value stored in an argument
    /// </summary>
    public string GetValue();
    
    /// <summary>
    /// returns the raw value stored in an argument
    /// </summary>
    public object GetParsedValue();
    
    /// <summary>
    /// sets the value in an argument by parsing from a string
    /// </summary>
    public void SetValue(string value);

    /// <summary>
    /// sets the value stored in an argument from a raw value
    /// </summary>
    public void SetParsedValue(object value);

    /// <summary>
    /// clones the argument (all its field including its value)
    /// </summary>
    public ArgumentType Clone();
}