//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;

namespace GAP.util.settings.argumentType;

/// <summary>
/// String Argument Type
/// </summary>
public class StringArgumentType : ArgumentType {

    private string? value { get; set; } = null;
    public uint minLength { get; }
    public uint maxLength { get; }

    internal StringArgumentType(uint minLength = 0, uint maxLength = UInt32.MaxValue) {
        this.minLength = minLength;
        this.maxLength = maxLength;
    }
    
    public string GetInputType() {
        return "String";
    }

    public string GetValue() {
        if (value == null) throw new SettingsBuilderException("Accessing value that has not been set.");

        return value;
    }

    public object GetParsedValue() {
        if (value == null) throw new SettingsBuilderException("Accessing value that has not been set.");

        return value;
    }

    public void SetValue(string value) {
        this.value = Arguments.ParseString(value, this);
    }

    public void SetParsedValue(object value) {
        if (value.GetType() != typeof(string)) throw new SettingsArgumentException(value.GetType(), typeof(string));
        if (((string)value).Length < minLength || ((string)value).Length > maxLength) 
            throw new SettingsArgumentException($"Length of string '{value}' must be greater than " +
                                                $"{minLength} and smaller than {maxLength}.");

        this.value = (string)value;
    }

    public ArgumentType Clone() {
        StringArgumentType clone = new StringArgumentType(minLength, maxLength) {
            value = value
        };
        
        return clone;
    }

    public override string ToString() {
        return $"{{\"type\": \"string\", " +
               $"\"minLength\": \"{minLength}\", " +
               $"\"maxLength\": \"{maxLength}\", " +
               $"\"value\": {value ?? "\"null\""}}}";
    }
}