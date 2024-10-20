//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;
using GAP.util.settings.argumentType;

namespace GAP.util.settings;

/// <summary>
/// Context Struct <br/>
/// provides context, argument types and values to settings builders
/// </summary>
public sealed class Context : ICloneable {
    private Dictionary<string, ArgumentType> arguments { get; } = new();

    public ArgumentType this[string key] {
        get {
            try {
                return arguments[key];
            }
            catch (KeyNotFoundException) {
                throw new SettingsBuilderException($"Context value '{key}' does not exist.");
            }
        }
    }

    public ArgumentType this[int index] => GetAtIndex(index).value;

    public Context(params (string name, ArgumentType argument)[] arguments) {
        Add(arguments);
    }
    
    public void Add(string name, ArgumentType argument) {
        if (!arguments.TryAdd(name, argument))
            throw new ContextException($"Context value with name '{name}' already exists.");
    }

    public void Add((string name, ArgumentType argument)[] arguments) {
        foreach ((string name, ArgumentType argument) a in arguments) {
            Add(a.name, a.argument);
        }
    }

    public void Add(Context context) {
        foreach ((string key, ArgumentType value) in context.arguments) {
            Add(key, value);
        }
    }
    
    public int Length => arguments.Count;
    
    public object Clone() {
        Context context = new Context();
        
        foreach ((string key, ArgumentType value) in arguments) {
            context.Add(key, value);
        }
        
        return context;
    }

    public override string ToString() {
        return string.Join(", ", arguments.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
    }

    public (string name, ArgumentType value) GetAtIndex(int index) {
        if (index >= Length) throw new ContextException($"Context at index {index} out of range.");
        return (arguments.Keys.ElementAt(index), arguments.Values.ElementAt(index));
    }

    public static Context New(params (string name, ArgumentType argument)[] arguments) {
        return new Context(arguments);
    }
}
