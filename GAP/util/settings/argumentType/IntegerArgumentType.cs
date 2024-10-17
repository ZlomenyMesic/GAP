//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;

namespace GAP.util.settings.argumentType;

/// <summary>
/// Int32 Argument Type
/// </summary>
public sealed class IntegerArgumentType : ArgumentType {
    
    private int? value { get; set; } = null;
    public int min { get; }
    public int max { get; }

    internal IntegerArgumentType(int min = int.MinValue, int max = int.MaxValue) {
        this.min = min;
        this.max = max;
    }
    
    public string GetInputType() {
        return "Integer";
    }

    public string GetValue() {
        if (value == null) throw new SettingsBuilderException("Accessing value that has not been set.");

        return ((int)value).ToString();
    }

    public object GetParsedValue() {
        if (value == null) throw new SettingsBuilderException("Accessing value that has not been set.");

        return value;
    }

    public void SetValue(string value) {
        this.value = Arguments.ParseInteger(value, this);
    }

    public void SetParsedValue(object value) {
        if (value.GetType() != typeof(int)) throw new SettingsArgumentException(value.GetType(), typeof(int));
        if ((int)value < min || (int)value > max) 
            throw new SettingsArgumentException($"Value of {value} must be greater than {min} and smaller than {max}.");

        this.value = (int)value;
    }

    public ArgumentType Clone() {
        return (ArgumentType)MemberwiseClone();
    }

    public override string ToString() {
        return $"{{\"type\": \"int\", " +
               $"\"min\": {min}, " +
               $"\"max\": {max}, " +
               $"\"value\": {(value == null ? "\"null\"" : value)}}}}}";
    }
}


/// <summary>
/// Unsigned Int32 Argument Type
/// </summary>
public sealed class UnsignedIntegerArgumentType : ArgumentType {
    
    private uint? value { get; set; } = null;
    public uint min { get; }
    public uint max { get; }

    internal UnsignedIntegerArgumentType(uint min = uint.MinValue, uint max = uint.MaxValue) {
        this.min = min;
        this.max = max;
    }
    
    public string GetInputType() {
        return "UInteger";
    }

    public string GetValue() {
        if (value == null) throw new SettingsBuilderException("Accessing value that has not been set.");

        return ((int)value).ToString();
    }

    public object GetParsedValue() {
        if (value == null) throw new SettingsBuilderException("Accessing value that has not been set.");

        return value;
    }

    public void SetValue(string value) {
        this.value = Arguments.ParseUInteger(value, this);
    }

    public void SetParsedValue(object value) {
        if (value.GetType() != typeof(uint)) throw new SettingsArgumentException(value.GetType(), typeof(uint));
        if ((uint)value < min || (uint)value > max) 
            throw new SettingsArgumentException($"Value of {value} must be greater than {min} and smaller than {max}.");
        
        this.value = (uint)value;
    }

    public ArgumentType Clone() {
        return (ArgumentType)MemberwiseClone();
    }

    public override string ToString() {
        return $"{{\"type\": \"uint\", " +
               $"\"min\": {min}, " +
               $"\"max\": {max}, " +
               $"\"value\": {(value == null ? "\"null\"" : value)}}}";
    }
}