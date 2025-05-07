using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Reader;

public static class DataTypeReader
{
    public static DataType From(ITypeSymbol symbol)
        => new(
            TypeIdReader.GetNamespace(symbol),
            TypeIdReader.GetName(symbol),
            FieldReader.From(symbol));
}
