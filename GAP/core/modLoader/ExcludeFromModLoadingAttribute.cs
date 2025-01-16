namespace GAP.core.modLoader;

[AttributeUsage(AttributeTargets.Class)]
public class ExcludeFromModLoadingAttribute : Attribute {
    public readonly bool Exclude;

    public ExcludeFromModLoadingAttribute(bool exclude = true) {
        Exclude = exclude;
    }
}