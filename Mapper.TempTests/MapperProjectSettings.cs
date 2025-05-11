using DumbFramework;

namespace Mapper.TempTests;

/// <summary>
/// <see href="http://stackoverflow.com">Possible settings</see>
/// </summary>
[ProjectSettings(MappingRule = MappingRuleEnum.MapByDestination)]
[ImportTypeMappings(Import = [typeof(JObjectMapper)])]
internal class MapperProjectSettings
{
}
