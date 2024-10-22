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
    private List<object>? parsedValues { get; set; } = null;

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
        throw new SettingsBuilderException("This method is not supported for FreeListArgumentType. " +
                                           "Use AddValue(<ArgumentType> item) and PopValue(<int> index)" +
                                           " methods instead.");
        
        // value = value.Replace(",", "");
        // value = value.Replace("[", "");
        // string[] singleValues = value.Split(' ');
        //
        // values = new List<ArgumentType>();
        //
        // foreach (string v in singleValues) {
        //     ArgumentType a = type.Clone();
        //     a.SetValue(v);
        //     values.Add(a);
        // }
    }

    public void SetParsedValue(object? value) {
        throw new SettingsBuilderException("This method is not supported for FreeListArgumentType. " +
                                           "Use AddValue(<object> item) and PopValue(<int> index)" +
                                           " methods instead.");
        
        // values = value switch {
        //     null => null,
        //     List<ArgumentType> list => list,
        //     ArgumentType[] array => array.ToList(),
        //     _ => throw new SettingsArgumentException(
        //         "Cannot set value of free list argument to a value of type that is not " +
        //         "System.Collections.Generic.List<GAP.util.settings.ArgumentType>.")
        // };
    }

    /// <summary>
    /// adds a new value to the argument values
    /// </summary>
    public void AddValue(object value) {
        parsedValues ??= new List<object>();
        
        parsedValues.Add(Arguments.Parse(value, type));
    }

    /// <summary>
    /// removes a value using an index
    /// </summary>
    /// <exception cref="SettingsBuilderException">no values have been set, index out of bounds</exception>
    public void PopValue(int index) {
        if (parsedValues is null) throw new SettingsBuilderException("Accessing value that has not been set.");
        if (index < 0 || index >= parsedValues.Count) throw new SettingsBuilderException("Index is out of range.");
        
        parsedValues.RemoveAt(index);
    }

    /// <summary>
    /// removes a value using the actual object
    /// </summary>
    /// <exception cref="SettingsBuilderException">no values have been set, object does not exist</exception>
    public void PopValue(object value) {
        if (parsedValues is null) throw new SettingsBuilderException("Accessing value that has not been set.");
        
        if (parsedValues.Remove(value)) throw new SettingsBuilderException("Tried to pop an nonexistent element.");
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