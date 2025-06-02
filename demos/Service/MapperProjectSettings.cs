using DumbFramework;
using Mapper;
using static Mapper.MappingRuleEnum;

namespace Service;

[ProjectSettings(MappingRule = MapByDestination, NullableStructAutoResolving = true)]
[ImportTypeMappings(Import = [typeof(JObjectMapper)])]
internal class MapperProjectSettings;
