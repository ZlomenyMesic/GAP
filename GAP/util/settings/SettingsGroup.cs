//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;
using GAP.util.settings.argumentType;

namespace GAP.util.settings;

/// <summary>
/// groups options of different inputs of settings  
/// </summary>
public sealed class SettingsGroup : ICloneable {
    public string name { get; private set; }
    public Context? outputContext { get; private init; } = null;
    private List<SettingsGroupOption>? options { get; set; } = null;
    public SettingsGroupOption[] Options => 
        options?.ToArray() ?? throw new SettingsBuilderException("No options have been set.");

    private Action<Context, Context>? onParse = null;
    
    public SettingsGroupOption this[string name] {
        get {
            if (options is null) {
                throw new SettingsBuilderException("No options have been set.");
            }
            
            foreach (var o in options) {
                if (o.name == name) return o;
            }
            
            throw new SettingsBuilderException($"Option with name '{name}' doesn't exist in group '{this.name}'.");
        }
    }

    /// <summary>
    /// creates a new instance of <see cref="SettingsGroup"/>
    /// </summary>
    /// <param name="name">name of the new group</param>
    /// <param name="outputContext">unified output context of the group</param>
    public static SettingsGroup New(string name, params (string name, ArgumentType argument)[] outputContext) {

        if (outputContext == null)
            throw new SettingsBuilderException($"Output context is not initialized for group '{name}'.");
        
        SettingsGroup sg = new SettingsGroup(name) {
            outputContext = new Context()
        };
        
        sg.outputContext.Add(outputContext);
        
        return sg;
    }

    public static SettingsGroup New(string name, Context outputContext) {
        if (outputContext is null) {
            throw new SettingsBuilderException($"Output context is not initialized for group '{name}'.");
        }
        
        SettingsGroup sg = new SettingsGroup(name) {
            outputContext = outputContext
        };
        
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
    /// sets the node context variables to group context variables 
    /// </summary>
    /// <param name="parse">parameter 1: group context, parameter 2: node context</param>
    public SettingsGroup OnParse(Action<Context, Context> parse) {
        onParse = parse;
        return this;
    }
    

    /// <summary>
    /// parses the arguments to a context
    /// </summary>
    /// <exception cref="SettingsBuilderException">
    /// no available options exist, no option with provided name exists
    /// </exception>
    public void Execute(string optionName, in Context outputContext) {
        
        if (options == null)
            throw new SettingsBuilderException($"No options have been set for group '{name}'.");

        if (onParse is null)
            throw new SettingsBuilderException($"No parsing method has been set for group '{name}'.");
        
        if (this.outputContext is null)
            throw new SettingsBuilderException($"Output context is not initialized for group '{name}'.");

        foreach (var o in options) {
            if (o.name == optionName) {
                o.Execute(outputContext);
                onParse(this.outputContext, outputContext);
                return;
            }
        }
        
        throw new SettingsBuilderException($"Option '{optionName}' does not exist in group '{name}'.");
    }

    /// <summary>
    /// whether all properties have been properly initialized
    /// </summary>
    public bool IsFullyInitialized() => outputContext is { Length: > 0 } && options is { Count: > 0 };
    
    /// <summary>
    /// sets the value of an argument of an option
    /// </summary>
    public void SetValue(string optionName, string argumentName, object value) {
        if (options is null) {
            throw new SettingsBuilderException($"No options have been set in group '{name}'.");
        }
        
        foreach (var o in options) {
            if (o.name == optionName) {
                o.SetValue(argumentName, value);
                return;
            }
        }
        
        throw new SettingsBuilderException($"No option named '{optionName}' found in group '{name}'.");
    }
    
    public object Clone() {
        SettingsGroup sg = new SettingsGroup(name) {
            onParse = onParse,
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