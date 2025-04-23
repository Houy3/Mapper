using Microsoft.CodeAnalysis;

namespace Mapper.Core.Settings;

public static class TypeReader
{
    public static string GetName(ITypeSymbol symbol)
        => symbol.ToDisplayString(NullableFlowState.NotNull, new(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly));

    public static string GetNamespace(ITypeSymbol symbol)
        => symbol.ContainingNamespace.ToDisplayString();
}
