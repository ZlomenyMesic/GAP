//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;
using Kolors;

namespace GAP.util.settings;

/// <summary>
/// Settings Builder <br/>
/// builds settings for generator (or other) classes 
/// </summary>
/// <typeparam name="TResult">settings result class</typeparam>
public class SettingsBuilder<TResult> {

    public string name { get; }
    public List<SettingsNode<TResult>>? nodes { get; private set; } = null;
    private bool isEmpty = false;


    public SettingsNode<TResult> this[string name] {
        get {
            if (nodes is null) {
                throw new SettingsBuilderException($"No nodes have been set in builder '{name}'");
            }

            foreach (var n in nodes) {
                if (n.name == name) {
                    return n;
                }
            }
            
            throw new SettingsBuilderException($"No node with name '{name}' exists in the builder '{name}'");
        }
    } 
    
    
    /// <summary>
    /// constructor, checks if type has a parameterless constructor
    /// </summary>
    /// <exception cref="InvalidTypeException">
    /// no parameterless constructor available for <see cref="TResult"/>
    /// </exception>
    public SettingsBuilder(string name) {
        if (typeof(TResult).GetConstructors().All(c => c.GetParameters().Length != 0))
            throw new InvalidTypeException($"Cannot create settings node for type {typeof(TResult).Name}. " +
                                           $"Class must have a public parameterless constructor.");
        
        this.name = name;
    }
    
    /// <summary>
    /// adds new settings nodes to the builder
    /// </summary>
    /// <param name="nodes">node creation actions</param>
    public void Build(params SettingsNode<TResult>[] nodes) {
        
        this.nodes ??= nodes.ToList();

        this.nodes.AddRange(nodes);
    }

    
    /// <summary>
    /// creates a new <see cref="SettingsBuilder{TResult}"/> with nodes and returns it
    /// </summary>
    public static SettingsBuilder<TResult> Build(string name, params SettingsNode<TResult>[] nodes) {
        SettingsBuilder<TResult> builder = new SettingsBuilder<TResult>(name) {
            nodes = nodes.ToList()
        };
        
        return builder;
    }

    /// <summary>
    /// converts the settings to an object of type <see cref="TResult"/>
    /// </summary>
    /// <exception cref="SettingsBuilderException">
    /// no settings have been configured,
    /// no matching settings node was found
    /// </exception>
    public TResult Execute(string nodeName, params (string groupName, string optionName)[] selectedOptions) {
        if (isEmpty)
            throw new SettingsBuilderException("SettingsBuilder is empty. Cannot execute.");
        
        if (nodes == null) 
            throw new SettingsBuilderException("Cannot execute settings before building them.");
        
        if (nodes.Count == 0) 
            throw new SettingsBuilderException("No settings nodes have been configured.");
        
        foreach (var n in nodes) {
            if (n.name == nodeName) {
                return n.Execute(selectedOptions);
            }
        }

        throw new SettingsBuilderException("No matching settings node found.");
    }

    /// <summary>
    /// creates a new empty <see cref="SettingsBuilder{TResult}"/>, <b>ONLY for FALLBACK values!</b>
    /// </summary>
    public static SettingsBuilder<T> Empty<T>(string name) {
        Debug.warn($"Empty '{name}' settings were created.");
        
        var empty = new SettingsBuilder<T>(name) {
            isEmpty = true
        };
        empty.Build(SettingsNode<T>.Empty<T>());
        
        return empty;
    }
    

    public SettingsBuilder<TResult> Clone() {
        SettingsBuilder<TResult> clone = new SettingsBuilder<TResult>(name);
        
        if (nodes == null) return clone;
        
        foreach (SettingsNode<TResult> n in nodes) {
            clone.Build((SettingsNode<TResult>)n.Clone());
        }

        return clone;
    }

    public override string ToString() {
        string output = $"{{\"name\": \"{name}\", \"nodes\": [";

        if (nodes == null) {
            output += "]}";
            return output;
        }
        
        for (int i = 0; i < nodes.Count; i++) {
            output += "" + nodes[i] + (i == nodes.Count - 1 ? "" : ", ");
        }
        
        output += "]}";
        
        return output;
    }
}