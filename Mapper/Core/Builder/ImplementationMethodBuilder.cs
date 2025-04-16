using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Builder;
public static class ImplementationMethodBuilder
{
    public static EquatableArrayWrap<ImplementationMethod> Build(IEnumerable<ISymbol> symbolList)
        => Build(symbolList.Where(x => x is IMethodSymbol).Select(x => (x as IMethodSymbol)!));
    
    public static EquatableArrayWrap<ImplementationMethod> Build(IEnumerable<IMethodSymbol> symbolList)
        => new([.. symbolList.Select(Build).Where(x => x != default)]);

    public static ImplementationMethod Build(IMethodSymbol symbol)
    {
        if (symbol.Parameters.Length > 1)
        {
            var t = SymbolEqualityComparer.Default.Equals(symbol.ReturnType, symbol.Parameters[1].Type);

        }

        return new(symbol.Name, TypeBuilder.Build(symbol.ReturnType), ParameterBuilder.Build(symbol.Parameters));
    }


}

public record Test;
public record Test2 : Test;
