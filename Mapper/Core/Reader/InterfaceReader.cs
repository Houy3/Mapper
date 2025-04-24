using Mapper.Attributes;
using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Settings;

public static class InterfaceReader
{
    public static Interface? From(ISymbol symbol)
        => symbol is ITypeSymbol targetSymbol ? From(targetSymbol) : null;

    public static Interface From(ITypeSymbol symbol)
        => new(
            TypeIdReader.GetNamespace(symbol),
            TypeIdReader.GetName(symbol),
            MethodReader.From(symbol.GetMembers()),
            SettingOverrideReader.From(symbol));
}
