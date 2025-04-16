using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Builder;

public static class ImplementationInfoBuilder
{
    public static Implementation Build(ISymbol symbol)
        => symbol is INamedTypeSymbol targetSymbol ? Build(targetSymbol) : default;

    public static Implementation Build(INamedTypeSymbol symbol)
    {
        var @namespace = symbol.ContainingNamespace.ToDisplayString();

        var interfaceName = GetInterfaceName(symbol);
        var className = GetClassName(interfaceName);

        foreach (var memberSymbol in symbol.GetMembers())
        {
            if (memberSymbol is not IMethodSymbol methodSymbol)
                continue;

            var methodName = methodSymbol.Name;

            var returnType = methodSymbol.ReturnType;

            var parameters = methodSymbol.Parameters;
            var methodParam1 = 1;
        }

        var result = new Implementation(@namespace, interfaceName, className, new([]));

        return result;
    }

    private static string GetInterfaceName(INamedTypeSymbol symbol)
        => symbol.ToDisplayString(NullableFlowState.NotNull, new(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly));

    private static string GetClassName(string interfaceName)
        => interfaceName.StartsWith("I") ? interfaceName.Substring(1) : interfaceName + "Impl";



}
