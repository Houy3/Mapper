using Microsoft.CodeAnalysis;

namespace Mapper.Core.Entity;

public record TypeMappingMethod(
    TypeIdPair Key,
    TypeMappingMethodId Value)
{
    public TypeMappingMethod(ITypeSymbol typeSymbol, IMethodSymbol methodSymbol)
        : this(new TypeIdPair(methodSymbol), new(typeSymbol, methodSymbol)) { }

    public TypeMappingMethod(PlannedMapperType mapperType, MappingMethod mappingMethod)
        : this(new TypeIdPair(mappingMethod), new(mapperType, mappingMethod)) { }
}
