using System.Collections.Immutable;
using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Reader;

public static class VariableReader
{
    public static Variable[] From(ImmutableArray<IParameterSymbol> symbolList)
        => [.. symbolList.Select(From)];

    public static Variable From(IParameterSymbol symbol)
        => new(symbol.Name, DataTypeReader.From(symbol.Type));

}
