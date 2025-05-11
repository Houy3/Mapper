using Mapper.Core.Entity;

namespace Mapper.Core.Settings;

public record SettingsStorage(
    TypeMappingStorage TypeMappingStorage,
    MappingRule MappingRule = MappingRule.ByDestination);

public enum MappingRule
{
    /// <summary>
    /// Мапить только по полям приемника. "Лишние" поля источника игнорируются.
    /// </summary>
    ByDestination,

    /// <summary>
    /// Мапить только по полям источника. "Лишние" поля приемника игнорируются.
    /// </summary>
    BySource,

    /// <summary>
    /// Мапить только найденные пары полей. "Лишние" поля источника и приемника игнорируются.
    /// </summary>
    OnlyPairs
}