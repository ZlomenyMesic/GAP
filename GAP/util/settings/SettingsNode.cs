//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;
using GAP.util.settings.argumentType;

namespace GAP.util.settings;

/// <summary>
/// Settings Node Class <br/>
/// single settings node object
/// </summary>
/// <typeparam name="TResult">result class</typeparam>
public class SettingsNode<TResult> : ICloneable where TResult : SettingsObject {
    
    public string name { get; }
    public Context context { get; private init; } = new();
    public List<SettingsGroup>? groups { get; private set; } = null;
    
    
    /// <summary>
    /// creates a new instance
    /// </summary>
    public static SettingsNode<TResult> New(string name) {
        return new SettingsNode<TResult>(name);
    }

    /// <summary>
    /// constructor, checks if type has a parameterless constructor
    /// </summary>
    /// <exception cref="InvalidTypeException">
    /// no parameterless constructor available for <see cref="TResult"/>
    /// </exception>
    private SettingsNode(string name) {
        if (typeof(TResult).GetConstructors().All(c => c.GetParameters().Length != 0))
            throw new InvalidTypeException($"Cannot create settings node for type {typeof(TResult).Name}. " +
                                           $"Class must have a public parameterless constructor.");
        
        this.name = name;
    }
    
    
    /// <summary>
    /// adds an argument to the settings node
    /// </summary>
    /// <param name="argumentName">name of the argument</param>
    /// <param name="argumentType">type of the argument</param>
    public SettingsNode<TResult> Argument(string argumentName, ArgumentType argumentType) {
        context.Add(argumentName, argumentType);
        return this;
    }


    /// <summary>
    /// creates a new group of settings
    /// </summary>
    /// <param name="group">added group option</param>
    public SettingsNode<TResult> Group(SettingsGroup group) {
        groups ??= [];

        if (!group.IsFullyInitialized())
            throw new SettingsBuilderException($"Group '{group}' in '{name}' is not fully initialized.");
        
        if (groups.Any(g => g.name == group.name)) {
            throw new SettingsBuilderException($"Group '{group.name}' in '{name}' already exists.");
        }
        
        groups.Add(group);
        context.Add(group.outputContext ?? 
                    throw new SettingsBuilderException($"Output context of group '{group.name}' in '{name}' is null."));

        return this;
    }
    

    /// <summary>
    /// parses the arguments to an object of type <see cref="TResult"/>
    /// </summary>
    /// <returns>an instance of the <see cref="TResult"/> type</returns>
    public TResult Execute((string name, string value)[] input) {

        (string name, object value)[] output = new (string, object)[input.Length];

        for (int i = 0; i < input.Length; i++) {
            output[i].name = input[i].name;
            output[i].value = Arguments.Parse(input[i].value, context[input[i].name]);
        }
        
        TResult result = Activator.CreateInstance<TResult>();
        result.Deserialize(output);
        
        return result;
    }

    public object Clone() {
        SettingsNode<TResult> clone = new SettingsNode<TResult>(name) {
            context = (Context)context.Clone()
        };

        if (groups == null) return clone;
        
        clone.groups = [];
        foreach (SettingsGroup g in groups) {
            clone.groups.Add((SettingsGroup)g.Clone());
        }
        
        return clone;
    }

    public override string ToString() {
        string output = $"{{\"name\": \"{name}\", \"arguments\": [";

        for (int i = 0; i < context.Length; i++) {
            var argument = context.GetAtIndex(i); 
            output += $"{{\"name\": \"{argument.name}\", \"value\": {argument.value}}}" + 
                      (i == context.Length - 1 ? "" : ", ");
        }

        output += "], \"groups\": [";

        if (groups == null) {
            output += "]";
            return output;
        }
        
        for (int i = 0; i < groups.Count; i++) {
            output += groups[i] + (i == groups.Count - 1 ? "" : ", ");
        }

        output += "]}";

        return output;
    }
}