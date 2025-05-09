using Microsoft.CodeAnalysis;

namespace Mapper.Core.Reader;

public static class ISymbolExtension
{
    public static bool IsPublic(this ISymbol symbol)
        => symbol.DeclaredAccessibility == Accessibility.Public;
    public static bool IsPrivate(this ISymbol symbol)
        => symbol.DeclaredAccessibility == Accessibility.Private;
}
