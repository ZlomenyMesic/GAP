//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Globalization;
using GAP.util.exceptions;

namespace GAP.util.settings.argumentType;

/// <summary>
/// Double Argument Type <br/>
/// double decimal number
/// </summary>
public sealed class DoubleArgumentType : ArgumentType {

    private double? value { get; set; } = null;
    public double min { get; }
    public double max { get; }

    internal DoubleArgumentType(double min = double.MinValue, double max = double.MaxValue) {
        this.min = min;
        this.max = max;
    }
    
    public string GetInputType() {
        return "Double";
    }

    public string GetValue() {
        if (value == null) throw new SettingsBuilderException("Accessing value that has not been set.");

        return ((double)value).ToString(CultureInfo.InvariantCulture);
    }

    public object GetParsedValue() {
        if (value == null) throw new SettingsBuilderException("Accessing value that has not been set.");

        return value;
    }

    public void SetValue(string value) {
        this.value = Arguments.ParseDouble(value, this);
    }

    public void SetParsedValue(object value) {
        if (value.GetType() != typeof(bool)) throw new SettingsArgumentException(value.GetType(), typeof(double));
        if ((double)value < min || (double)value > max) 
            throw new SettingsArgumentException($"Value of {value} must be greater than {min} and smaller than {max}.");

        this.value = (double)value;
    }

    public ArgumentType Clone() {
        DoubleArgumentType newArg = new DoubleArgumentType(min, max) {
            value = value
        };
        
        return newArg;
    }

    public override string ToString() {
        return $"{{\"type\": \"double\", " +
               $"\"min\": {min}, " +
               $"\"max\": {max}, " +
               $"\"value\": {(value == null ? "\"null\"" : ((float)value).ToString(CultureInfo.InvariantCulture))}}}";
    }
}