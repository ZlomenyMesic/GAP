//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;

namespace GAP.util.settings.argumentType;

/// <summary>
/// Bool Argument Type <br/>
/// simple bool select/checkbox
/// </summary>
public sealed class BoolArgumentType : ArgumentType {
    
    private bool? value { get; set; } = null;
    
    public string GetInputType() {
        return "Bool";
    }

    public string GetValue() {
        if (value == null) throw new SettingsBuilderException("Accessing value that has not been set.");
        
        return ((bool)value).ToString();
    }

    public object GetParsedValue() {
        if (value == null) throw new SettingsBuilderException("Accessing value that has not been set.");

        return value;
    }

    public void SetValue(string value) {
        this.value = Arguments.ParseBool(value);
    }

    public void SetParsedValue(object value) {
        if (value.GetType() != typeof(bool)) throw new SettingsArgumentException(value.GetType(), typeof(bool));
        
        this.value = (bool)value;
    }

    public ArgumentType Clone() { 
        BoolArgumentType newArg = new BoolArgumentType {
            value = value
        };
        
        return newArg;
    }

    public override string ToString() {
        return $"{{\"type\": \"bool\", \"value\": \"{(value == null ? "\"null\"" : value)}\"}}";
    }
}