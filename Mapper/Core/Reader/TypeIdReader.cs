using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Reader;

public static class TypeIdReader
{
    public static TypeId From(ITypeSymbol symbol)
        => new(GetNamespace(symbol), GetName(symbol));

    public static string GetNamespace(ISymbol symbol)
        => symbol.ContainingNamespace.ToDisplayString();

    public static string GetName(ITypeSymbol symbol)
        => symbol.ToDisplayString(NullableFlowState.NotNull, 
            new(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly, 
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters));

}
