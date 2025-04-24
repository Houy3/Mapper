using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Mapper.Core.Settings;

public static class SettingsHelper
{
    public static SettingsStorage From(ImmutableArray<AttributeData> attributeList)
        => From(new(), SettingOverrideReader.From(attributeList).Array);

    public static SettingsStorage From(SettingsStorage storage, Interface @interface)
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

    public static InterfaceWithSettings SpreadOutSettings(Interface @interface, SettingsStorage globalSettings, CancellationToken cancellationToken)
    {
        var interfaceSettings = From(globalSettings, @interface);

        cancellationToken.ThrowIfCancellationRequested();

        var methodList = @interface.MethodList.Array.Select(x =>
            new MethodWithSettings(
                x.Name,
                x.ReturnType,
                x.ParameterList,
                From(interfaceSettings, x)
                )).ToArray();

        return new InterfaceWithSettings(
            @interface.Namespace,
            @interface.Name,
            new(methodList),
            interfaceSettings
            );
    }

    //todo > 1
    public static SettingsStorage FirstOrDefaultSetting(ImmutableArray<SettingsStorage> settingsStorageList)
        => settingsStorageList.Where(x => x is not null).FirstOrDefault() ?? new();

}

