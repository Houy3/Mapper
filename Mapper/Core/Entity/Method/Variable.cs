namespace Mapper.Core.Entity;

public record Variable(
    string Name,
    DataType Type)
{
    public override string ToString()
        => Type.FullName + " " + Name;
}
