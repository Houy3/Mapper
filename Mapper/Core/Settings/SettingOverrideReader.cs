using Mapper.Attributes;
using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Mapper.Core.Settings;

public static class SettingOverrideReader
{
    public static EquatableArrayWrap<SettingOverride> From(ISymbol symbol)
        => From(symbol.GetAttributes());

    public static EquatableArrayWrap<SettingOverride> From(ImmutableArray<AttributeData> attributeList)
        => From(FindSettingOverrideAttribyte(attributeList));

    public static AttributeData? FindSettingOverrideAttribyte(ImmutableArray<AttributeData> attributeList)
    {
        foreach (var attribute in attributeList)
        {
            var attributeType = attribute.AttributeClass;
            if (attributeType is null)
                continue;

            var attributeTypeName = TypeReader.GetName(attributeType);
            if (TypeReader.GetNamespace(attributeType) == SettingsAttribute.Namespace 
                && (attributeTypeName == SettingsAttribute.Name || attributeTypeName == GlobalSettingsAttribute.Name) )
                return attribute;
        }
        return null;
    }

    public static EquatableArrayWrap<SettingOverride> From(AttributeData? settingsAttribute)
    {
        if (settingsAttribute is null)
            return new([]);

        return From(settingsAttribute.NamedArguments);
    }

    public static EquatableArrayWrap<SettingOverride> From(ImmutableArray<KeyValuePair<string, TypedConstant>> keyValuePairList)
        => new([.. keyValuePairList.Select(From).Where(x => x is not null)!]);
        
    public static SettingOverride? From(KeyValuePair<string, TypedConstant> namedConstant)
    {
        var value = From(namedConstant.Value);
        if (value is null)
            return null;

        return new(namedConstant.Key, value);
    }


    public static object? From(TypedConstant constant)
        => constant.Kind switch
        {
            TypedConstantKind.Primitive => NullIfNegative(constant.Value),
            TypedConstantKind.Enum => NullIfNegative((int)constant.Value! - 1),
            _ => null
        };

    public static object? NullIfNegative(object? value)
        => value is int valueInt ? NullIfNegative(valueInt) : value;

    public static int? NullIfNegative(int @int)
        => @int >= 0 ? @int : null;
}
