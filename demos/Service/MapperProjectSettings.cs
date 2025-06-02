using DumbFramework;
using Mapper;
using static Mapper.MappingRuleEnum;

namespace Service;

[ProjectSettings(MappingRule = MapByDestination)]
[ImportTypeMappings(Import = [typeof(JObjectMapper)])]
internal class MapperProjectSettings;
