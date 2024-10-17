//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Text.Json;
using GAP.util.exceptions;
using GAP.util.settings.argumentType;

namespace GAP.util.settings;

/// <summary>
/// groups options of different inputs of settings  
/// </summary>
public sealed class SettingsGroup : ICloneable {
    public string name { get; private set; }
    public Context? outputContext { get; private init; } = null;
    public List<SettingsGroupOption>? options { get; private set; } = null;


    /// <summary>
    /// creates a new instance of <see cref="SettingsGroup"/>
    /// </summary>
    /// <param name="name">name of the new group</param>
    /// <param name="outputContext">unified output context of the group</param>
    public static SettingsGroup New(string name, params (string name, ArgumentType argument)[] outputContext) {

        if (outputContext == null || outputContext.Length == 0)
            throw new SettingsBuilderException($"Output context of group '{name}' is empty.");
        
        SettingsGroup sg = new SettingsGroup(name) {
            outputContext = new Context()
        };
        
        sg.outputContext.Add(outputContext);
        
        return sg;
    }
    
    /// <summary>
    /// basic constructor
    /// </summary>
    /// <param name="name">group name</param>
    /// <param name="outputContext">context that all available options must parse to</param>
    public SettingsGroup(string name, params (string name, ArgumentType argumentType)[] outputContext) {
        this.name = name;

        this.outputContext ??= new Context();
        
        foreach ((string name, ArgumentType argumentType) c in outputContext) {
            this.outputContext.Add(c.name, c.argumentType);
        }
    }
    
    /// <summary>
    /// private name constructor
    /// </summary>
    private SettingsGroup(string name) => this.name = name;

    /// <summary>
    /// adds an option  
    /// </summary>
    /// <param name="option">added option</param>
    public SettingsGroup Option(SettingsGroupOption option) {
        options ??= [];

        foreach (SettingsGroupOption o in options.Where(o => o.name == option.name)) {
            throw new SettingsBuilderException($"A settings option with the same ({o.name}) name already exists.");
        }
        
        options.Add(option);
        
        return this;
    }

    /// <summary>
    /// whether all properties have been properly initialized
    /// </summary>
    public bool IsFullyInitialized() => outputContext is { Length: > 0 } && options is { Count: > 0 };

    public object Clone() {
        SettingsGroup sg = new SettingsGroup(name) {
            outputContext = (Context?)outputContext?.Clone()
        };

        if (options == null) throw new SettingsBuilderException($"Options of group '{name}' cannot be uninitialized.");
        
        sg.options = new List<SettingsGroupOption>();
        sg.options.AddRange(options);
        
        return sg;
    }

    public override string ToString() {
        string output = $"{{\"name\": \"{name}\", \"outputContext\": [";

        if (outputContext != null) {
            for (int i = 0; i < outputContext.Length; i++) {
                var argument = outputContext.GetAtIndex(i); 
                output += $"{{\"name\": \"{argument.name}\", \"value\": {argument.value}}}" + 
                          (i == outputContext.Length - 1 ? "" : ", ");
            }
        }

        output += "], \"options\": [";
        
        if (options != null && options.Count != 0) {
            for (var i = 0; i < options.Count; i++) {
                output += options[i] + (i == options.Count - 1 ? "" : ", ");
            }
        }
        
        output += "]}";
        
        return output;
    }
}