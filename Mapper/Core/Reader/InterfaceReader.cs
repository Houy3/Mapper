using Mapper.Attributes;
using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Settings;

public static class InterfaceReader
{
    public static Interface? From(ISymbol symbol)
        => symbol is INamedTypeSymbol targetSymbol ? From(targetSymbol) : null;

    public static Interface From(INamedTypeSymbol symbol)
        => new(
            TypeReader.GetNamespace(symbol),
            TypeReader.GetName(symbol),
            MethodReader.From(symbol.GetMembers()),
            SettingOverrideReader.From(symbol)
            );
    

}
