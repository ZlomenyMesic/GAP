//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;

namespace GAP.util.settings.argumentType;

/// <summary>
/// Single Selection Argument Type <br/>
/// lets you select a single item from multiple options
/// </summary>
public class SingleSelectArgumentType : ArgumentType {
    public Type? type { get; } = null;
    public string[] values { get; }
    private int? selectedIndex { get; set; } = null;

    public SingleSelectArgumentType(string[] values) {
        type = null;
        this.values = values;
    }
    
    /// <summary>
    /// constructor that sources values from an enum
    /// </summary>
    /// <param name="type">source enum type</param>
    /// <exception cref="TypeNotEnumException"></exception>
    public SingleSelectArgumentType(Type type) {
        this.type = type;
        try {
            values = Enum.GetNames(type);
        }
        catch (ArgumentException) {
            throw new TypeNotEnumException(type);
        }
    }

    public string GetInputType() {
        return "SingleSelect";
    }

    public string GetValue() {
        if (values == null || values.Length == 0) {
            throw new SettingsBuilderException("No values provided to argument.");
        }

        if (selectedIndex == null) {
            throw new SettingsBuilderException("No value has been selected.");
        }
        
        return values[(int)selectedIndex];
    }

    public object GetParsedValue() {
        if (values == null || values.Length == 0)
            throw new SettingsBuilderException("No values provided to argument.");

        if (selectedIndex == null)
            throw new SettingsBuilderException("No value has been selected.");

        return type == null ? values[(int)selectedIndex] : Enum.Parse(type, values[(int)selectedIndex]);
    }

    public void SetValue(string value) {
        
        if (values == null || values.Length == 0)
            throw new SettingsArgumentException("No values provided to argument.");
        
        for (int i = 0; i < values.Length; i++) {
            if (value == values[i]) {
                selectedIndex = i;
                return;
            }
        }
        
        throw new SettingsArgumentException("Selected invalid value.");
    }

    public void SetParsedValue(object value) {
        if (value.GetType() == type) {
            SetValue(Enum.GetName(type, value) ?? string.Empty);
            return;
        }
        
        SetValue((string)value);
    }

    /// <summary>
    /// sets the index pointing to the selected value
    /// </summary>
    /// <exception cref="SettingsArgumentException">index is not valid</exception>
    public void SetIndex(int index) {
        if (index < 0 || index >= values.Length) throw new SettingsArgumentException("Index out of range.");
        selectedIndex = index;
    }

    /// <summary>
    /// returns the index pointing to the selected value, null if no value has been selected 
    /// </summary>
    public int? GetIndex() {
        return selectedIndex;
    }

    public ArgumentType Clone() {
        if (values == null || values.Length == 0)
            throw new SettingsArgumentException("No values provided to argument.");

        SingleSelectArgumentType clone = type == null ? 
            new SingleSelectArgumentType(values) : new SingleSelectArgumentType(type);
        
        clone.selectedIndex = selectedIndex;
        
        return clone;
    }

    public override string ToString() {
        string output = $"{{\"type\": \"single_select\", \"value\": " +
                        $"{(selectedIndex == null ? "\"null\"" : selectedIndex.ToString())}, \"values\": [";

        for (int i = 0; i < values.Length; i++) {
            output += "\"" + values[i] + "\"" + (i != values.Length - 1 ? ", " : "");
        }
        
        output += "]}";
        
        return output;
    }
}