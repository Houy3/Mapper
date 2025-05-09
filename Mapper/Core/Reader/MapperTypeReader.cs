using Mapper.Core.Entity;
using Mapper.Core.Settings;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Reader;

public static class MapperTypeReader
{
    public static MapperType? ReadMapperType(this ISymbol symbol)
        => symbol is ITypeSymbol targetSymbol ? From(targetSymbol) : default;

    public static MapperType From(ITypeSymbol symbol)
        => new(
            TypeIdReader.GetNamespace(symbol),
            TypeIdReader.GetName(symbol),
            MethodReader.From(symbol),
            SettingOverrideReader.From(symbol));
}
