//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using System.Text.Json;
using GAP.util.exceptions;
using GAP.util.settings.argumentType;
using Kolors;

namespace GAP.util.settings;

/// <summary>
/// single option of input method of settings
/// </summary>
public sealed class SettingsGroupOption : ICloneable {
    public string name { get; }
    public Context context { get; init; } = new();
    public Action<Context, Context>? parseContext { get; private set; } = null;

    
    /// <summary>
    /// creates a new instance
    /// </summary>
    public static SettingsGroupOption New(string name) {
        return new SettingsGroupOption(name);
    }
    
    private SettingsGroupOption(string name) => this.name = name;

    /// <summary>
    /// adds a new argument
    /// </summary>
    /// <param name="name">name of the argument</param>
    /// <param name="argument">argument type</param>
    public SettingsGroupOption Argument(string name, ArgumentType argument) {
        context.Add(name, argument);
        return this;
    }

    /// <summary>
    /// describes how to convert arguments from this option to shared group context
    /// </summary>
    /// <param name="parse">parameter 1: option context, parameter 2: shared group context</param>
    public SettingsGroupOption OnParse(Action<Context, Context> parse) {
        parseContext = parse;
        return this;
    }

    /// <summary>
    /// parses the option context into the shared group context  
    /// </summary>
    /// <exception cref="SettingsBuilderException">
    /// no parsing delegate has been provided through the <see cref="OnParse"/> method
    /// </exception>
    public void Execute(in Context context) {
        
        if (parseContext == null) {
            throw new SettingsBuilderException(
                "Parsing delegate has not been provided. Use OnParse(Action<Context,Context> parse) method to do so.");
        }
        
        parseContext(this.context, context);
    }

    /// <summary>
    /// whether the option is fully initialized
    /// </summary>
    public bool IsFullyInitialized() {
        return parseContext != null;
    }
    
    /// <summary>
    /// sets the value of an argument
    /// </summary>
    public void SetValue(string argumentName, object value) {
        context[argumentName].SetParsedValue(value);
    }
    
    public object Clone() {
        SettingsGroupOption sgo = new SettingsGroupOption(name) {
            parseContext = parseContext,
            context = (Context)context.Clone()
        };
        
        return sgo;
    }

    public override string ToString() {
        string output = $"{{\"name\": \"{name}\", \"context\": [";

        for (int i = 0; i < context.Length; i++) {
            (string name, ArgumentType value) argument = context.GetAtIndex(i); 
            output += $"{{\"name\": \"{argument.name}\", \"value\": {argument.value}}}" + 
                      (i == context.Length - 1 ? "" : ", ");
        }
        
        output += "]}";
        
        return output;
    }
}