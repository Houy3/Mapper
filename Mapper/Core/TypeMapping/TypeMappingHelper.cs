using Mapper.Core.Entity;
using Mapper.Core.Reader;
using Mapper.Core.Settings;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

using static Mapper.Attributes.ImportTypeMappingsAttribute;
using Mapper.Core.Entity.Common;
using System.Collections.Generic;
using Mapper.Core.Builder;

namespace Mapper.Core.TypeMapping;

public static class TypeMappingHelper
{

    public static EquatableArrayWrap<TypeMappingMethod> From(ImmutableArray<AttributeData> attributeList)
        => From(NamedValueReader.From(attributeList.FirstOrDefault()));

    public static EquatableArrayWrap<TypeMappingMethod> From(NamedValue[] namedValueList)
    {
        var importList = namedValueList.FirstOrDefault(x => x.Name == ImportPropertyName)?.Value;
        if (importList is null || importList is not object[] objectArray)
            return new();

        return new([.. objectArray.Select(From).SelectMany(x => x)]);
    }


    public static List<TypeMappingMethod> From(object @object)
    {
        if (@object is not ITypeSymbol symbol)
            return [];

        var typeNamespace = TypeIdReader.GetNamespace(symbol);
        var typeName = TypeIdReader.GetName(symbol);

        var typeMappingMethodList = new List<TypeMappingMethod>();
        foreach (var member in symbol.GetMembers())
        {
            if (member is not IMethodSymbol method)
                continue;

            if (!method.IsStatic || method.Parameters.Length != 1)
                continue;

            typeMappingMethodList.Add(new(
                new(TypeIdReader.From(method.Parameters[0].Type), TypeIdReader.From(method.ReturnType)),
                new(typeNamespace, typeName, method.Name))
                );
        }

        return typeMappingMethodList;
    }

    public static EquatableArrayWrap<TypeMappingMethod> From(MapperType @interface)
        => new([..
            @interface.MethodList.Array
                .Where(x => x.ParameterList.Array.Length == 1)
                .Select(x => new TypeMappingMethod(
                    new(x.ParameterList.Array[0].Type.ToTypeId(), x.ReturnType.ToTypeId()),
                    new(@interface.Namespace, ImplementationBuilder.GetImplementationName(@interface.Name), x.Name)))]);



    public static TypeMappingStorage BuildStorage(
        ImmutableArray<EquatableArrayWrap<TypeMappingMethod>> outsideTypeMappingList, 
        ImmutableArray<EquatableArrayWrap<TypeMappingMethod>> insideTypeMappingList,
        CancellationToken cancellationToken)
    {
        var dictionary = new Dictionary<TypeIdPair, TypeMappingMethodId>();

        AddTypeMappingListInStorage(dictionary, outsideTypeMappingList, cancellationToken);
        AddTypeMappingListInStorage(dictionary, insideTypeMappingList, cancellationToken);

        return new(new(dictionary));
    }


    public static void AddTypeMappingListInStorage(
        Dictionary<TypeIdPair, TypeMappingMethodId> storage,
        ImmutableArray<EquatableArrayWrap<TypeMappingMethod>> typeMappingList,
        CancellationToken cancellationToken)
    {
        foreach (var typeMappingMethod in typeMappingList.SelectMany(x => x.Array))
        {
            cancellationToken.ThrowIfCancellationRequested();
            storage[typeMappingMethod.Key] = typeMappingMethod.Value;
        }
    }

}
