//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Globalization;
using GAP.util.exceptions;

namespace GAP.util.settings.argumentType;

/// <summary>
/// Float Argument Type <br/>
/// float decimal number
/// </summary>
public sealed class FloatArgumentType : ArgumentType {

    private float? value { get; set; } = null;
    public float min { get; }
    public float max { get; }

    internal FloatArgumentType(float min = float.MinValue, float max = float.MaxValue) {
        this.min = min;
        this.max = max;
    }
    
    public string GetInputType() {
        return "Float";
    }

    public string GetProperties() {
        return $"{min};{max}";
    }

    public string GetValue() {
        if (value == null) throw new SettingsBuilderException("Accessing value that has not been set.");

        return ((float)value).ToString(CultureInfo.InvariantCulture);
    }

    public object GetParsedValue() {
        if (value == null) throw new SettingsBuilderException("Accessing value that has not been set.");

        return value;
    }

    public void SetValue(string value) {
        this.value = Arguments.ParseFloat(value, this);
    }

    public void SetParsedValue(object value) {
        if (value.GetType() != typeof(float)) throw new SettingsArgumentException(value.GetType(), typeof(float));
        if ((float)value < min || (float)value > max) 
            throw new SettingsArgumentException($"Value of {value} must be greater than {min} and smaller than {max}.");

        this.value = (float)value;
    }

    public ArgumentType Clone() {
        FloatArgumentType clone = new FloatArgumentType(min, max) {
            value = value
        };
        
        return clone;
    }

    public override string ToString() {
        return $"{{\"type\": \"float\", " +
               $"\"min\": {min}, " +
               $"\"max\": {max}, " +
               $"\"value\": {(value == null ? "\"null\"" : ((float)value).ToString(CultureInfo.InvariantCulture))}}}";
    }
}