using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

using static Mapper.Attributes.ImportTypeMappingsAttribute;
using static Mapper.Core.Entity.MethodDetails;
using Mapper.Core.Entity.Common;

namespace Mapper.Core.Reader;

public static class TypeMappingReader
{

    public static EquatableArrayWrap<TypeMappingMethod> ReadExternalTypeMappingList(this ImmutableArray<AttributeData> attributeList)
        => From(NamedValueReader.From(attributeList.FirstOrDefault()));

    private static EquatableArrayWrap<TypeMappingMethod> From(IEnumerable<NamedValue> namedValueList)
    {
        var importList = namedValueList.FirstOrDefault(x => x.Name == ImportPropertyName)?.Value;
        if (importList is null || importList is not object[] objectArray)
            return new();

        return new(objectArray.SelectMany(From));
    }

    private static IEnumerable<TypeMappingMethod> From(object @object)
    {
        if (@object is not ITypeSymbol symbol)
            yield break;

        var typeMappingMethodList = new List<TypeMappingMethod>();
        foreach (var methodSymbol in symbol.GetMembers().OfType<IMethodSymbol>())
        {
            if (!methodSymbol.IsStatic || methodSymbol.Parameters.Length != 1)
                continue;

            yield return new(symbol, methodSymbol);
        }
    }


    public static EquatableArrayWrap<TypeMappingMethod> FindTypeMappingMethodList(this PlannedMapperType mapperType)
        => new(mapperType.MappingMethodList
                .Where(x => x.Is(Static) && x.ParameterList.Length == 1)
                .Select(x => new TypeMappingMethod(mapperType, x)));



    public static TypeMappingStorage CombineInStorage(
        this ImmutableArray<TypeMappingMethod> outsideTypeMappingList, 
        ImmutableArray<TypeMappingMethod> insideTypeMappingList,
        CancellationToken cancellationToken)
    {
        var dictionary = new Dictionary<TypeIdPair, TypeMappingMethodId>();

        AddTypeMappingListInStorage(dictionary, outsideTypeMappingList, cancellationToken);
        AddTypeMappingListInStorage(dictionary, insideTypeMappingList, cancellationToken);

        return new(new(dictionary));
    }

    public static void AddTypeMappingListInStorage(
        Dictionary<TypeIdPair, TypeMappingMethodId> storage,
        ImmutableArray<TypeMappingMethod> typeMappingList,
        CancellationToken cancellationToken)
    {
        foreach (var typeMappingMethod in typeMappingList)
        {
            cancellationToken.ThrowIfCancellationRequested();
            storage[typeMappingMethod.Key] = typeMappingMethod.Value;
        }
    }

}
