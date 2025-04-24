namespace Mapper.Core.Entity;

public record TypeId(
    string Namespace,
    string Name)
{
    public string FullName => Namespace + "." + Name;
}

