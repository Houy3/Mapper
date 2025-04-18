namespace Mapper.Core.Entity;

public record Field(
    string Name,
    FieldType Type);

public record FieldType(
    string Namespace,
    string Name);