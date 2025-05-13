using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using static Mapper.Attributes.SettingsAttribute;
using static Mapper.Attributes.MethodSettingsAttribute;

namespace Mapper.Core.Settings;

public static class SettingsHelper
{
    public static SettingsStorage ReadProjectSettings(this ImmutableArray<AttributeData> attributeList)
        => From(SettingsStorage.Default(), SettingOverrideReader.From(attributeList));

    public static SettingsStorage From(SettingsStorage settings, EquatableArrayWrap<NamedValue> settingOverrideList)
    {
        if (settingOverrideList.Length == 0)
            return settings;

        return new(
            settings.TypeMappingStorage,
            FindAndParseMappingRule(settingOverrideList) ?? settings.MappingRule,
            FindAndParseIgnoreFieldList(settingOverrideList) ?? []
            );
    }

    public static MappingRule? FindAndParseMappingRule(EquatableArrayWrap<NamedValue> settingOverrideList)
        => (MappingRule?)(settingOverrideList.FirstOrDefault(x => x.Name == MappingRulePropertyName)?.Value as int?);
    public static string[] FindAndParseIgnoreFieldList(EquatableArrayWrap<NamedValue> settingOverrideList)
        => [.. (settingOverrideList.FirstOrDefault(x => x.Name == IgnoreFieldListPropertyName)?.Value as object[] ?? []).Select(x => x.ToString())];

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
                x.StaticMethod,
                x.BuilderMethod,
                x.AfterMappingMethod,
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
        var projectSettings = settingsStorageList.Where(x => x is not null).FirstOrDefault() ?? SettingsStorage.Default();

        return new(typeMappingStorage, projectSettings.MappingRule, projectSettings.IgnoreFieldList);
    }

}

