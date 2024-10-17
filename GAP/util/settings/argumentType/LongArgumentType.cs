using GAP.util.exceptions;

namespace GAP.util.settings.argumentType;

/// <summary>
/// Int64 Argument Type
/// </summary>
public sealed class LongArgumentType : ArgumentType {
    
    private long? value { get; set; } = null;
    public long min { get; }
    public long max { get; }

    internal LongArgumentType(long min = Int64.MinValue, long max = Int64.MaxValue) {
        this.min = min;
        this.max = max;
    }
    
    public string GetInputType() {
        return "Long";
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
        this.value = Arguments.ParseLong(value, this);
    }

    public void SetParsedValue(object value) {
        if (value.GetType() != typeof(long)) throw new SettingsArgumentException(value.GetType(), typeof(long));
        if ((long)value < min || (long)value > max) 
            throw new SettingsArgumentException($"Value of {value} must be greater than {min} and smaller than {max}.");

        this.value = (long)value;
    }

    public ArgumentType Clone() {
        LongArgumentType clone = new LongArgumentType(min, max) {
            value = value
        };
        
        return clone;
    }

    public override string ToString() {
        return $"{{\"type\": \"long\", " +
               $"\"min\": {min}, " +
               $"\"max\": {max}, " +
               $"\"value\": {(value == null ? "\"null\"" : value)}}}}}";
    }
}


/// <summary>
/// Unsigned Int64 Argument Type
/// </summary>
public sealed class UnsignedLongArgumentType : ArgumentType {
    
    private ulong? value { get; set; } = null;
    public ulong min { get; }
    public ulong max { get; }

    internal UnsignedLongArgumentType(ulong min = ulong.MinValue, ulong max = ulong.MaxValue) {
        this.min = min;
        this.max = max;
    }
    
    public string GetInputType() {
        return "ULong";
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
        this.value = Arguments.ParseULong(value, this);
    }

    public void SetParsedValue(object value) {
        if (value.GetType() != typeof(ulong)) throw new SettingsArgumentException(value.GetType(), typeof(ulong));
        if ((ulong)value < min || (ulong)value > max) 
            throw new SettingsArgumentException($"Value of {value} must be greater than {min} and smaller than {max}.");
        
        this.value = (ulong)value;
    }
    
    public ArgumentType Clone() {
        UnsignedLongArgumentType clone = new UnsignedLongArgumentType(min, max) {
            value = value
        };
        
        return clone;
    }
    
    public override string ToString() {
        return $"{{\"type\": \"ulong\", " +
               $"\"min\": {min}, " +
               $"\"max\": {max}, " +
               $"\"value\": {(value == null ? "\"null\"" : value)}}}}}";
    }
}