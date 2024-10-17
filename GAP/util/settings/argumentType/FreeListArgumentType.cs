//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;

namespace GAP.util.settings.argumentType;

/// <summary>
/// Free List Argument Type <br/>
/// list with freely addable items of another <see cref="ArgumentType"/>
/// </summary>
public sealed class FreeListArgumentType : ArgumentType {
    
    private int maxItemCount { get; }
    private ArgumentType type { get; }
    private List<ArgumentType>? values { get; set; } = null;

    /// <summary>
    /// constructor that sources values from an enum
    /// </summary>
    /// <param name="type">source enum type</param>
    /// <param name="maxItemCount">maximum list item count</param>
    public FreeListArgumentType(ArgumentType type, int maxItemCount = Int32.MaxValue) {
        if (maxItemCount < 1) {
            throw new SettingsBuilderException("Maximal item count cannot be less than 1.");
        }
        
        if (type is FreeListArgumentType or SelectListArgumentType or
            SingleSelectArgumentType or MultiSelectArgumentType) {
            throw new SettingsBuilderException("Type of FreeListArgumentType, SelectListArgumentType, " +
                                               "MultiSelectArgumentType and SingleSelectArgumentType are " +
                                               "not supported in FreeListArgumentType.");
        }
        
        this.type = type;
        this.maxItemCount = maxItemCount;
    }
    
    public string GetInputType() {
        return "FreeList";
    }

    public string GetValue() {
        if (values is null) throw new SettingsBuilderException("Accessing value that has not been set.");
        
        string value = "[";

        for (int i = 0; i < value.Length; i++) {
            value += values[i].GetValue() + (i == values.Count - 1 ? "" : ", ");
        }

        value += "]";
        
        return value;
    }

    public object GetParsedValue() {
        if (values is null) throw new SettingsBuilderException("Accessing value that has not been set.");
        
        List<object> output = new List<object>();

        foreach (ArgumentType v in values) {
            output.Add(v.GetParsedValue());
        }
        
        return output.ToArray();
    }

    public void SetValue(string value) {
        value = value.Replace(",", "");
        value = value.Replace("[", "");
        string[] singleValues = value.Split(' ');
        
        values = new List<ArgumentType>();

        foreach (string v in singleValues) {
            ArgumentType a = type.Clone();
            a.SetValue(v);
            values.Add(a);
        }
    }

    public void SetParsedValue(object? value) {
        values = value switch {
            null => null,
            List<ArgumentType> list => list,
            ArgumentType[] array => array.ToList(),
            _ => throw new SettingsArgumentException(
                "Cannot set value of free list argument to a value of type that is not " +
                "System.Collections.Generic.List<GAP.util.settings.ArgumentType>.")
        };
    }

    public ArgumentType Clone() {
        FreeListArgumentType clone = new FreeListArgumentType(type.Clone(), maxItemCount);
        
        if (values == null) {
            clone.SetParsedValue(null);
            return clone;
        }
        
        clone.values = new List<ArgumentType>();
        foreach (ArgumentType v in values) {
            clone.values.Add(v.Clone());
        }
        
        return clone;
    }
}