using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using static Mapper.Attributes.SettingsAttribute;

namespace Mapper.Core.Settings;

public static class SettingsHelper
{
    public static SettingsStorage ReadProjectSettings(this ImmutableArray<AttributeData> attributeList)
        => From(new(new()), SettingOverrideReader.From(attributeList));

    public static SettingsStorage From(SettingsStorage settings, EquatableArrayWrap<NamedValue> settingOverrideList)
    {
        if (settingOverrideList.Length == 0)
            return settings;

        return new(
            settings.TypeMappingStorage,
            Parse(settingOverrideList.FirstOrDefault(x => x.Name == MappingRulePropertyName)?.Value) ?? settings.MappingRule
            );
    }

    public static MappingRule? Parse(object? value)
        => (MappingRule?)(value as int?);

    public static ConfiguredMapperType Configure(
        this PlannedMapperType mapperType, 
        SettingsStorage projectSettings, 
        CancellationToken cancellationToken)
    {
        var interfaceSettings = From(projectSettings, mapperType.SettingOverrideList);

        cancellationToken.ThrowIfCancellationRequested();

        var methodList = mapperType.MappingMethodList.Select(x =>
            new ConfiguredMethod(
                x.Signature,
                x.Details,
                x.ConnectedMethod,
                From(interfaceSettings, x.SettingOverrideList)
                )).ToArray();

        return new ConfiguredMapperType(
            mapperType.Namespace,
            mapperType.Name,
            new(methodList)
            );
    }

    //todo > 1
    public static SettingsStorage FirstOrDefault(
        this ImmutableArray<SettingsStorage> settingsStorageList,
        TypeMappingStorage typeMappingStorage)
    { 
        var projectSettings = settingsStorageList.Where(x => x is not null).FirstOrDefault() ?? new(new());

        return new(typeMappingStorage, projectSettings.MappingRule);
    }

}

