using Microsoft.CodeAnalysis;

namespace Mapper.Core.Builder;

public static class TypeBuilder
{
    public static Entity.Type Build(ITypeSymbol symbol)
    {

        return new();
    }
}
