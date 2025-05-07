using Mapper.Core.Entity;
using Mapper.Core.TypeMapping;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Mapper.Core.Settings;

public static class SettingsHelper
{
    public static SettingsStorage From(ImmutableArray<AttributeData> attributeList)
        => From(new(), SettingOverrideReader.From(attributeList).Array);

    public static SettingsStorage From(SettingsStorage storage, MapperType @interface)
        => From(storage, @interface.SettingOverrideList.Array);

    public static SettingsStorage From(SettingsStorage storage, Method method)
        => From(storage, method.SettingOverrideList.Array);

    public static SettingsStorage From(SettingsStorage storage, NamedValue[] settingOverrideList)
    {
        if (settingOverrideList.Length == 0)
            return storage;

        return new(
            Parse(settingOverrideList.FirstOrDefault(x => x.Name == nameof(SettingsStorage.MappingRule))?.Value) ?? storage.MappingRule
            );
    }

    public static MappingRuleEnum? Parse(object? value)
        => (MappingRuleEnum?)(value as int?);

    public static ConfiguredMapperType SpreadOutSettings(
        MapperType @interface, 
        SettingsStorage globalSettings, 
        TypeMappingStorage typeMappingStorage, 
        CancellationToken cancellationToken)
    {
        var interfaceSettings = From(globalSettings, @interface);

        cancellationToken.ThrowIfCancellationRequested();

        var methodList = @interface.MethodList.Array.Select(x =>
            new ConfiguredMethod(
                x.Name,
                x.ReturnType,
                x.ParameterList,
                x.Details,
                x.Source,
                From(interfaceSettings, x),
                typeMappingStorage//todo
                )).ToArray();

        return new ConfiguredMapperType(
            @interface.Namespace,
            @interface.Name,
            new(methodList)
            );
    }

    //todo > 1
    public static SettingsStorage FirstOrDefaultSetting(ImmutableArray<SettingsStorage> settingsStorageList)
        => settingsStorageList.Where(x => x is not null).FirstOrDefault() ?? new();

}

