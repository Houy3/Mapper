using Mapper.Attributes;
using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Mapper.Core.Reader;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Mapper.Core.Settings;

public static class SettingOverrideReader
{
    public static EquatableArrayWrap<NamedValue> From(ISymbol symbol)
        => new(NamedValueReader.From(FindSettingOverrideAttribute(symbol.GetAttributes())));

    public static EquatableArrayWrap<NamedValue> From(ImmutableArray<AttributeData> attributeList)
        => new(NamedValueReader.From(attributeList.FirstOrDefault()));

    public static AttributeData? FindSettingOverrideAttribute(ImmutableArray<AttributeData> attributeList)
    {
        foreach (var attribute in attributeList)
        {
            var attributeType = attribute.AttributeClass;
            if (attributeType is null)
                continue;

            var attributeTypeName = TypeIdReader.GetName(attributeType);
            if (TypeIdReader.GetNamespace(attributeType) == SettingsAttribute.Namespace 
                && (attributeTypeName == SettingsAttribute.Name || attributeTypeName == ProjectSettingsAttribute.Name) )
                return attribute;
        }
        return null;
    }

}
