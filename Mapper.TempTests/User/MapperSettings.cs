using DumbFramework;

namespace Service.User;

//todo link Possible settings
/// <summary>
/// <see href="http://stackoverflow.com">Possible settings</see>
/// </summary>
[Mapper.GlobalSettings(MappingRule = Mapper.MappingRuleEnum.MapBySource)]
[Mapper.ImportTypeMappings(Import = [typeof(IJObjectMapper)])]
internal class MapperSettings
{
}
