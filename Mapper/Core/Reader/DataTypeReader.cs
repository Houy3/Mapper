using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Settings;

public static class DataTypeReader
{
    public static DataType From(ITypeSymbol symbol)
        => new(
            TypeReader.GetNamespace(symbol),
            TypeReader.GetName(symbol),
            PropertyReader.From(symbol.GetMembers()));
}
