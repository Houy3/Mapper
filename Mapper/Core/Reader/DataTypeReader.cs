using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Settings;

public static class DataTypeReader
{
    public static DataType From(ITypeSymbol symbol)
        => new(
            TypeIdReader.GetNamespace(symbol),
            TypeIdReader.GetName(symbol),
            FieldReader.From(symbol.GetMembers()));
}
