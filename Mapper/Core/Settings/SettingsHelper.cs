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

    public static SettingsStorage From(SettingsStorage settings, EquatableArrayWrap<NamedValue> settingOverrideList, TypeMappingStorage? typeMappingStorage = null)
    {
        typeMappingStorage ??= new();

        if (settingOverrideList.Length == 0 && typeMappingStorage.IsEmpty())
            return settings;

        return new(
            typeMappingStorage,
            Parse(settingOverrideList.FirstOrDefault(x => x.Name == MappingRulePropertyName)?.Value) ?? settings.MappingRule
            );
    }

    public static MappingRuleEnum? Parse(object? value)
        => (MappingRuleEnum?)(value as int?);

    public static ConfiguredMapperType SpreadOutSettings(
        PlannedMapperType mapperType, 
        SettingsStorage globalSettings, 
        TypeMappingStorage typeMappingStorage, 
        CancellationToken cancellationToken)
    {
        var interfaceSettings = From(globalSettings, mapperType);

        cancellationToken.ThrowIfCancellationRequested();

        var methodList = mapperType.MappingMethodList.Select(x =>
            new ConfiguredMethod(
                x.Signature,
                x.Details,
                x.ConnectedMethod,
                From(interfaceSettings, x, typeMappingStorage)
                )).ToArray();

        return new ConfiguredMapperType(
            mapperType.Namespace,
            mapperType.Name,
            new(methodList)
            );
    }
    public static SettingsStorage From(SettingsStorage storage, PlannedMapperType mapperType)
        => From(storage, mapperType.SettingOverrideList);

    public static SettingsStorage From(SettingsStorage storage, MappingMethod method, TypeMappingStorage typeMappingStorage)
    {
        var neededMappings = method.Se

        return From(storage, method.SettingOverrideList);
    }


    //todo > 1
    public static SettingsStorage FirstOrDefault(this ImmutableArray<SettingsStorage> settingsStorageList)
        => settingsStorageList.Where(x => x is not null).FirstOrDefault() ?? new(new());

}

