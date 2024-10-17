//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Text.Json;
using GAP.util.exceptions;

namespace GAP.util.settings;

/// <summary>
/// Settings Builder <br/>
/// builds settings for generator (or other) classes 
/// </summary>
/// <typeparam name="TResult">settings result class</typeparam>
public class SettingsBuilder<TResult> : ICloneable where TResult : SettingsObject {

    public string name { get; }
    public SettingsNode<TResult>[]? nodes { get; private set; } = null;
    
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
        this.nodes = nodes;
    }

    /// <summary>
    /// converts the settings to an object of type <see cref="TResult"/>
    /// </summary>
    /// <param name="nodeName">name of the node</param>
    /// <param name="arguments">arguments</param>
    /// <returns>a new object of type <see cref="TResult"/></returns>
    /// <exception cref="SettingsBuilderException">
    /// no settings have been configured,
    /// no matching settings node was found
    /// </exception>
    public TResult Execute(string nodeName, (string name, string value)[] arguments) {
        
        if (nodes == null) 
            throw new SettingsBuilderException("Cannot execute settings before building them.");
        
        if (nodes.Length == 0) 
            throw new SettingsBuilderException("No settings nodes have been configured.");
        
        
        SettingsNode<TResult>? executeNode = null;
        
        foreach (var n in nodes) {
            if (n.name == nodeName) {
                executeNode = n;
                break;
            }
        }

        if (executeNode == null) {
            throw new SettingsBuilderException("No matching settings node found.");
        }

        return executeNode.Execute(arguments);
    }

    public object Clone() {
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
        
        for (int i = 0; i < nodes.Length; i++) {
            output += "" + nodes[i] + (i == nodes.Length - 1 ? "" : ", ");
        }
        
        output += "]}";
        
        return output;
    }
}