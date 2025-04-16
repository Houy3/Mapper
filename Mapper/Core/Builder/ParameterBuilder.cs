using System.Collections.Immutable;
using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Builder;

public static class ParameterBuilder
{
    public static EquatableArrayWrap<Parameter> Build(ImmutableArray<IParameterSymbol> symbolList)
        => new([.. symbolList.Select(Build)]);

    public static Parameter Build(IParameterSymbol symbol)
        => new(symbol.Name, TypeBuilder.Build(symbol.Type));
}
