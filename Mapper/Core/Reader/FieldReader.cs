using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Settings;

public static class FieldReader
{
    public static EquatableArrayWrap<Field> From(IEnumerable<ISymbol> symbolList)
        => From(symbolList.Where(x => x is IPropertySymbol).Select(x => (x as IPropertySymbol)!));

    public static EquatableArrayWrap<Field> From(IEnumerable<IPropertySymbol> symbolList)
        => new([.. symbolList.Select(From).Where(x => x is not null)]);

    public static Field From(IPropertySymbol symbol)
        => new(symbol.Name, From(symbol.Type));

    public static FieldType From(ITypeSymbol symbol)
        => new(TypeIdReader.GetNamespace(symbol), TypeIdReader.GetName(symbol));
}
