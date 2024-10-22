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
public class SettingsNode<TResult> : ICloneable {
    
    public string name { get; }
    public Context context { get; private set; } = new();
    public List<SettingsGroup>? groups { get; private set; } = null;
    private Func<Context, TResult>? executionDelegate = null;
    private bool isEmpty = false;


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
    /// sets the <see cref="executionDelegate"/> that creates a
    /// <see cref="TResult"/> instance from <see cref="context"/>
    /// </summary>
    public SettingsNode<TResult> OnParse(Func<Context, TResult> execute) {
        executionDelegate = execute;
        return this;
    }


    /// <summary>
    /// creates a new empty <see cref="SettingsNode{TResult}"/>, <b>ONLY for FALLBACK values!</b> 
    /// </summary>
    internal static SettingsNode<T> Empty<T>() {
        SettingsNode<T> empty = SettingsNode<T>.New("empty");
        empty.isEmpty = true;
        empty.Argument("empty", Arguments.
            PlainText("This node is empty. " +
                      "For more information watch this video: https://youtu.be/dQw4w9WgXcQ?si=7aQZOGrCBTLmLRih"));
        
        return empty;
    }
    

    /// <summary>
    /// parses the arguments to an object of type <see cref="TResult"/>
    /// </summary>
    public TResult Execute(params (string groupName, string optionName)[] config) {
        if (isEmpty)
            throw new SettingsBuilderException("Node is empty. Cannot execute.");
        
        if (executionDelegate is null)
            throw new SettingsBuilderException("Execution delegate is not set.");

        if (groups is null || groups.Count == 0)
            return executionDelegate(context);
        
        foreach (SettingsGroup group in groups) {
            foreach ((string groupName, string optionName) c in config) {
                if (group.name != c.groupName) continue;
                
                group.Execute(c.optionName, context);
            }
        }

        return executionDelegate(context);
    }


    /// <summary>
    /// sets the value of an argument
    /// </summary>
    public void SetValue(string argumentName, object value) {
        context[argumentName].SetParsedValue(value);
    }


    public void SetGroupOptionValue(string groupName, string optionName, string argumentName, object value) {
        if (groups is null) {
            throw new SettingsBuilderException("No groups have been set.");
        }
        
        foreach (var g in groups) {
            if (g.name == groupName) {
                g[optionName].SetValue(argumentName, value);
                return;
            }
        }
    }
    

    public object Clone() {
        SettingsNode<TResult> clone = new SettingsNode<TResult>(name) {
            context = (Context)context.Clone(),
            executionDelegate = executionDelegate
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
            output += "]}";
            return output;
        }
        
        for (int i = 0; i < groups.Count; i++) {
            output += groups[i] + (i == groups.Count - 1 ? "" : ", ");
        }

        output += "]}";

        return output;
    }
}