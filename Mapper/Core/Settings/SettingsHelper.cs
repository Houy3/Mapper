using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Mapper.Core.Settings;

public static class SettingsHelper
{
    public static SettingsStorage From(ISymbol symbol)
        => From(new(), SettingOverrideReader.From(symbol).Array);

    public static SettingsStorage From(SettingsStorage storage, Interface @interface)
        => From(storage, @interface.SettingOverrideList.Array);

    public static SettingsStorage From(SettingsStorage storage, Method method)
        => From(storage, method.SettingOverrideList.Array);

    public static SettingsStorage From(SettingsStorage storage, SettingOverride[] settingOverrideList)
    {
        if (settingOverrideList.Length == 0)
            return storage;

        return new(
            Parse(settingOverrideList.FirstOrDefault(x => x.Name == nameof(SettingsStorage.MappingRule))?.Value) ?? storage.MappingRule
            );
    }

    public static MappingRuleEnum? Parse(object? value)
    {
        if (value is int valueInt)
            if (Enum.IsDefined(typeof(MappingRuleEnum), valueInt))
                return (MappingRuleEnum)valueInt;

        if (Enum.TryParse<MappingRuleEnum>(value?.ToString() ?? string.Empty, out var @enum))
            return @enum;

        return null;
    }

    public static InterfaceWithSettings SpreadOutSettings(Interface @interface, SettingsStorage globalSettings)
    {
        var interfaceSettings = From(globalSettings, @interface);

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
    public static SettingsStorage FirstOrDefaultSetting(ImmutableArray<SettingsStorage> settingsStorageList, CancellationToken cancellationToken)
        => settingsStorageList.Where(x => x is not null).FirstOrDefault() ?? new();

}

