//
// GAP - Generative Art Producer
//   by ZlomenyMesic & KryKom
//

using Kolors.console;

namespace GAP.util.settings;

public interface ISettingsBuilder<out TResult, TSource> {
    public void Build(params ISettingsNode<TSource, TSource>[] nodes);
    public TResult Execute(string nodeName, params (string groupName, string optionName)[] selectedOptions);
    public ISettingsBuilder<TResult, TSource> Clone();
    
    /// <summary>
    /// creates a new empty <see cref="SettingsBuilder{TResult}"/>, <b>ONLY for FALLBACK values!</b>
    /// </summary>
    public static SettingsBuilder<TResult> Empty(string name) {
        Debug.Warn($"Empty '{name}' settings were created.");
        var empty = new SettingsBuilder<TResult>(name);
        empty.Build(ISettingsNode<TResult, TResult>.Empty<TResult>());
        return empty;
    }
}