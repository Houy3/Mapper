using System.Collections.Immutable;
using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Settings;

public static class VariableReader
{
    public static EquatableArrayWrap<Variable> From(ImmutableArray<IParameterSymbol> symbolList)
        => new([.. symbolList.Select(From)]);

    public static Variable From(IParameterSymbol symbol)
        => new(symbol.Name, DataTypeReader.From(symbol.Type));

}
