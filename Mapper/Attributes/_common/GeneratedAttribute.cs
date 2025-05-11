namespace Mapper.Attributes;

public abstract class GeneratedAttribute
{
    public const string Namespace = "Mapper";

    public abstract string Name { get; }

    public string FullName => Namespace + "." + Name;

    public abstract string Text { get; }
}
