//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;
using Kolors;

namespace GAP.util.settings.argumentType;

/// <summary>
/// Multiselect Argument Type <br/>
/// lets you select multiple items from multiple options 
/// </summary>
public class MultiSelectArgumentType : ArgumentType {
    
    public Type? type { get; }
    public string[] values { get; }
    private bool[] selected { get; }

    /// <summary>
    /// constructor that sources values from an enum
    /// </summary>
    /// <param name="type">source enum type</param>
    /// <exception cref="TypeNotEnumException"></exception>
    public MultiSelectArgumentType(Type type) {
        this.type = type;

        try {
            values = Enum.GetNames(type);
        }
        catch (ArgumentException) {
            throw new TypeNotEnumException(type);
        }
        
        selected = new bool[values.Length];
    }

    public MultiSelectArgumentType(string[] values) {
        type = null;
        this.values = values;
        selected = new bool[values.Length];
    }
    
    public string GetInputType() {
        return "MultiSelect";
    }

    public string GetValue() {
        string output = "[";

        string[] selectedValues = (string[])GetParsedValue();

        for (int i = 0; i < selectedValues.Length; i++) {
            output += selectedValues[i] + (i == selectedValues.Length - 1 ? "" : ", ");
        }

        output += "]";
        
        return output;
    }

    public object GetParsedValue() {
        List<string> selectedValues = new List<string>();
        
        for (int i = 0; i < values.Length; i++) {
            if (selected[i]) {
                selectedValues.Add(values[i]);
            }
        }

        if (type == null) return selectedValues.ToArray();
        
        List<object> enumValues = new List<object>();

        foreach (string s in selectedValues) {
            enumValues.Add(Enum.Parse(type, s));
        }
        
        return enumValues.ToArray();
    }

    public void SetValue(string value) {
        throw new SettingsBuilderException("This method is not supported for MultiSelectArgumentType. " +
                                           "Use SelectValue(<string>, <bool>), SelectValue(<int>, <bool>)," +
                                           " ToggleValue, SelectValue or DeselectValue methods instead.");
    }

    /// <summary>
    /// sets a value by the actual name of the item
    /// </summary>
    public void SetValue(string value, bool state) {
        for (int i = 0; i < values.Length; i++) {
            if (values[i] != value) continue;
            selected[i] = state;
            return;
        }
        
        Debug.warn("Trying to set a value that does not exist.");
    }

    /// <summary>
    /// sets a value by its index in the <see cref="values"/>
    /// </summary>
    /// <exception cref="SettingsArgumentException">index is out of bounds</exception>
    public void SetValue(int index, bool state) {
        if (index < 0 || index >= values.Length) 
            throw new SettingsArgumentException("Inputted index is not valid (out of bounds).");
        
        selected[index] = state;
    }

    /// <summary>
    /// selects a value by the actual name of the item
    /// </summary>
    public void SelectValue(string value) {
        for (int i = 0; i < values.Length; i++) {
            if (values[i] != value) continue;
            selected[i] = true;
            return;
        }
        
        Debug.warn("Trying to select a value that does not exist.");
    }

    /// <summary>
    /// selects a value by its index in <see cref="values"/>
    /// </summary>
    /// <exception cref="SettingsArgumentException">index is out of bounds</exception>
    public void SelectValue(int index) {
        if (index < 0 || index >= values.Length) 
            throw new SettingsArgumentException("Inputted index is not valid (out of bounds).");
        
        selected[index] = true;
    }

    /// <summary>
    /// deselects a value by the actual name of the item
    /// </summary>
    public void DeselectValue(string value) {
        for (int i = 0; i < values.Length; i++) {
            if (values[i] != value) continue;
            selected[i] = false;
            return;
        }
        
        Debug.warn("Trying to deselect a value that does not exist.");
    }

    /// <summary>
    /// deselects a value by its index in <see cref="values"/>
    /// </summary>
    /// <exception cref="SettingsArgumentException">index is out of bounds</exception>
    public void DeselectValue(int index) {
        if (index < 0 || index >= values.Length) 
            throw new SettingsArgumentException("Inputted index is not valid (out of bounds).");
        
        selected[index] = false;
    }

    /// <summary>
    /// toggles a value by its actual name
    /// </summary>
    public void ToggleValue(string value) {
        for (int i = 0; i < values.Length; i++) {
            if (values[i] != value) continue;
            selected[i] = !selected[i];
            return;
        }
        
        Debug.warn("Trying to toggle a value that does not exist.");
    }

    /// <summary>
    /// toggles a value by its index in <see cref="values"/> 
    /// </summary>
    /// <exception cref="SettingsArgumentException">index is out of bounds</exception>
    public void ToggleValue(int index) {
        if (index < 0 || index >= values.Length) 
            throw new SettingsArgumentException("Inputted index is not valid (out of bounds).");
        
        selected[index] = !selected[index];
    }

    public void SetParsedValue(object value) {
        throw new SettingsBuilderException("This method is not supported for MultiSelectArgumentType. " +
                                           "Use SelectValue(<string>, <bool>), SelectValue(<int>, <bool>)," +
                                           " ToggleValue, SelectValue or DeselectValue methods instead.");
    }

    public ArgumentType Clone() {
        MultiSelectArgumentType clone = type == null ? 
            new MultiSelectArgumentType(values) : 
            new MultiSelectArgumentType(type);

        for (int i = 0; i < selected.Length; i++) {
            clone.selected[i] = selected[i];
        }
        
        return clone;
    }
}