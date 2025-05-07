namespace Mapper.Core.Entity;

//todo
public record FieldType(
    string Namespace,
    string Name)
    : TypeId(Namespace, Name);