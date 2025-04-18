using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Reader;

public static class InterfaceReader
{
    public static Interface? From(ISymbol symbol)
        => symbol is INamedTypeSymbol targetSymbol ? From(targetSymbol) : null;

    public static Interface From(INamedTypeSymbol symbol)
        => new(
            DataTypeReader.GetNamespace(symbol),
            DataTypeReader.GetName(symbol),
            MethodReader.From(symbol.GetMembers())
            );
}
