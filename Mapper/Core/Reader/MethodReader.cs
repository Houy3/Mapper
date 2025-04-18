using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Reader;

public static class MethodReader
{
    public static EquatableArrayWrap<Method> From(IEnumerable<ISymbol> symbolList)
        => From(symbolList.Where(x => x is IMethodSymbol).Select(x => (x as IMethodSymbol)!));
    
    public static EquatableArrayWrap<Method> From(IEnumerable<IMethodSymbol> symbolList)
        => new([.. symbolList.Select(From).Where(x => x is not null)]);

    public static Method From(IMethodSymbol symbol)
        => new(symbol.Name, DataTypeReader.From(symbol.ReturnType), ParameterReader.From(symbol.Parameters));
    
}