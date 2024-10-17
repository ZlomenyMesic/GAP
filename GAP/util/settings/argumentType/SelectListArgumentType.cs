//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;

namespace GAP.util.settings.argumentType;

/// <summary>
/// Selection List Argument Type <br/>
/// lets you add items to a list from multiple options
/// </summary>
public class SelectListArgumentType : ArgumentType {
    
    private int maxItemCount { get; }
    private Type? type { get; }
    private string[] values { get; }
    private List<int> selectedIndexes { get; set; }

    /// <summary>
    /// constructor that sources values from an enum
    /// </summary>
    /// <param name="type">source enum type</param>
    /// <param name="maxItemCount">maximum list item count</param>
    /// <exception cref="TypeNotEnumException"></exception>
    public SelectListArgumentType(Type type, int maxItemCount = Int32.MaxValue) {
        if (maxItemCount < 1) {
            throw new SettingsBuilderException("Maximal item count cannot be less than 1.");
        }
        
        this.type = type;
        this.maxItemCount = maxItemCount;
        try {
            values = Enum.GetNames(type);
        }
        catch (ArgumentException) {
            throw new TypeNotEnumException(type);
        }
        selectedIndexes = new List<int>();
    }

    public SelectListArgumentType(string[] values, int maxItemCount = Int32.MaxValue) {
        if (maxItemCount < 1) {
            throw new SettingsBuilderException("Maximal item count cannot be less than 1.");
        }
        
        this.values = values;
        this.maxItemCount = maxItemCount;
        type = null;
        selectedIndexes = new List<int>();
    }
    
    public string GetInputType() {
        return "SelectList";
    }

    public string GetValue() {
        string output = "[";

        for (int i = 0; i < selectedIndexes.Count; i++) {
            output += values[selectedIndexes[i]] + (i == selectedIndexes.Count - 1 ? "" : ", ");
        }
        
        output += "]";
        
        return output;
    }

    public object GetParsedValue() {
        List<object> parsedValues = new List<object>();

        if (type == null) {
            foreach (int i in selectedIndexes) {
                parsedValues.Add(values[i]);
            }
            
            return (string[])parsedValues.ToArray();
        }

        foreach (int i in selectedIndexes) {
            parsedValues.Add(Enum.Parse(type, values[i]));
        }
        
        return parsedValues.ToArray();
    }

    public void SetValue(string value) {
        throw new SettingsBuilderException("This method is not supported for SelectListArgumentType. " +
                                           "Use AddValue or RemoveValue methods instead.");
    }

    public void SetParsedValue(object value) {
        throw new SettingsBuilderException("This method is not supported for SelectListArgumentType. " +
                                           "Use AddValue or RemoveValue methods instead.");
    }

    /// <summary>
    /// adds a new value to the list using the values actual name
    /// </summary>
    /// <exception cref="SettingsArgumentException">no matching value found</exception>
    public void AddValue(string value) {
        for (int i = 0; i < value.Length; i++) {
            if (values[i] == value) {
                selectedIndexes.Add(i);
            }
        }

        throw new SettingsArgumentException("Inputted value name is not valid.");
    }

    /// <summary>
    /// adds a new value to the list using an index pointing to the <see cref="values"/> array
    /// </summary>
    /// <exception cref="SettingsArgumentException">index is out of bounds</exception>
    public void AddValue(int index) {
        if (index < 0 || index >= values.Length)
            throw new SettingsArgumentException("Inputted index is not valid (out of bounds).");
        
        selectedIndexes.Add(index);
    }

    /// <summary>
    /// removes a value from the selected items using an index pointing to the list
    /// </summary>
    public void RemoveValue(int index) {
        selectedIndexes.RemoveAt(index);
    }

    public ArgumentType Clone() {
        SelectListArgumentType clone = type == null ? 
            new SelectListArgumentType(values, maxItemCount) : 
            new SelectListArgumentType(type, maxItemCount);

        foreach (int i in selectedIndexes) {
            clone.selectedIndexes.Add(i);
        }
        
        return clone;
    }
}