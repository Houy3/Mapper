using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record DataType(
    string Namespace,
    string Name,
    EquatableArrayWrap<Field> FieldList)
    : TypeId(Namespace, Name)
{

    public TypeId ToTypeId()
        => new(Namespace, Name);
}

public record Field(
    string Name,
    FieldType Type);