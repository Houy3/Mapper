using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record DataType(
    string Namespace,
    string Name,
    EquatableArrayWrap<Field> FieldList)
{
    public string FullName => Namespace + "." + Name;
}
