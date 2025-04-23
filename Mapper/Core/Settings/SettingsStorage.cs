namespace Mapper.Core.Settings;

public record SettingsStorage(MappingRuleEnum MappingRule = SettingsStorage.DefaultMappingRule)
{
    public const MappingRuleEnum DefaultMappingRule = MappingRuleEnum.MapByDestination;
}

public enum MappingRuleEnum
{
    /// <summary>
    /// Мапить только по полям приемника. "Лишние" поля источника игнорируются.
    /// </summary>
    MapByDestination,

    /// <summary>
    /// Мапить только по полям источника. "Лишние" поля приемника игнорируются.
    /// </summary>
    MapBySource,

    /// <summary>
    /// Мапить только найденные пары полей. "Лишние" поля источника и приемника игнорируются.
    /// </summary>
    MapOnlyPairs
}