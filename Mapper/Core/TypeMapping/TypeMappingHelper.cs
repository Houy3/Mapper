using Mapper.Core.Entity;
using Mapper.Core.Reader;
using Mapper.Core.Settings;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Mapper.Attributes;

using static Mapper.Attributes.ImportTypeMappingsAttribute;
using Mapper.Core.Entity.Common;

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

            if (method.Parameters.Length != 1)
                continue;

            typeMappingMethodList.Add(new(
                typeNamespace,
                typeName,
                method.Name,
                TypeIdReader.From(method.Parameters[0].Type),
                TypeIdReader.From(method.ReturnType))
                );
        }

        return typeMappingMethodList;
    }

    public static EquatableArrayWrap<TypeMappingMethod> From(Interface @interface)
        => new([..
            @interface.MethodList.Array
                .Where(x => x.ParameterList.Array.Length == 1)
                .Select(x => new TypeMappingMethod(
                    @interface.Namespace,
                    @interface.Name,
                    x.Name,
                    x.ParameterList.Array[0].Type.ToTypeId(),
                    x.ReturnType.ToTypeId()))]);




    public static TypeMappingStorage BuildStorage(ImmutableArray<EquatableArrayWrap<TypeMappingMethod>> storage1, ImmutableArray<EquatableArrayWrap<TypeMappingMethod>> storage2)
        => new(new([.. storage1.Select(x => x.Array).SelectMany(x => x), .. storage2.Select(x => x.Array).SelectMany(x => x)]));

}
