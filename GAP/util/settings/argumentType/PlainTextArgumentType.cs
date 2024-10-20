//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using GAP.util.exceptions;

namespace GAP.util.settings.argumentType;

/// <summary>
/// Plain Text Argument Type <br/>
/// not actually an argument type, returns a static text
/// </summary>
internal class PlainTextArgumentType : ArgumentType {

    private string text { get; }

    public PlainTextArgumentType(string text) {
        this.text = text;
    }
    
    public string GetInputType() {
        return "Empty";
    }

    public string GetValue() {
        return text;
    }

    public object GetParsedValue() {
        return text;
    }

    public void SetValue(string value) {
        throw new SettingsBuilderException("Cannot set empty argument type.");
    }

    public void SetParsedValue(object value) {
        throw new SettingsBuilderException("Cannot set empty argument type.");
    }

    public ArgumentType Clone() {
        return (ArgumentType)MemberwiseClone();
    }
}