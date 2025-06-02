namespace Mapper.Core.Entity;

//todo
public record FieldType(
    string Namespace,
    string Name,
    bool IsNullable)
    : TypeId(Namespace, Name);