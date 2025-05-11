using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record DataType(
    string Namespace,
    string Name,
    EquatableArrayWrap<Field> FieldList)
    : TypeId(Namespace, Name)
{

    public EquatableDictionaryWrap<string, Field> FieldDictionary = new(FieldList.ToDictionary(x => x.Name, x => x));

    public TypeId ToTypeId()
        => new(Namespace, Name);
}

public record Field(
    string Name,
    FieldType Type);

public record FieldPair(
    Field SourceField,
    Field DestinationField);