namespace Mapper.Core.Entity;

public record Field(
    string Name,
    FieldType Type);

//todo
public record FieldType(
    string Namespace,
    string Name)
    : TypeId(Namespace, Name);