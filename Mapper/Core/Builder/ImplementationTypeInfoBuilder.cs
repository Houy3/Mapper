using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Builder;

public static class ImplementationTypeInfoBuilder
{
    public static ImplementationType Build(ISymbol symbol)
        => symbol is INamedTypeSymbol targetSymbol ? Build(targetSymbol) : default;

    public static ImplementationType Build(INamedTypeSymbol symbol)
        => new(
            symbol.ContainingNamespace.ToDisplayString(),
            symbol.ToDisplayString(NullableFlowState.NotNull, new(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly))
            ,
            ImplementationMethodBuilder.Build(symbol.GetMembers())
            );
}
