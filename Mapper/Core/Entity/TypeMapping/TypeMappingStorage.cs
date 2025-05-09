using Mapper.Core.Entity.Common;
using Mapper.Core.Reader;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Entity;

public record TypeMappingStorage(
    EquatableDictionaryWrap<TypeIdPair, TypeMappingMethodId> TypeMappingList)
{
    public TypeMappingStorage() : this(new EquatableDictionaryWrap<TypeIdPair, TypeMappingMethodId>()) { }

    public bool IsEmpty()
        => TypeMappingList.Count == 0;
}

public record TypeIdPair(
    TypeId Source,
    TypeId Destination)
{
    public TypeIdPair(IMethodSymbol symbol)
        : this(TypeIdReader.From(symbol.Parameters[0].Type), TypeIdReader.From(symbol.ReturnType)) { }

    public TypeIdPair(MappingMethod method)
        : this(method.ParameterList[0].Type, method.ReturnType) { }
}

public record TypeMappingMethodId(
    TypeId Class,
    string Name)
{
    public TypeMappingMethodId(ITypeSymbol typeSymbol, IMethodSymbol methodSymbol)
        : this(TypeIdReader.From(typeSymbol), methodSymbol.Name) { }

    public TypeMappingMethodId(PlannedMapperType mapperType, MappingMethod mappingMethod)
        : this(new TypeId(mapperType.Namespace, mapperType.Name), mappingMethod.Name) { }
}

